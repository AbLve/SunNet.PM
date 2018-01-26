using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Entity.LogModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Admin.Users
{
    public partial class NewSunneter : BasePage
    {
        UserApplication userApp;
        private EventsApplication eventsApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            userApp = new UserApplication();
            eventsApp = new EventsApplication();

            if (!IsPostBack)
            {
                InitControl();
                SetControlsStatus();
            }
        }
        private void SetControlsStatus()
        {
            bool isReadOnly = ISReadOnlyRole;

            ddlCompany.Enabled = !isReadOnly;
            ddlRole.Enabled = !isReadOnly;
            ddlOffice.Enabled = !isReadOnly;
            // btnForm.Visible = !isReadOnly;

            txtTitle.ReadOnly = isReadOnly;
            txtFirstName.ReadOnly = isReadOnly;
            txtLastName.ReadOnly = isReadOnly;
            txtUserName.ReadOnly = isReadOnly;
            txtPhone.ReadOnly = isReadOnly;
            txtSkype.ReadOnly = isReadOnly;
            txtPassword.ReadOnly = isReadOnly;
            txtConfirmPassword.ReadOnly = isReadOnly;

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
            ddlCompany.Enabled = false;
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
            model.UserType = ddlUserType.SelectedValue;
            model.Office = ddlOffice.SelectedValue;
            model.RoleID = int.Parse(ddlRole.SelectedValue);


            // Advance Infomation Client
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
                            UserID = 0,
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

            bool result;
            string msg = CheckInput(out result);
            if (!result)
            {
                ShowMessageToClient(msg, 2, false, false);
                return;
            }
            UsersEntity user = GetEntity();
            List<WorkTimeEntity> workTimes = BuildWorkTime();
            int id = userApp.AddUser(user);
            if (id > 0)
            {
                if (workTimes.Any())
                {
                    foreach (var t in workTimes)
                    {
                        t.UserID = id;
                    }
                    if (eventsApp.UpdateWorkTime(workTimes))
                    {
                        Redirect("/Admin/users/SunnetUserDetail.aspx?id=" + id + "&returnurl=/admin/Users/users.aspx",
                            true);
                        // ShowSuccessMessageToClient();
                    }
                    else
                    {
                        ShowFailMessageToClient(userApp.BrokenRuleMessages);
                    }
                }
                else
                {
                    Redirect("/Admin/users/SunnetUserDetail.aspx?id=" + id + "&returnurl=/admin/Users/users.aspx", true);
                    // ShowSuccessMessageToClient();
                }
            }
            else
            {
                ShowFailMessageToClient(userApp.BrokenRuleMessages);
            }

        }
    }
}