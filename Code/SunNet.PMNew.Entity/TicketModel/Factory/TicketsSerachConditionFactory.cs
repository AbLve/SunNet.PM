using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.TicketModel
{
    public class TicketsSerachConditionFactory
    {
        public static TicketsSearchCondition Create(int createdByUserId)
        {
            TicketsSearchCondition TicketSC = new TicketsSearchCondition();

            TicketSC.KeyWord = string.Empty;
            TicketSC.Status = Convert.ToString((int)TicketsState.Draft);
            TicketSC.Type = Enum.GetName(typeof(TicketsType), TicketsType.Bug); ;
            TicketSC.Project = string.Empty;
            TicketSC.AssignedUser = createdByUserId.ToString();
            TicketSC.Company = string.Empty;
            TicketSC.Client = string.Empty;
            TicketSC.ClientPriority = Enum.GetName(typeof(PriorityState), PriorityState.Low);
            TicketSC.IsInternal = true;
            return TicketSC;
        }
    }
}
