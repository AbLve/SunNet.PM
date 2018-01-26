using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// Summary description for DoDeleteWorkRequestDocuments
    /// </summary>
    public class DoDeleteWorkRequestDocuments : IHttpHandler
    {
        FileApplication app = new FileApplication();

        public void ProcessRequest(HttpContext context)
        {
            if (IdentityContext.UserID <= 0)
                return;

            UsersEntity user = new App.UserApplication().GetUser(IdentityContext.UserID);
            if (user == null || (user.Role != RolesEnum.ADMIN && user.Role != RolesEnum.PM && user.Role != RolesEnum.Sales))
            {
                return;
            }

            string id = context.Request["ID"];
            int fileId;
            if (!int.TryParse(id, out fileId))
                return;

            app.DeleteFile(fileId);
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