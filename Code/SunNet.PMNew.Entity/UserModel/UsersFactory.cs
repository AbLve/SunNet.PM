using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Entity.UserModel
{
    public static class UsersFactory
    {
        public static ModulesEntity CreateModulesEntity(int createdByUserID, ISystemDateTime timeProvider)
        {
            ModulesEntity model = new ModulesEntity();

            model.CreatedBy = createdByUserID;
            model.CreatedOn = timeProvider.Now;
            model.ID = 0;
            model.ModifiedBy = createdByUserID;
            model.ModifiedOn = timeProvider.Now;

            model.ModuleID = 0;
            model.ModulePath = string.Empty;
            model.ModuleTitle = string.Empty;
            model.Orders = 0;
            model.ParentID = 0;
            model.Status = 0;

            return model;
        }

        public static PagesEntity CreatePagesEntity(int createdByUserID, ISystemDateTime timeProvider)
        {
            PagesEntity model = new PagesEntity();

            model.CreatedBy = createdByUserID;
            model.CreatedOn = timeProvider.Now;
            model.ID = 0;
            model.ModifiedBy = createdByUserID;
            model.ModifiedOn = timeProvider.Now;

            model.IsMenu = false;
            model.MID = 0;
            model.Orders = 0;
            model.PageID = 0;
            model.PageName = string.Empty;
            model.PageTitle = string.Empty;
            model.Status = 0;

            return model;
        }

        public static RoleModulesEntity CreateRoleModulesEntity(int createdByUserID, ISystemDateTime timeProvider)
        {
            RoleModulesEntity model = new RoleModulesEntity();

            model.CreatedBy = createdByUserID;
            model.CreatedOn = timeProvider.Now;
            model.ID = 0;
            model.ModifiedBy = createdByUserID;
            model.ModifiedOn = timeProvider.Now;

            model.ModuleID = 0;
            model.RMID = 0;
            model.RoleID = 0;

            return model;
        }

        public static RolePagesEntity CreateRolePagesEntity(int createdByUserID, ISystemDateTime timeProvider)
        {
            RolePagesEntity model = new RolePagesEntity();

            model.CreatedBy = createdByUserID;
            model.CreatedOn = timeProvider.Now;
            model.ID = 0;
            model.ModifiedBy = createdByUserID;
            model.ModifiedOn = timeProvider.Now;

            model.PageID = 0;
            model.RoleID = 0;
            model.RPID = 0;

            return model;
        }

        public static RolesEntity CreateRolesEntity(int createdByUserID, ISystemDateTime timeProvider)
        {
            RolesEntity model = new RolesEntity();

            model.CreatedBy = createdByUserID;
            model.CreatedOn = timeProvider.Now;
            model.ID = 0;
            model.ModifiedBy = createdByUserID;
            model.ModifiedOn = timeProvider.Now;

            model.RoleID = 0;
            model.RoleName = string.Empty;
            model.Status = 0;

            return model;
        }

        public static UsersEntity CreateUsersEntity(int createdByUserID, ISystemDateTime timeProvider)
        {
            UsersEntity model = new UsersEntity();

            model.CreatedBy = createdByUserID;
            model.CreatedOn = timeProvider.Now;
            model.ID = 0;
            model.ModifiedBy = createdByUserID;
            model.ModifiedOn = timeProvider.Now;

            model.UserID = 0;
            model.ID = 0;
            model.CompanyName = string.Empty;
            model.CompanyID = 0;
            model.RoleID = 0;
            model.FirstName = string.Empty;
            model.LastName = string.Empty;
            model.UserName = string.Empty;
            model.Email = string.Empty;
            model.PassWord = string.Empty;
            model.Title = string.Empty;
            model.Phone = string.Empty;
            model.EmergencyContactFirstName = string.Empty;
            model.EmergencyContactLastName = string.Empty;
            model.EmergencyContactPhone = string.Empty;
            model.EmergencyContactEmail = string.Empty;
            model.HasAMaintenancePlan = false;
            model.DoesNotHaveAMaintenancePlan = false;
            model.NeedsAQuoteApproval = false;
            model.DoesNotNeedAQuoteApproval = false;
            model.AllowMeToChoosePerSubmission = false;
            model.CreatedOn = DateTime.Now;
            model.AccountStatus = 0;
            model.ForgotPassword = 0;
            model.IsDelete = false;
            model.Status = string.Empty;
            model.UserType = string.Empty;
            model.Skype = string.Empty;
            model.Office = string.Empty;

            return model;
        }
    }
}
