using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ProposalTrackerModel.Enums
{
    public enum ProposalTrackerStatusEnum : int
    {
        Awaiting_ETA = 1,
        Awaiting_Proposal = 2,
        Awaiting_Approval_PO = 3,
        Awaiting_Development = 4,
        Awaiting_Sending_Invoice = 5,
        Awaiting_Payment = 6,
        Paid_Completed = 7,
        On_Hold = 8,
        Not_Approved = 9
    }
}