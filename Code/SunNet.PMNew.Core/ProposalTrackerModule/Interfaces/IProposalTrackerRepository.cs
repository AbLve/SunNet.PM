using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.ProposalTrackerModel;

namespace SunNet.PMNew.Core.ProposalTrackerModule
{
    public interface IProposalTrackerRepository : IRepository<ProposalTrackerEntity>
    {
        SearchProposalTrackerRequest GetProposalTrackers(string keyword, int projectId, int status, int companyId,
             int payment, int userId, DateTime? beginTime, DateTime? endTime, string order, string dir, int pageCount, int pageIndex);

        List<ProposalTrackerEntity> GetProposalTrackers(string keyword, int projectId, int status,
            int payment, int userId, string orders, string dir);
        List<ProposalTrackerEntity> GetProposalTrackerByPid(int projectId);
        ProposalTrackerRelationEntity GetProposalTrackerByTid(int Tid);
        bool UpdateProposalByProposal(ProposalTrackerRelationEntity model);
        bool DelProposalTrackerRelationByID(int ID);
        decimal GetProposalTrackerHours(int ID);
        List<ProposalTrackerEntity> GetEntitiesForPaymentEmail(string condition, string connStr);

        bool UpdateProposalTrackerForPayment(ProposalTrackerEntity entity, string connStr);

        ProposalViewModel GetProposalViewModel(int proposalId);
    }
}
