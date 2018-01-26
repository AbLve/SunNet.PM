using SunNet.PMNew.Core.InvoiceModule.Interface;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Core.InvoiceModule
{
    public class InvoiceManager : BaseMgr
    {
        private IinvoiceRepository invoiceRepository;

        public InvoiceManager(IinvoiceRepository invoiceRepository)
        {
            this.invoiceRepository = invoiceRepository;
        }

        public InvoiceEntity GetInvoice(int id)
        {
            InvoiceEntity result = invoiceRepository.Get(id);
            return result;
        }

        public ProposalInvoiceModel GetInvoiceModelById(int id)
        {
            ProposalInvoiceModel result = invoiceRepository.GetInvoiceModelById(id);
            return result;
        }

        public bool DeleteInvoice(int id)
        {
            bool result = invoiceRepository.Delete(id);
            return result;
        }

        public int AddInvoice(InvoiceEntity model)
        {
            int result = invoiceRepository.Insert(model);
            return result;
        }

        public bool UpdateInvoice(InvoiceEntity model)
        {
            bool result = invoiceRepository.Update(model);
            return result;
        }

        public SearchInvoiceResponse SearchInvoices(SearchInvoiceRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchInvoiceResponse response = invoiceRepository.SearchInvoices(request);
            if (response == null)
            {
                this.AddBrokenRuleMessage();
            }
            return response;
        }
        public SearchInvoiceResponse SearchProposalInvoice(SearchInvoiceRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchInvoiceResponse response = invoiceRepository.SearchProposalInvoice(request);
            if (response == null)
            {
                this.AddBrokenRuleMessage();
            }
            return response;
        }
        public SearchInvoiceResponse SearchPOlist(SearchInvoiceRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchInvoiceResponse response = invoiceRepository.SearchPOlist(request);
            if (response == null)
            {
                this.AddBrokenRuleMessage();
            }
            return response;
        }

        public SearchInvoiceResponse SearchTimesheetInvoice(SearchInvoiceRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchInvoiceResponse response = invoiceRepository.SearchTimesheetInvoice(request);
            if (response == null)
            {
                this.AddBrokenRuleMessage();
            }
            return response;
        }

        public List<InvoiceEntity> GetInvoiceByProposalId(int proposalId)
        {
            return invoiceRepository.GetInvoiceByProposalId(proposalId);
        }

        public bool UpdateForPaymentEmail(InvoiceEntity model)
        {
            return invoiceRepository.UpdateForPaymentEmail(model);
        }
    }
}
