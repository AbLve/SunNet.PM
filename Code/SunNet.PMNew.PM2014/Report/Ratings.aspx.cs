using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Admin.Projects;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Report
{
    public partial class Ratings : BasePage
    {
        TicketsApplication tApp;
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
            tApp = new TicketsApplication();
            if (!IsPostBack)
            {
                InitControls();
                InitControlSatus();
                InitControl();
            }
        }

        private void InitControlSatus()
        {
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
            if (QS("type") != "")
            {
                for (int i = 0; i < ddlType.Items.Count; i++)
                {
                    if (ddlType.Items[i].Text.Trim().ToLower() == QS("type").ToLower())
                    {
                        ddlType.SelectedIndex = i;
                        break;
                    }
                }
            }

        }

        private void InitControls()
        {
            ProjectApplication projApp = new ProjectApplication();
            List<ProjectDetailDTO> list = projApp.GetAllProjects();
            list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID", "All", "0");
            ddlTickets.Items.Add(new ListItem("All", "0"));
        }
        private void InitControl()
        {
            int rowCount = 0;
            System.Data.DataTable list = GetList(true,out rowCount);
            rptReportList.DataSource = list;
            rptReportList.DataBind();
            ReportPage.RecordCount = rowCount;
        }

        private System.Data.DataTable GetList(bool ispageModel,out int rowCount)
        {
            rowCount = 0;
            SearchTicketsRequest request = new SearchTicketsRequest(SearchTicketsType.TicketsForReport, OrderBy, true);
            request.Keyword = txtKeyword.Text;
            if (ispageModel)
            {
                request.CurrentPage = CurrentPageIndex;
                request.PageCount = ReportPage.PageSize;
            }
            else
            {
                request.CurrentPage = 1;
                request.PageCount = int.MaxValue;
            }
            request.ProjectID = int.Parse(ddlProject.SelectedValue);
            request.TicketIDS = ddlTickets.SelectedValue.ToString();
            request.SearchTicketID = true;
            request.StartDate = StartDate;
            request.EndDate = EndDate;
            request.TicketType = ddlType.SelectedItem.Value;
            request.OrderBy = OrderBy;
            request.OrderDirection = OrderDirection;
            System.Data.DataTable response = tApp.ReortTicketRating(request, out rowCount);
            return response;
        }

        protected void iBtnDownload_Click(object sender, EventArgs e)
        {
            int rowCount = 0;
            System.Data.DataTable table = GetList(false, out rowCount);
            if (table.Rows.Count == 0)
                return;
            ExcelReport report = new ExcelReport();
            string projectStr = "All";
            string dateStr = "";
            if (ddlProject.SelectedValue != "0")
            projectStr = ddlProject.SelectedItem.Text;
            if(txtStartDate.Text.Trim() != "")
            dateStr = txtStartDate.Text + " ~ " + txtEndDate.Text;
            
            report.RatingExport(table, projectStr,dateStr);
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