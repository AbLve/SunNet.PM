using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;

namespace SunNet.PMNew.Web.Sunnet.Admin
{
    public partial class SealsList : BaseWebsitePage
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            SealsApplication app = new SealsApplication();
            rptSeals.DataSource = app.GetList();
            rptSeals.DataBind();
        }
    }
}
