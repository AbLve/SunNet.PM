using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.UserControls.Ticket.Sunnet
{
    public partial class ChangeHistory : BaseAscx
    {
        public int TicketID { get; set; }
        TicketsApplication ticketApp = new TicketsApplication();
        List<TicketHistorysEntity> list = new List<TicketHistorysEntity>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TicketID > 0)
            {
                GetListHistoryByTid(TicketID);
            }
        }


        private void GetListHistoryByTid(int tid)
        {
            list = ticketApp.GetHistoryListByTicketID(tid);

            if (null != list && list.Count > 0)
            {
                this.rptTicketsHistoryList.DataSource = list;
            }
            else
            {
                this.trNoTickets.Visible = true;
                this.rptTicketsHistoryList.DataSource = new List<TicketsEntity>();
            }

            this.rptTicketsHistoryList.DataBind();

        }
    }
}