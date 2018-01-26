using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using SunNet.PMNew.WinLogin;
using SunNet.PMNew.WinUpdate.PMLogin;

namespace SunNet.Capline.WinUpdate
{
    public partial class frmUpdate : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        private string xmlfile = "updater.xml";
        private string updatefolder = "update";
        private string loginapp = "SunNet.PMNew.WinLogin.exe";
        bool restart = false;
        private UpdateInfo updater;
        public frmUpdate()
        {
            InitializeComponent();
            xmlfile = Helper.GetPath(xmlfile);
            updatefolder = Helper.GetPath(updatefolder);
            loginapp = Helper.GetPath(loginapp);
        }

        private void frmUpdate_Load(object sender, EventArgs e)
        {
            try
            {
                CheckForIllegalCrossThreadCalls = false;
                updater = (UpdateInfo)Helper.Deserialize(xmlfile);
                if (updater != null && updater.Files != null && updater.Files.Count > 0)
                {
                    foreach (var file in updater.Files)
                    {
                        if (file.Version > updater.LocalVersion)
                        {
                            lisUpdateFile.Items.Add(file.Name);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No file need to update.", "Info");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Helper.FormatMessage(ex.Message), "Error");
                Application.Exit();
            }
        }

        private void frmUpdate_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0);
        }

        private void frmUpdate_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (restart && File.Exists(loginapp))
                Process.Start(loginapp);
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = System.Drawing.Color.FromArgb(29, 95, 211);
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = System.Drawing.Color.FromArgb(57, 137, 225);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = false;
            this.btnCancel.Enabled = false;
            Thread thread = new Thread(new ThreadStart(Update));
            thread.Start();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            restart = true;
            this.Close();
        }

        private new void Update()
        {
            this.progressBar1.Maximum = this.lisUpdateFile.Items.Count;
            bool blnDownload = true;
            DirectoryInfo directory = new DirectoryInfo(updatefolder);
            if (!directory.Exists)
            {
                directory.Create();
            }
            string key = "";
            PMLoginSoapClient client = new PMLoginSoapClient();
            foreach (object name in this.lisUpdateFile.Items)
            {
                try
                {
                    byte[] content = Convert.FromBase64String(client.Update(name.ToString()));
                    FileStream fileStream = File.Create(updatefolder + "/" + name.ToString());
                    fileStream.Write(content, 0, content.Length);
                    fileStream.Close();
                }
                catch
                {
                    blnDownload = false;
                    break;
                }
                ++this.progressBar1.Value;
            }
            this.Invoke(blnDownload ? new MethodInvoker(UpdateSuccessful) : new MethodInvoker(UpdateFailure));
        }

        private void UpdateFailure()
        {
            try
            {
                new DirectoryInfo(updatefolder).Delete(true);
            }
            catch
            {
            }
            MessageBox.Show("Update failure!", "Error");
            this.Close();
        }

        private void UpdateSuccessful()
        {
            DirectoryInfo directory = new DirectoryInfo(updatefolder);
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                file.CopyTo(Application.StartupPath + "/" + file.Name, true);
            }
            try
            {
                directory.Delete(true);

            }
            catch
            {
            }
            updater.Updated = true;
            Helper.Serialize(updater, xmlfile);

            MessageBox.Show("Update successful!", "Successful");
            restart = true;
            this.Close();
        }
    }
}
