using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using StructureMap;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel;
using System.Text;
using SunNet.PMNew.Entity.FileModel;
using System.IO;

namespace SunNet.PMNew.Web.Sunnet.Projects
{
    public partial class EditProject : BaseWebsitePage
    {
        ProjectApplication projApp;
        TicketsApplication ticketApp;
        private void SetControlsStatus()
        {
            //bool isReadOnly = ISReadOnlyRole;
            bool isReadOnly = !CheckRoleCanAccessPage("/Sunnet/Projects/EditProject.aspx");

            savebasicinfo.Visible = !isReadOnly;
            fileform.Visible = !isReadOnly;
            btnSaveSunnet.Visible = !isReadOnly;

            txtDesc.ReadOnly = isReadOnly;
            txtEndDate.ReadOnly = isReadOnly;
            txtFileTitle.ReadOnly = isReadOnly;
            txtFreeHour.ReadOnly = isReadOnly;
            txtProjectCode.ReadOnly = isReadOnly;
            txtStartDate.ReadOnly = isReadOnly;
            txtTestPassword.ReadOnly = isReadOnly;
            txtTestUrl.ReadOnly = isReadOnly;
            txtTestUserName.ReadOnly = isReadOnly;
            txtTitle.ReadOnly = isReadOnly;
            if (UserInfo.Role == RolesEnum.PM)
            {
                savebasicinfo.Visible = true;
                btnSaveSunnet.Visible = true;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            projApp = new ProjectApplication();
            ticketApp = new TicketsApplication();
            int id = QS("id", 0);
            if (!IsPostBack)
            {

                if (id == 0)
                {
                    this.ShowArgumentErrorMessageToClient();
                }
                else
                    InitControls();

                SetControlsStatus();
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

        private void InitCompany()
        {
            CompanyApplication comApp = new CompanyApplication();
            List<CompanysEntity> list = comApp.GetAllCompanies();
            list.BindDropdown(ddlCompany, "CompanyName", "ComID");
        }

        private List<UsersEntity> GetAllSunnetUsers()
        {
            int id = QS("id", 0);
            SearchUsersRequest searchUserRequest = new SearchUsersRequest(
                SearchUsersType.All, false, " FirstName ", " ASC ");

            searchUserRequest.IsSunnet = true;

            SearchUserResponse response = projApp.GetProjectUsers(searchUserRequest);
            return response.ResultList;
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

        protected string GetUsersHtml(RolesEnum role)
        {
            List<UsersEntity> listAll = GetAllSunnetUsers().FindAll(u => u.Role != RolesEnum.CLIENT);
            List<UsersEntity> listtarget = listAll.FindAll(u => u.Role == role && u.Status != "INACTIVE");
            StringBuilder htmls = new StringBuilder();
            foreach (UsersEntity user in listtarget)
            {
                htmls.AppendFormat("<li userid='{0}'>{1}</li>", user.ID, user.FirstName);
            }
            return htmls.ToString();
        }

        private void InitSunnetUsers()
        {
            List<UsersEntity> listCurrentProject = GetAllProjectUsers().FindAll(u => u.Role != RolesEnum.CLIENT);
            if (listCurrentProject == null && listCurrentProject.Count < 1)
            {
                hidSelectedSunneters.Value = "[]";
            }
            else
                hidSelectedSunneters.Value = GetUsersJson(listCurrentProject);
        }

        private string GetUsersJson(List<UsersEntity> listCurrentProject)
        {
            StringBuilder strSelected = new StringBuilder();
            strSelected.Append("[{\"id\":0}");
            foreach (UsersEntity item in listCurrentProject)
            {
                strSelected.Append(",{\"id\":");
                strSelected.Append(item.ID);
                strSelected.Append("}");
            }
            strSelected.Append("]");
            return strSelected.ToString();
        }

        private void InitControls()
        {
            InitPM();
            InitCompany();

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
                ddlCompany.SelectedValue = model.CompanyID.ToString();
                ddlCompany.Enabled = false;
                txtDesc.Text = model.Description;
                txtEndDate.Text = model.EndDate.ToString("MM/dd/yyyy");
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
                ddlStatus.SelectedValue = model.Status.ToString();
                txtTestUrl.Text = model.TestLinkURL;
                txtTestPassword.Text = model.TestPassword;
                txtTestUserName.Text = model.TestUserName;
                txtTitle.Text = model.Title;
                this.ClientMaintenancePlan1.TotalHours = model.TotalHours;
                this.ClientMaintenancePlan1.RemainHours = model.TotalHours - GetRemainHours(id);
                ClientMaintenancePlan1.SelectedMaintenancePlan = model.MainPlanOption;
                InitSunnetUsers();
                InitFiles();
                InitUsers();
                InitPrincipal();
            }
        }

        private void InitUsers()
        {
            List<UsersEntity> list = new ProjectApplication().GetPojectClientUsers(QS("id", 0), QS("companyid", 0));
            if (list.Count <= 0)
            {
                trNoUser.Visible = true;
            }
            else
            {
                rptUsers.DataSource = list;
                rptUsers.DataBind();
                trNoUser.Visible = false;
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
                rptPri.DataSource = list;
                rptPri.DataBind();
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
            if (!UtilFactory.Helpers.CommonHelper.IsDate(txtEndDate.Text))
            {
                msg += "EndDate must be a date";
                return false;
            }
            return true;
        }

        private ProjectsEntity GetEntity(out bool isChangeProject, out bool isTotalHoursChanged)
        {
            int id = QS("id", 0);
            ProjectsEntity model = projApp.Get(id);
            model.Billable = bool.Parse(rblBillable.SelectedValue);
            model.BugNeedApproved = chkBugNeedApprove.Checked;
            model.CompanyID = int.Parse(ddlCompany.SelectedValue);
            model.Description = txtDesc.Text;
            model.EndDate = Convert.ToDateTime(txtEndDate.Text);
            if (txtFreeHour.Visible == true && txtFreeHour.Text != model.FreeHour.ToString())
            {
                model.FreeHour = int.Parse(txtFreeHour.Text);
                model.IsOverFreeTime = false;
            }
            if (model.PMID != int.Parse(ddlPM.SelectedValue))
                isChangeProject = true;
            else
                isChangeProject = false;

            model.PMID = int.Parse(ddlPM.SelectedValue);
            model.Priority = ddlPriority.SelectedValue;
            model.ProjectCode = txtProjectCode.Text;
            model.RequestNeedApproved = chkRequestNeedApprove.Checked;
            model.StartDate = Convert.ToDateTime(txtStartDate.Text);
            model.Status = int.Parse(ddlStatus.SelectedValue);
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
            bool isChangeProject;
            bool isTotalHoursChanged;
            ProjectsEntity model = GetEntity(out isChangeProject, out isTotalHoursChanged);
            if (model.MainPlanOption == UserMaintenancePlanOption.HAS && isTotalHoursChanged)
            {
                projApp.updateRemainHoursSendEmailStatus(false, model.ProjectID);
            }
            if (projApp.Update(model))
            {
                if (isChangeProject)
                    ticketApp.UpdateTicketPM(model.PMID, model.ProjectID);
                this.ClientMaintenancePlan1.RemainHours = model.TotalHours - GetRemainHours(model.ID);
                this.ShowSuccessMessageToClient(false, false);
            }
            else
            {
                this.ShowFailMessageToClient(projApp.BrokenRuleMessages);
            }
        }
        #endregion

        private void AssignUsers(string userids, bool isClient)
        {
            string[] users = userids.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int id = QS("id", 0);
            if (projApp.RemoveAllUsers(id, isClient))
            {
                List<BrokenRuleMessage> listmsgs = new List<BrokenRuleMessage>();
                foreach (string user in users)
                {
                    ProjectUsersEntity model = ProjectsFactory.CreateProjectUser(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
                    model.ProjectID = id;
                    model.UserID = int.Parse(user);
                    model.ISClient = QueryISClient(user);
                    model.ISClient = isClient;
                    if (projApp.AssignUserToProject(model) < 0)
                    {
                        RecordMsg(listmsgs, projApp.BrokenRuleMessages);
                    }
                }
                if (listmsgs.Count > 0)
                {
                    this.ShowFailMessageToClient();
                }
                else
                {
                    if (!isClient)
                        InitSunnetUsers();
                    this.ShowSuccessMessageToClient(false, false);
                }
            }
        }

        private void RecordMsg(List<BrokenRuleMessage> listmsgs
            , List<BrokenRuleMessage> listBrokenMsgs)
        {
            foreach (BrokenRuleMessage msg in listBrokenMsgs)
            {
                listmsgs.Add(msg);
            }
        }

        private bool QueryISClient(string userid)
        {
            SearchUsersRequest searchUserRequest = new SearchUsersRequest(
                SearchUsersType.SingleInstance, false, " FirstName ", " ASC ");
            searchUserRequest.UserID = int.Parse(userid);
            SearchUserResponse response = projApp.GetProjectUsers(searchUserRequest);
            if (response.ResultList != null && response.ResultList.Count > 0)
            {
                return response.ResultList[0].Role == RolesEnum.CLIENT;
            }
            return false;
        }

        protected void btnSaveSunnet_Click(object sender, EventArgs e)
        {
            AssignUsers(hidSelectedSunneters.Value, false);
        }

        protected void btnSaveFiles_Click(object sender, EventArgs e)
        {
            int id = QS("id", 0);
            int companyid = QS("companyid", 0);
            if (fileProject.HasFile)
            {
                FilesEntity model = FileFactory.CreateFileEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
                model.FilePath = UtilFactory.Helpers.FileHelper.SaveUploadFiles("Project", id, fileProject.PostedFile); ;

                model.CompanyID = companyid;
                model.ContentType = fileProject.PostedFile.ContentType;
                model.FileID = 0;
                model.FileSize = fileProject.PostedFile.ContentLength;
                if (string.IsNullOrEmpty(txtFileTitle.Text))
                {
                    model.FileTitle = Path.GetFileName(fileProject.FileName);
                    model.FileTitle = model.FileTitle.Substring(0, model.FileTitle.LastIndexOf("."));
                }
                else
                {
                    model.FileTitle = txtFileTitle.Text;
                }
                model.IsDelete = false;
                model.IsPublic = UserInfo.Role == RolesEnum.CLIENT;
                model.SourceType = (int)FileSourceType.Project;
                model.ProjectId = id;
                model.ThumbPath = Path.GetFileName(fileProject.PostedFile.FileName);
                model.IsDelete = false;

                FileApplication fileApp = new FileApplication();
                int result = fileApp.AddFile(model);

                OperateDocManagements.OperateDocManagementSoapClient client = new OperateDocManagements.OperateDocManagementSoapClient();
                List<FilesEntity> clientFiles = new List<FilesEntity>();
                clientFiles.Add(model);
                client.AddDocManagement(Newtonsoft.Json.JsonConvert.SerializeObject(clientFiles));

                if (result <= 0)
                {
                    this.ShowFailMessageToClient(fileApp.BrokenRuleMessages, false);
                }
                else
                {
                    InitFiles();
                    this.ShowSuccessMessageToClient(false, false);
                    txtFileTitle.Text = "";
                }
            }
        }

        protected float GetRemainHours(int projectID)
        {
            return projApp.GetProjectTimeSheetTime(projectID);
        }
    }
}
