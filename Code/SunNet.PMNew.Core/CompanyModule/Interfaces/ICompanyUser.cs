using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.CompanyModule
{
    public interface ICompanyUser
    {
        List<BrokenRuleMessage> BrokenRuleMessages { get; set; }
        SearchUserResponse SearchUsers(SearchUsersRequest request);
    }
}
