using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.UserModule
{
    public class UpdateModuleValidator : BaseValidator<ModulesEntity>
    {
        protected override void ValidateExtraRules(ModulesEntity o)
        {
            if (o.ID <= 0)
            {
                this.AddBrokenRuleMessage("Primary key", "Primary key must greater than 0");
            }
        }
    }
}
