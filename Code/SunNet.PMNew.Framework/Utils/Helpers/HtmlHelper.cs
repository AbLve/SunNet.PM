using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace SunNet.PMNew.Framework.Utils.Helpers
{
    public class HtmlHelper
    {
        public string ToHtmlFormat(string str)
        {
            string input = str;
            if (input == null || input == "")
                return input;
            input = input.Replace("&", "&amp;");
            input = input.Replace("<", "&lt;");
            input = input.Replace(">", "&gt;");
            input = input.Replace(" ", "&nbsp;");
            input = input.Replace("\\", "&quot;");
            input = input.Replace("\r\n", "<br>\r\n");
            return input;
        }

        private string ProcessMatchedUrl(Match m)
        {
            string url = m.ToString();
            return string.Format(" <a href='{1}{0}' target='_blank'>{0}</a> ", url, url.IndexOf("://") > 0 ? "" : "http://");
        }

        public string ReplaceUrl(string source)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            Regex regexhtml = new Regex(@"(?is)(?<=>)[^<]+(?=<)");
            if (regexhtml.IsMatch(source))
                return source;
            var regexp = new Regex("((http[s]{0,1}|ftp)://)?[a-zA-Z0-9\\.\\-]+\\.(com|cn|us|org|net)(:\\d+)?(/[a-zA-Z0-9\\.\\-~!@#$%^&*+?:_/=<>\\(\\)]*)?");
            return regexp.Replace(source, new MatchEvaluator(ProcessMatchedUrl));
        }       
    }
}
