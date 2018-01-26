using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using System.IO;
using SunNet.PMNew.Framework.Utils.Helpers;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DoDownloadFileHandler : IHttpHandler
    {
        FileApplication fileApp = new FileApplication();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string strFileID = context.Request.QueryString["FileID"];
            string strSize = context.Request.QueryString["size"];
            string tableType = context.Request.QueryString["tableType"];
            int fileId = 0;
            if (!int.TryParse(strFileID, out fileId))
            {
                context.Response.Write("File not found.");
                return;
            }


            string filename = "";
            string fileurl = "";

            int tableTypeValue = 0;
            int.TryParse(tableType, out tableTypeValue);
            if (tableTypeValue == 2)
            {
                SunNet.PMNew.PM2014.OperateDocManagements.OperateDocManagementSoapClient client = new SunNet.PMNew.PM2014.OperateDocManagements.OperateDocManagementSoapClient();
                string str = client.GetFileInfo(fileId);
                filename = str.Substring(str.IndexOf("|") + 1);
                fileurl = str.Substring(0, str.IndexOf("|"));
            }
            else
            {
                FilesEntity fileEntity = fileApp.Get(fileId);
                filename = fileEntity.FileTitle;
                fileurl = fileEntity.FilePath;
            }
            if (fileurl == "")
            {
                context.Response.Write("File not found.");
                return;
            }

            string filePath = HttpContext.Current.Server.MapPath(string.Format("/{0}", fileurl));
            if (System.IO.File.Exists(filePath))
            {
                if (!filename.ToLower().EndsWith(Path.GetExtension(filePath).ToLower())) //检查filename 是否带了扩展名
                {
                    filename += Path.GetExtension(filePath);
                }
                
                System.IO.FileInfo file = new System.IO.FileInfo(filePath);

                switch (file.Extension.ToLower())
                {
                    case ".png":
                        context.Response.ContentType = "image/png";
                        ShowImage(context, filePath, ".png");
                        return;
                    case ".gif":
                        context.Response.ContentType = "image/GIF";
                        ShowImage(context, filePath, ".gif");
                        return;
                    case ".jpg":
                    case ".jpeg":
                        context.Response.ContentType = "image/jpeg";
                        ShowImage(context, filePath, ".jpg");
                        return;
                    case ".bmp":
                        context.Response.ContentType = "image/bmp";
                        ShowImage(context, filePath, ".bmp");
                        return;
                    case ".zip":
                        context.Response.ContentType = "application/octet-stream";
                        break;
                    case ".rar":
                        context.Response.ContentType = "application/octet-stream";
                        break;
                    case ".txt":
                        context.Response.ContentType = "text/plain";
                        break;
                    case ".wps":
                        context.Response.ContentType = "application/octet-stream";
                        break;
                    case ".doc":
                        context.Response.ContentType = "application/ms-word";
                        break;
                    case ".xls":
                        context.Response.ContentType = "application/ms-excel";
                        break;
                    case ".swf":
                        context.Response.ContentType = "application/x-shockwave-flash";
                        break;
                    case ".ppt":
                        context.Response.ContentType = "application/ms-powerpoint";
                        break;
                    case ".fla":
                        context.Response.ContentType = "application/octet-stream";
                        break;
                    case ".mp3":
                        context.Response.ContentType = "audio/mp3";
                        break;
                    default:
                        context.Response.ContentType = "text/plain";
                        break;
                }

                //解决Firefox等下载文件名乱码问题
                //1、添加编码规则Response.HeaderEncoding Response.ContentEncoding 为 utf-8
                context.Response.HeaderEncoding = System.Text.Encoding.UTF8;
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;

                context.Response.AddHeader("Content-Length", file.Length.ToString());
                //context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", System.Web.HttpUtility.UrlEncode(filename.NoHTML(), Encoding.UTF8)));

                //2、头部分 Content-Disposition 的设置要按照 rfc231 要求,  应该按照如下格式设置: "Content-Disposition","attachment;filename*=utf-8'zh_cn'文件名.xx"
                //   关键是 filename的设置，*= 后面是 两个单引号，分成三部分（编码 语言 文件名） 如：*=utf-8'zh_cn'文件名.xx 或者 *=utf-8' '文件名.xx
                context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename*=utf-8''{0}", HttpUtility.UrlPathEncode(filename.NoHTML())));
                context.Response.TransmitFile(filePath);
                context.Response.End();



            }
            else
            {
                context.Response.Write("File not found.");
                return;
            }
        }

        private void ShowImage(HttpContext context, string path, string extension)
        {
            Bitmap bitmap, bitmap2;
            System.Drawing.Graphics g;
            Image img;
            switch (extension)
            {
                case ".png":
                    bitmap = (Bitmap)Image.FromFile(path);
                    bitmap2 = new Bitmap(bitmap.Width, bitmap.Height);
                    g = Graphics.FromImage(bitmap2);
                    g.DrawImageUnscaled(bitmap, 0, 0);

                    g.Dispose();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap2.Save(ms, ImageFormat.Png);
                        bitmap2.Dispose();
                        ms.WriteTo(context.Response.OutputStream);
                    }
                    break;
                case ".gif":
                     byte[] imageBytes = null;
                    img = Image.FromFile(path);
                    Graphics graphic;
                    graphic = Graphics.FromImage(img);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        img.Save(memoryStream, ImageFormat.Gif);
                        imageBytes = memoryStream.ToArray();
                    }
                    context.Response.OutputStream.Write(imageBytes, 0, imageBytes.Length);
                    context.Response.End();
                    break;
                case ".jpg":
                    bitmap = new Bitmap(path);
                    g = System.Drawing.Graphics.FromImage(bitmap);
                    g.Dispose();
                    bitmap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);
                    bitmap.Dispose();
                    break;
                case ".bmp":
                    bitmap = (Bitmap)Image.FromFile(path);
                    bitmap2 = new Bitmap(bitmap.Width, bitmap.Height);
                    g = Graphics.FromImage(bitmap2);
                    g.DrawImageUnscaled(bitmap, 0, 0);
                    g.Dispose();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap2.Save(ms, ImageFormat.Png);
                        bitmap2.Dispose();
                        ms.WriteTo(context.Response.OutputStream);
                    }
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
