using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework;

namespace SunNet.PMNew.EmailSender
{
    class Program
    {
        static string GetLogContent(string content)
        {
            return string.Format("TimeSheetNoticeLog:{0}--------------------------{1}", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss fff"), content);
        }
        static void Main(string[] args)
        {
            WebLogAgent.Write(GetLogContent("Start Send"));
            try
            {
                var notice = new SunNet.PMNew.EmailSender.TimeSheetNoticeManager.TimeSheetNotcieSoapClient();
                string result = notice.SendTimeSheetEmail(Config.TimesheetNoticeSvrPw);
                WebLogAgent.Write(GetLogContent("Sent, return flag is " + result));
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(GetLogContent("Exception occured, exception: " + ex.Message));
            }
            finally
            {
                WebLogAgent.Write(GetLogContent("End Send"));
            }
        }
    }
}
