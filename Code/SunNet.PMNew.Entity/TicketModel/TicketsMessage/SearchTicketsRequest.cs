using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.TicketModel
{
    public enum SearchTicketsType
    {
        /// <summary>
        /// Just for Edit Priority
        /// </summary>
        Priority,
        /// <summary>
        /// Just for Category page
        /// </summary>
        CateGory,
        TicketsListByPID,
        All,
        /// <summary>
        /// Search tickets by project in timesheet page,need a ProjectID and a status []
        /// </summary>
        TicketsForTimesheets,
        /// <summary>
        /// TicketsReport need a status and a userID
        /// </summary>
        TicketsForReport,
        /// <summary>
        /// SearchTickets in schedule month module
        /// </summary>
        TicketsForScheduleMonth,
        /// <summary>
        /// SearchTickets in schedule day module
        /// </summary>
        TicketsForScheduleDay,
        /// <summary>
        /// 搜索KnowledgeShare，需要ProjectId,Keyword
        /// </summary>
        KnowledgeShare
    }
    public class SearchTicketsRequest
    {
        public SearchTicketsRequest(SearchTicketsType searchType, string orderby, bool isPageModel)
        {
            this.SearchType = searchType;
            this.OrderBy = orderby;
            this.IsPageModel = isPageModel;
            this.CurrentPage = 1;
            this.PageCount = 20;
        }
        public SearchTicketsType SearchType { get; set; }

        public int CompanyID { get; set; }
        public int ProjectID { get; set; }
        public int CateGoryID { get; set; }
        public int UserID { get; set; }

        private DateTime _sheetDate;
        public DateTime SheetDate
        {
            get
            {
                if (_sheetDate == null || _sheetDate == DateTime.MinValue)
                {
                    return new DateTime(1753, 1, 1);
                }
                return _sheetDate;
            }
            set { _sheetDate = value; }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get
            {
                if (_startDate == null || _startDate == DateTime.MinValue)
                {
                    return new DateTime(1753, 1, 1);
                }
                return _startDate;
            }
            set
            {
                _startDate = value;
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get
            {
                if (_endDate == null || _endDate == DateTime.MinValue)
                {
                    return new DateTime(1753, 1, 1);
                }
                return _endDate;
            }
            set { _endDate = value; }
        }

        public string TicketType { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public bool SearchTicketID { get; set; }
        public string TicketIDS { get; set; }
        public string Keyword { get; set; }
        public List<TicketsState> Status { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }
        public bool IsPageModel { get; set; }
        public int Source { get; set; }
        public int Star { get; set; }
    }
}
