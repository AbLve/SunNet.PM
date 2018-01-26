using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.Log
{
    public interface ILog
    {
        LogConfig Config { get; set; }
        void Log(Exception ex);
        void Log(string message);
    }
}
