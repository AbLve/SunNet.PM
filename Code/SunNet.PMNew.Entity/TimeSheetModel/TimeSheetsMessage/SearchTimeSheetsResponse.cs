using SunNet.PMNew.Entity.TicketModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.TimeSheetModel
{
    public class SearchTimeSheetsResponse
    {
        public List<ExpandTimeSheetsEntity> ResultList { get; set; }
        public List<TimeSheetTicket> TimeSheetsList { get; set; }
        public List<TicketsEntity> TicketProjectsList { get; set; }

        public int ResultCount { get; set; }
        public bool IsError { get; set; }
    }
}
