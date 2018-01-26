using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.App;

namespace SunNet.PMNew.Web
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divMessage.Visible = false;
            }
        }
        protected void ShowMessage(string msg, bool close)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                                                                   Request.Url.AbsolutePath,
                                                                   string.Format(@"<script type='text/javascript'>alert('{0}');{1}</script>",
                                                                   msg, close ? "WinClose();" : ""));
        }

        protected void ibtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                UserApplication userApp = new UserApplication();
                if (userApp.SendForgotPasswordEmail(txtEmail.Text))
                {
                    ltlEmail.Text = txtEmail.Text;
                    divMessage.Visible = true;
                    divform.Visible = false;
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                                                                  Request.Url.AbsolutePath, @"<script type='text/javascript'> CloseWindow();</script>");
                }
                else
                {
                    ShowMessage(userApp.BrokenRuleMessages[0].Message, false);
                }
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex.Message);
            }
        }
    }
}
