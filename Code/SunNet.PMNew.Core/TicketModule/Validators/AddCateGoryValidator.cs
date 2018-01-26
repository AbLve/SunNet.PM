using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.Core.TicketModule
{
    public class AddCateGoryValidator : BaseValidator<CateGoryEntity>
    {
        protected override void ValidateExtraRules(CateGoryEntity o)
        {
        }
    }
}
