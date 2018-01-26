using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Ticket.Sunnet
{
    public partial class Sunnet : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((Normal)this.Master).CurrentModule = Codes.SelectedSunnetMenu.Ticket;
            if (!IsPostBack)
            {
                //CateGory
                CateGoryApplication cgApp = new CateGoryApplication();
                List<CateGoryEntity> listCC = cgApp.GetCateGroyListByUserID(((BasePage)this.Page).UserInfo.ID);
                rptCategory.DataSource = listCC;
                rptCategory.DataBind();
            }
        }
    }
}