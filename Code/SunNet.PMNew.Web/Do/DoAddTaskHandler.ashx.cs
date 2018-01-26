using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoAddTaskHandler : IHttpHandler
    {
        TicketsApplication ticketApp = new TicketsApplication();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                if (IdentityContext.UserID <= 0)
                    return;

                #region get value
                int tid = Convert.ToInt32(context.Request["tid"]);
                String title = context.Request["title"];
                String descr = context.Request["descr"];
                #endregion

                #region declare

                TasksEntity ta = new TasksEntity();
                int result = 0;

                #endregion

                GetProjectIdAndUserIDResponse response = ticketApp.GetProjectIdAndUserID(tid);

                int pid = response.ProjectId;

                ta.TicketID = tid;
                ta.ProjectID = pid;
                ta.Title = title.NoHTML();
                ta.Description = descr.NoHTML();
                ta.IsCompleted = false;
                ta.CompletedDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();

                result = ticketApp.AddTaskEntity(ta);

                if (result > 0)
                {
                    context.Response.Write("Add Successful!");
                }
                else
                {
                    context.Response.Write("Add Fail!");
                }

            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoAddTaskHandler.ashx Messages:\r\n{0}", ex));
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
