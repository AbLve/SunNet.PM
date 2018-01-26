using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.KPIModel;

namespace SunNet.PMNew.Core.KPIModule
{
    public interface IKPICategoryRepository : IRepository<KPICategoriesEntity>
    {
        List<KPICategoriesEntity> GetAllKPICategories();
    }
}
