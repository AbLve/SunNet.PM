using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Framework.Log.Providers;
using SF.Framework.File.Providers;
using SF.Framework.EmailSender.Providers;
using SF.Framework.SysDateTime.Providers;
using SF.Framework.StringZipper.Providers;
using SF.Framework.Encrypt.Providers;
using SF.Framework.Cache.Providers;

namespace SF.Framework.IoCConfiguration
{
    public class DefaultIoCConfigure : IIoCConfigure
    {
        public virtual Log.ILog Log
        {
            get 
            {
                Log.ILog log = new TextFileLogger();
                log.Config = new Log.LogConfig();
                return log;
            }
        }

        public virtual File.IFile File
        {
            get { return new RealFileSystem(); }
        }

        public virtual EmailSender.IEmailSender EmailSender
        {
            get { return new SmtpClientEmailSender(this.Log); }
        }

        public virtual SysDateTime.ISystemDateTime SystemDateTime
        {
            get { return new RealSystemDateTime(); }
        }

        public virtual StringZipper.IStringZipper StringZipper
        {
            get { return new CSharpCodeStringZipper(); }
        }

        public virtual Encrypt.IEncrypt Encrypt
        {
            get { return new DESEncrypt(); }
        }

        public virtual Cache.ICache Cache
        {
            get { return new HttpRuntimeCache(); }
        }
    }
}
