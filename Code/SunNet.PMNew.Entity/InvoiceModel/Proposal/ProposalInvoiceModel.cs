using SunNet.PMNew.Entity.InvoiceModel.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.InvoiceModel
{
    public class ProposalInvoiceModel : InvoiceEntity
    {
        public static new ProposalInvoiceModel ReaderBind(IDataReader dataReader)
        {
            ProposalInvoiceModel model = new ProposalInvoiceModel();

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

            model.Status = (InvoiceStatus)Enum.Parse(typeof(InvoiceStatus), dataReader["Status"].ToString().Trim());

            model.InvoiceNo = dataReader["InvoiceNo"].ToString();

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
                model.ReceiveOn = (DateTime)obj;
            }

            model.Notes = dataReader["Notes"].ToString();

            try
            {
                obj = dataReader["CompanyId"];
                if (obj != null && obj != DBNull.Value)
                {
                    model.CompanyId = (int)obj;
                }
            }
            catch (Exception)
            {
            }
            try
            {
                model.CompanyName = dataReader["CompanyName"].ToString();
            }
            catch (Exception)
            {
            }

            try
            {
                obj = dataReader["ProjectId"];
                if (obj != null && obj != DBNull.Value)
                {
                    model.ProjectId = (int)obj;
                }
            }
            catch (Exception)
            {
            }

            obj = dataReader["HOURS"];
            if (obj != null && obj != DBNull.Value)
            {
                model.HOURS = (decimal)obj;
            }

            try
            {
                model.ProposalTitle = dataReader["ProposalTitle"].ToString();
            }
            catch (Exception)
            {
            }
            try
            {
                model.ProjectTitle = dataReader["ProjectTitle"].ToString();
                model.PONo = dataReader["PONo"].ToString();
            }
            catch (Exception)
            {
            }
            try
            {
                obj = dataReader["TimeSheetID"];
                if (obj != null && obj != DBNull.Value)
                {
                    model.TimeSheetID = (int)obj;
                }
            }
            catch (Exception)
            {
            }
            return model;
        }
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public int ProjectId { get; set; }

        public string ProjectTitle { get; set; }

        public string ProposalTitle { get; set; }

        public string PONo { get; set; }

        public decimal HOURS { get; set; }

        public int TimeSheetID { get; set; }
    }

    public class ProposalToDoModel
    {
        public static new ProposalToDoModel ReaderBind(IDataReader dataReader)
        {
            ProposalToDoModel model = new ProposalToDoModel();

            object obj = new object();

            obj = dataReader["ProjectTitle"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ProjectTitle = obj.ToString();
            }
            obj = dataReader["ProposalTrackerTitle"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ProposalTrackerTitle = obj.ToString();
            }

            obj = dataReader["PONo"];
            if (obj != null && obj != DBNull.Value)
            {
                model.PONo = obj.ToString();
            }
            obj = dataReader["Milestone"];
            if (obj != null && obj != DBNull.Value)
            {
                model.Milestone = obj.ToString();
            }
            else
            {
                model.Milestone = "";
            }
            obj = dataReader["InvoiceNo"];
            if (obj != null && obj != DBNull.Value)
            {
                model.InvoiceNo = obj.ToString();
            }
            else
            {
                model.InvoiceNo = "";
            }
            obj = dataReader["Milestone"];
            if (obj != null && obj != DBNull.Value)
            {
                model.Milestone = obj.ToString();
            }
            else
            {
                model.Milestone = "";
            }
            obj = dataReader["invoiceId"];
            if (obj != null && obj != DBNull.Value)
            {
                model.InvoiceId = (int)obj;
            }
            else
            {
                model.InvoiceId = 0;
            }

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
            obj = dataReader["ProposalTrackerID"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ProposalTrackerID = (int)obj;
            }
            else
            {
                model.ProposalTrackerID = 0;
            }
            obj = dataReader["ProjectId"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ProjectId = (int)obj;
            }
            else
            {
                model.ProjectId = 0;
            }
            return model;
        }

        public int ProposalTrackerID { get; set; }
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public string ProposalTrackerTitle { get; set; }
        public string PONo { get; set; }

        public string Milestone { get; set; }

        public string InvoiceNo { get; set; }

        public int InvoiceId { get; set; }


        public InvoiceStatus Status { get; set; }

    }
}
