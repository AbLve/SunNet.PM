namespace SunNet.PMNew.WinLogin
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtBoxUserName = new System.Windows.Forms.TextBox();
            this.txtBoxPassword = new System.Windows.Forms.TextBox();
            this.chkBoxRememberMe = new System.Windows.Forms.CheckBox();
            this.pBtnLogin = new System.Windows.Forms.PictureBox();
            this.pBtnClose = new System.Windows.Forms.PictureBox();
            this.btnLoginHide = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pBtnLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBtnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBoxUserName
            // 
            this.txtBoxUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxUserName.Location = new System.Drawing.Point(83, 172);
            this.txtBoxUserName.Name = "txtBoxUserName";
            this.txtBoxUserName.Size = new System.Drawing.Size(247, 24);
            this.txtBoxUserName.TabIndex = 0;
            // 
            // txtBoxPassword
            // 
            this.txtBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPassword.Location = new System.Drawing.Point(83, 217);
            this.txtBoxPassword.Name = "txtBoxPassword";
            this.txtBoxPassword.PasswordChar = '*';
            this.txtBoxPassword.Size = new System.Drawing.Size(247, 24);
            this.txtBoxPassword.TabIndex = 1;
            // 
            // chkBoxRememberMe
            // 
            this.chkBoxRememberMe.AutoSize = true;
            this.chkBoxRememberMe.BackColor = System.Drawing.Color.Transparent;
            this.chkBoxRememberMe.ForeColor = System.Drawing.Color.White;
            this.chkBoxRememberMe.Location = new System.Drawing.Point(83, 324);
            this.chkBoxRememberMe.Name = "chkBoxRememberMe";
            this.chkBoxRememberMe.Size = new System.Drawing.Size(95, 17);
            this.chkBoxRememberMe.TabIndex = 2;
            this.chkBoxRememberMe.Text = "Remember Me";
            this.chkBoxRememberMe.UseVisualStyleBackColor = false;
            // 
            // pBtnLogin
            // 
            this.pBtnLogin.BackColor = System.Drawing.Color.Transparent;
            this.pBtnLogin.Image = global::SunNet.PMNew.WinLogin.Properties.Resources.loginbtn;
            this.pBtnLogin.Location = new System.Drawing.Point(83, 267);
            this.pBtnLogin.Name = "pBtnLogin";
            this.pBtnLogin.Size = new System.Drawing.Size(247, 30);
            this.pBtnLogin.TabIndex = 5;
            this.pBtnLogin.TabStop = false;
            this.pBtnLogin.Click += new System.EventHandler(this.pBtnLogin_Click);
            // 
            // pBtnClose
            // 
            this.pBtnClose.Image = global::SunNet.PMNew.WinLogin.Properties.Resources.close;
            this.pBtnClose.Location = new System.Drawing.Point(668, 70);
            this.pBtnClose.Name = "pBtnClose";
            this.pBtnClose.Size = new System.Drawing.Size(16, 16);
            this.pBtnClose.TabIndex = 6;
            this.pBtnClose.TabStop = false;
            this.pBtnClose.Click += new System.EventHandler(this.pBtnClose_Click);
            // 
            // btnLoginHide
            // 
            this.btnLoginHide.Location = new System.Drawing.Point(83, 303);
            this.btnLoginHide.Name = "btnLoginHide";
            this.btnLoginHide.Size = new System.Drawing.Size(0, 0);
            this.btnLoginHide.TabIndex = 4;
            this.btnLoginHide.UseVisualStyleBackColor = true;
            this.btnLoginHide.Click += new System.EventHandler(this.btnLoginHide_Click);
            // 
            // frmLogin
            // 
            this.AcceptButton = this.btnLoginHide;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SunNet.PMNew.WinLogin.Properties.Resources.main;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(746, 485);
            this.Controls.Add(this.btnLoginHide);
            this.Controls.Add(this.pBtnClose);
            this.Controls.Add(this.pBtnLogin);
            this.Controls.Add(this.chkBoxRememberMe);
            this.Controls.Add(this.txtBoxPassword);
            this.Controls.Add(this.txtBoxUserName);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLogin_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pBtnLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBtnClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxUserName;
        private System.Windows.Forms.TextBox txtBoxPassword;
        private System.Windows.Forms.CheckBox chkBoxRememberMe;
        private System.Windows.Forms.PictureBox pBtnLogin;
        private System.Windows.Forms.PictureBox pBtnClose;
        private System.Windows.Forms.Button btnLoginHide;
    }
}

