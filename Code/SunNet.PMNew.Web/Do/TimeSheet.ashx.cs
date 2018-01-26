using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Framework.Core;
using StructureMap;
using System.Text;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Web.Codes;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class TimeSheet : IHttpHandler
    {
        private bool ContainsTickets(List<ExpandTimeSheetsEntity> list, int ticketID)
        {
            if (null != list && list.Count > 0)
            {
                foreach (ExpandTimeSheetsEntity timesheet in list)
                {
                    if (timesheet.TicketID == ticketID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private string GetTimesheetTicketJson(int categoryID, DateTime startDate, DateTime endDate, bool addDefaultEmpty, int userID, int projectID)
        {
            TimeSheetApplication tsApp = new TimeSheetApplication();
            List<TimeSheetTicket> list = tsApp.SearchTimeSheets(categoryID, startDate, endDate, userID, projectID, addDefaultEmpty);
            string json = UtilFactory.Helpers.JSONHelper.GetJson<List<TimeSheetTicket>>(list);
            return json;
        }
        private bool UpdateTimeSheet(TimeSheetsEntity model, out string msg)
        {
            msg = string.Empty;
            TimeSheetApplication tsApp = new TimeSheetApplication();
            bool result = tsApp.UpdateTimeSheet(model);
            msg = tsApp.BrokenRuleMessages.Count > 0 ? tsApp.BrokenRuleMessages[0].Message : SuccessMessage;
            return result;
        }
        private bool DeleteTimeSheet(int id)
        {
            TimeSheetApplication tsApp = new TimeSheetApplication();
            return tsApp.DeleteTimeSheet(id);
        }
        private int AddTimeSheet(TimeSheetsEntity model, out string msg)
        {
            msg = string.Empty;
            TimeSheetApplication tsApp = new TimeSheetApplication();
            int id = tsApp.AddTimeSheet(model);
            if (tsApp.BrokenRuleMessages.Count > 0 || id <= 0)
            {
                msg = tsApp.BrokenRuleMessages[0].Message;
            }
            return id;
        }
        private bool CancelSubmit(string date, string userID, out string msg)
        {
            msg = string.Empty;
            try
            {
                DateTime sheetDate = Convert.ToDateTime(date).Date;
                int id = int.Parse(userID);
                TimeSheetApplication tsApp = new TimeSheetApplication();
                if (tsApp.CancelSubmitTimeSheets(sheetDate, id))
                {
                    return true;
                }
                else
                {
                    msg = tsApp.BrokenRuleMessages[0].Message;
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        private string GetResponse(bool success, string msg, int value)
        {
            ResponseMessage m = new ResponseMessage();
            m.Success = success;
            m.MessageContent = msg;
            m.Value = value.ToString();
            m.TimeSheetID = value;

            return UtilFactory.Helpers.JSONHelper.GetJson<ResponseMessage>(m);
        }
        private string SuccessMessage = "Operation successful.";
        private string FailMessage = "Operation failed.";
        private TimeSheetsEntity GetEntity(HttpContext context, out string msg)
        {
            msg = string.Empty;
            HttpRequest request = context.Request;
            try
            {
                TimeSheetsEntity timesheet = TimeSheetFactory.CreateTimeSheet(IdentityContext.UserID, ObjectFactory.GetInstance<ISystemDateTime>());

                timesheet.Description = request.Params["WorkDetail"];
                decimal decValue;
                if (!decimal.TryParse(request.Params["Hours"], out decValue))
                {
                    msg = "Hours must be a number little than 24.";
                    return null;
                }
                if (!decimal.TryParse(request.Params["Percentage"], out decValue))
                {
                    msg = "Percentage must be a number little than 100.";
                    return null;
                }
                bool booValue = false;
                if (!bool.TryParse(request.Params["IsMeeting"], out booValue))
                {
                    msg = "IsMeeting must be a bool value or bit value.";
                    return null;
                }
                if (!bool.TryParse(request.Params["IsSubmitted"], out booValue))
                {
                    msg = "IsSubmitted must be a bool value or bit value.";
                    return null;
                }
                int intValue;
                if (!int.TryParse(request.Params["ProjectID"], out intValue))
                {
                    msg = "Project can not be null.";
                    return null;
                }
                if (!int.TryParse(request.Params["TicketID"], out intValue))
                {
                    msg = "Ticket can not be null.";
                    return null;
                }
                timesheet.Hours = decimal.Parse(request.Params["Hours"]);
                timesheet.IsMeeting = bool.Parse(request.Params["IsMeeting"]);
                timesheet.IsSubmitted = bool.Parse(request.Params["IsSubmitted"]);
                timesheet.Percentage = decimal.Parse(request.Params["Percentage"]);
                timesheet.ProjectID = int.Parse(request.Params["ProjectID"]);
                timesheet.SheetDate = Convert.ToDateTime(context.Server.UrlDecode(request.Params["SheetDate"]));
                timesheet.TicketID = int.Parse(request.Params["TicketID"]);
                timesheet.UserID = IdentityContext.UserID;
                timesheet.ID = int.Parse(request.Params["TimeSheetID"]);
                return timesheet;
            }
            catch (Exception ex)
            {
                msg = string.Format("Input Error:{0}", ex.Message);
                return null;
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
                if (string.IsNullOrEmpty(request.Params["type"]))
                {
                    context.Response.Write("");
                }
                else
                {
                    DateTime date = Convert.ToDateTime(request.Params["date"]);
                    DateTime startDate = date.Date;
                    DateTime endDate = date.Date;
                    int projectID = 0;
                    switch (request.Params["type"])
                    {
                        case "GetTicketsByCategory":
                            //int id = 0;
                            //if (int.TryParse(request.Params["cagegoryid"], out id) && id > 0)
                            //{
                            //    context.Response.Write(GetTicketsJsonByCategory(id));
                            //    break;
                            //}
                            //else
                            //{
                            //    context.Response.Write("");
                            //    break;
                            //}
                            context.Response.Write("");
                            break;
                        case "GetTimeSheetsByWeek":
                            if (string.IsNullOrEmpty(request["date"]))
                            {
                                context.Response.Write("");
                                break;
                            }
                            int.TryParse(request.Params["project"], out projectID);

                            if (string.IsNullOrEmpty(request.Params["category"]))
                            {
                                context.Response.Write(GetTimesheetTicketJson(0, startDate, endDate, true, IdentityContext.UserID, projectID));
                                break;
                            }
                            else
                            {
                                int category = 0;
                                if (int.TryParse(request.Params["category"], out category))
                                {
                                    context.Response.Write(GetTimesheetTicketJson(category, date, date, true, IdentityContext.UserID, projectID));
                                    break;
                                }
                            }
                            break;
                        case "GetTimeSheetsByDate":
                            if (string.IsNullOrEmpty(request["date"]))
                            {
                                context.Response.Write("");
                                break;
                            }
                            int.TryParse(request.Params["project"], out projectID);

                            int userID = IdentityContext.UserID;
                            if (!string.IsNullOrEmpty(request.Params["userid"]))
                            {
                                int.TryParse(request.Params["userid"], out userID);
                            }
                            if (string.IsNullOrEmpty(request.Params["category"]))
                            {
                                context.Response.Write(GetTimesheetTicketJson(0, startDate, endDate, false, userID, projectID));
                                break;
                            }
                            else
                            {
                                int category = 0;
                                if (int.TryParse(request.Params["category"], out category))
                                {
                                    context.Response.Write(GetTimesheetTicketJson(category, date, date, false, userID, projectID));
                                    break;
                                }
                            }
                            break;
                        case "update":
                            int updateID = 0;
                            if (int.TryParse(request.Params["TimeSheetID"], out updateID))
                            {
                                string response = string.Empty;
                                string msg = string.Empty;
                                TimeSheetsEntity model = GetEntity(context, out msg);
                                if (model == null)
                                {
                                    response = GetResponse(false, msg, 0);
                                    context.Response.Write(response);
                                    break;
                                }

                                if (updateID == 0)
                                {
                                    model.ID = AddTimeSheet(model, out msg);
                                    if (model.ID > 0)
                                    {
                                        response = GetResponse(true, SuccessMessage, model.ID);
                                        context.Response.Write(response);
                                        break;
                                    }
                                    else
                                    {
                                        response = GetResponse(false, msg, model.ID);
                                        context.Response.Write(response);
                                        break;
                                    }
                                }
                                if (model.IsSubmitted == false)
                                {

                                    bool update = UpdateTimeSheet(model, out msg);
                                    if (update)
                                    {
                                        response = GetResponse(true, SuccessMessage, model.ID);
                                        context.Response.Write(response);
                                        break;
                                    }
                                    else
                                    {
                                        response = GetResponse(false, msg, model.ID);
                                        context.Response.Write(response);
                                        break;
                                    }
                                }
                                else
                                {
                                    response = GetResponse(false, "Current item has submitted.", model.ID);
                                    context.Response.Write(response);
                                    break;
                                }
                            }
                            context.Response.Write("false");
                            break;
                        case "add":
                            string msgAdd = string.Empty;
                            TimeSheetsEntity modelAdd = GetEntity(context, out msgAdd);
                            string responseAdd = string.Empty;
                            if (modelAdd == null)
                            {
                                responseAdd = GetResponse(false, msgAdd, 0);
                                context.Response.Write(responseAdd);
                                break;
                            }
                            modelAdd.ID = AddTimeSheet(modelAdd, out msgAdd);
                            if (modelAdd.ID > 0)
                            {
                                responseAdd = GetResponse(true, SuccessMessage, modelAdd.ID);
                                context.Response.Write(responseAdd);
                                break;
                            }
                            else
                            {
                                responseAdd = GetResponse(false, msgAdd, modelAdd.ID);
                                context.Response.Write(responseAdd);
                                break;
                            }
                        case "delete":
                            int deleteID = 0;
                            string responseDelete = string.Empty;
                            if (int.TryParse(request.Params["id"], out deleteID) && deleteID > 0)
                            {
                                if (DeleteTimeSheet(deleteID))
                                {
                                    responseDelete = GetResponse(true, SuccessMessage, deleteID);
                                    context.Response.Write(responseDelete);
                                    break;
                                }
                                else
                                {
                                    responseDelete = GetResponse(false, FailMessage, deleteID);
                                    context.Response.Write(responseDelete);
                                    break;
                                }
                            }
                            responseDelete = GetResponse(true, SuccessMessage, deleteID);
                            context.Response.Write(responseDelete);
                            break;
                        case "cancelsubmit":
                            string sheeddate = request.Params["sheetdate"];
                            string userid = request.Params["userid"];
                            string responseMsg = string.Empty;
                            string msgcancel = string.Empty;
                            if (string.IsNullOrEmpty(sheeddate) || string.IsNullOrEmpty(userid))
                            {
                                responseMsg = GetResponse(false, FailMessage, 0);
                            }
                            else
                            {
                                if (CancelSubmit(sheeddate, userid, out msgcancel))
                                {
                                    responseMsg = GetResponse(true, SuccessMessage, 0);
                                }
                                else
                                {
                                    responseMsg = GetResponse(false, msgcancel, 0);
                                }
                            }
                            context.Response.Write(responseMsg);
                            break;
                        default:
                            responseDelete = GetResponse(false, FailMessage, 0);
                            context.Response.Write(responseDelete);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:TimeSheet.ashx Messages:\r\n{0}", ex));
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
