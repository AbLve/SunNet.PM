using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// Summary description for DoDeleteRelationWorkRequest
    /// </summary>
    public class DoDeleteRelationWorkRequest : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {

            if (IdentityContext.UserID <= 0)
                return;

            UsersEntity user = new App.UserApplication().GetUser(IdentityContext.UserID);
            if (user == null || (user.Role != RolesEnum.PM && user.Role != RolesEnum.Sales && user.Role != RolesEnum.ADMIN))
                return;

            ProposalTrackerApplication app = new ProposalTrackerApplication();

            context.Response.ContentType = "text/plain";
            string tkId = context.Request["ticketId"] + "";
            string wrId = context.Request["wrId"] + "";

            int ticketId;
            if (!int.TryParse(tkId, out ticketId))
                return;
            int proposaltrackerId;
            if (!int.TryParse(wrId, out proposaltrackerId))
                return;

            app.DeleteRelation(proposaltrackerId, ticketId);
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