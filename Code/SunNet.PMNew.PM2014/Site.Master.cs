using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;
namespace SunNet.PMNew.PM2014
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = string.Format("{0}{1}",
                string.IsNullOrEmpty(Page.Title) ? "Project Management - " : Page.Title + " - Project Management - ",
                "SunNet Solutions");
        }

        private bool ISIE
        {
            get
            {
                return Request.Browser.Browser.Equals("ie", StringComparison.CurrentCultureIgnoreCase)
                    || Request.Browser.Browser.Equals("InternetExplorer", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        /// <summary>
        /// IE - WebBrowser 模拟器以IE8模式运行.
        /// </summary>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/14 17:28
        public string IEEmulator
        {
            get
            {
                if (!ISIE)
                    return "";
                var version = 8.0f;
                if (float.TryParse(Request.Browser.Version, out version) && version < 8)
                {
                    version = 8;
                }
                return string.Format("<meta http-equiv='X-UA-Compatible' content='IE=EmulateIE{0}' />", (int)version);
            }
        }
    }
}