using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework.Core.BrokenMessage;

namespace SunNet.PMNew.Core.UserModule
{
    public interface ICompanyCore
    {
        List<BrokenRuleMessage> BrokenRuleMessages { get; }
    }
}
