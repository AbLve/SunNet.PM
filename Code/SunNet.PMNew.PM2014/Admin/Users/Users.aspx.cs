using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Admin.Users
{
    public partial class Users : BasePage
    {
        private Dictionary<string, string> _offices;
        private Dictionary<string, string> Offices
        {
            get
            {
                if (_offices == null)
                {
                    _offices = new Dictionary<string, string>();
                    _offices.Add("CN", "CN");
                    _offices.Add("US", "US");
                    _offices.Add("AO", "Administration Office");
                    _offices.Add("D1", "Department 1");
                    _offices.Add("D2", "Department 2");
                }
                return _offices;
            }
        }

        protected string GetOffice(string key)
        {
            if (Offices.Keys.Contains(key))
                return Offices[key];
            return "Unknown";
        }

        UserApplication userApp;
        protected override string DefaultOrderBy
        {
            get
            {
                return "UserName";
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
            // spanAddSunneter.Visible = CheckRoleCanAccessPage("/Sunnet/Admin/AddUser.aspx");
            //  spanAddClient.Visible = CheckRoleCanAccessPage("/Sunnet/Admin/AddClient.aspx");
            userApp = new UserApplication();
            if (!IsPostBack)
            {

                txtKeyword.Text = QS("keyword");
                if (QS("status") != "" && QS("status") != "0")
                    ddlStatus.SelectedValue = QS("status");
                if (QS("company") != "" && QS("company") != "0")
                    ddlCompany.SelectedValue = QS("company");
                List<CompanysEntity> list = new CompanyApplication().GetCompaniesHasUser();
                list.BindDropdown(ddlCompany, "CompanyName", "ComID", "All", "0");
                InitControl();
            }
        }
        private void InitControl()
        {

            SearchUsersRequest request = new SearchUsersRequest(SearchUsersType.List
                , true, OrderBy, OrderDirection);
            request.Status = ddlStatus.SelectedValue;
            request.CompanyID = int.Parse(ddlCompany.SelectedValue);
            request.Keywords = txtKeyword.Text.NoHTML();

            request.CurrentPage = CurrentPageIndex;
            request.PageCount = UsersPage.PageSize;

            SearchUserResponse response = userApp.SearchUsers(request);
            rptUsers.DataSource = response.ResultList;
            rptUsers.DataBind();

            UsersPage.RecordCount = response.ResultCount;


        }

    }
}