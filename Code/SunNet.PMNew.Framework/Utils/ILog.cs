using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Framework.Utils
{
    public interface ILog
    {
        void Log(Exception ex);
        void Log(string message);
    }
}
