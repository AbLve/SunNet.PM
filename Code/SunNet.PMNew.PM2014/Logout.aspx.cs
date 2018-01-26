using SunNet.PMNew.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.PM2014.Codes;
using System.IO;

namespace SunNet.PMNew.PM2014
{
    public partial class Logout : GlobalPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
            UtilFactory.Helpers.CookieHelper.Remove(encrypt.Encrypt("LoginUserID"));
            UtilFactory.Helpers.CookieHelper.RemoveAll();
            string url = "/Login.aspx";
            string returnUrl = Server.UrlDecode(Request.QueryString["returnurl"]);
            if (!string.IsNullOrEmpty(returnUrl) && returnUrl.IndexOf(".aspx") > 0)
            {
                bool exist = File.Exists(Server.MapPath(returnUrl.Substring(0, returnUrl.IndexOf(".aspx") + 5)));
                if (exist)
                    url += "?returnurl=" + Server.UrlEncode(returnUrl);
            }
            Response.Redirect(url);
        }
    }
}