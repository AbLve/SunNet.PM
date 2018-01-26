using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Framework.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SunNet.PMNew.Core.InvoiceModule.Interface
{
    public interface IinvoiceRepository : IRepository<InvoiceEntity>
    {
        ProposalInvoiceModel GetInvoiceModelById(int id);
        SearchInvoiceResponse SearchTimesheetInvoice(SearchInvoiceRequest request);
        List<InvoiceEntity> GetInvoiceByProposalId(int proposalId);
        SearchInvoiceResponse SearchInvoices(SearchInvoiceRequest request);
        SearchInvoiceResponse SearchProposalInvoice(SearchInvoiceRequest request);
        SearchInvoiceResponse SearchPOlist(SearchInvoiceRequest request);
        bool UpdateForPaymentEmail(InvoiceEntity model);
    }
}
