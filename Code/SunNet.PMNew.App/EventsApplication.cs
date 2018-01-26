using StructureMap;
using SunNet.PMNew.Core.EventsModule;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.App
{
    public class EventsApplication : BaseApp
    {
        EventsManager mgr;
        TimeSheetApplication tsp;

        public EventsApplication()
        {
            mgr = new EventsManager(ObjectFactory.GetInstance<IEventRepository>(), ObjectFactory.GetInstance<IEventCommentsRepository>());
            tsp = new TimeSheetApplication();
        }

        #region construct calendar

        /// <summary>
        /// 日历模块显示数据，获取数据源
        /// </summary>
        public List<EventCalendar> GetEventCalendar(int currentUserId, DateTime date, int userId, string allUser, RolesEnum role, int projectID, int dateCount = 4)
        {
            DateTime startDate = DateTime.Parse(string.Format("{0}/1/{1}", date.Month, date.Year));
            //开始时间往前推一周，结束时间往后推一周
            List<EventEntity> list = mgr.GetEvents(currentUserId, startDate.AddDays(-7), startDate.AddMonths(1).AddDays(7), userId, allUser, projectID);
            return GetList(list, date, userId, dateCount);
        }

        private List<EventCalendar> GetList(List<EventEntity> eventList, DateTime date, int userId, int dateCount)
        {
            List<EventCalendar> list = new List<EventCalendar>();
            DateTime head = DateTime.Parse(string.Format("{0}/01/{1}", date.Month, date.Year));
            DateTime start = head;

            if (head.DayOfWeek != DayOfWeek.Sunday) //1号不是星期天
            {
                start = head.AddDays(0 - (int)head.DayOfWeek);
            }

            DateTime monthEnd = head.AddMonths(1).AddDays(-1);
            DateTime end = monthEnd;
            if (monthEnd.DayOfWeek != DayOfWeek.Saturday) //月末不是星期六
            {
                end = monthEnd.AddDays(6 - (int)monthEnd.DayOfWeek);
            }

            while (start <= end)
            {
                EventCalendar eventCalendar = new EventCalendar();
                eventCalendar.Date = start;
                if (start == DateTime.Now.Date)
                {
                    if (start < head) //上个月的日子，灰色不可添加
                    {
                        eventCalendar.Type = 0;
                    }
                    else if (start > end)
                        eventCalendar.Type = 4; //下个月的日子，灰色可添加
                    else//可以添加
                    {
                        eventCalendar.Type = 3;
                    }
                }
                else if (start > DateTime.Now.Date)
                {
                    if (start > monthEnd) //下个月的日子，灰色可添加
                    {
                        eventCalendar.Type = 4;
                    }
                    else if (start < head) //上个月的日子，灰色不可添加
                    {
                        eventCalendar.Type = 4;
                    }
                    else //本月将来的日子，可添加
                        eventCalendar.Type = 2;
                }
                else
                {
                    if (start < head) //上个月的日子，灰色不可添加
                    {
                        eventCalendar.Type = 0;
                    }
                    else if (start > monthEnd)
                    {
                        eventCalendar.Type = 0;
                    }
                    else //本月已过去的日子，不可添加
                    {
                        eventCalendar.Type = 1;
                    }
                }
                eventCalendar.list = BuilderList(eventList, start, userId, dateCount);
                //if (eventCalendar.list.Count == dateCount)
                //{
                //    eventCalendar.MoreEvent = true;
                //    eventCalendar.list.RemoveAt(3);
                //}
                list.Add(eventCalendar);
                start = start.AddDays(1);
            }
            return list;
        }


        List<CalendarView> BuilderList(List<EventEntity> eventList, DateTime start, int userId, int dateCount)
        {
            List<CalendarView> list = new List<CalendarView>();
            int count = 1;
            foreach (EventEntity entity in eventList.FindAll(r => r.FromDay.Date == start.Date))
            {
                ///每个日期格子只显示 dateCount 条记录
                if (count > dateCount) break;
                count++;
                int inviteStatus = 0;

                UsersEntity user = null;
                list.Add(new CalendarView()
                {
                    ID = entity.ID,
                    Title = BuilderTitle(entity),
                    Name = entity.Name,
                    Icon = EventIconAgent.BuidlerIcon(entity.Icon),
                    date = BuilderTime(entity),
                    CreatedAt = entity.CreatedBy,
                    Invited = user != null,
                    FullName = user == null ? "" : string.Format("{0} {1}", user.FirstName, user.LastName),
                    IsEdit = entity.FromDay >= DateTime.Now.Date,
                    InviteStatus = inviteStatus,
                    Times = entity.Times
                });
            }
            return list;
        }

        string BuilderTime(EventEntity entity)
        {
            if (entity.AllDay)
                return entity.FromDay.ToString("MMMM d, yyyy");
            else
                return string.Format("{0} at {1}{2}"
                    , entity.FromDay.ToString("MMMM d, yyyy"), entity.FromTime, entity.FromTimeType == 1 ? "AM" : "PM");
        }

        string BuilderTitle(EventEntity entity)
        {
            if (entity.AllDay)
                return string.Format("{0} {1}", "(All day)", entity.Name);
            else
                return string.Format("{0}{1} {2}", entity.FromTime, entity.FromTimeType == 1 ? "AM" : "PM", entity.Name);
        }

        #endregion

        #region construct List

        public List<UpcomingEvent> GetList(List<EventEntity> eventList, DateTime date, int userId, bool more, int top)
        {
            List<UpcomingEvent> list = new List<UpcomingEvent>();
            if (date.Date == DateTime.Now.Date)
                list.Add(new UpcomingEvent()
                {
                    Day = date.Date,
                    Date = "Today",
                    list = new List<ListView>()
                });

            int index = 1;
            foreach (EventEntity entity in eventList)
            {
                if (index == top) break;
                index++;
                UpcomingEvent upcomingEvent = list.Find(r => r.Day == entity.FromDay.Date);
                if (upcomingEvent == null)
                {
                    upcomingEvent = new UpcomingEvent();
                    upcomingEvent.Day = entity.FromDay.Date;
                    upcomingEvent.Date = string.Format("{0}, {1}",
                            upcomingEvent.Day.DayOfWeek.ToString(), upcomingEvent.Day.ToString("MMMM d, yyyy"));
                    upcomingEvent.list = new List<ListView>();
                    upcomingEvent.list.Add(BuilderView(entity, userId));
                    list.Add(upcomingEvent);
                }
                else
                    upcomingEvent.list.Add(BuilderView(entity, userId));
            }
            if (date.Date == DateTime.Now.Date)
            {
                UpcomingEvent upcomingEvent = list.Find(r => r.Day == date.Date);
                if (more)
                    upcomingEvent.MoreDate = eventList[top - 1].FromDay.ToString("MM/dd/yyyy HH:mm");
                else
                    upcomingEvent.MoreDate = "";
            }
            else
            {
                if (more && list.Count > 0)
                {
                    list[0].MoreDate = eventList[top - 1].FromDay.ToString("MM/dd/yyyy HH:mm");
                }
                else
                {
                    if (list.Count > 0)
                    {
                        list[0].MoreDate = "";
                    }
                }
            }
            return list;
        }

        ListView BuilderView(EventEntity entity, int userId)
        {
            UsersEntity user = null;
            int inviteStatus = 0;
            if (entity.CreatedBy != userId)
            {
                user = new UserApplication().GetUser(entity.CreatedBy);
            }
            return new ListView()
            {
                ID = entity.ID,
                Name = entity.Name,
                Icon = EventIconAgent.BuidlerIcon(entity.Icon),
                Time = BuilderListTime(entity),
                CreatedBy = entity.CreatedBy,
                Invited = user != null,
                //LittleHeadImage = user == null ? "" : user.,
                FullName = user == null ? "" : string.Format("{0} {1}", user.FirstName, user.LastName),
                InviteStatus = inviteStatus
            };
        }

        string BuilderListTime(EventEntity entity)
        {
            if (entity.AllDay) return "All-day";
            return string.Format("{0}{1} - {2}{3}", entity.FromTime, entity.FromTimeType == 1 ? "AM" : "PM"
                , entity.ToTime, entity.ToTimeType == 1 ? "AM" : "PM"
                );
        }

        public List<UpcomingEvent> GetEventList(DateTime date, int userId, int projectID, int top)
        {
            List<EventEntity> list = mgr.GetEvents(date, userId, projectID, top);
            return GetList(list, date.Date, userId, list.Count == top, top);
        }

        public List<UpcomingEvent> GetEventList(int currentUserId, DateTime date, int userId, string allUser, int projectID, int pageSize, int pageNo, out int recordCount)
        {
            List<EventEntity> list = mgr.GetEvents(currentUserId, date, userId, allUser, projectID, pageSize, pageNo, out recordCount);
            return GetList(list, date.Date, currentUserId, list.Count == pageSize, pageSize);
        }
        #endregion construct List

        EventEntity AddEvent(EventsView entity, List<EventInviteEntity> inviteList)
        {
#if !DEBUG
            using (TransactionScope tran = new TransactionScope())
#endif
            {
                if (entity.Repeat != RepeatType.None && entity.End == EndType.on_date && entity.Times <= 1)
                {
                    var times = entity.Times > -1 ? entity.Times : 0;
                    DateTime start = entity.FromDay;
                    while (start < entity.EndDate)
                    {
                        times++;
                        // calc next event date
                        switch (entity.Repeat)
                        {
                            case RepeatType.Every_Day:
                                start = start.AddDays(1);
                                break;
                            case RepeatType.Every_Week:
                                start = start.AddDays(7);
                                break;
                            case RepeatType.Every_two_weeks:
                                start = start.AddDays(2 * 7);
                                break;
                            case RepeatType.Every_Month:
                            case RepeatType.Every_Month_First_Friday:
                                start = start.AddMonths(1);
                                break;
                            case RepeatType.Every_Year:
                                start = start.AddYears(1);
                                break;
                        }
                    }
                    entity.Times = times;
                }
                //entity.CreatedOn = DateTime.Now;
                entity.UpdatedOn = DateTime.Now;
                entity.Highlight = false;

                bool result = false;
                entity.ID = mgr.AddEvents(entity, entity.Times, entity.IsOff);
                if (entity.ID > 0)
                {
                    result = mgr.AddEventInvites(entity, inviteList);
                }
                if (entity.IsOff)//如果选择了OFF的ticket,则添加TimeSheets
                {
                    result = AddTimeSheetsWithEvent(entity);
                }
#if !DEBUG
                        if(result)
                            tran.Complete();
#endif
                if (result)
                    return entity;
                else
                    return null;
            }
        }

        public bool AddEvents(EventsView entity, List<EventInviteEntity> inviteList, out List<EventEntity> eventList)
        {
            eventList = new List<EventEntity>();

            if (entity.Icon == 0) entity.Icon = 1;

            if (!entity.AllDay)
                entity.FromDay = DateTime.Parse(string.Format("{0}/{1}/{2} {3} {4}"
                , entity.FromDay.Month, entity.FromDay.Day, entity.FromDay.Year, entity.FromTime, entity.FromTimeType == 1 ? "AM" : "PM"));

            if (entity.Repeat == RepeatType.None)
            {
                EventEntity eventEntity = AddEvent(entity, inviteList);

                if (eventEntity != null)
                {
                    eventList.Add(eventEntity);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                bool result = false;

                if (entity.End == EndType.After_number_of_times) //重复次数
                {
                    #region
                    DateTime tmpFromDay = entity.FromDay;
                    string tmpToTime = entity.ToTime;
                    int tmpToTimeType = entity.ToTimeType;
                    bool isSameDay = entity.FromDay.Date == entity.ToDay.Date;//开始日期和结束日期是否为同一天
#if !DEBUG
            using (TransactionScope tran = new TransactionScope())
#endif
                    {
                        for (int i = 0; i < entity.Times; i++)
                        {
                            if (entity.Repeat == RepeatType.Every_Month_First_Friday)
                            {
                                entity.FromDay = GetNextDateForRepeat7(tmpFromDay);
                            }
                            else
                            {
                                entity.FromDay = tmpFromDay;
                            }
                            entity.ToDay = tmpFromDay;
                            if (!isSameDay) //如果不是同一天，FromTime和ToTime要做处理
                            {
                                //若选择的日期为04/14/2015 1:00 pm 到 04/15/2015 6:00 am,
                                //则处理成 4.14号为 04/14/2015 1:00 pm 到 04/14/2015 11:59 pm，
                                //         4.15号为 04/15/2015 12:00 am 到 04/15/2015 6:00 am
                                if (i == entity.Times - 1)
                                {
                                    entity.ToTime = tmpToTime;
                                    entity.ToTimeType = tmpToTimeType;
                                }
                                else
                                {
                                    entity.ToTime = "11:59";
                                    entity.ToTimeType = 2;
                                }
                                if (i > 0)
                                {
                                    entity.FromTime = "12:00";
                                    entity.FromTimeType = 1;
                                }
                            }

                            EventEntity eventEntity = AddEvent(entity, inviteList);
                            result = eventEntity != null;

                            eventList.Add(new EventEntity()
                            {
                                Name = entity.Name,
                                Where = entity.Where,
                                Details = entity.Details,
                                ProjectID = entity.ProjectID,
                                FromDay = entity.FromDay,
                                FromTime = entity.FromTime,
                                FromTimeType = entity.FromTimeType,
                                ToDay = entity.ToDay,
                                ToTime = entity.ToTime,
                                ToTimeType = entity.ToTimeType,
                                AllDay = entity.AllDay
                            });

                            // calc next event date
                            switch (entity.Repeat)
                            {
                                case RepeatType.Every_Day:
                                    tmpFromDay = tmpFromDay.AddDays(1);
                                    break;
                                case RepeatType.Every_Week:
                                    tmpFromDay = tmpFromDay.AddDays(7);
                                    break;
                                case RepeatType.Every_two_weeks:
                                    tmpFromDay = tmpFromDay.AddDays(2 * 7);
                                    break;
                                case RepeatType.Every_Month:
                                case RepeatType.Every_Month_First_Friday:
                                    tmpFromDay = tmpFromDay.AddMonths(1);
                                    break;
                                case RepeatType.Every_Year:
                                    tmpFromDay = tmpFromDay.AddYears(1);
                                    break;
                            }
                            if (!result) return false;
                        }
#if !DEBUG
                        if(result)
                            tran.Complete();
#endif
                    }
                    #endregion
                }
                else
                {
                    #region
                    DateTime tmpFromDay = entity.FromDay;
                    string tmpToTime = entity.ToTime;
                    int tmpToTimeType = entity.ToTimeType;
                    bool isSameDay = entity.FromDay.Date == entity.ToDay.Date;//开始日期和结束日期是否为同一天

                    int tmpIndex = 0;

                    while (entity.FromDay < entity.EndDate.Date) //截止时间
                    {
#if !DEBUG
            using (TransactionScope tran = new TransactionScope())
#endif
                        {
                            switch (entity.Repeat)
                            {
                                case RepeatType.Every_Day:
                                    entity.FromDay = tmpFromDay.AddDays(tmpIndex);
                                    entity.ToDay = entity.FromDay;
                                    break;
                                case RepeatType.Every_Week:
                                    entity.FromDay = tmpFromDay.AddDays(tmpIndex * 7);
                                    entity.ToDay = entity.FromDay;
                                    break;
                                case RepeatType.Every_two_weeks:
                                    entity.FromDay = tmpFromDay.AddDays(2 * 7 * tmpIndex);
                                    entity.ToDay = entity.FromDay;
                                    break;
                                case RepeatType.Every_Month:
                                    entity.FromDay = tmpFromDay.AddMonths(1 * tmpIndex);
                                    entity.ToDay = entity.FromDay;
                                    break;
                                case RepeatType.Every_Month_First_Friday:
                                    if (tmpIndex == 0)
                                    {
                                        entity.FromDay = GetNextDateForRepeat7(tmpFromDay);
                                    }
                                    else
                                    {
                                       
                                        entity.FromDay = GetNextDateForRepeat7(tmpFromDay.AddMonths(1 * tmpIndex));
                                        if (entity.FromDay > entity.EndDate.Date)
                                        {
                                            continue;
                                        }
                                    }
                                    entity.ToDay = entity.FromDay;
                                    break;
                                case RepeatType.Every_Year:
                                    entity.FromDay = tmpFromDay.AddYears(1 * tmpIndex);
                                    entity.ToDay = entity.FromDay;
                                    break;
                            }
                            if (!isSameDay) //如果不是同一天，FromTime和ToTime要做处理
                            {
                                //若选择的日期为04/14/2015 1:00 pm 到 04/15/2015 6:00 am,
                                //则处理成 4.14号为 04/14/2015 1:00 pm 到 04/14/2015 11:59 pm，
                                //         4.15号为 04/15/2015 12:00 am 到 04/15/2015 6:00 am
                                if (entity.FromDay.Date >= entity.EndDate.Date)
                                {
                                    entity.ToTime = tmpToTime;
                                    entity.ToTimeType = tmpToTimeType;
                                }
                                else
                                {
                                    entity.ToTime = "11:59";
                                    entity.ToTimeType = 2;
                                }

                                if (tmpIndex > 0)
                                {
                                    entity.FromTime = "12:00";
                                    entity.FromTimeType = 1;
                                }
                            }
                            EventEntity eventEntity = AddEvent(entity, inviteList);
                            result = eventEntity != null;
                            eventList.Add(eventEntity);
                            if (result == false)
                                return false;
                            tmpIndex++;
#if !DEBUG
                           if(result)
                            tran.Complete();
#endif
                        }
                    }

                    #endregion
                }
                return result;
            }
        }
        public DateTime GetNextDateForRepeat7(DateTime tmpFromDay)
        {
            var dt = GetFirstFridayByYearAndMonth(tmpFromDay);
            if (DateTime.Parse(tmpFromDay.ToString("yyyy-MM-dd")) > dt)
            {
                return GetFirstFridayByYearAndMonth(tmpFromDay.AddMonths(1));
            }
            else
            {
                return dt;
            }
        }
        /// <summary>
        /// 根据年月获取第一个星期5
        /// </summary>
        /// <param name="tmpFromDay"></param>
        /// <returns></returns>
        public DateTime GetFirstFridayByYearAndMonth(DateTime tmpFromDay)
        {
            var monthfirstdate = DateTime.Parse(tmpFromDay.ToString("yyyy-MM-01"));
            switch (monthfirstdate.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return monthfirstdate.AddDays(5);
                case DayOfWeek.Monday:
                    return monthfirstdate.AddDays(4);
                case DayOfWeek.Tuesday:
                    return monthfirstdate.AddDays(3);
                case DayOfWeek.Wednesday:
                    return monthfirstdate.AddDays(2);
                case DayOfWeek.Thursday:
                    return monthfirstdate.AddDays(1);
                case DayOfWeek.Friday:
                    return monthfirstdate;
                case DayOfWeek.Saturday:
                    return monthfirstdate.AddDays(6);
            }
            return monthfirstdate;
        }

        /// <summary>
        /// 修改Event时 
        /// </summary>
        public bool UpdateEvent(EventsView entity, EventEntity oldEvent, List<EventInviteEntity> inviteUserList)
        {
            bool result = false;
            entity.UpdatedOn = DateTime.Now;

            if (!entity.AllDay)
                entity.FromDay = DateTime.Parse(string.Format("{0}/{1}/{2} {3} {4}"
                , entity.FromDay.Month, entity.FromDay.Day, entity.FromDay.Year, entity.FromTime, entity.FromTimeType == 1 ? "AM" : "PM"));

#if !DEBUG
            using (TransactionScope tran = new TransactionScope())
#endif
            {
                result = mgr.EditEvents(entity);

                if (entity.FromDay >= DateTime.Now.Date)  //当天之前的TimeSheet不做修改
                {
                    if (entity.ProjectID.ToString() == Config.HRProjectID) //项目为HR时，需要处理TimeSheets
                    {
                        if (!(oldEvent.IsOff) && entity.IsOff)//如果之前没有选择OFF的ticket，修改后选择了,则添加TimeSheets
                        {
                            result = AddTimeSheetsWithEvent(entity);
                        }
                        if (oldEvent.IsOff && entity.IsOff)//如果两次都选择了OFF的ticket，则修改Timesheet时间
                        {
                            TimeSheetsEntity timeSheetEntity = tsp.GetByEventId(entity.ID, entity.FromDay.Date);
                            if (entity.AllDay)
                            {
                                timeSheetEntity.Hours = 8;
                            }
                            else
                            {
                                DateTime FromDay = DateTime.Parse(string.Format("{0}/{1}/{2} {3} {4}"
                                , entity.FromDay.Month, entity.FromDay.Day, entity.FromDay.Year, entity.FromTime, entity.FromTimeType == 1 ? "AM" : "PM"));
                                DateTime EndDay = DateTime.Parse(string.Format("{0}/{1}/{2} {3} {4}"
                                , entity.ToDay.Month, entity.ToDay.Day, entity.ToDay.Year, entity.ToTime, entity.ToTimeType == 1 ? "AM" : "PM"));
                                string Hours = EndDay.Subtract(FromDay).TotalHours.ToString();
                                timeSheetEntity.Hours = decimal.Parse(Hours);
                            }
                            timeSheetEntity.IsSubmitted = entity.AllDay ? true : false;
                            timeSheetEntity.ModifiedOn = DateTime.Now;
                            result = tsp.UpdateTimeSheet(timeSheetEntity);
                        }
                        if (oldEvent.IsOff && !(entity.IsOff))//如果之前选择了OFF的ticket，修改后没有选择,则删除TimeSheets
                        {
                            result = tsp.DeleteByEventId(entity.ID, entity.FromDay.Date);
                        }
                    }
                }

                if (result)
                {
                    RemoveInviteUser(inviteUserList);
                    inviteUserList.RemoveAll(r => (r.UserID > 0 && (r.OptionStatus == 1 || r.OptionStatus == 3))
                        || (r.UserID == 0 && r.OptionStatus == 3));
                    mgr.AddEventInvites(entity, inviteUserList);
                }
#if !DEBUG
                if (result)
                {
                    tran.Complete();
                    return true;
                }
#endif
            }
            return result;
        }

        public EventEntity GetEventInfo(int id)
        {
            return mgr.Get(id);
        }

        public EventEntity GetEventByCreateId(int creatId)
        {
            return mgr.GetEventByCreateId(creatId);
        }

        public List<EventEntity> GetEventList(int currentUserId, DateTime startDate, DateTime endDate, int userId, string allUser, int projectID)
        {
            return mgr.GetEvents(currentUserId, startDate, endDate, userId, allUser, projectID);
        }

        /// <summary>
        /// 删除Event 并且删除权限与邀请的人，还要发通知给邀请的，告诉Event已取消
        /// </summary>
        public bool Delete(int entityId, DateTime date)
        {
            bool result = false;
            result = mgr.Delete(entityId);
            DeleteTimeSheet(entityId, date);
            return result;
        }

        public void DeleteTimeSheet(int entityId, DateTime date)
        {
            if (entityId > 0)
            {
                if (date >= DateTime.Now.Date)  //当天之前的TimeSheet不做修改
                {
                    tsp.DeleteByEventId(entityId, date);  //根据eventid和日期删除对应的timesheet
                }
            }
        }

        public bool DeleteAll(int createdBy, DateTime createdOn, DateTime fromDate)
        {
            return mgr.DeleteAll(createdBy, createdOn, fromDate);
        }

        public List<EventInviteEntity> GetEventInvites(int eventId)
        {
            return mgr.GetEventInvites(eventId);
        }

        /// <summary>
        /// 删除 UserId= 0 ; 或者 UserId > 0 and optionStatu =1
        /// </summary>
        /// <param name="inviteList"></param>
        /// <returns></returns>
        public bool RemoveInviteUser(List<EventInviteEntity> inviteList)
        {
            return mgr.RemoveInviteUser(inviteList);
        }

        private bool AddTimeSheetsWithEvent(EventEntity entity)
        {
            TimeSheetsEntity TimeSheet = new TimeSheetsEntity();
            TimeSheet.Description = entity.Details;
            if (entity.AllDay)
            {
                TimeSheet.Hours = 8;
                //删除之前用户当天的TimeSheet
                tsp.DeleteByUserAndDate(entity.CreatedBy, entity.FromDay.Date);
            }
            else
            {
                DateTime FromDay = DateTime.Parse(string.Format("{0}/{1}/{2} {3} {4}"
                , entity.FromDay.Month, entity.FromDay.Day, entity.FromDay.Year, entity.FromTime, entity.FromTimeType == 1 ? "AM" : "PM"));
                DateTime EndDay = DateTime.Parse(string.Format("{0}/{1}/{2} {3} {4}"
                , entity.ToDay.Month, entity.ToDay.Day, entity.ToDay.Year, entity.ToTime, entity.ToTimeType == 1 ? "AM" : "PM"));
                string Hours = EndDay.Subtract(FromDay).TotalHours.ToString();
                TimeSheet.Hours = decimal.Parse(Hours);
            }
            TimeSheet.IsMeeting = false;
            TimeSheet.IsSubmitted = entity.AllDay ? true : false;
            TimeSheet.Percentage = 0;
            TimeSheet.ProjectID = entity.ProjectID;
            TimeSheet.TicketID = string.IsNullOrEmpty(Config.HRTicketID) ? 0 : int.Parse(Config.HRTicketID);
            TimeSheet.SheetDate = entity.FromDay.Date;
            TimeSheet.UserID = entity.CreatedBy;
            TimeSheet.CreatedOn = DateTime.Now;
            TimeSheet.CreatedBy = entity.CreatedBy;
            TimeSheet.ModifiedOn = DateTime.Now;
            TimeSheet.ModifiedBy = entity.CreatedBy;
            TimeSheet.EventID = entity.ID;
            return tsp.AddTimeSheet(TimeSheet) > 0;
        }

        /// <summary>
        /// 获取可更新和删除的Event的id集合
        /// </summary>
        /// <param name="createdBy"></param>
        /// <param name="createdOn"></param>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public DataSet GetUpdateAndDeleteEvents(int createdBy, DateTime createdOn, DateTime fromDate)
        {
            return mgr.GetUpdateAndDeleteEvents(createdBy, createdOn, fromDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="userID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderDirectioin"></param>
        /// <returns></returns>
        public DataTable QueryReportDetailsByProject(int projectID, int userID, DateTime startDate
          , DateTime endDate, string orderBy, string orderDirectioin)
        {
            this.ClearBrokenRuleMessages();
            DataTable dt = mgr.QueryReportDetailsByProject(projectID, userID, startDate, endDate, orderBy, orderDirectioin);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return dt;
        }

        public DataTable QueryReportTotalHoursByProject(int projectID, int userID, DateTime startDate
           , DateTime endDate, string orderBy, string orderDirectioin)
        {
            this.ClearBrokenRuleMessages();
            DataTable dt = mgr.QueryReportTotalHoursByProject(projectID, userID, startDate, endDate, orderBy, orderDirectioin);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return dt;
        }

        public DataTable GetPtoByProjectUser(int projectID, int userID, DateTime startDate,
           DateTime endDate)
        {
            this.ClearBrokenRuleMessages();
            DataTable dt = mgr.GetPtoByProjectUser(projectID, userID, startDate, endDate);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return dt;
        }
        #region
        /// <summary>
        /// insert     
        /// </summary>
        /// <param name="workTimeEntitie"></param>
        /// <returns></returns>
        public int AddWorkTime(WorkTimeEntity workTimeEntitie)
        {
            return mgr.AddWorkTime(workTimeEntitie);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<WorkTimeEntity> GetWorkTime(int userId)
        {
            return mgr.GetWorkTime(userId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteWorkTimeByUserId(int userId)
        {
            if (GetWorkTime(userId).Any())
            {
                return mgr.DeleteWorkTimeByUserId(userId);
            }
            return true;
        }

        public bool UpdateWorkTime(List<WorkTimeEntity> entities)
        {
            if (GetWorkTime(entities.FirstOrDefault().UserID).Any())
            {
                if (DeleteWorkTimeByUserId(entities.FirstOrDefault().UserID))
                {
                    if (entities.Any(t => AddWorkTime(t) == 0))
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (entities.Any(t => AddWorkTime(t) == 0))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
