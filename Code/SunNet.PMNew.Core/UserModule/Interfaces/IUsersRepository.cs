using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.UserModel;
using System.Data;
using SunNet.PMNew.Entity.TicketModel.UserTicket;
using SunNet.PMNew.Entity.UserModel.UserModel;

namespace SunNet.PMNew.Core.UserModule
{
    public interface IUsersRepository : IRepository<UsersEntity>
    {
        bool ExistsUserName(string username, int exceptThis);
        UsersEntity GetUserByUserName(string username);
        SearchUserResponse SearchUsers(SearchUsersRequest request);
        List<UserTicketModel> SearchUserWithRole(List<RolesEnum> roles, string hideUserIds);
        List<DashboardUserModel> GetUserByRoles(List<RolesEnum> roles);
        bool IsLoginSuccess(string uname, string upwd);
    }
}
