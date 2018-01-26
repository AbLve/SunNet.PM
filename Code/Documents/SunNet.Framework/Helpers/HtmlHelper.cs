using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.Helpers
{
    public static class HtmlHelper
    {
        public static string ToHtmlFormat(string str)
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
    }
}
