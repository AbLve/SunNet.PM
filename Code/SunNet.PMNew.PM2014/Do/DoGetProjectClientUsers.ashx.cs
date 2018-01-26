using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// Summary description for DoGetProjectClientUsers
    /// </summary>
    public class DoGetProjectClientUsers : DoBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            HttpRequest request = context.Request;
            int projectID = QS(request.QueryString["projectID"], 0);
            if (projectID != 0)
            {
                ProjectApplication projectApplication = new ProjectApplication();

                List<UsersEntity> clientUsers = projectApplication.GetPojectClientUsers(projectID, projectApplication.Get(projectID).CompanyID);

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("[");
                foreach (UsersEntity user in clientUsers)
                {
                    stringBuilder.Append("{");
                    stringBuilder.AppendFormat("\"name\":\"{0}\",\"value\":\"{1}\"", user.FirstAndLastName,
                        user.UserID);
                    stringBuilder.Append("},");
                }
                response.Write(stringBuilder.ToString().TrimEnd(',') + "]");
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