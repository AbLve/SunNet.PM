using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web
{

    public partial class _Default : BaseWebsitePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string url = "/Sunnet/tickets/dashboard.aspx";
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                url = "/sunnet/Clients/dashboard.aspx";
            }
            Response.Redirect(url);
        }
    }
}
