using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class TicketStar : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (IdentityContext.UserID <= 0)
            {
                context.Response.Write("[]");
                return;
            }
            string type = context.Request["type"];
            if (string.IsNullOrEmpty(type))
            {
                context.Response.Write("");
                return;
            }
            string response = string.Empty;
            switch (type)
            {
                case "UpdateStar":
                    int ticketid = 0;
                    int star = 0;
                    if (int.TryParse(context.Request["ticketid"], out ticketid)
                        && int.TryParse(context.Request["star"], out star))
                    {
                        TicketsApplication tickApp = new TicketsApplication();
                        if (tickApp.UpdateTicketStar(ticketid, star))
                        {
                            response = ResponseMessage.GetResponse(true, ResponseMessage.SuccessMessage, star);
                        }
                        else
                        {
                            response = ResponseMessage.GetResponse(true, ResponseMessage.SuccessMessage, star);
                        }
                    }
                    else
                    {
                        response = ResponseMessage.GetResponse(true, "Arguments Error", 0);
                    }
                    context.Response.Write(response);
                    break;
                default: break;
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
