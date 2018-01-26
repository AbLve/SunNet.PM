using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using StructureMap;
namespace SunNet.PMNew.Web.UserControls
{
    public partial class Profile : BaseAscx
    {
        UserApplication userApp;

        public bool IsAdd
        {
            get;
            set;
        }
        public bool IsSunnet
        {
            get;
            set;
        }

        public UsersEntity UserToEdit { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            userApp = new UserApplication();

            if (!IsPostBack)
            {
                InitControl();
            }
        }
        private void InitCompany()
        {
            CompanyApplication comApp = new CompanyApplication();
            List<CompanysEntity> list = comApp.GetAllCompanies();
            list.BindDropdown(ddlCompany, "CompanyName", "ComID");
        }
        private void InitRole()
        {
            List<RolesEntity> list = userApp.GetAllRoles();
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataValueField = "ID";
            ddlRole.DataSource = list;
            ddlRole.DataBind();
        }
        private void InitControl()
        {
            InitCompany();
            InitRole();

            if (IsSunnet)
            {
                phClient.Visible = false;
            }
            else
            {
                phSunnet.Visible = false;
            }
            if (IsAdd)
            {
                if (IsSunnet)
                { }
                else
                {
                    ddlRole.SelectedValue = ((int)RolesEnum.CLIENT).ToString();
                    ddlUserType.SelectedValue = "CLIENT";
                    ddlOffice.SelectedValue = "US";
                }
            }
            else
            {
                UsersEntity user = UserToEdit;
                if (user == null)
                {
                    BaseWebsitePage.ShowArgumentErrorMessageToClient();
                    return;
                }
                InitControls(user);
            }

            // view my profile ,always hiddden advance options
            phSunnet.Visible = false;
        }


        private void InitControls(UsersEntity model)
        {
            ddlUserType.Enabled = false;
            ddlUserType.SelectedValue = model.UserType;

            ddlCompany.Enabled = false;
            ddlCompany.SelectedValue = model.CompanyID.ToString();

            ddlRole.SelectedValue = model.RoleID.ToString();

            if (IsSunnet)
            {
                ddlOffice.SelectedValue = model.Office;

            }
            else
            {
                ddlRole.Enabled = false;
                // Advance Infomation Client
                txtEFirstName.Text = model.EmergencyContactFirstName;
                txtELastName.Text = model.EmergencyContactLastName;
                txtEEmail.Text = model.EmergencyContactEmail;
                txtEPhone.Text = model.EmergencyContactPhone;
            }
            // basec infomation
            txtFirstName.Text = model.FirstName;
            txtLastName.Text = model.LastName;
            txtUserName.Text = model.UserName;
            txtUserName.Text = model.Email;
            txtTitle.Text = model.Title;
            txtPhone.Text = model.Phone;
            txtSkype.Text = model.Skype;
            ddlStatus.SelectedValue = model.Status;

        }
        private string CheckInput(out bool result)
        {
            result = true;
            if (IsAdd && string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                result = false;
                return "Required field cannot be left blank.";
            }
            if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                result = false;
                return "Passwords do not match.";
            }
            return string.Empty;
        }
        private UsersEntity GetEntity()
        {
            UsersEntity model = new UsersEntity();
            if (IsAdd)
            {
                model = UsersFactory.CreateUsersEntity(BaseWebsitePage.UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
                model.PassWord = txtPassword.Text;
            }
            else
            {
                model = (UsersEntity)UserToEdit.Clone();
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    model.PassWord = txtPassword.Text;
                }
            }
            // Advance Infomation Sunnet
            model.RoleID = int.Parse(ddlRole.SelectedValue);
            model.CompanyID = int.Parse(ddlCompany.SelectedValue);
            model.CompanyName = ddlCompany.SelectedItem.Text;
            model.UserType = ddlUserType.SelectedValue;
            model.Office = ddlOffice.SelectedValue;

            if (IsSunnet)
            {
                // Advance Infomation Client
                model.EmergencyContactFirstName = "Sunnet";
                model.EmergencyContactLastName = "Sunnet";
                model.EmergencyContactEmail = "Sunnet@sunnet.us";
                model.EmergencyContactPhone = "999-999-9999";

                model.MaintenancePlanOption = UserMaintenancePlanOption.NONE.ToString();
            }
            else
            {
                // Advance Infomation Client
                model.EmergencyContactFirstName = txtEFirstName.Text.Trim();
                model.EmergencyContactLastName = txtELastName.Text.Trim();
                model.EmergencyContactEmail = txtEEmail.Text.Trim();
                model.EmergencyContactPhone = txtEPhone.Text.Trim();
            }

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
            bool result;
            string msg = CheckInput(out result);
            if (!result)
            {
                BaseWebsitePage.ShowMessageToClient(msg, 2, false, false);
                return;
            }
            UsersEntity user = GetEntity();
            if (IsAdd)
            {
                int id = userApp.AddUser(user);
                if (id > 0)
                {
                    BaseWebsitePage.ShowSuccessMessageToClient();
                }
                else
                {
                    BaseWebsitePage.ShowFailMessageToClient(userApp.BrokenRuleMessages);
                }
            }
            else
            {
                if (userApp.UpdateUser(user))
                {
                    BaseWebsitePage.ShowSuccessMessageToClient(true, false);
                }
                else
                {
                    BaseWebsitePage.ShowFailMessageToClient(userApp.BrokenRuleMessages, true, false);
                }
            }
        }
    }
}