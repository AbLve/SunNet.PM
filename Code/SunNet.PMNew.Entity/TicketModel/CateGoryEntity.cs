using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using SunNet.PMNew.Framework.Core;
using System.ComponentModel.DataAnnotations;

namespace SunNet.PMNew.Entity.TicketModel
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
                model.ID = model.GID;
            }
            model.Title = dataReader["Title"].ToString();
            ojb = dataReader["CreatedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedOn = (DateTime)ojb;
                model.ModifiedOn = model.CreatedOn;
            }
            ojb = dataReader["CreatedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedBy = (int)ojb;
            }
            ojb = dataReader["IsOnlyShowTody"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsProtected = (bool)ojb;
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
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        /// <summary>
        /// 是否受保护的Category,受保护的Category通常是公共的，不能被删除
        /// </summary>		
        public bool IsProtected { get; set; }
        /// <summary>
        /// IsDelete
        /// </summary>		
        public bool IsDelete { get; set; }

    }
}