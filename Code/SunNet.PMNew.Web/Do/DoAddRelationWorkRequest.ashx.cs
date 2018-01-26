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
    public class DoAddRelationWorkRequest : IHttpHandler
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
                        WRDTO.WID = Convert.ToInt32(wid);

                        WRDTO.TID = Convert.ToInt32(item);

                        WRDTO.CreatedBy = IdentityContext.UserID;


                        if (app.AddWorkRequestRelation(WRDTO) > 0)
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
                    context.Response.Write(ErrorMsg.Count + "Files Add Not Successful!");
                }
                context.Response.Write("Add Success!");
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoAddRelationWorkRequest.ashx Messages:\r\n{0}", ex));
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
