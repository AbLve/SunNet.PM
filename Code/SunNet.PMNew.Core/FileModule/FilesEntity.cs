using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.Core.FileModule
{
    //Files
    public class FilesEntity:BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static FilesEntity ReaderBind(IDataReader dataReader)
        {
            FilesEntity model = new FilesEntity();
            object ojb;
            ojb = dataReader["FileID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FileID = (int)ojb;
            }
            ojb = dataReader["SourceID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SourceID = (int)ojb;
            }
            ojb = dataReader["SourceType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SourceType = (int)ojb;
            }
            model.FileTitle = dataReader["FileTitle"].ToString();
            model.ContentType = dataReader["ContentType"].ToString();
            ojb = dataReader["FileSize"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FileSize = (int)ojb;
            }
            model.FilePath = dataReader["FilePath"].ToString();
            model.ThumbPath = dataReader["ThumbPath"].ToString();
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
            ojb = dataReader["IsDelete"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsDelete = (bool)ojb;
            }
            ojb = dataReader["IsPublic"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsPublic = (bool)ojb;
            }
            return model;
        }
        /// <summary>
        /// FileID
        /// </summary>		
        public int FileID { get; set; }
        /// <summary>
        /// SourceID
        /// </summary>		
        public int SourceID { get; set; }
        /// <summary>
        /// SourceType
        /// </summary>		
        public int SourceType { get; set; }
        /// <summary>
        /// FileTitle
        /// </summary>		
        public string FileTitle { get; set; }
        /// <summary>
        /// ContentType
        /// </summary>		
        public string ContentType { get; set; }
        /// <summary>
        /// FileSize
        /// </summary>		
        public int FileSize { get; set; }
        /// <summary>
        /// FilePath
        /// </summary>		
        public string FilePath { get; set; }
        /// <summary>
        /// ThumbPath
        /// </summary>		
        public string ThumbPath { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>		
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>		
        public int CreatedBy { get; set; }
        /// <summary>
        /// IsDelete
        /// </summary>		
        public bool IsDelete { get; set; }
        /// <summary>
        /// IsPublic
        /// </summary>		
        public bool IsPublic { get; set; }

    }
}