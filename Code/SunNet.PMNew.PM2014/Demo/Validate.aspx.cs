using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Demo
{
    public partial class Validate : GlobalPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Write(UtilFactory.Helpers.HtmlHelper.ReplaceUrl("abcdewww.baidu.comsssssssssss"));
            Response.Write("<br/>");
            Response.Write(UtilFactory.Helpers.HtmlHelper.ReplaceUrl("abcdehttp://www.google.comsssssssssss"));
            Response.Write("<br/>");
            Response.Write(UtilFactory.Helpers.HtmlHelper.ReplaceUrl("http://pm2014.jackz.cc/SunnetTicket/Detail.aspx?tid=1013&returnurl=%2fSunnetTicket%2fIndex.aspx issues"));
            Response.Write("<br/>");
            Response.Write(UtilFactory.Helpers.HtmlHelper.ReplaceUrl("see http://pm2014.jackz.cc/SunnetTicket/Detail.aspx?tid=1013&returnurl=%2fSunnetTicket%2fIndex.aspx#lastFeedback for details"));

        }
    }
}