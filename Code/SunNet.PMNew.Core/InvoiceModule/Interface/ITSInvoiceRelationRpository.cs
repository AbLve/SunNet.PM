using SunNet.PMNew.Framework.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.InvoiceModel;

namespace SunNet.PMNew.Core.InvoiceModule.Interface
{
    public interface ITSInvoiceRelationRpository : IRepository<TSInvoiceRelationEntity>
    {
        List<TSInvoiceRelationEntity> GetAllTSInvoiceRelation();
    }
}
