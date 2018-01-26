using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.PM2014.Do
{
    public class DoGetProjectUsers : DoBase, IHttpHandler
    {
        ProjectApplication projApp = new ProjectApplication();
        UserApplication userApp = new UserApplication();

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            // [{role:"Dev",users:{role:"",id:"",name:""}},...]
            int projectID = QS(context.Request.QueryString["pid"], 0);
            List<ProjectUserforJson> USUsers = new List<ProjectUserforJson>();
            List<ProjectUserforJson> DEVUsers = new List<ProjectUserforJson>();
            List<ProjectUserforJson> QAUsers = new List<ProjectUserforJson>();
            List<ProjectUsersforJson> projectUsersforJson = new List<ProjectUsersforJson>();
            IEnumerable<ProjectUsersEntity> projectUsers = projApp.GetProjectSunnetUserList(projectID).Distinct(new ProjectUsersComparer());

            if (projectUsers != null && projectUsers.Count() > 0)
            {
                foreach (ProjectUsersEntity projectUser in projectUsers)
                {
                    UsersEntity user = userApp.GetUser(projectUser.UserID);
                    if (user != null && user.Status.Trim() != "INACTIVE")
                    {
                        if (user.Role != RolesEnum.CLIENT)
                        {
                            switch (user.Role)
                            {
                                case RolesEnum.Supervisor:
                                case RolesEnum.Sales:
                                    {
                                        USUsers.Add(new ProjectUserforJson()
                                        {
                                            role = ((int)(user.Role)),
                                            Id = user.UserID,
                                            UserName = user.FirstAndLastName,
                                            Lastname = user.LastName
                                        });
                                        break;
                                    }
                                case RolesEnum.PM:
                                    {
                                        USUsers.Add(new ProjectUserforJson()
                                        {
                                            role = ((int)(user.Role)),
                                            Id = user.UserID,
                                            UserName = user.FirstAndLastName,
                                            Lastname = user.LastName,
                                            Selected =true
                                        });
                                        break;
                                    }
                                case RolesEnum.Leader:
                                case RolesEnum.DEV:
                                case RolesEnum.Contactor:
                                    {
                                        DEVUsers.Add(new ProjectUserforJson()
                                        {
                                            role = ((int)(user.Role)),
                                            Id = user.UserID,
                                            UserName = user.FirstAndLastName,
                                            Lastname = user.LastName
                                        });
                                        break;
                                    }
                                case RolesEnum.QA:
                                    {
                                        QAUsers.Add(new ProjectUserforJson()
                                        {
                                            role = ((int)(user.Role)),
                                            Id = user.UserID,
                                            UserName = user.FirstAndLastName,
                                            Lastname = user.LastName
                                        });
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            USUsers.Sort(new ProjectUsersforJsonComparer());
            DEVUsers.Sort(new ProjectUsersforJsonComparer());
            QAUsers.Sort(new ProjectUsersforJsonComparer());

            projectUsersforJson.Add(new ProjectUsersforJson() { Role = "US", Users = USUsers });
            projectUsersforJson.Add(new ProjectUsersforJson() { Role = "DEV", Users = DEVUsers });
            projectUsersforJson.Add(new ProjectUsersforJson() { Role = "QA", Users = QAUsers });
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(projectUsersforJson));
        }
    }

    class ProjectUsersforJson
    {
        public string Role { get; set; }
        public List<ProjectUserforJson> Users = new List<ProjectUserforJson>();
    }

    class ProjectUserforJson
    {
        public int Id { get; set; }
        public int role { get; set; }
        public string UserName { get; set; }
        public string Lastname { get; set; }
        public bool Selected { get; set; }
    }

    class ProjectUsersforJsonComparer : IComparer<ProjectUserforJson>
    {
        public int Compare(ProjectUserforJson x, ProjectUserforJson y)
        {
            return string.Compare(x.UserName, y.UserName);
        }
    }
}
