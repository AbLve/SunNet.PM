using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.UserModule;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Entity.UserModel;

namespace Pm2012TEST.Fakes
{
    public class FakeGetUserInfo : IGetUserInfo
    {
        public UsersEntity GetId(int id)
        {
            return null;
        }

        #region IRepository<UsersEntity> Members

        public int Insert(UsersEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(UsersEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public UsersEntity Get(int entityId)
        {
            UsersEntity user = new UsersEntity();
            user.RoleID = 1;
            user.UserID = 1;
            user.Role = RolesEnum.Roles.QA;
            user.Email = "a@1263.com";
            return user;
        }

        #endregion
    }
}
