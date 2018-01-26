using SunNet.PMNew.App;
using SunNet.PMNew.Entity.EventModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for GetEventList
    /// </summary>
    public class DoGetEventList : DoBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int projectID = -1;
            if (!string.IsNullOrEmpty(context.Request.QueryString["projectID"]))
            {
                projectID = int.Parse(context.Request.QueryString["projectID"]);
            }
            List<UpcomingEvent> upcomingEvents = new EventsApplication().GetEventList(DateTime.Now, UserID, projectID, 10);
            StringBuilder stringBuilder = new StringBuilder();
            int itemCount = 0;

            foreach (SunNet.PMNew.Entity.EventModel.UpcomingEvent upcomingEvent in upcomingEvents)
            {
                stringBuilder.AppendFormat("<div class=\"pdlistTop\"> {0}</div>", upcomingEvent.Date)
                    .Append("<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"listtwo\">")
                    .Append("<tbody>");
                for (int i = 0; i < upcomingEvent.list.Count; i++)
                {
                    itemCount++;
                    SunNet.PMNew.Entity.EventModel.ListView item = upcomingEvent.list[i];
                    stringBuilder.AppendFormat("<tr {0} class=\"{1}\">", ShowEditEvent(item.ID), i % 2 == 0 ? "listrowone" : "listrowtwo")
                         .AppendFormat("<td width=\"40%\" notAction><img src=\"/images/clock.png\" style=\"vertical-align: middle;\"> {0}</td>", item.Time)
                          .AppendFormat("<td width=\"60%\">{0}</td>", item.Name)
                          .Append("</tr>");
                }
                stringBuilder.Append("</tbody></table>");

            }
            if (itemCount == 0)
            {
                stringBuilder.Append("<div class=\"mainowConbox\" style=\"min-height: 80px;\">")
                    .Append("<div class=\"ownothingText\">Nothing was scheduled.</div>")
                    .Append("</div>");
            }
            context.Response.Write(stringBuilder.ToString());
            context.Response.End();

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        protected string ShowEditEvent(int id)
        {
            return string.Format("opentype='popwindow' dialogtitle=\'{0}\' dialogwidth={1} dialogheight={2} href='/Sunnet/Events/EditEvent.aspx?ID={3}' target='edit'", "Edit Event", 460, 470, id);
        }
    }
}