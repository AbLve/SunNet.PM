using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using StructureMap;
using SF.Framework.Log;
using SunNet.PMNew.Framework;

namespace SF.Framework.EmailSender.Providers
{
    public class SmtpClientEmailSender : IEmailSender
    {
        private ILog logger = null;

        public SmtpClientEmailSender(ILog logger)
        {
            this.logger = logger;
        }

        public SmtpClientEmailSender(log4netProvider logger)
        {
            this.logger = logger;
        }

        public bool SendMail(string To, string subject, string body, bool isBodyHtml, System.Net.Mail.MailPriority priority) //6
        {
            return SendMail(To, "", "", "", subject, body, "", isBodyHtml, priority);
        }

        public bool SendMail(List<string> To, List<string> ToCc, List<string> ToBcc, string DisplayName
            , string Subject, string Body, string AttachFiles, bool IsBodyHtml, string priority)
        {
            return SendMail(ListToString(To), ListToString(ToCc), ListToString(ToBcc), DisplayName
                , Subject, Body, AttachFiles, IsBodyHtml, (MailPriority)Enum.Parse(typeof(MailPriority), priority, true));
        }


        public bool SendMail(string To, string ToCc, string ToBcc, string displayName
           , string subject, string body, string attachFiles, bool IsBodyHtml, System.Net.Mail.MailPriority priority)
        {
            if (To.Length == 0 && ToCc.Length == 0 && ToBcc.Length == 0)
            {
                //errorMessage = "Missing \"To\" address.";
                return false;
            }

            //errorMessage = "";
            bool flag = true;
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = IsBodyHtml;
            mail.BodyEncoding = Encoding.UTF8;
            mail.Priority = priority;
            mail.Body = body;

            if (!string.IsNullOrEmpty(ToCc))
            {
                foreach (string s in ToCc.Split(';'))
                {
                    if (string.IsNullOrEmpty(s)) continue;
                    mail.CC.Add(s);
                }
            }

            if (!string.IsNullOrEmpty(ToBcc))
            {
                foreach (string s in ToBcc.Split(';'))
                {
                    if (string.IsNullOrEmpty(s)) continue;
                    mail.Bcc.Add(s);
                }
            }
            if (!string.IsNullOrEmpty(To))
            {
                foreach (string s in To.Split(';'))
                {
                    if (string.IsNullOrEmpty(s)) continue;
                    mail.To.Add(s);
                }
            }

            if (!string.IsNullOrEmpty(To))
            {
                if (Config.IsTest && Config.TestMails != null && Config.TestMails.Trim().Length > 0)
                {
                    body = string.Format("{0}\r\n This is an email send from test website,it's really send to {1},bcc {2},cc{3}\r\n",
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
            }
            //mail.From = new MailAddress(from, displayName);
            if (string.IsNullOrEmpty(displayName))
            {
                if (!string.IsNullOrEmpty(SFConfig.EmailDisplayName))
                    displayName = SFConfig.EmailDisplayName;
            }
            mail.From = new MailAddress(SF.Framework.SFConfig.FromEmailAddress, displayName);        
            mail.Subject = subject;
            if (!string.IsNullOrEmpty(attachFiles))
            {
                foreach (string s in attachFiles.Split(';'))
                {
                    if (string.IsNullOrEmpty(s)) continue;
                    Attachment attach = new Attachment(s);
                    mail.Attachments.Add(attach);
                }
            }
            SmtpClient client = new SmtpClient();
            client.EnableSsl = SFConfig.EnableSSL;
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                flag = false;
            }
            finally
            {
                logger.Log(body);
                mail.Dispose();
            }
            return flag;
        }

        private string ListToString(List<string> listInput)
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
