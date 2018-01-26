using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Entity.KPIModel;

namespace SunNet.PMNew.Core.KPIModule.Validators
{
    class AddCategoryValidator : BaseValidator<KPICategoriesEntity>
    {
        protected override void ValidateExtraRules(KPICategoriesEntity o)
        {

        }
    }
}
