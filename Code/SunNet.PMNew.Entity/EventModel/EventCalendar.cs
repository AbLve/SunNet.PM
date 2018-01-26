using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.EventModel
{
    public class EventCalendar
    {
        public EventCalendar()
        {
        }

        public int Day { get { return Date.Day; } }
        public DateTime Date { get; set; }
        public string StrDate { get { return Date.ToString("MM/dd/yyyy"); } }
        public string Month { get { return Date.ToString("MMMM_yyyy"); } }

        /// <summary>
        /// 0:灰色；1：不可添加；2：可添加；3：当天; 4:灰色可添加
        /// </summary>
        public int Type { get; set; }

        public List<CalendarView> list;

        /// <summary>
        /// 显示More链接
        /// </summary>
        public bool MoreEvent { get; set; }

    
    }
}
