using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Core.ComplaintModel.Interfaces;
using StructureMap;
using SunNet.PMNew.Impl.SqlDataProvider.Complaint;
using SunNet.PMNew.Entity.ComplaintModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Entity.ComplaintModel.Enums;

namespace SunNet.PMNew.PM2014.Api
{
    /// <summary>
    /// Summary description for ReportAbuse
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ReportAbuse : System.Web.Services.WebService
    {

        [WebMethod]
        public int Report(int type, int targetID, int reason, string additionalInfo, int systemID,
            int appSource, int reporterID, string reporterEmail, long timeStamp, string sign)
        {
            try
            {
                ISystemRepository systemRepository = ObjectFactory.GetInstance<ISystemRepository>();
                SystemEntity systemEntity = systemRepository.Get(systemID);
                string md5Key = systemEntity.MD5Key; // "MFBUY#!982015"
                
                if (additionalInfo == null) additionalInfo = "";
                if (reporterEmail == null) reporterEmail = "";

                string seed = "" + type + targetID + reason + additionalInfo + systemID + appSource + reporterID + reporterEmail + timeStamp;

                string localSign = UtilFactory.GetEncryptProvider(EncryptType.MD5).Encrypt(seed + md5Key);

                localSign = localSign.Replace("-", "");

                //Log seed and Local Sign
                WebLogAgent.Write(string.Format("[Complaint Seed: {0},\r\nLocalSign: {1}\r\nSign:{2}]",
                        seed,
                        localSign,
                        sign));

                if (localSign == sign.ToUpper())
                {
                    //Insert to dababas
                    ComplaintEntity complaintEntity = new ComplaintEntity();
                    complaintEntity.Type = type;
                    complaintEntity.TargetID = targetID;
                    complaintEntity.Reason = reason;
                    complaintEntity.AdditionalInfo = additionalInfo;
                    complaintEntity.SystemID = systemID;
                    complaintEntity.AppSrc = appSource;
                    complaintEntity.ReporterID = reporterID;
                    complaintEntity.ReporterEmail = reporterEmail;

                    RealSystemDateTime time = new RealSystemDateTime();
                    complaintEntity.CreatedOn = time.Now;

                    complaintEntity.Status = 1;

                    ComplaintApplication complaintApp = new ComplaintApplication();
                    int newComID = complaintApp.AddComplaint(complaintEntity);

                    try
                    {
                        //Send email
                        IEmailSender sender = ObjectFactory.GetInstance<IEmailSender>();
                        string emailTitle = string.Format("Complaint Received from {0} ", systemEntity.SystemName);
                        string emailContent = "Please check this URL: \r\n " + Config.AppDomain+ "/OA/Complaints/ComplaintReview.aspx?ComplaintID=" + newComID + "\r\n\r\n";
                        sender.SendMail(Config.ComplainNotifyList, Config.DefaultSendEmail, emailTitle, emailContent);
                    }
                    catch (Exception ex)
                    {
                        WebLogAgent.Write(string.Format("[Email Sending Exception]: {0}", ex.Message));
                    }
                    return 1; //Accepted, Successed
                }

                return 2; //Invalid
            }
            catch (Exception ex)
            {
                //log this excption
                WebLogAgent.Write(string.Format("[Exception]: {0}", ex.Message));

                return 3;//System Error

            }
        }
    }
}
