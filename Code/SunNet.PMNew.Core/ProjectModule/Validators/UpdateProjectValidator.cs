using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Entity.ProjectModel;

namespace SunNet.PMNew.Core.ProjectModule
{
    public class UpdateProjectValidator : BaseValidator<ProjectsEntity>
    {
        protected override void ValidateExtraRules(ProjectsEntity o)
        {
        }
    }
}
