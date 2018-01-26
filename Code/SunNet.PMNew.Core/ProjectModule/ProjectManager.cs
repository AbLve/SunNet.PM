using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Core.Notify;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Core.Validator;

using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Core.ProjectModule.Interfaces;
using SunNet.PMNew.Entity.ProjectModel.ProjectTicket;

namespace SunNet.PMNew.Core.ProjectModule
{

    public class ProjectManager : BaseMgr
    {
        IEmailSender emailSender;
        ICache<ProjectManager> cache;

        IProjectsRepository projResp;
        IProjectUsersRepository projUserResp;
        IProjectPrincipalRepository proPriResp;

        private const string CACHE_PROJECTLIST_KEY = "Projects";

        public ISearchUsers SearchUsersAdapter
        {
            get;
            set;
        }
        public ProjectManager(IEmailSender emailSender,
                                ICache<ProjectManager> cache,
                                IProjectsRepository proResp,
                                IProjectUsersRepository proUserResp,
                                IProjectPrincipalRepository proPriResp
                                )
        {
            this.emailSender = emailSender;
            this.cache = cache;

            this.projResp = proResp;
            this.projUserResp = proUserResp;
            this.proPriResp = proPriResp;
        }

        #region IProjectsRepository Members
        public List<ProjectDetailDTO> GetAllProjects()
        {
            if (cache[CACHE_PROJECTLIST_KEY] == null)
            {
                SearchProjectsRequest request = new SearchProjectsRequest(SearchProjectsType.All, false, "Title", "ASC");
                this.ClearBrokenRuleMessages();
                SearchProjectsResponse response = projResp.SearchProjects(request);
                if (response.ResultList == null)
                {
                    this.AddBrokenRuleMessage();
                    return null;
                }
                cache[CACHE_PROJECTLIST_KEY] = response.ResultList;
            }
            return cache[CACHE_PROJECTLIST_KEY] as List<ProjectDetailDTO>;
        }

        public List<ProjectDetailDTO> GetUserProjects(UsersEntity user)
        {
            List<ProjectDetailDTO> list;
            List<ProjectDetailDTO> all = GetAllProjects();
            switch (user.Role)
            {
                case RolesEnum.ADMIN:
                    list = all;
                    break;
                case RolesEnum.CLIENT:
                    list = all.FindAll(p => p.CompanyID == user.CompanyID);
                    List<int> listProjectId = projResp.GetProjectIdByClientID(user.UserID);
                    if (listProjectId.Count == 0) list = new List<ProjectDetailDTO>();
                    list = list.FindAll(r => listProjectId.Contains(r.ID));
                    break;
                case RolesEnum.PM:
                    SearchProjectsRequest requestUserPM = new SearchProjectsRequest(SearchProjectsType.ListByUserID, false, "Title", "ASC");
                    requestUserPM.UserID = user.ID;
                    list = this.SearchProjects(requestUserPM).ResultList;
                    if (list.Count > 0)
                    {
                        list = list.Distinct<ProjectDetailDTO>(new ProjectCompare()).OrderBy(r => r.Title.Trim()).ToList<ProjectDetailDTO>();
                    }
                    break;
                default:
                    SearchProjectsRequest requestUser = new SearchProjectsRequest(SearchProjectsType.ListByUserID, false, "Title", "ASC");
                    requestUser.UserID = user.ID;
                    list = this.SearchProjects(requestUser).ResultList;
                    break;
            }
            return list;
        }


        public SearchProjectsResponse SearchProjects(SearchProjectsRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchProjectsResponse response = projResp.SearchProjects(request);
            if (response == null)
            {
                this.AddBrokenRuleMessage();
            }
            return projResp.SearchProjects(request);
        }

