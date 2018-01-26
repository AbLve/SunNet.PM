using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.CompanyModel
{
    public class CompanyProjectModel
    {
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public List<ProjectSelectModel> Projects { get; set; }
    }

    public class ProjectSelectModel
    {
        public int ProjectId { get; set; }

        public string ProjectTitle { get; set; }
    }
}
