using SunNet.PMNew.Core.InvoiceModule.Interface;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Core.InvoiceModule
{
    public class TSInvoiceRelationManager : BaseMgr
    {
        private ITSInvoiceRelationRpository tSInvoiceRelationRepository;
        public TSInvoiceRelationManager(ITSInvoiceRelationRpository tSInvoiceRelationRepository)
        {
            this.tSInvoiceRelationRepository = tSInvoiceRelationRepository;
        }

        public List<TSInvoiceRelationEntity> GetAllTSInvoiceRelation()
         {
             List<TSInvoiceRelationEntity> result = tSInvoiceRelationRepository.GetAllTSInvoiceRelation();
             return result;
         }

        public TSInvoiceRelationEntity GetTSInvoiceRelationById(int id)
         {
             TSInvoiceRelationEntity result = tSInvoiceRelationRepository.Get(id);
             return result;
         }

        public bool DeleteTSInvoiceRelationById(int id)
         {
             bool result = tSInvoiceRelationRepository.Delete(id);
             return result;
         }

        public int AddTSInvoiceRelation(TSInvoiceRelationEntity model)
         {
             int result = tSInvoiceRelationRepository.Insert(model);
             return result;
         }

        public bool UpdateTSInvoiceRelation(TSInvoiceRelationEntity model)
         {
             bool result = tSInvoiceRelationRepository.Update(model);
             return result;
         }
    }
}
