using System;
using System.Text;
using System.Web;

namespace SF.Framework.Utils.Helpers
{
    public class CookieHelper
    {
        #region Check Parameters
        private  void CheckKey(string key)
        {
            if (key == null || key == string.Empty)
            {
                throw new ArgumentNullException("key", "key should never be null or empty.");
            }
        }

        private  void CheckValue(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "value should never be null or empty.");
            }
        }
        #endregion

        #region Add Cookie
        private  void Add(string key, string value, int slidingMinutes, string domainName, string path)
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

        public  void Add(string key, string value, DateTime expirationDate)
        {
            CheckKey(key);
            CheckValue(value);

            HttpCookie responseCookie = new HttpCookie(key);
            responseCookie.Value = HttpContext.Current.Server.UrlEncode(value);

            responseCookie.Expires = expirationDate;

            HttpContext.Current.Response.Cookies.Add(responseCookie);
        }

        public  void Add(string key, string value, int slidingMinutes)
        {
            Add(key, value, slidingMinutes, "", "");
        }

        public  void Add(string key, string value)
        {
            Add(key, value, -1, "", "");
        }

        #endregion

        #region Get Cookie Value
        public  string Get(string key)
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
        #endregion

        #region Remove Cookie
        public  void Remove(string key)
        {
            CheckKey(key);

            HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddDays(-10);
        }

        public  void RemoveAll()
        {
            foreach (string key in HttpContext.Current.Request.Cookies.AllKeys)
            {
                HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddDays(-10);
            }
        }
        #endregion

        public  void Resume(string key, int slidingMinutes)
        {
            CheckKey(key);

            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddMinutes(slidingMinutes);
                HttpContext.Current.Response.Cookies.Add(cookie);
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
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }
}
