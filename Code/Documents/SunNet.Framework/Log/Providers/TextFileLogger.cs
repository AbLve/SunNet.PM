using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

namespace SF.Framework.Log.Providers
{
    /// <summary>
    /// This class provides a consistent interface for logging information to any destination. 
    /// </summary>
    public class TextFileLogger : ILog
    {
        public void Log(Exception ex)
        {
            Log(ex.ToString());
        }

        public void Log(string message)
        {
            if (!SFConfig.LogEnabled)
            {
                return;
            }

            string fileName = SFConfig.LogFileName;
            FileInfo fi = new FileInfo(fileName);
            if (fileName == null || fileName == "")
            {
                fileName = HttpContext.Current.Server.MapPath("~/Log.txt");
            }
            if (System.IO.File.Exists(fileName) == true)
            {
                if (fi.Length > 1024 * 1024 * SFConfig.LogFileSize)
                {
                    System.IO.File.Delete(fileName);
                }
            }
            else
            {
                if (!Directory.Exists(fi.DirectoryName))
                {
                    Directory.CreateDirectory(fi.DirectoryName);
                }
            }

            using (StreamWriter sw = System.IO.File.AppendText(fileName))
            {
                string msg = GetLogMessageFormat();
                msg = msg.Replace("{Now}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                msg = msg.Replace("{Message}", message);
                sw.WriteLine(msg);
                sw.Flush();
                sw.Close();
            }
        }

        public string GetLogMessageFormat()
        {
            string msg = @"{Now}
------------------------------------------------------------------------------------------------
{Message}
------------------------------------------------------------------------------------------------\
";
            return msg;
        }
        public LogConfig Config { get; set; }
    }
}
