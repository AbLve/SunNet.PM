using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.LogModel;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class ChangePassword : BaseAscx
    {
        public int UserID
        {
            get;
            set;
        }
        UserApplication userApp;

        protected void Page_Load(object sender, EventArgs e)
        {
            userApp = new UserApplication();
        }
        private string CheckInput()
        {
            UsersEntity model = UserInfo;
            string msg = string.Empty;
            if (model.PassWord != UtilFactory.GetEncryptProvider(EncryptType.MD5).Encrypt(txtOldPassword.Text))
            {
                msg = "The password you gave is incorrect, if you cannot remember you password, please contact the administrator.";
                txtOldPassword.Focus();
            }
            else
            {
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    msg = "New password can't be null ";
                    txtPassword.Focus();
                }
                else if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    msg = "Please confirm your new password ";
                    txtConfirmPassword.Focus();
                }
            }
            return msg;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            LogApplication logApplication = new LogApplication();
            LogEntity logEntity = new LogEntity();
            logEntity.logType = LogType.ModifyPassword;
            logEntity.operatingTime = DateTime.Now;
            logEntity.currentUserId = UserInfo.UserID;
            logEntity.referrer = Context.Request.UrlReferrer.ToString();
            logEntity.iPAddress = HttpContext.Current.Request.UserHostAddress;

            UsersEntity model = UserInfo;
            string msg = CheckInput();
            if (string.IsNullOrEmpty(msg))
            {
                model.PassWord = txtPassword.Text;
                if (userApp.UpdateUser(model))
                {
                    BaseWebsitePage.ShowSuccessMessageToClient(false, false);
                    logEntity.IsSuccess = true;
                    logEntity.Description = UserInfo.UserName + " modifiy own password to" + txtPassword.Text + " success. ";
                    logApplication.Write(logEntity);
                }
                else
                {
                    BaseWebsitePage.ShowFailMessageToClient(userApp.BrokenRuleMessages);
                    logEntity.IsSuccess = false;
                    logEntity.Description = UserInfo.UserName + " modifiy own password to" + txtPassword.Text + " fail. ";
                    logApplication.Write(logEntity);
                }
            }
            else
            {
                BaseWebsitePage.ShowMessageToClient(msg, 2, false, false);
                logEntity.IsSuccess = false;
                logEntity.Description = UserInfo.UserName + " modifiy own password to" + txtPassword.Text + " fail. " + msg;
                logApplication.Write(logEntity);
            }
        }
    }
}