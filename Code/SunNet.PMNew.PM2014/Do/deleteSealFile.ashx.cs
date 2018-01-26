using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// Summary description for deleteSealFile
    /// </summary>
    public class deleteSealFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (IdentityContext.UserID <= 0)
                return;

            string id = context.Request.Form["id"];
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