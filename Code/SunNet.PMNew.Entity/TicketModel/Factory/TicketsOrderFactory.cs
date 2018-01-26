using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Entity.TicketModel
{
    public class TicketsOrderFactory
    {
        public static TicketsOrderEntity CreateTicketsOrderEntity(int createdByUserID, ISystemDateTime timeProvider)
        {
            TicketsOrderEntity model = new TicketsOrderEntity();

            model.CreatedBy = createdByUserID;
            model.CreatedOn = timeProvider.Now;
            model.ID = 0;
            model.ModifiedBy = createdByUserID;
            model.ModifiedOn = timeProvider.Now;

            model.ProjectID = 0;
            model.TicketID = 0;
            model.OrderNum = 0;

            return model;
        }
    }
}
