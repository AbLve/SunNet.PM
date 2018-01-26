using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.ProjectModel;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using System.Text;
using SunNet.PMNew.Entity.FileModel;

namespace SunNet.PMNew.PM2014.Admin.Projects
{
    public partial class EditProject : BasePage
    {
        ProjectApplication projApp;
        TicketsApplication ticketApp;
        UserApplication userApp;


        protected void Page_Load(object sender, EventArgs e)
        {
            projApp = new ProjectApplication();
            ticketApp = new TicketsApplication();
            userApp = new UserApplication();
            int id = QS("id", 0);
            if (!IsPostBack)
            {

                if (id == 0)
                {
                    this.ShowArgumentErrorMessageToClient();
                }
                else
                    InitControls();
            }
        }

        private void InitPM()
        {
            SearchUsersRequest searchUsersRequest = new SearchUsersRequest(
                SearchUsersType.Role, false, " FirstName ", " ASC ");
            searchUsersRequest.Role = RolesEnum.PM;

            SearchUserResponse response = projApp.GetProjectUsers(searchUsersRequest);
            foreach (UsersEntity user in response.ResultList)
            {
                ddlPM.Items.Add(new ListItem(string.Format("{0} {1}", user.FirstName, user.LastName), user.UserID.ToString()));
            }
        }
        
        private List<UsersEntity> GetAllProjectUsers()
        {
            int id = QS("id", 0);
            SearchUsersRequest searchUserRequest = new SearchUsersRequest(
                SearchUsersType.Project, false, " FirstName ", " ASC ");
            searchUserRequest.ProjectID = id;

            SearchUserResponse response = projApp.GetProjectUsers(searchUserRequest);
            return response.ResultList;
        }


        private void InitUsers(int companyId)
        {
            List<UsersEntity> listCurrentProject = GetAllProjectUsers();
            var projectId = QS("ID", 0);
            List<UsersEntity> listUsers = userApp.GetActiveUserList().FindAll(u => u.UserType == "SUNNET"
                || (u.UserType == "CLIENT" && u.CompanyID == companyId));
            
            var tmpList = listUsers.Select(x => new
            {
                x.ID,
                ProjectID = projectId,
                x.FirstName,
                x.LastName,
                x.FirstAndLastName,
                x.LastNameAndFirst,
                Selected = listCurrentProject.Any(r => r.UserID == x.ID),
                x.Role
            }).ToList();

            rptDev.DataSource = tmpList.FindAll(r => r.Role == RolesEnum.DEV || r.Role == RolesEnum.Leader
                || r.Role == RolesEnum.Contactor).OrderBy(r => r.FirstName);
            rptDev.DataBind();

            rptQA.DataSource = tmpList.FindAll(r => r.Role == RolesEnum.QA).OrderBy(r => r.FirstName);
            rptQA.DataBind();

            rptUS.DataSource = tmpList.FindAll(r => r.Role == RolesEnum.ADMIN || r.Role == RolesEnum.PM
                || r.Role == RolesEnum.Sales || r.Role == RolesEnum.Supervisor).OrderBy(r => r.FirstName);
            rptUS.DataBind();

            rptClient.DataSource = tmpList.FindAll(r => r.Role == RolesEnum.CLIENT).OrderBy(r => r.FirstName);
            rptClient.DataBind();
        }


        private void InitControls()
        {
            InitPM();

            int id = QS("id", 0);
            ProjectsEntity model = projApp.Get(id);
            if (model == null)
            {
                this.ShowFailMessageToClient(projApp.BrokenRuleMessages, true);
            }
            else
            {
                rblBillable.SelectedValue = model.Billable.ToString().ToLower();
                chkBugNeedApprove.Checked = model.BugNeedApproved;

                CompanysEntity companyEntity = new CompanyApplication().GetCompany(model.CompanyID);
                if (companyEntity == null)
                    lblCompany.InnerText = "";
                else
                    lblCompany.InnerText = companyEntity.CompanyName;

                txtDesc.Text = model.Description;
                txtEndDate.Text = model.EndDate.ToText();
                txtFreeHour.Text = model.FreeHour.ToString();
                ltlFreeHour.Text = model.FreeHour.ToString();
                if (UserInfo.Role == RolesEnum.Sales)
                {
                    ltlFreeHour.Visible = false;
                }
                else
                {
                    txtFreeHour.Visible = false;
                }
                ltlFreeHourText.Text = model.IsOverFreeTime ? "<font color='red'>The project has been over free hour</font>" : "The project hasn't been over free hour";

                if (null == ddlPM.Items.FindByValue(model.PMID.ToString()))
                {
                    UsersEntity usersEntity = new UserApplication().GetUser(model.PMID);
                    ddlPM.Items.Add(new ListItem() { Text = string.Format("{0} {1}", usersEntity.FirstName, usersEntity.LastName), Value = usersEntity.UserID.ToString() });
                }
                ddlPM.SelectedValue = model.PMID.ToString();
                ddlPriority.SelectedValue = model.Priority;
                txtProjectCode.Text = model.ProjectCode;
                chkRequestNeedApprove.Checked = model.RequestNeedApproved;
                txtStartDate.Text = model.StartDate.ToString("MM/dd/yyyy");
                ddlStatus.SelectedValue = ((int)model.Status).ToString();
                txtTestUrl.Text = model.TestLinkURL;
                txtTestPassword.Text = model.TestPassword;
                txtTestUserName.Text = model.TestUserName;
                txtTitle.Text = model.Title;
                this.ClientMaintenancePlan1.TotalHours = model.TotalHours;
                this.ClientMaintenancePlan1.RemainHours = model.TotalHours - GetRemainHours(id);
                ClientMaintenancePlan1.SelectedMaintenancePlan = model.MainPlanOption;
                InitUsers(model.CompanyID);
                InitFiles();
                InitPrincipal();
            }
        }



