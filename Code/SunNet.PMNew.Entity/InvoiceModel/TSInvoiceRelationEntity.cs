using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.InvoiceModel
{
    public class TSInvoiceRelationEntity : BaseEntity
    {
        public static TSInvoiceRelationEntity ReaderBind(IDataReader dataReader)
        {
            TSInvoiceRelationEntity model = new TSInvoiceRelationEntity();
            object ojb = new object();
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            ojb = dataReader["InvoiceId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.InvoiceId = (int)ojb;
            }
            ojb = dataReader["TSId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TSId = (int)ojb;
            }


            return model;
        }



        #region Model

        public int InvoiceId { get; set; }

        public int TSId { get; set; }

        #endregion Model
    }
}
