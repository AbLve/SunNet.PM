using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;


namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoAddRelationTickets : IHttpHandler
    {
        TicketsRelationApplication trAPP = new TicketsRelationApplication();

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (IdentityContext.UserID <= 0)
                    return;

                Dictionary<int, string> ErrorMsg = new Dictionary<int, string>();

                context.Response.ContentType = "text/plain";

                String checkboxList = context.Request["checkboxList"];

                String tid = context.Request["tid"];

                TicketsRelationDTO TRDTO = new TicketsRelationDTO();

                AddTicketsRelationRequest request = new AddTicketsRelationRequest();

                AddTicketsRelationResponse response = null;

                string[] tidArray = checkboxList.Split(',');

                bool IsError = true;

                foreach (string item in tidArray)
                {
                    if (item.Length > 0)
                    {
                        TRDTO.RTID = Convert.ToInt32(item);

                        TRDTO.TID = Convert.ToInt32(tid);

                        TRDTO.CreatedBy = IdentityContext.UserID; 

                        request.dto = TRDTO;

                        response = trAPP.AddTR(request);

                        if (response.AddSuc > 0)
                        {
                            ErrorMsg.Add(Convert.ToInt32(item), "Suc");
                        }
                        else
                        {
                            ErrorMsg.Add(Convert.ToInt32(item), "Fail");
                            IsError = false;
                        }
                    }
                }
                if (!IsError)
                {
                    context.Response.Write(ErrorMsg.Count + "Files Add Not Successful!");
                }
                context.Response.Write("Add Success!");
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoAddRelationTickets.ashx Messages:\r\n{0}", ex));
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
