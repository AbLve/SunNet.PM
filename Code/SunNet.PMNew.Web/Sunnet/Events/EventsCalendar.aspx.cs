using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class EventCalendar : BaseWebsitePage
    {
        private ProjectApplication projApp;

        protected void Page_Load(object sender, EventArgs e)
        {
            projApp = new ProjectApplication();
            if (!Page.IsPostBack)
            {
                projApp.GetUserProjects(UserInfo).BindDropdown<ProjectDetailDTO>(ddlProjects, "Title", "ProjectID", "Please select...", "-1");

             
                ddlYears.SelectedValue = DateTime.Now.Year.ToString();
                ddlMonths.SelectedValue = DateTime.Now.Month.ToString();
            }
            if (ddlProjects.Items.Count <= 0)
            {
                ddlProjects.Items.Add(new ListItem() { Selected = true, Text = "None", Value = "0" });
            }
        }
    }


}
