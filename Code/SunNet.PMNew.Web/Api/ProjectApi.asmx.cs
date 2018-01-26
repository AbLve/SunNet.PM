using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Web.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SunNet.PMNew.Web.Api
{
    /// <summary>
    /// ProjectApi 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ProjectApi : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetClientTopMenu(int userId)
        {
            UsersEntity userInfo = new UserApplication().GetUser(userId);
            UserApplication userApp = new UserApplication();
            List<ModulesEntity> list = userApp.GetRoleModules(userInfo.RoleID, true);
            List<ModulesEntity> listTop = list.FindAll(m => m.ParentID == 1 && m.ShowInMenu);
            if (userInfo.Role == RolesEnum.CLIENT) // if current role is client then change Clients modules' module name to Tickets
            {
                ModulesEntity modulesEntity = listTop.Find(r => r.ModuleTitle.Trim().ToLower() == "clients");
                if (modulesEntity != null)
                {
                    modulesEntity.ModuleTitle = "Tickets";
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(listTop);
        }

        [WebMethod]
        public string GetProject(int userId)
        {
            ProjectApplication projApp = new ProjectApplication();
            UsersEntity user = new UserApplication().GetUser(userId);
            IOrderedEnumerable<ProjectDetailDTO> list = projApp.GetUserProjects(user).OrderBy(r => r.Title);
            var tempList = list.Select(e => new { e.ProjectID, ProjectName = e.Title });
            return Newtonsoft.Json.JsonConvert.SerializeObject(tempList);
        }
    }
}
