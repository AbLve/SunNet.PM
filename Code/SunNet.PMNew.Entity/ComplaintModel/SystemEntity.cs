using SunNet.PMNew.Entity.Common;
using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ComplaintModel
{
    public class SystemEntity : BaseEntity
    {

        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static SystemEntity ReaderBind(IDataReader dataReader)
        {
            SystemEntity model = new SystemEntity();

            object ojb;
            ojb = dataReader["SystemID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SystemID = (int)ojb;
            }

            model.SystemName = dataReader["SystemName"].ToString(); 
            model.MD5Key = dataReader["MD5Key"].ToString();

            model.IP = dataReader["IP"].ToString();
            model.Port = dataReader["Port"].ToString();
            model.DBLocation = dataReader["DBLocation"].ToString();
            model.UserName = dataReader["UserName"].ToString();
            model.UserPwd = dataReader["UserPwd"].ToString();
            model.Procedure = dataReader["Procedure"].ToString();
            
            return model;
        }

        /// <summary>
        /// System ID in database
        /// </summary>
        public int SystemID { get; set; }

        /// <summary>
        /// System Name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// System's md5 key
        /// </summary>
        public string MD5Key { get; set; }

        /// <summary>
        /// System's IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// System's Port
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// System's DBLocation
        /// </summary>
        public string DBLocation { get; set; }

        /// <summary>
        /// System's User Name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// System's User Password
        /// </summary>
        public string UserPwd { get; set; }

        /// <summary>
        /// System's Procedure Name
        /// </summary>
        public string Procedure { get; set; }
    }
}