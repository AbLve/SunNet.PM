using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Entity.ProposalTrackerModel.Enums;

namespace SunNet.PMNew.Entity.ProposalTrackerModel
{
    public class ProposalTrackerEntity
    {

        public static ProposalTrackerEntity ReaderBind(IDataReader dataReader)
        {
            ProposalTrackerEntity model = new ProposalTrackerEntity();
            object ojb;

            ojb = dataReader["ProposalTrackerID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProposalTrackerID = (int)ojb;
            }
            ojb = dataReader["ProjectID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProjectID = (int)ojb;
            }

            ojb = dataReader["Status"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Status = (int)ojb;
            }

            ojb = dataReader["Payment"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Payment = (int)ojb;
            }
            ojb = dataReader["DueDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DueDate = (DateTime)ojb;
            }

            ojb = dataReader["CreatedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedOn = (DateTime)ojb;
            }
            ojb = dataReader["CreatedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedBy = (int)ojb;
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
            model.RequestNo = dataReader["RequestNo"].ToString();
            model.ProjectName = dataReader["ProjectName"].ToString();
            model.InvoiceNo = dataReader["InvoiceNo"].ToString();
            model.Title = dataReader["Title"].ToString();
            model.Description = dataReader["Description"].ToString();
            ojb = dataReader["WorkScope"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.WorkScope = dataReader["WorkScope"].ToString();
            }
            ojb = dataReader["WorkScopeDisplayName"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.WorkScopeDisplayName = dataReader["WorkScopeDisplayName"].ToString();
            }

            model.ProposalSentTo = dataReader["ProposalSentTo"].ToString();
            ojb = dataReader["ProposalSentOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProposalSentOn = (DateTime)ojb;
            }
            model.PONo = dataReader["PONo"].ToString();
            model.ApprovedBy = dataReader["ApprovedBy"].ToString();
            ojb = dataReader["ApprovedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ApprovedOn = (DateTime)ojb;
            }
            ojb = dataReader["InvoiceSentOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.InvoiceSentOn = (DateTime)ojb;
            }

            ojb = dataReader["Reminded"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Reminded = (int)ojb;
            }

            ojb = dataReader["RemindTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RemindTime = (DateTime)ojb;
            }

            try
            {
                model.CompanyName = dataReader["CompanyName"].ToString();
                ojb = dataReader["PoTotalLessThenProposalTotal"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.PoTotalLessThenProposalTotal = (bool)ojb;
                } 
                ojb = dataReader["CompanyID"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.CompanyID = (int)ojb;
                }
            }
            catch (Exception)
            {
            }
            return model;
        }

        public int ProposalTrackerID { get; set; }

        public string RequestNo { get; set; }
        [Required]
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        public int Status { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        [Required]

        public string ProposalSentTo { get; set; }

        public DateTime? ProposalSentOn { get; set; }

        public string PONo { get; set; }

        public string ApprovedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime? InvoiceSentOn { get; set; }

        public string WorkScope { get; set; }
        public string WorkScopeDisplayName { get; set; }

        [Required]
        public int Payment { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifyOn { get; set; }
        public int ModifyBy { get; set; }

        /* 0未提醒，
         * 1第一次提醒填写Invoice后,
         * 2第二次提醒填写Invoice后,
         * 3第一次提醒填写SendDate后,
         * 4第二次提醒填写SendDate后,
         * 5提醒ReceiveDate后*/
        public int Reminded { get; set; }

        public DateTime? RemindTime { get; set; }

        public bool? PoTotalLessThenProposalTotal { get; set; }


    }
}
