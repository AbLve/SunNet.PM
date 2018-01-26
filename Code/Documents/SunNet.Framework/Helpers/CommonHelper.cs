using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Reflection;

namespace SF.Framework.Helpers
{
    public static class CommonHelper
    {
        public static int ToInt(object obj)
        {
            int ret;
            if (int.TryParse(Convert.ToString(obj), out ret) == false)
                ret = 0;

            return ret;
        }
        public static int ToInt(object obj, int defaultValue)
        {
            int ret;
            if (int.TryParse(Convert.ToString(obj), out ret) == false)
                ret = 0;

            return ret == 0 ? defaultValue : ret;
        }
        public static decimal ToDecimal(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return decimal.Zero;
            else
                return Convert.ToDecimal(obj.ToString().Replace(",", ""));
        }
        public static DateTime ToDateTime(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return DateTime.MinValue;
            else if (IsDate(obj.ToString()))
                return Convert.ToDateTime(obj);
            else
                return DateTime.MinValue;
        }
        public static Guid ToGuid(object obj)
        {
            Guid ret = Guid.Empty;
            if (obj == null)
                return Guid.Empty;
            try
            {
                string tmp = obj.ToString();
                Regex reg = new Regex("^[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}$", RegexOptions.IgnoreCase);
                if (reg.IsMatch(tmp))
                    ret = new Guid(obj.ToString());
                else
                    ret = Guid.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ret;
        }
        public static string CutString(string source, int length)
        {
            if (source.Length <= length)
                return source;
            else
                return source.Substring(0, length) + "...";
        }
        public static string CutString(object obj, int length)
        {
            if (obj == null)
                return "&nbsp;";
            if (obj.ToString().Length <= length)
                return obj.ToString();
            else
                return obj.ToString().Substring(0, length) + "...";
        }
        public static bool IsDate(object obj)
        {
            try
            {
                if (obj == null || obj == DBNull.Value)
                    return false;
                DateTime dt = Convert.ToDateTime(obj.ToString());
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool IsEmail(string email)
        {
            String EmailPattern = @"^[a-z]([a-z0-9]*[-_]?[a-z0-9]+)*@([a-z0-9]*[-_]?[a-z0-9]+)+[\.][a-z]{2,3}([\.][a-z]{2})?$";
            Regex regex = new Regex(EmailPattern, RegexOptions.IgnoreCase);
            if (regex.Match(email).Success)
                return true;
            else
                return false;
        }
        public static bool IsPhone(string phone)
        {
            String EmailPattern = @"^\(\d{3}\) ?\d{3}( |-)?\d{4}|^\d{3}( |-)?\d{3}( |-)?\d{4}";
            Regex regex = new Regex(EmailPattern, RegexOptions.IgnoreCase);
            if (regex.Match(phone).Success)
                return true;
            else
                return false;
        }
        public static bool IsEqual<T>(T x, T y)
        {
            PropertyInfo[] infos = typeof(T).GetProperties();
            foreach (PropertyInfo info in infos ?? new PropertyInfo[0])
            {
                if (info.GetValue(x, new object[0]) == null || info.GetValue(y, new object[0]) == null)
                { }
                else
                {
                    if (info.GetValue(x, new object[0]).GetHashCode() != info.GetValue(y, new object[0]).GetHashCode())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsDuplicateItemExists(List<string> list)
        {
            var linq =
                        from entity in list
                        group entity by new { Title = entity } into entityGroup
                        where entityGroup.Count() > 1
                        select entityGroup;
            if (linq.ToList().Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static  string ToIds(this List<int> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list == null || list.Count ==0) return "";
            foreach (int i in list)
                sb.AppendFormat("{0},", i);
            return sb.ToString(0, sb.Length - 1);
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static DateTime PaseDate(string date, DateTime v)
        {
            DateTime d;
            if (DateTime.TryParse(date, out d))
                return d;
            else return v;
        }

        /// <summary>
        /// @remark 根据参数转换类型，如果参数为空 或者 为1/1/1753， 则返回""，否则返回日期的MM/dd/yyyy
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertNullTime(DateTime? dt)
        {
            if (dt == null || dt.Value == DateTime.Parse("1/1/1753"))
            {
                return "";
            }
            else
            {
                return dt.Value.ToString("MM/dd/yyyy");
            }
        }
    }
}
