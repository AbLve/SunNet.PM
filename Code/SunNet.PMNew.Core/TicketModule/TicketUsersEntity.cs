using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

using SunNet.PMNew.Framework.Core;
namespace SunNet.PMNew.Core.TicketModule
{
    //TicketUsers
    public class TicketUsersEntity:BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static TicketUsersEntity ReaderBind(IDataReader dataReader)
        {
            TicketUsersEntity model = new TicketUsersEntity();
            object ojb;
            ojb = dataReader["TUID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TUID = (int)ojb;
            }
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
        /// TUID
        /// </summary>		
        public int TUID { get; set; }
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