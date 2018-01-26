using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.Web;
using SF.Framework.Mvc.Search.Common;
using SF.Framework.Mvc.Search.Model;

namespace SF.Framework.Mvc.Search.Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class MvcHtmlStringExtension
    {
        #region ForSearch

        /// <summary>
        /// For the current form elements add search conditions
        /// </summary>
        /// <param name="str"></param>
        /// <param name="method"></param>
        /// <param name="prefix"></param>
        /// <param name="hasId"></param>
        /// <param name="orGroup"></param>
        /// <returns></returns>
        public static MvcHtmlWrapper ForSearch(this IHtmlString str, QueryMethod? method, string prefix = "", bool hasId = false, string orGroup = "")
        {
            var wrapper = MvcHtmlWrapper.Create(str);
            Contract.Assert(null != wrapper);
            if (!method.HasValue) return wrapper;
            var html = wrapper.HtmlString;
            #region If it is CheckBox, then remove hidden

            if (html.Contains("type=\"checkbox\""))
            {
                var checkMatch = Regex.Match(html, "<input name=\"[^\"]+\" type=\"hidden\" [^>]+ />");
                if (checkMatch.Success)
                {
                    wrapper.Add(checkMatch.Groups[0].Value, string.Empty);
                }
            }

            #endregion

            #region Replace Name
            var match = Regex.Match(html, "name=\"(?<name>[^\"]+)\"");
            var strInsert = "";
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                strInsert += string.Format("({0})", prefix);
            }
            if (!string.IsNullOrWhiteSpace(orGroup))
            {
                strInsert += string.Format("{{{0}}}", orGroup);
            }
            if (match.Success)
            {
                wrapper.Add(match.Groups[0].Value,
                            string.Format("name=\"[{1}]{2}{0}\"", match.Groups[1].Value, method, strInsert));
            }

            #endregion

            return wrapper;
        }

        #endregion
    }
}
