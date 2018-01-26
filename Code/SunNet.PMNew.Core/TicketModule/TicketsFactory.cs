using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils;


namespace SunNet.PMNew.Entity.TicketModel
{
    public class TicketsFactory
    {
        public static TicketsEntity Create(int createdByUserId, ISystemDateTime timeProvider)
        {
            TicketsEntity info = new TicketsEntity();
            info.Title = string.Empty;
            info.URL = string.Empty;
            info.CompanyID = 0;
            info.ContinueDate = 0;
            info.ConvertDelete = (int)CovertDeleteState.Normal;
            info.CreateUserName = string.Empty;
            info.DeliveryDate = timeProvider.Now;
            info.Description = string.Empty;
            info.DevTsHours = 0;
            info.Hours = 0;
            info.ID = 0;
            info.IsEstimates = false;
            info.IsInternal = false;
            info.Priority = (int)PriorityState.Normal;
            info.ProjectID = 0;
            info.PublishDate = timeProvider.Now;
            info.QaTsHours = 0;
            info.SourceTicketID = 0;
            info.StartDate = timeProvider.Now;
            info.Status = 0;
            info.TicketID = 0;
            info.TicketType = Enum.GetName(typeof(TicketsType), TicketsType.Bug);
            info.CreatedBy = createdByUserId;
            info.CreatedOn = timeProvider.Now;
            info.ModifiedBy = createdByUserId;
            info.ModifiedOn = timeProvider.Now;

            return info;
        }
    }
}
