using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;


namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoDeleteWorkRequestDocuments : IHttpHandler
    {
        FileApplication app = new FileApplication();

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


                string[] tidArray = checkboxList.Split(',');

                bool IsError = true;

                foreach (string item in tidArray)
                {
                    if (item.Length > 0)
                    {
                        if (app.DeleteFile(Convert.ToInt32(item)))
                        {
                            ErrorMsg.Add(Convert.ToInt32(item), "Suc");
                        }
                        else
                        {
                            ErrorMsg.Add(Convert.ToInt32(item), "Fail");
                            IsError = false;
                        }
                    }
                }
                if (!IsError)
                {
                    context.Response.Write(ErrorMsg.Count + "Files Delete Not Successful!");
                }
                context.Response.Write("Delete Success!");
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoDeleteWorkRequestDocuments.ashx Messages:\r\n{0}", ex));
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
