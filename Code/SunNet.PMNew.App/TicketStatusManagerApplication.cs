using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Core.TicketModule;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Core.UserModule;
using SunNet.PMNew.Framework;
using System.IO;
using System.Net.Mail;
using System.Xml.Linq;
using SF.Framework;
using SunNet.PMNew.Entity.ProjectModel;

namespace SunNet.PMNew.App
{
    public class TicketStatusManagerApplication : BaseApp
    {
        private ITicketsRepository ticketsRpst;
        private IEmailSender emailSender;
        private ITicketsUserRepository ticketsUserRpst;
        private IUsersRepository userRpst;
        private EmailManager mgr;
        UserApplication userApp = new UserApplication();

        public TicketStatusManagerApplication()
        {
            mgr = new EmailManager(
                  ObjectFactory.GetInstance<IEmailSender>()
                );
            ticketsUserRpst = ObjectFactory.GetInstance<ITicketsUserRepository>();
            emailSender = ObjectFactory.GetInstance<IEmailSender>();
            ticketsRpst = ObjectFactory.GetInstance<ITicketsRepository>();
            userRpst = ObjectFactory.GetInstance<IUsersRepository>();
        }



        private string GetEmailList(List<UsersEntity> list)
        {
            List<string> listEmailString = new List<string>();

            if (null != list && list.Count > 0)
            {
                foreach (UsersEntity item in list)
                {
                    listEmailString.Add(item.Email);
                }
            }
            return ListToString(listEmailString);
        }

        /// <summary>
        /// for email
        /// </summary>
        /// <param name="listInput"></param>
        /// <returns></returns>
        private static string ListToString(List<string> listInput)
        {
            string output = "";
            foreach (string s in listInput)
            {
                output += s + ";";
            }
            return output;
        }

        /// <summary>
        /// get list user
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public List<UsersEntity> GetTicketUserListByTicketID(int tid)
        {
            List<TicketDistinctUsersResponse> list = ticketsUserRpst.GetListDistinctUsersByTicketId(tid);
            List<UsersEntity> listUsers = new List<UsersEntity>();
            UsersEntity user = new UsersEntity();

            if (null != list && list.Count > 0)
            {
                foreach (TicketDistinctUsersResponse item in list)
                {
                    if (item.UserID > 0)
                    {
                        user = userRpst.Get(item.UserID);
                        listUsers.Add(user);
                    }
                }
            }
            return listUsers;
        }

