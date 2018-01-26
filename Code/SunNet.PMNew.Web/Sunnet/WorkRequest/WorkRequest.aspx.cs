using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.WorkRequestModel;

namespace SunNet.PMNew.Web.Sunnet.WorkRequest
{
    public partial class WorkRequest : BaseWebsitePage
    {
        WorkRequestApplication wrApp;

        protected void Page_Load(object sender, EventArgs e)
        {
            wrApp = new WorkRequestApplication();
            if (!IsPostBack)
            {
                InitProject();
                InitControl();
            }
        }

        private void InitControl()
        {
            int userId = 0;
            if (UserInfo.Role == RolesEnum.CLIENT)
                userId = UserInfo.UserID;

            SearchWorkRequestRequest request = wrApp.GetSearchWorkRequests(txtKeyword.Text.Trim(), Convert.ToInt32(ddlProject.SelectedValue),
                                    Convert.ToInt32(ddlStatus.SelectedValue), Convert.ToInt32(ddlPayment.SelectedValue), userId
                                    , hidOrderBy.Value, hidOrderDirection.Value, anpWorkRequest.PageSize, anpWorkRequest.CurrentPageIndex);
            rptWR.DataSource = request.ResultList;
            rptWR.DataBind();
            anpWorkRequest.RecordCount = request.ResultCount;
        }

        private void InitProject()
        {
            ProjectApplication projApp = new ProjectApplication();
            List<ProjectDetailDTO> list = projApp.GetUserProjects(UserInfo);
            list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID", "All", "0");
        }

        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            anpWorkRequest.CurrentPageIndex = 1;
            InitControl();
        }

        protected void anpWorkRequest_PageChanged(object sender, EventArgs e)
        {
            InitControl();
        }


        public string ShowPriorityImgByDevDate(object date)
        {
            string imgString = "";
            if (date == null)
            {
                return imgString;
            }
            DateTime Now = DateTime.Now.Date;

            TimeSpan Diff = Convert.ToDateTime(date).Subtract(Now);

            int DiffDate = Diff.Days;

            if (DiffDate <= 3 && DiffDate > 0)
            {
                imgString = "<img src='/icons/02.gif' title='Due Day is 3 days before today'  />";
            }
            else if (DiffDate == 0)
            {
                imgString = "<img src='/icons/03.gif' title ='Due Date is today' />";
            }
            else if (Convert.ToDateTime(date) > UtilFactory.Helpers.CommonHelper.GetDefaultMinDate() && DiffDate < 0)
            {
                imgString = "<img src='/icons/01.gif' title='Passed Due Date' />";
            }
            return imgString;
        }


        protected void rptWR_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal labelID = e.Item.FindControl("lblWID") as Literal;
                Literal labelHour = e.Item.FindControl("lblHours") as Literal;
                string link = @"<a href='/sunnet/Reports/TimeSheet.aspx?WID=" + labelID.Text + "'>"
                    + wrApp.GetWorkRequestHours(Convert.ToInt32(labelID.Text)) + "</a>";
                labelHour.Text = link;

            }
        }
    }
}
