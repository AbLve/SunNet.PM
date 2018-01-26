using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Excel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Report
{
    public partial class Comparison : BasePage
    {
        TimeSheetApplication tsApp;
        
        protected override string DefaultOrderBy
        {
            get
            {
                return "ProjectTitle";
            }
        }
        protected override string DefaultDirection
        {
            get
            {
                return "ASC";
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
                if (QS("user") != "")
                    ddlUsers.SelectedValue = QS("user");
                InitControl();
            }
        }
        private void InitControls()
        {
            DateTime date = DateTime.Now.AddMonths(-1);

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
                true, OrderBy, OrderDirection);
            request.Keywords = txtKeyword.Text;
            request.CurrentPage = CurrentPageIndex;
            request.PageCount = ReportPage.PageSize;
            request.ProjectID = int.Parse(ddlProject.SelectedValue);
            request.TicketID = int.Parse(ddlTickets.SelectedValue);
            request.UserID = int.Parse(ddlUsers.SelectedValue);

            request.StartDate = StartDate;
            request.EndDate = EndDate; 
            int totalRows = 0;
             var response = tsApp.ReportConsumingComparison(request, out totalRows);
            rptReportList.DataSource = response;
            rptReportList.DataBind();

            ReportPage.RecordCount = totalRows;
        }
       
        
        protected void iBtnDownload_Click(object sender, EventArgs e)
        {
            SearchTimeSheetsRequest request = new SearchTimeSheetsRequest(SearchType.QueryReport,false, OrderBy, OrderDirection);
            request.Keywords = txtKeyword.Text;
            request.CurrentPage = CurrentPageIndex;
            request.PageCount = ReportPage.PageSize;
            request.ProjectID = int.Parse(ddlProject.SelectedValue);
            request.TicketID = int.Parse(ddlTickets.SelectedValue);
            request.UserID = int.Parse(ddlUsers.SelectedValue);
            request.StartDate = StartDate;
            request.EndDate = EndDate;
            
            if (ddlProject.SelectedIndex != 0)
            {
                request.OrderExpression = "projectTitle";
                request.OrderDirection = "asc";
            }
            List<TimeSheetTicket> response = tsApp.ComparisonExport(request);

            UserApplication userApp = new UserApplication();
            UsersEntity model = userApp.GetUser(int.Parse(ddlUsers.SelectedValue));
            ExcelReport report = new ExcelReport();
            report.ComparisonExport(response);
        } 
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ddlTickets.Items.Clear();
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