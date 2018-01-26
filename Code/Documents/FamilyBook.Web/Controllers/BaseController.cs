using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyBook.Business;
using FamilyBook.Common;
using FamilyBook.Web.Channels;
using SF.Framework.Utils;

namespace FamilyBook.Web.Controllers
{
    public class BaseController : Controller
    {

        private bool IsOut = false;

        protected BaseController()
        {
            ViewBag.LoginUserName = LoginUserName;
            IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
            string userId = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("LoginUserID")));
            if (string.IsNullOrEmpty(userId))
            {
                IsOut = true;
            }
            ResumeCookie();
        }

        public int UserID
        {
            get
            {
                IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                string userId = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("LoginUserID")));
                if (string.IsNullOrEmpty(userId))
                {
                    IsOut = true;
                    return 0;
                }
                ResumeCookie();
                return Convert.ToInt32(userId);
            }
        }

        public string UserType
        {
            get
            {
                IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                string userType = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("UserType")));
                
                ResumeCookie();
                return userType;
            }
        }

        public int CompanyID
        {
            get
            {
                IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                string companyId = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("CompanyID")));
                if (string.IsNullOrEmpty(companyId))
                {
                    return 0;
                }
                ResumeCookie();
                return Convert.ToInt32(companyId);
            }
        }
        public string LoginUserName
        {
            get
            {
                IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                string firstName = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("FirstName")));
                if (string.IsNullOrEmpty(firstName))
                {
                    IsOut = true;
                }
                ResumeCookie();
                string lastName = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("LastName")));
                return firstName + " " + lastName;
            }
        }

        private void ResumeCookie()
        {
            IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
            string userType = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("UserType")));
            if (userType != "SUNNET")
            {
                UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("LoginUserID"), 30);
                UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("FirstName"), 30);
                UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("LastName"), 30);
                UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("CompanyID"), 30);
                UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("UserType"), 30);
                UtilFactory.Helpers.CookieHelper.ResumeExpire(encrypt.Encrypt("ExpireTime"), 30);
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IsOut)
            {
                filterContext.Result = new RedirectResult(SF.Framework.SFConfig.WebSite);
            }
        }
    }
}
