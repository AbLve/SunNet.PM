using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Framework.Utils.Providers;
using System.Text;
using SunNet.PMNew.Framework;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for TimeSheetNotcie
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TimeSheetNotcie : System.Web.Services.WebService
    {

        [WebMethod]
        public string SendTimeSheetEmail(string password)
        {
            if (password == Config.TimesheetNoticeSvrPw)
            {
                EmailExecuter execute = new EmailExecuter();
                bool ret = false;
                string country = "CN";
                try
                {
                    if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 11)
                    {
                        ret = execute.SendDailyCNMails();
                    }
                    else
                    {
                        ret = execute.SendDailyUSMails();
                        country = "US";
                    }
                    return string.Format("{0}-{1}", country, ret.ToString());
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
