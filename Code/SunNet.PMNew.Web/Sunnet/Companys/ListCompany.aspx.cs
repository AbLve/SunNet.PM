using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Sunnet.Companys
{
    public partial class ListCompany : BaseWebsitePage
    {
        CompanyApplication companyApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            AddNewObject.Visible = CheckRoleCanAccessPage("/Sunnet/Companys/AddCompany.aspx");
            companyApp = new CompanyApplication();
            if (!IsPostBack)
                InitControl();

        }

        private void InitControl()
        {
            SearchCompaniesRequest request = new SearchCompaniesRequest();
            request.SearchType = SearchCompanyType.List;
            request.IsPageModel = true;
            request.CompanyName = txtKeyword.Text.NoHTML();
            request.CurrentPage = anpCompanies.CurrentPageIndex;
            request.PageCount = anpCompanies.PageSize;
            request.OrderExpression = hidOrderBy.Value;
            request.OrderDirection = hidOrderDirection.Value;
            SearchCompaniesResponse response = companyApp.SearchCompanies(request);
            rptCompanies.DataSource = response.ResultList;
            rptCompanies.DataBind();

            anpCompanies.RecordCount = response.ResultCount;
        }
        protected void anpCompanies_PageChanged(object sender, EventArgs e)
        {
            InitControl();
        }

        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            anpCompanies.CurrentPageIndex = 1;
        }
    }
}
