using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.IO;
using Yahoo.Yui.Compressor;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Js : IHttpHandler
    {
        private const string JsKey = "JSPATH_KEY";
        class JsCache
        {
            public string Content { set; get; }
            public DateTime Expires { set; get; }
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/javascript";
            try
            {
                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string jsPath = GetJsPaths();
                if (string.IsNullOrEmpty(jsPath))
                {
                    response.Write("No Content");
                    return;
                }
                string[] files = jsPath.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries);
                JsCache item = null;
                object obj = HttpRuntime.Cache.Get(JsKey);
#if DEBUG
                obj = null;
#endif
                if (null == obj)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string fileName in files)
                    {
                        string filePath = context.Server.MapPath(fileName);
                        if (File.Exists(filePath))
                        {
                            string content = File.ReadAllText(filePath, Encoding.UTF8);
# if !DEBUG
                            JavaScriptCompressor javascriptCompressor = new JavaScriptCompressor();
                            content = javascriptCompressor.Compress(content);
#endif
                            sb.Append(content);
                        }
                        else
                        {
                            sb.Append("\r\nSources not being found," + filePath + "\r\n");
                        }
                    }//end foreach
                    item = new JsCache() { Content = sb.ToString(), Expires = DateTime.Now.AddHours(1) };
                    HttpRuntime.Cache.Insert(JsKey, item, null, item.Expires, TimeSpan.Zero);
                }
                else
                {
                    item = obj as JsCache;
                }
                if (request.Headers["If-Modified-Since"] != null && TimeSpan.FromTicks(item.Expires.Ticks - DateTime.Parse(request.Headers["If-Modified-Since"]).Ticks).Seconds < 100)
                {
                    response.StatusCode = 304;
                    response.StatusDescription = "Not Modified";
                }
                else
                {
                    response.Write(item.Content);
                    SetClientCaching(response, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:JS.ashx Messages:\r\n{0}", ex));
                return;
            }

        }

        private void SetClientCaching(HttpResponse response, DateTime lastModified)
        {
            response.Cache.SetETag(lastModified.Ticks.ToString());
            response.Cache.SetLastModified(lastModified);
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
            response.Cache.SetSlidingExpiration(true);
        }

        private string GetJsPaths()
        {
            string jsPaths = HttpContext.Current.Cache["JsPaths"] as string;
            if (string.IsNullOrEmpty(jsPaths))
            {
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/config/js.txt")))
                {
                    jsPaths = reader.ReadToEnd();
                    HttpContext.Current.Cache.Insert("JsPaths", jsPaths, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(30));
                }
            }
            return jsPaths;
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
