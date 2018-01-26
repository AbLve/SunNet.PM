using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Framework.Utils.Helpers;
using SF.Framework.Utils.Providers;

namespace SF.Framework.Utils
{
    public class UtilFactory
    {
        public static class Helpers
        {
            private static CookieHelper cookieHelper = null;
            public static CookieHelper CookieHelper
            {
                get
                {
                    if (cookieHelper == null)
                        cookieHelper = new CookieHelper();
                    return cookieHelper;
                }
            }


        }

        private static IEncrypt desencrypt = null;
        private static IEncrypt md5Provider = null;
        public static IEncrypt GetEncryptProvider(EncryptType encryptType)
        {
            if (encryptType == EncryptType.DES)
            {
                if (desencrypt == null)
                    desencrypt = new Providers.DESEncrypt();
                return desencrypt;
            }
            else if (encryptType == EncryptType.MD5)
            {
                if (md5Provider == null)
                    md5Provider = new Providers.MD5Encrypt();
                return md5Provider;
            }
            return null;
        }
    }
}
