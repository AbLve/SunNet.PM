using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.CompanyModel
{
    public enum SearchCompanyType
    {
        /// <summary>
        /// GetAll Company
        /// </summary>
        All,
        /// <summary>
        /// Get a List,need a companyName
        /// </summary>
        List,
        /// <summary>
        /// Get Company for a project,need a projectID;
        /// </summary>
        Project,
        /// <summary>
        /// Get Company for a user,need a UserID;
        /// </summary>
        User,
        /// <summary>
        /// Get Company from a ComID,need a ComID;
        /// </summary>
        SingleCompany
    }
    public class SearchCompaniesRequest
    {
        public SearchCompanyType SearchType { get; set; }

        public int UserID { get; set; }
        public int ProjectID { get; set; }
        public int ComID { get; set; }

        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }

        public bool IsPageModel { get; set; }

        public string OrderExpression { get; set; }
        public string OrderDirection { get; set; }
        public string CompanyName { get; set; }
    }
}