        private void InitPrincipal()
        {
            List<ProjectPrincipalEntity> list = projApp.GetProjectPrincipal(QS("id", 0));
            if (list.Count <= 0)
            {
                trNoPri.Visible = true;
            }
            else
            {
                rptPrincipals.DataSource = list;
                rptPrincipals.DataBind();
                trNoPri.Visible = false;
            }
        }

        private void InitFiles()
        {
            int id = QS("id", 0);
            FileApplication fileApp = new FileApplication();
            SearchFilesRequest request = new SearchFilesRequest(SearchFileType.Project, false, "FileTitle", "ASC");
            request.ProjectID = id;
            request.IsPublic = UserInfo.Role == RolesEnum.CLIENT;
            List<FileDetailDto> list = fileApp.GetFiles(request);

            if (list == null || list.Count == 0)
            {
                trNoProject.Visible = true;
            }
            else
            {
                trNoProject.Visible = false;
                rptFiles.DataSource = list;
                rptFiles.DataBind();
            }
        }

        #region Save basic infos
        private bool CheckInput(out string msg)
        {
            msg = string.Empty;

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
            return true;
        }

        private ProjectsEntity GetEntity(out int OldPMId, out bool isTotalHoursChanged)
        {
            int id = QS("id", 0);
            ProjectsEntity model = projApp.Get(id);
            model.Billable = bool.Parse(rblBillable.SelectedValue);
            model.BugNeedApproved = chkBugNeedApprove.Checked;
            model.Description = txtDesc.Text;
            DateTime dtEnd;
            if (!DateTime.TryParse(txtEndDate.Text, out dtEnd))
                dtEnd = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
            model.EndDate = dtEnd;
            if (txtFreeHour.Visible == true && txtFreeHour.Text != model.FreeHour.ToString())
            {
                model.FreeHour = int.Parse(txtFreeHour.Text);
                model.IsOverFreeTime = false;
            }
            if (model.PMID != int.Parse(ddlPM.SelectedValue))
                OldPMId = model.PMID;
            else
                OldPMId = 0;

            model.PMID = int.Parse(ddlPM.SelectedValue);
            model.Priority = ddlPriority.SelectedValue;
            model.ProjectCode = txtProjectCode.Text;
            model.RequestNeedApproved = chkRequestNeedApprove.Checked;
            model.StartDate = Convert.ToDateTime(txtStartDate.Text);
            model.Status = ddlStatus.SelectedValue.ToEnum<ProjectStatus>();
            model.TestLinkURL = txtTestUrl.Text;
            model.TestPassword = txtTestPassword.Text;
            model.TestUserName = txtTestUserName.Text;
            model.Title = txtTitle.Text;
            model.ModifiedOn = DateTime.Now;
            model.ModifiedBy = UserInfo.UserID;
            model.MaintenancePlanOption = ClientMaintenancePlan1.SelectedMaintenancePlan.ToString();
            if (model.TotalHours != this.ClientMaintenancePlan1.TotalHours)
            {
                isTotalHoursChanged = true;
            }
            else
            {
                isTotalHoursChanged = false;
            }
            model.TotalHours = this.ClientMaintenancePlan1.TotalHours;
            return model;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (!CheckInput(out msg))
            {
                this.ShowMessageToClient(msg, 2, false, false);
                return;
            }
            int OldPMId;
            bool isTotalHoursChanged;
            ProjectsEntity model = GetEntity(out OldPMId, out isTotalHoursChanged);
            if (model.MainPlanOption == UserMaintenancePlanOption.HAS && isTotalHoursChanged)
            {
                projApp.updateRemainHoursSendEmailStatus(false, model.ProjectID);
            }
            if (projApp.Update(model))
            {
                if (OldPMId > 0)
                    ticketApp.UpdateTicketPM(OldPMId, model.PMID, model.ProjectID);
                this.ClientMaintenancePlan1.RemainHours = model.TotalHours - GetRemainHours(model.ID);
                Redirect(string.Format("EditProject.aspx?ID={0}&returnurl={1}", model.ProjectID, QS("returnurl")), true, false);
            }
            else
            {
                this.ShowFailMessageToClient(projApp.BrokenRuleMessages);
            }
        }
        #endregion


        protected float GetRemainHours(int projectID)
        {
            return projApp.GetProjectTimeSheetTime(projectID);
        }

    }
}