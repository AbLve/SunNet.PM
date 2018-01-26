using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ProjectModel
{
    public enum SearchProjectsType
    {
        /// <summary>
        /// All Projects,cache
        /// </summary>
        All,
        /// <summary>
        /// ListProject.aspx,need keyword
        /// </summary>
        List,
        /// <summary>
        /// Company's Projects,need a CompanyID
        /// </summary>
        Company,
        /// <summary>
        /// Only Instance,need a ProjectID
        /// </summary>
        SingleInstance,
        /// <summary>
        /// Ticket's Project,need a TicketID
        /// </summary>
        Ticket,
        /// <summary>
        /// Get Project List By UserID
        /// </summary>
        ListByUserID,

        /// <summary>
        /// All projects except current user have been assigned.
        /// </summary>
        AllExceptAssigned,

        /// <summary>
        ///  All projects of current user's Company except that have been assigned to current user.
        /// </summary>
        CompanyExceptAssigned

    }

    public class SearchProjectsRequest
    {
        public SearchProjectsRequest(SearchProjectsType type, bool isPageModel, string orderExpression, string orderDirection)
        {
            this.SearchType = type;
            this.IsPageModel = isPageModel;
            this.OrderDirection = orderDirection;
            this.OrderExpression = orderExpression;
        }
        public SearchProjectsType SearchType { get; set; }

        public int ProjectID { get; set; }
        public int CompanyID { get; set; }
        public int TicketID { get; set; }
        public int UserID { get; set; }

        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }

        public bool IsPageModel { get; set; }

        public string OrderExpression { get; set; }
        public string OrderDirection { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
    }
}
