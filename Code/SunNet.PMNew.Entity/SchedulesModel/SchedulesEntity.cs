using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SunNet.PMNew.Entity.SchedulesModel
{
    [Serializable]
    public class SchedulesEntity
    {
        public SchedulesEntity() { }

        public SchedulesEntity(IDataReader dataReader)
        {
            ID = (int)dataReader["ID"];
            Title = (string)dataReader["Title"];
            StartTime = (string)dataReader["StartTime"];
            EndTime = (string)dataReader["EndTime"];
            Description = (string)dataReader["Description"];
            CreateOn = (DateTime)dataReader["CreateOn"];
            CreateBy = (int)dataReader["CreateBy"];
            UpdateOn = (DateTime)dataReader["UpdateOn"];
            UpdateBy = (int)dataReader["UpdateBy"];


            MeetingStatus = (int)dataReader["MeetingStatus"];
            MeetingID = (string)dataReader["MeetingID"];
            PlanDate = (DateTime)dataReader["PlanDate"];
            UserID = (int)dataReader["UserID"];
        }


        public int ID { get; set; }

        public string Title { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Description { get; set; }

       
        public DateTime CreateOn { get; set; }

        /// <summary>
        /// 创建者不一定是所有者，如meeting
        /// </summary>
        public int CreateBy { get; set; }

        public DateTime UpdateOn { get; set; }

        public int UpdateBy { get; set; }

        /// <summary>
        /// 0:normal ; 1: waits for the acknowledgment  ; 2: acknowledgment ;3:cancel
        /// </summary>
        public int MeetingStatus { get; set; }

        public string MeetingID { get; set; }

        /// <summary>
        /// 计划日期
        /// </summary>
        public DateTime PlanDate { get; set; }

        /// <summary>
        /// Schedule的所有者
        /// </summary>
        public int UserID { get; set; }
    }
}
