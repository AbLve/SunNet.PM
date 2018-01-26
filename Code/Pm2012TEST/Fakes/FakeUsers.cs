using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.UserModule;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Core.TicketModule;

namespace Pm2012TEST.Fakes
{
    public class FakeUsersRepository : ITicketsUserRepository
    {
        public UsersEntity CreateUser()
        {
            UsersEntity user = new UsersEntity();
            user.RoleID = 2;
            user.UserID = 1;
            user.Email = "Aaron1@sunnet.us";
            return user;
        }


        #region ITicketsUserRepository Members

        public List<TicketUsersEntity> GetListUsersByTicketId(int tid)
        {
            List<TicketUsersEntity> list = new List<TicketUsersEntity>();
            if (tid >= 999)
            {
                TicketUsersEntity user = null;
                for (int i = 999; i < 1010; i++)
                {
                    user = new TicketUsersEntity();
                    user.UserID = 1011-i ;
                    user.TicketID = i;
                    list.Add(user);
                }
            }
            return list;
        }

        #endregion

        #region IRepository<TicketUsersEntity> Members

        public int Insert(TicketUsersEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(TicketUsersEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public TicketUsersEntity Get(int entityId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITicketsUserRepository Members


        public List<TicketDistinctUsersResponse> GetListDistinctUsersByTicketId(int ticketId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITicketsUserRepository Members


        public bool RemoveTicketUser(string ticketIdListString)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITicketsUserRepository Members


        public bool RemoveTicketUser(int ticketID, string userIdList)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
