using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;

using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Core.ProjectModule;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.UserModule.Adapters
{
    public class SearchUsersAdapter : ISearchUsers
    {
        UserManager mgr;
        public SearchUsersAdapter()
        {
            mgr =new UserManager(
                                        ObjectFactory.GetInstance<IEmailSender>(),
                                        ObjectFactory.GetInstance<ICache<UserManager>>(),
                                        ObjectFactory.GetInstance<IUsersRepository>(),
                                        ObjectFactory.GetInstance<IRolesRepository>(),
                                        ObjectFactory.GetInstance<IModulesRepository>(),
                                        ObjectFactory.GetInstance<IRoleModulesRepository>(),
                                        ObjectFactory.GetInstance<IHideUserRepository>()
                                        );
        }

        #region ISearchUsers Members

        public SearchUserResponse GetProjectUsers(SearchUsersRequest request)
        {
            return mgr.SearchUsers(request);
        }

        #endregion
    }
}
