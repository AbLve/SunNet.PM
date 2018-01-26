using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.TicketModel.UserTicket
{
    public class TicketListModel
    {
        public int ResponsibleUserID { get; set; }
        public int TicketID { get; set; }
        public string Title { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public PriorityState Priority { get; set; }
    }
}
