using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.HSSF.Model;
using NPOI.HSSF.Record.Formula.Functions;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Entity.LogModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Admin.Users
{
    public partial class SunnetUserDetail : BasePage
    {
        UserApplication userApp;
        private EventsApplication eventsApp;

        public UsersEntity UserToEdit { get; set; }
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
        protected void Page_Load(object sender, EventArgs e)
        {
            userApp = new UserApplication();
            eventsApp = new EventsApplication();

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
            InitWorkScheDule();
        }
        private void SetControlsStatus()
        {
            bool isReadOnly = ISReadOnlyRole;

            // ddlCompany.Enabled = !isReadOnly;
            ddlRole.Enabled = !isReadOnly;
            ddlOffice.Enabled = !isReadOnly;

            txtTitle.ReadOnly = isReadOnly;
            txtFirstName.ReadOnly = isReadOnly;
            txtLastName.ReadOnly = isReadOnly;
            txtUserName.ReadOnly = isReadOnly;
            txtPhone.ReadOnly = isReadOnly;
            txtSkype.ReadOnly = isReadOnly;
            txtPassword.ReadOnly = isReadOnly;
            txtConfirmPassword.ReadOnly = isReadOnly;

            litCompanyName.Visible = isReadOnly;
            ddlCompany.Visible = !isReadOnly;

            LitStatus.Visible = isReadOnly;
            ddlStatus.Visible = !isReadOnly;

            LitRole.Visible = isReadOnly;
            ddlRole.Visible = !isReadOnly;

            LitOffice.Visible = isReadOnly;
            ddlOffice.Visible = !isReadOnly;

            LitClient.Visible = isReadOnly;
            ddlUserType.Visible = !isReadOnly;

            btnSave.Visible = !isReadOnly;

            chkNotice.Enabled = !isReadOnly;
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

            ddlRole.Items.Remove(ddlRole.Items.FindByValue(((int)RolesEnum.CLIENT).ToString()));
            ddlUserType.SelectedIndex = 0;
            ddlUserType.Enabled = false;
        }

        private void InitControl()
        {
            InitCompany();
            InitRole();
            ddlCompany.SelectedValue = "1";
            //  ddlCompany.Enabled = false;

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
            ddlUserType.Enabled = false;
            ddlUserType.SelectedValue = model.UserType;

            //ddlCompany.Enabled = false;
            litCompanyName.Text = model.CompanyName;
            ddlCompany.SelectedValue = model.CompanyID.ToString();

            ddlRole.SelectedValue = model.RoleID.ToString();
            LitRole.Text = model.Role.ToString();
            LitOffice.Text = model.Office.ToString();
            LitClient.Text = model.UserType.ToString();
            ddlOffice.SelectedValue = model.Office;

            // basec infomation
            txtFirstName.Text = model.FirstName;
            txtLastName.Text = model.LastName;
            txtUserName.Text = model.UserName;
            txtUserName.Text = model.Email;
            txtTitle.Text = model.Title;
            txtPhone.Text = model.Phone;
            txtSkype.Text = model.Skype;
            ddlStatus.SelectedValue = model.Status;

            chkNotice.Checked = model.IsNotice;
            PTOhours.Text = model.PTOHoursOfYear + "";

            InitProjectList();
        }

        protected void InitWorkScheDule()
        {
            var worktimes = eventsApp.GetWorkTime(UserToEdit.UserID);
            List<WorkTimeView> workTimeViews = new List<WorkTimeView>();
            foreach (var t in worktimes)
            {
                WorkTimeView workTimeView = new WorkTimeView();
                if (t.FromTimeType == 1)
                {
                    workTimeView.FromTime = t.FromTime + " AM";
                }
                else
                {
                    workTimeView.FromTime = t.FromTime + " PM";
                }
                if (t.ToTimeType == 1)
                {
                    workTimeView.ToTime = t.ToTime + " AM";
                }
                else
                {
                    workTimeView.ToTime = t.ToTime + " PM";
                }
                workTimeViews.Add(workTimeView);
            }
            this.rptOtherUser.DataSource = workTimeViews;
            this.rptOtherUser.DataBind();
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
            model = UserToEdit;
            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                model.PassWord = txtPassword.Text;
                model.ForgotPassword = UserInfo.ID;
            }
            // Advance Infomation Sunnet
            model.CompanyID = int.Parse(ddlCompany.SelectedValue);
            model.CompanyName = ddlCompany.SelectedItem.Text;
            model.UserType = ddlUserType.SelectedValue;
            model.Office = ddlOffice.SelectedValue;
            model.RoleID = int.Parse(ddlRole.SelectedValue);
            model.EmergencyContactFirstName = "Sunnet";
            model.EmergencyContactLastName = "Sunnet";
            model.EmergencyContactEmail = "Sunnet@sunnet.us";
            model.EmergencyContactPhone = "999-999-9999";
            model.MaintenancePlanOption = UserMaintenancePlanOption.NONE.ToString();
            model.PTOHoursOfYear = double.Parse(String.Format("{0:F}", PTOhours.Text));

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
            model.IsNotice = chkNotice.Checked;

            return model;
        }
        private List<WorkTimeEntity> BuildWorkTime()
        {
            List<WorkTimeEntity> workTimeEntities = new List<WorkTimeEntity>();

            int workintervalCount = QF("workinterval_count", 0);
            if (workintervalCount > 0)
            {
                for (int i = 1; i <= workintervalCount; i++)
                {
                    var beginTime = QF("txtBeginTimeFirst" + i).Trim();
                    var endTime = QF("txtEndTimeFirst" + i).Trim();
                    if (!string.IsNullOrEmpty(beginTime) && !string.IsNullOrEmpty(endTime))
                    {
                        var fromtimetype = beginTime.Trim().Contains("AM") ? 1 : 2;
                        var totimetype = endTime.Trim().Contains("AM") ? 1 : 2;
                        var fromtime = beginTime.Trim().Replace(" AM", "").Replace(" PM", "");
                        var totime = endTime.Trim().Replace(" AM", "").Replace(" PM", "");
                        WorkTimeEntity newEntity = new WorkTimeEntity()
                        {
                            CreateOn = DateTime.Now,
                            UserID = UserToEdit.UserID,
                            FromTimeType = fromtimetype,
                            ToTimeType = totimetype,
                            FromTime = fromtime,
                            ToTime = totime
                        };
                        workTimeEntities.Add(newEntity);
                    }
                }
            }
            return workTimeEntities;
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
            List<WorkTimeEntity> workTimes = BuildWorkTime();
            //if (user.CompanyID != UserToEdit.CompanyID)
            //{
            //    ShowFailMessageToClient("All projects Assiged on the company will be removed.");
            //    return;
            //}
            if (userApp.UpdateUser(user))
            {
                if (workTimes.Any())
                {
                    if (eventsApp.UpdateWorkTime(workTimes))
                    {
                        ShowSuccessMessageToClient(false, true);
                        logEntity.IsSuccess = true;
                        logEntity.Description = UserInfo.UserName + " modifiy " + txtUserName.Text
                                                + " password to" + txtPassword.Text + " success. new password: " +
                                                txtPassword.Text;
                        Redirect(Request.RawUrl, true);
                    }
                    else
                    {
                        ShowFailMessageToClient(userApp.BrokenRuleMessages);
                        logEntity.IsSuccess = false;
                        logEntity.Description = UserInfo.UserName + " modifiy " + txtUserName.Text
                            + " password to" + txtPassword.Text + " fail. msg: " + msg;
                    }
                }
                else
                {
                    if (eventsApp.DeleteWorkTimeByUserId(UserToEdit.UserID))
                    {
                        ShowSuccessMessageToClient(false, true);
                        logEntity.IsSuccess = true;
                        logEntity.Description = UserInfo.UserName + " modifiy " + txtUserName.Text
                            + " password to" + txtPassword.Text + " success. new password: " + txtPassword.Text;
                        Redirect(Request.RawUrl, true);
                    }
                    else
                    {
                        ShowFailMessageToClient(userApp.BrokenRuleMessages);
                        logEntity.IsSuccess = false;
                        logEntity.Description = UserInfo.UserName + " modifiy " + txtUserName.Text
                            + " password to" + txtPassword.Text + " fail. msg: " + msg;
                    }

                }
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