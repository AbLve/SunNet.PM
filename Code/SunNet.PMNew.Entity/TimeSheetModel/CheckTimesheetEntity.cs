using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.TimeSheetModel
{
    public class CheckTimesheetEntity 
    {
        public static CheckTimesheetEntity ReaderBind(IDataReader dataReader)
        {
            CheckTimesheetEntity model = new CheckTimesheetEntity();
            object ojb;

            ojb = dataReader["UserId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UserId = (int)ojb;
            }
            ojb = dataReader["FirstName"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FirstName = (string)ojb;
            }
            ojb = dataReader["LastName"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.LastName = (string)ojb;
            }
            ojb = dataReader["Email"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Email = (string)ojb;
            }
            ojb = dataReader["TimesheetDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TimesheetDate = (DateTime)ojb;
            }
            return model;
        }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime TimesheetDate { get; set; }

    }
}
