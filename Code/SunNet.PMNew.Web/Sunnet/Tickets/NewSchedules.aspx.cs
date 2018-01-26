using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.SchedulesModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class NewSchedules : BaseWebsitePage
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
            string url = "<a href=\"#\" style=\"display:inline-block;width:99%;\" onclick=\"OpenAddModuleDialog('{1}');\">{0}</a>";
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
