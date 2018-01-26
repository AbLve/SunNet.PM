using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Framework.Core;
using System.Reflection;
using System.IO;

namespace SunNet.PMNew.Framework.Core.UI.JS
{
    public class JSDownloaderHttpHandler: IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string js = "";
            context.Response.ContentType = "text/javascript";
            string file = Convert.ToString(context.Request.QueryString["file"])??"";
            file = file.Trim().ToLower();

            if (file == "SunNetCore.js".ToLower())
            {
                js = GetStringByEmbededResource("SunNetCore.js");
                context.Response.Write(js);
            }
        }

        private static string GetStringByEmbededResource(string jsFileName)
        {
            string js = "";
            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("SunNet.Library.Core.UI.JS." + jsFileName);
            StreamReader sr = new StreamReader(s);
            js = sr.ReadToEnd();
            return js;
        }
    }
}