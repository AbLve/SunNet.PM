using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class ConvertReason : BaseWebsitePage
    {
        TicketsApplication ticketApp = new TicketsApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (QS("uType") == "cRequest")
                {
                    int tid = QS("tid", 0);
                    this.txtOriginalDesc.Value = ticketApp.GetTicketDescr(tid);
                    this.trCk.Visible = true;
                }
            }
        }
    }
}
