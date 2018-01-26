using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoUpdateTaskStatusHandler : IHttpHandler
    {
        TicketsApplication ticketApp = new TicketsApplication();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            try
            {
                if (IdentityContext.UserID <= 0)
                    return;
                String taskid = context.Request["taskid"];

                bool result = ticketApp.UpdateComplete(Convert.ToInt32(taskid));

                if (result)
                {
                    context.Response.Write("Update Success!");
                }
                else
                {
                    context.Response.Write("Update Fail!");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoUpdateTaskStatusHandler.ashx Messages:\r\n{0}", ex));
                return;
            }

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
