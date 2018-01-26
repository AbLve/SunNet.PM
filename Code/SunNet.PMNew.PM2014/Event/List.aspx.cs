using SunNet.PMNew.App;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;

namespace SunNet.PMNew.PM2014.Event
{
    public partial class List : BasePage
    {
        private ProjectApplication projApp;
        protected List<UpcomingEvent> upcomingEvents = new List<UpcomingEvent>();
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

                List<UsersEntity> userList = new List<UsersEntity>();
                List<int> userIdList = new App.ProjectApplication().GetProjectUserIds(UserInfo.UserID);

                if (UserInfo.Role == RolesEnum.CLIENT)
                {
                    foreach (int tmpId in userIdList)
                    {
                        UsersEntity tmpUserEntity = new App.UserApplication().GetUser(tmpId);
                        if (tmpUserEntity != null && tmpUserEntity.Status.ToUpper() == "ACTIVE")
                        {
                            if (tmpUserEntity.Role == RolesEnum.CLIENT)
                                userList.Add(tmpUserEntity);
                            else if (tmpUserEntity.Office.ToUpper() == "US" && tmpUserEntity.UserType.ToUpper() == "SUNNET")
                                userList.Add(tmpUserEntity);
                        }
                    }
                }
                else
                {
                    if (UserInfo.UserType.ToUpper() == "SUNNET")
                    {
                        if (UserInfo.Office.ToUpper() == "US")
                        {
                            foreach (int tmpId in userIdList)
                            {
                                UsersEntity tmpUserEntity = new App.UserApplication().GetUser(tmpId);
                                if (tmpUserEntity != null && tmpUserEntity.Status.ToUpper() == "ACTIVE")
                                {
                                    userList.Add(tmpUserEntity);
                                }
                            }
                        }
                        else
                        {
                            foreach (int tmpId in userIdList)
                            {
                                UsersEntity tmpUserEntity = new App.UserApplication().GetUser(tmpId);
                                if (tmpUserEntity != null && tmpUserEntity.Status.ToUpper() == "ACTIVE")
                                {
                                    if (tmpUserEntity.Office.ToUpper() == "CN")
                                        userList.Add(tmpUserEntity);
                                }
                            }
                        }
                    }
                    else
                    {
                        userList.Add(UserInfo);
                    }
                }

                if (userIdList.Count == 0)
                    userList.Add(UserInfo);
                userList = userList.Distinct(new CompareUser()).ToList();
                hiUserIds.Value = string.Join(",", userList.Select(r => r.UserID).ToArray());
                ddlUser.DataSource = userList.OrderBy(r=>r.FirstName);
                ddlUser.DataBind();
                ddlUser.Items.Insert(0, new ListItem() { Text = "ALL", Value = "0" });
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

        private DateTime GetCurrentSelectedDate()
        {
            return DateTime.Parse(ddlMonths.SelectedValue + "/" + "1" + "/" + ddlYears.SelectedValue);
        }


        public void SearchEvents()
        {
            int resultCount;
            upcomingEvents = new EventsApplication().GetEventList(UserInfo.UserID, GetCurrentSelectedDate(), int.Parse(ddlUser.SelectedValue)
                , hiUserIds.Value, GetProjectID(), int.MaxValue, 1, out resultCount);

        }

    }
}