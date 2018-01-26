using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;

namespace SunNet.PMNew.Web.Sunnet.Admin
{
    public partial class AddClient : BaseWebsitePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserEdit1.IsAdd = true;
            UserEdit1.IsSunnet = false;
        }
    }
}
