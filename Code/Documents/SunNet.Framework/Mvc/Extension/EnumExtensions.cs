using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI.WebControls;

#region Version Info
/* ========================================================================
*
* Author: lynnm Date: 2012/12/5 16:51:23
* File Name: EnumExtensions
* Version: 4.0.30319.296
*
* ========================================================================
*/
#endregion
namespace SF.Framework.Mvc.Extension
{
    public static class EnumExtensions
    {

        public static IEnumerable<ListItem> ToSelectList(this Enum enumValue)
        {
            return from Enum e in Enum.GetValues(enumValue.GetType())
                   select new ListItem
                   {
                       Selected = e.Equals(enumValue),
                       Text = e.ToDescription(),
                       Value = e.ToString()
                   };
        }

        public static IEnumerable<ListItem> ToList(this Type enumType)
        {
            return from Enum e in Enum.GetValues(enumType)
                   select new ListItem
                   {
                      Text = e.ToDescription(),
                      Value = e.GetValue().ToString()
                   };
        }

        /// <summary>
        /// Enum to SelectList
        /// </summary>
        /// <param name="enumValue">Enum</param>
        /// <param name="defaultText">default item's text</param>
        /// <returns>value | text</returns>
        public static IEnumerable<ListItem> ToSelectList(this Enum enumValue, string defaultText)
        {
            return enumValue.ToSelectList(true, true, defaultText);
        }
        /// <summary>
        /// Enum to SelectList
        /// </summary>
        /// <param name="enumValue">Enum</param>
        /// <param name="insertFirstDefault">add a default item at index 0</param>
        /// <param name="defaultText">default item's text</param>
        /// <returns>value | text</returns>
        public static IEnumerable<ListItem> ToSelectList(this Enum enumValue, bool insertFirstDefault, string defaultText = "ALL")
        {
            return enumValue.ToSelectList(true, insertFirstDefault, defaultText);
        }
        /// <summary>
        /// Enum to SelectList
        /// </summary>
        /// <param name="enumValue">Enum</param>
        /// <param name="ValueInInt">value format in int format or not</param>
        /// <param name="insertFirstDefault">add a default item at index 0</param>
        /// <param name="defaultText">default item's text</param>
        /// <returns>value | text</returns>
        public static IEnumerable<ListItem> ToSelectList(this Enum enumValue, bool ValueInInt, bool insertFirstDefault, string defaultText = "ALL")
        {
            var enums = from Enum e in Enum.GetValues(enumValue.GetType())
                        select new ListItem
                        {
                            Selected = e.Equals(enumValue),
                            Text = e.ToDescription(),
                            Value = e.ToString()
                        };
            if (ValueInInt)
                enums = from Enum e in Enum.GetValues(enumValue.GetType())
                        select new ListItem
                        {
                            Selected = e.Equals(enumValue),
                            Text = e.ToDescription(),
                            Value = e.GetValue().ToString()
                        };
            List<ListItem> list = enums.ToList<ListItem>();
            if (insertFirstDefault)
                list.Insert(0, new ListItem() { Text = defaultText, Value = "-1" });
            return list;
        }
        public static string ToDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null)
                return "";
            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
        public static int GetValue(this Enum value)
        {
            return Convert.ToInt32(value);
        }
        //public static SelectList ToSelectList<T, TU>(T enumObj)
        //    where T : struct
        //    where TU : struct
        //{
        //    if (!typeof(T).IsEnum) throw new ArgumentException("Enum is required.", "enumObj");

        //    var values = from T e in Enum.GetValues(typeof(T))
        //                 select new
        //                 {
        //                     Value = (TU)Convert.ChangeType(e, typeof(TU)),
        //                     Text = e.ToString()
        //                 };

        //    return new SelectList(values, "Value", "Text", enumObj);
        //}
    }
}
