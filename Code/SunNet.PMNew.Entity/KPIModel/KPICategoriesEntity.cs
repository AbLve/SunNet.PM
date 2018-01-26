using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace SunNet.PMNew.Entity.KPIModel
{

    public class KPICategoriesEntity : SunNet.PMNew.Framework.Core.BaseEntity   
    {
        public static KPICategoriesEntity ReaderBind(IDataReader dataReader)
        {
            KPICategoriesEntity model = new KPICategoriesEntity();
            object ojb;     
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
                model.ID = model.ID;
            }
            model.CategoryName = dataReader["Name"].ToString();
  
            ojb = dataReader["Status"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Status = (int)ojb;
            }

            return model;
        }

        public static KPICategoriesEntity CreateKPICategoriesEntity()
        {
            KPICategoriesEntity model = new KPICategoriesEntity();

            model.ID = 0;
            model.CategoryName = string.Empty;
            model.Status = 0;

            return model;
        }

        /// <summary>
        /// CategoryName
        /// </summary>		
        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }
           /// <summary>
        /// Status
        /// </summary>		
        [Required]
        public int Status { get; set; }


    }
}
