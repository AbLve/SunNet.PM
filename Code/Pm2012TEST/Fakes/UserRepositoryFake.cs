using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.UserModule;
namespace Pm2012TEST.Fakes
{
    public class UserRepositoryFake : IUsersRepository
    {
        #region IUsersRepository Members

        public bool ExistsUserName(string username)
        {
            throw new NotImplementedException();
        }

        public List<SunNet.PMNew.Entity.UserModel.UsersEntity> GetAllUsers(string status)
        {
            throw new NotImplementedException();
        }

        public SunNet.PMNew.Entity.UserModel.UsersEntity GetUserByUserName(string username)
        {
            throw new NotImplementedException();
        }

        public List<SunNet.PMNew.Entity.UserModel.UsersEntity> SearchUsers(string keywords)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRepository<UsersEntity> Members

        public int Insert(SunNet.PMNew.Entity.UserModel.UsersEntity entity)
        {
            return 1;
        }

        public bool Update(SunNet.PMNew.Entity.UserModel.UsersEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public SunNet.PMNew.Entity.UserModel.UsersEntity Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<SunNet.PMNew.Entity.UserModel.UsersEntity> SearchUsers(string keywords, string status, string orders, int page, int pageCount)
        {
            throw new NotImplementedException();
        }

        public int SearchUsersCount(string keywords, string status)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUsersRepository Members


        public List<SunNet.PMNew.Entity.UserModel.UsersEntity> SearchUsers(SunNet.PMNew.Entity.UserModel.SearchUsersRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUsersRepository Members


        SunNet.PMNew.Entity.UserModel.SearchUserResponse IUsersRepository.SearchUsers(SunNet.PMNew.Entity.UserModel.SearchUsersRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUsersRepository Members


        public int GetUserRoleIdByUserId(int UserID)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
