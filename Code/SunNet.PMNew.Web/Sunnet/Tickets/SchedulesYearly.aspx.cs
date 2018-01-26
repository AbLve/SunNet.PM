using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Web.Codes.Schedule;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class SchedulesYearly : BaseWebsitePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitStatus();
                InitUser();
                InitYear();
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

        private void InitControls()
        {
            YearEntity year = new YearEntity(
                int.Parse(ddlYears.SelectedValue),
                txtKeyword.Text.NoHTML(),
                int.Parse(ddlUsers.SelectedValue),
                int.Parse(ddlStatus.SelectedValue)
                );
            rptMonths.DataSource = year.Months;
            rptMonths.DataBind();
        }
        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            InitControls();
        }
    }
}
