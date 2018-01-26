using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Entity.KPIModel
{
    public static class KPIFactory
    {

        public static KPICategoriesEntity CreateKPICategoriesEntity(int createdByUserID, ISystemDateTime timeProvider)
        {
            KPICategoriesEntity model = new KPICategoriesEntity();

                    

            model.ID = 0;
            model.CategoryName = string.Empty;
            model.Status = 0;
            model.CreatedBy = createdByUserID;
            model.CreatedOn = timeProvider.Now;

            return model;
        }
    }
}
