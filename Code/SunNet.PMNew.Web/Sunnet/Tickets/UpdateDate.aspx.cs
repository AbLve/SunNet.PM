using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class UpdateDate : BaseWebsitePage
    {
        #region
        TicketsApplication ticketApp = new TicketsApplication();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int tid = QS("tid", 0);
                if (tid <= 0)
                {
                    this.ShowArgumentErrorMessageToClient();
                    return;
                }
                TicketsEntity model = ticketApp.GetTickets(tid);
                if (null != model)
                {
                    if (model.StartDate.DayOfYear > UtilFactory.Helpers.CommonHelper.GetDefaultMinDate().DayOfYear)
                    {
                        this.txtStartDate.Text = model.StartDate.ToString("MM/dd/yyyy");
                    }
                    if (model.DeliveryDate.DayOfYear > UtilFactory.Helpers.CommonHelper.GetDefaultMinDate().DayOfYear)
                    {
                        this.txtEndDate.Text = model.DeliveryDate.ToString("MM/dd/yyyy");
                    }
                }
            }
        }
    }
}
