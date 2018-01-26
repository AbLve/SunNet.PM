using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Entity.TicketModel;
using StructureMap;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoGetTaskList : IHttpHandler
    {
        TicketsApplication ticketApp = new TicketsApplication();

        private string GetTasksByTickets(int id)
        {
            List<TasksEntity> listTasks = new List<TasksEntity>();
            listTasks = ticketApp.GetTaskByID(id, true);
            string jsontasks = UtilFactory.Helpers.JSONHelper.GetJson<List<TasksEntity>>(listTasks);
            return jsontasks;
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                if (IdentityContext.UserID <= 0)
                    return;
                HttpRequest request = context.Request;
                string type = string.IsNullOrEmpty(request["type"]) ? request.Params["type"] : request["type"];
                bool checkinput = false;
                switch (type)
                {
                    case "GetTasksByTicket":
                        int ticketid = 0;
                        checkinput = ((!string.IsNullOrEmpty(request["ticketid"]))
                                            && int.TryParse(request["ticketid"], out ticketid));
                        if (checkinput)
                        {
                            context.Response.Write(GetTasksByTickets(ticketid));
                            break;
                        }
                        else
                        {
                            context.Response.Write("");
                            break;
                        }
                    default:
                        context.Response.Write("[]");
                        break;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoGetTaskList.ashx Messages:\r\n{0}", ex));
                return;
            }

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
