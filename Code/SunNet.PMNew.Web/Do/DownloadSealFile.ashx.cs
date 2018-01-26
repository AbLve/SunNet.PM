using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using System.Text;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DownloadSealFile : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
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
                System.IO.FileInfo file = new System.IO.FileInfo(sealFileEntity.Path);

                context.Response.AddHeader("Content-Length", file.Length.ToString());

                context.Response.AddHeader("Content-Disposition"
                    , string.Format("attachment;filename={0}", System.Web.HttpUtility.UrlEncode(sealFileEntity.Name.NoHTML(), Encoding.UTF8)));

                switch (file.Extension)
                {
                    case "gif":
                        context.Response.ContentType = "image/gif";
                        break;
                    case "jpg":
                        context.Response.ContentType = "image/jpeg";
                        break;
                    case "bmp":
                        context.Response.ContentType = "image/bmp";
                        break;
                    case "zip":
                        context.Response.ContentType = "application/octet-stream";
                        break;
                    case "rar":
                        context.Response.ContentType = "application/octet-stream";
                        break;
                    case "txt":
                        context.Response.ContentType = "text/plain";
                        break;
                    case "wps":
                        context.Response.ContentType = "application/octet-stream";
                        break;
                    case "doc":
                        context.Response.ContentType = "application/ms-word";
                        break;
                    case "xls":
                        context.Response.ContentType = "application/ms-excel";
                        break;
                    case "swf":
                        context.Response.ContentType = "application/x-shockwave-flash";
                        break;
                    case "ppt":
                        context.Response.ContentType = "application/ms-powerpoint";
                        break;
                    case "fla":
                        context.Response.ContentType = "application/octet-stream";
                        break;
                    case "mp3":
                        context.Response.ContentType = "audio/mp3";
                        break;
                }
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
