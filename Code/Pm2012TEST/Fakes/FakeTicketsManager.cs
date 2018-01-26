using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.TicketModule;

namespace Pm2012TEST.Fakes
{
    public class FakeTicketsManager
    {
        public void AddTicket(TicketsEntity te, TicketUsersEntity tu)
        {
            FakeEmailSender emailSender = new FakeEmailSender();
            emailSender.SendMail(tu.UserID.ToString(), "", te.Title, te.Description);
        }
    }
}
