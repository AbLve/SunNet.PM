using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.PM2014.OA.SSO
{
    public partial class ToCRM :  BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string email = UserInfo.UserName;
            long nowTick = DateTime.Now.Ticks;
            string seed = email + nowTick + "SunNet&543#53";
            string sign = UtilFactory.GetEncryptProvider(EncryptType.MD5).Encrypt(seed);
            Response.Redirect("http://crm.sunnet.us/sale/SSO.aspx?email="+email+"&sign="+ sign + "&Timestamp="+ nowTick);
        }
    }
}