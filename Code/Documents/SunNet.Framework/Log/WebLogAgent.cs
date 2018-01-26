using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace SF.Framework.Log
{
    public static class WebLogAgent
    { /// <summary>
        /// Write an exception object to log.
        /// </summary>
        /// <param name="ex">The exception data.</param>
        public static void Write(Exception ex)
        {
            Write(ex.ToString());
        }

        /// <summary>
        /// Write a new log message.
        /// </summary>
        /// <param name="message">Message body to log.</param>
        public static void Write(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Time:{0}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                sb.AppendFormat("IP:{0}\r\n", HttpContext.Current.Request.UserHostAddress)
                    .AppendFormat("Url:http://{0}{1}\r\n", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.RawUrl)
                    .AppendFormat("UrlRef:{0}\r\n", HttpContext.Current.Request.UrlReferrer == null ? "" : HttpContext.Current.Request.UrlReferrer.ToString())
                    .AppendFormat("Message:{0}\r\n\r\n", message);
            }
            else
            {
                sb.Append(message)
                    .Append("\r\n");
            }

            string fileName = SFConfig.LogFileName;
            if (fileName == null || fileName == "")
            {
                fileName = HttpContext.Current.Server.MapPath("~/Log.txt");
            }

            using (StreamWriter write = new StreamWriter(fileName, true, Encoding.Default))
            {
                write.WriteLine(sb.ToString());
            }
        }


    }
}
