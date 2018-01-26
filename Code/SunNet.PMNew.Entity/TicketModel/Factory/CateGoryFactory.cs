using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Entity.TicketModel
{
    public class CateGoryFactory
    {
        public static CateGoryEntity CreateCateGoryEntity(int createdByUserID, ISystemDateTime timeProvider)
        {
            CateGoryEntity model = new CateGoryEntity();

            model.CreatedBy = createdByUserID;
            model.CreatedOn = timeProvider.Now;
            model.ID = 0;
            model.ModifiedBy = createdByUserID;
            model.ModifiedOn = timeProvider.Now;

            model.Title = string.Empty;
            model.IsProtected = false;
            model.IsDelete = false;

            return model;
        }
        public static CateGoryTicketEntity CreateCateGoryTicketEntity(int createdByUserID, ISystemDateTime timeProvider)
        {
            CateGoryTicketEntity model = new CateGoryTicketEntity();

            model.CreatedBy = createdByUserID;
            model.CreatedOn = timeProvider.Now;
            model.ID = 0;
            model.ModifiedBy = createdByUserID;
            model.ModifiedOn = timeProvider.Now;

            model.CategoryID = 0;
            model.TicketID = 0;

            return model;
        }
    }
}
