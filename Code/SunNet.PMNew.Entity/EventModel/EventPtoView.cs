using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.EventModel
{
    public class EventPtoView
    {
        public string Title { get; set; }

        public int ProjectID
        {
            get; set;
        }
        public int CreatedBy
        {
            get; set;
        }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FromTime { get; set; }

        public bool AllDay { get; set; }
        public DateTime FromDay
        {
            get; set;
        }
        public int FromTimeType
        {
            get; set;
        }
        public string ToTime { get; set; }
        public DateTime ToDay
        {
            get; set;
        }
        public int ToTimeType
        {
            get; set;
        }

        public int UserID { get; set; }
        public string Office { get; set; }
        public double PTOHoursOfYear { get; set; }
    }
}
