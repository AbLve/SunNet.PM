using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Core.ProjectModule;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.CompanyModel;
using System.IO;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Core.ProjectModule.Interfaces;
using SunNet.PMNew.Entity.ProjectModel.ProjectTicket;

namespace SunNet.PMNew.App
{
    public class ProjectApplication : BaseApp
    {
        ProjectManager mgr;
        public ProjectApplication()
        {
            mgr = new ProjectManager(ObjectFactory.GetInstance<IEmailSender>(),
                                     ObjectFactory.GetInstance<ICache<ProjectManager>>(),
                                     ObjectFactory.GetInstance<IProjectsRepository>(),
                                     ObjectFactory.GetInstance<IProjectUsersRepository>(),
                                     ObjectFactory.GetInstance<IProjectPrincipalRepository>()
                                     );
            mgr.SearchUsersAdapter = ObjectFactory.GetInstance<ISearchUsers>();
        }

        public List<ProjectDetailDTO> GetUserProjects(UsersEntity user)
        {
            this.ClearBrokenRuleMessages();
            List<ProjectDetailDTO> list = mgr.GetUserProjects(user);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }
        /// <summary>
        /// 获取当前用户的可以用于创建Ticket，events的相关状态的Projects.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="selectedProject">编辑时的旧数据处理.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/8 21:53
        public List<ProjectDetailDTO> GetUserProjectsForCreateObject(UsersEntity user, int selectedProject = 0)
        {
            this.ClearBrokenRuleMessages();
            List<ProjectDetailDTO> list = mgr.GetUserProjects(user);
            list.RemoveAll(r => (r.Status == ProjectStatus.Cancelled
                || r.Status == ProjectStatus.Completed)
                && r.ProjectID != selectedProject);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }

        public List<ProjectDetailDTO> GetAllProjects()
        {
            this.ClearBrokenRuleMessages();
            List<ProjectDetailDTO> list = mgr.GetAllProjects();
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }

        public SearchProjectsResponse SearchProjects(SearchProjectsRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchProjectsResponse response = mgr.SearchProjects(request);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return response;
        }
        public int Insert(ProjectsEntity model)
        {
            this.ClearBrokenRuleMessages();
            int id = mgr.Insert(model);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }

        public bool Update(ProjectsEntity model)
        {
            this.ClearBrokenRuleMessages();
            bool result = mgr.Update(model);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }

        public ProjectsEntity Get(int id, string connStr = "")
        {
            this.ClearBrokenRuleMessages();
            ProjectsEntity model = mgr.Get(id);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return model;
        }

        public SearchUserResponse GetProjectUsers(SearchUsersRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchUserResponse response = mgr.GetProjectUsers(request);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return response;
        }

        public List<UsersEntity> GetPojectClientUsers(int projectId, int companyId)
        {
            return mgr.GetPojectClientUsers(projectId, companyId);
        }

        public List<UsersEntity> GetPojectPmUsers(int projectId, int companyId)
        {
            return mgr.GetPojectPmUsers(projectId, companyId);
        }


        public List<int> GetUserIdByProjectId(int projectId)
        {
            return mgr.GetUserIdByProjectId(projectId);
        }

        public List<int> GetActiveUserIdByProjectId(int projectId)
        {
            return mgr.GetActiveUserIdByProjectId(projectId);
        }

        /// <summary>
        /// 获取与User同一个项目下的用户ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<int> GetProjectUserIds(int userId)
        {
            return mgr.GetProjectUserIds(userId);
        }

        public int AssignUserToProject(ProjectUsersEntity model)
        {
            this.ClearBrokenRuleMessages();
            int id = mgr.AssignUserToProject(model);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }

        public List<ProjectUsersEntity> GetProjectSunnetUserList(int projectID)
        {
            this.ClearBrokenRuleMessages();

            return mgr.GetProjectSunnetUserList(projectID);
        }

        public float GetProjectTimeSheetTime(int projectId)
        {
            return mgr.GetProjectTimeSheetTime(projectId);
        }

