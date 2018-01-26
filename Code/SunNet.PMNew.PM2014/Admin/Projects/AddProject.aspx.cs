using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.ProjectModel;
using StructureMap;

namespace SunNet.PMNew.PM2014.Admin.Projects
{
    public partial class AddProject : BasePage
    {
        ProjectApplication projApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            projApp = new ProjectApplication();
            if (!IsPostBack)
            {
                InitControls();
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            //对需要加载页上的所有其他控件的任务使用该事件。
            ClientMaintenancePlan1.TotalHours = 0;
            ClientMaintenancePlan1.RemainHours = 0;
            ClientMaintenancePlan1.SelectedMaintenancePlan = UserMaintenancePlanOption.NO;
            base.OnLoadComplete(e);
        }

        #region init controls
        private void InitControls()
        {
            CompanyApplication comApp = new CompanyApplication();
            List<CompanysEntity> list = comApp.GetAllCompanies();
            list.BindDropdown(ddlCompany, "CompanyName", "ComID", "Please select", "0");
            InitPM();
            txtFreeHour.Text = "40";
        }

        private void InitPM()
        {
            SearchUsersRequest searchUsersRequest = new SearchUsersRequest(
                SearchUsersType.Role, false, " FirstName ", " ASC ");
            searchUsersRequest.Role = RolesEnum.PM;
            SearchUserResponse response = projApp.GetProjectUsers(searchUsersRequest);
            foreach (UsersEntity user in response.ResultList)
            {
                ddlPM.Items.Add(new ListItem(string.Format("{0} {1}", user.FirstName, user.LastName),
                    user.UserID.ToString()));
            }
            ddlPM.AddEmptyItem();
        }

        #endregion

        private ProjectsEntity GetEntity()
        {
            ProjectsEntity model = ProjectsFactory.CreateProject(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());

            model.BugNeedApproved = chkBugNeedApprove.Checked;
            model.CompanyID = int.Parse(ddlCompany.SelectedValue);
            model.Description = txtDesc.Text;

            DateTime dtEnd;
            if (!DateTime.TryParse(txtEndDate.Text, out dtEnd))
                dtEnd = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
            model.EndDate = dtEnd;

            model.FreeHour = int.Parse(txtFreeHour.Text);
            model.IsOverFreeTime = false;
            model.PMID = int.Parse(ddlPM.SelectedValue);
            model.Priority = ddlPriority.SelectedValue;
            model.ProjectCode = txtProjectCode.Text;
            model.RequestNeedApproved = chkRequestNeedApprove.Checked;
            model.StartDate = Convert.ToDateTime(txtStartDate.Text);
            model.Status = ddlStatus.SelectedValue.ToEnum<ProjectStatus>();
            model.TestLinkURL = txtTestUrl.Text;
            model.TestPassword = txtTestPassword.Text;
            model.TestUserName = txtTestUserName.Text;
            model.Title = txtTitle.Text.Trim();
            model.Billable = bool.Parse(rblBillable.SelectedValue);
            model.TotalHours = this.ClientMaintenancePlan1.TotalHours;
            model.MaintenancePlanOption = ClientMaintenancePlan1.SelectedMaintenancePlan.ToString();
            return model;
        }

        private bool CheckInput(out string msg)
        {
            msg = string.Empty;
            if (ddlPM.SelectedIndex == 0)
            {
                msg += "PM not null";
                return false;
            }
            if (ddlCompany.SelectedIndex == 0)
            {
                msg += "Company not null";
                return false;
            }
            if (!UtilFactory.Helpers.CommonHelper.TryIntParse(txtFreeHour.Text))
            {
                msg += "FreeHour must be a number";
                return false;
            }
            if (!UtilFactory.Helpers.CommonHelper.IsDate(txtStartDate.Text))
            {
                msg += "StartDate must be a date";
                return false;
            }
            if (!string.IsNullOrEmpty(txtEndDate.Text) && !UtilFactory.Helpers.CommonHelper.IsDate(txtEndDate.Text))
            {
                msg += "EndDate must be a date";
                return false;
            }
            if (ClientMaintenancePlan1.SelectedMaintenancePlan == UserMaintenancePlanOption.NONE)
            {
                msg += "Please select a maintenance plan.";
                return false;
            }
            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (!CheckInput(out msg))
            {
                this.ShowMessageToClient(msg, 2, false, false);
                return;
            }
            ProjectsEntity model = GetEntity();

            int id = projApp.Insert(model);
            if (id > 0)
            {

                ProjectUsersEntity projectUsersEntity = new ProjectUsersEntity();
                projectUsersEntity.ISClient = false;
                projectUsersEntity.CreatedBy = UserInfo.UserID;
                projectUsersEntity.CreatedOn = DateTime.Now;
                projectUsersEntity.ProjectID = id;
                projectUsersEntity.UserID = model.PMID;
                int result = projApp.AssignUserToProject(projectUsersEntity);
                if (result > 0)
                {
                    Redirect(string.Format("EditProject.aspx?ID={0}&returnurl={1}", id, Server.UrlEncode("/Admin/Projects/Projects.aspx")));
                }
                else
                {
                    this.ShowFailMessageToClient(projApp.BrokenRuleMessages);
                }
            }
            else
            {
                this.ShowFailMessageToClient(projApp.BrokenRuleMessages);
            }
        }
    }
}