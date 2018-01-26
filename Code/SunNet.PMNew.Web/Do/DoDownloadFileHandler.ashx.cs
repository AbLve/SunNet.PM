using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using System.IO;
using SunNet.PMNew.Framework.Utils.Helpers;

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
                OperateDocManagements.OperateDocManagementSoapClient client = new OperateDocManagements.OperateDocManagementSoapClient();
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
                if (filename.IndexOf(".") < 0)
                {
                    filename += Path.GetExtension(filePath);
                }
                filename = filename.Replace(" ", "_");
                context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename.NoHTML()));
                context.Response.TransmitFile(filePath);
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
