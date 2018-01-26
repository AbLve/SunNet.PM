using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SF.Framework.File;
using System.Transactions;
using FamilyBook.Common;

namespace FamilyBook.Web.Channels
{
    /// <summary>
    /// Upload 的摘要说明
    /// </summary>
    public class UploadDocument : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string obj = context.Request.QueryString["userId"];
            string projectId = context.Request.QueryString["projectId"];
            int userId = 0;
            int.TryParse(obj, out userId);
            string result = "false";

            HttpPostedFile file = context.Request.Files[0];
            string url = SF.Framework.SFConfig.FilePhysicalUrl + "/upload/Project/" + projectId;
            result = FileHelper.Upload(file, "/upload/Project/" + projectId, url);
            context.Response.Write(result);
            context.Response.End();
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