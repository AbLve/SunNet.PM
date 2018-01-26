using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.Common;

namespace SunNet.PMNew.Entity.EventModel
{
    public class EventInviteEntity : IShowUserName
    {
        public EventInviteEntity()
        {
        }

        public EventInviteEntity(IDataReader reader)
        {
            ID = (int)reader["ID"];
            CreatedID = (int)reader["CreatedID"];
            EventID = (int)reader["EventID"];
            UserID = (int)reader["UserID"];
            Status = (int)reader["Status"];
            FromDay = (DateTime)reader["FromDay"];
            Email = (string)reader["Email"];
            FirstName = (string)reader["FirstName"];
            LastName = (string)reader["LastName"];
        }


        public int ID { get; set; }

        /// <summary>
        /// Event的创建者
        /// </summary>
        public int CreatedID { get; set; }
        /// <summary>
        /// 聚集索引
        /// </summary>
        public int EventID { get; set; }

        /// <summary>
        /// 被邀请者
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 额外邀请的人，不是本系统账户
        /// </summary>
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        /// <summary>
        ///  0:失败 ;1:邀请; 2:加入; 3:拒绝; 4:忽略
        /// </summary>
        public int Status { get; set; }

        public DateTime FromDay { get; set; }

        /// <summary>
        /// 给前台使用的扩展属性
        /// </summary>
        public bool IsSeleted { get; set; }

        /// <summary>
        /// 给前台使用的扩展属性，编辑时使用(1： 正常的, 2：新加的, 3:删除的)
        /// </summary>
        public int OptionStatus { get; set; }

        /// <summary>
        /// 给前台使用的扩展属性  
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 给前台使用的扩展属性 
        /// </summary>
        public string CompanyName { get; set; }


        public string UserName
        {
            get { return FirstAndLastName; }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string FirstAndLastName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public string LastNameAndFirst
        {
            get
            {
                return string.Format("{0}, {1}", LastName, FirstName);
            }
        }
    }
}
