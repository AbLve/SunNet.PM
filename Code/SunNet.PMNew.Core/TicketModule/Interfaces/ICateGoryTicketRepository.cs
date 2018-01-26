using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.Core.TicketModule
{
    public interface ICateGoryTicketRepository : IRepository<CateGoryTicketEntity>
    {
        bool Delete(int ticketID, int cateGoryID);
    }
}
