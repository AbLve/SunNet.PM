using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework;
using System.Text;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.PM2014.Ticket
{
    public partial class New : BasePage
    {
        protected string jsonProjectInfo = string.Empty;
        ProjectApplication projectApplication = new ProjectApplication();
        TicketsApplication ticketAPP = new TicketsApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<ProjectDetailDTO> list = projectApplication.GetUserProjects(UserInfo);
                List<TicketsEntity> listTicket = ticketAPP.GetTicketsByCreateId(UserInfo.UserID);
                var projectDetailDto = list.FirstOrDefault(c =>
                {
                    var firstOrDefault = listTicket.FirstOrDefault();
                    return firstOrDefault != null && c.Title == firstOrDefault.ProjectTitle;
                });
                jsonProjectInfo = projectApplication.GetProjectInfoJson(list);
                if (QS("tid", 0) > 0)
                {
                    TicketsEntity entity = new TicketsApplication().GetTickets(QS("tid", 0));
                    if (entity == null && entity.CreatedBy != UserInfo.UserID)
                    {
                        ShowFailMessageToClient("unauthorized access.");
                        return;
                    }

                    this.ddlProject.DataTextField = "Title";
                    this.ddlProject.DataValueField = "ProjectID";
                    if (list != null && list.Count > 0)
                    {
                        list.RemoveAll(r => r.Status == ProjectStatus.Cancelled || r.Status == ProjectStatus.Completed);
                    }
                    ddlProject.DataSource = list;
                    ddlProject.DataBind();
                    jsonProjectInfo = projectApplication.GetProjectInfoJson(list);
                    if (list.Count != 1)
                        ddlProject.Items.Insert(0, new ListItem() { Value = "", Text = "Please select..." });
                    BindTicket(entity);
                    this.btnEditSave.Visible = true;
                    this.btnEditSubmit.Visible = true;
                    this.btnSaveAndNew.Visible = false;
                    this.btnSaveAsDraft.Visible = false;
                    this.btnClear.Visible = false;
                    btnSubmit.Visible = false;

                }
                else
                {
                    if (projectDetailDto != null)
                    {
                        list.Remove(projectDetailDto);
                        list.Insert(0, projectDetailDto);
                        list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ProjectID");
                        ddlProject.Items.FindByValue(projectDetailDto.ProjectID.ToString()).Selected = true;
                    }
                    else
                    {
                        list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ProjectID", DefaulSelectText, "",
                                 QS("project"));
                    }

                    if (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN)
                    {
                        dvSource.Style.Remove("visibility");
                        lblSource.Style.Remove("visibility");
                        ddlSource.DataSource = ConvertEnumtToListItem(typeof(RolesEnum));
                        ddlSource.DataBind();
                        ddlSource.SelectedValue = RolesEnum.CLIENT.ToString();
                    }
                    this.btnEditSave.Visible = false;
                    this.btnEditSubmit.Visible = false;
                }

                if (list.Count == 1 && !projectApplication.CheckIfEstimated(list[0]))  //只有一个Project，且已过期
                {
                    this.btnSaveAsDraft.Enabled = false;
                    this.btnSubmit.Enabled = false;
                    this.btnSaveAndNew.Enabled = false;
                }
            }
        }


        private void BindTicket(TicketsEntity entity)
        {
            this.rdoPriority.SelectedValue = ((int)entity.Priority).ToString();
            this.rdoType.SelectedValue = ((int)entity.TicketType).ToString();
            ddlProject.SelectedValue = entity.ProjectID.ToString();
            chkEN.Checked = entity.IsEstimates;
            txtTitle.Text = Server.HtmlDecode(entity.Title);
            txtUrl.Text = Server.HtmlDecode(entity.URL);
            txtDesc.Value = Server.HtmlDecode(entity.FullDescription);
            litFiles.Text = GetFilesHTML(entity.TicketID);
        }

        private const string editTemplateHTML = "<li  class=\"file\"><a href='{0}/do/DoDownloadFileHandler.ashx?FileID={1}&size={2}' target=\"_blank\">"
        + "{3}{4}</a><a onclick='deleteImgWhenStatusDraft({1})'><img alt='Delete' title='Delete' src=\"/images/icons/delete.png\" style=\"margin-left: 7px; vertical-align: middle; cursor:pointer;\"  width='12' height='12' border=\"0\" /></a></li>";

        protected string GetFilesHTML(int ticketId)
        {
            List<FilesEntity> list = new List<FilesEntity>();
            string Host = System.Configuration.ConfigurationManager.AppSettings["DomainHost"];
            StringBuilder sb = new StringBuilder();
            list = new FileApplication().GetFileListBySourceId(ticketId, FileSourceType.Ticket);
            string
                  templateHTML = string.Empty;
            if (null != list && list.Count > 0)
            {
                templateHTML = editTemplateHTML;

                foreach (FilesEntity filesEntity in list)
                {
                    sb.AppendFormat(templateHTML, Host, filesEntity.FileID, filesEntity.FileSize, filesEntity.FileTitle, filesEntity.ContentType);
                }
                return string.Format("<ul class=\"fileList\">{0}</ul>", sb);
            }
            return templateHTML;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int saveType = 1;
            if (int.TryParse(hidSaveType.Value, out saveType) == false)
                saveType = 1;

            switch (saveType)
            {
                case 1://Submit
                    SaveTicket(1);
                    break;
                case 2: //Save as Draft
                    SaveTicket(2);
                    break;
                case 3: //Draft 状态下的 Save
                    SaveTicket(false);
                    break;
                case 4: //Draft 状态下的  Submit
                    SaveTicket(true);
                    break;
                case 5: //Submit &amp; New
                    SaveTicket(3);
                    ClearFields();
                    break;
            }
        }



        /// <summary>
        /// save : 1:save ; 2:draft ;3 save and new
        /// </summary>
        /// <param name="save"></param>
        private void SaveTicket(int save)
        {
            ProjectsEntity projectsEntity = new ProjectApplication().Get(int.Parse(ddlProject.SelectedValue));
            //判断当前日期是否大于Project的End Date
            if (!projectApplication.CheckIfEstimated(projectsEntity))
            {
                ShowMessageToClient("This project is closed, if you need to submit new tickets for this project, please contact us at team@sunnet.us.",
                    2, false, false);
                return;
            }

            TicketsEntity ticketsEntity = GetEntity(save, projectsEntity.ProjectID, projectsEntity.CompanyID);

            int result = new TicketsApplication().AddTickets(ticketsEntity);
            if (result > 0)
            {
                AddTicketUsers(projectsEntity, ticketsEntity, result);
                AddIniHistroy(ticketsEntity);

                if (!ticketsEntity.IsInternal)
                {
                    SendHandler handler = new SendHandler(SendEamil);
                    handler.BeginInvoke(ticketsEntity, UserInfo, null, null);
                }

                string fileName = hidUploadFile.Value;

                if (fileName.Trim() != string.Empty)
                {
                    string[] files = fileName.Split('|');
                    foreach (string str in files)
                    {
                        InsertFile(str, ticketsEntity.ProjectID, result, ticketsEntity.CompanyID);
                    }
                }

                switch (save)
                {
                    case 1:
                        Redirect("/Ticket/Ongoing.aspx", true);
                        break;
                    case 2:
                        Redirect("/Ticket/Draft.aspx", true);
                        break;
                    case 3:
                        // ShowSuccessMessageToClient("The ticket has been added.", false, false);
                        Redirect("/Ticket/New.aspx?project=" + ddlProject.SelectedValue, true);
                        break;
                }
            }
            else
            {
                ShowFailMessageToClient();
            }
        }

        private void AddTicketUsers(ProjectsEntity projectsEntity, TicketsEntity ticketsEntity, int result)
        {
            TicketUsersEntity ticketUserEntity = new TicketUsersEntity();
            //add pm user
            ticketUserEntity.Type = TicketUsersType.PM;
            ticketUserEntity.TicketID = result;
            ProjectsEntity projectEntity = new ProjectApplication().Get(ticketsEntity.ProjectID);
            if (projectEntity != null)
            {
                ticketUserEntity.UserID = projectEntity.PMID;
                new TicketsApplication().AddTicketUser(ticketUserEntity);
            }
            else
            {
                WebLogAgent.Write(string.Format("Add Pm To Ticket User Error:Project :{0},Ticket:{1},CreateDate:{2}",
                    ticketsEntity.ProjectID, ticketsEntity.TicketID, DateTime.Now));
            }
            //add create user
            ticketUserEntity.Type = TicketUsersType.Create;
            ticketUserEntity.TicketID = result;
            ticketUserEntity.UserID = ticketsEntity.CreatedBy;
            new TicketsApplication().AddTicketUser(ticketUserEntity);

            if (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN)
            {
                //添加当前Project中的Leader到此ticket下.
                List<ProjectUsersEntity> ProjectUsers = new ProjectApplication().GetProjectSunnetUserList(projectsEntity.ID);
                if (ProjectUsers != null)
                {
                    List<ProjectUsersEntity> leaders = ProjectUsers.FindAll(r => r.Role == RolesEnum.Leader);
                    foreach (ProjectUsersEntity leader in leaders)
                    {
                        ticketUserEntity = new TicketUsersEntity();
                        ticketUserEntity.Type = TicketUsersType.Dev;
                        ticketUserEntity.TicketID = result;
                        ticketUserEntity.UserID = leader.UserID;
                        new TicketsApplication().AddTicketUser(ticketUserEntity);
                    }
                }
            }
        }

        private TicketsEntity GetEntity(int save, int projectID, int companyID)
        {
            TicketsEntity ticketsEntity = new TicketsEntity();
            ticketsEntity.Title = txtTitle.Text.Trim().NoHTML();
            ticketsEntity.FullDescription = txtDesc.Value.Trim().NoHTML();
            ticketsEntity.URL = txtUrl.Text.Trim();
            ticketsEntity.ProjectID = projectID;
            ticketsEntity.TicketType = (TicketsType)int.Parse(rdoType.SelectedValue);
            ticketsEntity.TicketCode = new TicketsApplication().ConvertTicketTypeToTicketCode(ticketsEntity.TicketType);
            ticketsEntity.Priority = (PriorityState)int.Parse(this.rdoPriority.SelectedValue);
            ticketsEntity.CompanyID = companyID;
            ticketsEntity.IsEstimates = chkEN.Checked;
            ticketsEntity.IsInternal = false;
            ticketsEntity.CreatedBy = UserInfo.UserID;
            ticketsEntity.CreatedOn = DateTime.Now;
            ticketsEntity.ModifiedBy = UserInfo.UserID;
            ticketsEntity.ModifiedOn = DateTime.Now;
            ticketsEntity.PublishDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
            ticketsEntity.ConvertDelete = CovertDeleteState.Normal;
            ticketsEntity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
            ticketsEntity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
            ticketsEntity.ResponsibleUser=new ProjectApplication().Get(projectID).PMID; ;
            if (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN)
            {
                ticketsEntity.Source = (RolesEnum)Enum.Parse(typeof(RolesEnum), ddlSource.SelectedValue);
            }
            else
            {
                ticketsEntity.Source = UserInfo.Role;
            }
            if (save == 2)
                ticketsEntity.Status = TicketsState.Draft;
            else
            {
                if (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN)
                {
                    ticketsEntity.Status = TicketsState.PM_Reviewed;
                }
                else
                {
                    ticketsEntity.Status = TicketsState.Submitted;
                }
            }
            return ticketsEntity;
        }


        private void ClearFields()
        {
            rdoType.SelectedIndex = 0;
            rdoPriority.SelectedIndex = 1;
            txtDesc.InnerText = string.Empty;
            txtTitle.Text = string.Empty;
            txtUrl.Text = string.Empty;
            chkEN.Checked = false;
        }

        private void SaveTicket(bool save)
        {
            #region add ticket
            TicketsEntity ticketsEntity = new TicketsApplication().GetTickets(QS("tid", 0));
            ticketsEntity.Title = txtTitle.Text.NoHTML();
            ticketsEntity.FullDescription = txtDesc.Value.NoHTML();
            ticketsEntity.URL = txtUrl.Text;
            ticketsEntity.ProjectID = int.Parse(ddlProject.SelectedValue);
            ticketsEntity.TicketType = (TicketsType)int.Parse(rdoType.SelectedValue);
            ticketsEntity.Priority = (PriorityState)int.Parse(this.rdoPriority.SelectedValue);
            ProjectsEntity projectsEntity = new ProjectApplication().Get(ticketsEntity.ProjectID);
            ticketsEntity.CompanyID = projectsEntity.CompanyID;
            ticketsEntity.IsEstimates = chkEN.Checked;

            ticketsEntity.TicketCode = new TicketsApplication().ConvertTicketTypeToTicketCode(ticketsEntity.TicketType);
            ticketsEntity.IsInternal = false;
            ticketsEntity.CreatedBy = UserInfo.UserID;
            ticketsEntity.CreatedOn = DateTime.Now;
            ticketsEntity.ModifiedBy = UserInfo.UserID;
            ticketsEntity.ModifiedOn = DateTime.Now;

            ticketsEntity.PublishDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
            ticketsEntity.ConvertDelete = CovertDeleteState.Normal;
            ticketsEntity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
            ticketsEntity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
            if (save)
                ticketsEntity.Status = TicketsState.Submitted;
            else
                ticketsEntity.Status = TicketsState.Draft;

            if (new TicketsApplication().UpdateTickets(ticketsEntity))
            {
                if (save)
                    new TicketStatusManagerApplication().SendEmailToPMWhenTicketAdd(ticketsEntity, UserInfo);
            }
            #endregion

            #region add file


            string fileName = hidUploadFile.Value;

            if (fileName.Trim() != string.Empty)
            {
                string[] files = fileName.Split('|');
                foreach (string str in files)
                {
                    InsertFile(str, ticketsEntity.ProjectID, ticketsEntity.TicketID, ticketsEntity.CompanyID);
                }
            }

            #endregion
            ShowMessageToClient("The ticket has been updated.", 0, true, true);
            if (save)
            {
                Redirect("/Ticket/Ongoing.aspx", true);
            }
            else
            {
                Redirect("/Ticket/Draft.aspx", true);
            }
        }

        private bool AddIniHistroy(TicketsEntity ticketsEntity)
        {
            TicketHistorysEntity historysEntity = new TicketHistorysEntity();
            historysEntity.Description = "Ticket Submitted";
            historysEntity.ModifiedBy = IdentityContext.UserID;
            historysEntity.CreatedOn=DateTime.Now;
            historysEntity.ModifiedOn=DateTime.Now;
            historysEntity.TicketID = ticketsEntity.ID;
            historysEntity.TDHID = 0;
            historysEntity.ResponsibleUserId = ticketsEntity.ResponsibleUser;

            if (ticketAPP.AddTicketHistory(historysEntity)>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void InsertFile(string file, int projectID, int ticketID, int companyID)
        {
            string realFileName = file.Replace(string.Format("{0}_", UserInfo.UserID), "");
            var fileLength = (int)SunNet.PMNew.PM2014.Codes.FileHelper.GetFileLength(file);
            string tempPath = Config.UploadPath;
            string folderName = projectID.ToString();
            string fileName = file;
            string savePath = Path.Combine(Server.MapPath(tempPath), folderName);
            string sExtension = Path.GetExtension(fileName);
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            string sNewFileName = string.Format("{0}_{1}_{2}{3}", ticketID, Path.GetFileNameWithoutExtension(file), DateTime.Now.ToString("ssmmfff"), sExtension);


            FilesEntity fileEntity = new FilesEntity();
            fileEntity.ContentType = sExtension.ToLower();
            fileEntity.CreatedBy = UserInfo.UserID;
            fileEntity.FilePath = string.Format("{0}{1}/{2}", tempPath.Substring(1), folderName, sNewFileName);
            fileEntity.FileSize = Convert.ToDecimal(fileLength);
            fileEntity.FileTitle = realFileName.Substring(0, realFileName.LastIndexOf('.'));
            fileEntity.IsPublic = true;
            fileEntity.FeedbackId = 0;
            fileEntity.TicketId = ticketID;
            fileEntity.ProjectId = projectID;
            fileEntity.SourceType = (int)FileSourceType.Ticket;
            fileEntity.ThumbPath = "";
            fileEntity.CreatedOn = DateTime.Now.Date;
            fileEntity.CompanyID = companyID;

            if (SunNet.PMNew.PM2014.Codes.FileHelper.Move(file, Server.MapPath(fileEntity.FilePath)))
            {
                new FileApplication().AddFile(fileEntity);
            }
        }

        public delegate void SendHandler(TicketsEntity te, UsersEntity createUser);

        protected void SendEamil(TicketsEntity te, UsersEntity createUser)
        {
            TicketStatusManagerApplication ex = new TicketStatusManagerApplication();
            ex.SendEmailToPMWhenTicketAdd(te, UserInfo);
        }
    }
}