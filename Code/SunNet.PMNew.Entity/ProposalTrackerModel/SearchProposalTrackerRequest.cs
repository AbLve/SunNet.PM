using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ProposalTrackerModel
{
    public class SearchProposalTrackerRequest
    {
        public List<ProposalTrackerEntity> ResultList { get; set; }
        public int ResultCount { get; set; }
    }
}
