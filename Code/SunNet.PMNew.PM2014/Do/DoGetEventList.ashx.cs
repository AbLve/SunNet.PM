using SunNet.PMNew.App;
using SunNet.PMNew.Entity.EventModel;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            UpcomingEvent todayEvent = new UpcomingEvent();
            List<UpcomingEvent> upcomingEvents = new EventsApplication().GetEventList(DateTime.Now, UserID, projectID, 10);
            int itemCount = 0;
            StringBuilder stringBuilder = new StringBuilder();
            if (upcomingEvents.Count > 0 && upcomingEvents[0].Day == DateTime.Now.Date)
            {
                todayEvent = upcomingEvents[0];
                stringBuilder.AppendFormat("<div class=\"contentTitle cdtitleeventlist\"> {0}</div>", "Today")
                        .Append("<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"table-advance\">")
                        .Append(" <thead><tr><th width=\"110\">Name</th><th>Title</th></tr></thead>")
                        .Append("<tbody>");
                for (int i = 0; i < todayEvent.list.Count; i++)
                {
                    itemCount++;
                    SunNet.PMNew.Entity.EventModel.ListView item = todayEvent.list[i];
                    stringBuilder.AppendFormat("<tr {0} class=\"{1}\">", ShowEditEvent(item.ID), i % 2 == 0 ? "" : "whiterow")
                         .AppendFormat("<td width=\"40%\" notAction><img src=\"/images/clock.png\" style=\"vertical-align: middle;\"> {0}</td>", item.Time)
                          .AppendFormat("<td width=\"60%\">{0}</td>", item.Name)
                          .Append("</tr>");
                }
                stringBuilder.Append("</tbody></table>");
            }
            if (itemCount == 0)
            {
                stringBuilder.Append("<div class=\"mainowConbox\" style=\"min-height: 80px;\">")
                    .Append("<div class=\"ownothingText\" style=\"color:red\">No scheduled event.</div>")
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
            return "href=\"/Event/Edit.aspx?ID=" + id + "&eventList=1\" data-target=\"#modalsmall\" data-toggle=\"modal\"";
        }
    }
}