using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.UserModel
{
    public enum SearchUsersType
    {
        /// <summary>
        /// Users of a single Role,need a Role
        /// </summary>
        Role,
        /// <summary>
        /// All Users
        /// </summary>
        All,
        /// <summary>
        /// List for Search
        /// </summary>
        List,
        /// <summary>
        /// Users of Projects,need a ProjectID
        /// </summary>
        Project,
        /// <summary>
        /// Company's Users,need a CompanyID
        /// </summary>
        Company,
        /// <summary>
        /// Company's Users,need a ProjectID
        /// </summary>
        CompanyByProject,
        /// <summary>
        /// Users of Tickets,need a TicketID
        /// </summary>
        Ticket,
        /// <summary>
        /// SingleInstance,need a UserID
        /// </summary>
        SingleInstance
    }
    public class SearchUsersRequest
    {
        public SearchUsersType SearchType { get; set; }
        public SearchUsersRequest(SearchUsersType searchtype, bool isPageMode, string orderby, string orderdirection)
        {
            this.SearchType = searchtype;
            this.IsPageModel = isPageMode;
            this.OrderExpression = orderby;
            this.OrderDirection = orderdirection;
        }
        public int UserID { get; set; }

        public int ProjectID { get; set; }
        public int CompanyID { get; set; }
        public int TicketID { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }

        public RolesEnum Role { get; set; }

        public bool IsClient { get; set; }
        public bool IsSunnet { get; set; }

        public bool IsPageModel { get; set; }

        public string UserName { get; set; }
        public string Keywords { get; set; }
        /// <summary>
        /// Status ="ALL" | "ACTIVE" | "INACTIVE"
        /// </summary>
        public string Status { get; set; }
        public string OrderExpression { get; set; }
        public string OrderDirection { get; set; }

    }
}
