using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ProjectModel
{
    public class SearchProjectsResponse
    {
        public List<ProjectDetailDTO> ResultList { get; set; }
        public int ResultCount { get; set; }
    }
}
