using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// TimeSheetNotcie 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://pm.sunnet.us/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class TimeSheetNotcie : System.Web.Services.WebService
    {

        [WebMethod]
        public string SendTimeSheetEmail(string password)
        {
            if (password == Config.TimesheetNoticeSvrPw)
            {
                EmailExecuter execute = new EmailExecuter();
                bool cnresult = false;
                bool usresult = false;
                string country = "";
                try
                {
                    int start = Config.TimesheetNoticeTimeStart;
                    if (DateTime.Now.Hour >= start && DateTime.Now.Hour < start + 1)
                    {
                        usresult = execute.SendDailyUSMails(DateTime.Now.AddDays(-1));
                        cnresult = execute.SendDailyCNMails(DateTime.Now.AddDays(-1));
                        //country += "AO: " + execute.SendDailyMail(DateTime.Now.AddDays(-1), "AO");
                        //country += ", D1: " + execute.SendDailyMail(DateTime.Now.AddDays(-1), "D1");
                        //country += ", D2: " + execute.SendDailyMail(DateTime.Now.AddDays(-1), "D2");
                    }
                    return string.Format("CN:{0},US:{1}, {2}", cnresult, usresult, country);
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(ex);
                    return ex.Message;
                }
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("###################Failed timesheet notice access###########")
                             .AppendFormat("IP: {0}", Context.Request.UserHostAddress)
                             .AppendFormat("Hostname: {0}", Context.Request.UserHostName)
                             .AppendFormat("Referer: {0}", Context.Request.UrlReferrer)
                             .AppendFormat("Password: {0}", password)
                             .Append("#############################################################");
                WebLogAgent.Write(stringBuilder.ToString());
                return false.ToString();
            }
        }
    }
}
