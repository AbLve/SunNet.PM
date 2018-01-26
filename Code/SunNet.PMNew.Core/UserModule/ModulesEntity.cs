using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

using System.ComponentModel.DataAnnotations;

namespace SunNet.PMNew.Core.UserModule
{
    //Modules
    public class ModulesEntity : SunNet.PMNew.Framework.Core.BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static ModulesEntity ReaderBind(IDataReader dataReader)
        {
            ModulesEntity model = new ModulesEntity();
            object ojb;
            ojb = dataReader["ModuleID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModuleID = (int)ojb;
                model.ID = model.ModuleID;
            }
            model.ModuleTitle = dataReader["ModuleTitle"].ToString();
            model.ModulePath = dataReader["ModulePath"].ToString();
            ojb = dataReader["Status"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Status = (int)ojb;
            }
            ojb = dataReader["Orders"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Orders = (int)ojb;
            }
            ojb = dataReader["ParentID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ParentID = (int)ojb;
            }
            return model;
        }
        /// <summary>
        /// ModuleID
        /// </summary>		
        public int ModuleID { get; set; }
        /// <summary>
        /// ModuleTitle
        /// </summary>		
        [Required]
        [StringLength(50)]
        public string ModuleTitle { get; set; }
        /// <summary>
        /// ModulePath
        /// </summary>		
        [Required]
        [StringLength(50)]
        public string ModulePath { get; set; }
        /// <summary>
        /// Status
        /// </summary>		
        [Required]
        public int Status { get; set; }
        /// <summary>
        /// Orders
        /// </summary>		
        [Required]
        public int Orders { get; set; }
        /// <summary>
        /// ParentID
        /// </summary>		
        [Required]
        public int ParentID { get; set; }

    }
}