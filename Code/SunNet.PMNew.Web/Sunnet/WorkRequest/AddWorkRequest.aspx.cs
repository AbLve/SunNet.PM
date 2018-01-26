using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;

namespace SunNet.PMNew.Web.Sunnet.WorkRequest
{
    public partial class AddWorkRequest : BaseWebsitePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddWorkRequest1.IsAdd = true;
        }
    }
}
