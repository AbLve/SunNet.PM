using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// Summary description for DoDeleteRelationDocument
    /// </summary>
    public class DoDeleteRelationDocument : IHttpHandler
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
            string fId = context.Request["fileId"] + "";
            string wrId = context.Request["wrId"] + "";

            int fileId;
            if (!int.TryParse(fId, out fileId))
                return;
            int proposaltrackerId;
            if (!int.TryParse(wrId, out proposaltrackerId))
                return;

            FileApplication fileApp = new FileApplication();
            fileApp.DeleteFile(fileId);
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