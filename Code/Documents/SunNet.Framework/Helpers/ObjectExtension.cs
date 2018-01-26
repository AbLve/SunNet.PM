using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Framework.Log;

namespace SF.Framework.Helpers
{
    public static class ObjectExtension
    {
        public static int ToInt32(this object obj)
        {
            int result = 0;
            int.TryParse(obj + "", out result);
            return result;
        }

        public static int ToInt16(this object obj)
        {
            int result = 0;
            int.TryParse(obj + "", out result);
            return result;
        }

        public static double ToDouble(this object obj)
        {
            double result = 0;
            double.TryParse(obj + "", out result);
            return result;
        }

        public static bool ToBoolean(this object obj)
        {
            bool result = false;
            bool.TryParse(obj + "", out result);
            return result;
        }

        public static string ToString(this object obj)
        {
            try
            {
                return Convert.ToString(obj);
            }
            catch
            {
                return "";
            }
        }

        public static DateTime ToDateTime(this object obj)
        {
            DateTime defaultDateTime = new DateTime(1753, 1, 1);
            DateTime.TryParse(obj + "", out defaultDateTime);
            return defaultDateTime;
        }
    }
}
