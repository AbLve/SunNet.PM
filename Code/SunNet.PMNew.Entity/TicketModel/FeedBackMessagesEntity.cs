using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.Entity.TicketModel
{
    public class FeedBackMessagesEntity : BaseEntity
    {
        public FeedBackMessagesEntity() { }

        public int TicketID { get; set; }

        public int UserID { get; set; }

        /// <summary>
        ///Request Feedback Id
        /// </summary>
        public int WaitClientFeedback { get; set; }

        /// <summary>
        ///Request Feedback Id
        /// </summary>
        public int WaitPMFeedback { get; set; }

        public static FeedBackMessagesEntity ReaderBind(IDataReader dataReader)
        {
            FeedBackMessagesEntity model = new FeedBackMessagesEntity();
            object ojb;
            ojb = dataReader["TicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketID = (int)ojb;
            }
            ojb = dataReader["UserID"];
            if (ojb != null && ojb != DBNull.Value)
                model.UserID = (int)ojb;
            ojb = dataReader["WaitClientFeedback"];
            if (ojb != null && ojb != DBNull.Value)
                model.WaitClientFeedback = (int)ojb;
            ojb = dataReader["WaitPMFeedback"];
            if (ojb != null && ojb != DBNull.Value)
                model.WaitPMFeedback = (int)ojb;
            return model;
        }
    }
}
