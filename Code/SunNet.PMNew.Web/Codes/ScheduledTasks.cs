using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;

namespace SunNet.PMNew.Web.Codes
{
    public static class ScheduledTasks
    {
        public static void UpdateProjectFreeHour()
        {
            ProjectApplication projApp = new ProjectApplication();
            projApp.UpdateProjectFreeHour();
        }
    }
}
