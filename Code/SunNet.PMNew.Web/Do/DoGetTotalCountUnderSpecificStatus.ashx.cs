using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>

    public class DoGetTotalCountUnderSpecificStatus : IHttpHandler
    {
        TicketsApplication ticketApp = new TicketsApplication();

        private string GetListCount()
        {
            List<int> list = new List<int>();

            list.Add(ticketApp.GetTotalCountByStatus(TotalCountByStatus.UnderDevelopingTotalCount, IdentityContext.UserID));
            list.Add(ticketApp.GetTotalCountByStatus(TotalCountByStatus.UnderTestTotalCount, IdentityContext.UserID));
            list.Add(ticketApp.GetTotalCountByStatus(TotalCountByStatus.UnderCoordination, IdentityContext.UserID));

            string jsonlist = UtilFactory.Helpers.JSONHelper.GetJson<List<int>>(list);
            return jsonlist;
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (IdentityContext.UserID <= 0)
                return;
            context.Response.Write(GetListCount());
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
