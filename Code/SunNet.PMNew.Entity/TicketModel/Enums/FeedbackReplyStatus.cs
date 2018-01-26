using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**************************************************************************
 * Developer: 		JackZhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		2014/7/27 20:24:43
 * Description:		Please input class summary
 * Version History:	Created,2014/7/27 20:24:43
 * 
 * 
 **************************************************************************/
namespace SunNet.PMNew.Entity.TicketModel.Enums
{
    public enum FeedbackReplyStatus
    {
        /// <summary>
        /// 不需要回复
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 需要回复
        /// </summary>
        Requested = 1,
        /// <summary>
        /// 已回复
        /// </summary>
        Replied = 2
    }
}
