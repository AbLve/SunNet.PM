using SunNet.PMNew.App;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.OA.ProposalTracker
{
    public partial class Comment : BasePage
    {
        ProposalTrackerApplication app = new ProposalTrackerApplication();
        private InvoicesApplication _invoiceApp = new InvoicesApplication();
        InvoiceEntity model = new InvoiceEntity();
        protected void Page_Load(object sender, EventArgs e)
        {
            ((Pop)this.Master).Width = 540;
            if (UserInfo.Role != RolesEnum.ADMIN && UserInfo.Role != RolesEnum.PM && UserInfo.Role != RolesEnum.Sales)
            {
                ShowFailMessageToClient("unauthorized access.");
                return;
            }
            if (QS("ID", 0) == 0)
            {
                ShowFailMessageToClient("unauthorized access.");
                Redirect(EmptyPopPageUrl, false, true);
                return;
            }
            int Id = QS("ID", 0);
            model = _invoiceApp.GetInvoice(Id);
            if (!IsPostBack && model.Notes != null)
                txtDescription.Text = model.Notes;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            model.Notes = this.txtDescription.Text;
            if (_invoiceApp.UpdateInvoice(model))
            {
                Redirect(EmptyPopPageUrl, false, true);
            }
            else
            {
                ShowFailMessageToClient(app.BrokenRuleMessages);
            }
        }
    }
}