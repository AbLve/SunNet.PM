using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.EventModel
{
    public class WorkTimeEntity
    {
        public WorkTimeEntity()
        {
            
        }
        public WorkTimeEntity(IDataReader reader)
        {
            ID = (int)reader["ID"];
            UserID = (int)reader["UserID"];
            FromTime = (string)reader["FromTime"];
            FromTimeType = (int)reader["FromTimeType"];
            ToTime = (string)reader["ToTime"];
            ToTimeType = (int)reader["ToTimeType"];
            CreateOn = (DateTime)reader["CreateOn"];
        }
        public int ID { get; set; }
        public int UserID { get; set; }
        public string FromTime   { get; set; }
        public int FromTimeType { get; set; }
        public string ToTime { get; set; }
        public int ToTimeType { get; set; }
        public DateTime CreateOn { get; set; }
    }
}
