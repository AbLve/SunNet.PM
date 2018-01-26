using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ProposalTrackerModel
{
    public class ProjectPaymentEntity : BaseEntity
    {
        public static ProjectPaymentEntity ReaderBind(IDataReader dataReader)
        {
            ProjectPaymentEntity model = new ProjectPaymentEntity();
            object obj;

            obj = dataReader["ID"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ID = (int)obj;
            }
            obj = dataReader["ProposalTrackerID"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ProposalTrackerID = (int)obj;
            }
            obj = dataReader["MilestoneNo"];
            if (obj != null && obj != DBNull.Value)
            {
                model.MilestoneNo = (string)obj;
            }
            obj = dataReader["Approved"];
            if (obj != null && obj != DBNull.Value)
            {
                model.Approved = (bool)obj;
            }
            obj = dataReader["InvoiceNo"];
            if (obj != null && obj != DBNull.Value)
            {
                model.InvoiceNo = (string)obj;
            }
            obj = dataReader["SendDate"];
            if (obj != null && obj != DBNull.Value)
            {
                model.SendDate = (DateTime)obj;
            }
            obj = dataReader["ReceiveDate"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ReceiveDate = (DateTime)obj;
            }
            obj = dataReader["DueDate"];
            if (obj != null && obj != DBNull.Value)
            {
                model.DueDate = (DateTime)obj;
            }
            obj = dataReader["Color"];
            if (obj != null && obj != DBNull.Value)
            {
                model.Color = (string)obj;
            }
            obj = dataReader["CreatedOn"];
            if (obj != null && obj != DBNull.Value)
            {
                model.CreatedOn = (DateTime)obj;
            }
            obj = dataReader["CreatedBy"];
            if (obj != null && obj != DBNull.Value)
            {
                model.CreatedBy = (int)obj;
            }
            obj = dataReader["ModifiedOn"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ModifiedOn = (DateTime)obj;
            }
            obj = dataReader["ModifiedBy"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ModifiedBy = (int)obj;
            }
            obj = dataReader["ColorFor"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ColorFor = (string)obj;
            }
            obj = dataReader["Comment"];
            if (obj != null && obj != DBNull.Value)
            {
                model.Comment = (string)obj;
            }
            return model;
        }

        public int ProposalTrackerID { get; set; }

        public string MilestoneNo { get; set; }

        public bool Approved { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime? SendDate { get; set; }

        public DateTime? ReceiveDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string Color { get; set; }

        public string ColorFor { get; set; }

        public string Comment { get; set; }
    }
}