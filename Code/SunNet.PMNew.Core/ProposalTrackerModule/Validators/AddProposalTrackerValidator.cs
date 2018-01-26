using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Entity.ProposalTrackerModel;


namespace SunNet.PMNew.Core.ProposalTrackerModule
{
    public class AddProposalTrackerValidator : BaseValidator<ProposalTrackerEntity>
    {
        protected override void ValidateExtraRules(ProposalTrackerEntity o)
        {

        }
    }
}
