using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel.ProjectTicket;

namespace SunNet.PMNew.Core.ProjectModule
{
    public interface IProjectsRepository : IRepository<ProjectsEntity>
    {
        SearchProjectsResponse SearchProjects(SearchProjectsRequest request);
        bool CheckExistsTitle(string title, int exceptThis);
        bool CheckExistsCode(string code, int exceptThis);
        List<UsersEntity> GetPojectClientUsers(int projectId, int companyId);
        List<UsersEntity> GetPojectPmUsers(int projectId, int companyId);
        List<int> GetProjectIdByClientID(int userId);
        float GetProjectTimeSheetTime(int projectId);
        bool updateRemainHoursSendEmailStatus(bool hasSend, int projectId);
        List<ProjectTicketModel> GetProjectTicketList(bool internalProject, int userId);
    }
}
