using SunNet.PMNew.Entity.InvoiceModel.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.InvoiceModel.Proposal
{
    public class POListModel : InvoiceEntity
    {
        public static new POListModel ReaderBind(IDataReader dataReader)
        {
            POListModel model = new POListModel();

            object obj = new object();

            obj = dataReader["CompanyName"];

            if (obj != null && obj != DBNull.Value)
            {
                model.CompanyName = (string)obj;
            }

            obj = dataReader["PONo"];
            if (obj != null && obj != DBNull.Value)
            {
                model.PONo = (string)obj;
            }

            obj = dataReader["Title"];
            if (obj != null && obj != DBNull.Value)
            {
                model.Title = (string)obj;
            }

            obj = dataReader["ApprovedOn"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ApprovedOn = (DateTime)obj;
            }
            try
            {
                obj = dataReader["Milestone"];
                if (obj != null && obj != DBNull.Value)
                {
                    model.Milestone = obj.ToString();
                }
            }
            catch { 
            }

            obj = dataReader["InvoiceNo"];
            if (obj != null && obj != DBNull.Value)
            {
                model.InvoiceNo = (string)obj;
            }

            model.Status = (InvoiceStatus)Enum.Parse(typeof(InvoiceStatus), dataReader["Status"].ToString());
            return model;
        }


        public string  CompanyName { get; set; }
        public string PONo { get; set; }
        public string Title { get; set; }
        public DateTime ApprovedOn { get; set; }
    }
}
