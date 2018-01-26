using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Core.TicketModule
{
    public class CateGoryFilterCondition
    {
        public int UserID { get; set; }
        public string Title { get; set; }
        public DateTime CreateOn { get; set; }
        public bool IsDelete { get; set; }
        public bool IsOnlyShowTody { get; set; }

        public CateGoryFilterCondition(int userID, string title, DateTime createOn, bool isDelete, bool isOnlyShowToday)
        {
            this.UserID = userID;
            this.Title = title;
            this.CreateOn = createOn;
            this.IsDelete = isDelete;
            this.IsOnlyShowTody = isOnlyShowToday;
        }
    }
}
