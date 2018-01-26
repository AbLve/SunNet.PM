using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SunNet.PMNew.Entity.SealModel
{
    public class SealFileEntity
    {

        public SealFileEntity()
        {
        }


        public SealFileEntity(IDataReader dataReader,bool isList)
        {
            ID = (int)dataReader["ID"];
            Title = (string)dataReader["Title"];
            Path = (string)dataReader["Path"];
            Name = (string)dataReader["Name"];
            SealRequestsID = (int)dataReader["SealRequestsID"];
            UserID = (int)dataReader["UserID"];
            IsDeleted = (bool)dataReader["IsDeleted"];
            WorkflowHistoryID = (int)dataReader["WorkflowHistoryID"];
            CreateOn = (DateTime)dataReader["CreateOn"];

            if (isList)
            {
                FirstName = (string)dataReader["FirstName"];
                LastName = (string)dataReader["LastName"];
            }
        }

        public int ID { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public int SealRequestsID { get; set; }

        public int UserID { get; set; }

        public bool IsDeleted { get; set; }

        public int WorkflowHistoryID { get; set; }

        public DateTime CreateOn { get; set; }

        #region
        public string FirstName { get; set; }
        public string LastName { get; set; }
        #endregion
    }
}
