using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class HistoryList : BaseAscx
    {
        TicketsApplication ticketApp = new TicketsApplication();
        UserApplication userAPP = new UserApplication();
        List<TicketHistorysEntity> list = new List<TicketHistorysEntity>();

        protected void Page_Load(object sender, EventArgs e)
        {
            int tid = QS("tid", 0);
            if (tid > 0)
            {
                GetListHistoryByTid(tid);
            }
        }

        public string ShowUserName(string uid)
        {
            int id = Convert.ToInt32(uid);
            if (id <= 0) return "";
            string name = userAPP.GetLastNameFirstName(id);
            return name.Length > 0 ? name : "";

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