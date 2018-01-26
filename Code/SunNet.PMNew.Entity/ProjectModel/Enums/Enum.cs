using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ProjectModel
{
    public enum ProjectStatus
    {
        Open = 1,
        Scheduled = 2,
        InProcess = 3,
        Completed = 4,
        Cancelled = 5,
        Other = 6     
    }
}
