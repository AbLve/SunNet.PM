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
    /// Ticket���ÿ���û���ʾ��ͬ״̬
    /// </summary>
    public enum UserTicketStatus
    {
        /// <summary>
        /// ����,ʹ��Ticket��״̬
        /// </summary>
        Normal = 0,

        /// <summary>
        /// �ȴ�Sunnet�ظ�
        /// </summary>
        [Description("Waiting for SunNet's feedback")]
        WaitSunnetFeedback = 32,

        /// <summary>
        /// �ȴ�Client�ظ�
        /// </summary>
        [Description("Waiting for Client's feedback")]
        WaitClientFeedback = 31
    }
}
