using SunNet.PMNew.App;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Framework.Utils;
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
            // Contactor 不能访问该模块
            // if (UserInfo.Role == Entity.UserModel.RolesEnum.Contactor) return; 从Dashboard访问
            ProcessDashBoardCalendar(context);
        }


        /// <summary>
        /// Client portal Dashboard 界面调用
        /// </summary>
        /// <param name="context"></param>
        private void ProcessDashBoardCalendar(HttpContext context)
        {
            int? t = null;
            int tmp;

            int dataCount = 4;
            if (context.Request.QueryString["dashboard"] + "" != string.Empty)
                dataCount = 2;

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
            if (!int.TryParse(context.Request.QueryString["projectID"], out projectID))
                projectID = -1;


            int userId = 0;
            if (!int.TryParse(context.Request.QueryString["userId"], out userId))
            {
                context.Response.Write("[]");
                context.Response.End();
                return;
            }
            string allUsers = string.Empty;
            if (userId == 0)
            {
                allUsers = context.Request.QueryString["alluser"] + "";
                allUsers = allUsers.Trim();
                if (allUsers == string.Empty)
                {
                    context.Response.Write("[]");
                    context.Response.End();
                    return;
                }
            }

            EventsApplication eventsApplication = new EventsApplication();
            List<EventCalendar> list = new List<EventCalendar>();

            if (d == null && t == null)
            {
                int year = 0;
                int month = 0;

                if (int.TryParse(context.Request.QueryString["year"] + "", out tmp))
                    year = tmp;
                else year = DateTime.Now.Year;

                if (int.TryParse(context.Request.QueryString["month"] + "", out tmp))
                    month = tmp;
                else month = DateTime.Now.Month;

                DateTime date = DateTime.Now;
                if (DateTime.TryParse(string.Format("{0}/1/{1}", month, year), out date) == false)
                    date = DateTime.Now;

                list = eventsApplication.GetEventCalendar(UserInfo.UserID, date, userId, allUsers, UserInfo.Role, projectID, dataCount);
            }
            if (d != null && t != null)
            {
                DateTime date = (DateTime)d;
                if (t == 1)
                {
                    if (y == null)
                        list = eventsApplication.GetEventCalendar(UserInfo.UserID, date.AddMonths(1), userId, allUsers, UserInfo.Role, projectID, dataCount);
                    else
                    {
                        if (y == 1)//year
                            list = eventsApplication.GetEventCalendar(UserInfo.UserID, date.AddYears(1), userId, allUsers, UserInfo.Role, projectID, dataCount);
                        else//month
                            list = eventsApplication.GetEventCalendar(UserInfo.UserID, date.AddMonths(1), userId, allUsers, UserInfo.Role, projectID, dataCount);
                    }
                }
                else if (t == 2)
                    if (y == null)
                        list = eventsApplication.GetEventCalendar(UserInfo.UserID, date.AddMonths(-1), userId, allUsers, UserInfo.Role, projectID, dataCount);
                    else
                    {
                        if (y == 1)//year
                            list = eventsApplication.GetEventCalendar(UserInfo.UserID, date.AddYears(-1), userId, allUsers, UserInfo.Role, projectID, dataCount);
                        else
                            list = eventsApplication.GetEventCalendar(UserInfo.UserID, date.AddMonths(-1), userId, allUsers, UserInfo.Role, projectID, dataCount);
                    }
                else
                    list = eventsApplication.GetEventCalendar(UserInfo.UserID, date, userId, allUsers, UserInfo.Role, projectID, 2); //刷新当前数据
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