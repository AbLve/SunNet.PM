using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Entity.TicketModel;

namespace Pm2012TEST.Fakes
{
    public class FakeTicketSearchCondition : TicketsSearchCondition
    {
        public FakeTicketSearchCondition Create()
        {
            FakeTicketSearchCondition info = new FakeTicketSearchCondition();
            info.AssignedUser = "";
            info.Client = "";
            info.ClientPriority = "";
            info.Company = "";
            info.KeyWord = "";
            info.PriorityView = true;
            info.Project = "";
            info.Status = Convert.ToString((int)TicketsState.Draft);
            info.Type = "";
            return info;
        }
    }
}
