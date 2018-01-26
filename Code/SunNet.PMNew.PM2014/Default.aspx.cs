using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014
{
    public partial class Default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = "/Dashboard.aspx";
            if (UserInfo.Role != Entity.UserModel.RolesEnum.CLIENT)
                url = "/SunnetTicket/Dashboard.aspx";
            string returnurl = Server.UrlDecode(Request.QueryString["returnurl"]);
            string page = Server.UrlDecode(returnurl);
            if (page != null && page.IndexOf(".aspx") > 0)
            {

                page = page.Substring(0, page.IndexOf(".aspx") + 5);
                if (CheckRoleCanAccessPage(page))
                    url = returnurl;
            }
            if (url.IndexOf(",") > 0)
                url = url.Substring(0, url.IndexOf(","));
            Response.Redirect(url);
        }
    }
}