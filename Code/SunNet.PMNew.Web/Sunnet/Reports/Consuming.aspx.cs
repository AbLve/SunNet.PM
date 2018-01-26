using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using System.Data;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Utils;
using System.Text.RegularExpressions;

namespace SunNet.PMNew.Web.Sunnet.Reports
{
    public partial class Consuming : BaseWebsitePage
    {
        private string ReportView
        {
            get
            {
                if (ViewState["ReportView"] == null || string.IsNullOrEmpty(ViewState["ReportView"].ToString()))
                {
                    return "DetailView";
                }
                return ViewState["ReportView"].ToString();
            }
            set
            {
                ViewState["ReportView"] = value;
            }
        }

        TimeSheetApplication tsApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            tsApp = new TimeSheetApplication();
            if (!IsPostBack)
            {
                InitSearchControls();
                btnDetailView_Click(null, null);
            }
        }
        private void InitSearchControls()
        {
            DateTime date = DateTime.Now.AddMonths(-1);
            //txtStartDate.Text = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
            //txtEndDate.Text = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)).ToString("MM/dd/yyyy");

            ProjectApplication projApp = new ProjectApplication();
            List<ProjectDetailDTO> listAll = projApp.GetUserProjects(UserInfo);
            listAll.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID", "All", "0");

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
        private void InitControls()
        {
            if (this.ReportView == "DetailView")
            {
                rptHoursView.Visible = false;
                rptDetailView.Visible = true;
                DataTable dt = tsApp.QueryReportDetailsByProject(
                    int.Parse(ddlProject.SelectedValue),
                    int.Parse(ddlUsers.SelectedValue),
                    StartDate,
                    EndDate,
                    hidOrderBy.Value,
                    hidOrderDirection.Value);
                rptDetailView.DataSource = dt;
                rptDetailView.DataBind();
            }
            else if (this.ReportView == "HoursView")
            {
                rptDetailView.Visible = false;
                rptHoursView.Visible = true;

                DataTable dt = tsApp.QueryReportTotalHoursByProject(
                    int.Parse(ddlProject.SelectedValue),
                    int.Parse(ddlUsers.SelectedValue),
                    StartDate,
                    EndDate,
                    hidOrderBy.Value,
                    hidOrderDirection.Value);
                rptHoursView.DataSource = dt;
                rptHoursView.DataBind();
            }
            else
            {
                rptDetailView.Visible = false;
                rptHoursView.Visible = false;
            }

        }
        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            InitControls();
        }
        protected void iBtnDownload_Click(object sender, ImageClickEventArgs e)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
            if (this.ReportView == "DetailView")
            {
                rptDetailView.RenderControl(hw);
            }
            else if (this.ReportView == "HoursView")
            {
                rptHoursView.RenderControl(hw);
            }
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            Page.EnableViewState = false;
            string fileName = string.Format("{0}_{1}.xls", ddlProject.SelectedItem.Text, ddlUsers.SelectedItem.Text).Replace(" ", "_");
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\"><title>Project Report</title></head><body><center>");
            string body = sw.ToString();
            // body = body.Replace("excelWidth", "width");
            body = body.Replace("width: 30%;", "width:300;");
            body = body.Replace("width: 20%;", "width:100;");
            body = body.Replace("width: 10%;", "width:100;");

            body = body.Replace("width: 71%;", "width:300;");
            body = body.Replace("width: 29%;", "width:100;");
            Regex removeActioin = new Regex(@"<(td|th).*(?=hidethis)(.|\n)*?</(th|td)>");
            body = removeActioin.Replace(body, "");
            body = body.Replace("<td", "<td style='mso-number-format:\"0\\.00\";'");
            body = body.Replace("<table border=\"0\"", "<table border=\"1\" ");
            Response.Write(body);
            Response.Write("</center></body></html>");
            Response.End();
        }

        protected void btnDetailView_Click(object sender, ImageClickEventArgs e)
        {
            btnDetailView.Enabled = false;
            btnHoursView.Enabled = true;
            btnDetailView.ToolTip = "Current is Detail View";
            btnHoursView.ToolTip = "Change to Hours View";
            this.ReportView = "DetailView";
            InitControls();
        }

        protected void btnHoursView_Click(object sender, ImageClickEventArgs e)
        {
            btnDetailView.Enabled = true;
            btnHoursView.Enabled = false;
            btnDetailView.ToolTip = "Change to Detail View";
            btnHoursView.ToolTip = "Current is Hours View";
            this.ReportView = "HoursView";
            InitControls();
        }
    }
}
