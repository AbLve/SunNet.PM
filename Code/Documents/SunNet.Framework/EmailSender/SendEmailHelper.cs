using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mail;
using SF.Framework.EmailSender.Providers;
using System.Threading;
using SF.Framework.Log;

namespace SF.Framework.EmailSender
{
    public class SendEmailHelper
    {
        public static bool Send(MailMessage mail)
        {
            try
            {
                SmtpClientEmailSender sces = new SmtpClientEmailSender(new log4netProvider());
                string attachments = "";
                if (mail.Attachments.Count > 0)
                {
                    for (int i = 0; i < mail.Attachments.Count; i++)
                    {
                        attachments += mail.Attachments[i].ToString() + ";";
                    }
                    if (attachments.EndsWith(";"))
                    {
                        attachments = attachments.Substring(0, attachments.Length - 1);
                    }
                }
                return sces.SendMail
                    (mail.To, mail.Cc, mail.Bcc, "", mail.Subject, mail.Body, attachments,
                    true, System.Net.Mail.MailPriority.Normal);
            }
            catch (Exception ex)
            {
                new log4netProvider().Log(ex);
                return false;
            }
        }

        /// <summary>
        /// Send
        /// </summary>
        /// <param name="mail"></param>
        public static void ExecuteInBackground(MailMessage mail)
        {
            Thread t = new Thread(new ParameterizedThreadStart(ExecuteSend));
            t.Start(mail);
        }

        private static void ExecuteSend(object m)
        {
            MailMessage mail = (MailMessage)m;
            SendEmailHelper.Send(mail);
        }
    }
}
