using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace SF.Framework.EmailSender
{
    public interface IEmailSender
    {

        bool SendMail(string To, string subject, string body, bool isBodyHtml, MailPriority priority);

        bool SendMail(List<string> To, List<string> ToCc, List<string> ToBcc, string DisplayName
            , string Subject, string Body, string AttachFiles, bool IsBodyHtml, string priority);

        bool SendMail(string To, string ToCc, string ToBcc, string displayName, string subject
            , string body, string attachFiles, bool IsBodyHtml, MailPriority priority);
    }
}