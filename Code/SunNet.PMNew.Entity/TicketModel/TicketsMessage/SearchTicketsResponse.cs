using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.TicketModel
{
    public class SearchTicketsResponse
    {
        public List<ExpandTicketsEntity> ResultList { get; set; }
        public int ResultCount { get; set; }
        public bool IsError { get; set; }
    }
}
