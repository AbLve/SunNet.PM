using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.LogModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Admin.Users
{
    public partial class ClientUserDetail : BasePage
    {

        UserApplication userApp;
        protected int userToEditID;

        protected override string DefaultOrderBy
        {
            get
            {
                return "ProjectCode";
            }
        }
        protected override string DefaultDirection
        {
            get
            {
                return "ASC";
            }
        }

        public UsersEntity UserToEdit { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            userApp = new UserApplication();
            int id = QS("id", 0);
            if (id == 0)
            {
                this.ShowArgumentErrorMessageToClient();
                return;
            }
            userToEditID = id;
            UserToEdit = userApp.GetUser(id);
            if (!IsPostBack)
            {
                InitControl();
                SetControlsStatus();
            }
        }
        private void SetControlsStatus()
        {
            bool isReadOnly = ISReadOnlyRole;

            //   ddlCompany.Enabled = !isReadOnly;

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

            litCompanyName.Visible = isReadOnly;
            ddlCompany.Visible = !isReadOnly;

            LitStatus.Visible = isReadOnly;
            ddlStatus.Visible = !isReadOnly;



            btnSave.Visible = !isReadOnly;
        }
        private void InitCompany()
        {
            CompanyApplication comApp = new CompanyApplication();
            List<CompanysEntity> list = comApp.GetAllCompanies();
            list.BindDropdown(ddlCompany, "CompanyName", "ComID");
        }


        private void InitControl()
        {
            InitCompany();
            UsersEntity user = UserToEdit;
            if (user == null)
            {
                ShowArgumentErrorMessageToClient();
                return;
            }
            InitControls(user);
        }
        private void InitControls(UsersEntity model)
        {


            //ddlCompany.Enabled = false;
            ddlCompany.SelectedValue = model.CompanyID.ToString();
            litCompanyName.Text = model.CompanyName;
            // Advance Infomation Client
            txtEFirstName.Text = model.EmergencyContactFirstName;
            txtELastName.Text = model.EmergencyContactLastName;
            txtEEmail.Text = model.EmergencyContactEmail;
            txtEPhone.Text = model.EmergencyContactPhone;
            //ClientMaintenancePlan1.SelectedMaintenancePlan = model.MainPlanOption;

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
                , true, OrderBy, OrderDirection);
            request.CurrentPage = CurrentPageIndex;
            request.PageCount = ProjectPage.PageSize;
            request.UserID = UserToEdit.UserID;
            SearchProjectsResponse response = new ProjectApplication().SearchProjects(request);
            rptProjects.DataSource = response.ResultList;
            rptProjects.DataBind();

            ProjectPage.RecordCount = response.ResultCount;
            if (ProjectPage.RecordCount > 0)
            {
                trNoProjects.Visible = false;
            }
        }


        private UsersEntity GetEntity()
        {
            UsersEntity model = new UsersEntity();
            model = UserToEdit;
            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                model.PassWord = txtPassword.Text;
                model.ForgotPassword = UserInfo.ID;
            }
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

        private string CheckInput(out bool result)
        {
            result = true;
            string msg = string.Empty;
            result = userApp.CheckPassword(txtPassword.Text, txtConfirmPassword.Text, out msg);
            return msg;
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

            string msg = "";
            if (!string.IsNullOrEmpty(txtPassword.Text) || !string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                bool result;
                msg = CheckInput(out result);
                if (!result)
                {
                    ShowMessageToClient(msg, 2, false, false);
                    return;
                }
            }

            UsersEntity user = GetEntity();
            if (userApp.UpdateUser(user))
            {
                ShowSuccessMessageToClient(false, true);
                logEntity.IsSuccess = true;
                logEntity.Description = UserInfo.UserName + " modifiy " + txtUserName.Text
                    + " password to" + txtPassword.Text + " success. new password: " + txtPassword.Text;
            }
            else
            {
                ShowFailMessageToClient(userApp.BrokenRuleMessages);
                logEntity.IsSuccess = false;
                logEntity.Description = UserInfo.UserName + " modifiy " + txtUserName.Text
                    + " password to" + txtPassword.Text + " fail. msg: " + msg;
            }
            logApplication.Write(logEntity);

        }
    }
}