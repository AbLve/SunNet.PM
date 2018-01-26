using SunNet.PMNew.App;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.Web.Sunnet.Profile
{
    /// <summary>
    /// Summary description for DoAddNote
    /// </summary>
    public class DoAddNote : DoBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            HttpRequest Request = context.Request;
            string type = Request["type"];
            string result = string.Empty;

            if (string.IsNullOrEmpty(type))
            {
                int sealRequestID = QF(Request.Form["ctl00$ContentPlaceHolder1$hdSealRequestID"], 0);
                result = AddNote(Request, sealRequestID);
            }
            else
            {
                int sealRequestID = QS(Request["sealRequestID"], 0);
                result = GetNotesJson(sealRequestID);
            }
            context.Response.Write(result);
            context.Response.End();
        }


        public string AddNote(HttpRequest Request, int sealRequestID)
        {
            SealNotesEntity entity = new SealNotesEntity();
            entity.Title = Request.Form["ctl00$ContentPlaceHolder1$txtNoteTitle"];
            entity.Description = Request.Form["ctl00$ContentPlaceHolder1$txtNoteDescription"].Replace("\r\n", "<br>");
            entity.SealRequestsID = sealRequestID;
            entity.CreateOn = DateTime.Now;
            entity.UserID = UserID;
            int count = new App.SealsApplication().InsertSealNotes(entity);
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