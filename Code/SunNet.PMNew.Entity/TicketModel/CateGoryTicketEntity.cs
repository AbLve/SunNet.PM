using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using SunNet.PMNew.Framework.Core;


namespace SunNet.PMNew.Entity.TicketModel
{
    //CateGoryTicket
    public class CateGoryTicketEntity : BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static CateGoryTicketEntity ReaderBind(IDataReader dataReader)
        {
            CateGoryTicketEntity model = new CateGoryTicketEntity();
            object ojb;
            ojb = dataReader["CGTID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CGTID = (int)ojb;
            }
            ojb = dataReader["CategoryID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CategoryID = (int)ojb;
            }
            ojb = dataReader["TicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketID = (int)ojb;
            }
            ojb = dataReader["CreatedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedOn = (DateTime)ojb;
            }
            return model;
        }
        /// <summary>
        /// CGTID
        /// </summary>		
        public int CGTID { get; set; }
        /// <summary>
        /// CategoryID
        /// </summary>		
        public int CategoryID { get; set; }
        /// <summary>
        /// TicketID
        /// </summary>		
        public int TicketID { get; set; }

    }
}