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
 * CreatedOn:		5/25 20:29:54
 * Description:		Knowledge share
 * Version History:	Created,5/25 20:29:54
 * 
 * 
 **************************************************************************/
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Entity.ShareModel
{
    /// <summary>
    /// Knowledge share class
    /// </summary>
    /// Author  :  Jack Zhang (JACKZ)
    /// Date    :  5/25 20:36
    public class ShareEntity : BaseEntity
    {
        public ShareEntity()
        {
            this.CreatedOn = DateTime.Now;
            this.ModifiedOn = DateTime.Now;
            this.TypeEntity = new ShareTypeEntity();
        }

        public ShareEntity(int creater, ISystemDateTime timeProvider)
        {
            this.CreatedBy = creater;
            this.CreatedOn = timeProvider.Now;
            this.ModifiedOn = timeProvider.Now;
            this.TypeEntity = new ShareTypeEntity(creater, timeProvider);
        }

        public static ShareEntity ReaderBind(IDataReader dataReader)
        {
            ShareEntity model = new ShareEntity();
            object obj;
            obj = dataReader["ID"];
            if (obj != null && obj != DBNull.Value)
                model.ID = (int)obj;
            model.Title = dataReader["Title"].ToString();
            model.Note = dataReader["Note"].ToString();

            obj = dataReader["CreatedBy"];
            if (obj != null && obj != DBNull.Value)
                model.CreatedBy = (int)obj;
            obj = dataReader["CreatedOn"];
            if (obj != null && obj != DBNull.Value)
                model.CreatedOn = (DateTime)obj;
            obj = dataReader["ModifiedOn"];
            if (obj != null && obj != DBNull.Value)
                model.ModifiedOn = (DateTime)obj;
            obj = dataReader["Type"];
            if (obj != null && obj != DBNull.Value)
                model.Type = (int)obj;
            obj = dataReader["TicketID"];
            if (obj != null && obj != DBNull.Value)
                model.TicketID = (int)obj;
            if (dataReader.Contains("TypeID"))
            {
                obj = dataReader["TypeID"];
                if (obj != null && obj != DBNull.Value)
                {
                    model.TypeEntity = ShareTypeEntity.ReaderBind(dataReader, true);
                }
            }
            obj = dataReader["Files"];
            model.Files = new Dictionary<int, string>();
            if (obj != null && obj != DBNull.Value)
            {
                string[] files = obj.ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var file in files)
                {
                    int fileID = 0;
                    int.TryParse(file.Substring(0, file.IndexOf("_")), out fileID);
                    var filetitle = file.Substring(file.IndexOf("_") + 1);
                    model.Files.Add(fileID, filetitle);
                }
            }
            return model;
        }

        public new int ID { get; set; }
        [Required]
        [StringLength(512)]
        public string Title { get; set; }
        [Required]
        [StringLength(512)]
        public string Note { get; set; }
        [Range(1, 999999, ErrorMessage = "Created By is required.")]
        public new int CreatedBy { get; set; }
        [Range(typeof(DateTime), "2012-10-1", "2112-1-1")]
        public new DateTime CreatedOn { get; set; }
        [Range(typeof(DateTime), "2012-10-1", "2112-1-1")]
        public new DateTime ModifiedOn { get; set; }
        public int TicketID { get; set; }
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Knowledge share type.
        /// </summary>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/25 20:36
        public int Type { get; set; }

        /// <summary>
        /// 具体的类型
        /// </summary>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/25 20:48
        public ShareTypeEntity TypeEntity { get; set; }

        public virtual Dictionary<int, string> Files { get; set; }
    }
}
