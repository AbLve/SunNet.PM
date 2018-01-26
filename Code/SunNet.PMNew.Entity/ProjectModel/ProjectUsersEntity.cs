using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;

using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Entity.UserModel;
using System.Collections;


namespace SunNet.PMNew.Entity.ProjectModel
{
    //ProjectUsers
    public class ProjectUsersEntity : BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static ProjectUsersEntity ReaderBind(IDataReader dataReader)
        {
            ProjectUsersEntity model = new ProjectUsersEntity();
            object ojb;
            ojb = dataReader["PUID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PUID = (int)ojb;
            }
            ojb = dataReader["ProjectID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProjectID = (int)ojb;
            }
            ojb = dataReader["UserID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UserID = (int)ojb;
            }
            ojb = dataReader["ISClient"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ISClient = (bool)ojb;
            }
            ojb = dataReader["RoleID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RoleID = (int)ojb;
            }
            return model;
        }
        /// <summary>
        /// PUID
        /// </summary>		
        [Required]
        public int PUID { get; set; }
        /// <summary>
        /// ProjectID
        /// </summary>		
        [Required]
        public int ProjectID { get; set; }
        /// <summary>
        /// UserID
        /// </summary>		
        [Required]
        public int UserID { get; set; }
        /// <summary>
        /// ISClient
        /// </summary>		
        [Required]
        public bool ISClient { get; set; }

        /// <summary>
        /// 只读属性, 并不插入表,实时从User表读取
        /// </summary>
        public int RoleID { get; set; }

        public RolesEnum Role
        {
            get
            {
                return (RolesEnum)RoleID;
            }
        }


    }


    public class ProjectUsersComparer : IEqualityComparer<ProjectUsersEntity>
    {

        public bool Equals(ProjectUsersEntity x, ProjectUsersEntity y)
        {
            return (x.ProjectID == y.ProjectID) && (x.UserID == y.UserID);
        }

        public int GetHashCode(ProjectUsersEntity obj)
        {
            return obj.GetHashCode();
        }
    }
}