using Newtonsoft.Json.Converters;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.Web.Do
{
    public class DoBase
    {
        public static IsoDateTimeConverter DateConverter
        {
            get
            {
                return new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy" };
            }
        }

        protected readonly UserApplication _userApp;
        public DoBase(UserApplication userApp)
        {
            _userApp = userApp;
        }
        public DoBase()
            : this(new UserApplication())
        {

        }

        private UsersEntity _user;
        protected UsersEntity UserInfo
        {
            get
            {
                if (_user == null)
                {
                    IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                    string userID = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("LoginUserID")));
                    if (string.IsNullOrEmpty(userID))
                    {
                        return null;
                    }
                    int id = int.Parse(userID);
                    _user = _userApp.GetUser(id);
                    if (_user.UserType!= "SUNNET")
                    {
                        UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("LoginUserID"), 30);
                    }
                }
                return _user;
            }
        }

        protected int UserID
        {
            get
            {
                IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                string strUserID = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("LoginUserID")));
                int result = 0;
                int.TryParse(strUserID, out result);
                return result;
            }
        }
        
        protected int QS(string requestValue, int v)
        {
            int result;
            if (int.TryParse(requestValue, out result))
                return result;
            return v;
        }

        protected int QF(string formValue, int v)
        {
            int result;
            if (int.TryParse(formValue, out result))
                return result;
            return v;
        }

    }
}