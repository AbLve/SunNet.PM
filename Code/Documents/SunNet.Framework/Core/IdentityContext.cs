using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Framework.Helpers;
using System.Web;

namespace SF.Framework.Core
{
    public static class IdentityContext
    {
        //public static ILoginSession Session
        //{
        //    get
        //    {
        //        ILoginSession user = SessionContext<ILoginSession>.CurrentSession.Get();
        //        return user;
        //    }
        //    set
        //    {
        //        if (value == null)
        //            SessionContext<ILoginSession>.CurrentSession.Clear();
        //        else
        //            SessionContext<ILoginSession>.CurrentSession.Save(value);
        //    }
        //}
        public static string GoogleID
        {
            get
            {
                string cookiekey = SFConfig.Components.Encrypt.Encrypt("googleid");
                return SFConfig.Components.Encrypt.Decrypt(CookieHelper.Get(cookiekey));
            }
        }
        public static int Role
        {
            get
            {
                string cookiekey = SFConfig.Components.Encrypt.Encrypt("currentuser");
                return int.Parse(SFConfig.Components.Encrypt.Decrypt(CookieHelper.Get(cookiekey)));
            }
        }
        /// <summary>
        /// Administrator ID
        /// </summary>
        public static int Administrator
        {
            get
            {
                string cookiekey = SFConfig.Components.Encrypt.Encrypt("_admin_");
                string cookie = SFConfig.Components.Encrypt.Decrypt(CookieHelper.Get(cookiekey));
                if (string.IsNullOrEmpty(cookie))
                    return 0;
                return int.Parse(SFConfig.Components.Encrypt.Decrypt(CookieHelper.Get(cookiekey)));
            }
            set
            {
                string cookiekey = SFConfig.Components.Encrypt.Encrypt("_admin_");
                CookieHelper.Add(cookiekey, SFConfig.Components.Encrypt.Encrypt(value.ToString()), DateTime.Now.AddMinutes(30));
            }
        }
        /// <summary>
        /// Administrator ID or UserID
        /// </summary>
        public static int UserID
        {
            get
            {
                string cookiekey = SFConfig.Components.Encrypt.Encrypt("_uid_");
                string cookie = SFConfig.Components.Encrypt.Decrypt(CookieHelper.Get(cookiekey));
                if (string.IsNullOrEmpty(cookie))
                    return 0;
                return int.Parse(cookie);
            }
            set
            {
                string cookiekey = SFConfig.Components.Encrypt.Encrypt("_uid_");
                CookieHelper.Add(cookiekey, SFConfig.Components.Encrypt.Encrypt(value.ToString()), DateTime.Now.AddMinutes(30));
            }
        }
        private static ILogin Login
        {
            get
            {
                ILogin user = SessionContext<ILogin>.CurrentSession.Get();
                return user;
            }
            set
            {
                if (value == null)
                    SessionContext<ILogin>.CurrentSession.Clear();
                else
                    SessionContext<ILogin>.CurrentSession.Save(value);
            }
        }
        //public static ICompany Company
        //{
        //    get
        //    {
        //        ICompany company = SessionContext<ICompany>.CurrentSession.Get();
        //        return company;
        //    }
        //    set
        //    {
        //        if (value == null)
        //            SessionContext<ICompany>.CurrentSession.Clear();
        //        else
        //            SessionContext<ICompany>.CurrentSession.Save(value);
        //    }
        //}
    }
}