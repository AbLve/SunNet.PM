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

namespace SunNet.PMNew.Web.Sunnet.Reports
{
    public partial class TimeSheet : BaseWebsitePage
    {
        TimeSheetApplication tsApp;
        WorkRequestApplication wrApp = new WorkRequestApplication();
        protected DateTime StartDate
        {
            get
            {
                if (string.IsNullOrEmpty(txtStartDate.Text))
                {
                    return new DateTime(1753, 1, 1);
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParse(txtStartDate.Text, out dt))
                    {
                        return dt;
                    }
                    return new DateTime(1753, 1, 1);
                }
            }

        }
        protected DateTime EndDate
        {
            get
            {
                if (string.IsNullOrEmpty(txtEndDate.Text))
                {
                    return new DateTime(2200, 1, 1);
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParse(txtEndDate.Text, out dt))
                    {
                        return dt;
                    }
                    return new DateTime(2200, 1, 1);
                }
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            tsApp = new TimeSheetApplication();
            if (!IsPostBack)
            {
                InitControls();
                InitControl();
            }
        }
        private void InitControls()
        {
            DateTime date = DateTime.Now.AddMonths(-1);
            //txtStartDate.Text = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
            //txtEndDate.Text = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)).ToString("MM/dd/yyyy");

            ProjectApplication projApp = new ProjectApplication();
            List<ProjectDetailDTO> list = projApp.GetAllProjects();
            list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID", "All", "0");

            ddlTickets.Items.Add(new ListItem("All", "0"));

            UserApplication userApp = new UserApplication();
            SearchUsersRequest requestUser = new SearchUsersRequest(
                SearchUsersType.All, false, " FirstName ", " ASC ");
            requestUser.IsSunnet = true;
            SearchUserResponse responseuser = userApp.SearchUsers(requestUser);
            ddlUsers.Items.Add(new ListItem("All", "0"));
            foreach (UsersEntity user in responseuser.ResultList)
            {
                ddlUsers.Items.Add(new ListItem(string.Format("{0} {1}", user.FirstName, user.LastName)
                    , user.ID.ToString()));
            }
        }
        private void InitControl()
        {
            SearchTimeSheetsRequest request = new SearchTimeSheetsRequest(SearchType.QueryReport,
                true, hidOrderBy.Value, hidOrderDirection.Value);
            request.Keywords = txtKeyword.Text;
            request.CurrentPage = anpTimesheet.CurrentPageIndex;
            request.PageCount = anpTimesheet.PageSize;
            request.ProjectID = int.Parse(ddlProject.SelectedValue);
            request.TicketID = int.Parse(ddlTickets.SelectedValue);
            request.UserID = int.Parse(ddlUsers.SelectedValue);

            request.StartDate = StartDate;
            request.EndDate = EndDate;

            int workRequestId = QS("WID", 0);
            if (workRequestId > 0)
                request.WID = workRequestId;

            SearchTimeSheetsResponse response = tsApp.QueryTimesheet(request);

            //int workRequestId = QS("WID", 0);
            //if (workRequestId > 0)
            //{
            //    List<int> lstTicketIDs = new List<int>();
            //    List<string> lstStr = wrApp.GetAllRelationStringByWorkRequest(workRequestId);
            //    foreach (string str in lstStr)
            //    {
            //        lstTicketIDs.Add(Convert.ToInt32(str));
            //    }
            //    response.TimeSheetsList = response.TimeSheetsList.FindAll(x =>lstTicketIDs.Contains(x.TicketID));
            //}
            rptTimesheet.DataSource = response.TimeSheetsList;
            rptTimesheet.DataBind();

            anpTimesheet.RecordCount = response.ResultCount;
        }
        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            anpTimesheet.CurrentPageIndex = 1;
            InitControl();
        }

        protected void anpTimesheet_PageChanged(object sender, EventArgs e)
        {
            InitControl();
        }

        protected void iBtnDownload_Click(object sender, ImageClickEventArgs e)
        {
            SearchTimeSheetsRequest request = new SearchTimeSheetsRequest(SearchType.QueryReport,
                false, hidOrderBy.Value, hidOrderDirection.Value);
            request.Keywords = txtKeyword.Text;
            request.CurrentPage = anpTimesheet.CurrentPageIndex;
            request.PageCount = anpTimesheet.PageSize;
            request.ProjectID = int.Parse(ddlProject.SelectedValue);
            request.TicketID = int.Parse(ddlTickets.SelectedValue);
            request.UserID = int.Parse(ddlUsers.SelectedValue);

            request.StartDate = StartDate;
            request.EndDate = EndDate;

            if (ddlProject.SelectedIndex != 0)
            {
                request.OrderExpression = "ticketcode, sheetdate";
                request.OrderDirection = "asc";
            }
            SearchTimeSheetsResponse response = tsApp.QueryTimesheet(request);
            UserApplication userApp = new UserApplication();

            UsersEntity model = userApp.GetUser(int.Parse(ddlUsers.SelectedValue));
            ExcelReport report = new ExcelReport();
            report.Generate(response.TimeSheetsList, model, ddlProject.SelectedItem.Text, DateTime.Now);
        }

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlTickets.Items.Clear();
            if (ddlProject.SelectedValue == "0")
            {
                ddlTickets.Items.Add(new ListItem("All", "0"));
            }
            else
            {
                TicketsApplication tickApp = new TicketsApplication();
                SearchTicketsRequest request = new SearchTicketsRequest(SearchTicketsType.TicketsListByPID, " TicketTitle Asc", false);
                request.ProjectID = int.Parse(ddlProject.SelectedValue);
                SearchTicketsResponse response = tickApp.SearchTickets(request);
                response.ResultList.BindDropdown<ExpandTicketsEntity>(ddlTickets, "Title", "ID", "All", "0");
            }
        }
    }
}
