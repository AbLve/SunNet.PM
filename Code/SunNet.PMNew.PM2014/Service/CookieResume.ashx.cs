using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.PM2014.Service
{
    /// <summary>
    /// CookieResume 的摘要说明
    /// </summary>
    public class CookieResume : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //UtilFactory.Helpers.CookieHelper.ResumeCookie();

            //旧方法有时间问题
            IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
            UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("ExpireTime"), DateTime.Now.ToUniversalTime().AddMinutes(30).ToString(), DateTime.Now.AddMinutes(30));
            UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("UtcTimeStamp"), (DateTime.UtcNow.AddMinutes(30) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString(), DateTime.Now.AddMinutes(30));
            context.Response.Write("true");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}