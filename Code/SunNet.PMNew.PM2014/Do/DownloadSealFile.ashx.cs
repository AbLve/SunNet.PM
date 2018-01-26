using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Framework.Utils.Helpers;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// Summary description for DownloadSealFile
    /// </summary>
    public class DownloadSealFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (IdentityContext.UserID <= 0)
                return;

            string id = context.Request.QueryString["id"];
            int fileId = 0;
            if (!int.TryParse(id, out fileId))
            {
                context.Response.Write("File not found.");
                return;
            }

            SealFileEntity sealFileEntity = new App.SealsApplication().GetSealFiles(fileId);

            if (sealFileEntity == null)
            {
                context.Response.Write("File not found.");
                return;
            }
            List<int> list = new App.SealsApplication().GetUsersId(sealFileEntity.SealRequestsID);
            if (!list.Contains(IdentityContext.UserID))
            {
                context.Response.Write("unauthorized access.");
                return;
            }

            if (System.IO.File.Exists(sealFileEntity.Path))
            {
                context.Response.Clear();
                System.IO.FileInfo file = new System.IO.FileInfo(sealFileEntity.Path);
                switch (file.Extension.ToLower())
                {
                    case ".png":
                        context.Response.ContentType = "image/png";
                        ShowImage(context, sealFileEntity.Path, ".png");
                        return;
                    case ".gif":
                        context.Response.ContentType = "image/GIF";
                        ShowImage(context, sealFileEntity.Path, ".gif");
                        return;
                    case ".jpg":
                    case ".jpeg":
                        context.Response.ContentType = "image/jpeg";
                        ShowImage(context, sealFileEntity.Path, ".jpg");
                        return;
                    case ".bmp":
                        context.Response.ContentType = "image/bmp";
                        ShowImage(context, sealFileEntity.Path, ".bmp");
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

                context.Response.AddHeader("Content-Length", file.Length.ToString());

                context.Response.AddHeader("Content-Disposition"
                    , string.Format("attachment;filename={0}", System.Web.HttpUtility.UrlEncode(sealFileEntity.Name.NoHTML(), Encoding.UTF8)));


                context.Response.TransmitFile(sealFileEntity.Path);
                context.Response.End();
                return;
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
                    bitmap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
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