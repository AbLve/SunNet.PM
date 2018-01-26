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
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Report
{
    public partial class TicketTimesheet : BasePage
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
        protected void Page_Load(object sender, EventArgs e)
        {
            ((Pop)this.Master).Width = 800;
            tsApp = new TimeSheetApplication();
            if (!IsPostBack)
            {
                string headStr = "";
                ProjectApplication projApp = new ProjectApplication();
                TicketsApplication ticketApp = new TicketsApplication();
                ProjectsEntity project = projApp.Get(QS("project", 0));
                TicketsEntity ticket = ticketApp.GetTickets(QS("ticket", 0));
                if (project != null )
                    headStr += project.Title;
                if(ticket != null)
                    headStr += " - "+ticket.Title;
                litHead.Text = headStr;
                InitControl();
            }
        }

        private void InitControl()
        {
            SearchTimeSheetsRequest request = new SearchTimeSheetsRequest(SearchType.QueryReport,true, OrderBy, OrderDirection);
            request.CurrentPage = CurrentPageIndex;
            request.PageCount = ReportPage.PageSize;
            request.ProjectID = QS("project", 0);
            request.TicketID = QS("ticket", 0); ;
            request.StartDate = QS("startdate", DateTime.MinValue);
            request.EndDate = QS("enddate", DateTime.MinValue);
            request.UserID = QS("user", 0);

            SearchTimeSheetsResponse response = tsApp.QueryTimesheet(request);
            rptReportList.DataSource = response.TimeSheetsList;
            rptReportList.DataBind();
            ReportPage.RecordCount = response.ResultCount; 
        }
        
    }
}