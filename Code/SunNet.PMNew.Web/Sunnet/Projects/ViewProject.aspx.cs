using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

using SunNet.PMNew.Web.Codes;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.FileModel;

namespace SunNet.PMNew.Web.Sunnet.Projects
{
    public partial class ViewProject : BaseWebsitePage
    {
        ProjectApplication projApp;
        TicketsApplication ticketApp;

        protected void Page_Load(object sender, EventArgs e)
        {

            projApp = new ProjectApplication();
            ticketApp = new TicketsApplication();
            if (!IsPostBack)
            {
                int id = QS("id", 0);
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
            List<UsersEntity> listNoPM = response.ResultList.FindAll(x => x.Role != RolesEnum.PM);
            return listNoPM;
        }
        private List<UsersEntity> GetAllCompanyUsers()
        {
            int id = QS("id", 0);
            SearchUsersRequest searchUserRequest = new SearchUsersRequest(
                SearchUsersType.CompanyByProject, false, " FirstName ", " ASC ");

            searchUserRequest.ProjectID = id;
            SearchUserResponse response = projApp.GetProjectUsers(searchUserRequest);
            return response.ResultList;
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
                chkBillable.Checked = model.Billable;
                chkBugNeedApprove.Checked = model.BugNeedApproved;
                ddlCompany.SelectedValue = model.CompanyID.ToString();
                txtDesc.Text = model.Description;
                txtEndDate.Text = model.EndDate.ToString("MM/dd/yyyy");
                txtFreeHour.Text = model.FreeHour.ToString();
                ltlFreeHour.Text = model.FreeHour.ToString();
                chkIsOverFreeTime.Checked = model.IsOverFreeTime;
                chkIsOverFreeTime.Text = chkIsOverFreeTime.Checked ? "Yes" : "No";
                if (UserInfo.Role == RolesEnum.Sales)
                {
                    ltlFreeHour.Visible = false;
                }
                else
                {
                    txtFreeHour.Visible = false;
                }
                //model.IsOverFreeTime = false;
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

                InitFiles();
            }
        }

        private void InitFiles()
        {
            int id = QS("id", 0);
            FileApplication fileApp = new FileApplication();
            SearchFilesRequest request = new SearchFilesRequest(SearchFileType.Project, false, "FileTitle", "ASC");
            request.ProjectID = id;
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

    }
}
