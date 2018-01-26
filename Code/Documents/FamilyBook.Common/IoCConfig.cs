using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using SF.Framework.IoCConfiguration;
using SF.Framework.Log;
using SF.Framework.File;
using SF.Framework.EmailSender;
using SF.Framework.SysDateTime;
using SF.Framework.StringZipper;
using SF.Framework.Encrypt;
using SF.Framework.Cache;

namespace FamilyBook.Common
{
    public class SFConfig
    {
        public static void Init()
        {
        }

        private static IIoCConfigure configure = new DefaultIoCConfigure();

        static SFConfig()
        {
            ObjectFactory.Initialize(x =>
            {
                // Tell StructureMap to look for configuration
                // from the App.config file
                // The default is false
                //x.PullConfigurationFromAppConfig = true;

                // We put the properties for an NHibernate ISession
                // in the StructureMap.config file, so this file
                // must be there for our application to
                // function correctly
                x.UseDefaultStructureMapConfigFile = true;
            });

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
    }
}
