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
using SunNet.PMNew.Framework.Core;
using System.Text;
using SunNet.PMNew.Entity.LogModel;

namespace SunNet.PMNew.Web
{
    public partial class Login : GlobalPageBase
    {
        UserApplication userApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            userApp = new UserApplication();
            if (!IsPostBack)
            {
                try
                {
                    IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                    string username = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("Login_UserName_")));
                    string password = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("Login_Password_")));
                    string preFailedUserName = QS("uname");
                    if (!string.IsNullOrEmpty(preFailedUserName))//如果前一次登陆成功过，那么就保存cookie, 否则就重新赋值为上一次失败的用户名。
                    {
                        txtUserName.Text = preFailedUserName;
                        txtPassword.Focus();
                    }
                    else
                    {
                        txtUserName.Text = username;
                        txtPassword.Text = password;
                    }
                    chkRemember.Checked = true;
                }
                catch
                {
                    txtUserName.Text = "jack@sunnet.us";
                    txtPassword.Text = "jacK1234";
                    chkRemember.Checked = false;
                }
            }
        }
        protected string FormatMessages(List<BrokenRuleMessage> list)
        {
            StringBuilder sbMsgs = new StringBuilder();
            foreach (BrokenRuleMessage msg in list)
            {
                sbMsgs.Append(msg.Message);
                sbMsgs.Append("");
            }
            return sbMsgs.ToString();
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            LogApplication logApplication = new LogApplication();
            LogEntity logEntity = new LogEntity();
            logEntity.logType = LogType.Login;
            logEntity.operatingTime = DateTime.Now;
            logEntity.referrer = Context.Request.UrlReferrer.ToString();
            logEntity.iPAddress = HttpContext.Current.Request.UserHostAddress;

            UsersEntity user = userApp.Login(txtUserName.Text, txtPassword.Text);
            if (user == null)
            {
                string msg = FormatMessages(userApp.BrokenRuleMessages);
                //log fail
                logEntity.IsSuccess = false;
                logEntity.currentUserId = -1;
                logEntity.Description = "login fail. user name: " + txtUserName.Text + " password: " + txtPassword.Text;
                logApplication.Write(logEntity);
                ShowMessageAndRedirect(msg, "/Login.aspx?uname=" + txtUserName.Text);
                return;
            }
            LoginSystem(user, txtPassword.Text, chkRemember.Checked);
            //log success
            logEntity.IsSuccess = true;
            logEntity.currentUserId = user.UserID;
            logEntity.Description = user.UserName + " login success. ";
            logApplication.Write(logEntity);
            Response.Redirect("/Default.aspx");

        }


    }
}
