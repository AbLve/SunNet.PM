using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Report
{
    public partial class Consuming : BasePage
    {
        private string ReportView
        {
            get
            {
                if (Request.QueryString["viewmodel"] == null || string.IsNullOrEmpty(Request.QueryString["viewmodel"]))
                {
                    return "DetailView";
                }
                else if (Request.QueryString["viewmodel"].ToLower() == "hoursmodel")
                {
                    return "HoursView";
                }
                else
                {
                    return "DetailView";
                }
            }
        }
        protected override string DefaultOrderBy
        {
            get
            {
                return "Title";
            }
        }
        protected override string DefaultDirection
        {
            get
            {
                return "ASC";
            }
        }
        TimeSheetApplication tsApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            tsApp = new TimeSheetApplication();
            if (!IsPostBack)
            {
                InitSearchControls();
                if (QS("project") != "")
                    ddlProject.SelectedValue = QS("project");
                if (QS("user") != "")
                    ddlUsers.SelectedValue = QS("user");
                txtStartDate.Text = QS("startdate");
                txtEndDate.Text = QS("enddate");
                InitControls();
            }
        }
        private void InitSearchControls()
        {
            DateTime date = DateTime.Now.AddMonths(-1);
            //txtStartDate.Text = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
            //txtEndDate.Text = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)).ToString("MM/dd/yyyy");

            ProjectApplication projApp = new ProjectApplication();
            List<ProjectDetailDTO> listAll = projApp.GetAllProjects();
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
                rptListReport.Visible = true;
                DataTable dt = tsApp.QueryReportDetailsByProject(
                    int.Parse(ddlProject.SelectedValue),
                    int.Parse(ddlUsers.SelectedValue),
                    StartDate,
                    EndDate,
                    OrderBy,
                    OrderDirection);
                rptListReport.DataSource = dt;
                rptListReport.DataBind();

                if (dt.Rows.Count == 0)
                {
                    trNoListRecord.Visible = true;
                }
            }
            else if (this.ReportView == "HoursView")
            {
                rptListReport.Visible = false;
                rptHoursView.Visible = true;

                DataTable dt = tsApp.QueryReportTotalHoursByProject(
                    int.Parse(ddlProject.SelectedValue),
                    int.Parse(ddlUsers.SelectedValue),
                    StartDate,
                    EndDate,
                   OrderBy,
                    OrderDirection);
                rptHoursView.DataSource = dt;
                rptHoursView.DataBind();
                if (dt.Rows.Count == 0)
                {
                    trNoHourRecord.Visible = true;
                }

            }
            else
            {
                rptListReport.Visible = false;
                rptHoursView.Visible = false;
            }

        }
        protected void iBtnDownload_Click(object sender, EventArgs e)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
            if (this.ReportView == "DetailView")
            {
                rptListReport.RenderControl(hw);
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

            body = body.Replace("width: 30%;", "width:300;");
            body = body.Replace("width: 20%;", "width:100;");
            body = body.Replace("width: 10%;", "width:100;");

            body = body.Replace("width: 71%;", "width:300;");
            body = body.Replace("width: 29%;", "width:100;");
            Regex removeActioin = new Regex(@"<(td|th).*(?=hidethis)(.|\n)*?</(th|td)>");
            body = removeActioin.Replace(body, "");
            body = body.Replace("<td", "<td style='mso-number-format:\"0\\.00\";'");//
            body = body.Replace("<table border=\"0\"", "<table border=\"1\" ");
            Response.Write(body);
            Response.Write("</center></body></html>");
            Response.End();
        }
    }
}