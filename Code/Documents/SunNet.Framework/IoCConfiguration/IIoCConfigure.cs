using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Framework.Log;
using SF.Framework.File;
using SF.Framework.EmailSender;
using SF.Framework.SysDateTime;
using SF.Framework.StringZipper;
using SF.Framework.Encrypt;
using SF.Framework.Cache;

namespace SF.Framework.IoCConfiguration
{
    public interface IIoCConfigure
    {
        ILog Log { get; }
        IFile File { get; }
        IEmailSender EmailSender { get; }
        ISystemDateTime SystemDateTime { get; }
        IStringZipper StringZipper { get; }
        IEncrypt Encrypt { get; }
        ICache Cache { get;}
    }
}
