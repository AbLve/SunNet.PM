using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.Web.Sunnet.Project
{
    /// <summary>
    /// Summary description for DoAddNote
    /// </summary>
    public class DoDeletePrincipal : DoBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int ID;
            if (int.TryParse(context.Request["id"], out ID))
            {
                if (new ProjectApplication().DeleteProjectPrincipal(ID))
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