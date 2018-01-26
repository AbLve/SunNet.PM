using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.Core.TicketModule
{
    //CateGory
    public class CateGoryEntity : BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static CateGoryEntity ReaderBind(IDataReader dataReader)
        {
            CateGoryEntity model = new CateGoryEntity();
            object ojb;
            ojb = dataReader["GID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.GID = (int)ojb;
            }
            model.Title = dataReader["Title"].ToString();
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
            ojb = dataReader["IsOnlyShowTody"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsOnlyShowTody = (bool)ojb;
            }
            ojb = dataReader["IsDelete"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsDelete = (bool)ojb;
            }
            return model;
        }
        /// <summary>
        /// GID
        /// </summary>		
        public int GID { get; set; }
        /// <summary>
        /// Title
        /// </summary>		
        public string Title { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>		
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>		
        public int CreatedBy { get; set; }
        /// <summary>
        /// IsOnlyShowTody
        /// </summary>		
        public bool IsOnlyShowTody { get; set; }
        /// <summary>
        /// IsDelete
        /// </summary>		
        public bool IsDelete { get; set; }

    }
}