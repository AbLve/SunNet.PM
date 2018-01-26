using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.SchedulesModel
{
    public class ScheduleTimeHelpers
    {

        protected const int WorkHour = 8;

        protected List<ScheduleTimeEntity> _time;
        public List<ScheduleTimeEntity> Times
        {
            get { return _time; }
        }

        public ScheduleTimeHelpers(List<SchedulesEntity> list)
        {
            _time = new List<ScheduleTimeEntity>();
            for (int i = WorkHour * 2; i < 48; i++)
                _time.Add(new ScheduleTimeEntity(i));

            ScheduleTimeEntity t;
            foreach (SchedulesEntity info in list)
            {
                int start = TimeHandle(info.StartTime);
                int end = TimeHandle(info.EndTime);
                for (int i = start; i < end; i++)
                {
                    t = _time.Find(r => r.Cell == i);
                    t.IsPlan = true;
                    t.ScheduleID = info.ID;
                }
            }
        }

        public static int TimeHandle(string time)
        {
            int sign;
            string[] s = time.Split(':');
            int hour = int.Parse(s[0]);
            sign = hour * 2;
            if (s[1] == "30")
                sign++;
            return sign;
        }

        public static List<DayEntity> GetDayList(DateTime date, string addurl)
        {
            List<DayEntity> list = new List<DayEntity>();
            DateTime head = DateTime.Parse(string.Format("{0}/01/{1}", date.Month, date.Year));
            DateTime start = head;
            if (head.DayOfWeek != DayOfWeek.Sunday) //1号不是星期天
            {
                start = head.AddDays(0 - (int)head.DayOfWeek);
            }

            DateTime monthEnd = head.AddMonths(1).AddDays(-1);
            DateTime end = monthEnd;
            if (monthEnd.DayOfWeek != DayOfWeek.Saturday) //月末不是星期六
            {
                end = monthEnd.AddDays(6 - (int)monthEnd.DayOfWeek);
            }

            while (start <= end)
            {
                if (start == DateTime.Now.Date) //高亮，并且可以添加
                {
                    if (start < head) //上个月的日子，灰色不可添加
                    {
                        list.Add(new DayEntity(start, "dateDisable", start.Day.ToString()));
                    }
                    else
                    {
                        list.Add(new DayEntity(start, "dateToday", string.Format(addurl, start.Day, start.ToString("MM/dd/yyyy"))));
                    }
                }
                else if (start > DateTime.Now.Date)
                {
                    if (start > monthEnd) //下个月的日子，灰色可添加
                    {
                        list.Add(new DayEntity(start, "dateDisable", string.Format(addurl, start.Day, start.ToString("MM/dd/yyyy"))));
                    }
                    else if (start < head) //上个月的日子，灰色不可添加
                    {
                        list.Add(new DayEntity(start, "dateDisable", start.Day.ToString()));
                    }
                    else //本月将来的日子，可添加
                        list.Add(new DayEntity(start, "", string.Format(addurl, start.Day, start.ToString("MM/dd/yyyy"))));
                }
                else
                {
                    if (start < head) //上个月的日子，灰色不可添加
                        list.Add(new DayEntity(start, "dateDisable", start.Day.ToString()));
                    else //本月已过去的日子，不可添加
                        list.Add(new DayEntity(start, "", start.Day.ToString()));
                }
                start = start.AddDays(1);
            }
            return list;
        }
    }

    [Serializable]
    public class ScheduleTimeEntity
    {
        public ScheduleTimeEntity(int cell)
        {
            Cell = cell;
            IsPlan = false;
        }
        public int Cell { get; set; }

        public bool IsPlan { get; set; }

        public int ScheduleID { get; set; }
    }

    public class DayEntity
    {
        public DayEntity(DateTime day, string css, string addUrl)
        {
            Day = day;
            Css = css;
            AddUrl = addUrl;
        }

        public DateTime Day { get; set; }

        public string Css { get; set; }

        public string AddUrl { get; set; }
    }
}
