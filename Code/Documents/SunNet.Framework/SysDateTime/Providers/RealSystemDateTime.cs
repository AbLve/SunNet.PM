using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.SysDateTime.Providers
{
    public class RealSystemDateTime:ISystemDateTime
    {
        public DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}
