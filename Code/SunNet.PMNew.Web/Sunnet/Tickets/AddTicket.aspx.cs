using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class AddTicket : BaseWebsitePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IniControl();
            }
        }

        private void IniControl()
        {
            this.AddTicket1.IsSunnet = "true";
        }
    }
}
