using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Core.CompanyModule;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Core.UserModule;

namespace SunNet.PMNew.App
{
    public class CompanyApplication : BaseApp
    {
        private CompanyManager mgr;

        public CompanyApplication()
        {
            mgr = new CompanyManager(
                                    ObjectFactory.GetInstance<IEmailSender>(),
                                    ObjectFactory.GetInstance<ICompanyRepository>(),
                                    ObjectFactory.GetInstance<ICache<CompanyManager>>());
            mgr.CompanyUserAdapter = ObjectFactory.GetInstance<CompanyUserAdapter>();
        }


        public int AddCompany(CompanysEntity model)
        {
            this.ClearBrokenRuleMessages();
            int id = mgr.AddCompany(model);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="oldCompanyName">check new company name if changed,else sent null</param>
        /// <returns></returns>
        public bool UpdateCompany(CompanysEntity model)
        {
            this.ClearBrokenRuleMessages();
            bool result = mgr.UpdateCompany(model);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }

        public CompanysEntity GetCompany(int id)
        {
            this.ClearBrokenRuleMessages();
            CompanysEntity model = mgr.GetCompany(id);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return model;
        }

        public SearchCompaniesResponse SearchCompanies(SearchCompaniesRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchCompaniesResponse response = mgr.SearchCompanies(request);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return response;
        }

        public List<CompanysEntity> GetAllCompanies()
        {
            this.ClearBrokenRuleMessages();
            List<CompanysEntity> list = mgr.GetAllCompanies();
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }

        public SearchUserResponse SearchUsers(SearchUsersRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchUserResponse response = mgr.SearchUsers(request);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return response;
        }

        public List<CompanysEntity> GetCompaniesHasUser()
        {
            return mgr.GetCompaniesHasUser();
        }
        public List<CompanysEntity> GetCompaniesHasProject()
        {
            return mgr.GetCompaniesHasProject();
        }

        public Dictionary<int, CompanyProjectModel> GetCompanyProjectModels(int companyId, int projectId)
        {
            return mgr.GetCompanyProjectModels(companyId, projectId);
        }
    }
}
