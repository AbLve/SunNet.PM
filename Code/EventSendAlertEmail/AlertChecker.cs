using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Xml.Linq;
using System.Configuration;
using SF.Framework;

namespace EventSendAlertEmail
{
    public class AlertChecker
    {
        private string emailBody;
        private string emailSubject;
        private XElement root;
        public AlertChecker()
        {
            root = XElement.Load(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "EventAlertEmail.xml");
        }

        EventEntity eventEntity = new EventEntity();
        public void Check()
        {
            try
            {
                string website = ConfigurationManager.AppSettings["WebSite"];
                List<EventEntity> EventEntities = eventEntity.GetEventsNeedAlert().Where(c => c.HasAlert == false).ToList();
                foreach (EventEntity myevent in EventEntities)
                {
                    string formatFromDay = myevent.FromDay.ToString("M/d/yyyy") + " at ";
                    string amOrpm = " AM";
                    if (myevent.FromDay.Hour > 12) { amOrpm = " PM"; }
                    formatFromDay += myevent.FromDay.ToString("h:mm") + amOrpm;

                    emailBody = root.Element("email").Elements("content").First().Value;
                    emailSubject = root.Element("email").Elements("subject").First().Value;

                    emailBody = emailBody.Replace("{ToFirstName}", myevent.CreateUserFristName)
                                           .Replace("{ToLastName}", myevent.CreateUserFristName)
                                           .Replace("{Link}", ConfigurationManager.AppSettings["WebSite"] + "/Event/Index.aspx")
                                           .Replace("{Date}", DateTime.Now.ToString("MM/dd/yyyy"))
                                           .Replace("{EventName}", myevent.Name)
                                           .Replace("{FromDay}", formatFromDay);
                    emailSubject = emailSubject.Replace("{EventName}", myevent.Name);

                    if (SFConfig.Components.EmailSender.SendMail(myevent.CreateEmailAccount, emailSubject, emailBody, true, MailPriority.Normal))
                    {
                        myevent.HasAlert = true;
                        eventEntity.Update(myevent);
                        LogProvider.WriteLog(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + " send a email to: " + myevent.CreateEmailAccount);
                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.WriteLog(ex.ToString());
            }
        }
    }
}
