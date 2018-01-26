using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SunNet.PMNew.PM2014.Do.Event
{
    /// <summary>
    /// Summary description for GetUsersByProjectId
    /// </summary>
    public class GetUsersByProjectId : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (IdentityContext.UserID <= 0)
            {
                context.Response.Write("[]");
                return;
            }

            int projectId;
            if (!int.TryParse(context.Request.QueryString["projectId"] + "", out projectId))
            {
                context.Response.Write("[]");
                return;
            }

            UsersEntity user = new App.UserApplication().GetUser(IdentityContext.UserID);

            ProjectsEntity projectEntity = new App.ProjectApplication().Get(projectId);

            List<int> listUserId = new App.ProjectApplication().GetActiveUserIdByProjectId(projectId);

            if (listUserId == null || listUserId.Count == 0)
            {
                context.Response.Write("[]");
                return;
            }
            if (!listUserId.Contains(user.UserID))
            {
                context.Response.Write("[]");
                return;
            }
            else
                listUserId.Remove(user.UserID);

            if (user.Office == "CN" && user.UserType == "SUNNET")//山诺 上海的员工只能获取本公司的项目的相关人员
            {       
                if (projectEntity.CompanyID == Config.SunnetCompany)
                {                   
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[");
                    int index = 0;
                    foreach (int tmpId in listUserId)
                    {
                        UsersEntity tmpUser = new App.UserApplication().GetUser(tmpId);
                        if (tmpUser == null) continue;
                        if (tmpUser.UserType == "SUNNET")
                        {
                            CompanysEntity companyEntity = new App.CompanyApplication().GetCompany(tmpUser.CompanyID);
                            if (index >0)
                                sb.Append(",");
                            index++;
                            sb.Append("{");
                            sb.AppendFormat("\"UserID\":\"{0}\"", tmpUser.UserID);
                            sb.AppendFormat(",\"FirstAndLastName\":\"{0}\"", tmpUser.FirstAndLastName);
                            sb.AppendFormat(",\"LastNameAndFirst\":\"{0}\"", tmpUser.LastNameAndFirst);
                            sb.AppendFormat(",\"CompanyName\":\"{0}\"", companyEntity.CompanyName);
                            sb.AppendFormat(",\"Title\":\"{0}\"", tmpUser.Title);
                            sb.Append("}");
                        }
                    }
                    sb.Append("]");
                    context.Response.Write(sb.ToString());
                    return;
                }
                else
                {
                    context.Response.Write("[]");
                    return;
                }
            }
            else
            {
                if (user.Role == RolesEnum.CLIENT || user.Role == RolesEnum.ADMIN || user.Role == RolesEnum.PM || user.Role == RolesEnum.Sales || user.Role == RolesEnum.Contactor)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[");
                    int index = 0;
                    foreach (int tmpId in listUserId)
                    {
                        UsersEntity tmpUser = new App.UserApplication().GetUser(tmpId);
                        if (tmpUser == null) continue;
                        if (projectEntity.CompanyID == Config.SunnetCompany)
                        {
                            CompanysEntity companyEntity = new App.CompanyApplication().GetCompany(tmpUser.CompanyID);
                            if (index > 0)
                                sb.Append(",");
                            index++;
                            sb.Append("{");
                            sb.AppendFormat("\"UserID\":\"{0}\"", tmpUser.UserID);
                            sb.AppendFormat(",\"FirstAndLastName\":\"{0}\"", tmpUser.FirstAndLastName);
                            sb.AppendFormat(",\"LastNameAndFirst\":\"{0}\"", tmpUser.LastNameAndFirst);
                            sb.AppendFormat(",\"CompanyName\":\"{0}\"", companyEntity.CompanyName);
                            sb.AppendFormat(",\"Title\":\"{0}\"", tmpUser.Title);
                            sb.Append("}");
                        }
                        else
                        {
                            if (tmpUser.Role == RolesEnum.CLIENT || tmpUser.Role == RolesEnum.ADMIN || tmpUser.Role == RolesEnum.PM || tmpUser.Role == RolesEnum.Sales || user.Role == RolesEnum.Contactor)
                            {
                                CompanysEntity companyEntity = new App.CompanyApplication().GetCompany(tmpUser.CompanyID);
                                if (index > 0)
                                    sb.Append(",");
                                index++;
                                sb.Append("{");
                                sb.AppendFormat("\"UserID\":\"{0}\"", tmpUser.UserID);
                                sb.AppendFormat(",\"FirstAndLastName\":\"{0}\"", tmpUser.FirstAndLastName);
                                sb.AppendFormat(",\"LastNameAndFirst\":\"{0}\"", tmpUser.LastNameAndFirst);
                                sb.AppendFormat(",\"CompanyName\":\"{0}\"", companyEntity.CompanyName);
                                sb.AppendFormat(",\"Title\":\"{0}\"", tmpUser.Title);
                                sb.Append("}");
                            }
                        }                        
                    }
                    sb.Append("]");
                    context.Response.Write(sb.ToString());
                    return;
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