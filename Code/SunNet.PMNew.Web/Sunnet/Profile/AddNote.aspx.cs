using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.SealModel;

namespace SunNet.PMNew.Web.Sunnet.Profile
{
    public partial class AddNote : BaseWebsitePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (QS("ID", 0) == 0)
            {
                return;
            }
            else
            {
                hdSealRequestID.Value = QS("ID");
            }
        }
    }
}
