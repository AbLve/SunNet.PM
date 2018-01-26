using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014
{
    public partial class ResetPassword : GlobalPage
    {
        protected void ShowMessage(string msg, bool close)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                Request.Url.AbsolutePath,
                "<script type='text/javascript'>jQuery(function(){ShowMessage('" + msg + "','danger');});</script>",
                false);
        }
        protected UsersEntity GetEntity()
        {
            IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
            string[] items = encrypt.Decrypt(Request.Params["link"]).Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (items.Length != 2)
            {
                return null;
            }
            int userid;
            DateTime date;
            if (!int.TryParse(items[0], out userid) || !DateTime.TryParse(items[1], out date))
            {
                return null;
            }
            if (date.Date != DateTime.Now.Date)
            {
                return null;
            }
            UserApplication userApp = new UserApplication();
            UsersEntity user = userApp.GetUser(userid, false);
            return user;
        }
        private string CheckInput()
        {
            string msg = string.Empty;
            UserApplication userApp = new UserApplication();
            userApp.CheckPassword(txtNewPsd.Text, txtConfirm.Text, out msg);
            return msg;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Request.Params["link"]))
                {
                    ShowMessage("Link is incorrect", true);
                    return;
                }
                UsersEntity user = GetEntity();
                if (user == null)
                {
                    ShowMessage("Link is incorrect or expired", true);
                    return;
                }
                if (user.AccountStatus != UsersEntity.ForgotPasswordFlag)
                {
                    ShowMessage("Link has expired", true);
                    return;
                }
                ltlEmail.Text = user.UserName;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string msg = CheckInput();
            if (string.IsNullOrEmpty(msg))
            {
                UsersEntity user = GetEntity();
                user.PassWord = txtNewPsd.Text;
                user.AccountStatus = UsersEntity.ResetPasswordFlag;
                UserApplication userApp = new UserApplication();
                if (userApp.UpdateUser(user))
                {
                    LoginSystem(user, txtNewPsd.Text, true);
                    Response.Redirect("/Default.aspx");
                }
                else
                {
                    ShowMessage(userApp.BrokenRuleMessages[0].Message, false);
                }
            }
            else
            {
                ShowMessage(msg, false);
            }
        }

    }
}