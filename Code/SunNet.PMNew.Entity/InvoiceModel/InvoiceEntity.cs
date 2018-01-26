using SunNet.PMNew.Entity.InvoiceModel.Enums;
using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Extensions;

namespace SunNet.PMNew.Entity.InvoiceModel
{
    public class InvoiceEntity : BaseEntity
    {
        public static InvoiceEntity ReaderBind(IDataReader dataReader)
        {
            InvoiceEntity model = new InvoiceEntity();
            object obj = new object();
            obj = dataReader["ID"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ID = (int)obj;
            }
            obj = dataReader["ProposalId"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ProposalId = (int)obj;
            }

            obj = dataReader["Milestone"];
            if (obj != null && obj != DBNull.Value)
            {
                model.Milestone = obj.ToString();
            }

            obj = dataReader["Approved"];
            if (obj != null && obj != DBNull.Value)
            {
                model.Approved = (bool)obj;
            }

            model.InvoiceNo = dataReader["InvoiceNo"].ToString();

            obj = dataReader["Status"];
            if (obj != null && obj != DBNull.Value)
            {
                model.Status = (InvoiceStatus)Enum.Parse(typeof(InvoiceStatus), dataReader["Status"].ToString().Trim());
            }
            else
            {
                if (string.IsNullOrEmpty(model.Milestone))
                    model.Status = InvoiceStatus.Missing_Milestone;
                else if (string.IsNullOrEmpty(model.InvoiceNo))
                    model.Status = InvoiceStatus.Missing_Invoice;
            }

            obj = dataReader["SendOn"];
            if (obj != null && obj != DBNull.Value)
            {
                model.SendOn = (DateTime)obj;
            }

            obj = dataReader["DueOn"];
            if (obj != null && obj != DBNull.Value)
            {
                model.DueOn = (DateTime)obj;
            }

            obj = dataReader["ReceiveOn"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ReceiveOn = ((DateTime)obj);
            }

            model.Notes = dataReader["Notes"].ToString();

            obj = dataReader["Color"];
            if (obj != null && obj != DBNull.Value)
            {
                model.Color = obj.ToString();
            }

            obj = dataReader["ColorFor"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ColorFor = obj.ToString();
            }

            obj = dataReader["CreatedOn"];
            if (obj != null && obj != DBNull.Value)
            {
                model.CreatedOn = ((DateTime)obj);
            }

            obj = dataReader["CreatedBy"];
            if (obj != null && obj != DBNull.Value)
            {
                model.CreatedBy = (int)obj;
            }

            obj = dataReader["ModifiedOn"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ModifiedOn = ((DateTime)obj);
            }

            obj = dataReader["ModifiedBy"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ModifiedBy = (int)obj;
            }

            obj = dataReader["ETADate"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ETADate = (DateTime) obj;
            }
            return model;
        }
        #region Model

        public int ProposalId { get; set; }

        public string Milestone { get; set; }

        public bool Approved { get; set; }

        public InvoiceStatus Status { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime? SendOn { get; set; }

        public DateTime? DueOn { get; set; }

        public DateTime? ReceiveOn { get; set; }

        public string Notes { get; set; }

        public string Color { get; set; }

        public string ColorFor { get; set; }

        public DateTime? ETADate { get; set; }

        public string StatusText
        {
            get { return Status.ToText(); }
        }

        #endregion Model
    }
}
