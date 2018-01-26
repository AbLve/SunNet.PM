using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.SealModel
{
    public enum  RequestStatus:int 
    {
        All = -88,
        Denied = -1,
        Canceled = 0,
        Draft = 1,
        Submitted = 2,
        PendingApproval = 3,
        Approved = 4,
        PendingProcess = 5,
        Processed = 6,
        Completed = 7
    }

    public static class RequestStatusHelper
    {
        public static string RequestStatusToText(this RequestStatus rs)
        {
            if (rs == RequestStatus.PendingApproval)
                return "Pending Approval";
            if (rs == RequestStatus.PendingProcess)
                return "Pending Process";
            else
                return rs.ToString();
        }
    }
}
