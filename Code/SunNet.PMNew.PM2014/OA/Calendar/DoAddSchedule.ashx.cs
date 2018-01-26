using SunNet.PMNew.App;
using SunNet.PMNew.Entity.SchedulesModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.PM2014.OA.Calendar
{
    /// <summary>
    /// Summary description for DoAddSchedule
    /// </summary>
    public class DoAddSchedule : DoBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string requestType = request.QueryString["type"];
            string startTime = request.QueryString["startTime"];
            string endTime = request.QueryString["endTime"];
            DateTime date;
            string title = request.Form["title"];

            string description = request.Form["description"];
            bool isMeeting = request.Form["isMeeting"] == "true" ? true : false;

            string allUsers = request.Form["meetingUserIds"];

            if (requestType == "savevalidate")
            {
                if (!DateTime.TryParse(request.QueryString["Date"], out date))
                {
                    response.Write("params error.");
                    response.End();
                }
                response.Write(IsValidSchedule(date, startTime, endTime));
                response.End();
            }
            else if (requestType == "editvalidate")
            {
                string id = request.QueryString["id"];
                int idResult;
                if (!int.TryParse(id, out idResult))
                {
                    response.Write("params error.");
                    response.End();
                }
                SchedulesEntity entity = new App.SchedulesApplication().GetInfo(int.Parse(id));
                response.Write(IsValidEditSchedule(entity, startTime, endTime));
                response.End();
            }
            else if (requestType == "save")
            {
                if (string.IsNullOrEmpty(title.NoHTML()))
                {
                    response.Write("params error.");
                    response.End();
                }

                if (!DateTime.TryParse(request.QueryString["Date"], out date))
                {
                    response.Write("params error.");
                    response.End();
                }
                response.Write(Save(title, description, date, startTime, endTime, isMeeting, allUsers.Split(',').ToList<string>()));
            }
            else if (requestType == "edit")
            {

                string id = request.QueryString["id"];
                SchedulesEntity entity = new App.SchedulesApplication().GetInfo(int.Parse(id));
                if (string.IsNullOrEmpty(id))
                {
                    response.Write("params error.");
                    response.End();
                }
                if (string.IsNullOrEmpty(title.NoHTML()))
                {
                    response.Write("params error.");
                    response.End();
                }
                response.Write(Edit(title, description, entity, startTime, endTime, isMeeting, allUsers.Split(',').ToList<string>()));
            }
            else if (requestType == "agree")
            {
                string id = request.QueryString["id"];
                SchedulesEntity entity = new App.SchedulesApplication().GetInfo(int.Parse(id));
                if (string.IsNullOrEmpty(id))
                {
                    response.Write("params error.");
                    response.End();
                }
                response.Write(dealMeeting(id, "agree"));
            }
            else if (requestType == "disagree")
            {

                string id = request.QueryString["id"];
                SchedulesEntity entity = new App.SchedulesApplication().GetInfo(int.Parse(id));
                if (null == entity)
                {
                    response.Write("params error.");
                    response.End();
                }
                response.Write(dealMeeting(id, "disagree"));
            }
            else if (requestType == "delete")
            {
                string id = request.QueryString["id"];
                response.Write(Delete(id));
            }
            response.End();
        }


        /// <summary>
        /// 主要用于验证是否有时间上的冲突
        /// </summary>
        /// <param name="date">哪一天的schedule</param>
        /// <param name="startTime">schedule的开始时间</param>
        /// <param name="endTime">schedule的结束时间</param>
        /// <returns></returns>
        public string IsValidSchedule(DateTime date, string startTime, string endTime)
        {
            if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
            {
                return "params error.";
            }

            int start = ScheduleTimeHelpers.TimeHandle(startTime);
            int end = ScheduleTimeHelpers.TimeHandle(endTime);

            if (start > end)
            {
                return "Please entry again , Starting time should not exceed finishing time.";
            }

            if (DateTime.Parse(string.Format("{0} {1}", date.ToString("MM/dd/yyyy"), startTime)) < DateTime.Now)
            {
                return "Plan time not less than the current time.";
            }

            List<SchedulesEntity> schedulesList = new App.SchedulesApplication().GetSchedules(date, UserID);
            List<ScheduleTimeEntity> scheduleTimeList = new ScheduleTimeHelpers(schedulesList).Times;
            for (int i = start; i < end; i++)
            {
                ScheduleTimeEntity t = scheduleTimeList.Find(r => r.Cell == i && r.IsPlan);
                if (t != null)
                {
                    return "-1";
                }
            }
            return "1";
        }

        public string IsValidEditSchedule(SchedulesEntity entity, string startTime, string endTime)
        {
            if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
            {
                return "params error.";
            }

            int start = ScheduleTimeHelpers.TimeHandle(startTime);
            int end = ScheduleTimeHelpers.TimeHandle(endTime);

            if (start > end)
            {
                return "Please entry again , Starting time should not exceed finishing time.";
            }

            if (DateTime.Parse(string.Format("{0} {1}", entity.PlanDate.ToString("MM/dd/yyyy"), startTime)) < DateTime.Now)
            {
                return "Plan time not less than the current time.";
            }

            if (!(entity.StartTime == startTime && entity.EndTime == endTime))
            {
                List<SchedulesEntity> schedulesList = new App.SchedulesApplication().GetSchedules(entity.PlanDate, UserID);
                List<ScheduleTimeEntity> scheduleTimeList = new ScheduleTimeHelpers(schedulesList).Times;
                for (int i = start; i < end; i++)
                {
                    ScheduleTimeEntity t = scheduleTimeList.Find(r => r.Cell == i && r.IsPlan);
                    if (t != null)
                    {
                        return "-1";
                    }
                }
            }
            return "1";
        }

        private string Save(string title, string description, DateTime date, string startTime
            , string endTime, bool isMeeting, List<string> meetingUserIDs)
        {
            SchedulesEntity entity = new SchedulesEntity();
            entity.Title = title.NoHTML();
            entity.Description = description.NoHTML();
            entity.CreateBy = UserID;
            entity.CreateOn = DateTime.Now;
            entity.StartTime = startTime;
            entity.EndTime = endTime;
            entity.UpdateBy = UserID;
            entity.UpdateOn = DateTime.Now;
            entity.PlanDate = date;

            string result = IsValidSchedule(date, startTime, endTime);
            if (result != "1" && result != "-1")
            {
                return result;
            }

            if (isMeeting)
            {
                entity.MeetingID = string.Format("{0}:{1}", UserID, DateTime.Now.ToString("MM_dd_yy_HH:mm_ss"));
                entity.MeetingStatus = 1;
                List<UsersEntity> usersList = new List<UsersEntity>();
                foreach (string userID in meetingUserIDs)
                {
                    usersList.Add(new App.UserApplication().GetUser(int.Parse(userID)));
                }
                UsersEntity userInfo = new App.UserApplication().GetUser(UserID);
                if (usersList.Find(r => r.UserID == UserID) == null) //如果不包括创建者，就添加他
                {
                    usersList.Add(userInfo);
                }
                if (new App.SchedulesApplication().Add(entity, usersList, userInfo) > 0)
                {
                    return "1";
                }
                else
                {
                    return "Operation failed.";
                }

            }
            else
            {
                entity.MeetingID = string.Empty;
                entity.UserID = UserID;

                if (new App.SchedulesApplication().Add(entity) > 0)
                {
                    return "1";
                }
                else
                {
                    return "Operation failed.";
                }
            }
        }

        private string Edit(string title, string description, SchedulesEntity entity, string startTime
            , string endTime, bool isMeeting, List<string> meetingUserIDs)
        {

            entity.Title = title.NoHTML();
            entity.Description = description.NoHTML();
            entity.StartTime = startTime;
            entity.EndTime = endTime;
            entity.UpdateBy = UserID;
            entity.UpdateOn = DateTime.Now;

            string result = IsValidEditSchedule(entity, startTime, endTime);
            if (result != "1" && result != "-1")
            {
                return result;
            }

            if (isMeeting)
            {
                List<UsersEntity> usersList = new List<UsersEntity>();
                foreach (string userID in meetingUserIDs)
                {
                    usersList.Add(new App.UserApplication().GetUser(int.Parse(userID)));
                }
                UsersEntity userInfo = new App.UserApplication().GetUser(UserID);
                if (usersList.Find(r => r.UserID == UserID) == null) //如果不包括创建者，就添加他
                {
                    usersList.Add(userInfo);
                }

                List<UsersEntity> moveSales = new App.SchedulesApplication().GetMeetingUsers(entity.MeetingID);
                List<UsersEntity> newList = new List<UsersEntity>();
                UsersEntity tmpUser;

                foreach (UsersEntity tmp in usersList)
                {
                    tmpUser = moveSales.Find(r => r.UserID == tmp.UserID);
                    if (tmpUser != null)
                        moveSales.Remove(tmpUser);
                    else
                        newList.Add(tmp);
                }

                if (new App.SchedulesApplication().Update(entity, moveSales, newList))
                {
                    return "1";
                }
                else
                    return "Operation failed.";
            }
            else
            {
                if (new App.SchedulesApplication().Update(entity))
                {
                    return "1";
                }
                else
                    return "Operation failed.";
            }
        }

        protected string dealMeeting(string ID, string type)
        {
            bool dealResult = false;
            string result = string.Empty;
            SchedulesEntity entity = new App.SchedulesApplication().GetInfo(int.Parse(ID));
            if (entity != null && entity.UserID == UserID)
            {
                UsersEntity userInfo = new App.UserApplication().GetUser(UserID);
                if (type == "agree")
                {
                    dealResult = new App.SchedulesApplication().AgreeMeeting(entity, DateTime.Now.Date, userInfo);
                }
                else
                {
                    dealResult = new App.SchedulesApplication().VoteMeeting(entity, DateTime.Now.Date, userInfo);
                }
                if (dealResult)
                {
                    result = "1";
                    //   Response.Redirect(string.Format("EditSchedules.aspx?id={0}&target={1}", ID, userInfo.UserID));
                }
                else
                {
                    result = "Operation failed.";
                }
            }
            else
            {
                result = "unauthorized access.";
            }
            return result;
        }

        protected string Delete(string ID)
        {
            //当前用户为创建用户且meeting状态没有approve
            SchedulesEntity entity = new App.SchedulesApplication().GetInfo(int.Parse(ID));
            if (entity.CreateBy == UserID && (entity.MeetingStatus == 1 || string.IsNullOrEmpty(entity.MeetingID)) && entity.PlanDate >= DateTime.Now.Date)
            {
                SchedulesApplication schedulesApplication = new SchedulesApplication();
                if (string.IsNullOrEmpty(entity.MeetingID))
                {
                    if (schedulesApplication.Delete(entity.ID))
                    {
                        return "1";
                    }
                    else
                    {
                        return "Operation failed.";
                    }
                }
                else
                {
                    if (schedulesApplication.DeleteMeetingSchedule(entity.MeetingID))
                    {
                        UsersEntity userInfo = new UserApplication().GetUser(UserID);
                        if (schedulesApplication.VoteMeeting(entity, DateTime.Now.Date, userInfo))
                        {
                            return "1";
                        }
                        else
                        {
                            return "Operation failed.";
                        }
                    }
                    else
                    {
                        return "Operation failed.";
                    }

                }
            }
            else
            {
                return "unauthorized access.";
            }

            //List<SchedulesEntity> list = new App.SchedulesApplication().GetSchedules(entity.PlanDate, targetUserId);
            //jscheduleResult = SerializeScheduleList(list);
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