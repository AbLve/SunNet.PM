using SunNet.PMNew.Entity.InvoiceModel.Proposal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.InvoiceModel
{
    public class SearchInvoiceResponse
    {
        public List<ProposalInvoiceModel> ResultList { get; set; }

        public List<TimesheetInvoiceModel> TimesheetList { get; set; }

        public List<ProposalToDoModel> ProposalList { get; set; }

        public List<POListModel> POList { get; set; }

        public int ResultCount { get; set; }

        public int TimesheetCount { get; set; }

        public int POListCount { get; set; }
    }
}
