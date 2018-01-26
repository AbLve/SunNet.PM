using SunNet.PMNew.App;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for DoAddFile
    /// </summary>
    public class DoAddFile : DoBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            if (Request.ContentLength > Config.MaxRequestLength)
            {
                context.Response.Write("3");
                context.Response.End();
            }
            string type = Request["type"];
            string result = string.Empty;

            if (string.IsNullOrEmpty(type))
            {
                int sealRequestID = QF(Request.Form["ctl00$ContentPlaceHolder1$hdSealRequestID"], 0);
                result = AddFile(Request, sealRequestID);
            }
            else
            {
                int sealRequestID = QS(Request["sealRequestID"], 0);
                result = GetFielsJson(sealRequestID);
            }

            context.Response.Write(result);
            context.Response.End();
        }

        public string AddFile(HttpRequest Request, int sealRequestID)
        {
            if (Request.Files.Count == 0)
            {
                return "0";
            }
            else
            {
                string fileTitle = Request.Form["ctl00$ContentPlaceHolder1$txtFileTitle"];
                string fileName = Request.Files[0].FileName;
                string tmpFileName = string.Format("{0}{2}{1}", DateTime.Now.ToString("MMddyyHHmmss"), fileName.Substring(fileName.LastIndexOf(".")), 1);
                Request.Files[0].SaveAs(Config.SealFilePath + tmpFileName);
                SealFileEntity fileEntity = new SealFileEntity();
                fileEntity.Title = fileTitle;
                fileEntity.Name = fileName;
                fileEntity.Path = Config.SealFilePath + tmpFileName;
                fileEntity.SealRequestsID = sealRequestID;
                fileEntity.UserID = UserID;
                fileEntity.Type = 2;
                fileEntity.IsDeleted = false;
                fileEntity.CreateOn = DateTime.Now;
                new App.SealsApplication().SealFilesInsert(fileEntity);
                return "1";
            }
        }

        public string GetFielsJson(int SealRequestsID)
        {
            SealsApplication app = new SealsApplication();
            SealRequestsEntity sealRequestsEntity = app.GetSealRequests(SealRequestsID);
            string tmpFiles = string.Empty;
            List<SealFileEntity> list = app.GetSealFilesList(SealRequestsID);
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("files", list.FindAll(r => r.Type == 2));
            result.Add("currentUserID", UserID);
            result.Add("NotShowDelete", sealRequestsEntity.Status >= RequestStatus.Approved);
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
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