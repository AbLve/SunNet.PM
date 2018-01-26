using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.App;
using System.Data;
using System.Text;

namespace SunNet.PMNew.Web.Sunnet.Reports
{
    public partial class ProjectUserTimeSheet : BaseWebsitePage
    {
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
        public string DateQueue
        {
            get;
            set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            int userID = QS("user", 0);
            int projectID = QS("project", 0);
            if (userID == 0 || projectID == 0)
            {
                this.Alert("Current Page get an error,please check your argument!", "/Sunnet/Reports/Consuming.aspx");
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
                StringBuilder dates = new StringBuilder();
                foreach (DataRow dr in dt.Rows)
                {
                    dates.Append(Convert.ToDateTime(dr[0].ToString()).ToString("yyyy-MM-dd"));
                    dates.Append(",");
                }
                this.DateQueue = dates.ToString().TrimEnd(",".ToCharArray());
            }
            else
            {
                this.Alert("There is no records ", "/Sunnet/Reports/Consuming.aspx");
            }
        }
    }
}
