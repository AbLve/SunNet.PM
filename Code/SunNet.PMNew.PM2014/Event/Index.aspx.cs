using SunNet.PMNew.App;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;

namespace SunNet.PMNew.PM2014.Event
{
    public partial class Index : BasePage
    {
        private ProjectApplication projApp;
        public string DateTitle { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            projApp = new ProjectApplication();
            if (!Page.IsPostBack)
            {
                List<ProjectDetailDTO> list = projApp.GetUserProjects(UserInfo);
                switch (UserInfo.Role)
                {
                    case RolesEnum.Leader:
                    case RolesEnum.DEV:
                    case RolesEnum.QA:
                        list = list.FindAll(r => r.CompanyID == Config.SunnetCompany);
                        break;
                }

                list.BindDropdown<ProjectDetailDTO>(ddlProjects, "Title", "ProjectID", "ALL", "-1");
                ddlYears.SelectedValue = DateTime.Now.Year.ToString();
                ddlMonths.SelectedValue = DateTime.Now.Month.ToString();

                List<UsersEntity> userList = new App.ProjectApplication().GetProjectUsersByUserId(UserInfo);
                userList = userList.Distinct(new CompareUser()).ToList();
                hiUserIds.Value = string.Join(",", userList.Select(r => r.UserID).ToArray());
                ddlUser.DataSource = userList.OrderBy(r => r.FirstName);
                ddlUser.DataBind();
                ddlUser.Items.Insert(0, new ListItem() { Text = "ALL", Value = "0" });
            }
            if (ddlProjects.Items.Count <= 0)
            {
                ddlProjects.Items.Add(new ListItem() { Selected = true, Text = "None", Value = "0" });
            }
            DateTitle = DateTime.Now.ToString("MMMM yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
    }

    class CompareUser : IEqualityComparer<UsersEntity>
    {
        public bool Equals(UsersEntity x, UsersEntity y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(UsersEntity obj)
        {
            return obj.ID.GetHashCode();
        }
    }
}