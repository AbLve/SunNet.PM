using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// AssignProjectsToUser 的摘要说明
    /// </summary>
    public class DoAssignProjectsToUser : IHttpHandler
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
                #region 获取已经分配了的project 
                SearchProjectsRequest request = new SearchProjectsRequest(SearchProjectsType.ListByUserID
               , false, "projectId", "ASC"); 
                request.UserID = uid;
                SearchProjectsResponse projectList = new ProjectApplication().SearchProjects(request);
                for (int i = 0; i < projectIds.Length; i++)
                {
                    int projectId = 0;
                    int.TryParse(projectIds[i],out projectId);
                    ProjectDetailDTO project = projectList.ResultList.Find(t => t.ProjectID == projectId);
                    if (project != null && project.ProjectID !=0)
                    {
                        context.Response.Write("some of the selected projects have been assigned to this user!"); 
                        return;
                    }
                }

                #endregion

                List<BrokenRuleMessage> listmsgs = new List<BrokenRuleMessage>();
                foreach (string projectId in projectIds)
                {
                    ProjectUsersEntity model = ProjectsFactory.CreateProjectUser(IdentityContext.UserID
                        , ObjectFactory.GetInstance<ISystemDateTime>());
                    model.ProjectID = int.Parse(projectId);
                    model.UserID = uid;
                    model.ISClient = Boolean.Parse(isClient);

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