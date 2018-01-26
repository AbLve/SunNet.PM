using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;
namespace SunNet.PMNew.Core.UserModule
{
    //RoleModules
    public class RoleModulesEntity : SunNet.PMNew.Framework.Core.BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static RoleModulesEntity ReaderBind(IDataReader dataReader)
        {
            RoleModulesEntity model = new RoleModulesEntity();
            object ojb;
            ojb = dataReader["RMID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RMID = (int)ojb;
                model.ID = model.RMID;
            }
            ojb = dataReader["RoleID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RoleID = (int)ojb;
            }
            ojb = dataReader["ModuleID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModuleID = (int)ojb;
            }
            return model;
        }
        /// <summary>
        /// RMID
        /// </summary>		
        public int RMID { get; set; }
        /// <summary>
        /// RoleID
        /// </summary>		
        [Required]
        public int RoleID { get; set; }
        /// <summary>
        /// ModuleID
        /// </summary>		
        [Required]
        public int ModuleID { get; set; }

    }
}