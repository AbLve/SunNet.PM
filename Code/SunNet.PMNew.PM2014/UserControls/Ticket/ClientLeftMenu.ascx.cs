using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.PM2014.UserControls.Ticket
{
    public partial class ClientLeftMenu : BaseAscx
    {
        public override int ModuleID
        {
            get
            {
                return 4;
            }
        }
        private TicketsApplication ticketApp = new TicketsApplication();
        public int waitingResponse { get; set; }
        public int myOngoing { get; set; }
        public int allOngoing { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetMenuCount();
            }
        }

        private void GetMenuCount()
        {
            waitingResponse = ticketApp.GetWaitingforResponseCount(UserInfo.UserID, 0, 0, ClientTicketState.None, TicketsType.None, "");
            myOngoing = ticketApp.GetMyOngoingTicketsCount(UserInfo, 0, 0, ClientTicketState.None, TicketsType.None, "");
            allOngoing = ticketApp.GetOngoingTicketsCount(UserInfo.UserID, 0, 0, ClientTicketState.None, TicketsType.None, "", true);
        }
    }
}