using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.EventModel
{
    public enum AlertType
    {
        None = 1,
        five_minutes_before = 2,
        fifteen_minutes_before = 3,
        thirty_minutes_before = 4,
        one_hour_before = 5,
        two_hours_before = 6,
        one_day_before = 7,
        two_days_before = 8,
        On_date_of = 9
    }
}
