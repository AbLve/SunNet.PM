using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.Web.Do
{
    public class DoBase
    {
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

        private UsersEntity _user;
        protected UsersEntity UserInfo
        {
            get
            {
                if (_user == null)
                {
                    _user = new App.UserApplication().GetUser(UserID);
                }
                return _user;
            }
        }
    }
}