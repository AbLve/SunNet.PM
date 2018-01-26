using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.TicketModel
{
    public enum CovertDeleteState
    {
        Normal = 1,
        ConvertToHistory = 2,
        NotABug = 3,
        ForeverDelete = 4
    }
}
