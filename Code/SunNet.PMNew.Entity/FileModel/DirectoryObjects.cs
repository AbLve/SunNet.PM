using System;
using SunNet.PMNew.Framework.Core;
using System.ComponentModel.DataAnnotations;
using System.Data;
namespace SunNet.PMNew.Entity.FileModel
{
    public enum DirectoryObjectType
    {
        Directory,
        File,
        Ticket
    }
    /// <summary>
    /// DirectoryObjects
    /// </summary>
    public class DirectoryObjectsEntity : BaseEntity
    {
        public static DirectoryObjectsEntity ReaderBind(IDataReader dataReader)
        {
            DirectoryObjectsEntity model = new DirectoryObjectsEntity();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            ojb = dataReader["DirectoryID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DirectoryID = (int)ojb;
            }
            model.Type = dataReader["Type"].ToString();
            model.Logo = dataReader["Logo"].ToString();
            ojb = dataReader["ObjectID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ObjectID = (int)ojb;
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
            ojb = dataReader["ModifiedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifiedOn = (DateTime)ojb;
            }
            ojb = dataReader["ModifiedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifiedBy = (int)ojb;
            }
            return model;
        }
        public DirectoryObjectsEntity()
        { }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int DirectoryID
        {
            get;
            set;
        }
        /// <summary>
        /// Directory Ticket File
        /// </summary>
        public DirectoryObjectType ObjectType
        {
            get
            {
                return (DirectoryObjectType)Enum.Parse(typeof(DirectoryObjectType), this.Type);
            }
        }
        public string Type
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Logo
        {
            get;
            set;
        }
        /// <summary>
        /// Ticket ID or File ID
        /// </summary>
        [Required]
        public int ObjectID
        {
            get;
            set;
        }
        #endregion Model

    }
}

