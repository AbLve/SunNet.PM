using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.EventModel;

namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class EventList : BaseWebsitePage
    {
        private ProjectApplication projApp;
        protected List<UpcomingEvent> upcomingEvents = new List<UpcomingEvent>();
        protected void Page_Load(object sender, EventArgs e)
        {
            projApp = new ProjectApplication();
            if (!Page.IsPostBack)
            {
                projApp.GetUserProjects(UserInfo).BindDropdown<ProjectDetailDTO>(ddlProjects, "Title", "ProjectID", "Please select...", "-1");
                ddlYears.SelectedValue = DateTime.Now.Year.ToString();
                ddlMonths.SelectedValue = DateTime.Now.Month.ToString();
            }
            SearchEvents();
            if (ddlProjects.Items.Count <= 0)
            {
                ddlProjects.Items.Add(new ListItem() { Selected = true, Text = "None", Value = "0" });
            }
        }

        private int GetProjectID()
        {
            int projectID = 0;
            if (ddlProjects.Items.Count > 0)
            {
                projectID = int.Parse(ddlProjects.SelectedValue);
            }
            return projectID;
        }

        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            anpTicketReport.CurrentPageIndex = 1;
            //;
            //anpTicketReport.PageSize
            SearchEvents();
        }

        private DateTime GetCurrentSelectedDate()
        {
            return DateTime.Parse(ddlMonths.SelectedValue + "/" + "1" + "/" + ddlYears.SelectedValue);
        }

        protected void anpTicketReport_PageChanged(object sender, EventArgs e)
        {
            SearchEvents();
        }

        public void SearchEvents()
        {
            int resultCount;
            upcomingEvents = new EventsApplication().GetEventList(GetCurrentSelectedDate(), UserInfo.UserID,   GetProjectID(), anpTicketReport.PageSize, anpTicketReport.CurrentPageIndex, out resultCount);
            anpTicketReport.RecordCount = resultCount;
        }


    }
}
