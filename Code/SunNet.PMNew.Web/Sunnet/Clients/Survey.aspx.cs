using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils;
using StructureMap;
using System.IO;

namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class Survey : BaseWebsitePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtCompanyName.Text = UserInfo.CompanyName;
                txtUserName.Text = string.Format("{0},{1}", UserInfo.LastName, UserInfo.FirstName);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            GetInformation();
        }
        private void GetInformation()
        {
            string str1 = this.survey1.SelectedValue;
            string str2 = this.survey2.SelectedValue;
            string str3 = this.survey3.SelectedValue;
            string str4 = this.survey4.SelectedValue;

            string str5 = this.survey5.SelectedValue;

            string str6 = string.Empty;
            for (int i = 0; i < this.survey6.Items.Count; i++)
            {
                if (this.survey6.Items[i].Selected)
                {
                    if (str6 == string.Empty)
                        str6 += this.survey6.Items[i].Value;
                    else
                        str6 += "," + this.survey6.Items[i].Value;
                }
            }
            string str7 = this.survey7.SelectedValue;
            string content1 = this.txt7.Text.Trim();
            string str8 = this.survey8.SelectedValue;
            string content2 = this.txt9.Text.Trim();
            string str9 = this.survey9.SelectedValue;
            string content3 = this.txt9.Text.Trim();
            string str10 = this.survey10.SelectedValue;
            string str11 = this.survey11.SelectedValue;
            string content4 = this.txt11.Text;
            string content5 = this.txt12.Text;

            string tempalte = AppDomain.CurrentDomain.BaseDirectory + "/Template/SurveyEmail.txt";
            StreamReader sr = new StreamReader(tempalte);
            string content = sr.ReadToEnd();
            if (content != string.Empty)
            {
                string to = Config.DefaultSurveyEmail;
                string from = Config.DefaultSendEmail;
                string subject = "PM Survey";
                string body = content;
                body = body.Replace("[Company_Name]", this.txtCompanyName.Text.Trim());
                body = body.Replace("[User_Name]", this.txtUserName.Text.Trim());

                body = body.Replace("[Client_Survey_Email1]", str1);
                body = body.Replace("[Client_Survey_Email2]", str2);
                body = body.Replace("[Client_Survey_Email3]", str3);
                body = body.Replace("[Client_Survey_Email4]", str4);

                body = body.Replace("[Client_Survey_Email5]", str5);

                body = body.Replace("[Client_Survey_Email6]", str6);
                body = body.Replace("[Client_Survey_Email7]", str7);
                body = body.Replace("[Client_Survey_Email8]", str8);
                body = body.Replace("[Client_Survey_Email9]", str9);
                body = body.Replace("[Client_Survey_Email10]", str10);
                body = body.Replace("[Client_Survey_Email11]", str11);

                body = body.Replace("[Client_Descriptions6]", content1);
                body = body.Replace("[Client_Descriptions7]", content2);
                body = body.Replace("[Client_Descriptions8]", content3);
                body = body.Replace("[Client_Descriptions10]", content4);
                body = body.Replace("[Client_Descriptions11]", content5);

                if (string.IsNullOrEmpty(str1))
                {
                    this.ShowMessageToClient("Please select.", 0, false, false);
                    this.survey1.Focus();
                    return;
                }

                if (Validata(str1))
                    this.survey1.Focus();
                else if (Validata(str2))
                    this.survey2.Focus();
                else if (Validata(str3))
                    this.survey3.Focus();
                else if (Validata(str4))
                    this.survey4.Focus();
                else if (Validata(str5))
                    this.survey5.Focus();
                else if (Validata(str6))
                    this.survey6.Focus();
                else if (Validata(str7))
                    this.survey7.Focus();
                else if (Validata(str8))
                    this.survey8.Focus();
                else if (Validata(str9))
                    this.survey9.Focus();
                else if (Validata(str10))
                    this.survey10.Focus();
                else
                {

                    IEmailSender emailSender = ObjectFactory.GetInstance<IEmailSender>();
                    if (emailSender.SendMail(Config.DefaultSurveyEmail, Config.DefaultSendEmail, subject, body))
                    {
                        Response.Redirect("~/Sunnet/Clients/SurveySuccess.aspx");
                    }
                    else
                    {
                        this.ShowMessageToClient("SendFail", 0, false, false);
                    }
                }
            }

        }
        private bool Validata(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMessageToClient("Please select.", 0, false, false);
                return true;
            }
            else
                return false;
        }
    }
}
