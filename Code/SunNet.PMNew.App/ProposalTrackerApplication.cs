using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;

using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Core.UserModule;

using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Core.ProposalTrackerModule;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.App
{
    public class ProposalTrackerApplication : BaseApp
    {

        private ProposalTrackerManager mgr;
        private ProposalTrackerRelationManager relationMgr;
        private ProposalTrackerNoteManager noteMgr;

        public ProposalTrackerApplication()
        {
            mgr = new ProposalTrackerManager(ObjectFactory.GetInstance<IProposalTrackerRepository>());
            relationMgr = new ProposalTrackerRelationManager(ObjectFactory.GetInstance<IProposalTrackerRelationRepository>());
            noteMgr = new ProposalTrackerNoteManager(ObjectFactory.GetInstance<IProposalTrackerNoteRepository>());
        }

        public ProposalTrackerEntity Get(int tid)
        {
            this.ClearBrokenRuleMessages();

            if (tid <= 0) return null;

            ProposalTrackerEntity ticketInfo = mgr.Get(tid);

            if (ticketInfo == null)
            {
                this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
                return null;
            }

            return ticketInfo;
        }

        public int AddProposalTracker(ProposalTrackerEntity model)
        {
            this.ClearBrokenRuleMessages();
            int id = mgr.Add(model);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }


        public bool UpdateProposalTracker(ProposalTrackerEntity model)
        {
            this.ClearBrokenRuleMessages();
            bool result = mgr.Update(model);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }

        public bool UpdateProposalTrackerForPayment(ProposalTrackerEntity model, string connStr)
        {
            this.ClearBrokenRuleMessages();
            bool result = mgr.UpdateProposalTrackerForPayment(model, connStr);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }

        public string GetProposalTrackerNo()
        {
            int wRCount = mgr.GetProposalTrackerNo(DateTime.Now, DateTime.Now.AddDays(1));
            int yCount = mgr.GetProposalTrackerNo(new DateTime(DateTime.Now.Year, 1, 1), DateTime.Now.AddDays(1));
            return string.Format("Appendix A {0}-{1}"
                , DateTime.Now.ToString("yyyy-MM-dd"), yCount + 1);
        }

        public SearchProposalTrackerRequest GetSearchProposalTrackers(string keyword, int projectId, int status, int companyId,
            int payment, int userId,DateTime? beginTime, DateTime? endTime, string order, string orderdir, int pageCount, int pageIndex)
        {
            return mgr.GetSearchProposalTrackers(keyword, projectId, status, companyId, payment, userId, beginTime, endTime,
                order, orderdir, pageCount, pageIndex);
        }

        public List<ProposalTrackerEntity> GetProposalTrackerByPid(int projectId)
        {
            return mgr.GetProposalTrackerByPid(projectId);
        }

        public ProposalTrackerRelationEntity GetProposalTrackerByTid(int Tid)
        {
            return mgr.GetProposalTrackerByTid(Tid);
        }
        public bool UpdateProposalByProposal(ProposalTrackerRelationEntity model)
        {
            return mgr.UpdateProposalByProposal(model);
        }
        public bool DelProposalTrackerRelationByID(int ID)
        {
            return mgr.DelProposalTrackerRelationByID(ID);
        }

        public int AddProposalTrackerRelation(ProposalTrackerRelationEntity model)
        {
            this.ClearBrokenRuleMessages();
            int id = relationMgr.Add(model);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }

        public List<TicketsEntity> GetAllRelation(int wid)
        {
            return relationMgr.GetAllRelation(wid);
        }

        public bool DeleteRelation(int wid, int tid)
        {
            return relationMgr.Delete(wid, tid);
        }

        public int AddNote(ProposalTrackerNoteEntity model)
        {
            this.ClearBrokenRuleMessages();
            int id = noteMgr.Add(model);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }


        public List<ProposalTrackerNoteEntity> GetProposalTrackerNotes(int proposalTrackerId)
        {
            return noteMgr.GetProposalTrackerNotes(proposalTrackerId);
        }


        public decimal GetProposalTrackerHours(int ID)
        {
            return mgr.GetProposalTrackerHours(ID);
        }

        public List<ProposalTrackerEntity> GetEntitiesForPaymentEmail(string condition, string connStr)
        {
            return mgr.GetEntitiesForPaymentEmail(condition, connStr);
        }

        public ProposalViewModel GetProposalViewModel(int proposalId)
        {
            return mgr.GetProposalViewModel(proposalId);
        }
    }
}
