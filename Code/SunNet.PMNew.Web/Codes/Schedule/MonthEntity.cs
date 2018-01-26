using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using System.Data;

namespace SunNet.PMNew.Web.Codes.Schedule
{
    public class MonthEntity : BaseSchedule
    {
        public DateTime Datetime
        {
            get
            {
                return new DateTime(this.Year, this.Month, 1);
            }
        }

        private void FillPrevMonthDays()
        {
            if (days == null)
                days = new List<DayEntity>();
            DateTime first = new DateTime(Year, Month, 1);
            int week = (int)first.DayOfWeek;
            week = week == 7 ? 0 : week;
            for (int i = week; i >= 1; i--)
            {
                DayEntity model = new DayEntity(first.AddDays(-i));
                days.Add(model);
            }
        }
        private void FillNextMonthDays()
        {
            if (days == null)
                return;
            DateTime last = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
            int week = (int)last.DayOfWeek;
            week = week == 7 ? 0 : week;
            while (week < 6)
            {
                last = last.AddDays(1);
                DayEntity model = new DayEntity(last);
                days.Add(model);
                week++;
            }
        }
        private List<DayEntity> days;
        public List<DayEntity> Days
        {
            get
            {
                if (_monthTickets == null)
                {
                    List<ScheduleTicketEntity> list = _tickApp.SearchScheduleTickets(SearchRequest);
                    _monthTickets = list;
                }
                if (days != null && days.Count < 28)
                {
                    days = null;
                }
                if (days == null)
                {
                    days = new List<DayEntity>();
                    FillPrevMonthDays();
                    int daysCount = DateTime.DaysInMonth(Year, Month);
                    for (int i = 1; i <= daysCount; i++)
                    {
                        DayEntity model = new DayEntity(Year, Month, i, Keyword, UserID, Status, _monthTickets);
                        days.Add(model);
                    }
                    FillNextMonthDays();
                }
                return days;
            }
        }

        private List<ScheduleTicketEntity> _monthTickets;
        public List<ScheduleTicketEntity> Tickets
        {
            get
            {
                if (_monthTickets == null)
                {
                    List<ScheduleTicketEntity> list = _tickApp.SearchScheduleTickets(SearchRequest);
                    _monthTickets = list;
                }
                return _monthTickets;
            }
        }
        private int _ticketsCount;
        public int TicketsCount
        {
            get
            {
                if (_ticketsCount == 0)
                {
                    int count = _tickApp.SearchScheduleTicketsCount(SearchRequest);
                    if (count < 0)
                        _ticketsCount = 0;
                    else
                        _ticketsCount = count;
                }
                return _ticketsCount;
            }
        }

        private SearchTicketsRequest _request;
        public SearchTicketsRequest SearchRequest
        {
            get
            {
                if (_request == null)
                {
                    _request = new SearchTicketsRequest(SearchTicketsType.TicketsForScheduleMonth, " Title ASC ", false);
                    _request.UserID = this.UserID;
                    _request.StartDate = _startDate;
                    _request.EndDate = _endDate;
                    _request.Keyword = Keyword;
                    _request.IsPageModel = IsPageModal;
                    _request.CurrentPage = CurrentPage;
                    _request.PageCount = PageCount;
                    if (Status < 0)
                    {
                        _request.Status = TicketsStateHelper.ScheduleStates;
                    }
                    else
                    {
                        List<TicketsState> list = new List<TicketsState>();
                        list.Add((TicketsState)Status);
                        _request.Status = list;
                    }
                }
                return _request;
            }
        }

        private DateTime _startDate;
        private DateTime _endDate;
        private TicketsApplication _tickApp;
        public MonthEntity(int year, int month, string keyword, int userID, int status)
        {
            this.Year = year;
            this.Month = month;
            this.Keyword = keyword;
            this.Status = status;
            this._startDate = new DateTime(year, month, 1);
            this._endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            this.UserID = userID;
            this.IsPageModal = false;
            _tickApp = new TicketsApplication();
        }
        public MonthEntity(int year, int month, string keyword, int userID, int status, int page, int pageCount)
        {
            this.Year = year;
            this.Month = month;
            this.Keyword = keyword;
            this.Status = status;
            this._startDate = new DateTime(year, month, 1);
            this._endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            this.UserID = userID;
            this.IsPageModal = true;
            this.CurrentPage = page;
            this.PageCount = pageCount;
            _tickApp = new TicketsApplication();
        }

    }
}
