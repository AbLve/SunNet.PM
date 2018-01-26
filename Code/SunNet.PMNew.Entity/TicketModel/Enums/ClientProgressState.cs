using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.TicketModel
{
    public enum ClientProgressState
    {
        None = -1,
        Draft = 0,
        Submit,
        PM_Review,
        Developing,
        Testing,
        Client_Confirm,
        Completed
    }
}
