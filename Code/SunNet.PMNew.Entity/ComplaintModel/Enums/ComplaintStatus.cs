using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ComplaintModel.Enums
{
    public enum ComplaintStatusEnum
    {
        Pending = 0,
        Deleted = 1,
        ApprovedButNotDeleted = 2,
        Denied = 3,
        SysError = 4
    }

    public class ComplaintStatusHelper
    {
        public static List<ComplaintStatusEnum> AllStatus
        {
            get
            {
                return new List<ComplaintStatusEnum>()
                {
                    ComplaintStatusEnum.Pending,
                    ComplaintStatusEnum.Deleted,
                    ComplaintStatusEnum.ApprovedButNotDeleted,
                    ComplaintStatusEnum.Denied,
                    ComplaintStatusEnum.SysError
                };
            }
        }
    }
}
