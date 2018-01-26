using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace SF.Framework.WebForm.Extension
{
    public static class Expand
    {
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
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
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
            else
            {
                item = ddl.Items.FindByText(value);
                if (item != null)
                {
                    ddl.ClearSelection();
                    item.Selected = true;
                }
            }
        }
        #endregion

        #region Options
        /// <summary>
        /// Convert listitems to options html
        /// </summary>
        /// <param name="list"></param>
        /// <param name="defaultText">null means do not add default item</param>
        /// <param name="defaultValue">null means do not add default item</param>
        /// <param name="optGroupLabel">Optgroup label,null means do not add optgroup</param>
        /// <returns></returns>
        public static string ToOptions(this List<ListItem> list, string defaultText, string defaultValue, string optGroupLabel = "")
        {
            StringBuilder options = new StringBuilder();
            if (!string.IsNullOrEmpty(defaultText) && !string.IsNullOrEmpty(defaultValue))
                options.AppendFormat("<option value='{0}' {2}>{1}</option>", defaultValue, defaultText,
                    list.Count<ListItem>(l => l.Selected) > 0 ? "" : "selected='selected'");
            foreach (ListItem item in list)
            {
                options.AppendFormat("<option value='{0}' {2}>{1}</option>", item.Value, item.Text,
                    item.Selected ? "selected='selected'" : "");
            }
            if (!string.IsNullOrEmpty(optGroupLabel))
            {
                options.Insert(0, string.Format("<optgroup label='{0}'> ", optGroupLabel));
                options.Append("</optgroup>");
            }
            return options.ToString();
        }
        #endregion

        #region TextBox
        public static string Text(this TextBox textBox)
        {
            return textBox.Text.Trim();
        }
        #endregion
    }
}
