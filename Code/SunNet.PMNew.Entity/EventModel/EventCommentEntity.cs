using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.EventModel
{
   public class EventCommentEntity
    {
        public EventCommentEntity(IDataReader reader)
        {
            ID = (int)reader["ID"];
            EventID = (int)reader["EventID"];
            UserID = (int)reader["UserID"];
            Context = (string)reader["Context"];
            ParentID = (int)reader["ParentID"];
            CreateOn = (DateTime)reader["CreateOn"];
        }

        public int ID { get; set; }

        public int EventID { get; set; }

        public int UserID { get; set; }

        public string Context { get; set; }

        public int ParentID { get; set; }

        public DateTime CreateOn { get; set; }
    }
}
