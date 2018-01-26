using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web;

namespace SunNet.PMNew.Framework.Utils.Helpers
{
    public static class Expand
    {
        /// <summary>
        /// 根据长度截取字符串
        /// </summary>
        /// <param name="s">The source string.</param>
        /// <param name="length">The length.</param>
        /// <param name="padright">发生截取时，替换的字符串.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/24 12:27
        public static string SubString(this string s, int length, string padright = "...")
        {
            if (string.IsNullOrEmpty(s))
                return "";
            else if (s.Length < length)
                return s;
            else
                return s.Substring(0, length) + "...";
        }

        public static string SubString2(this string s, int startIndex, int endIndex)
        {
            if (startIndex < 0 || endIndex < 0)
            {
                throw new System.Exception("Neither startIndex nor endIndex can less than 0");
            }
            if (startIndex > endIndex)
            {
                throw new System.Exception("startIndex must less than endIndex");
            }
            return s.Substring(startIndex, endIndex - startIndex);
        }

        public static string NoHTML(this string Htmlstring)
        {
            if (Htmlstring == null)
            {
                return "";
            }
            Htmlstring = Regex.Replace(Htmlstring, "<script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(iexcl|#161);", "\x00a1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(cent|#162);", "\x00a2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(pound|#163);", "\x00a3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(copy|#169);", "\x00a9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            return Htmlstring;
        }

        public static string FilterSqlString(this string sql)
        {
            if (sql != null)
            {
                sql = sql.Replace("[", "[[]");
            }
            return sql;
        }

        //public static void BindDropdown<T>(this List<T> list, DropDownList ddl, string t, string v)
        //{
        //    ddl.DataSource = list;
        //    ddl.DataTextField = t;
        //    ddl.DataValueField = v;
        //    ddl.DataBind();
        //}

        #region BindControls
        /// <summary>
        /// Bind list to Dropdownlist(no default item)
        /// </summary>
        /// <typeparam name="T">Entity type to bind</typeparam>
        /// <param name="list">Entities to bind</param>
        /// <param name="ddl">DropDownList control</param>
        /// <param name="disMember">Text Field</param>
        /// <param name="valMember">Value Field</param>
        public static void BindDropdown<T>(this List<T> list, DropDownList ddl, string disMember, string valMember)
        {
            ddl.Items.Clear();

            ddl.DataSource = list;
            ddl.DataTextField = disMember;
            ddl.DataValueField = valMember;

            ddl.DataBind();
        }

        /// <summary>
        /// Bind list to Dropdownlist(with default item)
        /// </summary>
        /// <typeparam name="T">Entity type to bind</typeparam>
        /// <param name="list">Entities to bind</param>
        /// <param name="ddl">DropDownList control</param>
        /// <param name="disMember">Text Field</param>
        /// <param name="valMember">Value Field</param>
        /// <param name="defaultText">Default Item Text,Default Item Value is -1</param>
        public static void BindDropdown<T>(this List<T> list, DropDownList ddl, string disMember, string valMember, string defaultText)
        {
            list.BindDropdown<T>(ddl, disMember, valMember, defaultText, "-1");
        }
        /// <summary>
        /// Bind list to Dropdownlist(with default item)
        /// </summary>
        /// <typeparam name="T">Entity type to bind</typeparam>
        /// <param name="list">Entities to bind</param>
        /// <param name="ddl">DropDownList control</param>
        /// <param name="disMember">Text Field</param>
        /// <param name="valMember">Value Field</param>
        /// <param name="defaultText">Default Item Text</param>
        /// <param name="defaultValue">Default Item Value</param>
        public static void BindDropdown<T>(this List<T> list, DropDownList ddl, string disMember, string valMember, string defaultText, string defaultValue, bool insertDefaultItem = false)
        {
            ddl.Items.Clear();

            ddl.DataSource = list;
            ddl.DataTextField = disMember;
            ddl.DataValueField = valMember;

            ddl.DataBind();
            if (!string.IsNullOrEmpty(defaultText) &&
                (ddl.Items.Count != 1 || insertDefaultItem))
            {
                ddl.Items.Insert(0, new ListItem(defaultText, defaultValue));
            }
        }

        /// <summary>
        /// Bind list to Dropdownlist(with default item)
        /// </summary>
        /// <typeparam name="T">Entity type to bind</typeparam>
        /// <param name="list">Entities to bind</param>
        /// <param name="ddl">DropDownList control</param>
        /// <param name="disMember">Text Field</param>
        /// <param name="valMember">Value Field</param>
        /// <param name="defaultText">Default Item Text</param>
        /// <param name="defaultValue">Default Item Value</param>
        /// <param name="selectedValue">value to select by value </param>
        /// <param name="insertDefaultItem">true to insert default item</param>
        public static void BindDropdown<T>(this List<T> list, DropDownList ddl, string disMember, string valMember, string defaultText, string defaultValue, string selectedValue, bool insertDefaultItem = false)
        {
            list.BindDropdown<T>(ddl, disMember, valMember, defaultText, defaultValue, insertDefaultItem);
            ddl.SelectItem(selectedValue);
        }

        /// <summary>
        /// insert empty item to a DropDownList
        /// </summary>
        /// <param name="ddl">DropDownList to add </param>
        /// <param name="index">Empty item index,-1 to last item</param>
        /// <param name="defaultText">Empty item text</param>
        /// <param name="defaultValue">Empty item value</param>
        /// <param name="InsertAnyWay">false means insert only the existsted items count bigger than 1,true means will insert any condition</param>
        public static void AddEmptyItem(this  DropDownList ddl, int index, string defaultText, string defaultValue, bool InsertAnyWay)
        {
            if (ddl.Items.Count != 1 || InsertAnyWay)
            {
                if (index == -1)
                    ddl.Items.Insert(ddl.Items.Count, new ListItem(defaultText, defaultValue));
                else
                    ddl.Items.Insert(index, new ListItem(defaultText, defaultValue));
            }
        }
        /// <summary>
        /// insert empty item to a DropDownList(when existsted items count bigger than 1,will insert new ListItem("Please select","0") at 0)
        /// </summary> 
        public static void AddEmptyItem(this  DropDownList ddl)
        {
            AddEmptyItem(ddl, 0, "Please select...", "-1", false);
        }
        /// <summary>
        /// Select a item by value
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="value"></param>
        public static void SelectItem(this DropDownList ddl, string value)
        {
            ListItem item = ddl.Items.FindByValue(value);
            if (item != null)
            {
                ddl.ClearSelection();
                item.Selected = true;
            }
        }
        #endregion

        public static string GetName<T>(this Enum value)
        {
            return Enum.GetName(typeof(T), value).Replace('_', ' ');
        }

        /// <summary>
        /// Determines whether dataReader [contains] [the specified column].
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/26 23:09
        public static bool Contains(this IDataReader reader, string columnName)
        {
            return reader.GetSchemaTable()
               .Rows
               .OfType<DataRow>()
               .Any(row => (string)row["ColumnName"] == columnName);
        }

        /// <summary>
        /// 格式化时间：小于UtilFactory.Helpers.CommonHelper.GetDefaultMinDate()的显示空，否则按格式显示.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  6/5 01:15
        public static string ToText(this DateTime source, string format = "MM/dd/yyyy")
        {
            if (source <= UtilFactory.Helpers.CommonHelper.GetDefaultMinDate())
            {
                return "";
            }
            return source.ToString(format);
        }
    }
}
