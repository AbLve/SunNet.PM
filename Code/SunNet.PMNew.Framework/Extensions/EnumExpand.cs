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
 * Description:		ö����չ��
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
        /// ��һ������ö�ٳ��������ƻ�����ֵ���ַ�����ʾת���ɵ�Ч��ö�ٶ���.
        /// </summary>
        /// <typeparam name="T">Ҫת����ö������</typeparam>
        /// <param name="value">ֵ����ö�ٵ��ַ���.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/24 16:14
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        /// <summary>
        /// ת��ö��Ϊ�ַ�����ͬʱ�滻 "_" Ϊ " ".
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
