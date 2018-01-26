using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.LogModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Admin.Users
{
    public partial class NewClientUser : BasePage
    {
        UserApplication userApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            userApp = new UserApplication();

            if (!IsPostBack)
            {
                InitCompany();
                SetControlsStatus();
            }
        }
        private void SetControlsStatus()
        {
            bool isReadOnly = ISReadOnlyRole;

            ddlCompany.Enabled = !isReadOnly;
            txtTitle.ReadOnly = isReadOnly;
            txtFirstName.ReadOnly = isReadOnly;
            txtLastName.ReadOnly = isReadOnly;
            txtUserName.ReadOnly = isReadOnly;
            txtPhone.ReadOnly = isReadOnly;
            txtSkype.ReadOnly = isReadOnly;
            txtPassword.ReadOnly = isReadOnly;
            txtConfirmPassword.ReadOnly = isReadOnly;

            txtEEmail.ReadOnly = isReadOnly;
            txtEFirstName.ReadOnly = isReadOnly;
            txtELastName.ReadOnly = isReadOnly;
            txtEPhone.ReadOnly = isReadOnly;

        }
        private void InitCompany()
        {
            CompanyApplication comApp = new CompanyApplication();
            List<CompanysEntity> list = comApp.GetAllCompanies();
            list.BindDropdown(ddlCompany, "CompanyName", "ComID");
        }

        private string CheckInput(out bool result)
        {
            result = true;
            string msg = string.Empty;
            result = userApp.CheckPassword(txtPassword.Text, txtConfirmPassword.Text, out msg);
            return msg;
        }
        private UsersEntity GetEntity()
        {
            UsersEntity model = new UsersEntity();
            model = UsersFactory.CreateUsersEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
            model.PassWord = txtPassword.Text;
            // Advance Infomation Sunnet
            model.CompanyID = int.Parse(ddlCompany.SelectedValue);
            model.CompanyName = ddlCompany.SelectedItem.Text;
            model.UserType = "CLIENT";
            model.Office = "US";
            model.RoleID = (int)RolesEnum.CLIENT; 
            // Advance Infomation Client
            model.EmergencyContactFirstName = txtEFirstName.Text.Trim();
            model.EmergencyContactLastName = txtELastName.Text.Trim();
            model.EmergencyContactEmail = txtEEmail.Text.Trim();
            model.EmergencyContactPhone = txtEPhone.Text.Trim();
             
            // basec infomation
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtLastName.Text.Trim();
            model.UserName = txtUserName.Text;
            model.Email = txtUserName.Text.Trim();
            model.Title = txtTitle.Text.Trim();
            model.Phone = txtPhone.Text.Trim();
            model.Skype = txtSkype.Text.Trim();

            model.Status = ddlStatus.SelectedValue;
            model.ForgotPassword = 0;

            model.IsDelete = false;
            model.AccountStatus = 0;

            return model;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            LogApplication logApplication = new LogApplication();
            LogEntity logEntity = new LogEntity();
            logEntity.logType = LogType.ModifyPassword;
            logEntity.operatingTime = DateTime.Now;
            logEntity.currentUserId = UserInfo.UserID;
            logEntity.referrer = Context.Request.UrlReferrer.ToString();
            logEntity.iPAddress = HttpContext.Current.Request.UserHostAddress;

            bool result;
            string msg = CheckInput(out result);
            if (!result)
            {
                ShowMessageToClient(msg, 2, false, false);
                return;
            }
            UsersEntity user = GetEntity();
            int id = userApp.AddUser(user);
            if (id > 0)
            {
                //ShowSuccessMessageToClient();
                Redirect("/Admin/users/ClientUserDetail.aspx?id="+id+"&returnurl=/admin/Users/users.aspx",true);
            }
            else
            {
                ShowFailMessageToClient(userApp.BrokenRuleMessages);
            }
        }
    }
}