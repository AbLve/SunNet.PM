using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core;
namespace SunNet.PMNew.Web
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
            UtilFactory.Helpers.CookieHelper.Remove(encrypt.Encrypt("LoginUserID"));
            Response.Redirect("/Login.aspx");
        }
    }
}
