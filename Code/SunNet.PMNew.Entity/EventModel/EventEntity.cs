using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.EventModel
{
    public class EventEntity
    {
        public EventEntity()
        {
        }

        public EventEntity(IDataReader reader)
        {
            ID = (int)reader["ID"];
            Icon = (int)reader["Icon"];
            Name = (string)reader["Name"];
            Details = (string)reader["Details"];
            Where = (string)reader["Where"];
            AllDay = (bool)reader["AllDay"];
            FromDay = (DateTime)reader["FromDay"];
            FromTime = (string)reader["FromTime"];
            FromTimeType = (int)reader["FromTimeType"];
            ToDay = (DateTime)reader["ToDay"];
            ToTime = (string)reader["ToTime"];
            ToTimeType = (int)reader["ToTimeType"];
            Privacy = (Privates)((int)reader["Privacy"]);
            HasInvite = (bool)reader["HasInvite"];
            GroupID = (string)reader["GroupID"];
            CreatedBy = (int)reader["CreatedBy"];
            CreatedOn = (DateTime)reader["CreatedOn"];
            Highlight = (bool)reader["Highlight"];
            UpdatedOn = (DateTime)reader["UpdatedOn"];
            Alert = (AlertType)((int)reader["Alert"]);
            ProjectID = (int)reader["ProjectID"];
            Times = Convert.ToInt32(reader["Times"] == DBNull.Value ? "0" : reader["Times"]);
            IsOff = (bool)reader["IsOff"];
        }



        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// 图标ID
        /// </summary>
        public int Icon
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Details
        {
            get;
            set;
        }

        public string Where
        {
            get;
            set;
        }

        public bool AllDay
        {
            get;
            set;
        }

        public DateTime FromDay
        {
            get;
            set;
        }

        /// <summary>
        /// AllDay 时为0 ；
        /// </summary>
        public string FromTime
        {
            get;
            set;
        }

        /// <summary>
        /// AllDay 时为0; AM=1 or PM=2
        /// </summary>
        public int FromTimeType
        {
            get;
            set;
        }

        public DateTime ToDay
        {
            get;
            set;
        }

        /// <summary>
        /// AllDay 时为0 ； 
        /// </summary>
        public string ToTime
        {
            get;
            set;
        }

        /// <summary>
        /// AllDay 时为0; AM=1 or PM=2
        /// </summary>
        public int ToTimeType
        {
            get;
            set;
        }

        /// <summary>
        /// 1、只限自己；2、表示所有好友，在页面中以 P 表示；3、指定组；4、只有特邀 以 0 表示；
        /// </summary>
        public Privates Privacy
        {
            get;
            set;
        }

        /// <summary>
        /// 有邀请人
        /// </summary>
        public bool HasInvite
        {
            get;
            set;
        }

        /// <summary>
        /// Privacy 对应的值，以逗号分开
        /// </summary>
        public string GroupID
        {
            get;
            set;
        }

        /// <summary>
        /// 聚集索引
        /// </summary>
        public int CreatedBy
        {
            get;
            set;
        }

        public DateTime CreatedOn
        {
            get;
            set;
        }

        public bool Highlight
        {
            get;
            set;
        }

        public AlertType Alert
        {
            get;
            set;
        }

        public DateTime UpdatedOn
        {
            get;
            set;
        }

        public string IconPath
        {
            get
            {
                return EventIconAgent.BuidlerIcon(Icon);
            }
        }

        public int ProjectID { get; set; }

        /// <summary>
        /// 已发送过提醒
        /// </summary>
        public bool HasAlert { get; set; }

        public string FromDayString { get { return FromDay.ToString("MM/dd/yyyy"); } }
        public string ToDayString { get { return ToDay.ToString("MM/dd/yyyy"); } }

        public int Times { get; set; }

        /// <summary>
        /// 是否是OffTicket
        /// </summary>
        public bool IsOff { get; set; }
    }
}
