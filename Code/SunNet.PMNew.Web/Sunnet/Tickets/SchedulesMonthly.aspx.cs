using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Web.Codes.Schedule;
using SunNet.PMNew.Framework.Utils.Helpers;
namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class SchedulesMonthly : BaseWebsitePage
    {
        YearEntity year;
        private int MonthIndex
        {
            get
            {
                if (ddlMonths.Items.Count > 0)
                {
                    int month = int.Parse(ddlMonths.SelectedValue);
                    return month - 1;
                }
                return DateTime.Now.Month - 1;
            }
        }
        protected string GetClassName(object datetime, string todayClass, string disableClass, string weekendClass, string contentClass)
        {
            DateTime dt = (DateTime)datetime;
            if (dt.Date == DateTime.Now.Date)
            {
                return todayClass;
            }
            int year = int.Parse(ddlYears.SelectedValue);
            int month = int.Parse(ddlMonths.SelectedValue);
            int days = DateTime.DaysInMonth(year, month);
            if (dt < new DateTime(year, month, 1)
                || dt > new DateTime(year, month, days))
            {
                return disableClass;
            }
            if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
            {
                return weekendClass;
            }
            return contentClass;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitStatus();
                InitUser();
                InitYear();
                InitMonth();
                InitControls();
            }
        }
        private void InitStatus()
        {
            List<ListItem> ticketStatus = new List<ListItem>();
            ticketStatus.Add(new ListItem("All", "-1"));
            foreach (TicketsState type in TicketsStateHelper.ScheduleStates)
            {
                ticketStatus.Add(new ListItem(type.ToString().Replace('_', ' '), ((int)type).ToString()));
            }
            ddlStatus.DataSource = ticketStatus;
            ddlStatus.DataTextField = "text";
            ddlStatus.DataValueField = "value";
            ddlStatus.DataBind();
        }
        private void InitDEVSQAUser()
        {
            UserApplication userApp = new UserApplication();
            SearchUsersRequest requestUser = new SearchUsersRequest(
                SearchUsersType.All, false, " FirstName ", " ASC ");
            requestUser.IsSunnet = true;
            SearchUserResponse responseuser = userApp.SearchUsers(requestUser);
            ddlUsers.Items.Add(new ListItem("Please Select", "-1"));
            foreach (UsersEntity user in responseuser.ResultList)
            {
                if (user.Role == RolesEnum.DEV || user.Role == RolesEnum.QA)
                {
                    ddlUsers.Items.Add(new ListItem(string.Format("{0} {1}", user.FirstName, user.LastName)
                        , user.ID.ToString()));
                }
            }
        }
        private void InitCurrentUser()
        {
            ddlUsers.Items.Add(
                           new ListItem(
                               string.Format("{0} {1}",
                                               UserInfo.FirstName,
                                               UserInfo.LastName),
                               UserInfo.ID.ToString()));
            ddlUsers.Items.FindByValue(UserInfo.ID.ToString()).Selected = true;
        }
        private void InitUser()
        {
            switch (UserInfo.Role)
            {
                case RolesEnum.PM:
                    InitDEVSQAUser();
                    InitCurrentUser();
                    break;
                case RolesEnum.ADMIN:
                    InitDEVSQAUser();
                    break;
                default:
                    InitCurrentUser();
                    break;
            }
        }
        private void InitYear()
        {
            for (int i = 2010; i < 2021; i++)
            {
                ddlYears.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            ddlYears.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
        }
        private void InitMonth()
        {
            ddlMonths.Items.Clear();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            //if (int.Parse(ddlYears.SelectedValue) != year)
            month = 12;
            for (int i = 1; i <= month; i++)
            {
                DateTime dt = new DateTime(year, i, 1);
                ddlMonths.Items.Add(new ListItem(dt.ToString("MMMM"), i.ToString()));
            }
            ddlMonths.Items.FindByValue(DateTime.Now.Month.ToString()).Selected = true;
        }
        private void InitControls()
        {
            year = new YearEntity(
                int.Parse(ddlYears.SelectedValue),
                txtKeyword.Text.NoHTML(),
                int.Parse(ddlUsers.SelectedValue),
                int.Parse(ddlStatus.SelectedValue)
                );
            rptDays.DataSource = year.Months[MonthIndex].Days;
            rptDays.DataBind();
        }

        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            InitControls();
        }

        protected void rptDays_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptTickets = e.Item.FindControl("rptTickets") as Repeater;
            List<ScheduleTicketEntity> list = year.Months[MonthIndex].Days[e.Item.ItemIndex].Tickets;
            if (list != null && list.Count > 0)
            {
                rptTickets.Visible = true;
                rptTickets.DataSource = list;
                rptTickets.DataBind();
            }
            else
            {
                rptTickets.Visible = false;
            }
        }

        protected void ddlYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitMonth();
        }

        protected void iBtnLastMonth_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlMonths.SelectedIndex == 0)
            {
                if (ddlYears.SelectedIndex == 0)
                {
                    ListItem firstYear = ddlYears.Items[0];
                    string year = (int.Parse(firstYear.Value) - 1).ToString();
                    ddlYears.Items.Insert(0, new ListItem(
                                            year,
                                            year));
                    ddlYears.SelectedIndex = 0;
                }
                else
                {
                    ddlYears.SelectedIndex = ddlYears.SelectedIndex - 1;
                }
                ddlYears_SelectedIndexChanged(null, null);
                ddlMonths.SelectedIndex = ddlMonths.Items.Count - 1;
            }
            else
            {
                ddlMonths.SelectedIndex = ddlMonths.SelectedIndex - 1;
            }
            InitControls();
        }

        protected void iBtnNextMonth_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlMonths.SelectedIndex == ddlMonths.Items.Count - 1)
            {
                if (ddlYears.SelectedIndex == ddlYears.Items.Count - 1)
                {
                    ListItem firstYear = ddlYears.Items[0];
                    string year = (int.Parse(firstYear.Value) + 1).ToString();
                    ddlYears.Items.Insert(ddlYears.Items.Count, new ListItem(
                                            year,
                                            year));
                    ddlYears.SelectedIndex = ddlYears.Items.Count;
                }
                else
                {
                    ddlYears.SelectedIndex = ddlYears.SelectedIndex + 1;
                }
                ddlYears_SelectedIndexChanged(null, null);
                ddlMonths.SelectedIndex = 0;
            }
            else
            {
                ddlMonths.SelectedIndex = ddlMonths.SelectedIndex + 1;
            }
            InitControls();
        }
    }
}
