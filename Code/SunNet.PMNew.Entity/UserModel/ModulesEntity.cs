using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace SunNet.PMNew.Entity.UserModel
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
            model.DefaultPage = dataReader["DefaultPage"].ToString();
            model.ClickFunctioin = dataReader["ClickFunctioin"].ToString();
            ojb = dataReader["ShowInMenu"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ShowInMenu = (bool)ojb;
            }
            ojb = dataReader["PageOrModule"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PageOrModule = (int)ojb;
            }
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
        #region IsPage IsModule
        /// <summary>
        /// Current item is a page or not
        /// </summary>
        public bool IsPage
        {
            get { return this.PageOrModule == 0; }
        }
        /// <summary>
        ///  Current item is a module or not
        /// </summary>
        public bool IsModule
        {
            get { return !IsPage; }
        }
        #endregion

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
        [StringLength(500)]
        public string ModulePath { get; set; }
        /// <summary>
        /// DefaultPage
        /// </summary>
        [Required]
        [StringLength(500)]
        public string DefaultPage { set; get; }
        /// <summary>
        /// extra click function for module 
        /// </summary>
        [Required]
        [StringLength(500)]
        public string ClickFunctioin { set; get; }
        /// <summary>
        /// ShowInMenu
        /// </summary>
        [Required]
        public bool ShowInMenu { set; get; }
        /// <summary>
        /// PageOrModule
        /// </summary>
        [Required]
        public int PageOrModule { set; get; }

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