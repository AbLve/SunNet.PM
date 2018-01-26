using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.TicketModel.UserTicket
{
    public class UserTicketModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public List<TicketListModel> Tickets { get; set; }
        public int TicketCount { get; set; }
        public int Previous { get; set; }
        public int Current { get; set; }
    }
}
