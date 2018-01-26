using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class TicketTasks : IHttpHandler
    {
        public int UserID
        {
            get
            {
                return IdentityContext.UserID;
            }
        }

        private string GetTasksByTickets(int id)
        {
            TicketsApplication tickApp = new TicketsApplication();
            List<TasksEntity> listtasks = tickApp.GetTaskByID(id, true);
            string jsontasks = UtilFactory.Helpers.JSONHelper.GetJson<List<TasksEntity>>(listtasks);
            return jsontasks;
        }
        private string GetTicketsByProject(int projID, DateTime sheetDate)
        {
            SearchTicketsRequest request = new SearchTicketsRequest(SearchTicketsType.TicketsForTimesheets, "TicketTitle ASC ", false);
            request.SheetDate = sheetDate;
            request.UserID = IdentityContext.UserID;
            request.ProjectID = projID;
            List<TicketsState> listStatus = TicketsStateHelper.TimeSheetStates;
            request.Status = listStatus;
            TicketsApplication tickApp = new TicketsApplication();
            SearchTicketsResponse response = tickApp.SearchTickets(request);
            if (!response.IsError)
            {
                string json = UtilFactory.Helpers.JSONHelper.GetJson<List<ExpandTicketsEntity>>(response.ResultList);
                return json;
            }
            else
            {
                return "[]";
            }
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
                    case "GetTicketsListByProject":
                        int projectID = 0;
                        DateTime sheetDate = Convert.ToDateTime(request["SheetDate"]);
                        checkinput = ((!string.IsNullOrEmpty(request["projectid"]))
                                           && int.TryParse(request["projectid"], out projectID));
                        if (checkinput)
                        {
                            context.Response.Write(GetTicketsByProject(projectID, sheetDate));
                            break;
                        }
                        else
                        {
                            context.Response.Write("[]");
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
                WebLogAgent.Write(string.Format("Error Ashx:TicketTasks.ashx Messages:\r\n{0}", ex));
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
