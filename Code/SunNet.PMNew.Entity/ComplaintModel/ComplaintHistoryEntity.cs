using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ComplaintModel
{
    public class ComplaintHistoryEntity : BaseEntity
    {
        public static ComplaintHistoryEntity ReaderBind(IDataReader dataReader)
        {
            ComplaintHistoryEntity model = new ComplaintHistoryEntity();
            object ojb;
            ojb = dataReader["CHID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CHID = (int)ojb;
            }
            ojb = dataReader["ComplaintID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ComplaintID = (int)ojb;
            }
            model.Comments = dataReader["Comments"].ToString();
            ojb = dataReader["ModifiedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifiedOn = (DateTime)ojb;
            }

            model.ModifiedByName = dataReader["ModifiedByName"].ToString();
            model.Action = dataReader["Action"].ToString();

            return model;
        }

        /// <summary>
        /// CHID
        /// </summary>
        public int CHID { get; set; }
        /// <summary>
        /// ComplaintID
        /// </summary>		
        public int ComplaintID { get; set; }
        /// <summary>
        /// Comments
        /// </summary>		
        public string Comments { get; set; }
        /// <summary>
        /// ModifiedOn
        /// </summary>		
        public DateTime ModifiedOn { get; set; }
        /// <summary>
        /// ModifiedBy
        /// </summary>		
        public string ModifiedByName { get; set; }

        public int ModifiedByID { get; set; }
        public string Action { get; set; }
    }
}
