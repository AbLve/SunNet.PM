using SunNet.PMNew.App;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.Event
{
    public partial class DayEvent : BasePage
    {
        protected string title = string.Empty;
        protected List<EventEntity> dayEvents = new List<EventEntity>();
        protected void Page_Load(object sender, EventArgs e)
        {
            int projectID = QS("ProjectID", -1);

            DateTime date = QS("Date", DateTime.Now);
            string allUser = QS("allUser").Trim();
            int userId = QS("UserId", 0);

            if (userId != 0 || !string.IsNullOrEmpty(allUser))
            {
                dayEvents = new EventsApplication().GetEventList(UserInfo.UserID, date, date.AddDays(1), userId, allUser, projectID);
            }

            title = string.Format("{0}, {1}", date.DayOfWeek.ToString(), date.ToString("MMMM dd, yyyy", DateTimeFormatInfo.InvariantInfo));
        }
    }
}