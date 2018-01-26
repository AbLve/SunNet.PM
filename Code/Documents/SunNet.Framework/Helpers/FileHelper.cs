using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Diagnostics;
using System.Threading;
using SF.Framework.Log;

namespace SF.Framework.Helpers
{
    public static class FileHelper
    {
        public static string ReadFileContent(HttpPostedFile file)
        {
            string fileContent = "";
            using (StreamReader sr = new StreamReader(file.InputStream))
            {
                fileContent = sr.ReadToEnd();
            }
            return fileContent;
        }
        public static string ReadFileContent(string filePath)
        {
            string fileContent = "";
            using (StreamReader sr = new StreamReader(filePath))
            {
                fileContent = sr.ReadToEnd();
            }
            return fileContent;
        }
        /// <summary>
        /// HTML Convert to PDF
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="path">PDF File Path</param>
        public static bool HtmlToPdf(string url, string path)
        {
            try
            {
                if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(path))
                {
                    return false;
                }
                using (Process p = new Process())
                {
                    string str = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wkhtmltopdf.exe");
                    if (!System.IO.File.Exists(str))
                    {
                        return false;
                    }
                    KillWKHtmltoPDF();
                    p.StartInfo.FileName = str;
                    p.StartInfo.Arguments = String.Format("\"{0}\" \"{1}\"", url, path);
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                    //_log.Info("HtmlToPdf:Convert Begin");
                    return ConfirmConvertSuccess(path);
                }
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
                //_log.Warn(ex.Message,ex);
            }
            return false;
        }

        private static bool ConfirmConvertSuccess(string path)
        {
            int count = 0;
            bool isSuccessful = true;
            while (true)
            {
                if (System.IO.File.Exists(path))
                {
                    //_log.Info("HtmlToPdf:Waiting For Converting Completion ..");
                    WaitWKHtmltoPDFClose();
                    //_log.Info("HtmlToPdf:Convert Successfully!");
                    break;
                }
                Thread.Sleep(1000);
                count++;
                if (count >= 300)
                {
                    //_log.Warn("HtmlToPdf:Convert Time Out,Convert Fail!");
                    isSuccessful = false;
                    break;
                }
            }
            return isSuccessful;
        }

        private static void WaitWKHtmltoPDFClose()
        {
            while (true)
            {
                Process[] procs = Process.GetProcessesByName("wkhtmltopdf");
                if (procs.Length > 0)
                {
                    Thread.Sleep(5000);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Kill WKHTMLTOPDF exe
        /// </summary>
        private static void KillWKHtmltoPDF()
        {
            try
            {
                Process[] procs = Process.GetProcessesByName("wkhtmltopdf");
                Array.ForEach(procs,
                delegate(Process proc)
                {
                    proc.Kill();
                });
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
                //_log.Warn(ex.Message, ex);
            }
        }
    }
}
