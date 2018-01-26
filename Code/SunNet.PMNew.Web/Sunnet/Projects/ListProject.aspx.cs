using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.CompanyModel;

namespace SunNet.PMNew.Web.Sunnet.Projects
{
    public partial class ListProject : BaseWebsitePage
    {
        ProjectApplication projApp;
        CompanyApplication companyApplication = new CompanyApplication();
        protected void Page_Load(object sender, EventArgs e)
        {
            AddNewObject.Visible = CheckRoleCanAccessPage("/Sunnet/Projects/AddProject.aspx");
            projApp = new ProjectApplication();
            if (!IsPostBack)
            {
                List<CompanysEntity> list = companyApplication.GetCompaniesHasProject();
                list.BindDropdown(ddlCompany, "CompanyName", "ComID", "All", "0");
                InitControl();
            }
        }
        private void InitControl()
        {
            SearchProjectsRequest request = new SearchProjectsRequest(SearchProjectsType.List, true, hidOrderBy.Value, hidOrderDirection.Value);

            request.Keywords = txtKeyword.Text;

            request.CurrentPage = anpProjects.CurrentPageIndex;
            request.PageCount = anpProjects.PageSize;
            request.CompanyID = int.Parse(ddlCompany.SelectedValue);
            SearchProjectsResponse response = projApp.SearchProjects(request);
            rptProjects.DataSource = response.ResultList;
            rptProjects.DataBind();

            anpProjects.RecordCount = response.ResultCount;
            //ltlTotal.Text = request.RecordCount.ToString();
        }
        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            anpProjects.CurrentPageIndex = 1;
            InitControl();
        }
        protected void anpProjects_PageChanged(object sender, EventArgs e)
        {
            InitControl();
        }

    }
}
