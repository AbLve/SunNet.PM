using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoUpdateEnStatus : IHttpHandler
    {
        ProjectApplication projectAPP = new ProjectApplication();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                if (IdentityContext.UserID <= 0)
                    return;
                String tType = context.Request["tType"];
                String pId = context.Request["pId"];

                ProjectsEntity project = projectAPP.Get(Convert.ToInt32(pId));
                if (null != project)
                {
                    if ((tType == "0" && project.BugNeedApproved == true) ||
                       (tType == "1" && project.RequestNeedApproved == true))
                    {
                        context.Response.Write("true");
                    }
                    else
                    {
                        context.Response.Write("false");
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoUpdateEnStatus.ashx Messages:\r\n{0}", ex));
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
