using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SunNet.PMNew.Entity.SealModel
{
    public class SealNotesEntity
    {
        public SealNotesEntity()
        {
        }

        public SealNotesEntity(IDataReader dataReader, bool isList)
        {
            ID = (int)dataReader["ID"];
            Title = (string)dataReader["Title"];
            SealRequestsID = (int)dataReader["SealRequestsID"];
            Description = (string)dataReader["Description"];
            UserID = (int)dataReader["UserID"];
            CreateOn = (DateTime)dataReader["CreateOn"];
            if (isList)
            {
                FirstName = (string)dataReader["FirstName"];
                LastName = (string)dataReader["LastName"];
            }
        }

        public int ID { get; set; }

        public string Title { get; set; }

        public int SealRequestsID { get; set; }

        public string Description { get; set; }

        public int UserID { get; set; }

        public DateTime CreateOn { get; set; }

        #region
        public string FirstName { get; set; }

        public string LastName { get; set; }
        #endregion
    }
}
