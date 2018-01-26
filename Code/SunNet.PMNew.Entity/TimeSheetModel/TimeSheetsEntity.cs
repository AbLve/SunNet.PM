using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using SunNet.PMNew.Framework.Core;
using System.ComponentModel.DataAnnotations;

namespace SunNet.PMNew.Entity.TimeSheetModel
{
    //TimeSheets
    public class TimeSheetsEntity : BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static TimeSheetsEntity ReaderBind(IDataReader dataReader)
        {
            TimeSheetsEntity model = new TimeSheetsEntity();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            ojb = dataReader["SheetDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SheetDate = (DateTime)ojb;
            }
            ojb = dataReader["ProjectID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProjectID = (int)ojb;
            }
            ojb = dataReader["TicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketID = (int)ojb;
            }
            ojb = dataReader["UserID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UserID = (int)ojb;
            }
            ojb = dataReader["Hours"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Hours = (decimal)ojb;
            }
            ojb = dataReader["Percentage"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Percentage = (decimal)ojb;
            }
            model.Description = dataReader["Description"].ToString();
            ojb = dataReader["IsSubmitted"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsSubmitted = (bool)ojb;
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
            ojb = dataReader["ModifiedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifiedOn = (DateTime)ojb;
            }
            ojb = dataReader["ModifiedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifiedBy = (int)ojb;
            }
            ojb = dataReader["IsMeeting"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsMeeting = (bool)ojb;
            }
            ojb = dataReader["EventID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EventID = (int)ojb;
            }
            return model;
        }

        /// <summary>
        /// SheetDate
        /// </summary>		
        [Required]
        [Range(typeof(DateTime), "2012-10-1", "2112-1-1")]
        public DateTime SheetDate { get; set; }
        /// <summary>
        /// ProjectID
        /// </summary>		
        public int ProjectID { get; set; }
        /// <summary>
        /// TicketID
        /// </summary>		
        public int TicketID { get; set; }
        /// <summary>
        /// UserID
        /// </summary>		
        public int UserID { get; set; }
        /// <summary>
        /// Hours
        /// </summary>		
        [Required(ErrorMessage = "Hours can not be null")]
        [Range(0, 24, ErrorMessage = "Hours must between 0.1 to 24")]
        public decimal Hours { get; set; }
        /// <summary>
        /// Percentage
        /// </summary>		
        [Required(ErrorMessage = "PCT can not be null")]
        [Range(0, 100, ErrorMessage = "Percentage must between 1 to 100")]
        public decimal Percentage { get; set; }
        /// <summary>
        /// Description
        /// </summary>		
        [Required(ErrorMessage = "WorkDetail can not be null")]
        [StringLength(4000, ErrorMessage = "WorkDetail length can not bingger than 4000 characters")]
        public string Description { get; set; }
        /// <summary>
        /// IsSubmitted
        /// </summary>		
        [Required]
        public bool IsSubmitted { get; set; }
        /// <summary>
        /// IsMeeting
        /// </summary>		
        [Required]
        public bool IsMeeting { get; set; }

        public int EventID { get; set; }

    }
}