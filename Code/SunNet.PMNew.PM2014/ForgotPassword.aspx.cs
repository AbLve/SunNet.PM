using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014
{
    public partial class ForgotPassword : GlobalPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (QS("success") == "1")
                {
                    btnSubmit.Visible = false;
                    phlForm.Visible = false;
                }
                else
                {
                    divMessage.Visible = false;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                UserApplication userApp = new UserApplication();
                if (userApp.SendForgotPasswordEmail(txtEmail.Text))
                {
                    Redirect(Request.RawUrl, true);
                }
                else
                {
                    ShowFailMessageToClient(userApp.BrokenRuleMessages, false);
                }
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex.Message);
            }
        }
    }
}