using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.SealModel.Enum
{
    public enum WorkflowAction
    {
        Inactive = -2,  //Deprecated
        Pending = -1,
        Save = 0,
        Submit = 1,
        ForwardApproval = 2,
        Approve = 3,
        ForwardProcess = 4,
        FinishProcess = 5,
        ContinueProcess = 6,
        Deny = 7,
        Complete = 8,
        Cancel = 9
    }

    public static class WorkflowActionHelper
    {
        public static string WorkflowActionToText(this WorkflowAction wfa)
        {
            switch (wfa)
            {
                case WorkflowAction.ForwardApproval:
                    return "Forward Approval";
                    
                case WorkflowAction.ForwardProcess:
                    return "Forward Processing";
                    
                case WorkflowAction.FinishProcess:
                    return "Finish Process";
                    
                case WorkflowAction.ContinueProcess:
                    return "Continue Process";
                    
                default:
                    return wfa.ToString();
                    
            }
        }
    }
}
