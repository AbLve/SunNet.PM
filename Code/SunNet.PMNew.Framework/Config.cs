using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Framework
{
    public class Config
    {
        public static bool LogEnabled
        {
            get
            {
                return GetAppSettingByBool("LogEnabled");
            }
        }

        public static int LogFileSize
        {
            get
            {
                return UtilFactory.Helpers.CommonHelper.ToInt(GetAppSetting("LogFileSize"));
            }
        }

        public static string LogFileName
        {
            get
            {
                return GetAppSetting("LogFileName");
            }
        }

        private static string GetAppSetting(string key)
        {
            if (!System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(key))
                return "";
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        private static bool GetAppSettingByBool(string key)
        {
            if (GetAppSetting(key).Equals("1"))
                return true;
            else if (GetAppSetting(key).Equals("0"))
                return false;
            else
            {
                bool result = false;
                bool.TryParse(GetAppSetting(key), out result);
                return result;
            }
        }

        public static string HrEmail
        {
            get { return GetAppSetting("HrEmail"); }
        }

        public static int TimeSheetDays
        {
            get
            {
                string days = GetAppSetting("TimeSheetDays");
                int ds = 0;
                if (int.TryParse(days, out ds))
                {
                    return ds;
                }
                return 3;
            }
        }
        public static string DefaultSendEmail
        {
            get
            {
                return GetAppSetting("DefaultSendEmail");
            }
        }
        public static string ChangeStatusAndSendEmailTo
        {
            get
            {
                return GetAppSetting("ChangeStatusAndSendEmailTo");
            }
        }
        public static string DefaultSurveyEmail
        {
            get
            {
                return GetAppSetting("DefaultSurveyEmail");
            }
        }
        public static string SubmittedEmailAddrs
        {
            get
            {
                return GetAppSetting("SubmittedEmailAddrs");
            }
        }
        public static string NoSubmittedEmailAddrs
        {
            get
            {
                return GetAppSetting("NoSubmittedEmailAddrs");
            }
        }

        public static string AppDomain
        {
            get
            {
                return GetAppSetting("DomainHost");
            }
        }
        public static int CollectedTicketsDirectory
        {
            get
            {
                return Convert.ToInt32(GetAppSetting("CollectedTicketsDirectory"));
            }
        }

        public static string UploadPath
        {
            get
            {
                return GetAppSetting("FolderPath");
            }
        }

        /// <summary>
        /// 发送timesheet通知时间，从该时间开始一个小时内发送邮件，否则不发送
        /// </summary>
        /// <value>
        /// The timesheet notice time start.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/19 04:45
        public static int TimesheetNoticeTimeStart
        {
            get
            {
                string start = GetAppSetting("TimesheetNoticeTime");
                int s = 1;
                int.TryParse(start, out s);
                return s;
            }
        }


        #region Seal
        public static int SealOwner
        {
            get
            {
                return int.Parse(GetAppSetting("SealOwner"));
            }
        }

        public static int SealApprover
        {
            get
            {
                return int.Parse(GetAppSetting("SealApprover"));
            }
        }

        public static string SealFilePath
        {
            get
            {
                return GetAppSetting("SealFilePath");
            }
        }

        public static int WorkflowAdmin
        {
            get
            {
                return int.Parse(GetAppSetting("WorkflowAdmin"));
            }
        }

        
        #endregion


        public static bool IsTest
        {
            get
            {
                return GetAppSetting("IsTest") == "1";
            }
        }

        public static string TestMails
        {
            get
            {
                return GetAppSetting("TestMails");
            }
        }

        public static string RemainHousrsMail
        {
            get
            {
                return GetAppSetting("emailGroup");
            }
        }

        public static string RemainHousrsCheckInternal
        {
            get
            {
                return GetAppSetting("checkRemainHoursIntervalHours");
            }
        }

        public static string TimesheetNoticeSvrPw
        {
            get
            {
                return GetAppSetting("TimesheetNoticeSvrPw");
            }
        }

        public static DayOfWeek? weekPlanNoticeDayofWeek
        {
            get
            {
                try
                {
                    return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), GetAppSetting("weekplanNoticeDay"), true);
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(ex);
                    return null;
                }
            }
        }

        public static int MaxRequestLength
        {
            get
            {
                int result;
                if (!int.TryParse(GetAppSetting("MaxRequestLength"), out result))
                {
                    return 1024000000;
                }
                else
                {
                    return result;
                }
            }
        }

        public static string NearForumsDataBase
        {
            get
            {
                return GetAppSetting("NearForumsDataBase");
            }
        }

        private static int _SunnetCompany;
        public static int SunnetCompany
        {
            get
            {
                if (_SunnetCompany > 0)
                    return _SunnetCompany;
                int.TryParse(GetAppSetting("SunnetCompany"), out _SunnetCompany);
                return _SunnetCompany;
            }
        }

        private static string _HRProjectID;
        public static string HRProjectID
        {
            get
            {
                return ConfigurationManager.AppSettings["HRProjectID"];
            }
        }

        private static string _HRTicketID;
        public static string HRTicketID
        {
            get
            {
                return ConfigurationManager.AppSettings["HRTicketID"];
            }
        }

        private static string _timesheetReport = ConfigurationManager.AppSettings["TimesheetReport"];
        public static List<int> TimesheetReport
        {
            get
            {
                if (string.IsNullOrEmpty( _timesheetReport))
                    _timesheetReport = ConfigurationManager.AppSettings["TimesheetReport"];

                return _timesheetReport.Split(',').Select(r => int.Parse(r)).ToList();
            }
        }

        /// <summary>
        /// ComplainNotify
        /// </summary>
        public static string ComplainNotifyList
        {
            get
            {
                return GetAppSetting("ComplainNotifyList");
            }
        }

        public static string PtoAdmin
        {
            get
            {
                return GetAppSetting("PtoAdmin");
            }
        }
    }
}
