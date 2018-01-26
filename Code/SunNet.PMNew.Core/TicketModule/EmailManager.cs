using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils;
using System.Net.Mail;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Core.TicketModule
{
    public class EmailManager : SqlHelper, IEmailSender
    {
        private IEmailSender emailSender;
        public EmailManager(
                                IEmailSender emailSender

                            )
        {
            this.emailSender = emailSender;
        }

        #region Send Mail basic method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="To">Send mail to who</param>
        /// <param name="ToCc">CC to</param>
        /// <param name="ToBcc">BCC to</param>
        /// <param name="replayTo">reply to</param>
        /// <param name="from">Mail From</param>
        /// <param name="displayName">name display on sender in mail</param>
        /// <param name="subject">title</param>
        /// <param name="body">content</param>
        /// <param name="attachFiles">attachFile(s)</param>
        /// <param name="IsBodyHtml">html switch</param>
        /// <param name="priority">mail priority</param>
        /// <param name="errorMessage">out error message,"" for success without mistakes</param>
        /// <returns>true for success</returns>
        public static bool SendMail(string To, string ToCc, string ToBcc, string replyTo, string from, string displayName, string subject, string body, string attachFiles, bool IsBodyHtml, MailPriority priority, out string errorMessage)
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
            //client.EnableSsl = Config.SSL;
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                WebLogAgent.Write(ex);
                flag = false;
            }
            finally
            {
                mail.Dispose();
            }
            return flag;
        }

        #endregion

        public static bool SendMail(List<string> To, List<string> ToCc, List<string> ToBcc, string ReplyTo, string From, string DisplayName, string Subject, string Body, string AttachFiles, bool IsBodyHtml, string priority, out string ErrorMessage)
        {
            return SendMail(ListToString(To), ListToString(ToCc), ListToString(ToBcc), ReplyTo, From, DisplayName, Subject, Body, AttachFiles, IsBodyHtml, (MailPriority)Enum.Parse(typeof(MailPriority), priority, true), out ErrorMessage);
        }

        public static bool SendMail(string To, string from, string subject, string body, out string errorMessage)
        {
            return SendMail(To, "", "", from, from, from, subject, body, "", true, MailPriority.Normal, out errorMessage);
        }

        public static bool SendToMail(string To, string ToCc, string from, string subject, string body, out string errorMessage)
        {
            return SendMail(To, ToCc, "", from, from, from, subject, body, "", true, MailPriority.Normal, out errorMessage);
        }

        public static bool SendMail(string To, string from, string subject, string body, string attachfiles, out string errorMessage)
        {
            return SendMail(To, from, subject, body, true, attachfiles, out errorMessage);
        }

        public static bool SendMail(string To, string from, string subject, string body, bool isBodyHtml, string attachfiles, out string errorMessage)
        {
            return SendMail(To, "", "", from, from, from, subject, body, attachfiles, isBodyHtml, MailPriority.Normal, out errorMessage);
        }

        public static bool SendMail(string To, string from, string subject, string body, bool isBodyHtml, MailPriority priority, out string errorMessage)
        {
            return SendMail(To, "", "", from, from, from, subject, body, "", true, priority, out errorMessage);
        }

        public static bool SendMail(string To, string from, string subject, string body)
        {

            return SendMail(To, from, subject, body);
        }

        public static bool SendToMail(string To, string ToCc, string from, string subject, string body)
        {
            string errorMessage;
            return SendToMail(To, ToCc, from, subject, body, out errorMessage);
        }

        public static bool SendMail(string To, string from, string subject, string body, bool isBodyHtml)
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


        #region IEmailSender Members

        bool IEmailSender.SendMail(string To, string ToCc, string ToBcc, string replyTo, string from, string displayName, string subject, string body, string attachFiles, bool IsBodyHtml, MailPriority priority, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        bool IEmailSender.SendMail(List<string> To, List<string> ToCc, List<string> ToBcc, string ReplyTo, string From, string DisplayName, string Subject, string Body, string AttachFiles, bool IsBodyHtml, string priority, out string ErrorMessage)
        {
            throw new NotImplementedException();
        }

        bool IEmailSender.SendMail(string To, string from, string subject, string body, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        bool IEmailSender.SendToMail(string To, string ToCc, string from, string subject, string body, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        bool IEmailSender.SendMail(string To, string from, string subject, string body, string attachfiles, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        bool IEmailSender.SendMail(string To, string from, string subject, string body, bool isBodyHtml, string attachfiles, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        bool IEmailSender.SendMail(string To, string from, string subject, string body, bool isBodyHtml, MailPriority priority, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        bool IEmailSender.SendMail(string To, string from, string subject, string body)
        {
            throw new NotImplementedException();
        }

        bool IEmailSender.SendToMail(string To, string ToCc, string from, string subject, string body)
        {
            throw new NotImplementedException();
        }

        bool IEmailSender.SendMail(string To, string from, string subject, string body, bool isBodyHtml)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