        public int Insert(ProjectsEntity model)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<ProjectsEntity> validator = new AddProjectValidator();
            if (!validator.Validate(model))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return 0;
            }
            if (projResp.CheckExistsTitle(model.Title, 0))
            {
                this.AddBrokenRuleMessage("Error", "Project title existed!Please input a new one.");
                return 0;
            }
            if (projResp.CheckExistsCode(model.ProjectCode, 0))
            {
                this.AddBrokenRuleMessage("Error", "Project code existed!Please input a new one.");
                return 0;
            }
            int id = projResp.Insert(model);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }
            model.ProjectID = id;
            model.ID = id;
            ClearCache();
            return id;
        }

        public bool Update(ProjectsEntity model)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<ProjectsEntity> validator = new UpdateProjectValidator();
            if (!validator.Validate(model))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return false;
            }
            if (projResp.CheckExistsTitle(model.Title, model.ID))
            {
                this.AddBrokenRuleMessage("Error", "Project title existed!Please input a new one.");
                return false;
            }
            if (projResp.CheckExistsCode(model.ProjectCode, model.ID))
            {
                this.AddBrokenRuleMessage("Error", "Project code existed!Please input a new one.");
                return false;
            }
            if (!projResp.Update(model))
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            cache[CACHE_PROJECTLIST_KEY] = null;
            return true;
        }
        private void ClearCache()
        {
            cache[CACHE_PROJECTLIST_KEY] = null;
        }

        public ProjectsEntity Get(int id, string connStr = "")
        {
            if (id < 0)
                return null;
            this.ClearBrokenRuleMessages();
            List<ProjectDetailDTO> list = GetAllProjects();
            if (list == null || list.Count == 0) return null;

            ProjectsEntity model = list.Find(r => r.ProjectID == id);
            if (model == null)
                this.AddBrokenRuleMessage();
            return model;
        }

        #endregion

        #region IRepository<ProjectUsersEntity> Members

        public int AssignUserToProject(ProjectUsersEntity model)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<ProjectUsersEntity> validator = new AddProjectUserValidator();
            if (!validator.Validate(model))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return 0;
            }
            int id = projUserResp.Insert(model);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }
            model.PUID = id;
            model.ID = id;
            return id;
        }
        public List<ProjectUsersEntity> GetProjectSunnetUserList(int projectID)
        {
            this.ClearBrokenRuleMessages();
            List<ProjectUsersEntity> list = projUserResp.GetProjectSunnetUserList(projectID);
            if (list == null && list.Count < 1)
            {
                this.AddBrokenRuleMessage();
            }
            return list;
        }



        public List<int> GetUserIdByProjectId(int projectId)
        {
            return projUserResp.GetUserIdByProjectId(projectId);
        }

        public List<int> GetActiveUserIdByProjectId(int projectId)
        {
            return projUserResp.GetActiveUserIdByProjectId(projectId);
        }

        /// <summary>
        /// 获取与User同一个项目下的用户ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<int> GetProjectUserIds(int userId)
        {
            return projUserResp.GetProjectUserIds(userId);
        }

        public bool DeleteProjectUser(int projectId, int userId)
        {
            return projUserResp.Delete(projectId, userId);
        }

        #endregion

        public float GetProjectTimeSheetTime(int projectId)
        {
            return projResp.GetProjectTimeSheetTime(projectId);
        }

        public bool updateRemainHoursSendEmailStatus(bool hasSend, int projectId)
        {
            return projResp.updateRemainHoursSendEmailStatus(hasSend, projectId);
        }

        #region ISearchUsers Members

        public SearchUserResponse GetProjectUsers(SearchUsersRequest request)
        {
            return SearchUsersAdapter.GetProjectUsers(request);
        }

        public List<UsersEntity> GetPojectClientUsers(int projectId, int companyId)
        {
            return projResp.GetPojectClientUsers(projectId, companyId);
        }

        public List<UsersEntity> GetPojectPmUsers(int projectId, int companyId)
        {
            return projResp.GetPojectPmUsers(projectId, companyId);
        }


        #endregion

        public int AddPrincipal(ProjectPrincipalEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Module))
            {
                this.AddBrokenRuleMessage("Module/Function not null", "please input the Module/Function!");
                return 0;
            }
            return proPriResp.Insert(entity);
        }

        public List<ProjectPrincipalEntity> GetProjectPrincipal(int projectId)
        {
            return proPriResp.GetProjectPrincipal(projectId);
        }

        public ProjectPrincipalEntity GetProjectPrincipalInfo(int id)
        {
            return proPriResp.Get(id);
        }

        public bool UpdateProjectPrincipal(ProjectPrincipalEntity entity)
        {
            return proPriResp.Update(entity);
        }

        public List<ProjectTicketModel> GetProjectTicketList(bool internalProject, int userId)
        {
            return projResp.GetProjectTicketList(internalProject, userId);
        }
    }
}
