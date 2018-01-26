using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;

namespace SunNet.PMNew.Web.Sunnet.Profile
{
    public partial class MyCompany : BaseWebsitePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.CompanyEdit1.CompanyID = UserInfo.CompanyID;
        }
    }
}
