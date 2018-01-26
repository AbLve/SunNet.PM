using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.TicketModel;
using System.IO;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Entity.UserModel;
using StructureMap;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// Summary description for SendEmailClientCancelTicketHandler
    /// </summary>
    public class SendEmailClientCancelTicketHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            TicketsApplication ticketAPP = new TicketsApplication();
            UserApplication userAPP  = new UserApplication();
            try
            {
                if (IdentityContext.UserID <= 0)
                    return;

                int tid = Convert.ToInt32(context.Request["tid"]);

                TicketsEntity ticketEntity = ticketAPP.GetTickets(tid);
                UsersEntity userEntity = userAPP.GetUser(IdentityContext.UserID);
                if (ticketEntity != null)
                {
                    string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "Template\\SendEmailToTeemByClientCancleTicket.txt" ;
                    string contentTemplete = string.Empty;
                    //if (File.Exists(filePath))
                    //{
                    //    contentTemplete = File.ReadAllText(filePath);
                    //}
                    contentTemplete = string.Format("<br/>The Ticket {0}, {1} has been cancelled by {2}<br/>"
                        , ticketEntity.TicketID, ticketEntity.Title, userEntity.FirstAndLastName);

                    string from = Config.DefaultSendEmail;
                    string to = "team@sunnet.us";


                    string subject = "Ticket be cancelled";
                    ObjectFactory.GetInstance<IEmailSender>().SendMail(to, from, subject, contentTemplete);
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:SendEmailClientCancelTicketHandler.ashx Messages:\r\n{0}", ex));
                return;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}