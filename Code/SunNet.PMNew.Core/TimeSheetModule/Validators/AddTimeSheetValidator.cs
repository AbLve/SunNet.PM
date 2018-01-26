using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Framework.Core.Validator;

namespace SunNet.PMNew.Core.TimeSheetModule
{
    public class AddTimeSheetValidator : BaseValidator<TimeSheetsEntity>
    {
        protected override void ValidateExtraRules(TimeSheetsEntity o)
        {
        }
    }
}
