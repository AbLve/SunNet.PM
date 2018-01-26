using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Web.Do;

namespace SunNet.PMNew.PM2014.Service
{
    /// <summary>
    /// GetCookieData 的摘要说明
    /// </summary>
    public class GetCookieData : DoBase, IHttpHandler,System.Web.SessionState.IRequiresSessionState
    {
        public class CookieData
        {
             public string UserType { get; set; }
             public DateTime? ExpireDate { get; set; }
             public string UserId { get; set; }

            /// <summary>
            /// 距离过期秒数(UTC)
            /// </summary>
            public int IntervaSeconds { get; set; }

        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var userType =UtilFactory.GetEncryptProvider(EncryptType.DES).Decrypt(UtilFactory.Helpers.CookieHelper.Get(UtilFactory.GetEncryptProvider(EncryptType.DES).Encrypt("UserType")));
            var expireDate = UtilFactory.Helpers.CookieHelper.Get(UtilFactory.GetEncryptProvider(EncryptType.DES).Encrypt("ExpireTime"));
            var userId = UtilFactory.Helpers.CookieHelper.Get(UtilFactory.GetEncryptProvider(EncryptType.DES).Encrypt("LoginUserID"));
            var utcTimeStampString = UtilFactory.Helpers.CookieHelper.Get(UtilFactory.GetEncryptProvider(EncryptType.DES).Encrypt("UtcTimeStamp"));
            double utcTimeStamp = 0;
            double.TryParse(utcTimeStampString, out utcTimeStamp);

            var cookieData = new CookieData
            {
                UserType = userType,
                ExpireDate =string.IsNullOrEmpty(expireDate)?  (DateTime?) null:Convert.ToDateTime(expireDate),
                UserId = userId,
                IntervaSeconds = GetIntervaSeconds(utcTimeStamp)
            };
            context.Response.Write(JsonConvert.SerializeObject(cookieData));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private int GetIntervaSeconds(double utcStamp)
        {
            double currentUtc = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            return (int)Math.Floor(utcStamp - currentUtc);
        }
    }
}