using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Framework.Utils
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



            private static FileHelper fileHelper = null;
            public static FileHelper FileHelper
            {
                get
                {
                    if (fileHelper == null)
                        fileHelper = new FileHelper();
                    return fileHelper;
                }
            }


            private static HtmlHelper htmlHelper = null;
            public static HtmlHelper HtmlHelper
            {
                get
                {
                    if (htmlHelper == null)
                        htmlHelper = new HtmlHelper();
                    return htmlHelper;
                }
            }


            private static JSONHelper jsonHelper = null;
            public static JSONHelper JSONHelper
            {
                get
                {
                    if (jsonHelper == null)
                        jsonHelper = new JSONHelper();
                    return jsonHelper;
                }
            }


            private static SqlHelper sqlHelper = null;
            public static SqlHelper SqlHelper
            {
                get
                {
                    if (sqlHelper == null)
                        sqlHelper = new SqlHelper();
                    return sqlHelper;
                }
            }


            private static ThumbnailHelper thumbnailHelper = null;
            public static ThumbnailHelper ThumbnailHelper
            {
                get
                {
                    if (thumbnailHelper == null)
                        thumbnailHelper = new ThumbnailHelper();
                    return thumbnailHelper;
                }
            }


            private static XMLSerializerHelper xmlSerializerHelper = null;
            public static XMLSerializerHelper XMLSerializerHelper
            {
                get
                {
                    if (xmlSerializerHelper == null)
                        xmlSerializerHelper = new XMLSerializerHelper();
                    return xmlSerializerHelper;
                }
            }


            private static CommonHelper commonHelper = null;
            public static CommonHelper CommonHelper
            {
                get
                {
                    if (commonHelper == null)
                        commonHelper = new CommonHelper();
                    return commonHelper;
                }
            }
        }

        private static Dictionary<Type, object> caches = new Dictionary<Type, object>();
        public static ICache<T> GetCacheProvider<T>()
        {
            ICache<T> cache = null;
            if (!caches.ContainsKey(typeof(T)))
            {
                cache = new HttpRuntimeCache<T>();
                caches.Add(typeof(T), cache);
            }
            else
            {
                cache = caches[typeof(T)] as ICache<T>;
            }
            return cache;
        }


        private static Dictionary<Type, object> idMaps = new Dictionary<Type, object>();
        public static IIDMap<T> GetIDMapProvider<T>()
        {
            IIDMap<T> idMap = null;
            if (!idMaps.ContainsKey(typeof(T)))
            {
                idMap = new DictionaryIDMap<T>();
                idMaps.Add(typeof(T), idMap);
            }
            else
            {
                idMap = idMaps[typeof(T)] as IIDMap<T>;
            }
            return idMap;
        }


        private static IEmailSender emailSender = null;
        public static IEmailSender EmailSenderProvider
        {
            get
            {
                if (emailSender == null)
                    emailSender = new Providers.SmtpClientEmailSender(LogProvider);
                return emailSender;
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

        private static IFile fileProvider = null;
        public static IFile FileProvider
        {
            get
            {
                if (fileProvider == null)
                    fileProvider = new Providers.RealFileSystem();
                return fileProvider;
            }
        }


        private static ILog logProvider = null;
        public static ILog LogProvider
        {
            get
            {
                if (logProvider == null)
                    logProvider = new Providers.TextFileLogger();
                return logProvider;
            }
        }


        private static IStringZipper stringZipperProvider = null;
        public static IStringZipper StringZipperProvider
        {
            get
            {
                if (stringZipperProvider == null)
                    stringZipperProvider = new Providers.ICSharpCodeStringZipper();
                return stringZipperProvider;
            }
        }


        private static ISystemDateTime systemDateTimeProvider = null;
        public static ISystemDateTime SystemDateTimeProvider
        {
            get
            {
                if (systemDateTimeProvider == null)
                    systemDateTimeProvider = new Providers.RealSystemDateTime();
                return systemDateTimeProvider;
            }
        }
    }
}
