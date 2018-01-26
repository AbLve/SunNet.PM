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
using System.Text;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class WeekPlanList : BaseWebsitePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUsers();
                BindRepeater();
            }
        }

        private void BindUsers()
        {
            UserApplication userApp = new UserApplication();
            SearchUsersRequest requestUser = new SearchUsersRequest(
            SearchUsersType.All, false, " FirstName ", " ASC ");
            requestUser.IsSunnet = true;
            requestUser.IsActive = true;
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

        protected void aspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindRepeater();
        }

        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            aspNetPager1.CurrentPageIndex = 1;
            BindRepeater();
        }

        private void BindRepeater()
        {
            int recordCount;
            DateTime startDate;
            if (!DateTime.TryParse(txtStartDate.Text, out startDate)) //如果没有填时间，就当前时间减一个星期
                startDate = DateTime.Now.AddDays(-7);

            DateTime endDate;
            if (!DateTime.TryParse(txtEndDate.Text, out endDate))
                endDate = DateTime.Now.AddDays(7);
            List<WeekPlanEntity> list =
                new App.WeekPlanApplication().GetList(int.Parse(ddlUsers.SelectedValue), startDate, endDate, UserInfo.Role, aspNetPager1.CurrentPageIndex, aspNetPager1.PageSize, out recordCount);

            if (recordCount == 0)
            {
                lblNoResult.Visible = true;
                lblNoResult.InnerHtml = "<span style='color: Red;'>&nbsp; No records</span>";
                rptList.DataSource = null;
                rptList.DataBind();
            }
            else
            {
                lblNoResult.Visible = false;
                rptList.DataSource = list;
                rptList.DataBind();
            }
            aspNetPager1.RecordCount = recordCount;

        }

        protected string ShowEdit(object id, object userId)
        {
            if (UserInfo.Role == RolesEnum.ADMIN || UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.Sales)
                return string.Format("<a href=\"javascript:void(0);\" onclick=\"OpenPlanDetail('{0}')\"><img border=\"0\" title=\"Edit\" src=\"/icons/05.gif\"></a>"
                    , id);
            else if ((int)userId == UserInfo.UserID)
                return string.Format("<a href=\"javascript:void(0);\" onclick=\"OpenPlanDetail('{0}')\"><img border=\"0\" title=\"Edit\" src=\"/icons/05.gif\"></a>"
                    , id);
            else
                return "&nbsp;";
        }
    }
}
