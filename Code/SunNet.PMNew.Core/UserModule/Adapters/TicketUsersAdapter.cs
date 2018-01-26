using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.TicketModule;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.UserModule
{
    public class TicketUsersAdapter : IGetTicketUser
    {

        UserManager mgr;
        public TicketUsersAdapter()
        {
            mgr = new UserManager(
                                        ObjectFactory.GetInstance<IEmailSender>(),
                                        ObjectFactory.GetInstance<ICache<UserManager>>(),
                                        ObjectFactory.GetInstance<IUsersRepository>(),
                                        ObjectFactory.GetInstance<IRolesRepository>(),
                                        ObjectFactory.GetInstance<IModulesRepository>(),
                                        ObjectFactory.GetInstance<IRoleModulesRepository>(),
                                        ObjectFactory.GetInstance<IHideUserRepository>()
                                        );
        }

        public UsersEntity GetUserInfo(int uid)
        {
            return mgr.GetUser(uid);
        }
    }
}
