using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FamilyBook.Entity
{
    public class CommentEntity
    {
        public int ID { get; set; }
        public int ReplyID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Office { get; set; }
        public string UserType { get; set; }
        public DateTime CreatedTime { get; set; }
        //添加下面2个属性 20140725
        public int UserID { get; set; }
        public int DocumentID { get; set; }

        public List<CommentEntity> ReplyList { get; set; }
        public  string  CreatedToString {
            get { return CreatedTime.ToString("MM/dd/yyyy HH:mm:ss"); }
        }


        public CommentEntity() { }

        public CommentEntity(IDataReader reader)
        {
            ID = Convert.ToInt32(reader["ID"]);
            ReplyID = Convert.ToInt32(reader["ReplyID"] ?? 0);
            UserID = Convert.ToInt32(reader["UserID"] ?? 0);
            DocumentID = Convert.ToInt32(reader["DocumentID"] ?? 0);
            Name = reader["Name"] != null ? reader["Name"].ToString() : "";
            Content = reader["Content"] != null ? reader["Content"].ToString() : "";
            Office = reader["Office"] != null ? reader["Office"].ToString() : "";
            UserType = reader["UserType"] != null ? reader["UserType"].ToString() : "";
            CreatedTime = reader["CreatedTime"] != null ? Convert.ToDateTime(reader["CreatedTime"].ToString()) : System.DateTime.Now;
        }
    }
}
