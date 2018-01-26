using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Web.Do;

namespace SunNet.PMNew.PM2014.Timesheet
{
    public partial class Date : BasePage
    {
        private CateGoryApplication cgApp = new CateGoryApplication();
        private ProjectApplication projApp = new ProjectApplication();
        private TimeSheetApplication tsApp = new TimeSheetApplication();
        protected DateTime SelectedDate { get; set; }
        protected string CategoryJson { get; set; }
        protected string ProjectJson { get; set; }
        public decimal totalQWeeklyHours { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.Params["date"]))
                {
                    this.SelectedDate = Convert.ToDateTime(Request.QueryString["date"]);
                    if (!TimeSheetTicket.CanEdit(this.SelectedDate))
                    {
                        Redirect("Index.aspx");
                    }
                    if (SelectedDate.Date > DateTime.Now)
                    {
                        btnSubmit.Visible = false;
                    }
                }
                else
                {
                    this.SelectedDate = DateTime.Now;
                }

            }
            catch
            {
                this.SelectedDate = DateTime.Now;
            }
            if (!IsPostBack)
            {
                Title = "Write timesheet - " + SelectedDate.ToString("MM/dd/yyyy");

                List<CateGoryEntity> list = cgApp.GetCateGroyListByUserID(UserInfo.ID);
                if (list == null) list = new List<CateGoryEntity>();
                var category = list.Select(x => new { category = x.Title, id = x.ID }).ToList();
                category.Insert(0, new { category = DefaulSelectText, id = 0 });
                this.CategoryJson = JsonConvert.SerializeObject(category, DoBase.DateConverter);

                List<ProjectDetailDTO> listProj = projApp.GetUserProjects(UserInfo);
                if (listProj != null && listProj.Count > 0)
                {
                    listProj.RemoveAll(r => r.Status == ProjectStatus.Cancelled || r.Status == ProjectStatus.Completed);
                }
                var project = listProj.Select(x => new { title = x.Title, id = x.ProjectID, tickets = new List<int>() }).ToList();
                project.Insert(0, new { title = DefaulSelectText, id = 0, tickets = new List<int>() });
                this.ProjectJson = JsonConvert.SerializeObject(project, DoBase.DateConverter);
            }
            totalQWeeklyHours = tsApp.TotalWeeklyHours(this.SelectedDate,UserInfo.ID);
        }

        public delegate void SendHandler();

        private void SendEmailToHr()
        {
            string contentTemplete = FileHelper.GetEmailTemplate("TimehseetNoticeToHR.txt");
            string from = Config.DefaultSendEmail;
            string to = Config.NoSubmittedEmailAddrs;
            string subject = string.Format("Timesheet Submit Status Reminder - {0}", UserInfo.FirstAndLastName);
            string content = string.Empty;
            if (!string.IsNullOrEmpty(contentTemplete.Trim()))
            {
                content = contentTemplete.Trim().Replace("{Firstname}", UserInfo.FirstName)
                                                .Replace("{Lastname}", UserInfo.LastName)
                                                .Replace("{Date}", SelectedDate.ToString("MM/dd/yyyy"))
                                                .Replace("{EmailDate}", DateTime.Now.ToString("MM/dd/yyyy"));
            }
            IEmailSender sender = ObjectFactory.GetInstance<IEmailSender>();
            sender.SendMail(to, from, subject, content);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tsApp.SubmitTimeSheets(this.SelectedDate, UserInfo.ID))
            {
                if (chkNoticeHR.Checked)
                {
                    new SendHandler(SendEmailToHr).BeginInvoke(null, null);
                }
                Redirect(Request.RawUrl, true);
            }
            else
            {
                this.ShowFailMessageToClient(tsApp.BrokenRuleMessages, false);
            }
        }
    }
}