using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Admin
{
    public partial class Seals : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SealsApplication app = new SealsApplication();
                rptSealList.DataSource = app.GetList();
                rptSealList.DataBind();
            }
        }
    }
}