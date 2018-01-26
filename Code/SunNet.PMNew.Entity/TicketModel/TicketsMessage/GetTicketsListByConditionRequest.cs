using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SunNet.PMNew.Entity.TicketModel
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
            TicketInfo.Type = TicketSc.TicketType;
            TicketInfo.Project = TicketSc.Project;
            TicketInfo.ProjectID = TicketSc.projectId;
            TicketInfo.AssignedUser = TicketSc.AssignedUser;
            TicketInfo.Company = TicketSc.Company;
            TicketInfo.Client = TicketSc.Client;
            TicketInfo.ClientPriority = TicketSc.ClientPriority;
            TicketInfo.IsInternal = TicketSc.IsInternal;
            TicketInfo.OrderDirection = TicketSc.OrderDirection;
            TicketInfo.OrderExpression = TicketSc.OrderExpression;
            TicketInfo.FeedBackTicketsList = TicketSc.FeedBackTicketsList;
            if (requestByUserId != 0)
            {
                TicketInfo.AssignedUser = requestByUserId.ToString();
            }
            return TicketInfo;
        }
    }
}
