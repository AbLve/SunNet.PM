using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Core.Notify;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.CompanyModule
{
    public class CompanyManager : BaseMgr
    {
        private IEmailSender emailSender;
        private ICompanyRepository repository;

        private ICache<CompanyManager> cache;
        private const string CACHECOMPANIES_KEY = "Companines";

        public ICompanyUser CompanyUserAdapter
        {
            get;
            set;
        }
        public CompanyManager(IEmailSender emailSender, ICompanyRepository companyRespository, ICache<CompanyManager> cache)
        {
            this.emailSender = emailSender;
            this.repository = companyRespository;
            this.cache = cache;
        }
        public int AddCompany(CompanysEntity model)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<CompanysEntity> validator = new PostCompanyValidator();
            if (!validator.Validate(model))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return 0;
            }
            if (repository.ExistsCompanyName(model.CompanyName, 0))
            {
                this.AddBrokenRuleMessage("Error", "Company name existed!Please input a new one.");
                return 0;
            }
            int id = repository.Insert(model);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }
            model.ComID = id;
            AddToCached(model);

            return id;
        }
        public bool UpdateCompany(CompanysEntity model)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<CompanysEntity> validator = new UpdateCompanyValidator();
            if (!validator.Validate(model))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return false;
            }
            if (repository.ExistsCompanyName(model.CompanyName, model.ComID))
            {
                this.AddBrokenRuleMessage("Error", "Company name existed!Please input a new one.");
                return false;
            }
            bool result = repository.Update(model);
            if (!result)
            {
                this.AddBrokenRuleMessage();
            }
            if (result)
            {
                UpdateCached(model);
            }
            return result;
        }

        private List<CompanysEntity> CachedCompanies()
        {
            if (cache[CACHECOMPANIES_KEY] == null)
            {
                SearchCompaniesRequest request = new SearchCompaniesRequest();
                request.SearchType = SearchCompanyType.All;
                request.IsPageModel = false;
                request.OrderExpression = " CompanyName ";
                request.OrderDirection = " ASC ";
                SearchCompaniesResponse response = repository.SearchCompanies(request);
                if (response == null)
                {
                    this.AddBrokenRuleMessage();
                }
                else
                {
                    cache[CACHECOMPANIES_KEY] = response.ResultList;
                }
            }
            return cache[CACHECOMPANIES_KEY] as List<CompanysEntity>;
        }
        private void AddToCached(CompanysEntity model)
        {
            if (cache[CACHECOMPANIES_KEY] != null)
            {
                List<CompanysEntity> list = cache[CACHECOMPANIES_KEY] as List<CompanysEntity>;
                list.Add(model);
            }
        }
        private void UpdateCached(CompanysEntity model)
        {
            if (cache[CACHECOMPANIES_KEY] != null)
            {
                List<CompanysEntity> list = cache[CACHECOMPANIES_KEY] as List<CompanysEntity>;
                CompanysEntity findone = list.Find(com => com.ID == model.ID);
                if (findone != null)
                {
                    list.Remove(findone);
                }
                list.Add(model);
                cache[CACHECOMPANIES_KEY] = list;
            }
        }
        public CompanysEntity GetCompany(int id)
        {
            List<CompanysEntity> list = CachedCompanies();
            if (list != null && list.Count > 0)
            {
                CompanysEntity findone = list.Find(com => com.ID == id);
                if (findone != null)
                {
                    return findone;
                }
                else
                {
                    if (id < 0)
                        return null;
                    this.ClearBrokenRuleMessages();
                    CompanysEntity model = repository.Get(id);
                    if (model == null)
                        this.AddBrokenRuleMessage();
                    return model;
                }
            }
            return null;
        }


        public SearchCompaniesResponse SearchCompanies(SearchCompaniesRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchCompaniesResponse response = repository.SearchCompanies(request);
            if (response == null)
            {
                this.AddBrokenRuleMessage();
            }
            return response;
        }
        public List<CompanysEntity> GetAllCompanies()
        {
            return CachedCompanies();
        }
        public SearchUserResponse SearchUsers(SearchUsersRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchUserResponse response = CompanyUserAdapter.SearchUsers(request);
            if (response == null)
            {
                this.AddBrokenRuleMessage();
            }
            return response;
        }

        public List<CompanysEntity> GetCompaniesHasUser()
        {
            return repository.GetCompaniesHasUser();
        }

        public List<CompanysEntity> GetCompaniesHasProject()
        {
            return repository.GetCompaniesHasProject();
        }

        public Dictionary<int, CompanyProjectModel> GetCompanyProjectModels(int companyId, int projectId)
        {
            return repository.GetCompanyProjectModels(companyId, projectId);
        }
    }
}
