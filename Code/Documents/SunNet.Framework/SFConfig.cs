using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using SF.Framework.Helpers;
using StructureMap;
using System.Web;
using SF.Framework.Encrypt;
using SF.Framework.Cache;
using SF.Framework.IDMap;
using SF.Framework.Log;
using SF.Framework.Log.Providers;
using SF.Framework.File.Providers;
using SF.Framework.EmailSender.Providers;
using SF.Framework.StringZipper.Providers;
using SF.Framework.SysDateTime.Providers;
using SF.Framework.IDMap.Providers;
using SF.Framework.Cache.Providers;
using SF.Framework.File;
using SF.Framework.EmailSender;
using SF.Framework.SysDateTime;
using SF.Framework.StringZipper;
using SF.Framework.Encrypt.Providers;
using SF.Framework.Runtime;
using SF.Framework.IoCConfiguration;

namespace SF.Framework
{
    public class SFConfig
    {
        public static void Init()
        {
        }

        private static IIoCConfigure configure = new DefaultIoCConfigure();
        static SFConfig()
        {
            IoC_Required(configure);
        }
        public static void IoC_Required(IIoCConfigure config)
        {
            configure = config;
            ObjectFactory.Configure(x =>
            {
                x.For<ILog>().Singleton().Use(configure.Log);
                x.For<IFile>().Singleton().Use(configure.File);
                x.For<IEmailSender>().Singleton().Use(configure.EmailSender);
                x.For<ISystemDateTime>().Singleton().Use(configure.SystemDateTime);
                x.For<IStringZipper>().Singleton().Use(configure.StringZipper);
                x.For<IEncrypt>().Singleton().Use(configure.Encrypt);
                x.For<ICache>().Singleton().Use(configure.Cache);
            });
        }
        public static void IoC_ExternalXMLFile(ConfigurationExpression x)
        {
            string xmlPath = GetIoCInjectionXMLPath();
            IoCInjection.LoadInjectionFromXmlFile(xmlPath, x);
        }

        public static Runtime.Components Components = new Runtime.Components();

        private static string GetIoCInjectionXMLPath()
        {
            string xmlFileName = "IoCMap.xml";
            if (HttpContext.Current != null)
                return HttpContext.Current.Server.MapPath("~/" + xmlFileName);
            else
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFileName);
        }

        #region Family Book
        public static string FilePhysicalUrl
        {
            get
            {
                return GetAppSetting("FilePhysicalUrl");
            }
        }

        public static string FileVirtualPath
        {
            get
            {
                return GetAppSetting("FileVirtualPath");
            }
        }

        public static string Ffmpeg
        {
            get
            {
                return GetAppSetting("Ffmpeg");
            }
        }

        public static string CatchFlvImgSize
        {
            get
            {
                return GetAppSetting("CatchFlvImgSize");
            }
        }

        public static int InitStorageSpace
        {
            get
            {
                return CommonHelper.ToInt(GetAppSetting("InitStorageSpace"));
            }
        }

        public static bool IsShowDeletedUser
        {
            get
            {
                return Boolean.Parse(GetAppSetting("IsDisplayDeletedUser"));
            }
        }
        #endregion

        public static bool LogEnabled
        {
            get
            {
                return GetAppSettingByBool("LogEnabled");
            }
        }

        public static int LogFileSize
        {
            get
            {
                return CommonHelper.ToInt(GetAppSetting("LogFileSize"));
            }
        }

        public static string LogFileName
        {
            get
            {
                return GetAppSetting("LogFileName");
            }
        }

        public static string FromEmailAddress
        {
            get
            {
                return GetAppSetting("FromEmailAddress");
            }
        }

        public static string EVOPDFKEY
        {
            get
            {
                return GetAppSetting("EVOPDFKEY");
            }
        }

        public static string WebSite
        {
            get
            {
                return GetAppSetting("WebSite");
            }
        }
        public static int CookieTimeOut
        {
            get
            {
                return CommonHelper.ToInt(GetAppSetting("CookieTimeOut"));
            }
        }
        public static bool TestMode
        {
            get
            {
                if (GetAppSetting("TestMode").Equals("1"))
                    return true;
                return false;
            }
        }


