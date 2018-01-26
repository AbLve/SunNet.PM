using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.HSSF.Model;
using NPOI.HSSF.Record.Formula.Functions;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Admin.Users
{
    public partial class AssignProjectToUser : BasePage
    {
        public bool isClient = false;
        private UsersEntity UserToEdit;
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
            ((Pop)this.Master).Width = 800;
            UserApplication userApp = new UserApplication();
            int id = QS("uid", 0);
            if (id == 0)
            {
                return;
            }
            UsersEntity model = userApp.GetUser(id);
            isClient = (model.Role == RolesEnum.CLIENT || model.Role == RolesEnum.Supervisor);
            UserToEdit = model;
            if (!Page.IsPostBack)
            {
                txtKeyword.Text = QS("keyword");
                InitProjectList();
            }
        }

        protected void InitProjectList()
        {
            SearchProjectsRequest request;
            if (!isClient)
            {
                request = new SearchProjectsRequest(SearchProjectsType.AllExceptAssigned
                  , true, "ProjectCode", "ASC");
                request.UserID = UserToEdit.UserID;
            }
            else
            {
                request = new SearchProjectsRequest(SearchProjectsType.CompanyExceptAssigned
                  , true, DefaultOrderBy, DefaultDirection);
                request.CompanyID = UserToEdit.CompanyID;
                request.UserID = UserToEdit.UserID;
            }
            request.Keywords = txtKeyword.Text.Trim();
            request.CurrentPage = CurrentPageIndex;
            request.PageCount = ProjectPage.PageSize;
            request.UserID = UserToEdit.UserID;
            SearchProjectsResponse response = new ProjectApplication().SearchProjects(request);
            List<ProjectDetailDTO> ResultList = response.ResultList;
            if (ResultList != null && ResultList.Count > 0)
            {
                // ResultList.RemoveAll(r => r.Status == (int)ProjectStatus.Cancelled || r.Status == (int)ProjectStatus.Completed);
                rptProjects.DataSource = ResultList;
                rptProjects.DataBind();
                ProjectPage.RecordCount = response.ResultCount;
                trNoProjects.Visible = false;
            }

        }

    }
}