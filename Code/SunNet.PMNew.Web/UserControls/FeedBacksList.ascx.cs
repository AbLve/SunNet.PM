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
using SunNet.PMNew.Entity.FileModel;
using System.Text;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class FeedBacksList : BaseAscx
    {
        private string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public int TicketID { get; set; }

        public int ProjectID { get; set; }

        private bool isShowAdd = true;

        public bool IsShowAdd
        {
            get { return isShowAdd; }
            set { isShowAdd = value; }
        }

        #region declare
        FeedBackApplication fbApp = new FeedBackApplication();
  
        TicketsApplication ticketApp = new TicketsApplication();
        UserApplication userApp = new UserApplication();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TicketsEntityInfo != null && TicketsEntityInfo.TicketID > 0)
            {
                GetListFeedBackByTid(TicketsEntityInfo.TicketID);
                TicketID = TicketsEntityInfo.TicketID;
                ProjectID = TicketsEntityInfo.ProjectID;
                if (!IsShowAdd)
                {
                    divAddFeedback.Visible = false;
                }
                Initial();
            }
            else
            {
                this.Visible = false;
            }
        }

        private void GetListFeedBackByTid(int tid)
        {
            List<FeedBacksEntity> list = new List<FeedBacksEntity>();
            list = fbApp.GetFeedBackListByTicketId(tid, IsSunnet, IsSunnetMember(),
                UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN,
                UserInfo.Role == RolesEnum.Supervisor);
            if (null != list && list.Count > 0)
            {
                this.rptFeedBacksList.DataSource = list;

                this.rptFeedBacksList.DataBind();
            }
            else
            {
                //this.trNoFeedbacks.Visible = true;
            }
        }

        private bool IsSunnetMember()
        {
            return UserInfo.Role != RolesEnum.CLIENT
                            && UserInfo.Role != RolesEnum.Contactor;
        }

        public string ShowImageList(string tid)
        {
            FileApplication fileApp = new FileApplication();
            if (string.IsNullOrEmpty(tid)) return "";
            List<FilesEntity> list = new List<FilesEntity>();
            string Host = System.Configuration.ConfigurationManager.AppSettings["DomainHost"];
            StringBuilder sb = new StringBuilder();
            list = fileApp.GetFileListBySourceId(Convert.ToInt32(tid), FileSourceType.FeedBack);
            foreach (FilesEntity filesEntity in list)
            {
                sb.AppendFormat("<span class=\"fdfile\"><a href='{0}/do/DoDownloadFileHandler.ashx?FileID={1}&size={2}' target=\"iframeDownloadFile\");\"><img src=\"~\\Images\\icons\\file.png\"> {3}</a></span>"
                    , Host, filesEntity.FileID, filesEntity.FileSize, filesEntity.FileTitle);
            }
            return sb.ToString();
        }

        protected string ShowReply(object feedbackId, object ticketId, object WaitClientFeedback, object WaitPMFeedback)
        {
            FeedBacksEntity entity = fbApp.GetFeedBacksEntity((int)feedbackId);
            if (((entity.CreatedBy == UserInfo.UserID) ||
                (UserInfo.UserType == "SUNNET" || ticketEntity(ticketId).CompanyID == UserInfo.CompanyID)) &&
                ticketEntity(ticketId).Status != TicketsState.Completed &&
                ticketEntity(ticketId).Status != TicketsState.Cancelled)
            {
                if (UserInfo.Role == RolesEnum.CLIENT && (int)WaitClientFeedback == 1)
                    return string.Format("<a href=\"###\" onclick=\"OpenReplyFeedBackDialog({0},{1})\" title=\"Need FeedBack\"><img src=\"/icons/needReply.gif\" alt=\"Need FeedBack\" /></a>"
               , feedbackId, ticketId);
                else if (UserInfo.Role == RolesEnum.PM && (int)WaitPMFeedback == 1)
                    return string.Format("<a href=\"###\" onclick=\"OpenReplyFeedBackDialog({0},{1})\" title=\"Need FeedBack\"><img src=\"/icons/needReply.gif\" alt=\"Need FeedBack\" /></a>"
              , feedbackId, ticketId);
                else if ((int)WaitClientFeedback != 1 && (int)WaitPMFeedback != 1)
                    return string.Format("<a href=\"###\" onclick=\"OpenReplyFeedBackDialog({0},{1})\" title=\"Reply FeedBack\"><img src=\"/icons/reply.gif\" alt=\"Reply FeedBack\" /></a>"
               , feedbackId, ticketId);
            }
            return "";
        }

        private TicketsEntity ticketEntity(object ticketId)
        {
            TicketsEntity entity = ticketApp.GetTickets((int)ticketId);
            return entity;
        }

        #region public attribute
        public TicketsEntity TicketsEntityInfo { get; set; }
        public bool IsSunnet { get; set; }

        public bool IsSunneter
        {
            get;
            set;
        }

        #endregion

        private DateTime dateCache = DateTime.MinValue;
        public string GetFeedBackHTML(int createdByID, DateTime createdOn, bool isPublic
            , string title, string description, string feedbackID, object waitClientFeedback, object waitPMFeedback)
        {
            string repley = string.Empty;
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                if (waitClientFeedback.ToString() == "1")
                {
                    repley = "Waiting for Client feedback--";
                }
                else if (waitPMFeedback.ToString() == "1")
                {
                    repley = "Waiting PM feedback--";
                }
            }
            else if (UserInfo.Role != RolesEnum.CLIENT)
            {
                if (waitPMFeedback.ToString() == "1")
                {
                    repley = "Waiting PM feedback--";
                }
                else if (waitClientFeedback.ToString() == "1")
                {
                    repley = "Waiting for Client feedback--";
                }
            }

            StringBuilder stringBuilder = new StringBuilder();
            var filesHTML = ShowImageList(feedbackID);
            if (createdOn.Date != dateCache.Date)
            {
                dateCache = createdOn.Date;
                stringBuilder.Append("<li class=\"fdmessageDate\">" + dateCache.ToString("MM/dd/yyyy") + "</li>");
            }
            if (createdByID != UserInfo.UserID)
            {
                stringBuilder.Append("<li class=\"otherbox\"><table border=\"0\" cellspacing=\"0\" cellpadding=\"4\">")
                    .Append("<tbody><tr><td class=\"fdtdLeft\"><div class=\"fdUser\">"
                    + BaseWebsitePage.GetClientUserName(createdByID) + "</div>")
                    .Append("</td>")
                    .Append("<td class=\"fdcontentBox1\" ><div class=\"fdarrowbox1\"><img src=\"/images/fdarrow1.png\"></div><span class=\"fdNotice\">" + repley + "</span>")
                    .Append(description);
                if (filesHTML.Length > 0)
                {
                    stringBuilder.Append("<div class=\"fdfileBox\">" + filesHTML + "</div>");

                }
                stringBuilder.Append("</td>")
                        .Append("</tr></tbody></table></li>");

            }
            else
            {
                stringBuilder.Append("<li class=\"myselfbox\">")
                    .Append("<table border=\"0\" cellpadding=\"4\" cellspacing=\"0\">")
                    .Append("<tbody><tr><td class=\"fdcontentBox2\"> ")
                    .Append("<div class=\"rightClose\" onclick=\"deleteOwnFeedBack(" + feedbackID + "," + UserInfo.UserID + ")\" title=\"Delete\">×</div>")
                    .Append("<span class=\"fdNotice\">" + repley + "</span>" + description);
                if (filesHTML.Length > 0)
                {
                    stringBuilder.Append("<div class=\"fdfileBox\">" + filesHTML + "</div>");
                }

                stringBuilder.Append("</td>")
                .Append("<td class=\"fdtdright\">")
                .Append("<div class=\"fdarrowbox2\"><img src=\"/images/fdarrow2.png\"></div>")
                .Append("<div class=\"fdUser\">" + BaseWebsitePage.GetClientUserName(createdByID) + "</div>")
                .Append("</td></tr> </tbody></table></li>");
            }
            return stringBuilder.ToString();
        }

        #region set upload value

        public void SetUploadValue()
        {
            UploadFile1.ProjectName = this.hd_Project.ClientID; // ddlProject.ClientID;
            this.UploadFile1.PrimaryKey = this.ID;
        }

        #endregion

        private void Initial()
        {
            SetUploadValue();//set upload value
            hd_Project.Value = TicketsEntityInfo.ProjectID.ToString();
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                this.ckIsPublic.Visible = false;
                this.chkIsWaitClientFeedback.Visible = false;
                this.chkIsWaitPMFeedback.Visible = false;
                hfPublic.Value = "true";
            }
            else if (UserInfo.Role == RolesEnum.Supervisor)
            {
                this.ckIsPublic.Visible = false;
                this.chkIsWaitClientFeedback.Visible = false;
                this.chkIsWaitPMFeedback.Visible = false;
            }
            else
            {

                if (UserInfo.Role == RolesEnum.QA || UserInfo.Role == RolesEnum.DEV
                    || UserInfo.Role == RolesEnum.Leader || UserInfo.Role == RolesEnum.Contactor)
                {
                    this.ckIsPublic.Visible = false;
                    this.chkIsWaitClientFeedback.Visible = false;
                    this.chkIsWaitPMFeedback.Visible = true;
                }
                else
                {
                    this.ckIsPublic.Visible = true;
                    this.chkIsWaitClientFeedback.Visible = true;
                    this.chkIsWaitPMFeedback.Visible = false;
                }

                if (TicketsEntityInfo.IsInternal && UserInfo.Role == RolesEnum.PM)
                {
                    this.ckIsPublic.Visible = false;
                    this.chkIsWaitClientFeedback.Visible = false;
                    this.chkIsWaitPMFeedback.Visible = false;
                }
            }
        }
    }
}