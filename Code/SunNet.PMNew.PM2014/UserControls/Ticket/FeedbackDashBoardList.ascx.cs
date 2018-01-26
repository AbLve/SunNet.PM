using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.UserControls.Ticket
{
    public partial class FeedbackDashBoardList : BaseAscx
    {
        private DateTime showAllDateCache = DateTime.MinValue;
        public TicketsEntity TicketsEntityInfo;
        public UsersEntity userInfo;
        public int ProjectID { get; set; }
        public int TicketID { get; set; }
        private bool isShowAdd = true;

        public bool IsShowAdd
        {
            get { return isShowAdd; }
            set { isShowAdd = value; }
        }
        public bool IsSunnet { get; set; }
        FeedBackApplication fbAPP = new FeedBackApplication();
        UserApplication userApp = new UserApplication();
        TicketsApplication ticketApp = new TicketsApplication();
        private bool hasWaitingStatus = false;

        public bool IsWaitClientFeedback;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (TicketsEntityInfo != null && TicketsEntityInfo.TicketID > 0)
            {
                TicketID = TicketsEntityInfo.TicketID;
                ProjectID = TicketsEntityInfo.ProjectID;
                BindFeedBacks(TicketsEntityInfo.TicketID);
                if (!IsShowAdd)
                {
                    //divAddFeedback.Visible = false;
                }
                Initial();
            }
            else
            {
                this.Visible = false;
            }

            ticketApp.ClearNotification(TicketID, UserInfo.ID);
        }

        private void Initial()
        {
            chkIsWaitSunnetFeedback.Visible = false;
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                this.ckIsPublic.Visible = false;
                this.IsWaitClientFeedback = false;
                this.chkIsWaitSunnetFeedback.Visible = true;
            }
            else if (UserInfo.Role == RolesEnum.Supervisor)
            {
                this.ckIsPublic.Visible = false;
                this.IsWaitClientFeedback = false;
                this.chkIsWaitSunnetFeedback.Visible = false;
                //trOthers.Visible = false;
            }
            else
            {
                if (UserInfo.Role == RolesEnum.QA || UserInfo.Role == RolesEnum.DEV
                    || UserInfo.Role == RolesEnum.Leader || UserInfo.Role == RolesEnum.Contactor)
                {
                    this.ckIsPublic.Visible = false;
                    this.IsWaitClientFeedback = false;
                }
                else
                {
                    this.ckIsPublic.Visible = true;
                    this.IsWaitClientFeedback = true;
                }

                if (TicketsEntityInfo.IsInternal && UserInfo.Role == RolesEnum.PM)
                {
                    this.ckIsPublic.Visible = false;
                    this.IsWaitClientFeedback = false;
                    //trOthers.Visible = false;
                }
            }

            var visibleToPm = ticketApp.IsTicketUser(TicketID, UserInfo.ID, TicketUsersType.PM);
            var visibleToClient = ticketApp.IsTicketUser(TicketID, UserInfo.ID, TicketUsersType.Create)
                && UserInfo.Role == RolesEnum.CLIENT;
            phlClientUsers.Visible = visibleToClient && TicketsEntityInfo.IsInternal == false;  //visibleToClient;
            phlClientUsers2.Visible = visibleToPm && TicketsEntityInfo.IsInternal == false;// visibleToPm;
            if (visibleToPm || visibleToClient)
            {
                InitClientUsers();
            }
            if (!ticketApp.CanFeedbackWaiting(TicketID))
            {
                IsWaitClientFeedback = false;
                chkIsWaitSunnetFeedback.Visible = false;
            }
        }

        private void InitClientUsers()
        {
            SearchUsersRequest searchUserRequest = new SearchUsersRequest(
                SearchUsersType.Project, false, " FirstName ", " ASC ");

            searchUserRequest.ProjectID = ProjectID;
            SearchUserResponse response = userApp.SearchUsers(searchUserRequest);
            var listUsers = response.ResultList.FindAll(x => x.Role == RolesEnum.CLIENT && x.UserID != UserInfo.UserID);

            var listTicketUsers = ticketApp.GetListUsersByTicketId(TicketID);
            var tmpList = listUsers.Select(x => new
            {
                x.ID,
                ProjectID = ProjectID,
                x.FirstName,
                x.LastName,
                x.FirstAndLastName,
                x.LastNameAndFirst,
                Writed = hasWaitingStatus && listTicketUsers.Any(r => r.UserID == x.ID)
                        && listTicketUsers.Find(r => r.UserID == x.ID).TicketStatus == UserTicketStatus.Normal,
                Selected = listTicketUsers.Any(r => r.UserID == x.ID)
                        && listTicketUsers.Find(r => r.UserID == x.ID).TicketStatus != UserTicketStatus.Normal,
                Update = listTicketUsers.Any(r => r.UserID == x.ID) ? listTicketUsers.Find(r => r.UserID == x.ID).TUID : 0,
                x.Role
            }).ToList();
            rptClient.DataSource = tmpList;
            rptClient.DataBind();

            rptClient2.DataSource = tmpList;
            rptClient2.DataBind();
        }


        public void BindFeedBacks(int tid)
        {
            List<FeedBacksEntity> list = new List<FeedBacksEntity>();
            list = fbAPP.GetFeedBackListByTicketId(tid, IsSunnet, IsSunnetMember(),
                UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN,
                UserInfo.Role == RolesEnum.Supervisor);
            if (null != list && list.Count > 0)
            {
                list = list.FindAll(r => r.IsDelete == false);
                hasWaitingStatus =
                    list.Any(
                        x => x.WaitClientFeedback == FeedbackReplyStatus.Requested);
                FbCreatedOn = list.Select(x => x.CreatedOn.ToString()).ToList();
                this.rptFeedBacksList.DataSource = list;
                this.rptFeedBacksList.DataBind();
            }
        }

        protected List<String> FbCreatedOn { get; set; }

        public UsersEntity UserInfo
        {
            get
            {
                return userInfo;
            }

            set
            {
                userInfo = value;
            }
        }

        public string GetFeedBackHTMLBegin(object entity)
        {
            var feedback = (FeedBacksEntity)entity;

            var description = feedback.Description.Trim("\n".ToCharArray());
            StringBuilder stringBuilder = new StringBuilder();
            var filesHTML = ShowImageList(feedback.FeedBackID.ToString());
            var lastTwoDaysStyle = "";
            if (FbCreatedOn != null && FbCreatedOn.Count > 3)
            {
                if (FbCreatedOn.IndexOf(feedback.CreatedOn.ToString()) < FbCreatedOn.Count - 3)
                    lastTwoDaysStyle = "style='display:none;'";
            }
            if (feedback.CreatedOn.Date != showAllDateCache.Date)
            {
                showAllDateCache = feedback.CreatedOn.Date;
                stringBuilder.AppendFormat("<li class='fdmessageDate' date='{1}'  {0}>{1}</li>", lastTwoDaysStyle, feedback.CreatedOn.Date.ToString("MM/dd/yyyy"));
            }

            //Request response from
            if (feedback.WaitClientFeedback == FeedbackReplyStatus.Requested)
            {
                string str = ResponsibleUsers();
                if (str.Length > 0)
                {
                    stringBuilder.AppendFormat("<div class='requestfrom'><a>Request Response from {0}</a></div>", str);
                }
            }

            if (feedback.CreatedBy != UserInfo.UserID)
            {
                stringBuilder.AppendFormat("<li class='otherbox' index={2}  date='{1}' {0}>",
                    lastTwoDaysStyle, feedback.CreatedOn.Date.ToString("MM/dd/yyyy"), feedback.Order)
                    .Append("<span class='fdUser'>" + BasePage.GetClientUserName(feedback.CreatedBy) + "</span>")
                    .AppendFormat("<span class='fdDate'>{0}</span>", feedback.CreatedOn.ToString("hh:mm tt"))
                    .Append("<table border='0' cellspacing='0' cellpadding='4'>")
                    .Append("<tbody><tr><td class='fdcontentBox1 fdcontent' style='max-width:40%; word-break:break-all;'><div class='fdarrowboxleft'></div>")
                    .Append(FormatOrder(feedback) + ". ");
            }
            else
            {
                if (feedback.IsPublic && UserInfo.Role == RolesEnum.PM)
                {
                    stringBuilder.AppendFormat("<li class='myselfbox' index={2}  date='{1}'  {0}>", lastTwoDaysStyle, feedback.CreatedOn.Date.ToString("MM/dd/yyyy"), feedback.Order)
                    .Append("<div style='text-align: right;'><span class='fdUser'>" + BasePage.GetClientUserName(feedback.CreatedBy) + "</span>")
                    .AppendFormat("<span class='fdDate'>{0}</span></div>", feedback.CreatedOn.ToString("hh:mm tt"))
                    .Append("<table border='0' cellpadding='4' cellspacing='0'>")
                    .Append("<tbody><tr><td class='fdcontentBox3 fdcontent' style='max-width:40%; word-break:break-all;'> ")
                    .Append("<div class='rightClose' onclick='deleteOwnFeedBack(this," + feedback.FeedBackID + "," + UserInfo.UserID + ")' title='Delete'>×</div>")
                    .Append(FormatOrder(feedback) + ". ");
                }
                else
                {
                    stringBuilder.AppendFormat("<li class='myselfbox' index={2}  date='{1}'  {0}>", lastTwoDaysStyle, feedback.CreatedOn.Date.ToString("MM/dd/yyyy"), feedback.Order)
                    .Append("<div style='text-align: right;'><span class='fdUser'>" + BasePage.GetClientUserName(feedback.CreatedBy) + "</span>")
                    .AppendFormat("<span class='fdDate'>{0}</span></div>", feedback.CreatedOn.ToString("hh:mm tt"))
                    .Append("<table border='0' cellpadding='4' cellspacing='0'>")
                    .Append("<tbody><tr><td class='fdcontentBox2 fdcontent' style='max-width:40%; word-break:break-all;'> ")
                    .Append("<div class='rightClose' onclick='deleteOwnFeedBack(this," + feedback.FeedBackID + "," + UserInfo.UserID + ")' title='Delete'>×</div>")
                    .Append(FormatOrder(feedback) + ". ");
                }
            }
            return stringBuilder.ToString();
        }

        public string GetFeedBackHTMLEnd(object entity)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var feedback = (FeedBacksEntity)entity;
            var description = feedback.Description.Trim("\n".ToCharArray());
            var filesHTML = ShowImageList(feedback.FeedBackID.ToString());
            if (feedback.CreatedBy != UserInfo.UserID)
            {
                stringBuilder
                    .Append((string.IsNullOrEmpty(description) ? "" : "<br/>"));
                if (filesHTML.Length > 0)
                {
                    stringBuilder.Append("<span class='fdfileBox'>" + filesHTML + "</span>");
                }
                stringBuilder.Append("</td>")
                        .Append("</tr></tbody></table></li>");

            }
            else
            {
                stringBuilder
                    .Append((string.IsNullOrEmpty(description) ? "" : "<br/>"));
                if (filesHTML.Length > 0)
                {
                    stringBuilder.Append("<span class='fdfileBox'>" + filesHTML + "</span>");
                }
                stringBuilder.Append("</td>")
                        .Append("</tr></tbody></table></li>");
            }
            return stringBuilder.ToString();
        }

        private string FormatOrder(FeedBacksEntity feedback)
        {
            var templates = new Dictionary<int, string>();
            templates.Add((int)FeedbackReplyStatus.Requested, "<span title='{0}' class='feedback_waiting'>#{1}</span>");
            templates.Add((int)FeedbackReplyStatus.Replied, "<span title='{0}' class='feedback_writed'>#{1}</span>");
            var clientStatusTexts = new Dictionary<int, string>();
            clientStatusTexts.Add((int)FeedbackReplyStatus.Requested, "Waiting for Client feedback");
            clientStatusTexts.Add((int)FeedbackReplyStatus.Replied, "Replied");
            var sunnetStatusTexts = new Dictionary<int, string>();
            sunnetStatusTexts.Add((int)FeedbackReplyStatus.Requested, "Waiting Sunnet feedback");
            sunnetStatusTexts.Add((int)FeedbackReplyStatus.Replied, "Replied");
            if (feedback.WaitClientFeedback != FeedbackReplyStatus.Normal)
                return string.Format(templates[(int)feedback.WaitClientFeedback],
                    clientStatusTexts[(int)feedback.WaitClientFeedback], feedback.Order);
            if (feedback.WaitPMFeedback != FeedbackReplyStatus.Normal)
                return string.Format(templates[(int)feedback.WaitPMFeedback],
                    sunnetStatusTexts[(int)feedback.WaitPMFeedback], feedback.Order);
            return string.Format("#{0}", feedback.Order);
        }

        public string ShowImageList(string tid)
        {
            if (string.IsNullOrEmpty(tid)) return "";
            List<FilesEntity> list = new List<FilesEntity>();
            string Host = System.Configuration.ConfigurationManager.AppSettings["DomainHost"];
            StringBuilder sb = new StringBuilder();
            FileApplication fileApp = new FileApplication();
            list = fileApp.GetFileListBySourceId(Convert.ToInt32(tid), FileSourceType.FeedBack);
            string[] imgFormat = ".gif,.jpg,.jpeg,.bmp,.png,.svg,.raw".Split(new char[] { ',' });
            foreach (FilesEntity filesEntity in list)
            {
                string style = "width:110px;height:110px;";
                if (!imgFormat.Any(format => format == filesEntity.ContentType))
                {
                    style = "width:13px;";
                    filesEntity.FilePath = "/Images/icons/file.png";
                }

                sb.AppendFormat("<span class='fdfile'><a href='{0}/do/DoDownloadFileHandler.ashx?FileID={1}&size={2}' target='_blank'><img src='{5}' style='{6}'> {3}{4}</a></span>"
                    , Host, filesEntity.FileID, filesEntity.FileSize, filesEntity.FileTitle, filesEntity.ContentType, filesEntity.FilePath, style);
            }
            return sb.ToString();
        }

        private bool IsSunnetMember()
        {
            //return UserInfo.Role != RolesEnum.CLIENT
            //                && UserInfo.Role != RolesEnum.Contactor;
            return UserInfo.Role != RolesEnum.CLIENT;//Steven 10/20/2016
        }


        public string FormatHTML(string source, HttpServerUtility Server)  //处理a标签和其他html标签的显示
        {
            bool isMulti = false;
            if (string.IsNullOrEmpty(source))
                return "";
            if (source.IndexOf("<a") > -1 && source.IndexOf("href") > -1 && source.IndexOf("</a>") > source.IndexOf("<a"))
            {
                StringBuilder sb = new StringBuilder();
                string first = source.IndexOf("<a") > 0 ? source.Substring(0, source.IndexOf("<a")) : "";
                string second = source.Substring(source.IndexOf("<a"), source.IndexOf("</a>") - source.IndexOf("<a") + 4);
                string third = source.Substring(source.IndexOf("</a>") + 4);
                if (!string.IsNullOrEmpty(first))
                    sb.Append(Server.HtmlEncode(first));
                sb.Append(second);
                while (third.IndexOf("<a") > -1 && third.IndexOf("href") > -1 && third.IndexOf("</a>") > third.IndexOf("<a"))
                {
                    first = third.IndexOf("<a") > 0 ? third.Substring(0, third.IndexOf("<a")) : "";
                    second = third.Substring(third.IndexOf("<a"), third.IndexOf("</a>") - third.IndexOf("<a") + 4);
                    third = third.Substring(third.IndexOf("</a>") + 4);
                    if (!string.IsNullOrEmpty(first))
                        sb.Append(Server.HtmlEncode(first));
                    sb.Append(second);
                    if (!string.IsNullOrEmpty(third))
                        sb.Append(Server.HtmlEncode(third));
                    isMulti = true;
                }
                if (!string.IsNullOrEmpty(third) && !isMulti)
                    sb.Append(Server.HtmlEncode(third));
                return sb.ToString();
            }
            else
            {
                string a = Server.HtmlEncode(source);
                return Server.HtmlEncode(source);
            }
        }

        public string ResponsibleUsers()
        {
            List<String> lst = ticketApp.GetUsersWithStatus(this.TicketID, UserTicketStatus.WaitClientFeedback, TicketUsersType.Create,
                                TicketUsersType.Client);

            string result = string.Join(", ", lst.ToArray());

            return result;
        }
    }
}