        public bool updateRemainHoursSendEmailStatus(bool hasSend, int projectId)
        {
            return mgr.updateRemainHoursSendEmailStatus(hasSend, projectId);
        }

        public int AddPrincipal(ProjectPrincipalEntity model)
        {
            this.ClearBrokenRuleMessages();
            int id = mgr.AddPrincipal(model);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }

        public List<ProjectPrincipalEntity> GetProjectPrincipal(int projectId)
        {
            return mgr.GetProjectPrincipal(projectId);
        }

        public ProjectPrincipalEntity GetProjectPrincipalInfo(int id)
        {
            return mgr.GetProjectPrincipalInfo(id);
        }

        public bool UpdateProjectPrincipal(ProjectPrincipalEntity entity)
        {
            return mgr.UpdateProjectPrincipal(entity);
        }

        public string GetProjectInfoJson(List<ProjectDetailDTO> projectEntities)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("({");
            foreach (ProjectsEntity projectEntity in projectEntities)
            {
                stringBuilder.AppendFormat("'{0}':['{1}','{2}'],", projectEntity.ProjectID,
                    projectEntity.IsNeedClientEstimate,
                    projectEntity.EndDate == UtilFactory.Helpers.CommonHelper.GetDefaultMinDate() ?
                    true : projectEntity.EndDate >= DateTime.Now.Date);
            }
            return stringBuilder.ToString().TrimEnd(',') + "})";
        }

        public bool CheckIfEstimated(ProjectsEntity entity)
        {
            if (entity.EndDate != UtilFactory.Helpers.CommonHelper.GetDefaultMinDate()
                && entity.EndDate < DateTime.Now.Date)
                return false;
            else
                return true;
        }

        public bool DeleteProjectUser(int projectId, int userId)
        {
            return mgr.DeleteProjectUser(projectId, userId);
        }

        //此方法用于查看Event操作
        public List<UsersEntity> GetProjectUsersByUserId(UsersEntity user)
        {
            List<UsersEntity> userList = new List<UsersEntity>();
            List<int> userIdList = GetProjectUserIds(user.UserID);

            if (user.Role == RolesEnum.CLIENT)
            {
                foreach (int tmpId in userIdList)
                {
                    UsersEntity tmpUserEntity = new App.UserApplication().GetUser(tmpId);
                    if (tmpUserEntity != null && tmpUserEntity.Status.ToUpper() == "ACTIVE")
                    {
                        if (tmpUserEntity.Role == RolesEnum.CLIENT)
                            userList.Add(tmpUserEntity);
                        else if (tmpUserEntity.Office.ToUpper() == "US" && tmpUserEntity.UserType.ToUpper() == "SUNNET")
                            userList.Add(tmpUserEntity);
                    }
                }
            }
            else
            {
                if (user.UserType.ToUpper() == "SUNNET")
                {
                    if (user.Office.ToUpper() == "US")
                    {
                        foreach (int tmpId in userIdList)
                        {
                            UsersEntity tmpUserEntity = new App.UserApplication().GetUser(tmpId);
                            if (tmpUserEntity != null && tmpUserEntity.Status.ToUpper() == "ACTIVE")
                            {
                                userList.Add(tmpUserEntity);
                            }
                        }
                    }
                    else
                    {
                        foreach (int tmpId in userIdList)
                        {
                            UsersEntity tmpUserEntity = new App.UserApplication().GetUser(tmpId);
                            if (tmpUserEntity != null && tmpUserEntity.Status.ToUpper() == "ACTIVE")
                            {
                                if (tmpUserEntity.Office.ToUpper() == "CN")
                                    userList.Add(tmpUserEntity);
                            }
                        }
                    }
                }
                else
                {
                    userList.Add(user);
                }
            }

            if (userIdList.Count == 0)
                userList.Add(user);

            return userList;
        }

        public List<ProjectTicketModel> GetProjectTicketList(bool internalProject, int userId)
        {
            return mgr.GetProjectTicketList(internalProject, userId);
        }

    }
}
