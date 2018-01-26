using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.LogModel;
using SunNet.PMNew.Framework.Core.Validator;

namespace SunNet.PMNew.Core.Log.Validators
{
    public class AddLogValidator : BaseValidator<LogEntity>
    {
        protected override void ValidateExtraRules(LogEntity o)
        {
           
        }
    }
}
