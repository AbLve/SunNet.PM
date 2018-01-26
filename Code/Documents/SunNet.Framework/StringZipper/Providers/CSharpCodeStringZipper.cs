using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System.Threading;
using System.Web;
using SF.Framework.File;

namespace SF.Framework.StringZipper.Providers
{
    public class CSharpCodeStringZipper : IStringZipper
    {
        public string Zip(string uncompressedString)
        {
            byte[] bytData = System.Text.Encoding.Unicode.GetBytes(uncompressedString);
            MemoryStream ms = new MemoryStream();
            Stream s = new ICSharpCode.SharpZipLib.BZip2.BZip2OutputStream(ms);
            s.Write(bytData, 0, bytData.Length);
            s.Close();
            byte[] compressedData = (byte[])ms.ToArray();
            return System.Convert.ToBase64String(compressedData, 0, compressedData.Length);
        }

        public string UnZip(string compressedString)
        {
            System.Text.StringBuilder uncompressedString = new System.Text.StringBuilder();
            int totalLength = 0;
            byte[] bytInput = System.Convert.FromBase64String(compressedString); ;
            byte[] writeData = new byte[4096];
            Stream s2 = new ICSharpCode.SharpZipLib.BZip2.BZip2InputStream(new MemoryStream(bytInput));
            while (true)
            {
                int size = s2.Read(writeData, 0, writeData.Length);
                if (size > 0)
                {
                    totalLength += size;
                    uncompressedString.Append(System.Text.Encoding.Unicode.GetString(writeData, 0, size));
                }
                else
                {
                    break;
                }
            }
            s2.Close();
            return uncompressedString.ToString();
        }

        /// <summary>
        /// 创建zip  directories 和 filenames 是对应的
        /// </summary>
        /// <param name="directories">文件目录</param>
        /// <param name="filenames">文件路径</param>
        /// <param name="zipFileName">文件名及路径</param>
        /// <param name="dic">所在目录</param>
        /// <param>进行对加密文件压缩下载</param>
        public static bool CreateZip(List<string> directories, IEnumerable<string> filenames, IEnumerable<string> displayNames, string zipFileName, string dir)
        {
            if (!System.IO.Directory.Exists(dir + "/zip/"))
            {
                System.IO.Directory.CreateDirectory(dir + "/zip/");
            }
            using (ZipOutputStream ZipStream = new ZipOutputStream(System.IO.File.Create(dir + "/zip/" + zipFileName)))
            {
                ZipStream.SetLevel(9);
                ZipEntryFactory factory = new ZipEntryFactory();
                foreach (var directory in directories)
                {
                    if (!string.IsNullOrEmpty(directory))
                    {
                        string virtualDirectory = directory;
                        ZipEntry zipEntry = factory.MakeDirectoryEntry(virtualDirectory);
                        zipEntry.DateTime = DateTime.Now;
                        ZipStream.PutNextEntry(zipEntry);
                    }
                }

                byte[] buffer = new byte[4096];
                for (int i = 0; i < filenames.Count(); i++)
                {
                    string file = filenames.ElementAt(i);
                    if (!string.IsNullOrEmpty(file))
                    {
                        string newfileName = displayNames.ElementAt(i);
                        ZipEntry entry;
                        if (!string.IsNullOrEmpty(directories[i]))
                        {
                            entry = factory.MakeFileEntry(directories[i] + "//" + newfileName);
                        }
                        else
                        {
                            entry = factory.MakeFileEntry(newfileName);
                        }

                        entry.DateTime = DateTime.Now;
                        ZipStream.PutNextEntry(entry);
                        using (FileStream fs = System.IO.File.OpenRead(file))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                ZipStream.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }
                }
                ZipStream.Finish();
                ZipStream.Close();
            }



            //解决Firefox等下载文件名乱码问题
            //1、添加编码规则Response.HeaderEncoding Response.ContentEncoding 为 utf-8
            System.Web.HttpContext.Current.Response.HeaderEncoding = Encoding.UTF8;
            System.Web.HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;


            System.Web.HttpContext.Current.Response.ContentType = "application/x-compress zip";
            //2、头部分 Content-Disposition 的设置要按照 rfc231 要求,  应该按照如下格式设置: "Content-Disposition","attachment;filename*=utf-8'zh_cn'文件名.xx"
            //   关键是 filename的设置，*= 后面是 两个单引号，分成三部分（编码 语言 文件名） 如：*=utf-8'zh_cn'文件名.xx 或者 *=utf-8' '文件名.xx

            //在Firefox中，保存时文件名中空格后面的内容会被截断(老的解决方法，不全面)
            //System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + zipFileName + "\"");
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename*=utf-8''{0}", HttpUtility.UrlPathEncode(zipFileName)));


            System.Web.HttpContext.Current.Response.TransmitFile(dir + "/zip/" + zipFileName);
            return true;
        }
    }
}
