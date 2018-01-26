using Newtonsoft.Json;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.UserModel.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.PM2014.Service
{
    /// <summary>
    /// User 的摘要说明
    /// </summary>
    public class User : DoBase, IHttpHandler
    {
        UserApplication userApp = new UserApplication();
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            context.Response.ContentType = "application/json";
            string action = context.Request.Params["action"].ToLower();
            int currentUserId = int.Parse(context.Request.Params["currentuserid"]);
            switch (action)
            {
                case "getdashboardusers":
                    context.Response.Write(GetDashboardUsers(currentUserId));
                    break;
                case "savehideuser":
                    string hideUserIds = context.Request.Params["hideuserids"];
                    context.Response.Write(SaveHideUser(currentUserId, hideUserIds) ? "1" : "0");
                    break;
                default:
                    Response.Write("[]");
                    break;
            }
        }

        public string GetDashboardUsers(int currentUserId)
        {
            List<RolesEnum> roles = new List<RolesEnum>();
            roles.Add(RolesEnum.PM);
            roles.Add(RolesEnum.DEV);
            roles.Add(RolesEnum.QA);
            roles.Add(RolesEnum.Leader);
            List<DashboardUserModel> userModels = userApp.GetUserByRoles(roles);
            HideUserEntity hideUser = userApp.GetHideUserByUserId(currentUserId);
            if (hideUser != null)
            {
                List<string> userIds = hideUser.HideUserIds.Split(',').ToList();
                if (userIds.Count > 0)
                {
                    userModels.ForEach(e =>
                    {
                        if (userIds.Contains(e.UserID.ToString()))
                        {
                            e.IsHide = true;
                        }
                    });
                }
            }
            string strUserModels = JsonConvert.SerializeObject(userModels);
            return strUserModels;
        }

        private bool SaveHideUser(int currentUserId, string hideUserIds)
        {
            bool isSuccess = false;
            HideUserEntity entity = userApp.GetHideUserByUserId(currentUserId);
            if (entity == null)
            {
                entity = UsersFactory.CreateHideUserEntity(currentUserId, ObjectFactory.GetInstance<ISystemDateTime>());
                entity.UserID = currentUserId;
                entity.HideUserIds = hideUserIds;
                int count = userApp.AddHideUsers(entity);
                isSuccess = count > 0;
                return isSuccess;
            }
            entity.HideUserIds = hideUserIds;
            return userApp.UpdateHideUsers(entity);
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