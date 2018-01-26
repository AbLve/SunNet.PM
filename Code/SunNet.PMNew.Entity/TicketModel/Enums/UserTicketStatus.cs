using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

/**************************************************************************
 * Developer: 		JackZhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		2014/7/23 1:59:45
 * Description:		Please input class summary
 * Version History:	Created,2014/7/23 1:59:45
 * 
 * 
 **************************************************************************/

namespace SunNet.PMNew.Entity.TicketModel.Enums
{
    /// <summary>
    /// Ticket针对每个用户显示不同状态
    /// </summary>
    public enum UserTicketStatus
    {
        /// <summary>
        /// 正常,使用Ticket的状态
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 等待Sunnet回复
        /// </summary>
        [Description("Waiting for SunNet's feedback")]
        WaitSunnetFeedback = 32,

        /// <summary>
        /// 等待Client回复
        /// </summary>
        [Description("Waiting for Client's feedback")]
        WaitClientFeedback = 31
    }
}
