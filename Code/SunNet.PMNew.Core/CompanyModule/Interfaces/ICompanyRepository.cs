using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.CompanyModel;

namespace SunNet.PMNew.Core.CompanyModule
{
    public interface ICompanyRepository : IRepository<CompanysEntity>
    {
        SearchCompaniesResponse SearchCompanies(SearchCompaniesRequest request);
        bool ExistsCompanyName(string name, int exceptThis);
        List<CompanysEntity> GetCompaniesHasUser();
        List<CompanysEntity> GetCompaniesHasProject();

        Dictionary<int, CompanyProjectModel> GetCompanyProjectModels(int companyId, int projectId);
    }
}
