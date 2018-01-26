using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.LogModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;

using IWshRuntimeLibrary;

namespace SunNet.PMNew.PM2014
{
    public partial class Login : GlobalPage
    {
        UserApplication userApplication = new UserApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            UsersEntity user = userApplication.Login(txtUserName.Text.Trim(), txtPassword.Text);
            if (user == null)
            {
                string msg = FormatMessages(userApplication.BrokenRuleMessages, "Login failed.");
                WriteLoginLog(false, -1, txtUserName.Text, txtPassword.Text);
                ShowFailMessageToClient(msg);
            }
            else
            {
                LoginSystem(user, txtPassword.Text, chkRemember.Checked);
                WriteLoginLog(true, user.UserID, txtUserName.Text, txtPassword.Text);
                string url = "/Default.aspx";
                if (!string.IsNullOrEmpty(Request.QueryString["returnurl"]))
                {
                    string page = Server.UrlDecode(Request.QueryString["returnurl"]);
                    if (page != null && page.IndexOf(".aspx") > 0)
                    {
                        page = page.Substring(0, page.IndexOf(".aspx") + 5);
                        if (System.IO.File.Exists(Server.MapPath(page)))
                            url += "?returnurl=" + Server.UrlEncode(Request.QueryString["returnurl"]);
                    }
                }
                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Write Login log.
        /// </summary>
        /// <param name="isLoginSuccess">is loin success</param>
        /// <param name="currentUserId">current login user id ,if login fail then -1</param>
        /// <param name="loginUserName"></param>
        /// <param name="password"></param>
        private void WriteLoginLog(bool isLoginSuccess, int currentUserId,
            string loginUserName, string password)
        {
            LogApplication logApplication = new LogApplication();
            LogEntity logEntity = new LogEntity();
            logEntity.logType = LogType.Login;
            logEntity.operatingTime = DateTime.Now;
            logEntity.referrer = Context.Request.UrlReferrer.ToString();
            logEntity.iPAddress = HttpContext.Current.Request.UserHostAddress;
            logEntity.IsSuccess = false;
            logEntity.currentUserId = -1;

            if (isLoginSuccess)
            {
                logEntity.Description = loginUserName + " login success. ";
            }
            else
            {
                logEntity.Description = "login fail. user name: " + loginUserName + " password: " + password;
            }
            logApplication.Write(logEntity);
        }

    }
}