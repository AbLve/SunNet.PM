using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Entity.TicketModel
{
    //FeedBacks
    public class FeedBacksEntity : BaseEntity
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
                model.ID = model.FeedBackID;
            }
            ojb = dataReader["TicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketID = (int)ojb;
            }
            model.Title = dataReader["Title"].ToString();
            model.Description = dataReader["Description"].ToString();
            model.Description = UtilFactory.Helpers.HtmlHelper.ReplaceUrl(model.Description);
            ojb = dataReader["CreatedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedBy = (int)ojb;
            }
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
            ojb = dataReader["IsPublic"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsPublic = (bool)ojb;
            }
            ojb = dataReader["WaitClientFeedback"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.WaitClientFeedback = (FeedbackReplyStatus)ojb;
            }
            ojb = dataReader["WaitPMFeedback"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.WaitPMFeedback = (FeedbackReplyStatus)ojb;
            }

            ojb = dataReader["Order"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Order = (int)ojb;
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
        /// CreatedOn
        /// </summary>		
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// IsDelete
        /// </summary>		
        public bool IsDelete { get; set; }
        /// <summary>
        /// IsPublic
        /// </summary>		
        public bool IsPublic { get; set; }

        /// <summary>
        /// 0:normal ; 1:request ; 2:have replied to 
        /// </summary>
        public FeedbackReplyStatus WaitClientFeedback { get; set; }

        /// <summary>
        /// 0:normal ; 1:request ; 2:have replied to 
        /// </summary>
        public FeedbackReplyStatus WaitPMFeedback { get; set; }

        public int Order { get; set; }

    }
}