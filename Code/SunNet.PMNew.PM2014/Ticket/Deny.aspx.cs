using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Entity.UserModel;
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
using SunNet.PMNew.Entity.ProjectModel;

namespace SunNet.PMNew.PM2014.Ticket
{
    public partial class Deny : BasePage
    {
        TicketsApplication ticketAPP = new TicketsApplication();
        TicketStatusManagerApplication ticketStatusMgr = new TicketStatusManagerApplication();
        FeedBackApplication fbAPP = new FeedBackApplication();
        TicketsApplication ticketsApp = new TicketsApplication();
        ProjectApplication projectApp = new ProjectApplication();
        TicketsEntity _ticketEntity;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ((Pop)this.Master).Width = 460;
                int ticketID = QS("tid", 0);
                _ticketEntity = ticketAPP.GetTickets(ticketID);
                litHead.Text = "Ticket ID: " + _ticketEntity.TicketID + ", " + _ticketEntity.Title;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int ticketID = QS("tid", 0);
            if (ticketID != 0)
            {
                bool isSuccess = UpdateTicketStatusToDeny(ticketID);
                if (isSuccess)
                {
                    if (txtDescription.Text.Trim() != "" || (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0))
                    {
                        FeedBacksEntity feedbacksEntity = new FeedBacksEntity();
                        feedbacksEntity.IsDelete = false;
                        feedbacksEntity.TicketID = ticketID;
                        feedbacksEntity.Title = "";
                        feedbacksEntity.Description = txtDescription.Text.Trim().NoHTML();
                        feedbacksEntity.CreatedBy = UserInfo.UserID;
                        feedbacksEntity.CreatedOn = DateTime.Now;
                        feedbacksEntity.ModifiedOn = DateTime.Now;
                        feedbacksEntity.IsPublic = true;
                        feedbacksEntity.WaitClientFeedback = FeedbackReplyStatus.Normal;
                        feedbacksEntity.WaitPMFeedback = FeedbackReplyStatus.Normal;

                        feedbacksEntity.ID = fbAPP.AddFeedBacks(feedbacksEntity);

                        if (feedbacksEntity.ID <= 0)
                        {
                            ShowFailMessageToClient();
                            return;
                        }
                        ticketStatusMgr.SendEmailtoPMForFeedBack(feedbacksEntity);//状态更新不进行跟新气泡，by dave 2017/09/22

                        //if (ticketAPP.CreateNotification(ticketID,UserInfo.UserID))
                        //{
                        //    //发邮件给PM
                        //    ticketStatusMgr.SendEmailtoPMForFeedBack(feedbacksEntity);
                        //}

                        HttpFileCollection files = Request.Files;
                        string fileuploadErrMsg = string.Empty;
                        TicketsEntity ticketsEntity = new TicketsApplication().GetTickets(feedbacksEntity.TicketID);
                        for (int i = 0; i < files.Count; i++)
                        {
                            if (files[i].ContentLength > 0 && IsValidFile(files[i].FileName))
                            {
                                if (!InsertFile(files[i], ticketsEntity.ProjectID, feedbacksEntity.TicketID, feedbacksEntity.IsPublic, feedbacksEntity.ID))
                                {
                                    fileuploadErrMsg += files[i].FileName + " Upload failed.";
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(fileuploadErrMsg))
                        {
                            ShowFailMessageToClient(fileuploadErrMsg);
                            return;
                        }
                    }

                    string returnurl = Request.QueryString["returnurl"];
                    if (string.IsNullOrEmpty(returnurl))
                    {
                        Redirect(Request.RawUrl, false, true);
                    }
                    else
                    {
                        ParentToUrl(returnurl);
                    }
                }
                else
                {
                    ShowFailMessageToClient("Update ticket’s status fail.");
                }
            }
            else
            {
                ShowFailMessageToClient("Update ticket’s status fail.");
            }
        }

        private bool InsertFile(HttpPostedFile file, int projectID, int ticketID, bool isPublicFeedback, int feedbacksID)
        {
            string tempPath = System.Configuration.ConfigurationManager.AppSettings["FolderPath"];
            string folderName = string.Empty;
            folderName = projectID.ToString();
            string fileName = file.FileName;
            string savePath = Server.MapPath(tempPath) + folderName;
            string sExtension = Path.GetExtension(fileName);
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            string sNewFileName = string.Format("{0}_{1}{2}", feedbacksID, DateTime.Now.ToString("yyMMddssmm"), sExtension); ;
            file.SaveAs(savePath + @"\" + sNewFileName);

            FilesEntity fileEntity = new FilesEntity();
            fileEntity.ContentType = sExtension.ToLower();
            fileEntity.CreatedBy = UserInfo.UserID;
            fileEntity.FilePath = tempPath.Substring(2) + folderName + @"/" + sNewFileName;
            fileEntity.FileSize = Convert.ToDecimal(file.ContentLength);
            fileEntity.FileTitle = fileName.Substring(0, fileName.LastIndexOf('.'));
            fileEntity.IsPublic = isPublicFeedback;
            fileEntity.FeedbackId = feedbacksID;
            fileEntity.TicketId = ticketID;
            fileEntity.ProjectId = projectID;
            fileEntity.SourceType = (int)FileSourceType.FeedBack;
            fileEntity.ThumbPath = "";
            fileEntity.CreatedOn = DateTime.Now.Date;
            fileEntity.CompanyID = ticketsApp.GetCompanyIdByTicketId(ticketID);
            return new FileApplication().AddFile(fileEntity) > 0;
        }

        public bool UpdateTicketStatusToDeny(int ticketId)
        {
            try
            {
                if (UserInfo.UserID <= 0)
                    return false;
                TicketsEntity ticketEntity = new TicketsEntity();

                ticketEntity = ticketAPP.GetTickets(ticketId);

                TicketsState originalStatus = ticketEntity.Status;
                bool Update = true;
                ticketEntity.Status = TicketsState.Not_Approved;

                ticketEntity.ModifiedOn = DateTime.Now;
                ticketEntity.ModifiedBy = UserInfo.UserID;
                ticketEntity.PublishDate = DateTime.Now.Date;
                ProjectsEntity projectEntity = projectApp.Get(ticketEntity.ProjectID);

                Update = ticketAPP.UpdateTickets(ticketEntity, true, projectEntity.PMID);
                ticketStatusMgr.SendEmailWithClientNotApp(ticketEntity);

                return Update;

            }
            catch (Exception ex)
            {
                WebLogAgent.Write(string.Format("Error Ashx:DoUpdateTicketStatus.ashx Messages:\r\n{0}", ex));
                return false;
            }

        }
    }
}