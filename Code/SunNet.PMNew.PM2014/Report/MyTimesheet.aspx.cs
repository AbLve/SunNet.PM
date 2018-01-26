using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Report
{
    public partial class MyTimesheet : BasePage
    {
        TimeSheetApplication tsApp;
        protected override string DefaultOrderBy
        {
            get
            {
                return "SheetDate";
            }
        }
        protected override string DefaultDirection
        {
            get
            {
                return "DESC";
            }
        }
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
                txtKeyword.Text = QS("keyword");
                if (QS("project") != "")
                {
                    ddlProject.SelectedValue = QS("project");
                    BindTickets();
                }
                if (QS("ticket") != "")
                    ddlTickets.SelectedValue = QS("ticket");
                txtStartDate.Text = QS("startdate");
                txtEndDate.Text = QS("enddate");
                InitControl();
            }
        }
        private void InitControls()
        {
            DateTime date = DateTime.Now.AddMonths(-1);
            //txtStartDate.Text = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
            //txtEndDate.Text = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)).ToString("MM/dd/yyyy");

            ProjectApplication projApp = new ProjectApplication();
            List<ProjectDetailDTO> list = projApp.GetUserProjects(UserInfo);
            list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID", "All", "0");

            ddlTickets.Items.Add(new ListItem("All", "0"));
        }
        private void InitControl()
        {
            SearchTimeSheetsRequest request = new SearchTimeSheetsRequest(SearchType.QueryReport,
                true, OrderBy, OrderDirection);
            request.Keywords = txtKeyword.Text;
            request.CurrentPage = CurrentPageIndex;
            request.PageCount = ReportPage.PageSize;
            request.ProjectID = int.Parse(ddlProject.SelectedValue);
            request.TicketID = int.Parse(ddlTickets.SelectedValue);
            request.UserID = UserInfo.ID;
            request.StartDate = StartDate;
            request.EndDate = EndDate;
            SearchTimeSheetsResponse response = tsApp.QueryTimesheet(request);
            rptReportList.DataSource = response.TimeSheetsList;
            rptReportList.DataBind();

            ReportPage.RecordCount = response.ResultCount;
        }

        protected void iBtnDownload_Click(object sender, EventArgs e)
        {
            SearchTimeSheetsRequest request = new SearchTimeSheetsRequest(SearchType.QueryReport,
                false, OrderBy, OrderDirection);
            request.Keywords = txtKeyword.Text;
            request.CurrentPage = CurrentPageIndex;
            request.PageCount = ReportPage.PageSize;
            request.ProjectID = int.Parse(ddlProject.SelectedValue);
            request.TicketID = int.Parse(ddlTickets.SelectedValue);
            request.UserID = UserInfo.ID;
            request.StartDate = StartDate;
            request.EndDate = EndDate;
            SearchTimeSheetsResponse response = tsApp.QueryTimesheet(request);

            ExcelReport report = new ExcelReport();
            report.Generate(response.TimeSheetsList, null, UserInfo, ddlProject.SelectedItem.Text, DateTime.Now);
        }

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTickets();
        }
        private void BindTickets()
        {
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