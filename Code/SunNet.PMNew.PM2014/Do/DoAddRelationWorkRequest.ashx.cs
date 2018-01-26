using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// Summary description for DoAddRelationWorkRequest
    /// </summary>
    public class DoAddRelationWorkRequest : IHttpHandler
    {

        ProposalTrackerApplication app = new ProposalTrackerApplication();

        public void ProcessRequest(HttpContext context)
        {
            if (IdentityContext.UserID <= 0)
                return;
            UsersEntity userEntity = new App.UserApplication().GetUser(IdentityContext.UserID);
            if (userEntity.Role != RolesEnum.ADMIN && userEntity.Role != RolesEnum.PM && userEntity.Role != RolesEnum.Sales)
                return;

            context.Response.ContentType = "text/plain";

            String checkboxList = context.Request["checkboxList"] + "";
            String wid = context.Request["wid"];
            int proposaltrackerId;
            if (!int.TryParse(wid, out proposaltrackerId))
                return;

            checkboxList = checkboxList.Trim();
            if (string.IsNullOrEmpty(checkboxList))
                return;

            ProposalTrackerRelationEntity WRDTO = new ProposalTrackerRelationEntity();
            if (checkboxList.EndsWith(","))
                checkboxList = checkboxList.Remove(checkboxList.Length - 1);


            string[] tidArray = checkboxList.Split(',');

            foreach (string item in tidArray)
            {
                if (item.Length > 0)
                {
                    WRDTO.WID = Convert.ToInt32(wid);

                    WRDTO.TID = Convert.ToInt32(item);

                    WRDTO.CreatedBy = IdentityContext.UserID;

                    app.AddProposalTrackerRelation(WRDTO);
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