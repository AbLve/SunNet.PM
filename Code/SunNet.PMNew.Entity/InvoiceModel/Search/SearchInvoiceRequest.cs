using SunNet.PMNew.Entity.InvoiceModel.Enums;
using SunNet.PMNew.Entity.InvoiceModel.Proposal;
using SunNet.PMNew.Entity.ProjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.InvoiceModel
{
    public enum InvoiceSearchType
    {
        All = 1,
        ProposalOnly = 2,
        TsOnly = 3,
        [Description("Awiting Payment")]
        AwitingPayment = 4,
        PassDue = 5,
        [Description("Payment Received")]
        Payment_Received = 6,
        [Description("Payment Confirmed")]
        Payment_Confirmed = 8
    }
    public class SearchInvoiceRequest
    {
        public SearchInvoiceRequest()
        {

        }

        public InvoiceSearchType Searchtype { get; set; }

        public string Keywords { get; set; }

        public int CompanyId { get; set; }

        public int ProjectId { get; set; }

        public string ProjectIds { get; set; }

        public string timeTsheetIDs { get; set; }

        public InvoiceStatus InvoiceStatus { get; set; }

        public string OrderExpression { get; set; }

        public string OrderDirection { get; set; }

        public int order { get; set; }
        public int dir { get; set; }

        public string ApproveOn { get; set; }
    }
}
