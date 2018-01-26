using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.Core.ProposalTrackerModule
{
    public interface IProposalTrackerRelationRepository : IRepository<ProposalTrackerRelationEntity>
    {
        List<TicketsEntity> GetAllRelation(int wid);
        bool Delete(int wid, int tid);
    }
}
