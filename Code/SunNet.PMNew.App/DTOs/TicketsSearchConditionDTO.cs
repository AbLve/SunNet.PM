using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.App.DTOs
{
    public class TicketsSearchConditionDTO
    {
        public string KeyWord { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public string Project { get; set; }

        public string AssignedUser { get; set; }

        public string Company { get; set; }

        public string Client { get; set; }

        public string ClientPriority { get; set; }

        public bool PriorityView { get; set; }
    }
}
