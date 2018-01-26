using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Entity.SchedulesModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.OA.Calendar
{
    public partial class Index : BasePage
    {
        public List<SchedulesEntity> SchedulesList { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDrop();
                DateTime startDate = DateTime.Parse(string.Format("{0}/1/{1}", ddlMonths.SelectedValue, ddlYears.SelectedValue));
                SchedulesList = new App.SchedulesApplication().GetSchedules(startDate
                    , GetCurrentMonthEndDayOfSchudule().AddDays(1), UserInfo.UserID);
                BindSchedules();
            }
        }

        private DateTime GetCurrentMonthEndDayOfSchudule()
        {
            return ScheduleTimeHelpers.GetDayList(DateTime.Parse(string.Format("{0}/1/{1}"
            , ddlMonths.SelectedValue, ddlYears.SelectedValue)), "").Last().Day;
        }

        private void BindDrop()
        {
            ddlYears.SelectedValue = DateTime.Now.Year.ToString();
            ddlMonths.SelectedValue = DateTime.Now.Month.ToString();
            List<UsersEntity> allActiviteUserList = new App.UserApplication().GetActiveUserList();
            if (UserInfo.Office == "US")
                ddlUsers.DataSource = allActiviteUserList.FindAll(r => r.Role != RolesEnum.CLIENT).OrderBy(r => r.FirstName);
            else
                ddlUsers.DataSource = allActiviteUserList.FindAll(r => r.Office == "CN" && r.Role != RolesEnum.CLIENT).OrderBy(r => r.FirstName);
            ddlUsers.DataTextField = "FirstAndLastName";
            ddlUsers.DataValueField = "UserID";
            ddlUsers.DataBind();
            ListItem li = ddlUsers.Items.FindByValue(UserInfo.UserID.ToString());
            if (li != null)
            {
                li.Selected = true;
            }
        }

        private void BindSchedules()
        {
            DateTime startDate = DateTime.Parse(string.Format("{0}/1/{1}", ddlMonths.SelectedValue, ddlYears.SelectedValue));
            SchedulesList = new App.SchedulesApplication().GetSchedules(startDate
                , GetCurrentMonthEndDayOfSchudule().AddDays(1), int.Parse(ddlUsers.SelectedValue));
            string url = "<div>{0}<img src=\"/images/icons/add1.png\" style=\"cursor:pointer;float:right\" title=\"Add Schedule\" href=\'Add.aspx?Date=" + ddlMonths.SelectedValue + "/{0}/" + ddlYears.SelectedValue
                + "' data-target=\"#modalsmall\" data-toggle=\"modal\" ></div>";
            rptDays.DataSource = ScheduleTimeHelpers.GetDayList(DateTime.Parse(string.Format("{0}/1/{1}", ddlMonths.SelectedValue, ddlYears.SelectedValue)), url);
            rptDays.DataBind();
        }

        protected void rptDays_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {                                                                                                                                                                                                                                                      
            DayEntity entity = e.Item.DataItem as DayEntity;
            Repeater rpt = e.Item.FindControl("rptSchedules") as Repeater;
            rpt.DataSource = SchedulesList.FindAll(r => r.PlanDate.Date == entity.Day);
            rpt.DataBind();
        }

        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindSchedules();
        }
    }
}