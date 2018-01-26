using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SunNet.PMNew.WinLogin.PMLogin1;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;
namespace SunNet.PMNew.WinLogin
{
    public partial class frmLogin : Form
    {
        public delegate void LoginHandler(string result, string id, string username
            , string password, bool isRemember);
        public event LoginHandler onLogin;

        public event EventHandler onFrmClose;

        private string UserName
        {
            get
            {
                return txtBoxUserName.Text.Trim();
            }
        }

        private string Password
        {
            get
            {
                return txtBoxPassword.Text.Trim();
            }
        }

        private bool isRemember
        {
            get
            {
                return chkBoxRememberMe.Checked;
            }
        }

        public frmLogin()
        {
            InitializeComponent();

        }


        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (onFrmClose != null)
            {
                onFrmClose(sender, e);
            }
        }

        private void Login()
        {
            if (string.IsNullOrEmpty(txtBoxUserName.Text))
            {
                MessageBox.Show(Properties.Resources.frmLogin_Login_Username_is_required, "Login");
                txtBoxUserName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtBoxPassword.Text))
            {
                MessageBox.Show(Properties.Resources.frmLogin_Login_Password_is_required, "Login");
                txtBoxPassword.Focus();
                return;
            }

            pBtnLogin.BeginInvoke(new InvokeDelegate(WaitingCursor));

            string id = string.Empty;
            PMLoginSoapClient pMLoginSoapClient = new PMLoginSoapClient();
            try
            {
                string result = pMLoginSoapClient.Login(
                    DESEncrypt.Encrypt(UserName),
                    DESEncrypt.Encrypt(Password),
                    isRemember,
                    out id);
                onLogin(result, id, UserName, Password, isRemember);

                CancelWaiting();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void WaitingCursor()
        {
            this.Cursor = Cursors.WaitCursor;
        }

        private void CancelWaiting()
        {
            this.Cursor = Cursors.Default;
        }

        private delegate void InvokeDelegate();
        private void pBtnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }
        private void pBtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoginHide_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBoxUserName.Text))
            {
                txtBoxUserName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtBoxPassword.Text))
            {
                txtBoxPassword.Focus();
                return;
            }
            Login();
        }

    }


}
