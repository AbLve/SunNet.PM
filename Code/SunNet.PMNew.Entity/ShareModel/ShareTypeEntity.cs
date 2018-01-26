using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		5/25 20:29:43
 * Description:		Knowledge share type
 * Version History:	Created,5/25 20:29:43
 * 
 * 
 **************************************************************************/
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Entity.ShareModel
{
    /// <summary>
    /// Knowledge share type
    /// </summary>
    /// Author  :  Jack Zhang (JACKZ)
    /// Date    :  5/25 20:30
    public class ShareTypeEntity : BaseEntity
    {
        public ShareTypeEntity()
        {
            this.CreatedOn = DateTime.Now;
        }

        public ShareTypeEntity(int creater, ISystemDateTime timeProvider)
        {
            this.CreatedBy = creater;
            this.CreatedOn = timeProvider.Now;
        }

        public static ShareTypeEntity ReaderBind(IDataReader dataReader)
        {
            var model = new ShareTypeEntity();
            object obj;
            obj = dataReader["ID"];
            if (obj != null && obj != DBNull.Value)
                model.ID = (int)obj;
            model.Title = dataReader["Title"].ToString();
            obj = dataReader["CreatedBy"];
            if (obj != null && obj != DBNull.Value)
                model.CreatedBy = (int)obj;
            obj = dataReader["CreatedOn"];
            if (obj != null && obj != DBNull.Value)
                model.CreatedOn = (DateTime)obj;
            obj = dataReader["Type"];
            if (obj != null && obj != DBNull.Value)
                model.Type = (int)obj;
            return model;
        }

        /// <summary>
        /// 初始化KnowledgeShare时，同时初始化其相关联类型.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="initWithShareEntity">if set to <c>true</c> [initialize with share entity].</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/25 20:54
        internal static ShareTypeEntity ReaderBind(IDataReader dataReader, bool initWithShareEntity)
        {
            if (!initWithShareEntity)
                return ReaderBind(dataReader);
            var model = new ShareTypeEntity();
            object obj = dataReader["TypeID"];
            if (obj != null && obj != DBNull.Value)
                model.ID = (int)obj;
            model.Title = dataReader["TypeTitle"].ToString();
            obj = dataReader["TypeCreatedBy"];
            if (obj != null && obj != DBNull.Value)
                model.CreatedBy = (int)obj;
            obj = dataReader["TypeCreatedOn"];
            if (obj != null && obj != DBNull.Value)
                model.CreatedOn = (DateTime)obj;
            obj = dataReader["TypeType"];
            if (obj != null && obj != DBNull.Value)
                model.Type = (int)obj;
            return model;
        }

        public int ID { get; set; }
        [Required]
        [StringLength(64)]
        public string Title { get; set; }
        [Range(1, 999999, ErrorMessage = "Created By is required.")]
        public int CreatedBy { get; set; }
        [Range(typeof(DateTime), "2012-10-1", "2112-1-1")]
        public DateTime CreatedOn { get; set; }
        public int Type { get; set; }

    }
}
