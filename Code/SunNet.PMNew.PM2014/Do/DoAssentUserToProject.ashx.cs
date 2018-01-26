using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// Summary description for DoAssentUserToProject
    /// </summary>
    public class DoAssentUserToProject : IHttpHandler
    {
        UserApplication userApp = new UserApplication();
        ProjectApplication proApp = new ProjectApplication();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (IdentityContext.UserID <= 0)
                return;
            UsersEntity userEntity = userApp.GetUser(IdentityContext.UserID);
            if (userEntity == null
                || (userEntity.Role != RolesEnum.ADMIN && userEntity.Role != RolesEnum.PM))
            {
                context.Response.Write("Not authority");
                return;
            }

            string project = context.Request.Form["projectId"];
            int projectId;
            if (!int.TryParse(project, out projectId))
            {
                context.Response.Write("Parameter errors.");
                return;
            }

            string optionType = context.Request.Form["type"] + "";
            optionType = optionType.ToLower();
            if (string.IsNullOrEmpty(optionType.Trim()) ||
                (optionType != "add" && optionType != "del"))
            {
                context.Response.Write("Parameter errors.");
                return;
            }

            string user = context.Request.Form["UserId"];
            int userId;
            if (!int.TryParse(user, out userId))
            {
                context.Response.Write("Parameter errors.");
                return;
            }

            if (optionType == "del")
            {
                proApp.DeleteProjectUser(projectId, userId);
            }
            else
            {
                string client = context.Request.Form["client"] + "";
                if (string.IsNullOrEmpty(client)
                    || (client != "sunnet" && client != "client"))
                {
                    context.Response.Write("Parameter errors.");
                    return;
                }

                UsersEntity assentUserEntity = userApp.GetUser(userId);
                if (assentUserEntity == null)
                {
                    context.Response.Write("Parameter errors.");
                    return;
                }
                ProjectUsersEntity projectuserEntity = new ProjectUsersEntity()
                {
                    ProjectID = projectId,
                    UserID = userId,
                    ISClient = client.Equals("client", StringComparison.CurrentCultureIgnoreCase)
                };
                proApp.AssignUserToProject(projectuserEntity);
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