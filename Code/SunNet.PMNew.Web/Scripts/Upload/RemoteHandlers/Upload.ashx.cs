using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Framework.Utils;

namespace UploadifyDemo.RemoteHandlers
{

    public class Upload : IHttpHandler
    {
        ProjectApplication projectApp = new ProjectApplication();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;
            try
            {
                string FolderName = "";

                string pid = context.Request["pid"];//get project id

                ProjectsEntity entity = projectApp.Get(Convert.ToInt32(pid));

                if (null != entity)
                {
                    FolderName = entity.ProjectID.ToString();
                }
                else
                {
                    context.Response.StatusCode = 500;
                    context.Response.StatusDescription = "ss";
                    return;
                }

                HttpPostedFile postedFile = context.Request.Files["Filedata"];

                string savepath = "";
                string tempPath = "";
                tempPath = System.Configuration.ConfigurationManager.AppSettings["FolderPath"];
                savepath = context.Server.MapPath(tempPath) + FolderName;
                string filename = postedFile.FileName;
                string sExtension = Path.GetExtension(filename);

                if (!Directory.Exists(savepath))
                    Directory.CreateDirectory(savepath);

                string sNewFileName = FolderName + filename;//DateTime.Now.ToString("yyyyMMddhhmmsfff");
                if (File.Exists(savepath + @"\" + sNewFileName))
                {
                    context.Response.Write("0");
                    return;
                }
                postedFile.SaveAs(savepath + @"\" + sNewFileName);
                context.Response.Write(tempPath + sNewFileName);
                context.Response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                context.Response.Write("Error: " + ex.Message);
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
