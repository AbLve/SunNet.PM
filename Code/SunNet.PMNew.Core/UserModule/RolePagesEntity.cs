using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;
namespace SunNet.PMNew.Core.UserModule
{
    //RolePages
    public class RolePagesEntity : SunNet.PMNew.Framework.Core.BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static RolePagesEntity ReaderBind(IDataReader dataReader)
        {
            RolePagesEntity model = new RolePagesEntity();
            object ojb;
            ojb = dataReader["RPID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RPID = (int)ojb;
                model.ID = model.RPID;
            }
            ojb = dataReader["RoleID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RoleID = (int)ojb;
            }
            ojb = dataReader["PageID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PageID = (int)ojb;
            }
            return model;
        }
        /// <summary>
        /// RPID
        /// </summary>		
        public int RPID { get; set; }
        /// <summary>
        /// RoleID
        /// </summary>		
        [Required]
        public int RoleID { get; set; }
        /// <summary>
        /// PageID
        /// </summary>		
        [Required]
        public int PageID { get; set; }

    }
}