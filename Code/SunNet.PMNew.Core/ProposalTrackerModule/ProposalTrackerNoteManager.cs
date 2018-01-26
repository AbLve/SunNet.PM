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

namespace SunNet.PMNew.Core.ProposalTrackerModule
{
    public class ProposalTrackerNoteManager : BaseMgr
    {
        private IProposalTrackerNoteRepository proposalTrackerNoteRepository;
        public ProposalTrackerNoteManager(IProposalTrackerNoteRepository proposalTrackerNoteRepository)
        {
            this.proposalTrackerNoteRepository = proposalTrackerNoteRepository;
        }

        public int Add(ProposalTrackerNoteEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Title))
            {
                this.AddBrokenRuleMessage("Title", "Please input Title");
                return 0;
            }
            if (string.IsNullOrEmpty(entity.Description))
            {
                this.AddBrokenRuleMessage("Description", "Please input Description");
                return 0;
            }
            int id = proposalTrackerNoteRepository.Insert(entity);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }
            return id;
        }


        public List<ProposalTrackerNoteEntity> GetProposalTrackerNotes(int proposalTrackerId)
        {
            return proposalTrackerNoteRepository.GetProposalTrackerNotes(proposalTrackerId);
        }
    }
}
