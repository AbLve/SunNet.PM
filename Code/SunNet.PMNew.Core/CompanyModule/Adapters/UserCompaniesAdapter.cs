using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.UserModule;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.CompanyModel;
using StructureMap;
using SunNet.PMNew.Framework.Core.BrokenMessage;

namespace SunNet.PMNew.Core.CompanyModule.Adapters
{
    public class UserCompaniesAdapter : ICompanyCore
    {
        private CompanyManager mgr;
        public UserCompaniesAdapter()
        {
            mgr = new CompanyManager(
                                    ObjectFactory.GetInstance<IEmailSender>(),
                                    ObjectFactory.GetInstance<ICompanyRepository>(),
                                    ObjectFactory.GetInstance<ICache<CompanyManager>>());
        }
        #region ISearchCompany Members

        #endregion

        #region ICompanyCore Members

        public List<BrokenRuleMessage> BrokenRuleMessages
        {
            get
            {
                return mgr.BrokenRuleMessages;
            }
        }

        #endregion
    }
}
