using System;
using SunNet.PMNew.Framework.Core;
using System.ComponentModel.DataAnnotations;
using System.Data;
namespace SunNet.PMNew.Entity.FileModel
{
    /// <summary>
    /// Directories
    /// </summary>
    public class DirectoryEntity : BaseEntity
    {
        public static DirectoryEntity ReaderBind(IDataReader dataReader)
        {
            DirectoryEntity model = new DirectoryEntity();
            object ojb = new object();
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            model.Title = dataReader["Title"].ToString();
            model.Description = dataReader["Description"].ToString();
            model.Logo = dataReader["Logo"].ToString();

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
            ojb = dataReader["ParentID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ParentID = (int)ojb;
            }
            model.Type = dataReader["Type"].ToString();
            model.ObjectType = (DirectoryObjectType)Enum.Parse(typeof(DirectoryObjectType), dataReader["Type"].ToString().Trim());
            model.Logo = model.ObjectType.ToString() + ".png";
            if (model.ObjectType == DirectoryObjectType.File)
            {
                if (model.Title.IndexOf(".") > 0)
                    model.Logo = System.IO.Path.GetExtension(model.Title).Substring(1) + ".png";
            }
            model.FirstName = dataReader["FirstName"].ToString();
            model.LastName = dataReader["LastName"].ToString();
            model.UserName = dataReader["UserName"].ToString();
            ojb = dataReader["ObjectID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ObjectID = (int)ojb;
            }
            return model;
        }
        public DirectoryEntity()
        { }
        #region Model

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Logo
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int ParentID
        {
            get;
            set;
        }

        /// <summary>
        /// Directory Ticket File (database does not save this property)
        /// </summary>
        public DirectoryObjectType ObjectType
        {
            get;
            set;
        }
        public int ObjectID { get; set; }
        public string Type
        {
            get;
            set;
        }
        /// <summary>
        /// database does not save this property
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// database does not save this property
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// database does not save this property
        /// </summary>
        public string UserName { get; set; }
        #endregion Model

    }
}

