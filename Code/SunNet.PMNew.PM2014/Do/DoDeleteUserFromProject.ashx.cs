using SunNet.PMNew.App;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// Summary description for DoDeleteUserFromProject
    /// </summary>
    public class DoDeleteUserFromProject : DoBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            int projectID = QS(request.Form["projectid"], 0);
            int userToEdit = QS(request.Form["userToEdit"], 0);
            if (projectID != 0 && userToEdit != 0)
            {
                ProjectApplication projectApplication = new ProjectApplication();
                if (projectApplication.DeleteProjectUser(projectID, userToEdit))
                {
                    response.Write("1");
                }
                else
                {
                    response.Write("0");
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