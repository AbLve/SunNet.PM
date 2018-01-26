using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SunNet.PMNew.Entity.TicketModel
{
    public class TicketDistinctUsersResponse
    {
        public static TicketDistinctUsersResponse ReaderBind(IDataReader dataReader)
        {
            TicketDistinctUsersResponse model = new TicketDistinctUsersResponse();
            object ojb;
            ojb = dataReader["TicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketID = (int)ojb;
            }
            ojb = dataReader["UserID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UserID = (int)ojb;
            }
            return model;
        }
        /// <summary>
        /// TicketID
        /// </summary>		
        public int TicketID { get; set; }
        /// <summary>
        /// UserID
        /// </summary>		
        public int UserID { get; set; }
    }
}
