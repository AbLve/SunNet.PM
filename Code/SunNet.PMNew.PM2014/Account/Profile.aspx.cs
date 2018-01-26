using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Account
{
    public partial class Profile : BasePage
    {
        UserApplication userApp = new UserApplication();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitCompany();
                InitControls(UserInfo);
            }
        }

        private void InitCompany()
        {
            var comApp = new CompanyApplication();
            CompanysEntity company = comApp.GetCompany(UserInfo.CompanyID);
            if (company != null) ltlCompany.Text = company.CompanyName;
        }

        private void InitControls(UsersEntity model)
        {
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                phlEmergency.Visible = true;

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
            txtTitle.Text = model.Title;
            txtPhone.Text = model.Phone;
            txtSkype.Text = model.Skype;
            txtBoxEmail.Text = model.Email;
        }

        private UsersEntity GetEntity()
        {
            UsersEntity model = UserInfo;
            if (model.Role == RolesEnum.CLIENT)
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
            model.Email = txtBoxEmail.Text.Trim();
            model.Title = txtTitle.Text.Trim();
            model.Phone = txtPhone.Text.Trim();
            model.Skype = txtSkype.Text.Trim();
            model.ForgotPassword = 0;

            model.IsDelete = false;
            model.AccountStatus = 0;

            return model;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UsersEntity user = GetEntity();

            if (userApp.UpdateUser(user))
            {
                Redirect(Request.RawUrl, true);
            }
            else
            {
                ShowFailMessageToClient(userApp.BrokenRuleMessages);
            }
        }
    }
}