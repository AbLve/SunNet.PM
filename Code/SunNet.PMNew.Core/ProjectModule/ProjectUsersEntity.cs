using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

using SunNet.PMNew.Framework.Core;
namespace SunNet.PMNew.Core.ProjectModule
{
    //ProjectUsers
    public class ProjectUsersEntity:BaseEntity
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
            return model;
        }
        /// <summary>
        /// PUID
        /// </summary>		
        public int PUID { get; set; }
        /// <summary>
        /// ProjectID
        /// </summary>		
        public int ProjectID { get; set; }
        /// <summary>
        /// UserID
        /// </summary>		
        public int UserID { get; set; }
        /// <summary>
        /// ISClient
        /// </summary>		
        public bool ISClient { get; set; }

    }
}