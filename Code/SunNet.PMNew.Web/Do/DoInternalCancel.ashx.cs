using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DoInternalCancel : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (IdentityContext.UserID <= 0)
                return;

            int pid = 0;
            if (!int.TryParse(context.Request.Form["pId"], out pid))
                return;
            int tid = 0;
            if (!int.TryParse(context.Request.Form["tId"], out tid))
                return;
            int status;
            if (!int.TryParse(context.Request.Form["status"], out status))
                return;

            UsersEntity userEntity = new App.UserApplication().GetUser(IdentityContext.UserID);
            if (userEntity.Role == RolesEnum.PM || userEntity.Role == RolesEnum.Leader)
            {
                TicketsApplication ticketAPP = new TicketsApplication();
                if (ticketAPP.ChangeInternalTicketStatus(IdentityContext.UserID, tid, (SunNet.PMNew.Entity.TicketModel.TicketsState)status))
                    context.Response.Write("1"); //ok
                else
                    context.Response.Write("2"); //ticket associated personnel operate it!
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
