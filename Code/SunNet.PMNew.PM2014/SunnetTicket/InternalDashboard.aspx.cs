using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Event;
using System.Data;
using SunNet.PMNew.Entity.TicketModel.UserTicket;

namespace SunNet.PMNew.PM2014.SunnetTicket
{
    public partial class InternalDashboard : TicketPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }
    }
}