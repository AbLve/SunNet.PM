using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Entity.FileModel;
using System.IO;

namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class SystemEnhancement : BaseWebsitePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<ProjectDetailDTO> list = new ProjectApplication().GetUserProjects(UserInfo);
                this.ddlProject.DataTextField = "Title";
                this.ddlProject.DataValueField = "ProjectID";
                this.radioPriority.SelectedIndex = 1;
                ddlProject.DataSource = list;
                ddlProject.DataBind();
                if (list.Count != 1)
                    ddlProject.Items.Insert(0, new ListItem() { Value = "", Text = "Please select..." });
                int pid = QS("pid", 0);
                if (pid != 0)
                {
                    ddlProject.SelectedValue = pid.ToString();
                }
                BindTicketTypes();

            }
        }

        private List<ListItem> GetEnumerateList(Type enu, List<string> exceptValues)
        {
            string[] names = Enum.GetNames(enu);
            Array values = Enum.GetValues(enu);
           
            List<ListItem> results = new List<ListItem>();
            for (int i = 0; i < names.Length; i++)
            {
                if (!exceptValues.Contains(values.GetValue(i).ToString()))
                {
                    results.Add(new ListItem() { Text = names[i].Replace('_', ' '), Value = values.GetValue(i).ToString() });
                }
            }
            return results;
        }

        private void BindTicketTypes()
        {
            this.ddlType.DataSource = GetEnumerateList(typeof(TicketsType), new List<string>() { "1" });
            this.ddlType.DataBind();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveTicket(1);
        }

        protected void btnSaveAsDraft_Click(object sender, EventArgs e)
        {
            SaveTicket(2);
        }

        private int GetPMProjectID()
        {
            return 0;
        }

        /// <summary>
        /// save : 1:save ; 2:draft ;3 save and new
        /// </summary>
        /// <param name="save"></param>
        private void SaveTicket(int save)
        {
            #region add ticket
            TicketsEntity ticketsEntity = new TicketsEntity();
            ticketsEntity.Title = txtTitle.Value;
            ticketsEntity.FullDescription = txtDesc.Value;
            ticketsEntity.URL = txtUrl.Value;
            ticketsEntity.ProjectID = GetPMProjectID();
            if (ddlType.SelectedValue == "0")
            {
                ticketsEntity.TicketType = TicketsType.Bug;
                ticketsEntity.TicketCode = "B";
            }
            else
            {
                ticketsEntity.TicketType = TicketsType.Request;
                ticketsEntity.TicketCode = "R";
            }
            ticketsEntity.Priority = (PriorityState)int.Parse(this.radioPriority.SelectedValue);
            ProjectsEntity projectsEntity = new ProjectApplication().Get(ticketsEntity.ProjectID);
            ticketsEntity.CompanyID = projectsEntity.CompanyID;
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
            ticketsEntity.Source = UserInfo.Role;
            if (save == 2)
                ticketsEntity.Status = TicketsState.Draft;
            else
                ticketsEntity.Status = TicketsState.Submitted;
            int result = new TicketsApplication().AddTickets(ticketsEntity);

            if (result > 0)
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
            }
            #endregion

            #region send email
            TicketStatusManagerApplication ex = new TicketStatusManagerApplication();
            if (!ticketsEntity.IsInternal)
            {
                ex.SendEmailToPMWhenTicketAdd(result, ticketsEntity.TicketType);
            }

            #endregion

            #region add file

            if (fileupload.PostedFile.ContentLength > 0)
            {

                string FolderName = string.Empty;
                if (null != projectsEntity)
                {
                    FolderName = projectsEntity.ProjectID.ToString();
                }


                string filderPath = System.Configuration.ConfigurationManager.AppSettings["FolderPath"]; //～/path
                string savepath = Server.MapPath(filderPath) + FolderName;
                string filename = fileupload.PostedFile.FileName;
                string fileExtension = Path.GetExtension(filename);

                if (!Directory.Exists(savepath))
                    Directory.CreateDirectory(savepath);

                string sNewFileName = string.Format("{0}_{1}{2}", result, DateTime.Now.ToString("yyMMddssmm"), fileExtension);

                fileupload.PostedFile.SaveAs(savepath + @"\" + sNewFileName);



                FilesEntity fileEntity = new FilesEntity();

                fileEntity.ContentType = fileExtension.ToLower();
                fileEntity.CreatedBy = ticketsEntity.CreatedBy;
                fileEntity.FilePath = filderPath.Substring(2) + FolderName + @"/" + sNewFileName;
                fileEntity.FileSize = fileupload.PostedFile.ContentLength;
                fileEntity.FileTitle = filename.Substring(0, filename.LastIndexOf('.'));
                fileEntity.IsPublic = !ticketsEntity.IsInternal;
                fileEntity.ProjectId = ticketsEntity.ProjectID;
                fileEntity.TicketId = result;
                fileEntity.CreatedOn = DateTime.Now.Date;
                fileEntity.FeedbackId = 0;
                fileEntity.SourceType = (int)FileSourceType.Ticket;
                fileEntity.ThumbPath = "";
                fileEntity.CompanyID = ticketsEntity.CompanyID;
                int response = new FileApplication().AddFile(fileEntity);
            }

            #endregion

            switch (save)
            {
                case 1:
                    ShowMessageAndRedirect("The ticket has been added.", "/sunnet/Clients/ListTicket.aspx");
                    break;
                case 2:
                    ShowMessageAndRedirect("The ticket has been added.", "/sunnet/Clients/ListTicketDrafted.aspx");
                    break;
                case 3:
                    ShowMessageAndRedirect("The ticket has been added.", "/sunnet/Clients/AddBug.aspx");
                    break;
            }
        }


        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProject.SelectedValue == "") return;
            int project = int.Parse(ddlProject.SelectedValue);
            if (project > 0)
            {
                ProjectsEntity projectEntity = new ProjectApplication().Get(project);
                chkEN.Checked = projectEntity.BugNeedApproved;
            }
        }

        protected void btnSaveAndNew_Click(object sender, EventArgs e)
        {
            SaveTicket(3);
        }
    }
}
