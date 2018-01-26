using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Account
{
    public partial class Company : BasePage
    {
        CompanyApplication companyApp = new CompanyApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = UserInfo.CompanyID;
                InitControls(id);
            }
        }
        private void InitControls(int id)
        {
            CompanysEntity model = companyApp.GetCompany(id);

            txtAddress1.Text = model.Address1;
            txtAddress2.Text = model.Address2;
            //model.AssignedSystemUrl = "http://client.sunnet.us";
            txtCity.Text = model.City;
            txtCompanyName.Text = model.CompanyName;
            txtFax.Text = model.Fax;
            txtPhone.Text = model.Phone;
            ListItem state = ddlState.Items.FindByText(model.State);
            if (state != null)
            {
                state.Selected = true;
            }
            txtWebsite.Text = model.Website;

            InitProjects();
            InitUsers();
        }



        private void InitUsers()
        {
            int id = UserInfo.CompanyID;
            SearchUsersRequest request = new SearchUsersRequest(
                SearchUsersType.Company, false, " FirstName ", " ASC ");
            request.CompanyID = id;

            SearchUserResponse response = companyApp.SearchUsers(request);
            if (response.ResultList.Count <= 0)
            {
                trNoUser.Visible = true;
            }
            else
            {
                rptUsers.DataSource = response.ResultList;
                rptUsers.DataBind();
                trNoUser.Visible = false;
            }

        }

        private void InitProjects()
        {
            int id = UserInfo.CompanyID;

            ProjectApplication projApp = new ProjectApplication();
            SearchProjectsRequest request = new SearchProjectsRequest(SearchProjectsType.Company, false, " Title ", " ASC ");
            request.CompanyID = id;

            SearchProjectsResponse response = projApp.SearchProjects(request);
            if (response.ResultList == null || response.ResultList.Count <= 0)
            {
                trNoProject.Visible = true;
            }
            else
            {
                rptProjects.DataSource = response.ResultList;
                rptProjects.DataBind();
                trNoProject.Visible = false;
            }
        }
        protected CompanysEntity GetEntity()
        {
            int id = UserInfo.CompanyID;
            CompanysEntity model = companyApp.GetCompany(id);
            model.Address1 = txtAddress1.Text;
            model.Address2 = txtAddress2.Text;
            //model.AssignedSystemUrl = "http://client.sunnet.us";
            model.City = txtCity.Text;
            model.CompanyName = txtCompanyName.Text;
            model.CreateUserName = UserInfo.UserName;
            model.Fax = txtFax.Text;
            //model.Logo = "/Images/nologo.jpg";
            model.Phone = txtPhone.Text;
            model.State = ddlState.SelectedItem.Text;
            //model.Status = "ACTIVE";
            model.Website = txtWebsite.Text;

            return model;
        }

        private bool CheckInput(out string msg)
        {
            msg = string.Empty;
            if (ddlState.SelectedValue == "0")
            {
                msg = "Please select a state";
                return false;
            }
            return true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (!CheckInput(out msg))
            {
                ShowMessageToClient(msg, 2, false, false);
                return;
            }
            CompanysEntity model = GetEntity();

            if (companyApp.UpdateCompany(model))
            {
                Redirect(Request.RawUrl, true);
            }
            else
            {
                ShowFailMessageToClient(companyApp.BrokenRuleMessages, false);
            }
        }

    }
}