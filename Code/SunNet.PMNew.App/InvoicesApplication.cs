using StructureMap;
using SunNet.PMNew.Core.InvoiceModule;
using SunNet.PMNew.Core.InvoiceModule.Interface;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.App
{
    public class InvoicesApplication : BaseApp
    {
        private InvoiceManager mgr;
        private TSInvoiceRelationManager tsmgr;


        public InvoicesApplication()
        {
            var invoiceRepository = ObjectFactory.GetInstance<IinvoiceRepository>();
            mgr = new InvoiceManager(invoiceRepository);
            tsmgr = new TSInvoiceRelationManager(ObjectFactory.GetInstance<ITSInvoiceRelationRpository>());
        }

        #region  Invoiceapplication
        public ProposalInvoiceModel GetInvoiceModelById(int id)
        {
            ProposalInvoiceModel result = mgr.GetInvoiceModelById(id);
            return result;
        }
        public InvoiceEntity GetInvoice(int id)
        {
            InvoiceEntity result = mgr.GetInvoice(id);
            return result;
        }
        public int AddInvoice(InvoiceEntity model)
        {
            int result = mgr.AddInvoice(model);
            return result;
        }
        public bool DeleteInvoice(int id)
        {
            bool result = mgr.DeleteInvoice(id);
            return result;
        }
        public bool UpdateInvoice(InvoiceEntity model)
        {
            bool result = mgr.UpdateInvoice(model);
            return result;
        }
        public List<InvoiceEntity> GetInvoiceByProposalId(int proposalId)
        {
            return mgr.GetInvoiceByProposalId(proposalId);
        }
        public SearchInvoiceResponse SearchInvoices(SearchInvoiceRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchInvoiceResponse response = mgr.SearchInvoices(request);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return response;
        }
        public SearchInvoiceResponse SearchProposalInvoice(SearchInvoiceRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchInvoiceResponse response = mgr.SearchProposalInvoice(request);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return response;
        }
        public SearchInvoiceResponse SearchPOlist(SearchInvoiceRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchInvoiceResponse response = mgr.SearchPOlist(request);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return response;
        }
        public SearchInvoiceResponse SearchTimesheetInvoice(SearchInvoiceRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchInvoiceResponse response = mgr.SearchTimesheetInvoice(request);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return response;
        }

        public bool UpdateForPaymentEmail(InvoiceEntity model)
        {
            return mgr.UpdateForPaymentEmail(model);
        }
        #endregion

        #region  TSInvoiceRelationapplication
        public List<TSInvoiceRelationEntity> GetAllTSInvoiceRelation()
        {
            List<TSInvoiceRelationEntity> result = tsmgr.GetAllTSInvoiceRelation();
            return result;
        }
        public TSInvoiceRelationEntity GetTSInvoiceRelationById(int id)
        {
            TSInvoiceRelationEntity result = tsmgr.GetTSInvoiceRelationById(id);
            return result;
        }
        public int AddTSInvoiceRelation(TSInvoiceRelationEntity model)
        {
            int result = tsmgr.AddTSInvoiceRelation(model);
            return result;
        }
        public bool DeleteTSInvoiceRelationById(int id)
        {
            bool result = tsmgr.DeleteTSInvoiceRelationById(id);
            return result;
        }
        public bool UpdateTSInvoiceRelation(TSInvoiceRelationEntity model)
        {
            bool result = tsmgr.UpdateTSInvoiceRelation(model);
            return result;
        }
        #endregion
    }
}
