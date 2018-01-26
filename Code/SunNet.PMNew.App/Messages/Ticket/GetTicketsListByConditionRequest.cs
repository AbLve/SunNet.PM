using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.App.Messages.Ticket
{
    public class GetTicketsListByConditionRequest
    {
        public TicketsSearchConditionDTO TicketSc { get; set; }

        public int requestByUserId { get; set; }

        public TicketsSearchCondition ToBusinessEntity()
        {
            TicketsSearchCondition TicketInfo = TicketsSerachConditionFactory.Create(this.requestByUserId);
            TicketInfo.KeyWord = TicketSc.KeyWord;
            TicketInfo.Status = TicketSc.Status;
            TicketInfo.Type = TicketSc.Type;
            TicketInfo.Project = TicketSc.Project;
            TicketInfo.AssignedUser = TicketSc.AssignedUser;
            TicketInfo.Company = TicketSc.Company;
            TicketInfo.Client = TicketSc.Client;
            TicketInfo.ClientPriority = TicketSc.ClientPriority;
            TicketInfo.PriorityView = TicketSc.PriorityView;

            return TicketInfo;
        }
    }
}
