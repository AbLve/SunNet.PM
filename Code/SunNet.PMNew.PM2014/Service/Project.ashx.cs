using Newtonsoft.Json;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel.ProjectTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.PM2014.Service
{
    /// <summary>
    /// Project 的摘要说明
    /// </summary>
    public class Project : IHttpHandler
    {
        ProjectApplication projectApp = new ProjectApplication();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string action = context.Request.Params["action"].ToLower();
            int userid = int.Parse(context.Request.Params["userid"]);
            switch (action)
            {
                case "getclientproject":
                    context.Response.Write(GetProjectTicket(false, userid));
                    break;
                case "getinternalproject":
                    context.Response.Write(GetProjectTicket(true, userid));
                    break;
                default:
                    context.Response.Write("[]");
                    break;
            }
        }

        private string GetProjectTicket(bool internalProject, int userId)
        {
            List<ProjectTicketModel> models = new List<ProjectTicketModel>();
            models = projectApp.GetProjectTicketList(internalProject, userId);
            string projectTickets = JsonConvert.SerializeObject(models);
            return projectTickets;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}