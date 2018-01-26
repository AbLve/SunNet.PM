using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;
using SunNet.PMNew.Entity.Common;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Entity.ProposalTrackerModel.Enums;

namespace SunNet.PMNew.Entity.ProposalTrackerModel
{
    public class ProposalTrackerNoteEntity : BaseEntity, IShowUserName
    {

        public static ProposalTrackerNoteEntity ReaderBind(IDataReader dataReader)
        {
            ProposalTrackerNoteEntity model = new ProposalTrackerNoteEntity();
            object ojb;

            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            ojb = dataReader["ProposalTrackerID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProposalTrackerID = (int)ojb;
            }
            ojb = dataReader["ModifyOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifyOn = (DateTime)ojb;
            }
            ojb = dataReader["ModifyBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifyBy = (int)ojb;
            }
            model.Title = dataReader["Title"].ToString();
            model.Description = dataReader["Description"].ToString();
            model.UserName = dataReader["UserName"].ToString();
            model.FirstName = dataReader["FirstName"].ToString();
            model.LastName = dataReader["LastName"].ToString();
            return model;
        }


        public int ID { get; set; }
        public int ProposalTrackerID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int ModifyBy { get; set; }
        public DateTime ModifyOn { get; set; }

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
