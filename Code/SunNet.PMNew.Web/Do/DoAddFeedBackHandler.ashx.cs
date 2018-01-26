using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.ProjectModel;
using System.IO;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;
using System.Text;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoAddFeedBackHandler : IHttpHandler
    {
        #region declare

        TicketStatusManagerApplication ticketStatusMgr = new TicketStatusManagerApplication();
        FeedBackApplication fbAPP = new FeedBackApplication();
        FileApplication fileApp = new FileApplication();
        UserApplication userApp = new UserApplication();
        ProjectApplication projectApp = new ProjectApplication();
        FeedBackMessagesApplication fbmApp = new FeedBackMessagesApplication();
        TicketsApplication ticketApp = new TicketsApplication();
        string tempPath = "";
        string FolderName = "";
        bool HasFileMsG = true;
        List<string> stringErrorMsg = new List<string>();
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (IdentityContext.UserID <= 0)
                return;
            try
            {
                #region get value

                int tid = Convert.ToInt32(context.Request["tid"]);
                int pId = Convert.ToInt32(context.Request["projectId"]);
                String title = context.Request["title"] + "";
                String descr = context.Request["descr"] + "";
                String imageList = context.Request["imageList"] + "";
                String imageSizeList = context.Request["imageSizeList"] + "";
                string ckbIsPublic = context.Request["ckIsPublic"] + "";
                string chkIsWaitClientFeedback = context.Request["chkIsWaitClientFeedback"] + "";
                string chkIsWaitPMFeedback = context.Request["chkIsWaitPMFeedback"] + "";
                string ClientReplyFeedbackID = context.Request["ClientReplyFeedbackID"] + "";
                string PMReplyFeedbackID = context.Request["PMReplyFeedbackID"] + "";

                #endregion

                #region get user and entity

                UsersEntity entity = userApp.GetUser(IdentityContext.UserID);

                ProjectsEntity ProjectEntity = projectApp.Get(pId);

                #endregion

                #region set value,add feedback

                FeedBacksEntity feedbacksEntity = new FeedBacksEntity();

                feedbacksEntity.IsDelete = false;
                feedbacksEntity.TicketID = tid;
                feedbacksEntity.Title = title.NoHTML();
                feedbacksEntity.Description = descr.NoHTML().Replace("\n", "<br/>");
                feedbacksEntity.CreatedBy = IdentityContext.UserID;
                feedbacksEntity.CreatedOn = DateTime.Now;
                feedbacksEntity.ModifiedOn = DateTime.Now;
                feedbacksEntity.IsPublic = string.IsNullOrEmpty(ckbIsPublic) ? false : Boolean.Parse(ckbIsPublic);
                feedbacksEntity.WaitClientFeedback = string.IsNullOrEmpty(chkIsWaitClientFeedback) ? 0 : Boolean.Parse(chkIsWaitClientFeedback) ? 1 : 0;
                feedbacksEntity.WaitPMFeedback = string.IsNullOrEmpty(chkIsWaitPMFeedback) ? 0 : Boolean.Parse(chkIsWaitPMFeedback) ? 1 : 0;
                if (feedbacksEntity.WaitClientFeedback > 0)
                    feedbacksEntity.IsPublic = true;

                feedbacksEntity.ID = fbAPP.AddFeedBacks(feedbacksEntity);

                if (feedbacksEntity.ID <= 0)
                {
                    context.Response.Write("Feedback add fail.");
                    return;
                }

                int tmpReplyFeedbackId;
                int.TryParse(ClientReplyFeedbackID, out tmpReplyFeedbackId);
                if (tmpReplyFeedbackId > 0)
                {
                    fbmApp.DeleteClientFeedback(feedbacksEntity.CreatedBy, feedbacksEntity.TicketID, tmpReplyFeedbackId);
                }
                else if (int.TryParse(PMReplyFeedbackID, out tmpReplyFeedbackId))
                {
                    if (tmpReplyFeedbackId > 0)
                        fbmApp.DeletePMFeedback(feedbacksEntity.CreatedBy, feedbacksEntity.TicketID, tmpReplyFeedbackId);
                }

                if (fbmApp.Add(feedbacksEntity)) //feedbackmessages
                {
                    if (feedbacksEntity.WaitClientFeedback > 0 ||
                          (feedbacksEntity.WaitClientFeedback <= 0 &&
                         feedbacksEntity.IsPublic))
                    {
                        ticketStatusMgr.SendEmailtoClientForFeedBack(feedbacksEntity);
                    }
                    else if (feedbacksEntity.WaitPMFeedback > 0)
                    {
                        ticketStatusMgr.SendEmailtoPMForFeedBack(feedbacksEntity);
                    }
                }
                #endregion

                #region add file

                FilesEntity fileEntity = new FilesEntity();

                if (null != ProjectEntity)
                {
                    FolderName = ProjectEntity.ProjectID.ToString();
                }

                string sNewFileName = "";

                tempPath = System.Configuration.ConfigurationManager.AppSettings["FolderPath"];

                string[] listStringName = imageList.Split(',');

                string[] listStringSize = imageSizeList.Split(',');

                foreach (string Name in listStringName)
                {
                    if (Name.Length == 0) break;

                    string sExtension = Path.GetExtension(Name).Replace(".", "").Trim();

                    foreach (string Size in listStringSize)
                    {
                        sNewFileName = FolderName + Name;
                        fileEntity.ContentType = "." + sExtension.ToLower();
                        fileEntity.CreatedBy = entity.UserID;
                        fileEntity.FilePath = tempPath.Substring(2) + FolderName + @"/" + sNewFileName;
                        fileEntity.FileSize = Convert.ToDecimal(Size.ToLower().Replace("kb", ""));
                        fileEntity.FileTitle = Name.Substring(0, Name.LastIndexOf('.'));
                        fileEntity.IsPublic = feedbacksEntity.IsPublic;
                        fileEntity.FeedbackId = feedbacksEntity.ID;
                        fileEntity.TicketId = tid;
                        fileEntity.ProjectId = pId;
                        fileEntity.SourceType = (int)FileSourceType.FeedBack;
                        fileEntity.ThumbPath = context.Server.MapPath(tempPath) + FolderName + sNewFileName; ;//
                        fileEntity.CreatedOn = DateTime.Now.Date;
                        fileEntity.CompanyID = ticketApp.GetCompanyIdByTicketId(tid);

                        int result = fileApp.AddFile(fileEntity);

                        if (result <= 0)
                        {
                            HasFileMsG = false;
                            stringErrorMsg.Add(fileEntity.FileTitle);
                        }
                        break;
                    }
                }

                #endregion

                #region response
                if (feedbacksEntity.ID > 0)
                {
                    if (HasFileMsG)
                    {
                        context.Response.Write("Feedback add success.");
                    }
                    else
                    {
                        string error = "";
                        foreach (string item in stringErrorMsg)
                        {
                            error += item + "File Upload failed.";
                        }
                        context.Response.Write(error);
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoAddFeedBackHandler.ashx Messages:\r\n{0}", ex));
                return;
            }


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
