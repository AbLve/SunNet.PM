using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace SF.Framework.Mvc.Extension
{
    /// <summary>
    /// String expansion
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Database empty time.
        /// </summary>
        public static readonly DateTime NullSqlDateTime = ((DateTime)System.Data.SqlTypes.SqlDateTime.Null);

        /// <summary>
        /// String [] to int []
        /// </summary>
        /// <param name="source">string[]</param>
        /// <returns>int[]</returns>
        public static int[] ToIntArray(this string[] source)
        {
            int[] result = new int[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                int temp_int = 0;
                int.TryParse(source[i], out temp_int);
                result[i] = temp_int;
            }
            return result;
        }

        /// <summary>
        /// Remove an array of the same data.
        /// </summary>
        /// <param name="ArrInput">string array.</param>
        /// <returns></returns>
        public static string[] removeDuplicate(this string[] ArrInput)
        {
            ArrayList nStr = new ArrayList();
            for (int i = 0; i < ArrInput.Length; i++)
            {
                if (!nStr.Contains(ArrInput[i]))
                {
                    nStr.Add(ArrInput[i]);
                }
            }
            return (string[])nStr.ToArray(typeof(string));
        }

        #region Type Convert
        /// <summary>
        /// String convert to bool
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string source)
        {
            bool reValue;
            bool.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// String convert to byte
        /// </summary>
        /// <returns>Byte</returns>
        public static Byte ToByte(this string source)
        {
            Byte reValue;
            Byte.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// String convert to Short
        /// </summary>
        /// <returns>Short</returns>
        public static short ToShort(this string source)
        {
            short reValue;
            short.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// String convert to int16
        /// </summary>
        /// <returns>Short</returns>
        public static short ToInt16(this string source)
        {
            short reValue;
            short.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// String convert to int32
        /// </summary>
        /// <returns>int32</returns>
        public static int ToInt32(this string source)
        {
            int reValue;
            Int32.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// String convert to int64
        /// </summary>
        /// <returns>int64</returns>
        public static long ToInt64(this string source)
        {
            long reValue;
            Int64.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// String convert to Double
        /// </summary>
        /// <returns>decimal</returns>
        public static Double ToDouble(this string source)
        {
            Double reValue;
            Double.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// String convert to decimal
        /// </summary>
        /// <returns>decimal</returns>
        public static decimal ToDecimal(this string source)
        {
            decimal reValue;
            decimal.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// Into the date is empty to return to NullSqlDateTime, byark
        /// </summary>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTime(this string source)
        {
            DateTime reValue;
            if (DateTime.TryParse(source, out reValue))
                return reValue;
            else
                return NullSqlDateTime;
        }
        /// <summary>
        /// Into the date is empty to return to NullSqlDateTime, byark
        /// </summary>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTimeByNum(this string source)
        {
            //20050102010101
            DateTime reValue = NullSqlDateTime;
            if (source.Length == 14)
            {
                if (!DateTime.TryParse(source.Substring(0, 4) + "-" + source.Substring(4, 2) + "-" + source.Substring(6, 2) + " "
                    + source.Substring(8, 2) + ":" + source.Substring(10, 2) + ":" + source.Substring(12, 2), out reValue))
                    reValue = NullSqlDateTime;
            }
            return reValue;
        }
        /// <summary>
        /// Into digital type of date
        /// </summary>
        /// <returns>DateTime</returns>
        public static decimal ToDateTimeDecimal(this string source)
        {
            DateTime reValue;
            if (DateTime.TryParse(source, out reValue))
            {
                return reValue.ToString("yyyyMMddHHmmss").ToDecimal();
            }
            else
                return 0;
        }
        /// <summary>
        /// Time converted to digital
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal ToDateTimeDecimal(this DateTime source)
        {
            return source.ToString("yyyyMMddHHmmss").ToDecimal();
        }
        #endregion
        #region Validation
        /// <summary>Judgment is the correct E-mail format  or not. </summary>
        public static bool IsEmail(this string source)
        {
            return Regex.IsMatch(source, @"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", RegexOptions.Compiled);
        }

        /// <summary>Judgment is the correct postal code format</summary>
        public static bool IsPostcode(this string source)
        {
            return Regex.IsMatch(source, @"^[1-9]{1}(\d){5}$", RegexOptions.Compiled);
        }

        /// <summary>Judgment is the correct China mobile and China unicom phone.</summary>
        public static bool IsMobilePhone(this string source)
        {
            return Regex.IsMatch(source, @"^(86)*0*13\d{9}$", RegexOptions.Compiled);
        }

        /// <summary>Judgment is the correct Chinese fixed phone </summary>
        public static bool IsTelephone(this string source)
        {
            return Regex.IsMatch(source, @"^((\d{3,4})|\d{3,4}-|\s)?\d{8}$", RegexOptions.Compiled);
        }
        /// <summary>Judgment is the correct QQ number </summary>
        public static bool IsQQ(this string source)
        {
            return Regex.IsMatch(source, "[1-9][0-9]{4,}", RegexOptions.Compiled);
        }
        /// <summary>Judgment is the correct user name </summary>
        public static bool IsUserName(this string source)
        {
            if (source.Length < 4)
            {
                return false;
            }
            else if (source.Length > 16)
            {
                return false;
            }
            else
            {
                return Regex.IsMatch(source, "/^[a-zA-Z0-9_]{4,16}$/", RegexOptions.Compiled);
            }
        }
        /// <summary>Contains HTML</summary>
        public static bool IsHasHtml(this string source)
        {
            Regex reg = new Regex(@"<|>");
            if (reg.IsMatch(source))
                return true;
            return false;
        }
        /// <summary>Whether regular expression matching, matching returning true or false</summary>
        public static bool IsMatchRegex(this string source, string regex)
        {
            Regex r = new Regex(regex);
            if (r.IsMatch(source))
                return true;
            return false;
        }
        /// <summary>Judge whether string IP, if it is to return to True, not returns False</summary>
        public static bool IsIP(this string source)
        {
            //Regex regex = new Regex(@"(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))");
            Regex regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])"
                + @"\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$", RegexOptions.Compiled);
            if (regex.Match(source).Success)
                return true;
            else
                return false;
        }
        /// <summary>Does it include Chinese or full Angle character</summary>
        public static bool IsHasChinese(this string checkStr)
        {
            ASCIIEncoding n = new ASCIIEncoding();
            byte[] b = n.GetBytes(checkStr);
            for (int i = 0; i <= b.Length - 1; i++)
                if (b[i] == 63) return true;
            return false;
        }
        /// <summary>Judgment is site address</summary>
        public static bool IsURLAddress(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"[a-zA-z]+://[^s]*", RegexOptions.Compiled);
        }
        /// <summary>Judgment is Chinese</summary>
        public static bool IsAllChinese(this string checkStr)
        {
            checkStr = checkStr.Trim();
            if (checkStr == string.Empty) return false;
            Regex reg = new Regex(@"^([\u4e00-\u9fa5]*)$", RegexOptions.Compiled);
            if (reg.IsMatch(checkStr))
                return true;
            else
                return false;
        }
        /// <summary>Whether for positive integer</summary>
        public static bool IsInt(this string intStr)
        {
            Regex regex = new Regex(@"^\d+$", RegexOptions.Compiled);
            return regex.IsMatch(intStr.Trim());
        }
        /// <summary>Nonnegative integer</summary>
        public static bool IsIntWithZero(this string intStr)
        {
            return Regex.IsMatch(intStr, @"^\d+$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Whether the number (positive and negative Numbers and decimal)
        /// </summary>
        /// <param name="checkStr"></param>
        /// <returns></returns>
        public static bool IsNumber(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"^[+-]?[0123456789]*[.]?[0123456789]*$", RegexOptions.Compiled);
        }
        /// <summary>
        /// Whether it is digital, can take two decimal point, point before six
        /// </summary>
        /// <param name="checkStr"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"^(([1-9]\d{0,8})|0)(\.\d{1,2})?$", RegexOptions.Compiled);
        }
        /// <summary>
        /// Date is
        /// </summary>
        /// <param name="checkStr"></param>
        /// <returns></returns>
        //public static bool IsDateTime(this string checkStr)
        //{
        //    return Regex.IsMatch(checkStr, @"^[ ]*[012 ]?[0123456789]?[0123456789]{2}[ ]*[-]{1}[ ]*[01]?[0123456789]{1}[ ]*[-]{1}[ ]*[0123]?[0123456789]"
        //        + @"{1}[ ]*[012]?[0123456789]{1}[ ]*[:]{1}[ ]*[012345]?[0123456789]{1}[ ]*[:]{1}[ ]*[012345]?[0123456789]{1}[ ]*$", RegexOptions.Compiled);
        //}
        public static bool IsDateTime(this string checkStr)
        {
            bool res = false;
            DateTime timeTemp = new DateTime();
            res = DateTime.TryParse(checkStr, out timeTemp);
            return res;
        }
        /// <summary>To judge a XML 1.0 allowed character</summary>
        private static bool IsLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */        ||
                 character == 0xA /* == '\n' == 10  */        ||
                 character == 0xD /* == '\r' == 13  */        ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }

        /// <summary>To judge a legal XML 1.0 standard allow string true: standard false: contains no standard character</summary>
        public static bool IsLegalXmlChar(this string xml)
        {
            if (string.IsNullOrEmpty(xml)) return true;
            foreach (char c in xml)
                if (!IsLegalXmlChar(c)) return false;
            return true;
        }

        /// <summary>DropDownList judge whether a selected (default first value value is 1 used to indicate the user to select)</summary>
        public static bool SelectNone(this string valueStr)
        {
            if (string.IsNullOrEmpty(valueStr) || valueStr.Trim().Equals("-1")) return false;
            return true;
        }

        /// <summary>CheckBoxList judge whether a selected</summary>
        public static bool atLeastOneChecked(this string valueStr)
        {
            if (string.IsNullOrEmpty(valueStr)) return false;
            return true;
        }

        #endregion
        #region Auxiliary
        /// <summary>Get the string length, according to the Chinese two, English one calculation。</summary>
        public static int CharCodeLength(this string source)
        {
            int n = 0;
            foreach (var c in source.ToCharArray())
                if ((int)c < 128)
                    n++;
                else
                    n += 2;
            return n;
        }
        #endregion

        #region String Extension

        /// <summary>
        /// Extension SubString()
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetStringByNeedLength(this string source, int length = 0)
        {
            if (source == null)
            {
                return "";
            }
            else
            {
                if (source.Length > length)
                {
                    return source.Substring(0, length) + "...";
                }
                else
                {
                    return source;
                }
            }
        }

        #endregion
    }
}
