using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoRemoveFileHandler : IHttpHandler
    {
        FileApplication fileApp = new FileApplication();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int fileId = Convert.ToInt32(context.Request["fileid"]);
            bool result = fileApp.DeleteFile(fileId);
            if (result)
            {
                context.Response.Write("Remove Success!");
            }
            else
            {
                context.Response.Write("Remove Fail!");
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
