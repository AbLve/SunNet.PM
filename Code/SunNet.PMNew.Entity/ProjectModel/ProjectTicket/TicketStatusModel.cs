using SunNet.PMNew.Entity.TicketModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ProjectModel.ProjectTicket
{
    public class TicketStatusModel
    {
        public int TicketID { get; set; }
        public int ProjectID { get; set; }
        public TicketsState TicketStatus { get; set; }
        public string Title { get; set; }
        public int ResponsibleUserID { get; set; }
        public PriorityState Priority { get; set; }
        public string StatusText
        {
            get
            {
                return TicketStatus.ToString().Replace('_', ' ');
            }
        }
    }
}
