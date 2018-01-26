using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Framework.Core.Validator;

namespace SunNet.PMNew.Core.ProjectModule
{
    public class AddProjectUserValidator : BaseValidator<ProjectUsersEntity>
    {
        protected override void ValidateExtraRules(ProjectUsersEntity o)
        {
        }
    }
}
