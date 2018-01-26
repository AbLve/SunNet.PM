using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StructureMap;

using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using System.Text.RegularExpressions;
using SunNet.PMNew.Entity.LogModel;
using SunNet.PMNew.Entity.ProjectModel;
namespace SunNet.PMNew.Web.UserControls
{
    public partial class UserEdit : BaseAscx
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
                SetControlsStatus();
            }
        }
        private void SetControlsStatus()
        {
            bool isReadOnly = BaseWebsitePage.ISReadOnlyRole;

            ddlCompany.Enabled = !isReadOnly;
            ddlRole.Enabled = !isReadOnly;
            ddlOffice.Enabled = !isReadOnly;
            btnForm.Visible = !isReadOnly;

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
        private void InitRole()
        {
            List<RolesEntity> list = userApp.GetAllRoles();
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataValueField = "ID";
            ddlRole.DataSource = list;
            ddlRole.DataBind();

            if (this.IsSunnet)
            {
                ddlRole.Items.Remove(ddlRole.Items.FindByValue(((int)RolesEnum.CLIENT).ToString()));
                ddlUserType.SelectedIndex = 0;
                ddlUserType.Enabled = false;
            }
        }

        private void InitControl()
        {
            InitCompany();
            InitRole();

            if (IsSunnet)
            {
                phClient.Visible = false;

                ddlCompany.SelectedValue = "1";
                ddlCompany.Enabled = false;

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
        }
        private void InitControls(UsersEntity model)
        {
            ddlUserType.Enabled = false;
            ddlUserType.SelectedValue = model.UserType;

            //ddlCompany.Enabled = false;
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
                //ClientMaintenancePlan1.SelectedMaintenancePlan = model.MainPlanOption;
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
            InitProjectList();
        }

        protected void InitProjectList()
        {
            SearchProjectsRequest request = new SearchProjectsRequest(SearchProjectsType.ListByUserID
                , true, hidOrderBy.Value, hidOrderDirection.Value);
            request.CurrentPage = anpProjects.CurrentPageIndex;
            request.PageCount = anpProjects.PageSize;
            request.UserID = UserToEdit.UserID;
            SearchProjectsResponse response = new ProjectApplication().SearchProjects(request);
            rptProjects.DataSource = response.ResultList;
            rptProjects.DataBind();

            anpProjects.RecordCount = response.ResultCount;
            if (anpProjects.RecordCount > 0)
            {
                trNoRecords.Visible = false;
            }
        }

        protected void anpProjects_PageChanged(object sender, EventArgs e)
        {
            InitProjectList();
        }

        private string CheckInput(out bool result)
        {
            result = true;
            string msg = string.Empty;
            if (IsAdd)
            {
                result = userApp.CheckPassword(txtPassword.Text, txtConfirmPassword.Text, out msg);
            }
            //if (!IsSunnet)
            //{
            //    if (ClientMaintenancePlan1.SelectedMaintenancePlan == UserMaintenancePlanOption.NONE)
            //    {
            //        result = false;
            //        return "Please select a maintenance plan.";
            //    }
            //}
            return msg;
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
                model = UserToEdit;
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    model.PassWord = txtPassword.Text;
                    model.ForgotPassword = UserInfo.ID;
                }
            }
            // Advance Infomation Sunnet
            model.CompanyID = int.Parse(ddlCompany.SelectedValue);
            model.CompanyName = ddlCompany.SelectedItem.Text;
            model.UserType = ddlUserType.SelectedValue;
            model.Office = ddlOffice.SelectedValue;
            model.RoleID = int.Parse(ddlRole.SelectedValue);

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

                //model.MaintenancePlanOption = ClientMaintenancePlan1.SelectedMaintenancePlan.ToString();
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
                    BaseWebsitePage.ShowSuccessMessageToClient(false, true);
                    logEntity.IsSuccess = true;
                    logEntity.Description = UserInfo.UserName + " modifiy " + txtUserName.Text
                        + " password to" + txtPassword.Text + " success. new password: " + txtPassword.Text;
                }
                else
                {
                    BaseWebsitePage.ShowFailMessageToClient(userApp.BrokenRuleMessages);
                    logEntity.IsSuccess = false;
                    logEntity.Description = UserInfo.UserName + " modifiy " + txtUserName.Text
                        + " password to" + txtPassword.Text + " fail. msg: " + msg;
                }
                logApplication.Write(logEntity);
            }
        }
    }
}