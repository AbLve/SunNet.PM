using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

using SunNet.PMNew.Framework.Core;
namespace SunNet.PMNew.Core.TicketModule
{
    //FeedBacks
    public class FeedBacksEntity:BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static FeedBacksEntity ReaderBind(IDataReader dataReader)
        {
            FeedBacksEntity model = new FeedBacksEntity();
            object ojb;
            ojb = dataReader["FeedBackID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FeedBackID = (int)ojb;
            }
            ojb = dataReader["TicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketID = (int)ojb;
            }
            model.Title = dataReader["Title"].ToString();
            model.Description = dataReader["Description"].ToString();
            ojb = dataReader["CreatedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedBy = (int)ojb;
            }
            model.CreateUserName = dataReader["CreateUserName"].ToString();
            ojb = dataReader["CreatedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedOn = (DateTime)ojb;
            }
            ojb = dataReader["IsDelete"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsDelete = (bool)ojb;
            }
            ojb = dataReader["IsReview"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsReview = (bool)ojb;
            }
            ojb = dataReader["ReviewFeedBackID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ReviewFeedBackID = (int)ojb;
            }
            ojb = dataReader["IsPublic"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsPublic = (bool)ojb;
            }
            ojb = dataReader["IsClientResponse"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsClientResponse = (bool)ojb;
            }
            return model;
        }
        /// <summary>
        /// FeedBackID
        /// </summary>		
        public int FeedBackID { get; set; }
        /// <summary>
        /// TicketID
        /// </summary>		
        public int TicketID { get; set; }
        /// <summary>
        /// Title
        /// </summary>		
        public string Title { get; set; }
        /// <summary>
        /// Description
        /// </summary>		
        public string Description { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>		
        public int CreatedBy { get; set; }
        /// <summary>
        /// CreateUserName
        /// </summary>		
        public string CreateUserName { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>		
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// IsDelete
        /// </summary>		
        public bool IsDelete { get; set; }
        /// <summary>
        /// IsReview
        /// </summary>		
        public bool IsReview { get; set; }
        /// <summary>
        /// ReviewFeedBackID
        /// </summary>		
        public int ReviewFeedBackID { get; set; }
        /// <summary>
        /// IsPublic
        /// </summary>		
        public bool IsPublic { get; set; }
        /// <summary>
        /// IsClientResponse
        /// </summary>		
        public bool IsClientResponse { get; set; }

    }
}