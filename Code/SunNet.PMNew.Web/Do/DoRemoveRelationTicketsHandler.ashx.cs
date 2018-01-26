using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoRemoveRelationTicketsHandler : IHttpHandler
    {
        TicketsRelationApplication trAPP = new TicketsRelationApplication();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                if (IdentityContext.UserID <= 0)
                    return;
                int tid = Convert.ToInt32(context.Request["tid"]);
                int rid = Convert.ToInt32(context.Request["rid"]);

                RemoveTicketsRelationRequest request = new RemoveTicketsRelationRequest();

                request.Tid = tid;
                request.Rid = rid;

                RemoveTicketsRelationResponse reponse = trAPP.RemoveTR(request);

                if (reponse.RemoveSuc)
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
                WebLogAgent.Write(string.Format("Error Ashx:DoRemoveRelationTicketsHandler.ashx Messages:\r\n{0}", ex));
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
