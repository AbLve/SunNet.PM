using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.CompanyModel;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web
{
    public partial class Register : GlobalPageBase
    {
        //CompanyApplication comApp;
        //UserApplication userApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            return; //取消首页注册链接
            //comApp = new CompanyApplication();
            //userApp = new UserApplication();

        }
        //private bool CheckInput(out string msg)
        //{
        //    msg = string.Empty;
        //    if (string.IsNullOrEmpty(txtCompanyName.Text))
        //    {
        //        msg = "Company Name Required.";
        //        return false;
        //    }
        //    if (string.IsNullOrEmpty(txtFirstName.Text))
        //    {
        //        msg = "First Name Required.";
        //        return false;
        //    }
        //    if (string.IsNullOrEmpty(txtLastName.Text))
        //    {
        //        msg = "Last Name Required.";
        //        return false;
        //    }
        //    if (string.IsNullOrEmpty(txtUserName.Text))
        //    {
        //        msg = "User Name Required.";
        //        return false;
        //    }
        //    if (!string.Equals(txtUserName.Text, txtUserNameConfirm.Text))
        //    {
        //        msg = "User Name do not match.";
        //        return false;
        //    }
        //    if (!userApp.CheckPassword(txtPassword.Text, txtPasswordConfirm.Text, out msg))
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        //private CompanysEntity GetCompany()
        //{
        //    string nullText = " "; ;
        //    CompanysEntity model = CompanyFactory.CreateCompanys(1, ObjectFactory.GetInstance<ISystemDateTime>());
        //    model.CompanyName = txtCompanyName.Text;
        //    model.Address1 = nullText;
        //    model.Address2 = nullText;
        //    model.AssignedSystemUrl = "http://client.sunnet.us";
        //    model.City = nullText;
        //    model.CreateUserName = "Registe by client";
        //    model.Fax = "";
        //    model.Logo = "/Images/nologo.jpg";
        //    model.Phone = "";
        //    model.State = "";
        //    model.Status = "ACTIVE";
        //    model.Website = "";

        //    return model;
        //}
        //private UsersEntity GetUser()
        //{
        //    UsersEntity model = UsersFactory.CreateClient(1, ObjectFactory.GetInstance<ISystemDateTime>());
        //    model.CompanyName = txtCompanyName.Text;
        //    model.FirstName = txtFirstName.Text;
        //    model.LastName = txtLastName.Text;
        //    model.UserName = txtUserName.Text;
        //    model.PassWord = txtPassword.Text;
        //    model.EmergencyContactFirstName = txtFirstNameEmger.Text;
        //    model.EmergencyContactLastName = txtLastNameEmger.Text;
        //    model.EmergencyContactPhone = "";
        //    model.EmergencyContactEmail = "";
        //    model.MaintenancePlanOption = ClientMaintenancePlan1.SelectedMaintenancePlan.ToString();
        //    return model;
        //}
        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    string msg = string.Empty;
        //    if (CheckInput(out msg))
        //    {
        //        CompanysEntity company = GetCompany();
        //        UsersEntity user = GetUser();
        //        int userid = userApp.RegisteUser(user, company);
        //        if (userid > 0)
        //        {
        //            LoginSystem(user, txtPassword.Text, true);
        //            string rmsg = string.Format("{0}{1}", "Registration is successful.",
        //                                                    userApp.BrokenRuleMessages.Count > 0 ? "" + userApp.BrokenRuleMessages[0].Message + "." : " A message has been sent to your inbox.");
        //            Redirect(rmsg, "/Sunnet/Profile/MyCompany.aspx");

        //        }
        //        else
        //        {
        //            ShowFailMessageToClient(userApp.BrokenRuleMessages, false);
        //        }
        //    }
        //    else
        //    {
        //        ShowMessageToClient(msg, 0, false, false);
        //    }
        //}
    }
}
