using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.Entity.TicketModel
{
    //TicketsOrder
    public class TicketsOrderEntity:BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static TicketsOrderEntity ReaderBind(IDataReader dataReader)
        {
            TicketsOrderEntity model = new TicketsOrderEntity();
            object ojb;
            ojb = dataReader["TicketOrderID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketOrderID = (int)ojb;
            }
            ojb = dataReader["ProjectID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProjectID = (int)ojb;
            }
            ojb = dataReader["TicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketID = (int)ojb;
            }
            ojb = dataReader["OrderNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OrderNum = (int)ojb;
            }
            return model;
        }
        /// <summary>
        /// TicketOrderID
        /// </summary>		
        public int TicketOrderID { get; set; }
        /// <summary>
        /// ProjectID
        /// </summary>		
        public int ProjectID { get; set; }
        /// <summary>
        /// TicketID
        /// </summary>		
        public int TicketID { get; set; }
        /// <summary>
        /// OrderNum
        /// </summary>		
        public int OrderNum { get; set; }

    }
}