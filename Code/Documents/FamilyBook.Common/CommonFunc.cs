using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyBook.Common
{
    public class CommonFunc
    {
        /// <summary>
        /// 修改头像路径
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="headImage"></param>
        /// <param name="littleHeadImage"></param>
        public static void GetUserEntityImagePath(int userID, int gender, ref string headImage, ref string littleHeadImage)
        {
            //头像图片
            if (string.IsNullOrEmpty(headImage))
            {
                switch (gender)
                {
                    case (int)Gender.Female:
                        headImage = CommonConst.DefaultFemaleHeadImagePath;
                        break;
                    default:
                        headImage = CommonConst.DefaultMaleHeadImagePath;
                        break;
                }
            }
            else if (headImage.Equals(CommonConst.DefaultMaleHeadImagePath))
            {
                headImage = CommonConst.DefaultMaleHeadImagePath;
            }
            else if (headImage.Equals(CommonConst.DefaultFemaleHeadImagePath))
            {
                headImage = CommonConst.DefaultFemaleHeadImagePath;
            }
            else
            {
                headImage = FileAgent.BuilderVirtualDir(userID) + headImage;
            }

            //小头像
            if (string.IsNullOrEmpty(littleHeadImage))
            {
                switch (gender)
                {
                    case (int)Gender.Female:
                        headImage = CommonConst.DefaultFemaleLittleHeadImagePath;
                        break;
                    default:
                        headImage = CommonConst.DefaultMaleLittleHeadImagePath;
                        break;
                }
            }
            else if (littleHeadImage.Equals(CommonConst.DefaultMaleLittleHeadImagePath))
            {
                littleHeadImage = CommonConst.DefaultMaleLittleHeadImagePath;
            }
            else if (littleHeadImage.Equals(CommonConst.DefaultFemaleLittleHeadImagePath))
            {
                littleHeadImage = CommonConst.DefaultFemaleLittleHeadImagePath;
            }
            else
            {
                littleHeadImage = FileAgent.BuilderVirtualDir(userID) + littleHeadImage;
            }
        }
    }
}
