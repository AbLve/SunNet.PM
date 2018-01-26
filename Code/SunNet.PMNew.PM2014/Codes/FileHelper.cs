using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		5/19 9:25:42
 * Description:		Web层IO操作相关类
 * Version History:	Created,5/19 9:25:42
 * 
 * 
 **************************************************************************/
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.PM2014.Codes
{
    /// <summary>
    /// 处理上传文件
    /// </summary>
    /// Author  :  Jack Zhang (JACKZ)
    /// Date    :  5/19 9:25
    public static class FileHelper
    {
        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <param name="sourdeFileName"> ~/Config.UploadPath/{filename} 或者C:/....../{filename}.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  6/3 03:13
        public static long GetFileLength(string sourdeFileName)
        {
            try
            {
                string fullPath = sourdeFileName.IndexOf(":") > 0
                    ? sourdeFileName
                    : HttpContext.Current.Server.MapPath(Path.Combine(Config.UploadPath, sourdeFileName));
                FileInfo file = new FileInfo(fullPath);
                if (file.Exists)
                    return file.Length;
                return 0;
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
                return 0;
            }
        }

        /// <summary>
        /// Moves the specified source file name.
        /// </summary>
        /// <param name="sourceFileName">只有filename，内部自动从上传目录读取全路径.</param>
        /// <param name="dest">完整的目标路径.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/19 9:35
        public static bool Move(string sourceFileName, string dest)
        {

            string fullPath = HttpContext.Current.Server.MapPath(Path.Combine(Config.UploadPath, sourceFileName));
            if (!File.Exists(fullPath))
                return false;
            File.Move(fullPath, dest);
            return true;
        }

        /// <summary>
        /// Copies the specified source file name.
        /// </summary>
        /// <param name="sourceFileName">只有filename，内部自动从上传目录读取全路径.</param>
        /// <param name="dest">The dest.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/19 14:23
        public static bool Copy(string sourceFileName, string dest)
        {
            try
            {
                string fullPath = HttpContext.Current.Server.MapPath(Path.Combine(Config.UploadPath, sourceFileName));
                if (!File.Exists(fullPath))
                    return false;
                File.Copy(fullPath, dest);
                return true;
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
                return false;
            }
        }

        /// <summary>
        /// 读取邮件模版内容.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/19 21:03
        public static string GetEmailTemplate(string filename, string folder = "")
        {
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "Template\\" + filename;
            return File.Exists(filePath) ? File.ReadAllText(filePath) : "";
        }
    }
}