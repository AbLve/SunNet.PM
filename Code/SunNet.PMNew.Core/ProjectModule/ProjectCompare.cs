using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.ProjectModel;

namespace SunNet.PMNew.Core.ProjectModule
{
    internal class ProjectCompare : IEqualityComparer<ProjectDetailDTO>
    {
        #region IComparer<ProjectDetailDTO> Members
        public int GetHashCode(ProjectDetailDTO obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;
            int hashProductName = obj.ID == 0 ? 0 : obj.ID.GetHashCode();
            int hashProductCode = obj.ID.GetHashCode();
            return hashProductName ^ hashProductCode;
        }

        public bool Equals(ProjectDetailDTO x, ProjectDetailDTO y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            return x.ID == y.ID;
        }

        #endregion
    }
}
