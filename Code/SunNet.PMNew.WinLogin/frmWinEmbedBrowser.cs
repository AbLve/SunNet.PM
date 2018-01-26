using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Xml;
using SunNet.PMNew.WinLogin.PMLogin1;

namespace SunNet.PMNew.WinLogin
{
    public partial class frmWinEmbedBrowser : Form
    {


        /// <summary>
        /// 检查是否应该启动更新程序.
        /// </summary>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/15 12:08
        private bool CheckUpdate
        {
            get
            {
                var date = Properties.Settings.Default.CheckUpdateDate;
                var check = Properties.Settings.Default.ISCheckUpdate;

                return date < new DateTime(2014, 5, 1) || date.Date == DateTime.Now.Date && check;
            }
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        private const string loginHistory = "data.dat";
        private const string udat = "u.dat";
        private const string updateapp = "SunNet.PMNew.WinUpdate.exe";
        private EventHandler onCloseLoginHandler = null;
        private DataTable dtUser = new DataTable();
        private string filename = "updater.xml";
        delegate void NoneArguMethod();

        private readonly string _defaultUrl = ConfigurationManager.AppSettings["defaultPage"].ToString();

        //"http://www.justin.cc/default.aspx";
        private frmLogin frm = new frmLogin();

        public frmWinEmbedBrowser()
        {
            InitializeComponent();
            SetToolBarVisible(false);
        }

        private Thread thrd;
        private void frmWinEmbedBrowser_Load(object sender, EventArgs e)
        {
            if (CheckUpdate)
            {
                thrd = new Thread(new ThreadStart(UpdateApplication));
                thrd.Start();
            }
            object obj = Helper.Deserialize(filename);
            if (obj != null)
            {
                var updater = (UpdateInfo)obj;
                if (updater.Updated)
                {
                    Properties.Settings.Default.LocalVersion = updater.Version;
                    Properties.Settings.Default.Save();

                    File.Delete(Helper.GetPath(filename));
                }
            }

            this.Icon = global::SunNet.PMNew.WinLogin.Properties.Resources.logo;
            bool isRememberMe;
            string userName, password, userId;
            string id = string.Empty;
            FileInfo updateFileInfo = new FileInfo(udat); //This is temp user file for log in after update.
            bool login = false;
            if (updateFileInfo.Exists)
            {
                AccountHelper.ReadData(udat, out isRememberMe, out userName, out password, out userId);
                login = true;
                try
                {
                    updateFileInfo.Delete();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                AccountHelper.ReadData(loginHistory, out isRememberMe, out userName, out password, out userId);
            }
            if (isRememberMe || login)
            {
                PMLoginSoapClient loginClient = new PMLoginSoapClient();
                string result = loginClient.Login(
                    DESEncrypt.Encrypt(userName),
                    DESEncrypt.Encrypt(password),
                    isRememberMe,
                    out id);
                frm_onLogin(result, id, userName, password, isRememberMe);
            }
            else
            {
                onCloseLoginHandler = new EventHandler(frm_onFrmClose);
                frm.onLogin += new frmLogin.LoginHandler(frm_onLogin);
                frm.onFrmClose += onCloseLoginHandler;
                ((TextBox)frm.Controls.Find("txtBoxUserName", false)[0]).Text = userName;
                frm.ShowDialog(this);
            }
        }

        public void frm_onLogin(string message, string userId, string userName
            , string password, bool isRemember)
        {
            if (message.StartsWith("successful"))
            {
                Login(userId, userName, password, isRemember);

                AccountHelper.SaveData(loginHistory, isRemember, userName, password, userId);
                var items = message.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string userinfo = items.Length > 1 ? items[1] : "";
                lblUser.Text = userinfo;
                this.Text = string.Format("{0} - {1}", userinfo, this.Text);
            }
            else
            {
                MessageBox.Show(message, "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void frm_onFrmClose(object sender, EventArgs e)
        {
            if (thrd != null)
                thrd.Abort();
            Application.Exit();
        }

        public void Login(string userId, string userName, string password, bool isRemember)
        {
            CookieHelper cookieHelper = new CookieHelper(_defaultUrl);
            cookieHelper.DeleteCookie("LoginUserID", userId);
            cookieHelper.DeleteCookie("Login_UserName_", userName);

            cookieHelper.SetCookie("LoginUserID", userId, DateTime.Now.AddMinutes(30));
            cookieHelper.SetCookie("Login_UserName_", userName, DateTime.Now.AddDays(7));
            cookieHelper.SetCookie("source", "winform", DateTime.Now.AddMinutes(30));
            cookieHelper.DeleteCookie("Login_Password_", password);
            if (isRemember)
            {
                cookieHelper.SetCookie("Login_Password_", password, DateTime.Now.AddDays(7));
            }
            else
            {
                cookieHelper.SetCookie("Login_Password_", "", DateTime.Now.AddDays(7));
            }
            frm.onFrmClose -= new EventHandler(frm_onFrmClose);
            try
            {
                wbBroser.Navigate(_defaultUrl);
            }
            catch (Exception ex)
            {
            }
            frm.Close();
            SetToolBarVisible(true);
        }

        private void SetToolBarVisible(bool visible)
        {
            if (visible)
            {
                splitContainer1.Panel1.BackColor = Color.White;
                splitContainer1.BackColor = Color.White;
            }
            lblUser.Visible = visible;
            lblWelcome.Visible = visible;
            lnkLogout.Visible = visible;
            wbBroser.Visible = visible;
        }

        private void lnkLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool isRememberMe;
            string userName, password, userId;
            AccountHelper.ReadData(loginHistory, out isRememberMe, out userName, out password, out userId);
            AccountHelper.SaveData(loginHistory, false, userName, password, userId);
            Application.Exit();
        }

        private void UpdateApplication()
        {
            PMLoginSoapClient loginClient = new PMLoginSoapClient();
            var updateXml = loginClient.GetVersion("");
            XmlDocument docUpdateFile = new XmlDocument();
            var updater = new UpdateInfo();
            try
            {
                // 更新下次检查日期
                Properties.Settings.Default.CheckUpdateDate = DateTime.Today.AddDays(7);
                Properties.Settings.Default.ISCheckUpdate = true;
                Properties.Settings.Default.Save();

                docUpdateFile.LoadXml(updateXml);
                var root = docUpdateFile.SelectSingleNode("root");
                if (root != null && root.Attributes != null && root.Attributes["version"] != null)
                {
                    var newVersion = 0f;
                    if (float.TryParse(root.Attributes["version"].Value, out newVersion) &&
                        newVersion > Properties.Settings.Default.LocalVersion)
                    {
                        updater.Version = newVersion;
                        updater.LocalVersion = Properties.Settings.Default.LocalVersion;
                    }
                }
                if (updater.Version < 1) return;

                XmlNodeList remlist = docUpdateFile.SelectNodes("//file");
                if (remlist != null)
                    foreach (XmlNode file in remlist)
                    {
                        if (file.Attributes != null)
                        {
                            string name = file.Attributes["name"].Value;
                            var fileVersion = 0f;
                            float.TryParse(file.Attributes["version"].Value, out fileVersion);
                            updater.Files.Add(new FileToUpdate() { Name = name, Version = fileVersion });
                        }
                    }
                if (updater.NeedUpdate)
                {
                    if (MessageBox.Show(Properties.Resources.update_Confirm_message, "Update Confirm", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Helper.Serialize(updater, filename);
                        this.BeginInvoke(new NoneArguMethod(StartUpdateApplication));
                    }
                    else
                    {
                    }
                }
            }
            catch (XmlException xmlEx)
            {
                MessageBox.Show(Helper.FormatMessage(xmlEx.Message), "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(Helper.FormatMessage(ex.Message), "Error");
            }
        }

        private void StartUpdateApplication()
        {
            if (File.Exists(Path.Combine(Application.StartupPath, updateapp)))
                Process.Start(Path.Combine(Application.StartupPath, updateapp));
            else
            {
                MessageBox.Show(Properties.Resources.msg_updateApplicationLost, "Error");
            }
            this.Close();
        }

        private void frmWinEmbedBrowser_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thrd != null)
                thrd.Abort();
        }

    }
}

