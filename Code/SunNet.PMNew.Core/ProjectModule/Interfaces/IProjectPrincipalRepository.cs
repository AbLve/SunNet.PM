﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.ProjectModule.Interfaces
{
    public interface IProjectPrincipalRepository : IRepository<ProjectPrincipalEntity>
    {
        List<ProjectPrincipalEntity> GetProjectPrincipal(int projectId);

    }
}
