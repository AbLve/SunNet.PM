using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using FamilyBook.Entity;

namespace FamilyBook.Entity.DocManagements
{
    //Files
    public class FilesEntity : BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static FilesEntity ReaderBind(IDataReader dataReader)
        {
            FilesEntity model = new FilesEntity();
            object ojb;
            ojb = dataReader["FileID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FileID = (int)ojb;
            }
            ojb = dataReader["CompanyID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CompanyID = (int)ojb;
            }
            ojb = dataReader["ProjectId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProjectId = (int)ojb;
            }
            ojb = dataReader["TicketId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketId = (int)ojb;
            }
            ojb = dataReader["FeedbackId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FeedbackId = (int)ojb;
            }
            ojb = dataReader["SourceType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SourceType = (int)ojb;
            }
            model.FileTitle = dataReader["FileTitle"].ToString();
            model.ContentType = dataReader["ContentType"].ToString();
            ojb = dataReader["FileSize"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FileSize = (decimal)ojb;
            }
            model.FilePath = dataReader["FilePath"].ToString();
            model.ThumbPath = dataReader["ThumbPath"].ToString();
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
            ojb = dataReader["IsDelete"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsDelete = (bool)ojb;
            }
            ojb = dataReader["IsPublic"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsPublic = (bool)ojb;
            }
            model.Tags = dataReader["Tags"].ToString();
            ojb = dataReader["ProposalTrackerId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProposalTrackerId = (int)ojb;
            }
            return model;
        }
        /// <summary>
        /// FileID
        /// </summary>		
        public int FileID { get; set; }

        public int CompanyID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TicketId { get; set; }
        /// <summary>
        /// SourceID
        /// </summary>		
        public int FeedbackId { get; set; }
        /// <summary>
        /// SourceType
        /// </summary>		
        public int SourceType { get; set; }
        /// <summary>
        /// FileTitle
        /// </summary>		
        public string FileTitle { get; set; }
        /// <summary>
        /// ContentType
        /// </summary>		
        public string ContentType { get; set; }
        /// <summary>
        /// FileSize
        /// </summary>		
        public decimal FileSize { get; set; }
        /// <summary>
        /// FilePath
        /// </summary>		
        public string FilePath { get; set; }
        /// <summary>
        /// ThumbPath
        /// </summary>		
        public string ThumbPath { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>		
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>		
        public int CreatedBy { get; set; }
        /// <summary>
        /// IsDelete
        /// </summary>		
        public bool IsDelete { get; set; }
        /// <summary>
        /// IsPublic
        /// </summary>		
        public bool IsPublic { get; set; }

        public string Tags { get; set; }

        public int ProposalTrackerId { get; set; }

    }
}