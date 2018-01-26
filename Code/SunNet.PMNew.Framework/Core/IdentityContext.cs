using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Framework.Core
{
    public static class IdentityContext
    {
        private static IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
        private static SunNet.PMNew.Framework.Utils.Helpers.CookieHelper helper = new SunNet.PMNew.Framework.Utils.Helpers.CookieHelper();
        public static int UserID
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    string cValue = helper.Get(encrypt.Encrypt("_ud_"));
                    if (cValue != null && cValue != string.Empty && cValue.Trim().Length > 0)
                    {
                        cValue = encrypt.Decrypt(cValue);
                        int result = 0;
                        if (Int32.TryParse(cValue.Trim(), out result))
                            return result;
                    }
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    helper.Add(encrypt.Encrypt("_ud_"), encrypt.Encrypt(value.ToString()));
                }
            }
        }

        public static int CompanyID
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    string cValue = helper.Get(encrypt.Encrypt("_companyId_"));
                    if (cValue != null && cValue != string.Empty && cValue.Trim().Length > 0)
                    {
                        cValue = encrypt.Decrypt(cValue);
                        int result = 0;
                        if (Int32.TryParse(cValue.Trim(), out result))
                            return result;
                    }
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    helper.Add(encrypt.Encrypt("_companyId_"), encrypt.Encrypt(value.ToString()));
                }
            }
        }
    }
}
