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

using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework;
using System.Web;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Data;
using SunNet.PMNew.Entity.TicketModel.UserTicket;
using SunNet.PMNew.Entity.UserModel.UserModel;

namespace SunNet.PMNew.Core.UserModule
{
    public class UserManager : BaseMgr
    {

        private IEmailSender emailSender;
        private ICache<UserManager> cache;

        private IUsersRepository userRepository;
        private IRolesRepository roleRepository;
        private IModulesRepository moduleRepository;
        private IRoleModulesRepository rmRepository;
        private IHideUserRepository hideUserRepository;
        public ICompanyCore CompanyAdapter
        {
            get;
            set;
        }

        /// <summary>
        /// Role Cache Key
        /// </summary>
        private const string CACHE_ROLEMODULES = "RolesModulesList::{0}";

        /// <summary>
        /// UserCache Key
        /// </summary>
        private const string CACHE_USERINFO = "UserInfo::{0}";
        private const string CACHE_USERlIST = "USERlIST";

        #region  constructor
        public UserManager(IEmailSender emailSender
                            , ICache<UserManager> cache
                            , IUsersRepository userRepository
                            , IRolesRepository roleRepository
                            , IModulesRepository moduleRepository
                            , IRoleModulesRepository rmRespository
                            , IHideUserRepository hideUserRepository
                            )
        {
            this.emailSender = emailSender;
            this.cache = cache;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.moduleRepository = moduleRepository;
            this.rmRepository = rmRespository;
            this.hideUserRepository = hideUserRepository;
        }
        #endregion

