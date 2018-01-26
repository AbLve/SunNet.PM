using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// Summary description for iCalFeed
    /// </summary>
    public class iCalFeed : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (IdentityContext.UserID <= 0)
                return;
            UsersEntity user = new App.UserApplication().GetUser(IdentityContext.UserID);
            if (user == null) return;

            Thread.CurrentThread.Name = "Host" + IdentityContext.UserID;
            string s = CreateICSString(user);
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1.0);
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.Charset = "UTF-8";
            context.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            context.Response.ContentType = "application/octet-stream";
            context.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode("events.ics", Encoding.UTF8));
            context.Response.BinaryWrite(bytes);
            context.Response.Flush();
            context.Response.End();
        }

        private string CreateICSString(UsersEntity user)
        {
            List<EventEntity> list = new App.EventsApplication().GetEventList(user.UserID, DateTime.Parse("1990/01/01"), DateTime.MaxValue, user.UserID,string.Empty, 0);
            StringBuilder builder = new StringBuilder();
            builder.Append("BEGIN:VCALENDAR\n");
            builder.Append("PRODID:-//SunNet Solution//My PM Event Book 1.0000//EN\n");
            builder.Append("VERSION:2.0\n");
            builder.Append("CALSCALE:GREGORIAN\n");
            builder.Append("METHOD:PUBLISH\n");
            builder.Append("X-WR-CALNAME:My PM Event Book\n");
            builder.Append("X-WR-TIMEZONE:America/Chicago\n");
            foreach (EventEntity entity in list)
            {
                DateTime dtFromDay = entity.FromDay;               
                DateTime dtToDay = entity.ToDay;
                if (!entity.AllDay)
                {
                    dtFromDay = DateTime.Parse(string.Format("{0}/{1}/{2} {3} {4}"
               , entity.FromDay.Month, entity.FromDay.Day, entity.FromDay.Year, entity.FromTime, entity.FromTimeType == 1 ? "AM" : "PM"));

                    dtToDay = DateTime.Parse(string.Format("{0}/{1}/{2} {3} {4}"
               , entity.ToDay.Month, entity.ToDay.Day, entity.ToDay.Year, entity.ToTime, entity.ToTimeType == 1 ? "AM" : "PM"));

                }

                StringBuilder builder2 = new StringBuilder();
                ProjectsEntity project = new App.ProjectApplication().Get(entity.ProjectID);
                builder2.Append("Project Name: " + project.Title + @"\n");
                builder2.Append("Name: " + entity.Name + @"\n");
                builder2.Append("Details: " + entity.Details + @"\n");
                if (entity.AllDay)
                {
                    builder2.Append("FromDay: " + dtFromDay.ToString("MM/dd/yyyy") + @"\n");
                    builder2.Append("ToDay: " + dtToDay.ToString("MM/dd/yyyy") + @"\n");
                }
                else
                {
                    builder2.Append("FromDay: " + dtFromDay.ToString("MM/dd/yyyy hh:mm t") + @"\n");
                    builder2.Append("ToDay: " + dtToDay.ToString("MM/dd/yyyy hh:mm t") + @"\n");
                }

                builder.Append("BEGIN:VEVENT\n");

                if (entity.AllDay)
                {
                    builder.Append("DTEND;VALUE=DATE:" + dtToDay.AddDays(1).ToString("yyyyMMdd") + "\n");
                }
                else
                    builder.Append("DTEND:" + dtToDay.ToString("yyyyMMddTHHmmss") + "\n");


                builder.Append("DTSTAMP:" + DateTime.Now.ToString("yyyyMMddTHHmmss") + "\n");

                if(entity.AllDay)
                    builder.Append("DTSTART;VALUE=DATE:" + dtFromDay.ToString("yyyyMMdd") + "\n");
                else
                    builder.Append("DTSTART:" + dtFromDay.ToString("yyyyMMddTHHmmss") + "\n");


                builder.Append("UID:" + entity.CreatedBy.ToString("N").ToLower() + "@myfamilybook.us\n");
                builder.Append("CREATED:" + entity.CreatedOn.ToString("yyyyMMddTHHmmss") + "\n");
                builder.Append("DESCRIPTION:" + builder2.ToString() + "\n");
                builder.Append("LAST-MODIFIED:" + entity.UpdatedOn.ToString("yyyyMMddTHHmmss") + "\n");
                builder.Append("LOCATION:" + entity.Where + "\n");
                builder.Append("SEQUENCE:0\n");
                builder.Append("STATUS:CONFIRMED\n");
                builder.Append("SUMMARY:" + entity.Name + "\n");
                builder.Append("TRANSP:TRANSPARENT\n");

                builder.Append("END:VEVENT\n");
            }
            builder.Append("END:VCALENDAR");
            return builder.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}