using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.UserModule
{
    public class UpdateUserValidator : BaseValidator<UsersEntity>
    {
        protected override void ValidateExtraRules(UsersEntity o)
        {

        }
    }
}
