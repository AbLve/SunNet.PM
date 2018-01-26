using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.EventModel
{
    public class WorkTimeView
    {
        public int Priority { get; set; }
        public string FromTime { get; set; }
        public int FromTimeType { get; set; }
        public string ToTime { get; set; }
        public int ToTimeType { get; set; }

        public DateTime FromDate
        {
            get
            {
                var dtFormat = new DateTimeFormatInfo {ShortDatePattern = "HH:mm"};
                var fromTime = FromTime.Split(':');
                switch (FromTimeType)
                {
                    case 1:
                        return Convert.ToDateTime(FromTime, dtFormat);
                    case 2:
                        if (Int32.Parse(fromTime[0]) == 12)
                        {
                            return Convert.ToDateTime(FromTime, dtFormat);
                        }
                        return Convert.ToDateTime(FromTime, dtFormat).AddHours(12);
                }
                return Convert.ToDateTime("8:30", dtFormat);
            }
        }

        public DateTime ToDate
        {
            get
            {
                var dtFormat = new DateTimeFormatInfo {ShortDatePattern = "HH:mm"};
                var toTime = ToTime.Split(':');
                switch (ToTimeType)
                {
                    case 1:
                        return Convert.ToDateTime(ToTime, dtFormat);
                    case 2:
                        if (Int32.Parse(toTime[0])==12)
                        {
                            return Convert.ToDateTime(ToTime, dtFormat);
                        }
                        return Convert.ToDateTime(ToTime, dtFormat).AddHours(12);
                }
                return Convert.ToDateTime("5:30", dtFormat).AddHours(12);
            }
        }
    }
}
