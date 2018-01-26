using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.Entity.TicketModel
{
    //TicketUsers
    public class TicketUsersEntity : BaseEntity
    {
        public TicketUsersEntity()
        {
            WorkingOnStatus = TicketUserStatus.WorkingOn;
        }
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static TicketUsersEntity ReaderBind(IDataReader dataReader)
        {
            TicketUsersEntity model = new TicketUsersEntity();
            object ojb;
            ojb = dataReader["TUID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TUID = (int)ojb;
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
            ojb = dataReader["Type"];
            if (ojb != null && ojb != DBNull.Value)
                model.Type = (TicketUsersType)(int)ojb;

            ojb = dataReader["Status"];
            if (ojb != null && ojb != DBNull.Value)
                model.WorkingOnStatus = (TicketUserStatus)(int)ojb;

            ojb = dataReader["ShowNotification"];
            if (ojb != null && ojb != DBNull.Value)
                model.ShowNotification = (bool)ojb;

            ojb = dataReader["TicketStatus"];
            if (ojb != null && ojb != DBNull.Value)
                model.TicketStatus = (UserTicketStatus)(int)ojb;
            return model;
        }
        /// <summary>
        /// TUID
        /// </summary>		
        public int TUID { get; set; }
        /// <summary>
        /// TicketID
        /// </summary>		
        public int TicketID { get; set; }
        /// <summary>
        /// UserID
        /// </summary>		
        public int UserID { get; set; }

        public TicketUsersType Type { get; set; }

        public TicketUserStatus WorkingOnStatus { get; set; }

        /// <summary>
        /// 是否需要显示气泡提示
        /// </summary>
        public bool ShowNotification { get; set; }

        /// <summary>
        /// 每个用户显示Ticket不一定是Ticket本来的状态
        /// </summary>
        public UserTicketStatus TicketStatus { get; set; }
    }
}