using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.Entity.TicketModel
{
    //TicketHistorys
    public class TicketHistorysEntity : BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static TicketHistorysEntity ReaderBind(IDataReader dataReader)
        {
            TicketHistorysEntity model = new TicketHistorysEntity();
            object ojb;
            ojb = dataReader["TDHID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TDHID = (int)ojb;
            }
            ojb = dataReader["TicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketID = (int)ojb;
            }
            model.Description = dataReader["Description"].ToString();
            ojb = dataReader["ModifiedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifiedOn = (DateTime)ojb;
            }
            ojb = dataReader["ModifiedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifiedBy = (int)ojb;
            }
            ojb = dataReader["ResponsibleUserId"];
            if (ojb != null && ojb != DBNull.Value)
                model.ResponsibleUserId = (int)ojb;
            return model;
        }
        /// <summary>
        /// TDHID
        /// </summary>		
        public int TDHID { get; set; }
        /// <summary>
        /// TicketID
        /// </summary>		
        public int TicketID { get; set; }
        /// <summary>
        /// Description
        /// </summary>		
        public string Description { get; set; }
        /// <summary>
        /// ModifiedOn
        /// </summary>		
        public DateTime ModifiedOn { get; set; }
        /// <summary>
        /// ModifiedBy
        /// </summary>		
        public int ModifiedBy { get; set; }


        public int ResponsibleUserId { get; set; }
    }
}