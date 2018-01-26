using System;
using System.Linq;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using System.Web.UI.WebControls;
using SunNet.PMNew.Entity.TicketModel;
using System.Collections.Generic;
using System.Text;

namespace SunNet.PMNew.PM2014.WeekPlan
{
    public partial class Index : BasePage
    {
        public int weekDay = 10;

        private DateTime _date = DateTime.MinValue;
        private DateTime DateProvider
        {
            get
            {
                if (_date > DateTime.MinValue)
                    return _date;
                if (Request.UserLanguages != null)
                {
                    var zhcn =
                        Request.UserLanguages.ToList().Find(x => x.IndexOf("zh", StringComparison.CurrentCultureIgnoreCase) >= 0);
                    if (zhcn != null)
                    {
                        TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
                        DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(), cstZone);
                        _date = cstTime;
                    }
                    else
                    {
                        _date = DateTime.Now;
                    }
                }
                else
                {
                    _date = DateTime.Now;
                }
                return _date;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUsers();
                BindRepeater(QS("date"));
                txtDate.Text = QS("date") == "" ? DateProvider.ToString("MM/dd/yyyy") : QS("date");
                ddlUsers.SelectedValue = QS("user");
            }
        }
        private void BindUsers()
        {
            UserApplication userApp = new UserApplication();
            SearchUsersRequest requestUser = new SearchUsersRequest(
            SearchUsersType.All, false, " FirstName ", " ASC ");
            requestUser.IsSunnet = true;
            SearchUserResponse responseuser = userApp.SearchUsers(requestUser);
            ddlUsers.Items.Add(new ListItem("All", "0"));
            switch (UserInfo.Role)
            {
                case RolesEnum.ADMIN:
                case RolesEnum.Sales:
                case RolesEnum.PM:
                    foreach (UsersEntity user in responseuser.ResultList)
                    {
                        ddlUsers.Items.Add(new ListItem(string.Format("{0} {1}", user.FirstName, user.LastName)
                            , user.ID.ToString()));
                    }
                    break;
                case RolesEnum.Leader:
                case RolesEnum.QA:
                case RolesEnum.DEV:
                    foreach (UsersEntity user in responseuser.ResultList.FindAll(r => r.Role == RolesEnum.Leader
                        || r.Role == RolesEnum.QA || r.Role == RolesEnum.DEV))
                    {
                        ddlUsers.Items.Add(new ListItem(string.Format("{0} {1}", user.FirstName, user.LastName)
                            , user.ID.ToString()));
                    }
                    break;
            }
        }

        private void BindRepeater(string date)
        {
            int recordCount;
            DateTime startDate;
            DateTime endDate;

            if (!DateTime.TryParse(date, out startDate))
            {
                int weekOfDay = (int)DateProvider.DayOfWeek;
                weekOfDay = weekOfDay == 0 ? 7 : weekOfDay;
                startDate = DateProvider.AddDays(1 - weekOfDay);
            }
            else
            {
                int weekOfDay = (int)startDate.DayOfWeek;
                weekOfDay = weekOfDay == 0 ? 7 : weekOfDay;
                startDate = startDate.AddDays(1 - weekOfDay);
            }
            endDate = startDate.AddDays(6);
            lblStartDate.Text = startDate.ToString("MM/dd/yyyy");
            lblEndDate.Text = endDate.ToString("MM/dd/yyyy");

            List<WeekPlanEntity> list =
                new App.WeekPlanApplication().GetList(QS("user", 0), startDate, endDate, UserInfo.Role, CurrentPageIndex, int.MaxValue, out recordCount);


            DateTime.Parse(DateProvider.ToShortDateString());
            if (DateTime.Parse(DateProvider.ToShortDateString()) >= DateTime.Parse(startDate.ToShortDateString())
                && DateTime.Parse(DateProvider.ToShortDateString()) <= DateTime.Parse(endDate.ToShortDateString()))
            {
                weekDay = (int)DateProvider.DayOfWeek;
            }
            StringBuilder headHtml = new StringBuilder();
            headHtml.AppendFormat("<td >&nbsp;</td>");
            headHtml.AppendFormat("<td width='10%'><strong>Total Remaining &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Hour</strong></td>");
            headHtml.AppendFormat("<td width='13%' class='{1}'><strong>Monday</strong> {0}</td>", startDate.ToString("MM/dd"), weekDay == 1 ? "wpitemtoday" : "");
            headHtml.AppendFormat("<td width='13%' class='{1}'><strong>Tuesday</strong> {0}</td>", startDate.AddDays(1).ToString("MM/dd"), weekDay == 2 ? "wpitemtoday" : "");
            headHtml.AppendFormat("<td width='13%' class='{1}'><strong>Wednesday</strong> {0}</td>", startDate.AddDays(2).ToString("MM/dd"), weekDay == 3 ? "wpitemtoday" : "");
            headHtml.AppendFormat("<td width='13%' class='{1}'><strong>Thursday</strong> {0}</td>", startDate.AddDays(3).ToString("MM/dd"), weekDay == 4 ? "wpitemtoday" : "");
            headHtml.AppendFormat("<td width='13%' class='{1}'><strong>Friday</strong> {0}</td>", startDate.AddDays(4).ToString("MM/dd"), weekDay == 5 ? "wpitemtoday" : "");
            headHtml.AppendFormat("<td width='13%' class='{1}'><strong>Saturday</strong> {0}</td>", startDate.AddDays(5).ToString("MM/dd"), weekDay == 6 ? "wpitemtoday" : "");
            headHtml.AppendFormat("<td width='13%' class='{1}'><strong>Sunday</strong> {0}</td>", startDate.AddDays(6).ToString("MM/dd"), weekDay == 0 ? "wpitemtoday" : "");
            dvHead.InnerHtml = headHtml.ToString();
            if (recordCount == 0)
            {
                trNoRecords.Visible = true;
                rptList.DataSource = null;
                rptList.DataBind();
            }
            else
            {
                trNoRecords.Visible = false;
                rptList.DataSource = list.OrderBy(e => e.FirstName);
                rptList.DataBind();
            }
        }

        protected void ibtnFrontWeek_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            DateTime frontWeekDate = DateTime.Parse(lblStartDate.Text).AddDays(-7);
            BindRepeater(frontWeekDate.ToShortDateString());
        }

        protected void ibtnNextWeek_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            DateTime nextWeekDate = DateTime.Parse(lblEndDate.Text).AddDays(7);
            BindRepeater(nextWeekDate.ToShortDateString());
        }
    }
}