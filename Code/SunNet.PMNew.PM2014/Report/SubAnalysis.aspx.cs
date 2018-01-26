using System;
using System.Collections.Generic;
using System.Linq;
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
using SunNet.PMNew.Framework.Extensions;
namespace SunNet.PMNew.PM2014.Report
{
    public partial class SubAnalysis : BasePage
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
                return "asc";
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
            if (QS("user") != "")
                ddlUsers.SelectedValue = QS("user");
            if (QS("star") != "")
            {
                tdButtons.ColSpan = 1;
                tdStar.Visible = true;
                tdStarText.Visible = true;
                ddlStars.SelectedValue = QS("star");
            }
            else
            {
                tdButtons.ColSpan = 3;
                tdStar.Visible = false;
                tdStarText.Visible = false;

            }
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

            RolesEnum.DEV.ToSelectList().ToList().BindDropdown(ddlSource, "Text", "Value", DefaulAllText, "0", QS("source"));

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
            SearchTicketsRequest request = new SearchTicketsRequest(SearchTicketsType.TicketsForReport, OrderBy, true);
            request.Keyword = txtKeyword.Text;
            request.CurrentPage = CurrentPageIndex;
            request.PageCount = ReportPage.PageSize;
            request.ProjectID = int.Parse(ddlProject.SelectedValue);
            request.TicketIDS = ddlTickets.SelectedValue.ToString();
            request.UserID = int.Parse(ddlUsers.SelectedValue);
            request.SearchTicketID = true;
            request.StartDate = StartDate;
            request.EndDate = EndDate;
            request.TicketType = ddlType.SelectedItem.Value;
            if (ddlStars.SelectedValue != "")
            {
                request.Star = int.Parse(ddlStars.SelectedValue);
            }
            else
            {
                request.Star = -1;
            }

            request.Source = Convert.ToInt32(ddlSource.SelectedValue);
            int rowCount = 0;
            var response = tApp.SearchReortTickets(request, out rowCount);

            rptReportList.DataSource = response;
            rptReportList.DataBind();
            ReportPage.RecordCount = rowCount;
        }

        protected void iBtnDownload_Click(object sender, EventArgs e)
        {
            SearchTicketsRequest request = new SearchTicketsRequest(SearchTicketsType.TicketsForReport, OrderBy, true);
            request.Keyword = txtKeyword.Text;
            request.CurrentPage = 1;
            request.PageCount = int.MaxValue;
            request.ProjectID = int.Parse(ddlProject.SelectedValue);
            request.TicketIDS = ddlTickets.SelectedValue.ToString();
            request.TicketType = ddlType.SelectedValue;
            request.SearchTicketID = true;
            request.StartDate = StartDate;
            request.EndDate = EndDate;
            request.UserID = int.Parse(ddlUsers.SelectedValue); 


            request.Source = Convert.ToInt32(ddlSource.SelectedValue);
            int rowCount = 0;
            System.Data.DataTable table = tApp.SearchReortTickets(request, out rowCount);
            ExcelReport report = new ExcelReport();
            if(QS("star","") !="")
            report.AnalysisExport(table,"Ticket Rating");
            else
                report.AnalysisExport(table, "Ticket Analysis");
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