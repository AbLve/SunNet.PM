using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TicketModel.TicketsMessage;
using SunNet.PMNew.Framework.Utils;
using StructureMap;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.FileModel;
using System.IO;
using System.Text;
namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class EditTicket : BaseWebsitePage
    {
        protected int Prioirty;
        protected string jsonProjectAndEstimate = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (QS("tid", 0) > 0)
                {
                    TicketsEntity entity = new TicketsApplication().GetTickets(QS("tid", 0));
                    if (entity == null && entity.CreatedBy != UserInfo.UserID)
                    {
                        ShowFailMessageToClient("unauthorized access.");
                        return;
                    }
                    ProjectApplication projectApplication = new ProjectApplication();
                    List<ProjectDetailDTO> list = projectApplication.GetUserProjects(UserInfo);
                    this.ddlProject.DataTextField = "Title";
                    this.ddlProject.DataValueField = "ProjectID";

                    ddlProject.DataSource = list;
                    ddlProject.DataBind();
                    jsonProjectAndEstimate = projectApplication.GetProjectAndEstimateRelationJson(list);
                    if (list.Count != 1)
                        ddlProject.Items.Insert(0, new ListItem() { Value = "", Text = "Please select..." });

                    BindTicket(entity);
                }
            }
        }

        private void BindTicket(TicketsEntity entity)
        {
            this.radioPriority.SelectedValue = ((int)entity.Priority).ToString();
            ddlTicketType.SelectedValue = ((int)entity.TicketType).ToString();
            ddlProject.SelectedValue = entity.ProjectID.ToString();
            chkEN.Checked = entity.IsEstimates;
            Prioirty = (int)entity.Priority;
            txtTitle.Value = entity.Title;
            txtUrl.Value = entity.URL;
            txtDesc.Value = entity.FullDescription;
            litFile.Text = fileList(entity.TicketID);
        }

        private string fileList(int ticketId)
        {
            List<FilesEntity> list = new List<FilesEntity>();
            string Host = System.Configuration.ConfigurationManager.AppSettings["DomainHost"];
            StringBuilder sb = new StringBuilder();
            list = new FileApplication().GetFileListBySourceId(ticketId, FileSourceType.Ticket);

            foreach (FilesEntity filesEntity in list)
            {
                sb.AppendFormat("<li  class=\"file\"><a href='{0}/do/DoDownloadFileHandler.ashx?FileID={1}&size={2}' target=\"iframeDownloadFile\");\">", Host, filesEntity.FileID, filesEntity.FileSize)
                    .AppendFormat("{2}{3} (Size:{1})</a><a onclick='deleteImgWhenStatusDraft({0})'><img src=\"/Scripts/Upload/images/error_fuck.png\" width='12' height='12' border=\"0\" /></a></li>"
                   , filesEntity.FileID, filesEntity.FileSize, filesEntity.FileTitle, filesEntity.ContentType);
            }

            return string.Format("<ul id='demo-list'>{0}</ul>", sb);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveTicket(false);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SaveTicket(true);
        }

        private void SaveTicket(bool save)
        {
            #region add ticket
            TicketsEntity ticketsEntity = new TicketsApplication().GetTickets(QS("tid", 0));
            ticketsEntity.Title = txtTitle.Value.NoHTML();
            ticketsEntity.FullDescription = txtDesc.Value.NoHTML();
            ticketsEntity.URL = txtUrl.Value;
            ticketsEntity.ProjectID = int.Parse(ddlProject.SelectedValue);
            ticketsEntity.TicketType = (TicketsType)int.Parse(ddlTicketType.SelectedValue);
            ticketsEntity.Priority = (PriorityState)int.Parse(this.radioPriority.SelectedValue);
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
                    new TicketStatusManagerApplication().SendEmailToPMWhenTicketAdd(ticketsEntity.TicketID, ticketsEntity.TicketType);
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

                string sNewFileName = string.Format("{0}_{1}{2}", ticketsEntity.TicketID, DateTime.Now.ToString("yyMMddssmm"), fileExtension);

                fileupload.PostedFile.SaveAs(savepath + @"\" + sNewFileName);



                FilesEntity fileEntity = new FilesEntity();

                fileEntity.ContentType = fileExtension.ToLower();
                fileEntity.CreatedBy = ticketsEntity.CreatedBy;
                fileEntity.FilePath = filderPath.Substring(2) + FolderName + @"/" + sNewFileName;
                fileEntity.FileSize = fileupload.PostedFile.ContentLength;
                fileEntity.FileTitle = filename.Substring(0, filename.LastIndexOf('.'));
                fileEntity.IsPublic = !ticketsEntity.IsInternal;
                fileEntity.ProjectId = ticketsEntity.ProjectID;
                fileEntity.TicketId = ticketsEntity.TicketID;
                fileEntity.CreatedOn = DateTime.Now.Date;
                fileEntity.FeedbackId = 0;
                fileEntity.SourceType = (int)FileSourceType.Ticket;
                fileEntity.ThumbPath = "";
                fileEntity.CompanyID = ticketsEntity.CompanyID;
                int response = new FileApplication().AddFile(fileEntity);
            }

            #endregion
            ShowMessageToClient("The ticket has been updated.", 0, true, true);
        }


        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProject.SelectedValue == "") return;
            int project = int.Parse(ddlProject.SelectedValue);
            if (project > 0)
            {
                ProjectsEntity projectEntity = new ProjectApplication().Get(project);
                if (ddlTicketType.SelectedValue == "1")
                    chkEN.Checked = projectEntity.BugNeedApproved;
                else
                    chkEN.Checked = projectEntity.BugNeedApproved;
            }
        }
    }
}
