using System;

/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		5/26 01:07:56
 * Description:		Please input class summary
 * Version History:	Created,5/26 01:07:56
 * 
 * 
 **************************************************************************/
namespace SunNet.PMNew.Entity.ShareModel.DTO
{
    public class SearchShareRequest
    {
        public SearchShareRequest(bool isPageModel, string orderby, string orderdire)
            : this(isPageModel, 1, 10, orderby, orderdire)
        {

        }
        public SearchShareRequest(bool isPageModel, int currentPage, int pageCount, string orderby, string orderdire)
        {
            this.IsPageModel = isPageModel;
            this.OrderExpression = orderby;
            this.OrderDirection = orderdire;
            this.CurrentPage = currentPage;
            this.PageCount = pageCount;

            this.StartDate = DateTime.MinValue;
            this.EndDate = DateTime.MinValue;
        }
        public bool IsPageModel { get; set; }
        public string OrderExpression { get; set; }
        public string OrderDirection { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }

        public int CreatedBy { get; set; }
        public string Keyword { get; set; }
        public int Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
