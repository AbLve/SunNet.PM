using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;

namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class ViewRelatedTicket : BaseWebsitePage
    {
        TicketsApplication TicketApp = new TicketsApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int tid = QS("tid", 0);

                #region bind data

                if (tid <= 0)
                {
                    this.ShowArgumentErrorMessageToClient();
                    return;
                }
                else
                {
                    TicketsEntity ticketEntity = TicketApp.GetTickets(tid);
                    if (ticketEntity != null)
                    {
                        AddTicket1.TicketsEntityInfo = ticketEntity;
                        AddTicket1.IsEnable = true;
                    }
                    else
                    {
                        this.ShowArgumentErrorMessageToClient();
                        return;
                    }
                }
                #endregion
            }

        }
    }
}
