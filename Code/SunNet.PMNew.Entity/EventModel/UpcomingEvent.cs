using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.EventModel
{
    public class UpcomingEvent
    {
        public string ID { get { return Day.ToString("MMddyyyy"); } }

        public DateTime Day { get; set; }

        public string Date { get; set; }

        public string NewDate { get { return Day.ToString("MM/dd/yyyy"); } }

        public List<ListView> list { get; set; }

        public string MoreDate { get; set; }

       
    }

    public class ListView
    {
        public int ID { get; set; }
        public string Time { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int CreatedBy { get; set; }
        public bool Invited { get; set; }
        public int InviteStatus { get; set; }
        public string LittleHeadImage { get; set; }
        public string FullName { get; set; }
    }
}
