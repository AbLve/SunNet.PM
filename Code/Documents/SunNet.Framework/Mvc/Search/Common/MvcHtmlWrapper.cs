using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SF.Framework.Mvc.Search.Common
{
    public class MvcHtmlWrapper : IHtmlString
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static MvcHtmlWrapper Create(IHtmlString str)
        {
            Contract.Requires(str != null);
            if (str is MvcHtmlWrapper)
                return str as MvcHtmlWrapper;
            if (str is MvcHtmlString)
                return new MvcHtmlWrapper(str);
            Contract.Assert(false);
            return null;
        }

        IHtmlString HtmlStringInterface { get; set; }

        private string _htmlString;

        /// <summary>
        /// 
        /// </summary>
        public string HtmlString
        {
            get { return _htmlString ?? (_htmlString = HtmlStringInterface.ToHtmlString()); }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Tuple<string, string>> ReplaceDict { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        MvcHtmlWrapper(IHtmlString str)
        {
            Contract.Requires(str != null);
            HtmlStringInterface = str;
            ReplaceDict = new List<Tuple<string, string>>();
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToHtmlString()
        {
            return ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder(HtmlString);
            foreach (var item in ReplaceDict)
            {
                if (!string.IsNullOrEmpty(item.Item1))
                {
                    sb.Replace(item.Item1, item.Item2);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        internal void Add(string item1, string item2)
        {
            ReplaceDict.Add(Tuple.Create(item1, item2));
        }
    }
}