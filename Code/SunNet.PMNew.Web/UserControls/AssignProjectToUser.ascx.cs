using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class AssignProjectToUser : BaseAscx
    {
        public bool isClient = false;
        private UsersEntity UserToEdit;
        protected void Page_Load(object sender, EventArgs e)
        {
            UserApplication userApp = new UserApplication();
            int id = QS("uid", 0);
            if (id == 0)
            {
                return;
            }
            UsersEntity model = userApp.GetUser(id);
            isClient = !(model.Role == RolesEnum.CLIENT);
            UserToEdit = model;
            if (!Page.IsPostBack)
            {
                InitProjectList();
            }
        }

        protected void anpProjects_PageChanged(object sender, EventArgs e)
        {
            InitProjectList();
        }

        protected void InitProjectList()
        {
            SearchProjectsRequest request;
            if (isClient)
            {
                request = new SearchProjectsRequest(SearchProjectsType.AllExceptAssigned
                  , true, hidOrderBy.Value, hidOrderDirection.Value);
                request.UserID = UserToEdit.UserID;
            }
            else
            {
                request = new SearchProjectsRequest(SearchProjectsType.CompanyExceptAssigned
                  , true, hidOrderBy.Value, hidOrderDirection.Value);
                request.CompanyID = UserToEdit.CompanyID;
                request.UserID = UserToEdit.UserID;
            }
            request.Keywords = txtKeyWord.Text.Trim();
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            anpProjects.CurrentPageIndex = 1;
            InitProjectList();
        }
    }
}