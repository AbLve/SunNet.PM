using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.ProposalTrackerModel;


namespace SunNet.PMNew.Core.ProposalTrackerModule
{
    public interface IProposalTrackerNoteRepository: IRepository<ProposalTrackerNoteEntity>
    {
        List<ProposalTrackerNoteEntity> GetProposalTrackerNotes(int proposalTrackerId);
    }
}
