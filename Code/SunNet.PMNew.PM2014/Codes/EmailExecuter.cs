using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		5/14 10:25:19
 * Description:		Please input class summary
 * Version History:	Created,5/14 10:25:19
 * 
 * 
 **************************************************************************/
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.PM2014.Codes
{
    public class EmailExecuter
    {
        public EmailExecuter()
        {
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "Template\\NewDailyReportTemplate.html";
            if (File.Exists(filePath))
            {
                this.EmailBody = File.ReadAllText(filePath);
            }
            else
            {
                this.EmailBody = "";
            }

            // 初始化，访问每个Section，缓存模版，并从文件中移除
            var t = this.DateSection;
            t = this.UserContentSection;
            t = this.UserFirstRowSection;
            t = this.UsernoticketSection;
        }

        public string GetEmailExecuter(string fileName)
        {
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "Template\\" + fileName;
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                return "";
            }
        }
        public EmailExecuter(Type tp)
        {
            //string filePath = string.Empty;
            //if (tp == typeof(TicketNote) || tp == typeof(BugNote))
            //{
            //    filePath = System.AppDomain.CurrentDomain.BaseDirectory + "Template\\NoteTemplate.html";
            //}
            //else
            //{
            //    filePath = System.AppDomain.CurrentDomain.BaseDirectory + "Template\\WorkTemplate.htm";
            //}
            //if (File.Exists(filePath))
            //{
            //    this.EmailBody = File.ReadAllText(filePath);
            //}
            //else
            //{
            //    this.EmailBody = "";
            //}
        }
        protected string EmailBody
        {
            get;
            set;
        }
        private string GetHtmlNodeFromTemplate(string emailTag)
        {
            string search = string.Empty;
            Regex reg = new Regex(string.Format(@"<tr.*(?={0})(.|\n)*?</tr>", emailTag));
            MatchCollection matches = reg.Matches(this.EmailBody, 0);
            if (matches != null && matches.Count == 1)
            {
                search = matches[0].Value;
                this.EmailBody = reg.Replace(this.EmailBody, "");
            }
            return search;
        }
        private string _dateSection;
        protected string DateSection
        {
            get
            {
                if (string.IsNullOrEmpty(_dateSection))
                {
                    _dateSection = GetHtmlNodeFromTemplate("dateSection");
                }
                return _dateSection;
            }
        }

        private string _userFirstRowSection;
        protected string UserFirstRowSection
        {
            get
            {
                if (string.IsNullOrEmpty(_userFirstRowSection))
                {
                    _userFirstRowSection = GetHtmlNodeFromTemplate("userfirstrowSection");
                }
                return _userFirstRowSection;
            }
        }
        private string _userContentSection;
        public string UserContentSection
        {
            get
            {
                if (string.IsNullOrEmpty(_userContentSection))
                {
                    _userContentSection = GetHtmlNodeFromTemplate("usercontentSection");
                }
                return _userContentSection;
            }
        }

        private string _usernoticketSection;
        public string UsernoticketSection
        {
            get
            {
                if (string.IsNullOrEmpty(_usernoticketSection))
                {
                    _usernoticketSection = GetHtmlNodeFromTemplate("usernoticketSection");
                }
                return _usernoticketSection;
            }
        }

        public class Compairint : IEqualityComparer<TimeSheetTicket>
        {

            #region IEqualityComparer<TimeSheet> Members

            public bool Equals(TimeSheetTicket x, TimeSheetTicket y)
            {
                return x.ProjectID == y.ProjectID;
            }

            public int GetHashCode(TimeSheetTicket obj)
            {
                return obj.ProjectID.ToString().GetHashCode();
            }

            #endregion
        }

        public bool SendDailyCNMails()
        {
            return SendDailyCNMails(DateTime.Today);
        }
        public bool SendDailyCNMails(DateTime date)
        {
            if (Config.weekPlanNoticeDayofWeek != null)
            {
                if (DateTime.Now.DayOfWeek == Config.weekPlanNoticeDayofWeek)
                {
                    SendWeekPlanMail("CN");
                }
            }
            return SendDailyMail(date, "CN");
        }
        public bool SendDailyUSMails()
        {
            return SendDailyUSMails(DateTime.Today);
        }
        public bool SendDailyUSMails(DateTime date)
        {
            if (Config.weekPlanNoticeDayofWeek != null)
            {
                if (DateTime.Now.DayOfWeek == Config.weekPlanNoticeDayofWeek)
                {
                    SendWeekPlanMail("US");
                }
            }
            return SendDailyMail(date, "US");
        }
        #region EmailContent Send
        public string GetUserTicketsHtml(UsersEntity user, List<TimeSheetTicket> tickets)
        {
            StringBuilder ticketsHtml = new StringBuilder();
            bool firstRow = true;
            decimal totalHours = 0;

            string firstTmp = UserFirstRowSection;
            string content = UserContentSection;
            foreach (TimeSheetTicket ticket in tickets)
            {
                string tmp = firstRow ? firstTmp : content;
                tmp = tmp.Replace("{UserName}", string.Format("{0} {1}", user.FirstName, user.LastName));
                tmp = tmp.Replace("{Project}", ticket.ProjectTitle);
                tmp = tmp.Replace("{Ticket}", ticket.TicketTitle);
                tmp = tmp.Replace("{Description}", ticket.WorkDetail);
                tmp = tmp.Replace("{Percent}", ticket.Percentage.ToString());
                tmp = tmp.Replace("{Hour}", ticket.Hours.ToString());
                ticketsHtml.Append(tmp);
                firstRow = false;
                totalHours += ticket.Hours;
            }
            ticketsHtml.Replace("{Rowspan}", tickets.Count.ToString());
            ticketsHtml.Replace("{TotalHour}", totalHours.ToString());
            if (totalHours < 8)
            {
                ticketsHtml.Replace("{BackgroundColor_Css}", "td_yellow");
            }
            else
            {
                ticketsHtml.Replace("{BackgroundColor_Css}", "td_blue");
            }
            return ticketsHtml.ToString();
        }
        public string GetUserNoTicketsHtml(UsersEntity user)
        {
            string tmp = UsernoticketSection;

            string content = tmp.Replace("{UserName}", string.Format("{0} {1}", user.FirstName, user.LastName));
            return content;
        }
        public string GetUserNoTicketsHtml(List<UsersEntity> users)
        {
            StringBuilder ticketsHtml = new StringBuilder();
            foreach (UsersEntity user in users)
            {
                ticketsHtml.Append(GetUserNoTicketsHtml(user));
            }
            return ticketsHtml.ToString();
        }

        public string GetDateSectionHtml(DateTime dt, int count)
        {
            string content = DateSection;
            content = content.Replace("{Date}", dt.ToString("MM/dd/yyyy"));
            content = content.Replace("{Sum_Rowspan}", count.ToString());
            return content;
        }

        public bool SendWeekPlanMail(string office)
        {
            WeekPlanApplication weekPlanApplication = new WeekPlanApplication();
            UserApplication userApp = new UserApplication();
            List<UsersEntity> usersHavenotSubmitWeekPlan = new List<UsersEntity>();
            List<UsersEntity> allUsers = userApp.GetActiveUserList().FindAll(u => u.Role != RolesEnum.CLIENT && u.IsNotice);
            List<UsersEntity> currentUsers = allUsers.FindAll(u => u.Office.ToUpper() == office.ToUpper());
            DateTime weekDay = DateTime.Now;
            if (weekDay.DayOfWeek != DayOfWeek.Sunday)
            {
                weekDay = weekDay.AddDays(-(int)DateTime.Now.DayOfWeek).AddDays(7);
            }

            foreach (UsersEntity user in currentUsers)
            {
                WeekPlanEntity weekPlanEntity = weekPlanApplication.GetInfo(user.UserID, weekDay);
                if (weekPlanEntity == null)
                {
                    usersHavenotSubmitWeekPlan.Add(user);
                }
            }

            SendWeekPlanMail(usersHavenotSubmitWeekPlan, weekDay.Date);
            return true;
        }

        public bool SendDailyMail(DateTime date, string office)
        {
            UserApplication userApp = new UserApplication();
            // sunneters
            List<UsersEntity> allUsers = userApp.GetActiveUserList().FindAll(u => u.Role != RolesEnum.CLIENT && u.IsNotice);
            List<UsersEntity> currentUsers = allUsers.FindAll(u => u.Office.ToUpper() == office.ToUpper());
            List<UsersEntity> noSubmittedUsers = new List<UsersEntity>();

            TimeSheetApplication tsApp = new TimeSheetApplication();
            SearchTimeSheetsRequest request = new SearchTimeSheetsRequest(SearchType.EmailNotice,
               false, "ModifiedOn", "ASC");
            request.Office = office;
            request.SearchDate = date;
            SearchTimeSheetsResponse response = tsApp.QueryTimesheet(request);
            if (response.IsError)
            {
                WebLogAgent.Write("SunNet.PMNew.Web.Codes.EmailExecuter.SendDailyMail:136,GetDataError");
                return false;
            }

            StringBuilder ticketHtml = new StringBuilder();

            StringBuilder usersTickets = new StringBuilder();
            if (response.TimeSheetsList.Count == 0)
            {
                // no timesheet,clear htmlTemplate nodes
                string emptyTmeplate = UserFirstRowSection;
                emptyTmeplate = UserContentSection;
            }
            foreach (UsersEntity user in currentUsers)
            {
                List<TimeSheetTicket> userTickets = response.TimeSheetsList.FindAll(tts => tts.UserID == user.ID);
                if (userTickets == null || userTickets.Count == 0)
                {
                    noSubmittedUsers.Add(user);

                    continue;
                }
                else
                {
                    usersTickets.Append(GetUserTicketsHtml(user, userTickets));
                }
            }
            ticketHtml.Append(GetDateSectionHtml(date, response.TimeSheetsList.Count + noSubmittedUsers.Count + 1));
            ticketHtml.Append(usersTickets);
            ticketHtml.Append(GetUserNoTicketsHtml(noSubmittedUsers));
            // 不需要没有写的汇总了，因为另一封已经包含了。
            //SendNoSubmitEmail(noSubmittedUsers, date);

            foreach (UsersEntity user in noSubmittedUsers)
            {
                SendNoticeToUser(user, date);
            }

            SendSubmittedEmail(ticketHtml.ToString(), date);
            return true;
        }
        public void SendSubmittedEmail(string content, DateTime dt)
        {
            string subject = string.Format(" Daily report summary on {0}", dt.ToString("MM/dd/yyyy"));
            string emailbody = this.EmailBody.Replace("{EmailContent}", content);
            string to = Config.SubmittedEmailAddrs;
            this.SendMail(to, "", subject, emailbody);
        }

        public void SendNoSubmitEmail(List<UsersEntity> users, DateTime dt)
        {
            string htmlTemplate = GetEmailExecuter("NoSubmitTimeSheet.htm");

            string search = string.Empty;
            Regex reg = new Regex(@"<tr.*(?=blueBg)(.|\n)*?</tr>");
            MatchCollection matches = reg.Matches(htmlTemplate, 0);
            if (matches != null && matches.Count == 1)
            {
                search = matches[0].Value;
                htmlTemplate = reg.Replace(htmlTemplate, "");
            }
            string template = search;
            StringBuilder emailContent = new StringBuilder();
            emailContent.Append(htmlTemplate);

            StringBuilder usersHtml = new StringBuilder();
            foreach (UsersEntity user in users)
            {
                string tmp = template;
                tmp = tmp.Replace("{Date}", dt.ToString("MM/dd/yyyy"));
                tmp = tmp.Replace("{Name}", string.Format("{0} {1}", user.FirstName, user.LastName));
                tmp = tmp.Replace("{Title}", "&nbsp;&nbsp;There is no timesheet for completing.");
                usersHtml.Append(tmp);
                SendNoticeToUser(user, dt);
            }
            emailContent.Replace("{EmailContent}", usersHtml.ToString());
            string to = Config.NoSubmittedEmailAddrs;
            string subject = "Timesheet Submit Status Reminder";
            this.SendMail(to, "", subject, emailContent.ToString());
        }
        public void SendNoticeToUser(UsersEntity user, DateTime dt)
        {
            if (dt.DayOfWeek != DayOfWeek.Saturday
                && dt.DayOfWeek != DayOfWeek.Sunday
                && user.Role != RolesEnum.CLIENT
                && user.Role != RolesEnum.ADMIN)
            {
                string username = string.Format("{0} {1}", user.FirstName, user.LastName);
                string body = string.Format(@"<p class='MsoNormal' style='text-indent:0pt'>
                        <span lang=EN-US style='font-size:11.0pt;font-family:Calibri,sans-serif;color:#1F497D'>
                        Hello <span>{0}</span>,
                        <o:p></o:p></span></p>
                        <p class='MsoNormal' style='text-indent:20.0pt'>
                        <span lang='EN-US' style='font-size:11.0pt;font-family:Calibri,sans-serif;color:#1F497D'>
                        Your timesheet on <span>{1}</span> is missing. Please submit it as soon as possible. <o:p></o:p></span></p>",
                             username,
                             dt.ToString("MM/dd/yyyy")
                             );
                string subject = "Timesheet Submit Status Reminder";
                string to = user.Email;
                this.SendMail(to, "", subject, body);
            }
        }
        private void SendWeekPlanMail(List<UsersEntity> users, DateTime weekDay)
        {
            string htmlTemplate = GetEmailExecuter("WeekPlanNotice.txt");
            string search = string.Empty;
            foreach (UsersEntity user in users)
            {
                string emailContent = string.Empty;
                emailContent = htmlTemplate.Replace("{FirstName}", user.FirstName);
                if (user.Role != RolesEnum.CLIENT
                && user.Role != RolesEnum.ADMIN)
                {
                   // SendMail(user.Email, "", "Notice of unsubmitted PM week plan.", emailContent); David 12/15/2015 暂时关闭通知
                }
            }
        }
        #endregion

        private void SendMail(string mailto,
            string cc, string subject, string mailbody)
        {

            //System.Web.Mail.MailMessage mail = new System.Web.Mail.MailMessage();
            //mail.BodyFormat = MailFormat.Html;

            //mail.To = mailto;
            //mail.Cc = cc;

            //mail.BodyEncoding = System.Text.Encoding.UTF8;

            //if (mailfrom == "") mailfrom = "null@null.com";
            //mail.From = mailfrom;
            //mail.Subject = subject;
            //mail.Body = mailbody;

            //SmtpMail.SmtpServer = System.Configuration.ConfigurationSettings.AppSettings["SmtpServer"];//System.Configuration.ConfigurationManager.ConnectionStrings[""].ToString();
            //try
            //{
            //    SmtpMail.Send(mail);
            //}
            //catch (Exception e)
            //{
            //}
            IEmailSender emailSender = ObjectFactory.GetInstance<IEmailSender>();
            emailSender.SendMail(mailto, Config.DefaultSendEmail, subject, mailbody);
        }

    }
}