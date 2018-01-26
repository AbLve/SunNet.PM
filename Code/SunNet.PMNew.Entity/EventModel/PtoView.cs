using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.EventModel
{
    public class PtoView
    {
        public string Title { get; set; }

        public int ProjectID { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Hours { get; set; }
        public double PTOHoursOfYear { get; set; }
        public double Remaining
        {
            get
            {
                return PTOHoursOfYear - Hours;
            }
            private set { }
        }
    }

    public class PtoUserView
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public string FromTime { get; set; }
        public DateTime FromDay { get; set; }
        public int FromTimeType { get; set; }
        public string ToTime { get; set; }
        public DateTime ToDay { get; set; }
        public int ToTimeType { get; set; }
        public double Hours { get; set; }
        public bool AllDay { get; set; }
        public string Office { get; set; }
        public int UserID { get; set; }
    }

    public class PtoUserDetailView
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public string Name { get; set; }
        public DateTime FromDay { get; set; }
        public DateTime ToDay { get; set; }
        public double Hours { get; set; }
    }
}
