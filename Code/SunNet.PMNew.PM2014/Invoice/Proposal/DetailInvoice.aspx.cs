using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Entity.InvoiceModel.Enums;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.Invoice.Proposal
{
    public partial class DetailInvoice : BasePage
    {
        ProposalTrackerApplication proposalApp = new ProposalTrackerApplication();
        InvoicesApplication invoiceApp = new InvoicesApplication();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillProposalInfo();
            }
            else
            {
                string invoices = Request.Form["invoices"];
                string msg = "";
                if (!string.IsNullOrEmpty(invoices))
                {
                    List<InvoiceEntity> listInvoices = JsonHelper.DeserializeObject<List<InvoiceEntity>>(invoices);
                    foreach (var invoice in listInvoices)
                    {
                        if (invoice.Status == InvoiceStatus.Missing_Invoice ||
                            invoice.Status == InvoiceStatus.Missing_Milestone)
                            invoice.Status = InvoiceStatus.Invoice_Created;
                        if (invoice.Status == InvoiceStatus.Waive)
                            invoice.ReceiveOn = DateTime.Now;
                        AddInvoices(invoice, out msg);
                        msg += msg;
                    }
                }
                Response.Redirect("/Invoice/Proposal/DetailInvoice.aspx?proposalId=" + QS("proposalId", 0) +
                                  "&returnUrl=" + Request.QueryString["returnUrl"]);
                //if (string.IsNullOrEmpty(msg))
                //    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>history.go(-2);</script>");
            }
        }

        private void FillProposalInfo()
        {
            int proposalId = QS("proposalId", 0);

            ProposalViewModel model = proposalApp.GetProposalViewModel(proposalId);
            this.lblCompany.Text = model.CompanyName;
            this.lblProject.Text = model.ProjectTitle;
            this.lblProposal.Text = model.ProposalTitle;
            this.lblPO.Text = model.PONo;
        }
        private int AddInvoices(InvoiceEntity entity, out string msg)
        {
            msg = string.Empty;
            int id = 0; 
            if (entity.InvoiceNo == "")
            {
                entity.Color = "yellow";
                entity.ColorFor = "InvoiceNo";
            }
            else
            {
                entity.Color = "";
                entity.ColorFor = "";
            }
            if (entity.ID > 0)
            {
                entity.ModifiedBy = UserInfo.UserID;
                entity.ModifiedOn = DateTime.Now;
                var isSuccess = invoiceApp.UpdateInvoice(entity);
                id = entity.ID;
            }
            else
            {
                entity.CreatedBy = UserInfo.UserID;
                entity.CreatedOn = DateTime.Now;
                entity.ModifiedBy = UserInfo.UserID;
                entity.ModifiedOn = DateTime.Now;
                id = invoiceApp.AddInvoice(entity);
            }
            if (invoiceApp.BrokenRuleMessages.Count > 0 || id <= 0)
            {
                msg = invoiceApp.BrokenRuleMessages[0].Message;
            }
            return id;
        }
    }
    public static class JsonHelper
    {
        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string source)
        {
            return JsonConvert.DeserializeObject<T>(source);
        }
    }
}