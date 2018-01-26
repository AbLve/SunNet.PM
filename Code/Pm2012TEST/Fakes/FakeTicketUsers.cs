using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.TicketModel;

namespace Pm2012TEST.Fakes
{
    public class FakeTicketUsers
    {
        public List<TicketUsersEntity> GetUserListByTicketID(int id)
        {
            List<TicketUsersEntity> list = new List<TicketUsersEntity>();
            if (id == 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    list.Add(CreateTicketsEntity(i));
                }
            }
            return list;
        }
        //create user ticketUser Entity
        public TicketUsersEntity CreateTicketsEntity(int userId)
        {
            TicketUsersEntity info = new TicketUsersEntity();
            info.TicketID = 1;
            info.UserID = userId;
            info.TUID = userId;//auto-incrementing
            return info;
        }


    }
}
