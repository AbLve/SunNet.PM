using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.TicketModel
{
    public class TicketsSearchConditionDTO
    {
        public string KeyWord { get; set; }

        public string Status { get; set; }

        /// <summary>
        /// TicketType = "Bug" or "Request"
        /// </summary>
        public string TicketType { get; set; }

        public string Project { get; set; }

        public string AssignedUser { get; set; }

        public string Company { get; set; }

        public string Client { get; set; }

        public string ClientPriority { get; set; }

        public bool IsInternal { get; set; }

        public string OrderExpression { get; set; }

        public string OrderDirection { get; set; }

        public string FeedBackTicketsList { get; set; }

        public bool IsFeedBack { get; set; }

        public int projectId { get; set; }
    }
}
