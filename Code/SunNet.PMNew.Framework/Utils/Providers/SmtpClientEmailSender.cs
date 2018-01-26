using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.IO;

namespace SunNet.PMNew.Framework.Utils.Providers
{
    public class SmtpClientEmailSender : IEmailSender
    {
        private ILog logger = null;

        public SmtpClientEmailSender(ILog logger)
        {
            this.logger = logger;
        }

        public bool SendMail(string To, string ToCc, string ToBcc, string replyTo, string from, string displayName
            , string subject, string body, string attachFiles, bool IsBodyHtml, System.Net.Mail.MailPriority priority
            , out string errorMessage)
        {
            if (from == null) from = "";


            To = To.ToLower().Replace(";" + from.ToLower(), "").Replace(from.ToLower() + ";", "");
            if (To.Length == 0 && ToCc.Length == 0 && ToBcc.Length == 0)
            {
                errorMessage = "Missing \"To\" address.";
                return false;
            }
            if (from.Length == 0)
            {
                errorMessage = "Missing \"From\" address.";
                return false;
            }
            errorMessage = "";
            bool flag = true;
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = IsBodyHtml;
            mail.BodyEncoding = Encoding.UTF8;
            mail.Priority = priority;
            if (To.Length != 0)
            {
                foreach (string s in To.Split(';'))
                {
                    if (string.IsNullOrEmpty(s)) continue;
                    mail.To.Add(s);
                }
            }
            if (ToCc.Length != 0)
            {
                foreach (string s in ToCc.Split(';'))
                {
                    if (string.IsNullOrEmpty(s)) continue;
                    mail.CC.Add(s);
                }
            }
            mail.From = new MailAddress(from, displayName);
            if (ToBcc.Length != 0)
            {
                foreach (string s in ToBcc.Split(';'))
                {
                    if (string.IsNullOrEmpty(s)) continue;
                    mail.Bcc.Add(s);
                }
            }
            mail.Subject = subject;
            mail.Sender = mail.From;
            mail.Body = body;
            //mail.ReplyToList.Add(replyTo);
            if (attachFiles.Length != 0)
            {
                foreach (string s in attachFiles.Split(';'))
                {
                    if (string.IsNullOrEmpty(s)) continue;
                    Attachment attach = new Attachment(s);
                    mail.Attachments.Add(attach);
                }
            }
            SmtpClient client = new SmtpClient();
            client.EnableSsl = true;
            try
            {
                if (Config.IsTest && Config.TestMails != null && Config.TestMails.Trim().Length > 0)
                {
                    body = string.Format("{0}\r\n This is an emial send from test website,it's really send to {1},bcc {2},cc{3}\r\n",
                        body,
                        string.Join(",", mail.To.Select(x => x.Address).ToArray()),
                        string.Join(",", mail.CC.Select(x => x.Address).ToArray()),
                        string.Join(",", mail.Bcc.Select(x => x.Address).ToArray()));
                    mail.Body = body;
                    mail.To.Clear();
                    var testReceivers = Config.TestMails.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in testReceivers)
                        mail.To.Add(s);
                    mail.CC.Clear();
                    mail.Bcc.Clear();
                }
                client.Send(mail);
                using (StreamWriter write = new StreamWriter(System.Configuration.ConfigurationManager.AppSettings["EmailLog"], true))
                {
                    write.WriteLine(string.Format("To:{0}  On:{1}  发送成功", To, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")));
                    write.WriteLine(string.Format("Body: {0}", body));
                    write.WriteLine("");
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                logger.Log(ex);
                flag = false;
                using (StreamWriter write = new StreamWriter(System.Configuration.ConfigurationManager.AppSettings["EmailLog"], true))
                {
                    write.WriteLine(string.Format("To:{0}  On:{1}  发送失败", To, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")));
                    write.WriteLine(string.Format("Body: {0}", body));
                    write.WriteLine("");
                }
            }
            finally
            {
                mail.Dispose();
            }
            return flag;
        }

        public bool SendMail(List<string> To, List<string> ToCc, List<string> ToBcc, string ReplyTo, string From, string DisplayName, string Subject, string Body, string AttachFiles, bool IsBodyHtml, string priority, out string ErrorMessage)
        {
            return SendMail(ListToString(To), ListToString(ToCc), ListToString(ToBcc), ReplyTo, From, DisplayName, Subject, Body, AttachFiles, IsBodyHtml, (MailPriority)Enum.Parse(typeof(MailPriority), priority, true), out ErrorMessage);
        }

        public bool SendMail(string To, string from, string subject, string body, out string errorMessage)
        {
            return SendMail(To, "", "", from, from, from, subject, body, "", true, MailPriority.Normal, out errorMessage);
        }

        public bool SendToMail(string To, string ToCc, string from, string subject, string body, out string errorMessage)
        {
            return SendMail(To, ToCc, "", from, from, from, subject, body, "", true, MailPriority.Normal, out errorMessage);
        }

        public bool SendMail(string To, string from, string subject, string body, string attachfiles, out string errorMessage)
        {
            return SendMail(To, from, subject, body, true, attachfiles, out errorMessage);
        }

        public bool SendMail(string To, string from, string subject, string body, bool isBodyHtml, string attachfiles, out string errorMessage)
        {
            return SendMail(To, "", "", from, from, from, subject, body, attachfiles, isBodyHtml, MailPriority.Normal, out errorMessage);
        }

        public bool SendMail(string To, string from, string subject, string body, bool isBodyHtml, System.Net.Mail.MailPriority priority, out string errorMessage)
        {
            return SendMail(To, "", "", from, from, from, subject, body, "", true, priority, out errorMessage);
        }

        public bool SendMail(string To, string from, string subject, string body)
        {
            //return true;
            string errorMessage = "true";
            return SendMail(To, from, subject, body, out errorMessage);
        }

        public bool SendToMail(string To, string ToCc, string from, string subject, string body)
        {
            string errorMessage;
            return SendToMail(To, ToCc, from, subject, body, out errorMessage);
        }

        public bool SendMail(string To, string from, string subject, string body, bool isBodyHtml)
        {
            string errorMessage;
            return SendMail(To, from, subject, body, isBodyHtml, MailPriority.Normal, out errorMessage);
        }

        private static string ListToString(List<string> listInput)
        {
            string output = "";
            foreach (string s in listInput)
            {
                output += s + ";";
            }
            return output;
        }
    }
}
