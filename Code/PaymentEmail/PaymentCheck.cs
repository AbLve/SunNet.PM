using SF.Framework;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PaymentEmail
{
    public class PaymentCheck
    {
        private string emailBody;
        private string emailSubject;
        private string paymentManagerEmail;
        private string teamEmail;
        private ProposalTrackerApplication proposalTrackerApp;
        private InvoicesApplication _invoicesApp;
        private ProjectApplication projectApp;
        private string connStr = "PM";
        public PaymentCheck()
        {
            BootStrap.Config();
            paymentManagerEmail = ConfigurationManager.AppSettings["PaymentManagerEmail"];
            teamEmail = ConfigurationManager.AppSettings["TeamEmail"];
            proposalTrackerApp = new ProposalTrackerApplication();
            projectApp = new ProjectApplication();
            _invoicesApp = new InvoicesApplication();
        }

        public void Check()
        {
            DateTime now = DateTime.Now;
            DayOfWeek weekDay = now.DayOfWeek;
            if (weekDay == DayOfWeek.Saturday || weekDay == DayOfWeek.Sunday)
                return;
            CheckInvoiceAfterDevStatus();
            CheckInvoiceAfterThreeDays();
            CheckSendDateAfterFillInvoice();
            CheckSendDateAfterThreeDays();
            CheckReceiveDate();
        }
        /// <summary>
        /// 检查状态选择Awaiting Development的Proposal Tracker有没有添加Invoice
        /// </summary>
        public void CheckInvoiceAfterDevStatus()
        {
            try
            {
                string condition = " ProposalTracker.Status=4 AND Reminded=0 ";
                List<ProposalTrackerEntity> proposalTrackerEntities = proposalTrackerApp.GetEntitiesForPaymentEmail(condition, connStr);
                if (proposalTrackerEntities.Any())
                {
                    foreach (ProposalTrackerEntity entity in proposalTrackerEntities)
                    {
                        XElement xmlInvoice1 = XElement.Load(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "EmailTemps/RemindInvoice1.xml");
                        emailBody = xmlInvoice1.Element("email").Elements("content").First().Value;
                        emailSubject = xmlInvoice1.Element("email").Elements("subject").First().Value.Replace("{ProjectTitle}", entity.ProjectName);
                        emailBody = emailBody.Replace("{ProjectTitle}", entity.ProjectName)
                            .Replace("{ProposalTitle}", entity.Title)
                            .Replace("{Date}", DateTime.Now.ToString("MM/dd/yyyy"));

                        if (SFConfig.Components.EmailSender.SendMail(paymentManagerEmail, emailSubject, emailBody, true, MailPriority.Normal))
                        {
                            entity.Reminded = 1;
                            entity.RemindTime = DateTime.Now;
                            proposalTrackerApp.UpdateProposalTrackerForPayment(entity, connStr);
                            LogProvider.WriteLog(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + " send an email to: " + paymentManagerEmail);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.WriteLog(ex.ToString());
            }
        }
        /// <summary>
        /// 检查新建的Invoice三天之内有没有填写InvoiceNo
        /// </summary>
        public void CheckInvoiceAfterThreeDays()
        {
            try
            {
                DateTime now = DateTime.Now;
                DayOfWeek weekDay = now.DayOfWeek;
                string threeDaysAgo = weekDay > DayOfWeek.Wednesday ? now.AddDays(-3).ToString() : now.AddDays(-5).ToString();
                string condition = " (Reminded=0 or Reminded=1) and  RemindTime<'" + threeDaysAgo + "'";
                List<ProposalTrackerEntity> proposalTrackerEntities = proposalTrackerApp.GetEntitiesForPaymentEmail(condition, connStr);
                if (proposalTrackerEntities.Any())
                {
                    foreach (ProposalTrackerEntity entity in proposalTrackerEntities)
                    {
                        List<InvoiceEntity> paymentList = _invoicesApp.GetInvoiceByProposalId(entity.ProposalTrackerID);
                        List<InvoiceEntity> paymentList1 = paymentList.Where(p => p.InvoiceNo == null || p.InvoiceNo == "").ToList();
                        if (paymentList.Any() && !paymentList1.Any())
                            continue;

                        XElement xmlInvoice1 = XElement.Load(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "EmailTemps/RemindInvoice2.xml");
                        emailBody = xmlInvoice1.Element("email").Elements("content").First().Value;
                        emailSubject = xmlInvoice1.Element("email").Elements("subject").First().Value.Replace("{ProjectTitle}", entity.ProjectName);
                        emailBody = emailBody.Replace("{ProjectTitle}", entity.ProjectName)
                            .Replace("{ProposalTitle}", entity.Title)
                            .Replace("{Date}", DateTime.Now.ToString("MM/dd/yyyy"));

                        if (SFConfig.Components.EmailSender.SendMail(teamEmail, emailSubject, emailBody, true, MailPriority.Normal))
                        {
                            entity.Reminded = 2;
                            entity.RemindTime = DateTime.Now;
                            proposalTrackerApp.UpdateProposalTrackerForPayment(entity, connStr);
                            foreach (InvoiceEntity payment in paymentList1)
                            {
                                payment.Color = "red";
                                payment.ColorFor = "InvoiceNo";
                                _invoicesApp.UpdateForPaymentEmail(payment);
                            }
                            LogProvider.WriteLog(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + " send an email to: " + teamEmail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.WriteLog(ex.ToString());

            }
        }
        /// <summary>
        /// 检查填写过InvoiceNo的Invoice有没有填写SendDate
        /// </summary>
        public void CheckSendDateAfterFillInvoice()
        {
            try
            {
                string condition = " (Reminded=0 or Reminded=1 or Reminded=2) ";
                List<ProposalTrackerEntity> proposalTrackerEntities = proposalTrackerApp.GetEntitiesForPaymentEmail(condition, connStr);
                if (proposalTrackerEntities.Any())
                {
                    foreach (ProposalTrackerEntity entity in proposalTrackerEntities)
                    {
                        List<InvoiceEntity> paymentList = _invoicesApp.GetInvoiceByProposalId(entity.ProposalTrackerID);
                        paymentList = paymentList.Where(p => p.Approved == true && p.InvoiceNo != null && p.InvoiceNo != "" && p.SendOn == null).ToList();
                        if (!paymentList.Any())
                            continue;
                        List<string> milestoneStrList = paymentList.Select(p => p.Milestone).ToList();
                        string milestoneStr = string.Join(",", milestoneStrList);
                        XElement xmlInvoice1 = XElement.Load(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "EmailTemps/RemindSendDate1.xml");
                        emailBody = xmlInvoice1.Element("email").Elements("content").First().Value;
                        emailSubject = xmlInvoice1.Element("email").Elements("subject").First().Value;
                        emailBody = emailBody.Replace("{ProjectTitle}", entity.ProjectName)
                            .Replace("{ProposalTitle}", entity.Title)
                            .Replace("{Milestone}", milestoneStr)
                            .Replace("{Date}", DateTime.Now.ToString("MM/dd/yyyy"));

                        if (SFConfig.Components.EmailSender.SendMail(paymentManagerEmail, emailSubject, emailBody, true, MailPriority.Normal))
                        {
                            entity.Reminded = 3;
                            entity.RemindTime = DateTime.Now;
                            proposalTrackerApp.UpdateProposalTrackerForPayment(entity, connStr);
                            foreach (InvoiceEntity payment in paymentList)
                            {
                                payment.Color = "yellow";
                                payment.ColorFor = "SendOn";
                                _invoicesApp.UpdateForPaymentEmail(payment);
                            }
                            LogProvider.WriteLog(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + " send a email to: " + paymentManagerEmail);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.WriteLog(ex.ToString());
            }
        }
        /// <summary>
        /// 检查填写InvoiceNo三天后的Invoice有没有填写SendDate
        /// </summary>
        public void CheckSendDateAfterThreeDays()
        {
            try
            {
                DateTime now = DateTime.Now;
                DayOfWeek weekDay = now.DayOfWeek;
                string threeDaysAgo = weekDay > DayOfWeek.Wednesday ? now.AddDays(-3).ToString() : now.AddDays(-5).ToString();
                string condition = " (Reminded=0 or Reminded=1 or Reminded=2 or Reminded=3) and  RemindTime<'" + threeDaysAgo + "'";
                List<ProposalTrackerEntity> proposalTrackerEntities = proposalTrackerApp.GetEntitiesForPaymentEmail(condition, connStr);
                if (proposalTrackerEntities.Any())
                {
                    foreach (ProposalTrackerEntity entity in proposalTrackerEntities)
                    {
                        List<InvoiceEntity> paymentList = _invoicesApp.GetInvoiceByProposalId(entity.ProposalTrackerID);
                        paymentList = paymentList.Where(p => p.Approved == true && p.InvoiceNo != null && p.InvoiceNo != "" && p.SendOn == null).ToList();
                        if (!paymentList.Any())
                            continue;
                        List<string> milestoneStrList = paymentList.Select(p => p.Milestone).ToList();
                        string milestoneStr = string.Join(",", milestoneStrList);
                        XElement xmlInvoice1 = XElement.Load(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "EmailTemps/RemindSendDate2.xml");
                        emailBody = xmlInvoice1.Element("email").Elements("content").First().Value;
                        emailSubject = xmlInvoice1.Element("email").Elements("subject").First().Value;
                        emailBody = emailBody.Replace("{ProjectTitle}", entity.ProjectName)
                            .Replace("{ProposalTitle}", entity.Title)
                            .Replace("{Milestone}", milestoneStr)
                            .Replace("{Date}", DateTime.Now.ToString("MM/dd/yyyy"));

                        if (SFConfig.Components.EmailSender.SendMail(teamEmail, emailSubject, emailBody, true, MailPriority.Normal))
                        {
                            entity.Reminded = 4;
                            entity.RemindTime = DateTime.Now;
                            proposalTrackerApp.UpdateProposalTrackerForPayment(entity, connStr);
                            foreach (InvoiceEntity payment in paymentList)
                            {
                                payment.Color = "red";
                                payment.ColorFor = "SendOn";
                                _invoicesApp.UpdateForPaymentEmail(payment);
                            }
                            LogProvider.WriteLog(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + " send a email to: " + teamEmail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.WriteLog(ex.ToString());
            }
        }
        /// <summary>
        /// 检查填写了SendDate的Invoice若干天后有没有填写ReceiveDate
        /// </summary>
        public void CheckReceiveDate()
        {
            try
            {
                int ReceiveDateLimit = int.Parse(ConfigurationManager.AppSettings["ReceiveDateLimit"]);
                DateTime now = DateTime.Now;
                string someDaysAgo = now.AddDays(-ReceiveDateLimit).ToString();
                string condition = " (Reminded=0 or Reminded=1 or Reminded=2 or Reminded=3 or Reminded=4) and  RemindTime<'" + someDaysAgo + "'";
                List<ProposalTrackerEntity> proposalTrackerEntities = proposalTrackerApp.GetEntitiesForPaymentEmail(condition, connStr);
                if (proposalTrackerEntities.Any())
                {
                    foreach (ProposalTrackerEntity entity in proposalTrackerEntities)
                    {
                        List<InvoiceEntity> paymentList = _invoicesApp.GetInvoiceByProposalId(entity.ProposalTrackerID);
                        paymentList = paymentList.Where(p =>
                            p.Approved == true
                            && p.InvoiceNo != null && p.InvoiceNo != ""
                            && p.SendOn != null
                            && p.ReceiveOn == null).ToList();
                        if (!paymentList.Any())
                            continue;
                        List<string> milestoneStrList = paymentList.Select(p => p.Milestone).ToList();
                        string milestoneStr = string.Join(",", milestoneStrList);
                        XElement xmlInvoice1 = XElement.Load(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "EmailTemps/RemindReceiveDate.xml");
                        emailBody = xmlInvoice1.Element("email").Elements("content").First().Value;
                        emailSubject = xmlInvoice1.Element("email").Elements("subject").First().Value;
                        emailBody = emailBody.Replace("{ProjectTitle}", entity.ProjectName)
                            .Replace("{ProposalTitle}", entity.Title)
                            .Replace("{Milestone}", milestoneStr)
                            .Replace("{limit}", ReceiveDateLimit.ToString())
                            .Replace("{Date}", DateTime.Now.ToString("MM/dd/yyyy"));

                        if (SFConfig.Components.EmailSender.SendMail(teamEmail, emailSubject, emailBody, true, MailPriority.Normal))
                        {
                            entity.Reminded = 5;
                            entity.RemindTime = DateTime.Now;
                            proposalTrackerApp.UpdateProposalTrackerForPayment(entity, connStr);
                            foreach (InvoiceEntity payment in paymentList)
                            {
                                payment.Color = "red";
                                payment.ColorFor = "ReceiveOn";
                                _invoicesApp.UpdateForPaymentEmail(payment);
                            }
                            LogProvider.WriteLog(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + " send an email to: " + teamEmail);
                        }
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
