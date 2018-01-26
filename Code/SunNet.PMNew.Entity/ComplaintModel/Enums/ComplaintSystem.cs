using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ComplaintModel.Enums
{
    public enum ComplaintSystemEnum
    {
        MyFamilyBook = 0,
        EduBook = 1,
        PM = 2
    }

    public class ComplaintSystemHelper
    {
        public static List<ComplaintSystemEnum> AllComplaintSystem
        {
            get
            {
                return new List<ComplaintSystemEnum>()
                {
                    ComplaintSystemEnum.MyFamilyBook,
                    ComplaintSystemEnum.EduBook,
                    ComplaintSystemEnum.PM
                };
            }
        }
    }
}
