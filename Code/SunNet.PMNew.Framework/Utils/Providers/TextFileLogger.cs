using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

namespace SunNet.PMNew.Framework.Utils.Providers
{
    /// <summary>
    /// This class provides a consistent interface for logging information to any destination. 
    /// </summary>
    public class TextFileLogger:ILog
    {
        public void Log(Exception ex)
        {
            Log(ex.ToString());
        }

        public void Log(string message)
        {
            if (!Config.LogEnabled)
            {
                return;
            }

            string fileName = Config.LogFileName;
            if (fileName == null || fileName == "")
            {
                fileName = HttpContext.Current.Server.MapPath("~/Log.txt");
            }
            if (File.Exists(fileName) == true)
            {
                FileInfo fi = new FileInfo(fileName);
                if (fi.Length > 1024 * 1024 * Config.LogFileSize)
                {
                    File.Delete(fileName);
                }
            }

            using (StreamWriter sw = File.AppendText(fileName))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.WriteLine("------------------------------------------------------------------------------------------------");
                sw.WriteLine(message);
                sw.WriteLine("------------------------------------------------------------------------------------------------");
                sw.WriteLine();
                sw.Flush();
                sw.Close();
            }
        }
    }
}
