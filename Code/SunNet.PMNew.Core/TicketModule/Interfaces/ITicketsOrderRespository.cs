using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core.Repository;

namespace SunNet.PMNew.Core.TicketModule
{
    public interface ITicketsOrderRespository : IRepository<TicketsOrderEntity>
    {
        bool RemoveAllTicketsOrderByProject(int projectID);

    }
}
