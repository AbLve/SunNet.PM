using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;
using System.IO;
using SunNet.PMNew.Entity.FileModel;
using System.Text;
using StructureMap;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Entity.ShareModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.PM2014.Do;

namespace SunNet.PMNew.PM2014.Ticket.Sunnet
{
    public partial class New : BasePage
    {
        protected string jsonProjectInfo = string.Empty;
        TicketsApplication ticketAPP = new TicketsApplication();
        TicketStatusManagerApplication ex = new TicketStatusManagerApplication();
        ProposalTrackerApplication wrApp=new ProposalTrackerApplication();
        UserApplication userApp=new UserApplication();
        private readonly ShareApplication _shareApp = new ShareApplication();
        private delegate void SendHandler();

        public string CurRole
        {
            get { return (string)ViewState["curRole"] ?? UserInfo.Role.ToString(); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProjectApplication projectApplication = new ProjectApplication();
                List<ProjectDetailDTO> list = projectApplication.GetUserProjectsForCreateObject(UserInfo);
                List<TicketsEntity> listTicket = ticketAPP.GetTicketsByCreateId(UserInfo.UserID);
                var projectDetailDto = list.FirstOrDefault(c =>
                {
                    var firstOrDefault = listTicket.FirstOrDefault();
                    return firstOrDefault != null && c.Title == firstOrDefault.ProjectTitle;
                });
                if (projectDetailDto != null)
                {
                    List<UsersEntity> users = new List<UsersEntity>();
                    var sunnetUser = projectApplication.GetProjectSunnetUserList(projectDetailDto.ProjectID);
                    users.AddRange(sunnetUser.Select(t => userApp.GetUser(t.UserID)).Where(user => user != null && user.Status.Trim() != "INACTIVE"));
                    UsersEntity myselef = users.FirstOrDefault(c => c.UserID == UserInfo.UserID);
                    if (myselef != null)
                    {
                        users.Remove(myselef);
                        users.Add(new UsersEntity()
                        {
                            FirstName = "System",
                            UserID = -1

                        });
                        users.BindDropdown<UsersEntity>(ddlRes, "FirstAndLastName", "UserID", myselef.FirstAndLastName, myselef.UserID.ToString(), "", true);
                    }
                    else
                    {
                        users.Add(new UsersEntity()
                        {
                            FirstName = "System",
                            UserID = -1

                        });
                        users.BindDropdown<UsersEntity>(ddlRes, "FirstAndLastName", "UserID");

                    }
                    list.Remove(projectDetailDto);
                    list.Insert(0,projectDetailDto);
                    list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ProjectID");
                    ddlProject.Items.FindByValue(projectDetailDto.ProjectID.ToString()).Selected=true;
                }
                else
                {
                    list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ProjectID", DefaulSelectText, "", QS("project"));
                }
             
                jsonProjectInfo = projectApplication.GetProjectInfoJson(list);
                if (UserInfo.Role == RolesEnum.PM)
                {
                    dvRes.Style.Remove("display");
                    lblRes.Style.Remove("display");
                }
                if (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN || UserInfo.Role == RolesEnum.Sales)
                {
                    ViewState["curRole"] = UserInfo.Role.ToString();
                    dvSource.Style.Remove("display");
                    lblSource.Style.Remove("display");
                    lblAccounting.Style.Remove("display");
                    dvAccounting.Style.Remove("display");
                    rdoAccounting.SelectedIndex = 0;
                    ddlSource.DataSource = ConvertEnumtToListItem(typeof(RolesEnum));
                    ddlSource.DataBind();
                    if (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN)
                        ddlSource.SelectedValue = RolesEnum.PM.ToString();
                    else
                        ddlSource.SelectedValue = RolesEnum.Sales.ToString();
                    dvIsInteral.Style.Remove("display");
                    lblIsInternal.Style.Remove("display");
                }
                else
                {
                    rdoAccounting.SelectedIndex = 2;
                }

                if (list.Count == 1 && !projectApplication.CheckIfEstimated(list[0]))  //只有一个Project，且已过期
                {
                    this.btnSubmit.Enabled = false;
                    this.btnSave.Enabled = false;
                    this.btnSubmitAndNew.Enabled = false;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int saveType = 1;

            if (int.TryParse(hidSaveType.Value, out saveType) == false)
                saveType = 1;
            SaveTicket(saveType);
        }

        /// <summary>
        /// save : 1:save ; 2:draft ;3 save and new
        /// </summary>
        /// <param name="save"></param>
        private void SaveTicket(int save)
        {
            ProjectsEntity projectsEntity = new ProjectApplication().Get(int.Parse(ddlProject.SelectedValue));
            TicketsEntity ticketsEntity = GetEntity(save, projectsEntity.ProjectID, projectsEntity.CompanyID);

            int result = new TicketsApplication().AddTickets(ticketsEntity);
            if (result > 0)
            {
                ticketsEntity.TicketID = result;
                List<int> userIds;

                AddDefaultTicketUsers(projectsEntity, ticketsEntity, result, out userIds);
                AssignTicketUsers(ticketsEntity, userIds);
                AddIniHistroy(ticketsEntity);

                if (rdoShareKnowlege.Checked)
                {
                    ShareEntity shareEntity = new ShareEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
                    int type = 0;
                    shareEntity.Title = ticketsEntity.Title;
                    shareEntity.Note = ticketsEntity.FullDescription;
                    shareEntity.Type = type;
                    shareEntity.TicketID = result;
                    shareEntity.TypeEntity.Title= ticketsEntity.Title;
                    _shareApp.Save(shareEntity);
                }

                if (!ticketsEntity.IsInternal)
                {
                    new SendHandler(() => ex.SendEmailToPMWhenTicketAdd(ticketsEntity, UserInfo))
                        .BeginInvoke(null, null);
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
                ProposalTrackerRelationEntity model = new ProposalTrackerRelationEntity();
                if (hid_Proposal.Value != "")
                {
                    model.CreatedBy = IdentityContext.UserID;
                    model.TID = ticketsEntity.TicketID;
                    model.WID = int.Parse(this.hid_Proposal.Value);
                    wrApp.AddProposalTrackerRelation(model);
                }


                switch (save)
                {
                    case 1:
                        Redirect("/SunnetTicket/Index.aspx", true);
                        break;
                    case 3:
                        Redirect("/SunnetTicket/New.aspx?project=" + ddlProject.SelectedValue, true);
                        break;
                }
            }
            else
            {
                ShowFailMessageToClient();
            }
        }

        private TicketsEntity GetEntity(int save, int projectID, int companyID)
        {
            TicketsEntity ticketsEntity = new TicketsEntity();
            ticketsEntity.Title = txtTitle.Text.Trim().NoHTML();
            ticketsEntity.FullDescription = txtDesc.Value.Trim();//.NoHTML();
            ticketsEntity.URL = txtUrl.Text.Trim();
            ticketsEntity.ProjectID = projectID;
            ticketsEntity.TicketType = (TicketsType)int.Parse(rdoType.SelectedValue);
            ticketsEntity.TicketCode = new TicketsApplication().ConvertTicketTypeToTicketCode(ticketsEntity.TicketType);
            ticketsEntity.Priority = (PriorityState)int.Parse(this.rdoPriority.SelectedValue);
            ticketsEntity.Accounting = (AccountingState)int.Parse(this.rdoAccounting.SelectedValue);
            ticketsEntity.CompanyID = companyID;
            ticketsEntity.IsEstimates = chkEN.Checked;
            ticketsEntity.IsInternal = rdoIsInternal.Checked;

            ticketsEntity.CreatedOn = DateTime.Now;
            ticketsEntity.ModifiedOn = DateTime.Now;
            ticketsEntity.PublishDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
            ticketsEntity.ConvertDelete = CovertDeleteState.Normal;
            ticketsEntity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
            ticketsEntity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
            if (UserInfo.Role == RolesEnum.PM)
            {
                ticketsEntity.ResponsibleUser = int.Parse(ddlRes.SelectedValue);
            }
            else
            {
                ticketsEntity.ResponsibleUser = new ProjectApplication().Get(projectID).PMID; ;
            }
            //增加Sales  lyq 20140805
            if (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN || UserInfo.Role == RolesEnum.Sales)
            {
                ticketsEntity.Source = (RolesEnum)Enum.Parse(typeof(RolesEnum), ddlSource.SelectedValue);
            }
            else
            {
                ticketsEntity.Source = UserInfo.Role;
            }

            if (ticketsEntity.IsInternal)
            {
                ticketsEntity.ModifiedBy = UserInfo.UserID;
                ticketsEntity.CreatedBy = UserInfo.UserID;
                if (UserInfo.Role == RolesEnum.Supervisor)
                {
                    ticketsEntity.Status = TicketsState.Submitted;
                }
                else
                {
                    ticketsEntity.Status = TicketsState.PM_Reviewed;
                }
            }
            else
            {
                ticketsEntity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                ticketsEntity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                int clientId = 0;
                if (int.TryParse(QF(ddlClientUsers.Name), out clientId))
                {
                    ticketsEntity.ModifiedBy = clientId;
                    ticketsEntity.CreatedBy = clientId;
                }
                else
                {
                    ticketsEntity.ModifiedBy = UserInfo.UserID;
                    ticketsEntity.CreatedBy = UserInfo.UserID;
                }

            }

            if (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN)
            {
                ticketsEntity.Status = TicketsState.PM_Reviewed;
            }
            else
            {
                ticketsEntity.Status = TicketsState.Submitted;
            }

            return ticketsEntity;
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


        private void AddDefaultTicketUsers(ProjectsEntity projectsEntity, TicketsEntity ticketsEntity, int result, out List<int> userIds)
        {
            userIds = new List<int>();
            TicketUsersEntity ticketPMUserEntity = new TicketUsersEntity();
            //add pm user
            ticketPMUserEntity.Type = TicketUsersType.PM;
            ticketPMUserEntity.TicketID = result;
            ProjectsEntity projectEntity = new ProjectApplication().Get(ticketsEntity.ProjectID);
            if (projectEntity != null)
            {
                ticketPMUserEntity.UserID = projectEntity.PMID;
                new TicketsApplication().AddTicketUser(ticketPMUserEntity);
                userIds.Add(projectEntity.PMID);
                //发送Email给Responsible
                ticketPMUserEntity.UserID = int.Parse(ddlRes.SelectedValue);
                new SendHandler(() => ex.SendEmailToResponsibile(ticketPMUserEntity, ticketsEntity, UserInfo)).BeginInvoke(null, null);
            }
            else
            {
                WebLogAgent.Write(string.Format("Add Pm To Ticket User Error:Project :{0},Ticket:{1},CreateDate:{2}",
                    ticketsEntity.ProjectID, ticketsEntity.TicketID, DateTime.Now));
            }

            //add create user
            TicketUsersEntity ticketCreateUserEntity = new TicketUsersEntity();
            ticketCreateUserEntity.Type = TicketUsersType.Create;
            ticketCreateUserEntity.TicketID = result;
            ticketCreateUserEntity.UserID = ticketsEntity.CreatedBy;
            new TicketsApplication().AddTicketUser(ticketCreateUserEntity);

            if (userIds.Contains(ticketsEntity.CreatedBy) == false)
            {
                new SendHandler(() => ex.SendEmailToAssignedUser(ticketCreateUserEntity, ticketsEntity, UserInfo)).BeginInvoke(null, null);
                userIds.Add(ticketsEntity.CreatedBy);
            }
        }


        private bool AssignTicketUsers(TicketsEntity ticketsEntity, List<int> userIds)
        {
            TicketUsersEntity tuEntity = new TicketUsersEntity();
            string[] userWithRoleList = hdTicketUsers.Value.TrimEnd(',').Split(',');
            int assignResult = 0;
            if (userWithRoleList.Length > 0)
            {
                foreach (string item in userWithRoleList)
                {
                    if (item.Length > 0)
                    {
                        string[] userWithRole = item.Split('|');
                        if (userWithRole.Length > 0)
                        {
                            if (userIds.Contains(Convert.ToInt32(userWithRole[0])) == false)
                            {
                                tuEntity.TicketID = ticketsEntity.TicketID;
                                tuEntity.UserID = Convert.ToInt32(userWithRole[0]);
                                tuEntity.Type = GetUserTypeByRoleID(userWithRole[1]);  //Convert.ToInt32(userWithRole[0]);
                                assignResult = ticketAPP.AddTicketUser(tuEntity);
                                if (assignResult > 0)
                                {
                                    //当创建Ticket时，不发送邮件给Assign Users了
                                    //new SendHandler(() => ex.SendEmailToAssignedUser(tuEntity, ticketsEntity, UserInfo)).BeginInvoke(null, null);
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 下拉框 改变时 的做法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProjectApplication projectApplication = new ProjectApplication();
            List<ProjectDetailDTO> list = projectApplication.GetUserProjectsForCreateObject(UserInfo);
            ProjectDetailDTO projectDetailDto = list.FirstOrDefault(c => c.Title == ddlProject.SelectedItem.Text);
            List<UsersEntity> users = new List<UsersEntity>();
            if (projectDetailDto!=null)
            {
                var sunnetUser = projectApplication.GetProjectSunnetUserList(projectDetailDto.ProjectID);
                users.AddRange(sunnetUser.Select(t => userApp.GetUser(t.UserID)).Where(user => user != null && user.Status.Trim() != "INACTIVE"));
                list.Remove(projectDetailDto);
                list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ProjectID", projectDetailDto.Title, projectDetailDto.ProjectID.ToString(), "", false);
                ddlProject.Attributes.Add("onchange", "fnDisplayUsers();ChangeProject();check();");
                UsersEntity myselef = users.FirstOrDefault(c => c.UserID == UserInfo.UserID);
                if (myselef != null)
                {
                    users.Remove(myselef);
                    users.Add(new UsersEntity()
                    {
                        FirstName = "System",
                        UserID = -1

                    });
                    users.BindDropdown<UsersEntity>(ddlRes, "FirstAndLastName", "UserID", myselef.FirstAndLastName, myselef.UserID.ToString(), "", true);
                }
                else
                {
                    users.Add(new UsersEntity()
                    {
                        FirstName = "System",
                        UserID = -1

                    });
                    users.BindDropdown<UsersEntity>(ddlRes, "FirstAndLastName", "UserID");
                    
                }
            }
        }

        private bool AddIniHistroy(TicketsEntity ticketsEntity)
        {
            TicketHistorysEntity historysEntity = new TicketHistorysEntity();
            historysEntity.Description = "Ticket status is "+ticketsEntity.Status.ToString();
            historysEntity.ModifiedBy = IdentityContext.UserID;
            historysEntity.CreatedOn = DateTime.Now;
            historysEntity.ModifiedOn = DateTime.Now;
            historysEntity.TicketID = ticketsEntity.ID;
            historysEntity.TDHID = 0;
            historysEntity.ResponsibleUserId = ticketsEntity.ResponsibleUser;

            if (ticketAPP.AddTicketHistory(historysEntity) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}