        /// <summary>
        /// according to tickets state ,send email to matching user
        /// </summary>
        /// <param name="te"></param>
        /// <param name="ue"></param>
        public string SendEmailByTicketState(TicketsEntity te)
        {
            List<UsersEntity> list = GetTicketUserListByTicketID(te.TicketID);
            if (list == null || list.Count <= 0) return "";
            string to = "";
            switch (te.Status)
            {
                case TicketsState.Submitted:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.PM));
                    break;
                case TicketsState.Cancelled:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.CLIENT));
                    break;
                case TicketsState.PM_Reviewed:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.QA ||
                                                        x.Role == RolesEnum.DEV ||
                                                        x.Role == RolesEnum.Contactor));
                    break;
                case TicketsState.Waiting_For_Estimation:
                    to = GetEmailList(list.FindAll(x => x.UserID == te.EsUserID));
                    break;
                case TicketsState.PM_Verify_Estimation:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.PM));
                    break;
                case TicketsState.Waiting_Confirm:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.Sales));
                    break;
                case TicketsState.Denied:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.PM ||
                                                        x.Role == RolesEnum.Sales ||
                                                        x.Role == RolesEnum.DEV ||
                                                        x.Role == RolesEnum.Contactor ||
                                                        x.Role == RolesEnum.QA));
                    break;
                case TicketsState.Estimation_Approved:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.PM ||
                                                        x.Role == RolesEnum.DEV ||
                                                        x.Role == RolesEnum.Contactor ||
                                                        x.Role == RolesEnum.QA));
                    break;
                case TicketsState.Testing_On_Local:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.QA));
                    break;
                case TicketsState.Tested_Fail_On_Local:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.DEV ||
                                                        x.Role == RolesEnum.Contactor));
                    break;
                case TicketsState.Tested_Success_On_Local:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.DEV ||
                                                        x.Role == RolesEnum.Contactor));
                    break;
                case TicketsState.Testing_On_Client:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.QA));
                    break;
                case TicketsState.Tested_Fail_On_Client:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.DEV ||
                                                        x.Role == RolesEnum.Contactor));
                    break;
                case TicketsState.Tested_Success_On_Client:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.DEV ||
                                                        x.Role == RolesEnum.Contactor));
                    break;
                case TicketsState.PM_Deny:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.DEV ||
                                                        x.Role == RolesEnum.Contactor ||
                                                        x.Role == RolesEnum.QA));
                    break;
                case TicketsState.Ready_For_Review:
                    {
                        UsersEntity usersEntity = userApp.GetUser(te.CreatedBy);
                        if (usersEntity.Role == RolesEnum.Supervisor)
                        {
                            to = usersEntity.Email;
                        }
                        else
                        {
                            to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.CLIENT));
                        }
                        break;
                    }
                case TicketsState.Not_Approved:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.DEV ||
                                                        x.Role == RolesEnum.Contactor ||
                                                        x.Role == RolesEnum.QA ||
                                                        x.Role == RolesEnum.Leader ||
                                                        x.Role == RolesEnum.PM));
                    break;
                case TicketsState.Completed:
                    to = GetEmailList(list.FindAll(x => x.Role == RolesEnum.DEV ||
                                                        x.Role == RolesEnum.Contactor ||
                                                        x.Role == RolesEnum.QA ||
                                                        x.Role == RolesEnum.Leader ||
                                                        x.Role == RolesEnum.PM));
                    break;
            }
            return to;
        }

        #region ticket status email

        /// <summary>
        /// client create ticket 
        /// </summary>
        /// <param name="listInput"></param>
        /// <returns></returns>

        public void SendEmailToPMWhenTicketAdd(TicketsEntity te, UsersEntity createUser)
        {
            ProjectsEntity projectEntity = new ProjectApplication().Get(te.ProjectID);

            string contentTemplete = GetEmailExecuter("SendPMTicketAdd.txt");
            string to = "";
            string from = Config.DefaultSendEmail;

            to = userApp.GetUser(projectEntity.PMID).Email;


            string subject = string.Format("{0} ticket {1} submitted", projectEntity.Title, te.TicketCode);
            string content = string.Empty;
            if (!string.IsNullOrEmpty(contentTemplete.Trim()))
            {
                content = contentTemplete.Trim();
                content = content.Replace("[Project]", projectEntity.Title);
                content = content.Replace("[TicketID]", te.TicketCode);
                content = content.Replace("[ClientName]", createUser.FirstAndLastName);
            }

            emailSender.SendMail(to, from, subject, content.ToString());
        }

        private string GetNameById(int id)
        {
            string Name = "";
            Name = userApp.GetLastNameFirstName(id);
            return Name;
        }

        private string GetEmailByUserId(int id)
        {
            UsersEntity entity = userApp.GetUser(id);
            return null == entity ? "" : entity.Email;
        }

        private string GetProjectTitleByPid(int pid)
        {
            ProjectApplication proApp = new ProjectApplication();
            ProjectsEntity project = proApp.Get(pid);
            if (project != null)
            {
                return project.Title;
            }
            return "";
        }

        /// <summary>
        /// 新建Ticket时，发邮件给相关人员
        /// </summary>
        /// <param name="tu"></param>
        /// <param name="ticketEntity"></param>
        public void SendEmailToAssignedUser(TicketUsersEntity tu, TicketsEntity ticketEntity, UsersEntity createUser)
        {
            string to = "";

            to = GetEmailByUserId(tu.UserID);
            string projectTitle = GetProjectTitleByPid(ticketEntity.ProjectID);

            XElement xmlInvoice1 = XElement.Load(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Template/SendEmailToAssignUser.xml");
            emailBody = xmlInvoice1.Element("email").Elements("content").First().Value;
            emailSubject = xmlInvoice1.Element("email").Elements("subject").First().Value
                .Replace("{TicketID}", ticketEntity.TicketID.ToString())
                .Replace("{TicketTitle}", ticketEntity.Title);
            emailBody = emailBody.Replace("{TicketID}", ticketEntity.TicketID.ToString())
                .Replace("{Project}", projectTitle)
                .Replace("{CreateName}", createUser.FirstAndLastName)
                .Replace("{TicketTitle}", ticketEntity.Title);

            SFConfig.Components.EmailSender.SendMail(to, emailSubject, emailBody, true, MailPriority.Normal);
        }

        /// <summary>
        /// Send Email to Responsible when PM,Dev,QA,Seller Review Ticket
        /// </summary>
        /// <param name="tu"></param>
        /// <param name="ticketEntity"></param>
        public void SendEmailToResponsibile(TicketUsersEntity tu, TicketsEntity ticketEntity, UsersEntity createUser)
        {
            string to = "";

            to = GetEmailByUserId(tu.UserID);
            string projectTitle = GetProjectTitleByPid(ticketEntity.ProjectID);
            XElement xmlInvoice1 = XElement.Load(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Template/SendEmailToResponsible.xml");
            emailBody = xmlInvoice1.Element("email").Elements("content").First().Value;
            emailSubject = xmlInvoice1.Element("email").Elements("subject").First().Value
                .Replace("{TicketID}", ticketEntity.TicketID.ToString())
                .Replace("{TicketTitle}", ticketEntity.Title);
            emailBody = emailBody.Replace("{TicketID}", ticketEntity.TicketID.ToString())
                .Replace("{Project}", projectTitle)
                .Replace("{CreateName}", createUser.FirstAndLastName)
                .Replace("{TicketTitle}", ticketEntity.Title);

            SFConfig.Components.EmailSender.SendMail(to, emailSubject, emailBody, true, MailPriority.Normal);
        }

        public void SendEmailWithClientNotApp(TicketsEntity te)
        {
            string contentTemplete = GetEmailExecuter("SendEmailwithStatusNotApproved.txt");

            string from = Config.DefaultSendEmail;
            string to = "";
            to = SendEmailByTicketState(te);

            string subject = string.Format("{0} ticket {1} Not approved", GetProjectTitleByPid(te.ProjectID), te.TicketCode);
            string content = string.Empty;
            if (!string.IsNullOrEmpty(contentTemplete.Trim()))
            {
                content = contentTemplete.Trim();
                content = content.Replace("[ticketId]", te.TicketCode.ToString());
                content = content.Replace("[clientUserName]", GetNameById(te.ModifiedBy));
            }

            emailSender.SendMail(to, from, subject, content.ToString());
        }

        public string GetEmailExecuter(string fileName)
        {
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "Template\\" + fileName;
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                return "";
            }
        }

        public void SendEmailtoPMForFeedBack(FeedBacksEntity fe)
        {
            string to = "";
            TicketsEntity te = ticketsRpst.Get(fe.TicketID);
            te.Status = TicketsState.Submitted;
            to = SendEmailByTicketState(te);//get pm's email

            //xml 发送邮件
            XElement xmlInvoice1 = XElement.Load(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Template/SendEmailToPmForFeedBack.xml");
            emailBody = xmlInvoice1.Element("email").Elements("content").First().Value;
            emailSubject = xmlInvoice1.Element("email").Elements("subject").First().Value
                .Replace("{TicketID}", te.TicketID.ToString())
                .Replace("{TicketTitle}", te.Title);
            emailBody = emailBody.Replace("{TicketID}", te.TicketID.ToString())
                .Replace("{userName}", GetNameById(fe.CreatedBy));

            SFConfig.Components.EmailSender.SendMail(to, emailSubject, emailBody, true, MailPriority.Normal);
        }

        /// <summary>
        /// wait client feedback
        /// </summary>
        /// <param name="fe"></param>

        public void SendEmailtoClientForFeedBack(FeedBacksEntity fe)
        {
            string contentTemplete = GetEmailExecuter("SendEmailToClientForFeedBack.txt");

            string from = Config.DefaultSendEmail;
            string to = "";
            TicketsEntity te = ticketsRpst.Get(fe.TicketID);
            var creators = ticketsUserRpst.GetTicketUser(fe.TicketID, TicketUsersType.Create);
            var creator = creators != null && creators.Count > 0 ? creators[0] : null;
            if (creator != null)
            {
                var user = userRpst.Get(creator.UserID);
                to = user.Email;
                string subject = string.Format("Ticket #{0} {1} needs your feedback.", te.TicketCode, te.Title);
                string content = string.Empty;
                if (!string.IsNullOrEmpty(contentTemplete.Trim()))
                {
                    content = contentTemplete.Trim();
                    content = content.Replace("[ClientName]", user.FirstAndLastName);
                    content = content.Replace("[PmName]", GetUnameByRoleTypeAndTicketId(te.TicketID, TicketUsersType.PM));
                }

                emailSender.SendMail(to, @from, subject, content.ToString());
            }
        }

        /// <summary>
        /// Creator(Client) need another clients' feedback.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="clients">The clients.</param>
        public void SendEmailtoClientForFeedBack(int ticketId, params int[] clients)
        {
            if (clients == null)
                return;
            string contentTemplete = GetEmailExecuter("SendEmailToClientForFeedBack.txt");

            string from = Config.DefaultSendEmail;
            string to = "";
            TicketsEntity te = ticketsRpst.Get(ticketId);
            var creators = ticketsUserRpst.GetTicketUser(ticketId, TicketUsersType.Create);
            var creator = creators != null && creators.Count > 0 ? creators[0] : null;
            if (creator != null)
            {
                var createClient = userRpst.Get(creator.UserID);
                foreach (var client in clients)
                {
                    var user = userRpst.Get(client);
                    to = user.Email;
                    string subject = string.Format("Ticket #{0} {1} needs your feedback.", te.TicketCode, te.Title);
                    string content = string.Empty;
                    if (!string.IsNullOrEmpty(contentTemplete.Trim()))
                    {
                        content = contentTemplete.Trim();
                        content = content.Replace("[ClientName]", user.FirstAndLastName);
                        content = content.Replace("[PmName]", createClient.FirstAndLastName);
                    }
                    emailSender.SendMail(to, @from, subject, content.ToString());
                }
            }
        }
        private string GetUnameByRoleTypeAndTicketId(int ticketID, TicketUsersType role)
        {
            List<TicketUsersEntity> list = ticketsUserRpst.GetListUsersByTicketId(ticketID);

            TicketUsersEntity tu = new TicketUsersEntity();

            switch (role)
            {
                case TicketUsersType.PM:
                    tu = list.FindAll(x => x.Type == TicketUsersType.PM)[0];
                    break;
                case TicketUsersType.Dev:
                    break;
                case TicketUsersType.QA:
                    break;
                case TicketUsersType.Other:
                    break;
                case TicketUsersType.Create:
                    break;
                default:
                    break;
            }
            if (null != tu)
            {
                return GetNameById(tu.UserID);
            }
            return "";
        }


        public void SendEmailFeedbackReplied(UsersEntity fromUser, UsersEntity toUser, int ticketId)
        {
            string contentTemplete = GetEmailExecuter("SendEmailToFeedbackReplied.txt");
            string from = Config.DefaultSendEmail;
            string to = toUser.Email;
            TicketsEntity ticket = ticketsRpst.Get(ticketId);
            string subject = string.Format("{0} ticket {1} Feedback Replied", ticket.ProjectTitle, ticket.TicketID);
            string content = string.Empty;
            if (!string.IsNullOrEmpty(contentTemplete.Trim()))
            {
                content = contentTemplete.Trim();
                content = content.Replace("[ToUser]", toUser.FirstAndLastName);
                content = content.Replace("[FromUser]", fromUser.FirstAndLastName);
            }
            emailSender.SendMail(to, from, subject, content.ToString());
        }

        private string emailBody;
        private string emailSubject;
        /// <summary>
        /// 发邮件给创建者，请他确认ticket
        /// </summary>
        /// <param name="fe"></param>
        public void SendEmailtoCreate(TicketsEntity ticketEntity, UsersEntity userEntity)
        {
            //string contentTemplete = GetEmailExecuter("SendEmailForReadyforReview.txt");
            //string from = Config.DefaultSendEmail;
            string to = userApp.GetUser(ticketEntity.CreatedBy).Email;

            //string subject = string.Format("{0}  ticket {1} is complete. Please review.", GetProjectTitleByPid(ticketEntity.ProjectID), ticketEntity.TicketID);
            //string content = string.Empty;

            //xml 发送邮件
            XElement xmlInvoice1 = XElement.Load(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Template/SendEmailForReadyforReview.xml");
            emailBody = xmlInvoice1.Element("email").Elements("content").First().Value;
            emailSubject = xmlInvoice1.Element("email").Elements("subject").First().Value.Replace("{ProjectTitle}", ticketEntity.ProjectTitle);
            emailBody = emailBody.Replace("{TicketID}", ticketEntity.TicketID.ToString())
                .Replace("{TicketTitle}", ticketEntity.Title)
                .Replace("{TicketDescription}", ticketEntity.Description)
                .Replace("{PMUserName}", userEntity.FirstAndLastName)
                .Replace("{Date}", DateTime.Now.ToString("MM/dd/yyyy"));

            SFConfig.Components.EmailSender.SendMail(to, emailSubject, emailBody, true, MailPriority.Normal);

            //if (!string.IsNullOrEmpty(contentTemplete.Trim()))
            //{
            //    content = contentTemplete.Trim();
            //    content = content.Replace("[TicketID]", ticketEntity.TicketID.ToString());
            //    content = content.Replace("[PMUserName]", userEntity.FirstAndLastName);
            //}

            //emailSender.SendMail(to, from, subject, content.ToString());
        }

        /// <summary>
        /// 发邮件给创建者，提醒Ticket Status改为Cancelled,请他确认ticket
        /// </summary>
        /// <param name="fe"></param>
        public void SendEmailByCancel(TicketsEntity ticketEntity, UsersEntity userEntity)
        {
            string contentTemplete = GetEmailExecuter("SendEmailToTeemByClientCancleTicket.txt");
            string from = Config.DefaultSendEmail;
            string to = userApp.GetUser(ticketEntity.CreatedBy).Email;

            string subject = string.Format("{0}  ticket {1} has been cancelled", GetProjectTitleByPid(ticketEntity.ProjectID), ticketEntity.TicketID);
            string content = string.Empty;
            if (!string.IsNullOrEmpty(contentTemplete.Trim()))
            {
                content = contentTemplete.Trim();
                content = content.Replace("[ID]", ticketEntity.TicketID.ToString());
                content = content.Replace("[Client]", userEntity.FirstAndLastName);
            }

            emailSender.SendMail(to, from, subject, content.ToString());
        }

        /// <summary>
        /// 发邮件给创建者，提醒Ticket Status改为Internal Cancelled
        /// </summary>
        /// <param name="ticketEntity">Ticket</param>
        /// <param name="userEntity">Ticket Creator</param>
        public void SendEmailByInternalCancel(TicketsEntity ticketEntity, UsersEntity userEntity)
        {
            string from = Config.DefaultSendEmail;
            string to = userApp.GetUser(ticketEntity.CreatedBy).Email;

            XElement xmlElement = XElement.Load(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Template/SendEmailInternalCancel.xml");
            emailBody = xmlElement.Element("email").Elements("content").First().Value;
            emailSubject = xmlElement.Element("email").Elements("subject").First().Value
                .Replace("{TicketID}", ticketEntity.TicketID.ToString())
                .Replace("{TicketTitle}", ticketEntity.Title);
            emailBody = emailBody.Replace("{TicketTitle}", ticketEntity.Title)
                .Replace("{TicketID}", ticketEntity.TicketID.ToString())
                .Replace("{ProjectTitle}", ticketEntity.ProjectTitle)
                .Replace("{UserName}", userEntity.FirstAndLastName);

            SFConfig.Components.EmailSender.SendMail(to, emailSubject, emailBody, true, MailPriority.Normal);
        }

        public void SendEmailWaitConfirm(TicketsEntity ticketEntity)
        {
            string contentTemplete = GetEmailExecuter("SendEmailWaitConfirm.txt");
            string from = Config.DefaultSendEmail;
            string to = userApp.GetUser(ticketEntity.CreatedBy).Email;

            string subject = string.Format("{0}  ticket {1} ", GetProjectTitleByPid(ticketEntity.ProjectID), ticketEntity.TicketID);
            string content = string.Empty;
            if (!string.IsNullOrEmpty(contentTemplete.Trim()))
            {
                content = contentTemplete.Trim();
                content = content.Replace("[TicketID]", ticketEntity.TicketID.ToString());
            }

            emailSender.SendMail(to, from, subject, content.ToString());
        }


        #endregion

    }
}
