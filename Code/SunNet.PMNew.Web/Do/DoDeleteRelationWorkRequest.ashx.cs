using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Entity.WorkRequestModel;


namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoDeleteRelationWorkRequest : IHttpHandler
    {
        WorkRequestApplication app = new WorkRequestApplication();

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (IdentityContext.UserID <= 0)
                    return;

                Dictionary<int, string> ErrorMsg = new Dictionary<int, string>();

                context.Response.ContentType = "text/plain";

                String checkboxList = context.Request["checkboxList"];

                String wid = context.Request["wid"];

                WorkRequestRelationEntity WRDTO = new WorkRequestRelationEntity();

                string[] tidArray = checkboxList.Split(',');

                bool IsError = true;

                foreach (string item in tidArray)
                {
                    if (item.Length > 0)
                    {
                        if (app.DeleteRelation(Convert.ToInt32(wid), Convert.ToInt32(item)))
                        {
                            ErrorMsg.Add(Convert.ToInt32(item), "Suc");
                            IsError = false;
                        }
                        else
                        {
                            ErrorMsg.Add(Convert.ToInt32(item), "Fail");
                            IsError = true;
                        }
                    }
                }
                if (IsError)
                {
                    context.Response.Write(ErrorMsg.Count + " files are failed to delete.");
                }
                else
                {
                    context.Response.Write("Delete Success!");
                }

            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoDeleteRelationWorkRequest.ashx Messages:\r\n{0}", ex));
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
