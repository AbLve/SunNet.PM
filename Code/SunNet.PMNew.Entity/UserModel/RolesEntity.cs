using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace SunNet.PMNew.Entity.UserModel
{
    //Roles
    public class RolesEntity : SunNet.PMNew.Framework.Core.BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static RolesEntity ReaderBind(IDataReader dataReader)
        {
            RolesEntity model = new RolesEntity();
            object ojb;
            ojb = dataReader["RoleID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RoleID = (int)ojb;
                model.ID = model.RoleID;
            }
            model.RoleName = dataReader["RoleName"].ToString();
            model.Description = dataReader["Description"].ToString();
            ojb = dataReader["Status"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Status = (int)ojb;
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

        /// <summary>
        /// RoleID
        /// </summary>		
        public int RoleID { get; set; }
        /// <summary>
        /// RoleName
        /// </summary>		
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }
        /// <summary>
        /// Description
        /// </summary>		
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        /// <summary>
        /// Status
        /// </summary>		
        [Required]
        public int Status { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>		
        [Required]
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>		
        [Required]
        public int CreatedBy { get; set; }

    }
}