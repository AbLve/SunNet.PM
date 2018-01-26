using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ShareModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.SunnetTicket.Knowledge
{
    public partial class View : BasePage
    {
        private ShareApplication _shareApp = new ShareApplication();
        protected ShareEntity Current { get; set; }
        private TicketsApplication _ticketApp = new TicketsApplication();

        protected TicketsEntity Ticket { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            var master = (Pop)this.Master;
            if (master != null) master.Width = 580;
            if (!IsPostBack)
            {
                int id = QS("id", 0);
                if (id <= 0)
                {
                    Response.Redirect(EmptyPopPageUrl);
                }
                Current = _shareApp.Get(id);
                Ticket = _ticketApp.GetTickets(Current.TicketID);
                rptFiles.DataSource = new List<ShareEntity>() { Current };
                rptFiles.DataBind();
            }
        }
        protected void rptFiles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var hidFileTemplate = (HiddenField)e.Item.FindControl("hidFileTemplate");
            var ltlFiles = (Literal)e.Item.FindControl("ltlFiles");
            var files = ((ShareEntity)e.Item.DataItem).Files;
            string filesHtml = "";
            foreach (var file in files.Keys)
            {
                filesHtml += hidFileTemplate.Value.Replace("{FileID}", file.ToString()).Replace("{FileTitle}", files[file]);
                filesHtml += ", ";
            }
            ltlFiles.Text = filesHtml.TrimEnd(", ".ToCharArray());
            hidFileTemplate.Value = "";
        }
    }
}