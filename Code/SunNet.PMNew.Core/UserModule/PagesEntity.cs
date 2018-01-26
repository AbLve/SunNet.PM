using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace SunNet.PMNew.Core.UserModule
{
    //Pages
    public class PagesEntity : SunNet.PMNew.Framework.Core.BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static PagesEntity ReaderBind(IDataReader dataReader)
        {
            PagesEntity model = new PagesEntity();
            object ojb;
            ojb = dataReader["PageID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PageID = (int)ojb;
                model.ID = model.PageID;
            }
            model.PageName = dataReader["PageName"].ToString();
            model.PageTitle = dataReader["PageTitle"].ToString();
            ojb = dataReader["MID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MID = (int)ojb;
            }
            ojb = dataReader["Orders"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Orders = (int)ojb;
            }
            ojb = dataReader["Status"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Status = (int)ojb;
            }
            ojb = dataReader["IsMenu"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsMenu = (bool)ojb;
            }
            return model;
        }
        /// <summary>
        /// PageID
        /// </summary>		
        public int PageID { get; set; }
        /// <summary>
        /// PageName
        /// </summary>		
        [Required]
        [StringLength(50)]
        public string PageName { get; set; }
        /// <summary>
        /// PageTitle
        /// </summary>		
        [Required]
        [StringLength(50)]
        public string PageTitle { get; set; }
        /// <summary>
        /// MID
        /// </summary>		
        [Required]
        public int MID { get; set; }
        /// <summary>
        /// Orders
        /// </summary>		
        [Required]
        public int Orders { get; set; }
        /// <summary>
        /// Status
        /// </summary>		
        [Required]
        public int Status { get; set; }
        /// <summary>
        /// IsMenu
        /// </summary>
        [Required]
        public bool IsMenu { get; set; }

    }
}