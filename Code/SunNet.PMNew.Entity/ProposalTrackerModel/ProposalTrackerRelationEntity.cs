using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SunNet.PMNew.Entity.ProposalTrackerModel
{
    public class ProposalTrackerRelationEntity
    {

        public ProposalTrackerRelationEntity()
        { }
        /// <summary>
        /// bind data
        /// </summary>
        public static ProposalTrackerRelationEntity ReaderBind(IDataReader dataReader)
        {
            ProposalTrackerRelationEntity model = new ProposalTrackerRelationEntity();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            ojb = dataReader["WID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.WID = (int)ojb;
            }

            ojb = dataReader["TID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TID = (int)ojb;
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
        public int ID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int WID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int TID { set; get; }
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
