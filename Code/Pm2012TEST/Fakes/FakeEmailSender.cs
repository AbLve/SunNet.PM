using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils;

namespace Pm2012TEST.Fakes
{
    class FakeEmailSender : IEmailSender
    {
        public int Count { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        #region IEmailSender Members

        public bool SendMail(string To, string ToCc, string ToBcc, string replyTo, string from, string displayName, string subject, string body, string attachFiles, bool IsBodyHtml, System.Net.Mail.MailPriority priority, out string errorMessage)
        {
            errorMessage = "";
            Count++;
            return true;
        }

        public bool SendMail(List<string> To, List<string> ToCc, List<string> ToBcc, string ReplyTo, string From, string DisplayName, string Subject, string Body, string AttachFiles, bool IsBodyHtml, string priority, out string ErrorMessage)
        {
            ErrorMessage = "";
            Count++;
            return true;
        }

        public bool SendMail(string To, string from, string subject, string body, out string errorMessage)
        {
            errorMessage = "";
            Count++;
            return true;
        }

        public bool SendToMail(string To, string ToCc, string from, string subject, string body, out string errorMessage)
        {
            errorMessage = "";
            Count++;
            return true;
        }

        public bool SendMail(string To, string from, string subject, string body, string attachfiles, out string errorMessage)
        {
            errorMessage = "";
            Count++;
            return true;
        }

        public bool SendMail(string To, string from, string subject, string body, bool isBodyHtml, string attachfiles, out string errorMessage)
        {
            errorMessage = "";
            Count++;
            return true;
        }

        public bool SendMail(string To, string from, string subject, string body, bool isBodyHtml, System.Net.Mail.MailPriority priority, out string errorMessage)
        {
            errorMessage = "";
            Count++;
            return true;
        }

        public bool SendMail(string To, string from, string subject, string body)
        {
            Title = subject;
            Body = body;
            Count++;
            return true;
        }

        public bool SendToMail(string To, string ToCc, string from, string subject, string body)
        {
            Count++;
            return true;
        }

        public bool SendMail(string To, string from, string subject, string body, bool isBodyHtml)
        {
            Count++;
            return true;
        }

        #endregion
    }
}
