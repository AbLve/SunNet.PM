using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.Web.Codes.Schedule
{
    public class BaseSchedule
    {
        protected int Status;
        protected string Keyword;
        protected int UserID;
        protected bool IsPageModal;
        protected int CurrentPage;
        protected int PageCount;

        protected int Day { get; set; }
        protected int Month { get; set; }
        protected int Year { get; set; }
    }
}
