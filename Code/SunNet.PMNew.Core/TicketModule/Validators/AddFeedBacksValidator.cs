using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core.Validator;

namespace SunNet.PMNew.Core.TicketModule
{
    public class AddFeedBacksValidator : BaseValidator<FeedBacksEntity>
    {
        protected override void ValidateExtraRules(FeedBacksEntity o)
        {
        }
    }
}
