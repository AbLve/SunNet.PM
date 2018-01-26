using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.Admin
{
    public partial class Companies : BasePage
    {
        CompanyApplication companyApp;
        protected override string DefaultOrderBy
        {
            get
            {
                return "CompanyName";
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
            companyApp = new CompanyApplication();
            if (!IsPostBack)
            {
                txtKeyword.Text = QS("keyword");
                InitControl();
            }
        }
        private void InitControl()
        {
            SearchCompaniesRequest request = new SearchCompaniesRequest();
            request.SearchType = SearchCompanyType.List;
            request.IsPageModel = true;
            request.CompanyName = txtKeyword.Text.NoHTML();
            request.CurrentPage = CurrentPageIndex;
            request.PageCount = CompanyPage.PageSize;
            request.OrderExpression = OrderBy;
            request.OrderDirection = OrderDirection;
            SearchCompaniesResponse response = companyApp.SearchCompanies(request);
            if (response.ResultCount > 0)
            {
                rptCompanyList.DataSource = response.ResultList;
                rptCompanyList.DataBind();
                CompanyPage.RecordCount = response.ResultCount;
            }
            else
            {
                trNoTickets.Visible = true;
                rptCompanyList.Visible = false;
            } 
        }
    }
}