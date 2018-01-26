using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ComplaintModel.Enums
{
    public enum ComplaintReasonEnum
    {
        Porn = 0,
        Scam = 1,
        CommercialSpam = 2,
        SensitiveInfo = 3,
        CopyrightInfrin = 4
    }

    public class ComplaintReasonHelper
    {
        public static List<ComplaintReasonEnum> AllReason
        {
            get
            {
                return new List<ComplaintReasonEnum>()
                {
                    ComplaintReasonEnum.Porn,
                    ComplaintReasonEnum.Scam,
                    ComplaintReasonEnum.CommercialSpam,
                    ComplaintReasonEnum.SensitiveInfo,
                    ComplaintReasonEnum.CopyrightInfrin
                };
            }
        }
    }
}
