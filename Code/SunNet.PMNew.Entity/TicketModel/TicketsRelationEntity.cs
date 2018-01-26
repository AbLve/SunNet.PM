using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SunNet.PMNew.Entity.TicketModel
{
    public class TicketsRelationEntity
    {
        public TicketsRelationEntity()
        { }
        /// <summary>
        /// bind data
        /// </summary>
        public static TicketsRelationEntity ReaderBind(IDataReader dataReader)
        {
            TicketsRelationEntity model = new TicketsRelationEntity();
            object ojb;
            ojb = dataReader["RID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RID = (int)ojb;
            }
            ojb = dataReader["TID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TID = (int)ojb;
            }

            ojb = dataReader["RTID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RTID = (int)ojb;
            }
            ojb = dataReader["CreatedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedOn = (DateTime)ojb;
            }
            ojb = dataReader["CreatedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedBy = (int)ojb;
            }
            return model;
        }

        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int RID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int TID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int RTID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedOn { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int CreatedBy { set; get; }
        #endregion Model
    }
}
