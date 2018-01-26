using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.Admin.KPI
{
    public partial class KPICategories : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string ReturnUrl
        {
            get
            {
                var p = this.Master as Pop;
                if (p != null)
                    return "";
                string url = QS("returnurl");
                if (!string.IsNullOrEmpty(url))
                    return Server.UrlEncode(url);
                return Server.UrlEncode(Request.RawUrl);
            }
        }
        protected string QS(string key)
        {
            return Request.QueryString[key] + "";
        }
    }
}