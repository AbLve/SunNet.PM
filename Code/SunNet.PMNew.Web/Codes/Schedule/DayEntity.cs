using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.Web.Codes.Schedule
{
    public class DayEntity : BaseSchedule
    {
        public DateTime Datetime
        {
            get { return new DateTime(Year, Month, Day); }
        }
        private SearchTicketsRequest _request;
        public SearchTicketsRequest SearchRequest
        {
            get
            {
                if (_request == null)
                {
                    _request = new SearchTicketsRequest(SearchTicketsType.TicketsForScheduleDay, " Title ASC ", false);
                    _request.UserID = this.UserID;
                    _request.StartDate = Datetime;
                    _request.Keyword = Keyword;
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

        public int TicketsCount
        {
            get
            {
                if (Tickets == null)
                {
                    return 0;
                }
                return Tickets.Count;
            }
        }

        public List<ScheduleTicketEntity> Tickets
        {
            get
            {
                if (Datetime.DayOfWeek == DayOfWeek.Sunday || Datetime.DayOfWeek == DayOfWeek.Saturday)
                {
                    return null;
                }
                if (_monthTickets != null && _monthTickets.Count > 0)
                {
                    List<ScheduleTicketEntity> listToday = _monthTickets
                        .FindAll(ste => ste.StartDate <= Datetime && ste.DeliveryDate >= Datetime);
                    return listToday;
                }
                return null;
            }
        }
        private List<ScheduleTicketEntity> _monthTickets;

        private TicketsApplication _tickApp;
        public DayEntity(DateTime date)
        {
            this.Year = date.Year;
            this.Month = date.Month;
            this.Day = date.Day;
        }
        public DayEntity(int year, int month, int day, string keyword, int userID, int status, List<ScheduleTicketEntity> mts)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.Keyword = keyword;
            this.Status = status;
            this.UserID = userID;
            _tickApp = new TicketsApplication();
            _monthTickets = mts;
        }
    }
}
