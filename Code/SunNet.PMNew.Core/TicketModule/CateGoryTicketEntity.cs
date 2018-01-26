using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using SunNet.PMNew.Framework.Core;
namespace SunNet.PMNew.Core.TicketModule
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
            ojb = dataReader["GategoryID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.GategoryID = (int)ojb;
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
        /// GategoryID
        /// </summary>		
        public int GategoryID { get; set; }
        /// <summary>
        /// TicketID
        /// </summary>		
        public int TicketID { get; set; }

    }
}