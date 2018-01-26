using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Entity.CompanyModel;

namespace SunNet.PMNew.Core.CompanyModule
{
    public class PostCompanyValidator : BaseValidator<CompanysEntity>
    {
        protected override void ValidateExtraRules(CompanysEntity o)
        {
        }
    }
}
