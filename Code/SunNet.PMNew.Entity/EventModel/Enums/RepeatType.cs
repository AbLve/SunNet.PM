using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.EventModel
{
    public enum RepeatType
    {
        None = 1,
        Every_Day = 2,
        Every_Week = 3,
        Every_two_weeks = 4,
        Every_Month = 5,
        Every_Month_First_Friday = 7,
        Every_Year = 6,
    }
}
