using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ComplaintModel.Enums
{
    public enum ComplaintAppSrcEnum
    {
        IPhoneApp = 0, 
        AndroidApp = 1,
        WinPhoneApp = 2,
        Web = 3
    }

    public class ComplaintAppSrcHelper
    {
        public static List<ComplaintAppSrcEnum> AllAppSrc
        {
            get
            {
                return new List<ComplaintAppSrcEnum>()
                {
                    ComplaintAppSrcEnum.IPhoneApp,
                    ComplaintAppSrcEnum.AndroidApp,
                    ComplaintAppSrcEnum.WinPhoneApp,
                    ComplaintAppSrcEnum.Web
                };
            }
        }
    }
}
