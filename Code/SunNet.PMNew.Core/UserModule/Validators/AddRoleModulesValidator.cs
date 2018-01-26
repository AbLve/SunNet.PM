using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.UserModule
{
    public class AddRoleModulesValidator : BaseValidator<RoleModulesEntity>
    {
        protected override void ValidateExtraRules(RoleModulesEntity o)
        {
            if (o.RoleID <= 0 || o.ModuleID <= 0)
            {
                this.AddBrokenRuleMessage("Arguments Error", "RoleID or ModuleID can not be empty.");
            }
        }
    }
}
