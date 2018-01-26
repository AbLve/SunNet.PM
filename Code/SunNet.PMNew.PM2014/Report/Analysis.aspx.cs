using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Report
{
    public partial class Analysis : BasePage
    {
        TimeSheetApplication tsApp;
        TicketsApplication tApp = new TicketsApplication();
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
                }

                txtStartDate.Text = QS("startdate");
                txtEndDate.Text = QS("enddate");

                InitControl();
            }
        }
        private void InitControls()
        {
            DateTime date = DateTime.Now.AddMonths(-1);
            ProjectApplication projApp = new ProjectApplication();
            List<ProjectDetailDTO> list = projApp.GetAllProjects();
            list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID", "All", "0"); 
            RolesEnum.DEV.ToSelectList().ToList().BindDropdown(ddlSource, "Text", "Value", DefaulAllText, "0", QS("source"));
        }
        private void InitControl()
        {
            SearchTimeSheetsRequest request = new SearchTimeSheetsRequest(SearchType.QueryReport,true, OrderBy, OrderDirection);
            request.Keywords = txtKeyword.Text;
            request.CurrentPage = CurrentPageIndex;
            request.PageCount = ReportPage.PageSize;
            request.ProjectID = int.Parse(ddlProject.SelectedValue);
            request.StartDate = StartDate;
            request.EndDate = EndDate;
            request.Source = Convert.ToInt32(ddlSource.SelectedValue);
            int totalRows = 0;
            var response = tsApp.ReportComparisonAnalysis(request, out totalRows);
            rptReportList.DataSource = response;
            rptReportList.DataBind();
            ReportPage.RecordCount = totalRows;
        }

        protected void iBtnDownload_Click(object sender, EventArgs e)
        {
            SearchTicketsRequest request = new SearchTicketsRequest(SearchTicketsType.TicketsForReport, OrderBy, true);
            request.Keyword = txtKeyword.Text;
            request.CurrentPage = 1;
            request.PageCount = int.MaxValue;
            request.ProjectID = int.Parse(ddlProject.SelectedValue);
            request.TicketType ="-1";
            request.SearchTicketID = false;
            request.StartDate = StartDate;
            request.EndDate = EndDate;
         
            request.Source = Convert.ToInt32(ddlSource.SelectedValue);
            int rowCount = 0;
            DataTable table = tApp.SearchReortTickets(request, out rowCount);
            ExcelReport report = new ExcelReport();
            report.AnalysisExport(table,"Ticket Analysis");
        }
        
    }
}