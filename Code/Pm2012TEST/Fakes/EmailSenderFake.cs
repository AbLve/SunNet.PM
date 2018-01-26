using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils;

namespace Pm2012TEST.Fakes
{
    class EmailSenderFake : IEmailSender
    {
        #region IEmailSender Members

        public bool SendMail(string To, string ToCc, string ToBcc, string replyTo, string from, string displayName, string subject, string body, string attachFiles, bool IsBodyHtml, System.Net.Mail.MailPriority priority, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool SendMail(List<string> To, List<string> ToCc, List<string> ToBcc, string ReplyTo, string From, string DisplayName, string Subject, string Body, string AttachFiles, bool IsBodyHtml, string priority, out string ErrorMessage)
        {
            throw new NotImplementedException();
        }

        public bool SendMail(string To, string from, string subject, string body, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool SendToMail(string To, string ToCc, string from, string subject, string body, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool SendMail(string To, string from, string subject, string body, string attachfiles, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool SendMail(string To, string from, string subject, string body, bool isBodyHtml, string attachfiles, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool SendMail(string To, string from, string subject, string body, bool isBodyHtml, System.Net.Mail.MailPriority priority, out string errorMessage)
        {
            throw new NotImplementedException();
        }
        public string SendTo { get; set; }
        public bool SendMail(string To, string from, string subject, string body)
        {
            SendTo = To;
            return true;
        }

        public bool SendToMail(string To, string ToCc, string from, string subject, string body)
        {
            throw new NotImplementedException();
        }

        public bool SendMail(string To, string from, string subject, string body, bool isBodyHtml)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
