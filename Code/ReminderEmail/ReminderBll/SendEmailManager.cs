using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Xml.Linq;
using ReminderEmail.ReminderModel;
using SF.Framework;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;

namespace ReminderEmail
{
    /// <summary>
    /// 发送邮件管理类
    /// </summary>
    public static class SendEmailManager
    {
        public static void SendReminderEmail(UsersEntity user, List<ReminderModel.ReminderModel> tickets)
        {
            try
            {
                string emailSubject = string.Empty;
                string emailBodyBox = string.Empty;
                string emailBodyData = string.Empty;

                string basePath = Path.GetFullPath(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "../../");
                XElement xmlReminderEmailBody = XElement.Load(Path.Combine(basePath, "EmailTemps/ReminderBox.xml"));
                emailSubject = xmlReminderEmailBody.Element("email").Element("subject").Value;
                emailSubject = emailSubject.Replace("{{Date}}", DateTime.Now.Date.ToShortDateString());

                emailBodyBox = xmlReminderEmailBody.Element("email").Element("content").Value;
                emailBodyBox = emailBodyBox.Replace("{{CurrentDateTime}}", DateTime.Now.ToString());

                foreach (var ticket in tickets)
                {
                    string tempData = string.Empty;
                    XElement xmlData = XElement.Load(Path.Combine(basePath, "EmailTemps/ReminderData.xml"));
                    tempData = xmlData.Element("content").Value;
                    tempData = tempData
                        .Replace("{{ProjectId}}", ticket.ProjectId.ToString())
                        .Replace("{{ProjectName}}", ticket.ProjectName)
                        .Replace("{{TicketId}}", ticket.TicketId.ToString())
                        .Replace("{{TicketTitle}}", ticket.TicketTitle)
                        .Replace("{{TicketModeifiedOn}}", ticket.ModifiedOn.HasValue ? ticket.ModifiedOn.Value.ToString() : "");

                    emailBodyData += tempData;
                }

                emailBodyBox = emailBodyBox.Replace("{{ReminderData}}", emailBodyData);

                if (SFConfig.Components.EmailSender.SendMail(user.Email, emailSubject, emailBodyBox, true, MailPriority.Normal) == false)
                {
                    LogProvider.WriteLog(string.Format("{0} Send mailbox failure: user:" + user.FirstName + user.LastName + " emial:" + user.Email + " emailSubject:" + emailSubject));
                }
            }
            catch (Exception ex)
            {
                LogProvider.WriteLog(ex.ToString());
            }
        }
    }
}
