using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

using SunNet.PMNew.Framework.Core;
using System.ComponentModel.DataAnnotations;
namespace SunNet.PMNew.Core.TicketModule
{
    //Tickets
    public class TicketsEntity : BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static TicketsEntity ReaderBind(IDataReader dataReader)
        {
            TicketsEntity model = new TicketsEntity();
            object ojb;
            ojb = dataReader["TicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
                model.TicketID = (int)ojb;
            }
            ojb = dataReader["CompanyID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CompanyID = (int)ojb;
            }
            ojb = dataReader["ProjectID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProjectID = (int)ojb;
            }
            model.Title = dataReader["Title"].ToString();
            model.TicketType = dataReader["TicketType"].ToString();
            model.Description = dataReader["Description"].ToString();
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
            model.CreateUserName = dataReader["CreateUserName"].ToString();
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
            ojb = dataReader["PublishDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PublishDate = (DateTime)ojb;
            }
            ojb = dataReader["ClientPublished"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ClientPublished = (bool)ojb;
            }
            ojb = dataReader["StartDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.StartDate = (DateTime)ojb;
            }
            ojb = dataReader["DeliveryDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DeliveryDate = (DateTime)ojb;
            }
            ojb = dataReader["ContinueDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ContinueDate = (int)ojb;
            }
            model.URL = dataReader["URL"].ToString();
            ojb = dataReader["Priority"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Priority = (int)ojb;
            }
            ojb = dataReader["Status"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Status = (int)ojb;
            }
            ojb = dataReader["ConvertDelete"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ConvertDelete = (int)ojb;
            }
            ojb = dataReader["IsInternal"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsInternal = (bool)ojb;
            }
            ojb = dataReader["CreateType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreateType = (int)ojb;
            }
            ojb = dataReader["SourceTicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SourceTicketID = (int)ojb;
            }
            ojb = dataReader["IsEstimates"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsEstimates = (bool)ojb;
            }
            ojb = dataReader["DevTsHours"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DevTsHours = (int)ojb;
            }
            ojb = dataReader["QaTsHours"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.QaTsHours = (int)ojb;
            }
            ojb = dataReader["Hours"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Hours = (int)ojb;
            }
            return model;
        }
        /// <summary>
        /// TicketID
        /// </summary>		
        public int TicketID { get; set; }
        /// <summary>
        /// CompanyID
        /// </summary>		
        public int CompanyID { get; set; }
        /// <summary>
        /// ProjectID
        /// </summary>		
        public int ProjectID { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        [Required]
        [StringLength(100)]
        [RegularExpression("[A-Z][a-z][0-9]")]
        public string Title { get; set; }
        /// <summary>
        /// TicketType
        /// </summary>	
        [Required]
        [StringLength(20)]
        public string TicketType { get; set; }
        /// <summary>
        /// Description
        /// </summary>		
        [Required]
        [StringLength(5000)]
        public string Description { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>		
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// CreateUserName
        /// </summary>		
        public string CreateUserName { get; set; }
        /// <summary>
        /// ModifiedOn
        /// </summary>	
        [Required]
        [DataType(DataType.Date)]
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// PublishDate
        /// </summary>		
        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }
        /// <summary>
        /// ClientPublished
        /// </summary>		
        public bool ClientPublished { get; set; }
        /// <summary>
        /// StartDate
        /// </summary>		
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// DeliveryDate
        /// </summary>		
        [Required]
        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; }
        /// <summary>
        /// ContinueDate
        /// </summary>		
        public int ContinueDate { get; set; }
        /// <summary>
        /// URL
        /// </summary>		
        public string URL { get; set; }
        /// <summary>
        /// Priority
        /// </summary>		
        public int Priority { get; set; }
        /// <summary>
        /// Status
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// ConvertDelete,Normal:0,IsForverDelete:1,IsHistory:2 
        /// </summary>		
        public int ConvertDelete { get; set; }
        /// <summary>
        /// IsInternal
        /// </summary>		
        public bool IsInternal { get; set; }
        /// <summary>
        /// CreateType
        /// </summary>		
        public int CreateType { get; set; }
        /// <summary>
        /// SourceTicketID
        /// </summary>		
        public int SourceTicketID { get; set; }
        /// <summary>
        /// IsEstimates
        /// </summary>		
        public bool IsEstimates { get; set; }
        /// <summary>
        /// DevTsHours
        /// </summary>		
        public int DevTsHours { get; set; }
        /// <summary>
        /// QaTsHours
        /// </summary>		
        public int QaTsHours { get; set; }
        /// <summary>
        /// Hours
        /// </summary>		
        [Required]
        public int Hours { get; set; }

    }
}