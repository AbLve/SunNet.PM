using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.TicketModel;

namespace Pm2012TEST.Fakes
{
    public class FakeTicketEntity
    {
        public TicketsEntity CreateTicketsEntity(int type)
        {
            int code = 0;
            TicketsEntity info = new TicketsEntity();
            info.TicketID = 999;
            info.Title = "Tr1";
            info.URL = string.Empty;
            info.CompanyID = 1;
            info.ContinueDate = 0;
            switch (type)
            {
                case 1:
                    code = (int)CovertDeleteState.Normal;
                    break;
                case 2:
                    code = (int)CovertDeleteState.ConvertToHistory;
                    break;
                case 3:
                    code = (int)CovertDeleteState.ForeverDelete;
                    break;
            }
            info.ConvertDelete = (CovertDeleteState)code;
            info.DeliveryDate = DateTime.Now;
            info.Description = "T007";
            info.DevTsHours = 0;
            info.Hours = 0;
            info.ID = 0;
            info.IsEstimates = false;
            info.IsInternal = false;
            info.Priority = PriorityState.Normal;
            info.ProjectID = 1;
            info.PublishDate = DateTime.Now;
            info.QaTsHours = 1;
            info.SourceTicketID = 0;
            info.StartDate = DateTime.Now;
            info.Status =  TicketsState.RTestingPre;
            info.TicketType = TicketsType.Bug;
            info.CreatedBy = 1;
            info.CreatedOn = DateTime.Now;
            info.ModifiedBy = 1;
            info.ModifiedOn = DateTime.Now;
            return info;
        }
    }
}
