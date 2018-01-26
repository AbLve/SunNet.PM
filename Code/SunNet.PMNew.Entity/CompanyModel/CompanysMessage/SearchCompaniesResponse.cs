using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.CompanyModel
{
    public class SearchCompaniesResponse
    {
        public List<CompanysEntity> ResultList { get; set; }
        public int ResultCount { get; set; }
    }
}
