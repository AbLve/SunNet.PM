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

namespace SunNet.PMNew.Framework.Utils.Helpers
{
    public class CommonHelper
    {
        public int ToInt(object obj)
        {
            int ret;
            if (int.TryParse(Convert.ToString(obj), out ret) == false)
                ret = 0;

            return ret;
        }
        public DateTime GetDefaultMinDate()
        {
            DateTime dt = Convert.ToDateTime("01/01/1753");
            return dt;
        }
        public bool TryIntParse(object obj)
        {
            int ret;
            if (int.TryParse(Convert.ToString(obj), out ret))
            {
                return true;
            }
            return false;
        }
        public string UrlEncode(string str)
        {
            if (str == null)
            {
                return null;
            }
            string s = System.Web.HttpUtility.UrlEncode(str, System.Text.Encoding.UTF8);
            return s;
        }
        public int ToInt(object obj, int defaultValue)
        {
            int ret;
            if (int.TryParse(Convert.ToString(obj), out ret) == false)
                ret = 0;

            return ret == 0 ? defaultValue : ret;
        }
        public decimal ToDecimal(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return decimal.Zero;
            else
                return Convert.ToDecimal(obj.ToString().Replace(",", ""));
        }
        public DateTime ToDateTime(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return DateTime.MinValue;
            else if (IsDate(obj.ToString()))
                return Convert.ToDateTime(obj);
            else
                return DateTime.MinValue;
        }
        public Guid ToGuid(object obj)
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
        public string CutString(string source, int length)
        {
            if (source.Length <= length)
                return source;
            else
                return source.Substring(0, length) + "...";
        }
        public string CutString(object obj, int length)
        {
            if (obj == null)
                return "&nbsp;";
            if (obj.ToString().Length <= length)
                return obj.ToString();
            else
                return obj.ToString().Substring(0, length) + "...";
        }
        public bool IsDate(object obj)
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
        public bool IsEmail(string email)
        {
            String EmailPattern = @"^[a-z]([a-z0-9]*[-_]?[a-z0-9]+)*@([a-z0-9]*[-_]?[a-z0-9]+)+[\.][a-z]{2,3}([\.][a-z]{2})?$";
            Regex regex = new Regex(EmailPattern, RegexOptions.IgnoreCase);
            if (regex.Match(email).Success)
                return true;
            else
                return false;
        }
        public bool IsPhone(string phone)
        {
            String EmailPattern = @"^\(\d{3}\) ?\d{3}( |-)?\d{4}|^\d{3}( |-)?\d{3}( |-)?\d{4}";
            Regex regex = new Regex(EmailPattern, RegexOptions.IgnoreCase);
            if (regex.Match(phone).Success)
                return true;
            else
                return false;
        }
        public bool IsEqual<T>(T x, T y)
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
    }
}
