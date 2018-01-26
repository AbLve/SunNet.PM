using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ReminderEmail
{
    public static class ReminderConfig
    {
        public static string ManagerEmail = ConfigurationManager.AppSettings["ManagerEmail"];

        public static string TeamEmail = ConfigurationManager.AppSettings["TeamEmail"];

        public static int RunTimeSpan()
        {
            return int.Parse(ConfigurationManager.AppSettings["RunTimeSpan"]);
        }

        /// <summary>
        /// 程序执行时间间隔(分钟)
        /// </summary>
        public static int RunTimeInterval()
        {
            return int.Parse(ConfigurationManager.AppSettings["RunTimeInterval"]);
        }
    }
}
