using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;

using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Core.UserModule;

using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.CompanyModel;
using System.Data;
using SunNet.PMNew.Entity.TicketModel.UserTicket;
using SunNet.PMNew.Entity.UserModel.UserModel;

namespace SunNet.PMNew.App
{
    public class UserApplication : BaseApp
    {
        private UserManager mgr;

        public UserApplication()
        {
            mgr = new UserManager(
                                        ObjectFactory.GetInstance<IEmailSender>(),
                                        ObjectFactory.GetInstance<ICache<UserManager>>(),
                                        ObjectFactory.GetInstance<IUsersRepository>(),
                                        ObjectFactory.GetInstance<IRolesRepository>(),
                                        ObjectFactory.GetInstance<IModulesRepository>(),
                                        ObjectFactory.GetInstance<IRoleModulesRepository>(),
                                        ObjectFactory.GetInstance<IHideUserRepository>()
                                        );
            mgr.CompanyAdapter = ObjectFactory.GetInstance<ICompanyCore>();
        }

        #region Module
        public int AddModule(ModulesEntity module)
        {
            this.ClearBrokenRuleMessages();
            int mid = mgr.AddModule(module);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return mid;
        }

        public bool UpdateModule(ModulesEntity module, int roleID)
        {
            this.ClearBrokenRuleMessages();
            bool update = mgr.UpdateModule(module, roleID);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return update;
        }

        public ModulesEntity GetModule(int moduleID)
        {
            this.ClearBrokenRuleMessages();
            ModulesEntity module = mgr.GetModule(moduleID);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return module;
        }

        public List<ModulesEntity> GetAllModules(int parentID, int page, int pageCount, out int recordCount)
        {
            this.ClearBrokenRuleMessages();
            List<ModulesEntity> list = mgr.GetAllModules(parentID, page, pageCount, out recordCount);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }
        public List<ModulesEntity> GetRoleModules(int roleID, bool useCache)
        {
            this.ClearBrokenRuleMessages();
            List<ModulesEntity> list = mgr.GetModulesList(roleID, useCache);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }
        public List<ModulesEntity> GetRoleModules(int roleID)
        {
            return GetRoleModules(roleID, false);
        }

        public bool RemoveAllModules(int roleID)
        {
            this.ClearBrokenRuleMessages();
            bool removed = mgr.RemoveAllModules(roleID);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return removed;
        }
        #endregion

        #region RoleModules
        public bool AddRoleModule(RoleModulesEntity rm)
        {
            this.ClearBrokenRuleMessages();
            bool inserted = mgr.AddRoleModule(rm);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return inserted;
        }

        public bool RoleCanAccessPage(int roleID, string page)
        {
            return mgr.RoleCanAccessPage(roleID, page);
        }

        #endregion

        #region Role
        public int AddRole(RolesEntity role)
        {
            this.ClearBrokenRuleMessages();
            int id = mgr.AddRole(role);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }

        public bool UpdateRole(RolesEntity role)
        {
            this.ClearBrokenRuleMessages();
            bool updated = mgr.UpdateRole(role);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return updated;
        }

        public RolesEntity GetRole(int roleID)
        {
            this.ClearBrokenRuleMessages();
            RolesEntity role = mgr.GetRole(roleID);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return role;
        }

        public List<RolesEntity> GetAllRoles()
        {
            this.ClearBrokenRuleMessages();
            List<RolesEntity> list = mgr.GetAllRoles();
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }
        #endregion

        #region User

        public int AddUser(UsersEntity user)
        {
            this.ClearBrokenRuleMessages();
            int id = mgr.AddUser(user);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }

        public bool UpdateUser(UsersEntity user)
        {
            this.ClearBrokenRuleMessages();
            bool updated = mgr.UpdateUser(user);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return updated;
        }

        public UsersEntity GetUser(int userID)
        {
            this.ClearBrokenRuleMessages();
            UsersEntity user = mgr.GetUser(userID);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return user;
        }

        public UsersEntity GetUser(int userID, bool useCache)
        {
            this.ClearBrokenRuleMessages();
            UsersEntity user = mgr.GetUser(userID, useCache);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return user;
        }

        public string GetLastNameFirstName(int userID)
        {
            UsersEntity user = mgr.GetUser(userID);
            if (user == null) return string.Empty;
            return user.FirstAndLastName;
        }

        public UsersEntity Login(string username, string password)
        {
            this.ClearBrokenRuleMessages();
            UsersEntity model = mgr.Login(username, password);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return model;
        }

        public SearchUserResponse SearchUsers(SearchUsersRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchUserResponse response = mgr.SearchUsers(request);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return response;
        }

        public List<UserTicketModel> SearchUserWithRole(List<RolesEnum> roles, string hideUserIds)
        {
            this.ClearBrokenRuleMessages();
            List<UserTicketModel> list = mgr.SearchUserWithRole(roles, hideUserIds);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }

        public List<DashboardUserModel> GetUserByRoles(List<RolesEnum> roles)
        {
            this.ClearBrokenRuleMessages();
            List<DashboardUserModel> list = mgr.GetUserByRoles(roles);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }
        public bool IsLoginSuccess(string uname, string upwd)
        {
            return mgr.IsLoginSuccess(uname, upwd);
        }

        /// <summary>
        /// Caution: do not remove the list item.
        /// </summary>
        /// <returns></returns>
        public List<UsersEntity> GetActiveUserList()
        {
            this.ClearBrokenRuleMessages();
            List<UsersEntity> list = mgr.GetActiveUserList();
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }

        public bool SendForgotPasswordEmail(string username)
        {
            this.ClearBrokenRuleMessages();
            bool result = mgr.SendForgotPasswordEmail(username);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }

        public bool CheckPassword(string password, string confirm, out string outmsg)
        {
            return mgr.CheckPassword(password, confirm, out outmsg);
        }
        #endregion

        #region HideUsers
        public int AddHideUsers(HideUserEntity hideUser)
        {
            this.ClearBrokenRuleMessages();
            int id = mgr.AddHideUsers(hideUser);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }

        public bool UpdateHideUsers(HideUserEntity hideUser)
        {
            this.ClearBrokenRuleMessages();
            bool updated = mgr.UpdateHideUsers(hideUser);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return updated;
        }

        public HideUserEntity GetHideUserByUserId(int userId)
        {
            this.ClearBrokenRuleMessages();
            HideUserEntity hideUserEntity = mgr.GetHideUserByUserId(userId);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return hideUserEntity;
        }

        public int IsExistDataByUserId(int userId)
        {
            this.ClearBrokenRuleMessages();
            int isExist = mgr.IsExistDataByUserId(userId);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return isExist;
        }

        #endregion
    }
}
