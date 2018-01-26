using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.Web.Sunnet.Project
{
    /// <summary>
    /// Summary description for DoAddNote
    /// </summary>
    public class DoAddPrincipal : DoBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            HttpRequest Request = context.Request;
            string type = Request["type"];
            string result = string.Empty;

            if (string.IsNullOrEmpty(type))
            {
                int projectId = QF(Request.Form["ctl00$ContentPlaceHolder1$hdprojectId"], 0);
                result = AddPrincipal(Request, projectId);
            }
            else
            {
                int sealRequestID = QS(Request["sealRequestID"], 0);
                result = GetNotesJson(sealRequestID);
            }
            context.Response.Write(result);
            context.Response.End();
        }


        public string AddPrincipal(HttpRequest Request, int projectId)
        {
            ProjectPrincipalEntity entity = new ProjectPrincipalEntity();
            entity.Module = Request.Form["ctl00$ContentPlaceHolder1$txtModule"].Replace("\n", "<br />");
            entity.PM = Request.Form["ctl00$ContentPlaceHolder1$txtPM"].Replace("\n", "<br />");
            entity.DEV = Request.Form["ctl00$ContentPlaceHolder1$txtDEV"].Replace("\n", "<br />");
            entity.QA = Request.Form["ctl00$ContentPlaceHolder1$txtQA"].Replace("\n", "<br />");
            entity.ProjectID = projectId;
            int count = new ProjectApplication().AddPrincipal(entity);
            if (count > 0)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }


        public string GetNotesJson(int SealRequestsID)
        {
            SealsApplication app = new SealsApplication();
            List<SealNotesEntity> list = app.GetSealNotesList(SealRequestsID);
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("notes", list);
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