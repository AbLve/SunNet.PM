using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Entity.KPIModel;

namespace SunNet.PMNew.Core.KPIModule.Validators
{
    public class UpdateCategoryValidator : BaseValidator<KPICategoriesEntity>
    {
        protected override void ValidateExtraRules(KPICategoriesEntity o)
        {
            if (o.ID <= 0)
            {
                this.AddBrokenRuleMessage("Primary key", "Primary key must greater than 0");
            }
        }
    }
}
