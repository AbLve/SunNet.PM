using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.App.Messages.Ticket
{
    class PostTicketsResponse
    {
        public TicketsDetailDTO TicketsDetail { get; set; }
    }
}
