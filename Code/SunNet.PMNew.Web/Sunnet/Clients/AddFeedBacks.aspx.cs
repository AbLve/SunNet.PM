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

namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class AddFeedBacks : BaseWebsitePage
    {
        FeedBackApplication fbAPP = new FeedBackApplication();
        TicketsApplication ticketsApp = new TicketsApplication();
        FeedBackMessagesApplication fbmApp = new FeedBackMessagesApplication();
        TicketStatusManagerApplication ticketStatusMgr = new TicketStatusManagerApplication();

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
                if (feedbackId > 0) //回复
                {
                    FeedBacksEntity entity = fbAPP.GetFeedBacksEntity(feedbackId);
                    projectidanduseridResponse = ticketsApp.GetProjectIdAndUserID(entity.TicketID);
                    if (!CheckSecurity(projectidanduseridResponse))
                    {
                        this.ShowArgumentErrorMessageToClient();
                        return;
                    }
                    txtTitle.Value = string.Format("Re:{0}", entity.Title);
                    Page.Title = "Reply Note";
                    bannerTitle.Text = "Reply Note";
                    trOthers.Visible = false;
                    trOriDesc.Visible = true;
                    trOriFile.Visible = true;
                    trOriDate.Visible = true;
                    lblDate.InnerText = entity.CreatedOn.ToString("MM/dd/yyyy");
                    txtOriginalContent.Attributes["readonly"] = "readonly";
                    this.txtOriginalContent.Value = entity.Description;
                    this.lblFiles.InnerHtml = ShowImageList(entity.FeedBackID);

                    hdTicketID.Value = entity.TicketID.ToString();
                }
                else //添加
                {
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
                }
                hdProjectID.Value = projectidanduseridResponse.ProjectId.ToString();
            }
                #endregion
        }



        private bool CheckSecurity(GetProjectIdAndUserIDResponse info)
        {
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                if (UserInfo.UserID != info.CreateUserId && UserInfo.CompanyID != info.CompanyId)
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
            FeedBacksEntity feedbacksEntity = new FeedBacksEntity();

            feedbacksEntity.IsDelete = false;
            feedbacksEntity.TicketID = int.Parse(hdTicketID.Value);
            feedbacksEntity.Title = NoHTML(txtTitle.Value);
            feedbacksEntity.Description = txtContent.Value;
            feedbacksEntity.CreatedBy = UserInfo.UserID;
            feedbacksEntity.CreatedOn = DateTime.Now;
            feedbacksEntity.ModifiedOn = DateTime.Now;
            //如果是Client则IsPublic为true， 否则就看IsPublic Checkbox是否有选中。 
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

            if (QS("feedbackId", 0) > 0) //回复
            {
                FeedBacksEntity entity = fbAPP.GetFeedBacksEntity(QS("feedbackId", 0));
                if (entity.WaitClientFeedback == 1)  //删除通知Client回复的消息，并且将　需要回复的　Feedbacks标记为已回复
                    fbmApp.DeleteClientFeedback(feedbacksEntity.CreatedBy, entity.TicketID, entity.FeedBackID);
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
            ShowSuccessMessageToClient(true, true);
        }

        public static string NoHTML(string Htmlstring)
        {
            if (Htmlstring == null)
            {
                return "";
            }
            Htmlstring = Regex.Replace(Htmlstring, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(iexcl|#161);", "\x00a1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(cent|#162);", "\x00a2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(pound|#163);", "\x00a3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(copy|#169);", "\x00a9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
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
    }
}
