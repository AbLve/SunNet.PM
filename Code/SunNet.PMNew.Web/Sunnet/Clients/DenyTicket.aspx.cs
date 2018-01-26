using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using System.Text.RegularExpressions;
using SunNet.PMNew.Entity.FileModel;
using System.Text;
using System.IO;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class DenyTicket : BaseWebsitePage
    {
        TicketsApplication ticketAPP = new TicketsApplication();
        TicketStatusManagerApplication ticketStatusMgr = new TicketStatusManagerApplication();
        FeedBackApplication fbAPP = new FeedBackApplication();
        TicketsApplication ticketsApp = new TicketsApplication();
        FeedBackMessagesApplication fbmApp = new FeedBackMessagesApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            int tid = QS("tid", 0);
            int feedbackId = QS("feedbackId", 0);
            if (tid == 0 && feedbackId == 0)
            {
                this.ShowArgumentErrorMessageToClient();
                return;
            }

            GetProjectIdAndUserIDResponse projectidanduseridResponse = new GetProjectIdAndUserIDResponse();
            if (!IsPostBack)
            {
                #region role

                hdTicketID.Value = QS("tid");
                if (UserInfo.Role == RolesEnum.CLIENT)
                {
                    if (!string.IsNullOrEmpty(QS("deny")))
                    {
                        Page.Title = "Add Reason";
                        bannerTitle.Text = "Add Reason";
                        txtTitle.Value = "Deny Reason";
                    }
                    trOthers.Visible = false;
                }
                else
                {
                    trOthers.Visible = true;
                    this.ckIsPublic.Visible = true;
                    this.ckIsPublic.Checked = true;
                    this.chkIsWaitClientFeedback.Visible = true;
                }
                projectidanduseridResponse = ticketsApp.GetProjectIdAndUserID(tid);
                hdProjectID.Value = projectidanduseridResponse.ProjectId.ToString();
            }
                #endregion
        }



        private bool CheckSecurity(GetProjectIdAndUserIDResponse info)
        {
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                if (UserInfo.UserID != info.CreateUserId)
                {
                    return false;
                }
            }
            else if (UserInfo.Role != RolesEnum.ADMIN &&
                    UserInfo.Role != RolesEnum.PM &&
                    UserInfo.Role != RolesEnum.Sales)
            {
                return false;
            }
            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool isSuccess = UpdateTicketStatusToDeny(hdTicketID.Value);
            if (isSuccess)
            {
                FeedBacksEntity feedbacksEntity = new FeedBacksEntity();
                feedbacksEntity.IsDelete = false;
                feedbacksEntity.TicketID = int.Parse(hdTicketID.Value);
                feedbacksEntity.Title = txtTitle.Value.NoHTML();
                feedbacksEntity.Description = txtContent.Value.NoHTML();
                feedbacksEntity.CreatedBy = UserInfo.UserID;
                feedbacksEntity.CreatedOn = DateTime.Now;
                feedbacksEntity.ModifiedOn = DateTime.Now;
                feedbacksEntity.IsPublic = UserInfo.Role == RolesEnum.CLIENT ? true : ckIsPublic.Checked;
                feedbacksEntity.WaitClientFeedback = chkIsWaitClientFeedback.Checked ? 1 : 0;
                feedbacksEntity.WaitPMFeedback = 0;
                if (feedbacksEntity.WaitClientFeedback > 0)
                    feedbacksEntity.IsPublic = true;

                feedbacksEntity.ID = fbAPP.AddFeedBacks(feedbacksEntity);

                if (feedbacksEntity.ID <= 0)
                {
                    ShowFailMessageToClient();
                    return;
                }

                if (fbmApp.Add(feedbacksEntity)) //feedbackmessages
                {
                    if (feedbacksEntity.WaitClientFeedback > 0 ||
                          (feedbacksEntity.WaitClientFeedback <= 0 &&
                         feedbacksEntity.IsPublic)) //发邮件通知 PM
                    {
                        //发邮件给PM
                        ticketStatusMgr.SendEmailtoPMForFeedBack(feedbacksEntity);
                    }
                }


                if (fileUpload.PostedFile.ContentLength > 0)
                {
                    string FolderName = string.Empty;
                    FolderName = hdProjectID.Value;
                    string filderPath = System.Configuration.ConfigurationManager.AppSettings["FolderPath"]; //～/path
                    string savepath = Server.MapPath(filderPath) + FolderName;
                    string filename = fileUpload.PostedFile.FileName;
                    string fileExtension = Path.GetExtension(filename);

                    if (!Directory.Exists(savepath))
                        Directory.CreateDirectory(savepath);

                    string sNewFileName = string.Format("{0}_{1}{2}", feedbacksEntity.ID, DateTime.Now.ToString("yyMMddssmm"), fileExtension);

                    fileUpload.PostedFile.SaveAs(savepath + @"\" + sNewFileName);

                    FilesEntity fileEntity = new FilesEntity();

                    fileEntity.ContentType = fileExtension.ToLower();
                    fileEntity.CreatedBy = UserInfo.UserID;
                    fileEntity.FilePath = filderPath.Substring(2) + FolderName + @"/" + sNewFileName;
                    fileEntity.FileSize = fileUpload.PostedFile.ContentLength;
                    fileEntity.FileTitle = filename.Substring(0, filename.LastIndexOf('.'));
                    fileEntity.IsPublic = true;
                    fileEntity.ProjectId = int.Parse(hdProjectID.Value);
                    fileEntity.TicketId = int.Parse(hdTicketID.Value);
                    fileEntity.CreatedOn = DateTime.Now.Date;
                    fileEntity.FeedbackId = feedbacksEntity.ID;
                    fileEntity.SourceType = (int)FileSourceType.FeedBack;
                    fileEntity.ThumbPath = "";
                    fileEntity.CompanyID = ticketsApp.GetCompanyIdByTicketId(int.Parse(hdTicketID.Value));
                    int response = new FileApplication().AddFile(fileEntity);
                }
                ShowMessageToClient("The ticket’s status has been updated.", 0, true, true);
            }
            else
            {
                ShowMessageToClient("Update ticket’s status fail.", 0, true, true);
            }
        }

        public string ShowImageList(int feedbackId)
        {
            if (feedbackId < 1) return "";
            List<FilesEntity> list = new List<FilesEntity>();
            string Host = System.Configuration.ConfigurationManager.AppSettings["DomainHost"];
            StringBuilder sb = new StringBuilder();
            list = new FileApplication().GetFileListBySourceId(feedbackId, FileSourceType.FeedBack);
            foreach (FilesEntity filesEntity in list)
            {
                sb.AppendFormat("<span><a href='{0}/do/DoDownloadFileHandler.ashx?FileID={1}&size={2}' target=\"iframeDownloadFile\");\">{3}</a>"
                    , Host, filesEntity.FileID, filesEntity.FileSize, filesEntity.FileTitle)
                    .Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> ");
            }
            return sb.ToString();
        }

        public bool UpdateTicketStatusToDeny(string ticketId)
        {
            try
            {
                if (UserInfo.UserID <= 0)
                    return false;

                int tid = Convert.ToInt32(ticketId);

                TicketsEntity ticketEntity = new TicketsEntity();

                ticketEntity = ticketAPP.GetTickets(tid);

                TicketsState originalStatus = ticketEntity.Status;
                bool Update = true;
                ticketEntity.Status = TicketsState.Not_Approved;

                ticketEntity.ModifiedOn = DateTime.Now;
                ticketEntity.ModifiedBy = UserInfo.UserID;
                ticketEntity.PublishDate = DateTime.Now.Date;
                Update = ticketAPP.UpdateTickets(ticketEntity);
                ticketStatusMgr.SendEmailWithClientNotApp(ticketEntity);

                return Update;

            }
            catch (Exception ex)
            {
                WebLogAgent.Write(string.Format("Error Ashx:DoUpdateTicketStatus.ashx Messages:\r\n{0}", ex));
                return false;
            }

        }

        public bool HasDevOrQaUnderTicket(TicketsEntity entity)
        {
            if (null == entity || entity.TicketID <= 0) return false;
            List<TicketUsersEntity> listUser = ticketAPP.GetListUsersByTicketId(entity.TicketID);
            if (listUser == null || listUser.Count <= 0) return false;
            if (listUser.FindAll(x => (x.Type == TicketUsersType.Dev || x.Type == TicketUsersType.QA)).Count > 0)
                return true;
            return false;
        }
    }
}
