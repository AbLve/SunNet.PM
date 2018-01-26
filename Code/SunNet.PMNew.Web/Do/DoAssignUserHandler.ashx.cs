using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoAssignUserHandler : IHttpHandler
    {
        #region declare
        UserApplication userApp = new UserApplication();
        TicketsApplication ticketApp = new TicketsApplication();
        TicketStatusManagerApplication ticketStatusMgr = new TicketStatusManagerApplication();
        int userID = 0;
        bool enableEmail = !Config.IsTest;
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            try
            {
                if (IdentityContext.UserID <= 0)
                {
                    return;
                }
                String checkboxList = context.Request["checkboxList"];
                String tid = context.Request["tid"];
                String type = context.Request["type"];
                List<string> userArray = new List<string>();
                List<string> userids = new List<string>();
                List<string> userTypes = new List<string>();


                if (!string.IsNullOrEmpty(checkboxList))
                {
                    userArray = checkboxList.Split(',').ToList();
                }

                foreach (string user in userArray)
                {
                    string[] values = user.Split('|');
                    userids.Add(values[0]);
                    userTypes.Add(values[1]);
                }

                IEnumerable<string> distinctedUserTypes = userTypes.Distinct();
                ticketApp.RemoveAllTicketUsersExceptPM(Convert.ToInt32(tid));//remove all ticket user
                TicketUsersEntity entity = new TicketUsersEntity();

                entity.TicketID = Convert.ToInt32(tid);
                int result = 0;
                bool HasError = false;
                for (int i = 0; i < userids.Count; i++)
                {
                    if (userids[i].Length > 0)
                    {
                        userID = Convert.ToInt32(userids[i]);
                        entity.UserID = userID;
                        entity.Type = (TicketUsersType)int.Parse(userTypes[i] + "");
                        result = ticketApp.AddTicketUser(entity);
                        if (result > 0)
                        {
                            if (enableEmail)
                            {
                                ticketStatusMgr.SendEmailToAssignedUser(entity);
                            }
                        }
                        else
                        {
                            HasError = true;
                        }
                    }

                }
                if (!HasError)
                {
                    context.Response.Write("User Assign Successful!");
                }
                else
                {
                    context.Response.Write("User Assign Fail!");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoAssignUserHandler.ashx Messages:\r\n{0}", ex));
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
