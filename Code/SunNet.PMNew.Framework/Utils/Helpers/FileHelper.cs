using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Framework.Utils.Helpers
{
    public class FileHelper
    {
        public string ReadFileContent(HttpPostedFile file)
        {
            string fileContent = "";
            using (StreamReader sr = new StreamReader(file.InputStream))
            {
                fileContent = sr.ReadToEnd();
            }
            return fileContent;
        }
        public string ReadFileContent(string filePath)
        {
            string fileContent = "";
            using (StreamReader sr = new StreamReader(filePath))
            {
                fileContent = sr.ReadToEnd();
            }
            return fileContent;
        }
        private string NewUploadFileName
        {
            get
            {
                Random ran = new Random();
                string result = string.Format("{0}_{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff"), ran.Next(10000, 99999));
                System.Threading.Thread.Sleep(10);
                return result;
            }
        }
        private string GetFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                try
                {
                    Directory.CreateDirectory(folder);
                }
                catch (Exception ex)
                {
                    throw new Exception("CreateFolderError", ex);
                }
            }
            return folder;
        }
        public string SaveUploadFiles(string module, object objID, HttpPostedFile file)
        {
            string absl = GetFolder(HttpContext.Current.Server.MapPath("~/upload/"));
            absl = GetFolder(string.Format("{0}/{1}/", absl, module));
            absl = GetFolder(string.Format("{0}/{1}/", absl, objID));
            string rela = "/upload/";
            rela = string.Format("{0}/{1}/{2}", rela, module, objID.ToString());

            string filename = NewUploadFileName;
            string savepath = string.Format("{0}/{1}{2}", absl, filename, Path.GetExtension(file.FileName));
            file.SaveAs(savepath);
            filename = string.Format("{0}/{1}{2}", rela, filename, Path.GetExtension(file.FileName));
            return filename;
        }

        public string GetTemplateFileContent(string filename)
        {
            try
            {
                string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "Template\\" + filename;
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
                return "";
            }
        }
    }
}
