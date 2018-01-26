using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyBook.Common
{
    public class CommonConst
    {
        public static DateTime MinDateTime = new DateTime(1753, 1, 1);

        /// <summary>
        /// 格式化日期，如果小于最小日期返回空，否则按给定字符串格式化
        /// </summary>
        /// <param name="date">需要格式化的日期</param>
        /// <param name="format">格式化类型</param>
        /// <returns></returns>
        public static string FormatDateTime(DateTime date, string format = "MM/dd/yyyy")
        {
            if (date <= MinDateTime)
                return "";
            return date.ToString(format);
        }
        public static string DefaultMaleHeadImagePath = "/Images/Icons/userman_lsample.png";
        public static string DefaultMaleLittleHeadImagePath = "/Images/Icons/userman_sample.png";

        public static string DefaultFemaleHeadImagePath = "/Images/Icons/user_lsample.png";
        public static string DefaultFemaleLittleHeadImagePath = "/Images/Icons/user_sample.png";

        public static string DefaultCoverImagePath = "/images/coversample.png";
        public static string DefaultCoverPosition = "0px 0px";

        public static string DefaultCountryString = "United States";

        public static string DefaultFamilyGroupName = "Family";
        public static string DefaultFriendGroupName = "Friends";
        public static string DefaultCloseFriendGroupName = "Close Friends";

        public static string MenuHome = "Menu_Home";
        public static string MenuTree = "Menu_Tree";
        public static string MenuPhotos = "Menu_Photos";
        public static string MenuVideos = "Menu_Videos";
        public static string MenuDocuments = "Menu_Documents";
        public static string MenuEvents = "Menu_Events";
        public static string MenuTimeline = "MenuTimeline";
        public static string MenuFriends = "Menu_Friends";
        public static string MenuForums = "Menu_Forums";

        public static string REDIRECT_TO_ERROR = "REDIRECT_TO_ERROR";

    }
}
