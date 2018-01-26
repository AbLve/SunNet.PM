using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.Invoice
{
    public partial class Invoice : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Invoice Managment";
            ((Normal)this.Master).CurrentModule = Codes.SelectedSunnetMenu.Invoice;
        }
    }
}