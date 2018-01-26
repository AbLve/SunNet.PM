using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Core.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Core.UserModule
{
    public class UpdateHideUserValidator : BaseValidator<HideUserEntity>
    {
        protected override void ValidateExtraRules(HideUserEntity o)
        {
            if (o.ID <= 0)
            {
                this.AddBrokenRuleMessage("Primary key", "Primary key must greater than 0");
            }
        }
    }
}
