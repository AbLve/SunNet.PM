using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.TimeSheetModel
{
    public enum SearchType
    {
        /// <summary>
        /// All timesheets
        /// </summary>
        All,
        /// <summary>
        /// this week 's timesheets,need a date
        /// </summary>
        ListByWeek,
        /// <summary>
        /// today 's timesheets,need a date
        /// </summary>
        ListByDay,
        /// <summary>
        /// query timesheet
        /// </summary>
        QueryReport,
        /// <summary>
        /// For Email Notice ,need a Date and office(CN/US)
        /// </summary>
        EmailNotice
    }
    public class SearchTimeSheetsRequest
    {
        public SearchType SearchTimeSheetsType { get; set; }

        public bool IsPageModel { get; set; }
        public int CompanyID { get; set; }
        public int Accounting { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string OrderExpression { get; set; }
        public string OrderDirection { get; set; }
        public string Keywords { get; set; }
        public string Office { get; set; }
        public int ProjectID { get; set; }
        public int TicketID { get; set; }
        public int WID { get; set; }
        public int ID { get; set; }
        public int UserID { get; set; }
        public int Source { get; set; }
        private DateTime _searchDate;
        public DateTime SearchDate
        {
            get
            {
                if (_searchDate == null || _searchDate == DateTime.MinValue)
                {
                    return new DateTime(1753, 1, 1);
                }
                return _searchDate;
            }
            set
            {
                _searchDate = value;
            }
        }
        public DateTime Monday { get; set; }
        public DateTime Sunday { get; set; }
        private DateTime _start;
        private DateTime _end;
        public DateTime StartDate
        {
            get
            {
                if (_start == null || _start == DateTime.MinValue)
                {
                    return new DateTime(1753, 1, 1);
                }
                return _start;
            }
            set
            {
                _start = value;
            }
        }
        public DateTime EndDate
        {

            get
            {
                if (_end == null || _end == DateTime.MinValue)
                {
                    return new DateTime(2112, 1, 1);
                }
                return _end;
            }
            set
            {
                _end = value;
            }
        }
        public string TicketType { set; get; }
        public SearchTimeSheetsRequest(SearchType type, bool isPageModel, string orderExpression, string orderDirection)
        {
            this.IsPageModel = isPageModel;
            this.SearchTimeSheetsType = type;
            this.OrderExpression = orderExpression;
            this.OrderDirection = orderDirection;
        }

    }
}
