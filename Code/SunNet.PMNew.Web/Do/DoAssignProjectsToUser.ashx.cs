using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.App;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>

    public class AssignProjectsToUser : IHttpHandler
    {
        ProjectApplication projApp = new ProjectApplication();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            try
            {
                if (IdentityContext.UserID <= 0)
                {
                    return;
                }
                string isClient = context.Request["isClient"];
                string projectIdList = context.Request["checkboxList"];
                int uid = int.Parse(context.Request["uid"]);
                string[] projectIds = projectIdList.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                //这里原本传的是project的Id而我只需要保证弹出的窗口中没有已选择的project就ok了

                List<BrokenRuleMessage> listmsgs = new List<BrokenRuleMessage>();
                foreach (string projectId in projectIds)
                {
                    ProjectUsersEntity model = ProjectsFactory.CreateProjectUser(IdentityContext.UserID
                        , ObjectFactory.GetInstance<ISystemDateTime>());
                    model.ProjectID =int.Parse(projectId);
                    model.UserID =uid;
                    model.ISClient =Boolean.Parse(isClient);
                    if (projApp.AssignUserToProject(model) < 0)
                    {
                        RecordMsg(listmsgs, projApp.BrokenRuleMessages);
                    }
                }
                if (listmsgs.Count > 0)
                {
                    context.Response.Write("Assign Fail!");
                }
                else
                {
                    context.Response.Write("The project has been assigned.");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("Input special symbol is not allowed,please check title and description!");
                WebLogAgent.Write(string.Format("Error Ashx:DoAddTicketHandler.ashx Messages:\r\n{0}", ex));
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

        private void RecordMsg(List<BrokenRuleMessage> listmsgs, List<BrokenRuleMessage> listBrokenMsgs)
        {
            foreach (BrokenRuleMessage msg in listBrokenMsgs)
            {
                listmsgs.Add(msg);
            }
        }
    }
}
