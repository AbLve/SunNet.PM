using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Entity.FileModel;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// Summary description for DoCancelTicketHandler
    /// </summary>
    public class DoCancelTicketHandler : IHttpHandler
    {

        TicketsApplication ticketAPP = new TicketsApplication();
        FileApplication fileApp = new FileApplication();
        bool FileDeleteError = false;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            try
            {
                if (IdentityContext.UserID <= 0)
                    return;

                int tid = Convert.ToInt32(context.Request["tid"]);

                bool Result = ticketAPP.UpdateStatus(tid, Entity.TicketModel.TicketsState.Cancelled);

                if (Result)
                {
                    FileDeleteError = fileApp.DeleteFile(tid, FileSourceType.Ticket);
                    if (!FileDeleteError)
                    {
                        WebLogAgent.Write(string.Format("Error :DoCancelTicketHandler.ashx Messages:\r\n TicketID{0} File Delete Error ", tid));
                    }
                }

                if (Result)
                {
                    context.Response.Write("Remove Success!");
                }
                else
                {
                    context.Response.Write("Remove Fail!");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoCancelTicketHandler.ashx Messages:\r\n{0}", ex));
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