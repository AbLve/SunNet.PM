using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.OA.ProposalTracker
{
    public partial class AddPayment : BasePage
    {
        ProposalTrackerApplication ptApp = new ProposalTrackerApplication();
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
                return;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int proposalTrackerId = QS("ID", 0);
            ProjectPaymentEntity entity = new ProjectPaymentEntity();
            entity.ProposalTrackerID = proposalTrackerId;
            entity.MilestoneNo = txtMilestoneNo.Text.Trim();
            entity.Approved = ddlApproved.SelectedValue == "1";
            entity.PaymentNo = txtPaymentNo.Text.Trim();
            entity.InvoiceNo = txtInvoiceNo.Text.Trim();
            entity.SendDate = DateTime.Parse(txtInvoiceSentOn.Text.Trim());
            entity.ReceiveDate = DateTime.Parse(txtReceiveOn.Text.Trim());
            entity.CreatedBy = UserInfo.UserID;
            entity.CreatedOn = DateTime.Now;
            entity.ModifiedBy = UserInfo.UserID;
            //if (ptApp.AddPayment(entity) > 0)
            //{
            //    Redirect(EmptyPopPageUrl, false, true);
            //}
            //else
            //{
            //    ShowFailMessageToClient(ptApp.BrokenRuleMessages);
            //}
        }
    }
}