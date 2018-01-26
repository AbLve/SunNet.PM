using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.FileModel
{
    public enum SearchFileType
    {
        Project,
        Company,
        TicketAndFeedback,
        SunnetFile,
        WorkRequest
    }
    public enum SearchKeywordType
    {
        All = 4,
        TicketCode = 0,
        TicketTitle = 1,
        FileName = 2,
        None = 3
    }
    public class SearchFilesRequest
    {
        public bool IsPageModel { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public string OrderByExpression { get; set; }
        public string OrderByDirection { get; set; }
        public SearchFileType SearchType { get; set; }

        public bool IncludeChildDirectory { get; set; }
        public int SearchID { get; set; }
        public int ParentID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int ProjectID { get; set; }
        public int TicketID { get; set; }
        public int FeedBackID { get; set; }
        public bool IsPublic { get; set; }
        public string Keyword { get; set; }
        public SearchKeywordType KeywordType { get; set; }
        public SearchFilesRequest(SearchFileType type, bool isPageModel, string order, string direction)
        {
            this.SearchType = type;
            this.IsPageModel = isPageModel;
            this.OrderByDirection = direction;
            this.OrderByExpression = order;
        }

    }
}
