using SunNet.PMNew.App;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Web.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for DoGetCalendarList
    /// </summary>
    public class DoGetCalendarList : DoBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (string.IsNullOrEmpty(context.Request.QueryString["dashboard"]))
            {
                ProcessCalendar(context);
            }
            else
            {
                ProcessDashBoardCalendar(context);
            }
        }

        private void ProcessCalendar(HttpContext context)
        {
            int parsedInt;
            int? year = null;
            int? month = null;

            int projectID = -1;

            if (int.TryParse(context.Request.QueryString["year"], out parsedInt))
            {
                year = parsedInt;
            }
            if (int.TryParse(context.Request.QueryString["month"], out parsedInt))
            {
                month = parsedInt;
            }
            if (!string.IsNullOrEmpty(context.Request.QueryString["projectID"]))
            {
                projectID = int.Parse(context.Request.QueryString["projectID"]);
            }

            DateTime monthToDisplay;
            if (month == null && year == null)
            {
                monthToDisplay = DateTime.Now;
            }
            else
            {
                monthToDisplay = DateTime.Parse(month + "/" + "1" + "/" + year);
            }

            EventsApplication eventsApplication = new EventsApplication();
            List<EventCalendar> list = new List<EventCalendar>();

            list = eventsApplication.GetEventCalendar(monthToDisplay, UserID, UserInfo.Role, projectID);

            bool isOwner = false;

            if (UserID == 0)
            {
                context.Response.Write("0");
                context.Response.End();
            }

            isOwner = UserID == UserID;

            //判断当前选择的user是不是 当前登陆的user，返回一个bool值.
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("list", list);
            result.Add("isOwner", isOwner);
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result));
            context.Response.End();
        }

        private void ProcessDashBoardCalendar(HttpContext context)
        {
            int? t = null;
            int tmp;
            if (int.TryParse(context.Request.QueryString["t"], out tmp))
            {
                t = tmp;
            }

            int? y = null;
            if (int.TryParse(context.Request.QueryString["y"], out tmp))
            {
                y = tmp;
            }
            DateTime? d = null;
            DateTime tmpDate;
            if (DateTime.TryParse(context.Request.QueryString["d"], out tmpDate))
            {
                d = tmpDate;
            }
            int projectID = -1;
            if (int.TryParse(context.Request.QueryString["projectID"], out tmp))
            {
                projectID = tmp;
            }
            EventsApplication eventsApplication = new EventsApplication();
            List<EventCalendar> list = new List<EventCalendar>();

            if (d == null && t == null)
            {
                list = eventsApplication.GetEventCalendar(DateTime.Now, UserID, UserInfo.Role, projectID);
            }
            if (d != null && t != null)
            {
                DateTime date = (DateTime)d;
                if (t == 1)
                {
                    if (y == null)
                        list = eventsApplication.GetEventCalendar(date.AddMonths(1), UserID, UserInfo.Role, projectID);
                    else
                    {
                        if (y == 1)//year
                            list = eventsApplication.GetEventCalendar(date.AddYears(1), UserID, UserInfo.Role, projectID);
                        else//month
                            list = eventsApplication.GetEventCalendar(date.AddMonths(1), UserID, UserInfo.Role, projectID);
                    }
                }
                else if (t == 2)
                    if (y == null)
                        list = eventsApplication.GetEventCalendar(date.AddMonths(-1), UserID, UserInfo.Role, projectID);
                    else
                    {
                        if (y == 1)//year
                            list = eventsApplication.GetEventCalendar(date.AddYears(-1), UserID, UserInfo.Role, projectID);
                        else
                            list = eventsApplication.GetEventCalendar(date.AddMonths(-1), UserID, UserInfo.Role, projectID);
                    }
                else
                    list = eventsApplication.GetEventCalendar(date, UserID, UserInfo.Role, projectID); //刷新当前数据
            }

            if (UserID == 0)
            {
                context.Response.Write("0");
                context.Response.End();
            }

            //判断当前选择的user是不是 当前登陆的user，返回一个bool值.
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("list", list);
            result.Add("isOwner", true);
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result));
            context.Response.End();
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