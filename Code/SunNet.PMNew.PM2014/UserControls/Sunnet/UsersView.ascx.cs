using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.UserControls.Sunnet
{
    public partial class UsersView : BaseAscx
    {
        TicketsApplication ticketApp = new TicketsApplication();
        ProjectApplication projApp = new ProjectApplication();
        UserApplication userApp = new UserApplication();

        private int ticketID;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (int.TryParse(Request.QueryString["tid"], out ticketID))
            {
                CreateAssignUserList();
            }
        }

        public HtmlGenericControl GetUserLi(UsersEntity user, bool isCreate, bool isChecked,bool isUS=false)
        {
            HtmlGenericControl li = new HtmlGenericControl("li");

            if (isCreate)
            {
                li.Style.Add("display", "none");
                li.Attributes.Add("data-isCreate", "true");
            }

            // li.Attributes.Add("title", user.FirstName + " " + user.LastName);
            CheckBox chkBox = new CheckBox() { ID = "chk-" + user.ID, Text = "  " + BasePage.GetClientUserName(user) };
            chkBox.Attributes.Add("data-role", ((int)MapRoleToTicketUserType(user.Role)).ToString());
            chkBox.Attributes.Add("data-id", user.ID.ToString());
            if (isUS)
                chkBox.Attributes.Add("data-us", "1");

            if (isChecked)
            {
                chkBox.Checked = true;
            }
            li.Controls.Add(chkBox);
            return li;
        }

        private class CompareProjectUser : IEqualityComparer<ProjectUsersEntity>
        {
            public bool Equals(ProjectUsersEntity x, ProjectUsersEntity y)
            {
                return x.ProjectID == y.ProjectID && x.UserID == y.UserID;
            }

            public int GetHashCode(ProjectUsersEntity obj)
            {
                return obj.ID.GetHashCode();
            }
        }

        private void CreateAssignUserList()
        {
            GetProjectIdAndUserIDResponse response = ticketApp.GetProjectIdAndUserID(ticketID);
            List<ProjectUsersEntity> projectUsers = projApp.GetProjectSunnetUserList(response.ProjectId)
                .Distinct(new CompareProjectUser())
                .ToList<ProjectUsersEntity>();
            if (UserInfo.Role == RolesEnum.Leader)
            {
                dvUS.Style.Add("visibility", "hidden");
                dvUS.Style.Add("overflow", "auto");
                dvUS.Style.Add("width", "1px");
                dvUS.Style.Add("height", "1px");
            }

            List<TicketUsersEntity> ticketUsers = ticketApp.GetListUsersByTicketId(ticketID);
            TicketsEntity ticketEntity = ticketApp.GetTickets(ticketID);
            ProjectsEntity projectsEntity = projApp.Get(response.ProjectId);
            if (projectUsers != null && projectUsers.Count() > 0)
            {
                foreach (ProjectUsersEntity projectUser in projectUsers)
                {
                    UsersEntity user = userApp.GetUser(projectUser.UserID);
                    if (user != null && user.Status.Trim() != "INACTIVE")
                    {
                        if (user.Role != RolesEnum.CLIENT)
                        {
                            // 去掉“隐藏ticket的创建者和project的pm” lyq 20140808
                            //bool isTicketCreateUser = (ticketEntity.CreatedBy == user.UserID || user.UserID == projectsEntity.PMID);
                            bool isTicketCreateUser = false;
                            bool isChecked = ticketUsers.Find(r => r.UserID == user.UserID) != null;
                            switch (user.Role)
                            {
                                case RolesEnum.Supervisor:
                                case RolesEnum.Sales:
                                case RolesEnum.PM:
                                    {
                                        this.ulUS.Controls.Add(GetUserLi(user, isTicketCreateUser, isChecked,true));
                                        break;
                                    }
                                case RolesEnum.Leader:
                                case RolesEnum.DEV:
                                case RolesEnum.Contactor:
                                    {
                                        this.ulDev.Controls.Add(GetUserLi(user, isTicketCreateUser, isChecked));
                                        break;
                                    }
                                case RolesEnum.QA:
                                    {
                                        this.ulQA.Controls.Add(GetUserLi(user, isTicketCreateUser, isChecked));
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }

        public Dictionary<string, string> GetSelectedUserList()
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            GetProjectIdAndUserIDResponse response = ticketApp.GetProjectIdAndUserID(ticketID);
            List<ProjectUsersEntity> projectUsers = projApp.GetProjectSunnetUserList(response.ProjectId);

            foreach (ProjectUsersEntity projectUser in projectUsers)
            {
                switch (projectUser.Role)
                {
                    case RolesEnum.Supervisor:
                    case RolesEnum.Sales:
                    case RolesEnum.PM:
                        {
                            Control li = (Control)(this.ulUS.Controls[0]);
                            Control control = li.FindControl("chk-" + projectUser.UserID);
                            if (control != null)
                            {
                                CheckBox checkBox = (CheckBox)control;
                                if (checkBox.Checked)
                                {
                                    if (!results.ContainsKey(checkBox.Attributes["data-id"]))
                                        results.Add(checkBox.Attributes["data-id"], "");
                                    results[checkBox.Attributes["data-id"]] = checkBox.Attributes["data-role"];
                                }
                            }
                            break;
                        }
                    case RolesEnum.Leader:
                    case RolesEnum.DEV:
                    case RolesEnum.Contactor:
                        {
                            Control li = (Control)(this.ulDev.Controls[0]);
                            Control control = li.FindControl("chk-" + projectUser.UserID);
                            if (control != null)
                            {
                                CheckBox checkBox = (CheckBox)control;
                                if (checkBox.Checked)
                                {
                                    if (!results.ContainsKey(checkBox.Attributes["data-id"]))
                                        results.Add(checkBox.Attributes["data-id"], "");
                                    results[checkBox.Attributes["data-id"]] = checkBox.Attributes["data-role"];
                                }
                            }
                            break;
                        }

                    case RolesEnum.QA:
                        {
                            Control li = (Control)(this.ulQA.Controls[0]);
                            Control control = li.FindControl("chk-" + projectUser.UserID);
                            if (control != null)
                            {
                                CheckBox checkBox = (CheckBox)control;
                                if (checkBox.Checked)
                                {
                                    if (!results.ContainsKey(checkBox.Attributes["data-id"]))
                                        results.Add(checkBox.Attributes["data-id"], "");
                                    results[checkBox.Attributes["data-id"]] = checkBox.Attributes["data-role"];
                                }
                            }
                            break;
                        }
                }
            }
            return results;
        }
    }
}