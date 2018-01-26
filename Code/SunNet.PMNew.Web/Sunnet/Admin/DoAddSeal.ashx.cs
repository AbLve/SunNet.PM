using SunNet.PMNew.App;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Sunnet.Admin
{

    /// <summary>
    /// Summary description for DoAddSeal
    /// </summary>
    public class DoAddSeal : DoBase, IHttpHandler
    {
        SealsApplication app = new SealsApplication();

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string hdID = request.Form["ctl00$ContentPlaceHolder1$hdID"];
            string sealName = request.Form["ctl00$ContentPlaceHolder1$txtSealName"];
            string owner = request.Form["ctl00$ContentPlaceHolder1$ddlOwner"];
            string approver = request.Form["ctl00$ContentPlaceHolder1$ddlApprover"];
            string description = request.Form["ctl00$ContentPlaceHolder1$txtDescription"];
            string status = request.Form["ctl00$ContentPlaceHolder1$ddlStatus"];
            string result = Save(hdID, sealName, owner, approver, description, status);
            response.Write(result);
            response.End();
        }


        protected string Save(string hdID, string sealName, string owner, string approver, string description, string status)
        {
            SealsEntity sealsEntity = new SealsEntity();
            int id;
            if (int.TryParse(hdID, out id))
            {
                sealsEntity.ID = id;
            }
            else
            {
                sealsEntity.CreatedOn = DateTime.Now;
            }

            sealsEntity.SealName = sealName.Trim().NoHTML();
            sealsEntity.Owner = int.Parse(owner);
            sealsEntity.Approver = int.Parse(approver);
            sealsEntity.Description = description.Trim().NoHTML();
            sealsEntity.Status = (Status)Enum.Parse(typeof(Status), status);
            if (app.CheckSealName(sealsEntity.ID, sealsEntity.SealName))
            {
                //ShowFailMessageToClient("Seal Name already exists.");

                return "0";
            }
            if (sealsEntity.ID > 0) //edit
            {
                if (app.Update(sealsEntity))
                {
                    //ShowSuccessMessageToClient(true, true);
                    return "1";
                }
                else
                {
                    //ShowFailMessageToClient(app.BrokenRuleMessages);
                    return "2";
                }
            }
            else  //insert 
            {
                if (app.Insert(sealsEntity) > 0)
                {
                    //ShowSuccessMessageToClient();
                    return "1";
                }
                else
                {
                    //ShowFailMessageToClient(app.BrokenRuleMessages);
                    return "2";
                }
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