        public static string EmailDisplayName
        {
            get
            {
                return GetAppSetting("EmailDisplayName");
            }
        }

        public static bool EnableSSL
        {
            get
            {
                return GetAppSettingByBool("EnableSSL");
            }
        }

        public static string USPSUserID
        {
            get
            {
                return GetAppSetting("USPSUserID");
            }
        }

        public static string USPSZipCodeURI
        {
            get
            {
                return GetAppSetting("USPSZipCodeURI");
            }
        }

        public static string TestToFaxNumber
        {
            get
            {
                return GetAppSetting("TestToFaxNumber");
            }
        }

        public static string TestASQEmailAddress
        {
            get
            {
                return GetAppSetting("TestASQEmailAddress");
            }
        }

        public static string AccessID
        {
            get
            {
                return GetAppSetting("AccessID");
            }
        }

        public static string AccessPwd
        {
            get
            {
                return GetAppSetting("AccessPwd");
            }
        }

        public static string SCallerID
        {
            get
            {
                return GetAppSetting("SCallerID");
            }
        }

        public static string SenderEmail
        {
            get
            {
                return GetAppSetting("SenderEmail");
            }
        }

        public static string EFaxType
        {
            get
            {
                return GetAppSetting("EFaxType");
            }
        }

        public static string Retries
        {
            get
            {
                return GetAppSetting("Retries");
            }
        }

        /// <summary>
        /// 邮件链接过期时间（单位：小时）
        /// </summary>
        /// <value>
        /// The email link expired.
        /// </value>
        public static int EmailLinkExpired
        {
            get
            {
                int result = 0;
                int.TryParse(GetAppSetting("EmailLinkExpired"), out result);
                return result;
            }
        }

        private static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private static bool GetAppSettingByBool(string key)
        {
            if (GetAppSetting(key).Equals("1"))
                return true;
            else if (GetAppSetting(key).Equals("0"))
                return false;
            else
                return Convert.ToBoolean(GetAppSetting(key));
        }

        #region  Google Api
        public static string GoogleClientID
        {
            get { return GetAppSetting("GoogleClientID"); }
        }
        public static string GoogleEmailAddress
        {
            get { return GetAppSetting("GoogleEmailAddress"); }
        }
        public static string GoogleClientSecret
        {
            get { return GetAppSetting("GoogleClientSecret"); }
        }
        public static string GoogleRedirectURI
        {
            get { return SFConfig.WebSite + GetAppSetting("GoogleRedirectURI"); }
        }
        public static string GoogleTokenURI
        {
            get { return GetAppSetting("GoogleTokenURI"); }
        }
        public static string GoogleUserinfoURI
        {
            get { return GetAppSetting("GoogleUserinfoURI"); }
        }
        public static string GoogleRegisterURI
        {
            get { return GetAppSetting("GoogleRegisterURI"); }
        }
        #endregion

        /// <summary>
        /// personal file path
        /// </summary>
        public static string UploadFile
        {
            get { return GetAppSetting("UploadFile"); }
        }

        #region LDAP
        public static string LDAP
        {
            get { return GetAppSetting("LDAP"); }
        }
        public static string LDAP_DB
        {
            get { return GetAppSetting("LDAP_DB"); }
        }
        public static string LDAP_DBUser
        {
            get { return GetAppSetting("LDAP_DBUser"); }
        }
        public static string LDAP_DBPwd
        {
            get { return GetAppSetting("LDAP_DBPwd"); }
        }
        public static string LDAPUrl
        {
            get { return GetAppSetting("LDAPUrl"); }
        }
        #endregion

        /// <summary>
        /// 项目为HR的ID，用于添加Events时判断是否显示Off选项
        /// </summary>
        public static string HRProjectID
        {
            get { return GetAppSetting("HRProjectID"); }
        }

        public static string HRTicketID
        {
            get { return GetAppSetting("HRTicketID"); }
        }
    }
}
