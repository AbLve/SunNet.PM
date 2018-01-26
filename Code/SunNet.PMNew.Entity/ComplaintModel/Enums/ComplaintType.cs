using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ComplaintModel.Enums
{
    public enum ComplaintTypeEnum
    {
        Post = 0,
        Video = 1,
        Photo = 2,
        User = 3,
        Group = 4,
        Other = 5
    }

    public class ComplaintTypeHelper
    {
        public static List<ComplaintTypeEnum> AllComplaintType
        {
            get
            {
                return new List<ComplaintTypeEnum>()
                {
                    ComplaintTypeEnum.Post,
                    ComplaintTypeEnum.Video,
                    ComplaintTypeEnum.Photo,
                    ComplaintTypeEnum.User,
                    ComplaintTypeEnum.Group,
                    ComplaintTypeEnum.Other
                };
            }
        }
    }
}
