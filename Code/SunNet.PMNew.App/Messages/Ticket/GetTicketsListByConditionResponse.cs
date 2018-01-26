using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.App.Messages
{
    public class GetTicketsListByConditionResponse
    {
        public List<TicketsDetailDTO> ListTicketDetailDTO { get; set; }
    }
}
