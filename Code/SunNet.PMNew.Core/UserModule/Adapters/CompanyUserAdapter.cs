using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.CompanyModule;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.UserModule
{
    public class CompanyUserAdapter : ICompanyUser
    {
        #region ICompanyUser Members
        UserManager mgr;
        public CompanyUserAdapter()
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
        public List<SunNet.PMNew.Framework.Core.BrokenMessage.BrokenRuleMessage> BrokenRuleMessages
        {
            get;
            set;
        }

        public SunNet.PMNew.Entity.UserModel.SearchUserResponse SearchUsers(SearchUsersRequest request)
        {
            mgr.ClearBrokenRuleMessages();
            SearchUserResponse response = mgr.SearchUsers(request);
            if (mgr.BrokenRuleMessages.Count > 0)
            {
                this.BrokenRuleMessages = mgr.BrokenRuleMessages;
            }
            return response;
        }

        #endregion
    }
}
