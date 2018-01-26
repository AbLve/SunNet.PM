using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ComplaintModel
{
    public class ComplaintSearchEntity
    {

        public ComplaintSearchEntity(bool isPageMode, string orderby, string orderdirection)
        {
            this.IsPageModel = isPageMode;
            this.OrderExpression = orderby;
            this.OrderDirection = orderdirection;
        }
        
        public int ComplaintID { get; set; }
        //public DateTime ReportTime { get; set; }
        public string Reporter { get; set; }

        public int Type { get; set; }
        public int Reason { get; set; }
        public int SystemID { get; set; }
        public int AppSrc { get; set; }
        public int Status { get; set; }
        public string Keyword { get; set; }
        public string UpdatedByName { get; set; }

        public string OrderExpression { get; set; }
        public string OrderDirection { get; set; }

        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public bool IsPageModel { get; set; }

    }
}
