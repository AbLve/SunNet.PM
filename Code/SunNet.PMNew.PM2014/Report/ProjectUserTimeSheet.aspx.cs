using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.HSSF.Util;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Report
{
    public partial class ProjectUserTimeSheet : BasePage
    {
        TimeSheetApplication tsApp = new TimeSheetApplication();
        private decimal totalHours = 0;
        public UsersEntity SelectedUser
        {
            get;
            set;
        }
        public ProjectsEntity SelectedProject
        {
            get;
            set;
        }
        private List<TimeSheetTicket> GetTimesheetTicket(int categoryID, DateTime startDate, DateTime endDate, bool addDefaultEmpty, int userID, int projectID)
        {
            List<TimeSheetTicket> list = tsApp.SearchTimeSheets(categoryID, startDate, endDate, userID, projectID, addDefaultEmpty);
            return list;
        }
        private List<TimesheetDisplay> GetTimeSheets(DateTime time, int projectId, int category, int userId)
        {
            DateTime start = DateTime.Now, end = DateTime.Now;

            List<TimesheetDisplay> list = new List<TimesheetDisplay>();

            start = time;
            end = time;
            int.TryParse(Request.Params["project"], out projectId);
            int.TryParse(Request.Params["category"], out category);
            var list1 = GetTimesheetTicket(category, start, end, false, userId, projectId);
            if (list1 != null && list1.Count > 0)
                list = list1.Select(x => new TimesheetDisplay
                {
                    Title = x.TicketTitle,
                    Project = x.ProjectTitle,
                    ID = x.TimeSheetID,
                    Code = x.TicketID.ToString(),
                    WorkDetail = x.WorkDetail,
                    Hours = x.Hours,
                    IsMeeting = x.IsMeeting,
                    SubmittedText = x.IsSubmitted == true?"Yes":"No",
                    Percentage = x.Percentage
                }).ToList();
            else
                list = new List<TimesheetDisplay>();
            return list;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
             
            int userID = QS("user", 0);
            int projectID = QS("project", 0);
            if (userID == 0 || projectID == 0)
            {
                this.Alert("Current Page get an error,please check your argument!", "/Report/Consuming.aspx?viewmodel=detailmodel");
            }
            UserApplication userApp = new UserApplication();
            this.SelectedUser = userApp.GetUser(userID);
            ProjectApplication projApp = new ProjectApplication();
            this.SelectedProject = projApp.Get(projectID);

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            DateTime.TryParse(Request.QueryString["startdate"], out startDate);
            DateTime.TryParse(Request.QueryString["enddate"], out endDate);

            TimeSheetApplication tsApp = new TimeSheetApplication();
            DataTable dt = tsApp.GetSheetDateByProjectUser(projectID, userID, startDate, endDate);
            if (dt != null && dt.Rows.Count > 0)
            {
               
                this.rptDateView.DataSource = dt;
                this.rptDateView.DataBind();
            }
            else
            {
                ShowFailMessageToClient("There is no records ");
            }
            litUserName.Text = SelectedUser.FirstName + " " + SelectedUser.LastName;
            litTotalhours.Text = totalHours.ToString("#0.00") + " h";
        }

        protected void rptDateView_ItemDataBound(object sender, RepeaterItemEventArgs e)
        { 
            Repeater sublist = (Repeater)e.Item.FindControl("subDetail");
            HiddenField hiddenDate = e.Item.FindControl("hiddenDate") as HiddenField;
            if (sublist != null)
            {
                DateTime date = DateTime.Parse(hiddenDate.Value.ToString());
                int projectId = 0;
                if (!string.IsNullOrEmpty(QS("project")))
                    projectId = int.Parse(QS("project"));
                int userId = 0;
                if (SelectedUser != null)
                    userId = SelectedUser.UserID;

                if (sublist != null)
                {
                    List<TimesheetDisplay> list = GetTimeSheets(date, projectId, 0, userId);
                    
                    sublist.DataSource = list;
                    sublist.DataBind();
                    foreach (TimesheetDisplay item in list)
                    {
                        totalHours += item.Hours;
                    }
                }
            }
        }
    }
}

public class TimesheetDisplay
{
    public int ID { get; set; }
    public string Project { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public string WorkDetail { get; set; }
    public decimal Hours { get; set; }
    public decimal Percentage { get; set; }
    public string SubmittedText { get; set; }
    public bool IsMeeting { get; set; }
    public bool IsRed { get; set; }
}