using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoUpdateDate : IHttpHandler
    {
        #region declare
        TicketsApplication ticketApp = new TicketsApplication();
        bool result;
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                if (IdentityContext.UserID <= 0)
                    return;
                string tid = context.Request["tid"];
                string start = context.Request["start"];
                string end = context.Request["end"];

                TicketsEntity entity = ticketApp.GetTickets(Convert.ToInt32(tid));

                entity.StartDate = DateTime.Parse(start).Date;

                entity.DeliveryDate = DateTime.Parse(end).Date;

                result = ticketApp.UpdateTickets(entity);
               
                if (result)
                {
                    context.Response.Write("Update Successful!");
                }
                else
                {
                    context.Response.Write("Update Fail!");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoUpdateDate.ashx Messages:\r\n{0}", ex));
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
