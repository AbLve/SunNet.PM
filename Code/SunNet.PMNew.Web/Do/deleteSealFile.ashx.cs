using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Entity.SealModel;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class deleteSealFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (IdentityContext.UserID <= 0)
                return;

            string id = context.Request.QueryString["id"];
            int fileId = 0;
            if (!int.TryParse(id, out fileId))
            {
                context.Response.Write("File not found.");
                return;
            }

            if (new App.SealsApplication().SealFilesDelete(fileId, IdentityContext.UserID))
            {
                context.Response.Write("OK");
                return;
            }
            else
                context.Response.Write("Failed.");
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
