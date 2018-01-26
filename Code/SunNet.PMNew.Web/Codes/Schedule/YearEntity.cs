using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.Web.Codes.Schedule
{
    public class YearEntity : BaseSchedule
    {
        public int TicketsCount
        {
            get
            {
                int count = 0;
                foreach (MonthEntity month in Months)
                {
                    count += month.TicketsCount;
                }
                return count;
            }
        }

        private List<MonthEntity> months;
        public List<MonthEntity> Months
        {
            get
            {
                if (months == null)
                {
                    List<MonthEntity> list = new List<MonthEntity>();
                    int m = 12;
                    //int m = DateTime.Now.Month;
                    //if (Year != DateTime.Now.Year)
                    //{
                    //    m = 12;
                    //}
                    for (int i = 1; i <= m; i++)
                    {
                        if (!IsPageModal)
                        {
                            MonthEntity month = new MonthEntity(this.Year, i, Keyword, UserID, Status);
                            list.Add(month);
                        }
                        else
                        {
                            MonthEntity month = new MonthEntity(this.Year, i, Keyword, UserID, Status, CurrentPage, PageCount);
                            list.Add(month);
                        }
                    }
                    months = list;
                }
                return months;
            }
        }

        public YearEntity(int year, string keyword, int userID, int status)
        {
            this.Year = year;
            this.Keyword = keyword;
            this.UserID = userID;
            this.Status = status;
            this.IsPageModal = false;
        }
        public YearEntity(int year, string keyword, int userID, int status, int page, int pageCount)
        {
            this.Year = year;
            this.Keyword = keyword;
            this.UserID = userID;
            this.Status = status;
            this.IsPageModal = true;
            this.CurrentPage = page;
            this.PageCount = pageCount;
        }
    }
}
