using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyBook.Common
{
    public static class TimeAgent
    {
        /// <summary>
        /// 评论，等显示添加的时间，刚添加时，页面统一用 a few seconds ago
        /// </summary>
        public static string BuilderTime(DateTime date)
        {
            TimeSpan ts = DateTime.Now - date;
            if (ts.TotalDays > 3)
                return date.ToString("MM/dd/yyyy");
            else if (ts.TotalDays > 2)
                return "2 days ago";
            else if (ts.TotalDays > 1)
                return "1 day ago";
            else if (ts.TotalHours > 2)
                return string.Format("{0} hours ago", ((int)ts.TotalHours).ToString());
            else if (ts.TotalHours > 1)
                return "1 hour ago";
            else if (ts.TotalMinutes > 2)
                return string.Format("{0} minutes ago", ((int)ts.TotalMinutes).ToString());
            else if (ts.TotalMinutes > 1)
                return "1 minute ago";
            else if (ts.TotalSeconds > 2)
                return string.Format("{0} seconds  ago", ((int)ts.TotalSeconds).ToString());
            else
                return "1 second ago";
        }

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  1/23/2014 11:49
        public static DateTime MinValue
        {
            get
            {
                return CommonConst.MinDateTime;
            }
        }
    }
}
