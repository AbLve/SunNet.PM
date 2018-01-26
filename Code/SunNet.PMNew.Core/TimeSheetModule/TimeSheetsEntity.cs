using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace SunNet.PMNew.Core.TimeSheetModule
{
    //TimeSheets
    public class TimeSheetsEntity : SunNet.PMNew.Framework.Core.BaseEntity
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
            return model;
        }

        /// <summary>
        /// ID
        /// </summary>		
        public int ID { get; set; }
        /// <summary>
        /// SheetDate
        /// </summary>		
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
        public decimal Hours { get; set; }
        /// <summary>
        /// Percentage
        /// </summary>		
        public decimal Percentage { get; set; }
        /// <summary>
        /// Description
        /// </summary>		
        public string Description { get; set; }
        /// <summary>
        /// IsSubmitted
        /// </summary>		
        public bool IsSubmitted { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>		
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>		
        public int CreatedBy { get; set; }
        /// <summary>
        /// ModifiedOn
        /// </summary>		
        public DateTime ModifiedOn { get; set; }
        /// <summary>
        /// ModifiedBy
        /// </summary>		
        public int ModifiedBy { get; set; }
        /// <summary>
        /// IsMeeting
        /// </summary>		
        public bool IsMeeting { get; set; }

    }
}