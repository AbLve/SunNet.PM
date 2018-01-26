using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.Core.TicketModule
{
    public class AddTicketUserValidator : BaseValidator<TicketUsersEntity>
    {
        protected override void ValidateExtraRules(TicketUsersEntity te)
        {
        }
    }
}
