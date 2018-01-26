using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.ProjectModule
{
    public interface IProjectUsersRepository : IRepository<ProjectUsersEntity>
    {

        List<ProjectUsersEntity> GetProjectSunnetUserList(int projectID);

        bool Delete(int projectId, int userId);

        List<int> GetUserIdByProjectId(int projectId);

        List<int> GetActiveUserIdByProjectId(int projectId);

        /// <summary>
        /// 获取与User同一个项目下的用户ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<int> GetProjectUserIds(int userId);
    }
}
