using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace SunNet.PMNew.Framework.Utils
{
    public interface IEmailSender
    {
        bool SendMail(string To, string ToCc, string ToBcc, string replyTo, string from, string displayName, string subject, string body, string attachFiles, bool IsBodyHtml, MailPriority priority, out string errorMessage);
        bool SendMail(List<string> To, List<string> ToCc, List<string> ToBcc, string ReplyTo, string From, string DisplayName, string Subject, string Body, string AttachFiles, bool IsBodyHtml, string priority, out string ErrorMessage);
        bool SendMail(string To, string from, string subject, string body, out string errorMessage);
        bool SendToMail(string To, string ToCc, string from, string subject, string body, out string errorMessage);
        bool SendMail(string To, string from, string subject, string body, string attachfiles, out string errorMessage);
        bool SendMail(string To, string from, string subject, string body, bool isBodyHtml, string attachfiles, out string errorMessage);
        bool SendMail(string To, string from, string subject, string body, bool isBodyHtml, MailPriority priority, out string errorMessage);
        bool SendMail(string To, string from, string subject, string body);
        bool SendToMail(string To, string ToCc, string from, string subject, string body);
        bool SendMail(string To, string from, string subject, string body, bool isBodyHtml);
    }
}