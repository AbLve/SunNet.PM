using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Admin
{
    public partial class NewCompany : BasePage
    {
        CompanyApplication companyApp = new CompanyApplication();
        protected void Page_Load(object sender, EventArgs e)
        {
            //((Pop) this.Master).Width = 800;
        }
        protected CompanysEntity GetEntity()
        {
            CompanysEntity model = CompanyFactory.CreateCompanys(UserInfo.UserID, ObjectFactory.GetInstance<ISystemDateTime>());

            model.Address1 = txtAddress1.Text.Trim().NoHTML();
            model.Address2 = txtAddress2.Text.Trim().NoHTML();
            model.AssignedSystemUrl = "http://client.sunnet.us";
            model.City = txtCity.Text.Trim().NoHTML();
            model.CompanyName = txtCompanyName.Text.Trim().NoHTML();
            model.CreateUserName = UserInfo.UserName;
            model.Fax = txtFax.Text;
            model.Logo = "/Images/logomain.jpg";
            model.Phone = txtPhone.Text;
            model.State = ddlState.SelectedItem.Text;
            model.Status = "ACTIVE";
            model.Website = txtWebsite.Text.Trim();

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
                this.ShowMessageToClient(msg, 2, false, false);
                return;
            }
            CompanysEntity model = GetEntity();

            int id = companyApp.AddCompany(model);
            if (id > 0)
            {
                Redirect(Request.RawUrl, false, true);
               // this.ShowSuccessMessageToClient();
                //Response.Redirect(string.Format("EditCompany.aspx?id={0}", id.ToString()));
            }
            else
                this.ShowFailMessageToClient(companyApp.BrokenRuleMessages);
        }
    }
}