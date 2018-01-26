using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		5/27 20:06:04
 * Description:		�û����ض�Ticket�Ĺ���״̬����
 * Version History:	Created,5/27 20:06:04
 * 
 * 
 **************************************************************************/
namespace SunNet.PMNew.Entity.TicketModel.Enums
{
    /// <summary>
    /// WorkingOn Status
    /// </summary>
    public enum TicketUserStatus
    {
        None = 0,
        WorkingOn = 1,
        Completed = 2,
        Canceled = 3
    }
}
