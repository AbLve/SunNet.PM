using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Framework.Log;

namespace SF.Framework.ExceptionManager
{
    public class ExceptionPolicy
    {
        public static bool Handle(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            WebLogAgent.Write(ex);
            return false;
        }
    }
}
