using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.CompanyModel;

namespace SunNet.PMNew.Web.Sunnet.Admin
{
    public partial class ListUser : BaseWebsitePage
    {
        UserApplication userApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            spanAddSunneter.Visible = CheckRoleCanAccessPage("/Sunnet/Admin/AddUser.aspx");
            spanAddClient.Visible = CheckRoleCanAccessPage("/Sunnet/Admin/AddClient.aspx");
            userApp = new UserApplication();
            if (!IsPostBack)
            {
                List<CompanysEntity> list = new CompanyApplication().GetCompaniesHasUser();
                list.BindDropdown(ddlCompany, "CompanyName", "ComID", "All", "0");
                InitControl();
            }
        }
        private void InitControl()
        {
            int page = 1;
            page = anpUsers.CurrentPageIndex;
            SearchUsersRequest request = new SearchUsersRequest(SearchUsersType.List
                , true, hidOrderBy.Value, hidOrderDirection.Value);
            request.Status = ddlStatus.SelectedValue;
            request.CompanyID = int.Parse(ddlCompany.SelectedValue);
            request.Keywords = txtKeyword.Text.NoHTML();

            request.CurrentPage = page;
            request.PageCount = anpUsers.PageSize;

            SearchUserResponse response = userApp.SearchUsers(request);
            rptUsers.DataSource = response.ResultList;
            rptUsers.DataBind();

            anpUsers.RecordCount = response.ResultCount;


        }
        protected void ibtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            anpUsers.CurrentPageIndex = 1;
            InitControl();
        }
        protected void anpUsers_PageChanged(object sender, EventArgs e)
        {
            InitControl();
        }
    }
}
