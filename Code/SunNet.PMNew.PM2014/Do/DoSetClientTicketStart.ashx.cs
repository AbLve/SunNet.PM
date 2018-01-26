using SunNet.PMNew.App;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// Summary description for DoSetClientTicketStart
    /// </summary>
    public class DoSetClientTicketStart : DoBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int ticketID = QS(context.Request.QueryString["tid"], 0);
            int star = QS(context.Request.QueryString["star"], 0);
            if (ticketID > 0)
            {
                TicketsApplication ticketsApplication = new TicketsApplication();
                if (ticketsApplication.UpdateTicketStar(ticketID, star))
                {
                    context.Response.Write("1");
                }
                else
                {
                    context.Response.Write("0");
                }
            }
            else
            {
                context.Response.Write("0");
            }
            context.Response.End();
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