        #region Module
        public int AddModule(ModulesEntity module)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<ModulesEntity> validator = new AddModuleValidator();
            if (!validator.Validate(module))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
            }
            int id = moduleRepository.Insert(module);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }
            module.ID = id;

            return id;
        }

        /// <summary>
        /// Update Module
        /// </summary>
        /// <param name="module">Module to update</param>
        /// <param name="roleID">RoleID to update Cache</param>
        /// <returns></returns>
        public bool UpdateModule(ModulesEntity module, int roleID)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<ModulesEntity> validator = new UpdateModuleValidator();
            if (!validator.Validate(module))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
            }
            if (!moduleRepository.Update(module))
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            string key = string.Format(CACHE_ROLEMODULES, roleID);
            cache[key] = null;
            return true;
        }
        public ModulesEntity GetModule(int moduleID)
        {
            if (moduleID <= 0)
            {
                return null;
            }
            ModulesEntity module = moduleRepository.Get(moduleID);
            return module;
        }
        public List<ModulesEntity> GetAllModules(int parentID, int page, int pageCount, out int recordCount)
        {
            if (page > 0 && pageCount > 0)
            {
                recordCount = moduleRepository.GetAllModulesCount(parentID);
                return moduleRepository.GetAllModules(parentID, page, pageCount);
            }
            recordCount = 0;
            return null;
        }

        /// <summary>
        /// GetModulesListByRole(0=All)
        /// </summary>
        /// <param name="roleID">RoleID</param>
        /// <param name="useCache">Use Cache</param>
        /// <returns></returns>
        public List<ModulesEntity> GetModulesList(int roleID, bool useCache)
        {
            if (!useCache)
                return GetModulesList(roleID);
            string key = string.Format(CACHE_ROLEMODULES, roleID);
            List<ModulesEntity> list = (List<ModulesEntity>)cache[key];
            if (list == null || list.Count <= 0)
            {
                list = GetModulesList(roleID);
                cache[key] = list;
            }
            return list;
        }
        /// <summary>
        /// GetModulesListByRole(0=All),no cache
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<ModulesEntity> GetModulesList(int roleID)
        {
            var list = moduleRepository.GetModulesList(roleID);
            return list;
        }

        public bool RemoveAllModules(int roleID)
        {
            if (roleID <= 0)
            {
                return false;
            }
            if (!moduleRepository.RemoveAll(roleID))
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            ClearRoleCache(roleID);
            return true;
        }

        #endregion

        #region RoleModules

        public bool AddRoleModule(RoleModulesEntity rm)
        {
            BaseValidator<RoleModulesEntity> validator = new AddRoleModulesValidator();
            if (!validator.Validate(rm))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return false;
            }
            int id = rmRepository.Insert(rm);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public bool RoleCanAccessPage(int roleID, string page)
        {
            List<ModulesEntity> list = GetModulesList(roleID, true);
            int count = list.Count<ModulesEntity>(m => m.ModulePath.Trim().ToLower() == page.Trim().ToLower());
            return count > 0;
        }
        #endregion

        #region Role
        public int AddRole(RolesEntity role)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<RolesEntity> validator = new AddRoleValidator();
            if (!validator.Validate(role))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return 0;
            }
            int id = roleRepository.Insert(role);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }
            role.ID = id;
            role.RoleID = id;
            return id;
        }
        public bool UpdateRole(RolesEntity role)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<RolesEntity> validator = new UpdateRoleValidator();
            if (!validator.Validate(role))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return false;
            }
            if (!roleRepository.Update(role))
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            return true;
        }
        public RolesEntity GetRole(int roleID)
        {
            if (roleID <= 0)
            {
                return null;
            }
            RolesEntity role = roleRepository.Get(roleID);
            if (role == null)
            {
                return null;
            }
            return role;
        }
        public List<RolesEntity> GetAllRoles()
        {
            return roleRepository.GetAllRoles();
        }

        #endregion

        #region User
        public bool CheckPassword(string password, string confirm, out string outmsg)
        {
            outmsg = string.Empty;
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirm))
            {
                outmsg = "Password field and confirm password field cannot be left blank";
                return false;
            }
            if (!password.Equals(confirm))
            {
                outmsg = "Passwords do not match.";
                return false;
            }
            //Regex regPassword = new Regex("^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,15}$");
            Regex regPassword = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{6,}$");
            if (!regPassword.IsMatch(password))
            {
                outmsg = "Passwords must contain uppercase letters lowercase letters and numbers, and the length is greater than 6.";
                return false;
            }
            return true;
        }
        public int AddUser(UsersEntity user)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<UsersEntity> validator = new AddUserValidator();
            if (string.IsNullOrEmpty(user.Title))
            {
                user.Title = " ";
            }
            if (string.IsNullOrEmpty(user.Phone))
            {
                user.Phone = " ";
            }
            if (string.IsNullOrEmpty(user.Skype))
            {
                user.Skype = " ";
            }
            if (!validator.Validate(user))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return 0;
            }
            if (userRepository.ExistsUserName(user.UserName, 0))
            {
                this.AddBrokenRuleMessage("Existsted Username", "Username already exists. Please input a new username.");
                return 0;
            }
            if (user.PassWord.Length <= 15)
            {
                user.PassWord = UtilFactory.GetEncryptProvider(EncryptType.MD5).Encrypt(user.PassWord);
            }
            int id = userRepository.Insert(user);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }
            user.ID = id;
            user.UserID = id;
            if (user.Status == "ACTIVE")
            {
                List<UsersEntity> list = GetActiveUserList();
                if (list != null)
                    list.Add(user);
            }
            return id;
        }

        public bool UpdateUser(UsersEntity user)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<UsersEntity> validator = new UpdateUserValidator();
            if (!validator.Validate(user))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return false;
            }
            if (userRepository.ExistsUserName(user.UserName, user.ID))
            {
                this.AddBrokenRuleMessage("Existsted username", "Username already exists. Please input a new username.");
                return false;
            }
            if (user.PassWord.Length <= 15)
            {
                user.PassWord = UtilFactory.GetEncryptProvider(EncryptType.MD5).Encrypt(user.PassWord);
            }
            if (!userRepository.Update(user))
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            List<UsersEntity> list = GetActiveUserList();
            if (list != null)
            {
                UsersEntity tmpUser = list.Find(r => r.UserID == user.UserID);
                if (tmpUser != null)
                {
                    list.Remove(tmpUser);
                    if (user.Status == "ACTIVE")
                    {
                        list.Add(user);
                    }
                }
                else
                {
                    if (user.Status == "ACTIVE") list.Add(user);
                }
            }
            return true;
        }
        public UsersEntity GetUser(int userID)
        {
            this.ClearBrokenRuleMessages();
            if (userID <= 0)
            {
                return null;
            }
            List<UsersEntity> list = GetActiveUserList();
            UsersEntity user = null;
            if (list != null)
                user = list.Find(r => r.UserID == userID);
            if (user == null)
                user = userRepository.Get(userID);
            return user;
        }
        public UsersEntity GetUser(int userID, bool useCache)
        {
            if (useCache)
                return GetUser(userID);
            this.ClearBrokenRuleMessages();
            if (userID <= 0)
            {
                return null;
            }
            List<UsersEntity> list = GetActiveUserList();
            UsersEntity user = user = userRepository.Get(userID);
            return user;
        }



        public UsersEntity GetUserByUserName(string username)
        {
            this.ClearBrokenRuleMessages();
            UsersEntity model = userRepository.GetUserByUserName(username);
            if (model == null)
            {
                this.AddBrokenRuleMessage("Error", "Username does not exits.");
            }
            return model;
        }

        public UsersEntity Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                this.AddBrokenRuleMessage("Login error", "Username can not be null!");
                return null;
            }
            if (string.IsNullOrEmpty(password))
            {
                this.AddBrokenRuleMessage("Login error", "Password can not be null!");
                return null;
            }
            UsersEntity user = GetUserByUserName(username);
            if (user == null)
            {
                return null;
            }
            else if (user.Status == "INACTIVE")
            {
                this.AddBrokenRuleMessage("Login error", "Account disable!");
                return null;
            }
            if (user.PassWord == UtilFactory.GetEncryptProvider(EncryptType.MD5).Encrypt(password))
            {
                cache[string.Format(CACHE_USERINFO, user.ID)] = user;
                cache[string.Format(CACHE_USERINFO, user.UserName)] = user;
                return user;
            }
            else
            {
                this.AddBrokenRuleMessage("Login error", "The username or password you entered is incorrect.");
                return null;
            }
        }
        public SearchUserResponse SearchUsers(SearchUsersRequest request)
        {
            return userRepository.SearchUsers(request);
        }

        public List<UserTicketModel> SearchUserWithRole(List<RolesEnum> roles, string hideUserIds)
        {
            return userRepository.SearchUserWithRole(roles, hideUserIds);
        }

        public List<DashboardUserModel> GetUserByRoles(List<RolesEnum> roles)
        {
            return userRepository.GetUserByRoles(roles);
        }
        public bool IsLoginSuccess(string uname, string upwd)
        {
            upwd = UtilFactory.GetEncryptProvider(EncryptType.DES).Encrypt(upwd);
            return userRepository.IsLoginSuccess(uname, upwd);
        }
        public List<UsersEntity> GetActiveUserList()
        {
            List<UsersEntity> list = cache[CACHE_USERlIST] as List<UsersEntity>;
            if (list == null)
            {
                SearchUsersRequest request = new SearchUsersRequest(SearchUsersType.List, false, "UserName", "asc");
                request.Status = "ACTIVE";
                list = userRepository.SearchUsers(request).ResultList;
                if (list != null)
                    cache[CACHE_USERlIST] = list;
            }
            return list;
        }

        public bool SendForgotPasswordEmail(string username)
        {
            try
            {
                this.ClearBrokenRuleMessages();
                UsersEntity user = this.GetUserByUserName(username);
                if (user == null)
                {
                    return false;
                }
                user.AccountStatus = UsersEntity.ForgotPasswordFlag;
                this.UpdateUser(user);
                IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                string enkey = encrypt.Encrypt(string.Format("{0}_{1}", user.ID.ToString(), DateTime.Now.ToString()));
                string url = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host + "/ResetPassword.aspx?link=", enkey);
                string body = UtilFactory.Helpers.FileHelper.GetTemplateFileContent("SendEmailToUserForFindPassword.txt");
                body = body.Replace("{FirstName}", user.FirstName);
                body = body.Replace("{LastName}", user.LastName);
                body = body.Replace("{Date}", DateTime.Now.ToString("MM/dd/yyyy")).Replace("{URL}", url);
                if (emailSender.SendMail(username, Config.DefaultSendEmail, "Password Assistance", body))
                {
                    return true;
                }
                this.AddBrokenRuleMessage("Error", "Sorry,mail server is unavailable.");
                return false;
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
                return false;
            }
        }
        #endregion

        #region ClearCache
        private void ClearRoleCache(int roleID)
        {
            string key = string.Format(CACHE_ROLEMODULES, roleID);
            cache[key] = null;
        }

        #endregion

        #region HideUsers
        public int AddHideUsers(HideUserEntity hideUser)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<HideUserEntity> validator = new AddHideUserValidator();
            if (!validator.Validate(hideUser))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return 0;
            }
            int id = hideUserRepository.Insert(hideUser);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }
            hideUser.ID = id;
            return id;
        }

        public bool UpdateHideUsers(HideUserEntity hideUser)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<HideUserEntity> validator = new UpdateHideUserValidator();
            if (!validator.Validate(hideUser))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return false;
            }
            if (!hideUserRepository.Update(hideUser))
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            return true;
        }

        public HideUserEntity GetHideUserByUserId(int userId)
        {
            if (userId <= 0)
            {
                return null;
            }
            return hideUserRepository.GetHideUserByUserId(userId);
        }

        public int IsExistDataByUserId(int userId)
        {
            return hideUserRepository.IsExistDataByUserId(userId);
        }
        #endregion
    }
}
