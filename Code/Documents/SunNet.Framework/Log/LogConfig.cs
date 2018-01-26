using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.Log
{
    public class LogConfig
    {
        public virtual string GetLogMessageFormat()
        {
            string msg = @"{Now}
------------------------------------------------------------------------------------------------
{Message}
------------------------------------------------------------------------------------------------\
";
            return msg;
        }
    }
}
