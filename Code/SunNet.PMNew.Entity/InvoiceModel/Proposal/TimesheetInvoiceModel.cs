using SunNet.PMNew.Entity.TimeSheetModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.InvoiceModel.Proposal
{
    public class TimesheetInvoiceModel : TimeSheetsEntity
    {
        public static new TimesheetInvoiceModel ReaderBind(IDataReader dataReader)
        {
            TimesheetInvoiceModel model = new TimesheetInvoiceModel();

            object obj = new object();

            obj = dataReader["ID"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ID = (int)obj;
            }

            obj = dataReader["SheetDate"];
            if (obj != null && obj != DBNull.Value)
            {
                model.SheetDate = (DateTime)obj;
            }
            //obj = dataReader["InvoiceNo"];
            //if (obj != null && obj != DBNull.Value)
            //{
            //    model.InvoiceNo = (string)obj;
            //}
            obj = dataReader["CompanyName"];
            if (obj != null && obj != DBNull.Value)
            {
                model.CompanyName = (string)obj;
            }
            obj = dataReader["ProjectTitle"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ProjectTitle = (string)obj;
            }
            obj = dataReader["TicketID"];
            if (obj != null && obj != DBNull.Value)
            {
                model.TicketID = (int)obj;
            }
            obj = dataReader["TicketTitle"];
            if (obj != null && obj != DBNull.Value)
            {
                model.TicketTitle = (string)obj;
            }
            obj = dataReader["FirstName"];
            if (obj != null && obj != DBNull.Value)
            {
                model.UserName = (string)obj;
            }
            obj = dataReader["Hours"];
            if (obj != null && obj != DBNull.Value)
            {
                model.Hours = (decimal)obj;
            }
            return model;
        }

        public string CompanyName { get; set; }

        public string ProjectTitle { get; set; }

        public string TicketTitle { get; set; }

        public string UserName { get; set; }
    }
}
