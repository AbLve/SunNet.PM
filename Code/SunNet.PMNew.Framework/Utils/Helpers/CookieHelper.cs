using System;
using System.Text;
using System.Web;

namespace SunNet.PMNew.Framework.Utils.Helpers
{
    public class CookieHelper
    {
        #region Check Parameters
        private void CheckKey(string key)
        {
            if (key == null || key == string.Empty)
            {
                throw new ArgumentNullException("key", "key should never be null or empty.");
            }
        }

        private void CheckValue(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "value should never be null or empty.");
            }
        }
        #endregion

        #region Add Cookie
        private void Add(string key, string value, int slidingMinutes, string domainName, string path)
        {
            CheckKey(key);
            CheckValue(value);

            HttpCookie responseCookie = new HttpCookie(key);
            responseCookie.Value = HttpContext.Current.Server.UrlEncode(value);
            //responseCookie.Secure = Config.SSLCookie;

            if (slidingMinutes > 0)
                responseCookie.Expires = DateTime.Now.AddMinutes(slidingMinutes);
            else
                responseCookie.Expires = DateTime.Now.AddMonths(1);//DateTime.MaxValue;

            if (!(domainName == null || domainName == string.Empty))
                responseCookie.Domain = domainName;

            HttpContext.Current.Response.Cookies.Add(responseCookie);
        }

        public void Add(string key, string value, DateTime expirationDate)
        {
            CheckKey(key);
            CheckValue(value);

            HttpCookie responseCookie = new HttpCookie(key);
            responseCookie.Value = HttpContext.Current.Server.UrlEncode(value);

            responseCookie.Expires = expirationDate;

            HttpContext.Current.Response.Cookies.Add(responseCookie);
        }

        public void Add(string key, string value, int slidingMinutes)
        {
            Add(key, value, slidingMinutes, "", "");
        }

        public void Add(string key, string value)
        {
            Add(key, value, -1, "", "");
        }

        public void Add(string key, string value, bool closeBrowser)
        {
            CheckKey(key);
            CheckValue(value);

            HttpCookie responseCookie = new HttpCookie(key);
            responseCookie.Value = HttpContext.Current.Server.UrlEncode(value);

            HttpContext.Current.Response.Cookies.Add(responseCookie);
        }

        #endregion

        #region Get Cookie Value
        public string Get(string key)
        {
            CheckKey(key);

            string value = string.Empty;

            HttpCookie requestCookie = HttpContext.Current.Request.Cookies[key];
            if (requestCookie != null)
            {
                value = HttpContext.Current.Server.UrlDecode(requestCookie.Value);
            }

            return value;
        }

        public  DateTime GetExpireTime(string key)
        {
            CheckKey(key);

            DateTime value = DateTime.Now;

            HttpCookie requestCookie = HttpContext.Current.Request.Cookies[key];
            if (requestCookie != null)
            {
                value = requestCookie.Expires;
            }
            return value;
        }

        public void ResumeCookie()
        {
            IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);

            UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("LoginUserID"), 30);
            UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("FirstName"), 30);
            UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("LastName"), 30);
            UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("CompanyID"), 30);
            UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("UserType"), 30);
            UtilFactory.Helpers.CookieHelper.ResumeExpire(encrypt.Encrypt("ExpireTime"), 30);
            UtilFactory.Helpers.CookieHelper.ResumeExpire(encrypt.Encrypt("UtcTimeStamp"), 30);
        }
        #endregion

        #region Remove Cookie
        public void Remove(string key)
        {
            CheckKey(key);

            HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddDays(-10);
        }

        public void RemoveAll()
        {
            foreach (string key in HttpContext.Current.Request.Cookies.AllKeys)
            {
                HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddDays(-10);
            }
        }
        #endregion

        public void Resume(string key, int slidingMinutes)
        {
            CheckKey(key);

            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddMinutes(slidingMinutes);
                MakeResponeCookieDistinct(cookie);
                //HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public void ResumeExpire(string key, int slidingMinutes)
        {
            CheckKey(key);

            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                cookie.Value = DateTime.Now.ToUniversalTime().AddMinutes(slidingMinutes).ToString();
                cookie.Expires = DateTime.Now.AddMinutes(slidingMinutes);
                MakeResponeCookieDistinct(cookie);
                //HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public void MakeResponeCookieDistinct(HttpCookie cookie)
        {
            HttpCookie responseCookie = HttpContext.Current.Response.Cookies[cookie.Name];
            if (responseCookie != null)
            {
                responseCookie.Expires = cookie.Expires;
                responseCookie.Value = cookie.Value;
            }
            else
            {
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

        }
    }
}
