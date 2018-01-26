using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Web.Do;

namespace SunNet.PMNew.PM2014.Service
{
    /// <summary>
    /// Timesheet 的摘要说明
    /// </summary>
    public class Timesheet : DoBase, IHttpHandler
    {
        TimeSheetApplication tsApp = new TimeSheetApplication();
        private List<TimeSheetTicket> GetTimesheetTicket(int categoryID, DateTime startDate, DateTime endDate, bool addDefaultEmpty, int userID, int projectID)
        {
            List<TimeSheetTicket> list = tsApp.SearchTimeSheets(categoryID, startDate, endDate, userID, projectID, addDefaultEmpty);
            return list;
        }
        private SearchTicketsResponse GetTicketsByProject(int projID, DateTime sheetDate)
        {
            SearchTicketsRequest request = new SearchTicketsRequest(SearchTicketsType.TicketsForTimesheets, "TicketTitle ASC ", false);
            request.SheetDate = sheetDate;
            request.UserID = IdentityContext.UserID;
            request.ProjectID = projID;
            request.OrderBy = "TicketID";
            List<TicketsState> listStatus = TicketsStateHelper.TimeSheetStates;
            request.Status = listStatus;
            TicketsApplication tickApp = new TicketsApplication();
            SearchTicketsResponse response = tickApp.SearchTickets(request);
            return response;
        }
        public void ProcessRequest(HttpContext context)
        {
            var Request = context.Request;
            context.Response.ContentType = "application/json";
            if (UserID < 1 || UserInfo == null || UserInfo.Role == Entity.UserModel.RolesEnum.CLIENT)
            {
                context.Response.Write("[]");
                context.Response.End();
            }
            DateTime date = DateTime.MinValue, startDate = DateTime.Now, endDate = DateTime.Now;
            int projectId, category;
            string action = context.Request.Params["action"].ToLower();
            var list = new object();
            string msg = "";
            bool boolResult = false;
            int deleteID = 0;
            switch (action)
            {
                case "gettimesheetsbyweek":
                    if (DateTime.TryParse(Request.Params["date"], out date))
                    {
                        startDate = date;
                        endDate = date;
                        var list2 = GetTimesheetTicket(0, startDate, endDate, false, UserID, 0);
                        if (list2 != null && list2.Count > 0)
                            list = new
                            {
                                date = date,
                                totalHours = list2.Select(x => x.Hours).Sum(),
                                timesheets = list2.Select(x => new
                                {
                                    ticket = new { title = x.TicketTitle, id = x.TicketID, description = x.TicketDescription, pct = x.Percentage },
                                    project = new { id = x.ProjectID, title = x.ProjectTitle },
                                    id = x.TimeSheetID,
                                    workDetail = x.WorkDetail,
                                    hours = x.Hours,
                                    isMeeting = x.IsMeeting,
                                    submitted = x.IsSubmitted
                                })
                            };
                        else
                            list = new List<int>();
                        context.Response.Write(JsonConvert.SerializeObject(list, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" }));
                    }
                    else
                    {
                        context.Response.Write("[]");
                    }
                    break;
                case "gettimesheetsbydate":
                    DateTime.TryParse(Request.Params["date"], out date);
                    startDate = date;
                    endDate = date;
                    int.TryParse(Request.Params["project"], out projectId);
                    int.TryParse(Request.Params["category"], out category);
                    var list1 = GetTimesheetTicket(category, startDate, endDate, false, UserID, projectId);
                    if (list1 != null && list1.Count > 0)
                        list = list1.Select(x => new
                        {
                            ticket = new { title = x.TicketTitle, id = x.TicketID, description = x.TicketDescription, pct = x.Percentage },
                            project = new { id = x.ProjectID, title = x.ProjectTitle },
                            id = x.TimeSheetID,
                            workDetail = x.WorkDetail,
                            hours = x.Hours,
                            isMeeting = x.IsMeeting,
                            submitted = x.IsSubmitted
                        });
                    else
                        list = new List<int>();
                    context.Response.Write(JsonConvert.SerializeObject(list, DateConverter));
                    break;
                case "getticketslistbyproject":
                    int.TryParse(Request["project"], out projectId);
                    DateTime.TryParse(Request.Params["date"], out date);
                    var result = GetTicketsByProject(projectId, date);
                    if (result != null && result.ResultList != null)
                        list = result.ResultList.OrderByDescending(x=>x.TicketID).Select(x => new { title = x.Title, description = x.FullDescription, id = x.ID, pct = x.Percentage });
                    else
                        list = new List<int>();
                    context.Response.Write(JsonConvert.SerializeObject(list, DateConverter));
                    break;
                case "addtimesheet":
                    TimeSheetsEntity modelAdd = GetEntity(context, out msg);
                    if (modelAdd == null)
                        context.Response.Write(ResponseMessage.GetResponse(false, msg, 0));
                    else
                    {
                        modelAdd.ID = AddTimeSheet(modelAdd, out msg);
                        if (modelAdd.SheetDate > DateTime.Now)
                        {
                            SyncWeekPlan(modelAdd);
                        }
                        context.Response.Write(modelAdd.ID > 0
                            ? ResponseMessage.GetResponse(true, "", modelAdd.ID)
                            : ResponseMessage.GetResponse(false, msg, modelAdd.ID));
                    }
                    break;
                case "updatetimesheet":
                    TimeSheetsEntity modelUpdate = GetEntity(context, out msg);
                    if (modelUpdate == null)
                        context.Response.Write(ResponseMessage.GetResponse(false, msg, 0));
                    else
                    {
                        boolResult = UpdateTimeSheet(modelUpdate, out msg);
                        if (modelUpdate.SheetDate > DateTime.Now)
                        {
                            SyncWeekPlan(modelUpdate);
                        }
                        context.Response.Write(boolResult
                            ? ResponseMessage.GetResponse(true, "", modelUpdate.ID)
                            : ResponseMessage.GetResponse(false, msg, modelUpdate.ID));
                    }
                    break;
                case "deletetimesheet":
                    if (int.TryParse(Request["id"], out deleteID) && deleteID > 0)
                    {
                        if (DeleteTimeSheet(deleteID))
                            msg = ResponseMessage.GetResponse(true, "", deleteID);
                        else
                            msg = ResponseMessage.GetResponse(false, "", deleteID);
                        if (Convert.ToDateTime(context.Server.UrlDecode(context.Request.Params["SheetDate"])) >
                            DateTime.Now)
                        {
                            SyncWeekPlan(new TimeSheetsEntity()
                            {
                                SheetDate =
                                    Convert.ToDateTime(context.Server.UrlDecode(context.Request.Params["SheetDate"]))
                            });
                        }
                    }
                    context.Response.Write(msg);
                    break;
                case "cancelsubmit":
                    string sheeddate = Request.Params["sheetdate"];
                    string userid = Request.Params["userid"];
                    string responseMsg = string.Empty;
                    string msgcancel = string.Empty;
                    if (string.IsNullOrEmpty(sheeddate) || string.IsNullOrEmpty(userid))
                    {
                        responseMsg = ResponseMessage.GetResponse(false);
                    }
                    else
                    {
                        if (CancelSubmit(sheeddate, userid, out msgcancel))
                        {
                            responseMsg = ResponseMessage.GetResponse(true);
                        }
                        else
                        {
                            responseMsg = ResponseMessage.GetResponse(false, msgcancel, 0);
                        }
                    }
                    context.Response.Write(responseMsg);
                    break;
                case "getweekhours":
                    int userId = int.Parse(context.Request.Params["userID"]);
                    context.Response.Write(getWeekHours(userId));
                    break;
                default:
                    context.Response.Write("[]");
                    break;
            }

        }
        private decimal getWeekHours(int userId)
        {
            DateTime weekDay = DateTime.Now.Date.AddDays(7);
            int weekOfDay = (int)weekDay.DayOfWeek;
            weekOfDay = weekOfDay == 0 ? 7 : weekOfDay;
            weekDay = weekDay.AddDays(-weekOfDay + 1).AddSeconds(-1);
            DateTime oneDay = weekDay.AddDays(-6).Date;
            decimal hours = tsApp.GetTimesheetsHoursByWeek(userId, oneDay, weekDay);
            return hours;
        }
        private bool CancelSubmit(string date, string userID, out string msg)
        {
            msg = string.Empty;
            try
            {
                DateTime sheetDate = Convert.ToDateTime(date).Date;
                int id = int.Parse(userID);
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
                    msg = "Hours between 0-24 must be entered before continuing. ";
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

        private bool DeleteTimeSheet(int id)
        {
            return tsApp.DeleteTimeSheet(id);
        }
        private int AddTimeSheet(TimeSheetsEntity model, out string msg)
        {
            msg = string.Empty;
            int id = tsApp.AddTimeSheet(model);
            if (tsApp.BrokenRuleMessages.Count > 0 || id <= 0)
            {
                msg = tsApp.BrokenRuleMessages[0].Message;
            }
            return id;
        }

        public bool SyncWeekPlan(TimeSheetsEntity model)
        {
            WeekPlanApplication planApp = new WeekPlanApplication();

            DateTime weekDay = model.SheetDate;

            //DateTime weekDay = DateTime.Now.AddDays(7);
            int weekOfDay = (int)weekDay.DayOfWeek;
            weekOfDay = weekOfDay == 0 ? 7 : weekOfDay;
            weekDay = weekDay.AddDays(-weekOfDay).AddDays(7);
            List<TimeSheetTicket> listTimeSheetTicket = GetTimesheetTicket(0, model.SheetDate, model.SheetDate, false,
                UserID, 0);

            string description = "";
            string tickets = "";
            decimal hours = 0;
            foreach (var timeSheet in listTimeSheetTicket)
            {
                description += timeSheet.TicketID + ": " + timeSheet.TicketTitle + "\r\n";
                tickets += timeSheet.TicketID + "_";
                hours += timeSheet.Hours;
            }
            WeekPlanEntity entity = planApp.GetInfo(UserID, weekDay);
            if (entity == null)
            {
                entity = new WeekPlanEntity();

                entity.WeekDay = weekDay.Date;
                entity.UserID = UserInfo.UserID;
                entity.CreateDate = DateTime.Now;
                entity.UpdateDate = DateTime.Now;
                entity.UpdateUserID = UserInfo.UserID;
                entity.IsDeleted = false;
                entity.Monday = "";
                entity.MondayTickets = "";
                entity.MondayEstimate = 0;
                entity.Tuesday = "";
                entity.TuesdayTickets = "";
                entity.TuesdayEstimate = 0;
                entity.Wednesday = "";
                entity.WednesdayTickets = "";
                entity.WednesdayEstimate = 0;
                entity.Thursday = "";
                entity.ThursdayTickets = "";
                entity.ThursdayEstimate = 0;
                entity.Friday = "";
                entity.FridayTickets = "";
                entity.FridayEstimate = 0;
                entity.Saturday = "";
                entity.SaturdayTickets = "";
                entity.SaturdayEstimate = 0;
                entity.Sunday = "";
                entity.SundayTickets = "";
                entity.SundayEstimate = 0;
            }
            switch (weekOfDay)
            {
                case 1:
                    entity.Monday = description;
                    entity.MondayTickets = tickets;
                    entity.MondayEstimate = (int)hours;
                    break;
                case 2:
                    entity.Tuesday = description;
                    entity.TuesdayTickets = tickets;
                    entity.TuesdayEstimate = (int)hours;
                    break;
                case 3:
                    entity.Wednesday = description;
                    entity.WednesdayTickets = tickets;
                    entity.WednesdayEstimate = (int)hours;
                    break;
                case 4:
                    entity.Thursday = description;
                    entity.ThursdayTickets = tickets;
                    entity.ThursdayEstimate = (int)hours;
                    break;
                case 5:
                    entity.Friday = description;
                    entity.FridayTickets = tickets;
                    entity.FridayEstimate = (int)hours;
                    break;
                case 6:
                    entity.Saturday = description;
                    entity.SaturdayTickets = tickets;
                    entity.SaturdayEstimate = (int)hours;
                    break;
                case 7:
                    entity.Sunday = description;
                    entity.SundayTickets = tickets;
                    entity.SundayEstimate = (int)hours;
                    break;
            }

            if (entity.ID > 0)
            {
                return planApp.Update(entity, true);
            }
            else
            {
                return planApp.Add(entity) > 0;
            }
        }

        private bool UpdateTimeSheet(TimeSheetsEntity model, out string msg)
        {
            msg = string.Empty;
            var result = tsApp.UpdateTimeSheet(model);
            if (tsApp.BrokenRuleMessages.Count > 0)
            {
                msg = tsApp.BrokenRuleMessages[0].Message;
            }
            return result;
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