using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		4/21 10:21:43
 * Description:		枚举扩展类
 * Version History:	Created,4/21 10:21:43
 * 
 * 
 **************************************************************************/
using System.Web.UI.WebControls;

namespace SunNet.PMNew.Framework.Extensions
{
    public static class EnumExpand
    {
        /// <summary>
        /// 将一个或多个枚举常数的名称或数字值的字符串表示转换成等效的枚举对象.
        /// </summary>
        /// <typeparam name="T">要转换的枚举类型</typeparam>
        /// <param name="value">值或者枚举的字符串.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/24 16:14
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        /// <summary>
        /// 转化枚举为字符串，同时替换 "_" 为 " ".
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/24 16:19
        public static string ToText(this Enum source)
        {
            if (source == null)
                return "";
            var field = source.GetType().GetField(source.ToString());
            if (field == null)
                return "";
            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description.Replace("_", " ") : source.ToString().Replace("_", " ");
        }
        public static string ToStrText(this string source)
        {
            if (source == null)
                return "";
            return source.Replace("_", " ");
        }


        public static IEnumerable<ListItem> ToSelectList(this Enum enumValue)
        {
            return from Enum e in Enum.GetValues(enumValue.GetType())
                   select new ListItem
                   {
                       Selected = e.Equals(enumValue),
                       Text = e.ToText(),
                       Value = Convert.ToInt32(e).ToString()//((int)e).ToString()
                   };
        }
    }
}
