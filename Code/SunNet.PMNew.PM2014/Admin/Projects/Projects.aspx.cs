using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.PM2014.Admin.Projects
{
    public partial class Projects : BasePage
    {
        protected override string DefaultOrderBy
        {
            get
            {
                return "ModifiedOn";
            }
        }

        protected override string DefaultDirection
        {
            get { return "DESC"; }
        }

        ProjectApplication projApp = new ProjectApplication();
        CompanyApplication companyApplication = new CompanyApplication();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<CompanysEntity> list = companyApplication.GetCompaniesHasProject();
                list.BindDropdown(ddlCompany, "CompanyName", "ComID", "All", "0", QS("company"));
                InitControl();
            }
        }

        private void InitControl()
        {
            SearchProjectsRequest request = new SearchProjectsRequest(SearchProjectsType.List, true, OrderBy, OrderDirection);

            request.Keywords = QS("keywork");

            request.CurrentPage = CurrentPageIndex;
            request.PageCount = anpWaitting.PageSize;
            request.CompanyID = int.Parse(ddlCompany.SelectedValue);
            SearchProjectsResponse response = projApp.SearchProjects(request);
            rptProjects.DataSource = response.ResultList;
            rptProjects.DataBind();
            anpWaitting.RecordCount = response.ResultCount;
            if (response.ResultCount == 0)
                this.trNoTickets.Visible = true;
        }
    }
}