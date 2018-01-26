using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Web.Do;

namespace SunNet.PMNew.PM2014.Service
{
    /// <summary>
    /// File 的摘要说明
    /// </summary>
    public class File : DoBase, IHttpHandler
    {
        FileApplication fileApp = new FileApplication();
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            context.Response.ContentType = "application/json";
            if (UserID <= 0)
            {
                Response.Write("[]");
                return;
            }
            string action = context.Request.Params["action"].ToLower();
            int id = 0;
            switch (action)
            {
                case "delete":
                    if (int.TryParse(Request.Params["id"], out id) && fileApp.DeleteFile(id))
                        Response.Write(ResponseMessage.GetResponse(true));
                    else
                        Response.Write(ResponseMessage.GetResponse(false, fileApp.FirstBrokenRuleMessageContent));
                    break;
                default:
                    Response.Write("[]");
                    break;
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