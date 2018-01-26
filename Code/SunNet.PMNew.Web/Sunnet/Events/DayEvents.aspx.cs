using SunNet.PMNew.App;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Web.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.Web.Sunnet.Events
{
    public partial class DayEvents : BaseWebsitePage
    {
        protected string title = string.Empty;
        protected List<EventEntity> dayEvents = new List<EventEntity>();
        protected string selectedUserID;
        protected bool isEdit = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            int projectID = QS("ProjectID", -1);

            DateTime date = QS("Date", DateTime.Now);
            dayEvents = new EventsApplication().GetEventList(date, date.AddDays(1), UserInfo.UserID,  projectID);
            title = string.Format("{0}, {1}", date.DayOfWeek.ToString(), date.ToString("MMMM dd, yyyy"));
            if (date.AddDays(1) > DateTime.Now && date.AddDays(-1) < DateTime.Now)
            {
                isEdit = true;
            }
        }
    }
}