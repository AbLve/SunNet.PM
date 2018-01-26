using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SunNet.PMNew.App;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Web.Do;

namespace SunNet.PMNew.PM2014.Service
{
    /// <summary>
    /// Share 的摘要说明
    /// </summary>
    public class Share : DoBase, IHttpHandler
    {
        private ShareApplication _shareApp = new ShareApplication();
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
            string keyword = context.Request.Params["keyword"];
            var id = 0;
            string msg = "";
            switch (action)
            {
                case "getsharetype":
                    var list = _shareApp.GetShareTypes().FindAll(x => x.Title.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase) >= 0);
                    var json = list.Select(x => new { text = x.Title, value = x.ID });
                    Response.Write(JsonConvert.SerializeObject(json));
                    break;
                case "delete":
                    int.TryParse(Request.Params["id"], out id);
                    if (id > 0)
                    {
                        if (_shareApp.Delete(id))
                        {
                            Response.Write(ResponseMessage.GetResponse(true));
                        }
                        else
                        {
                            msg = _shareApp.BrokenRuleMessages.Count > 0 ? _shareApp.BrokenRuleMessages[0].Message : "";
                            Response.Write(ResponseMessage.GetResponse(false, msg));
                        }
                    }
                    else
                    {
                        Response.Write(ResponseMessage.GetResponse(false, "Arguments error."));
                    }
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