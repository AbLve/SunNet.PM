using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Core.Notify;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Core.Validator;

using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework;
using System.Web;
using System.Text.RegularExpressions;
using System.Transactions;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.Core.ProposalTrackerModule
{
    public class ProposalTrackerRelationManager : BaseMgr
    {

        private IProposalTrackerRelationRepository proposalTrackerRelationRepository;
        public ProposalTrackerRelationManager(IProposalTrackerRelationRepository proposalTrackerRelationRepository)
        {
            this.proposalTrackerRelationRepository = proposalTrackerRelationRepository;
        }


        public int Add(ProposalTrackerRelationEntity entity)
        {
            if (entity.WID == 0)
            {
                this.AddBrokenRuleMessage("Project", "Please select Work Request");
                return 0;
            }
            if (entity.TID == 0)
            {
                this.AddBrokenRuleMessage("Payment", "Please select Ticket");
                return 0;
            }
            int id = proposalTrackerRelationRepository.Insert(entity);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }
            return id;
        }


        public bool Delete(int wid, int tid)
        {
            return proposalTrackerRelationRepository.Delete(wid, tid);
        }

        public List<TicketsEntity> GetAllRelation(int wid)
        {
            return proposalTrackerRelationRepository.GetAllRelation(wid);
        }

    }
}
