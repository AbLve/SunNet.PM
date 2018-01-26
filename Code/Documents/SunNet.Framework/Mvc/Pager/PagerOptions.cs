
namespace SF.Framework.Mvc.Pager
{
    public class PagerOptions
    {
        public PagerOptions()
        {
            AutoHide = true;
            PageIndexParameterName = "pageIndex";
            NumericPagerItemCount = 10;
            AlwaysShowFirstLastPageNumber = false;
            ShowPrevNext = true;
            PrevPageText = "Previous page";
            NextPageText = "Next page";
            ShowNumericPagerItems = true;
            ShowFirstLast = true;
            FirstPageText = "Home page";
            LastPageText = "End page";
            ShowMorePagerItems = true;
            MorePageText = "...";
            ShowDisabledPagerItems = true;
            SeparatorHtml = "&nbsp;&nbsp;";
            UseJqueryAjax = false;
            ShowPageIndexBox = false;
            ShowGoButton = true;
            PageIndexBoxType = PageIndexBoxType.TextBox;
            MaximumPageIndexItems = 80;
            GoButtonText = "GO";
            ContainerTagName = "div";
            InvalidPageIndexErrorMessage = "Page index is invalid";
            PageIndexOutOfRangeErrorMessage = "Page index beyond ran";
            MaxPageIndex = 0;
        }

        /// <summary>
        /// When the total number of pages only one page is hidden automatically
        /// </summary>
        public bool AutoHide { get; set; }

        /// <summary>
        /// Page index beyond range shows error messages
        /// </summary>
        public string PageIndexOutOfRangeErrorMessage { get; set; }

        /// <summary>
        /// Invalid index page show when error messages
        /// </summary>
        public string InvalidPageIndexErrorMessage { get; set; }

        /// <summary>
        /// In the url of the page index parameter name
        /// </summary>
        public string PageIndexParameterName { get; set; }

        /// <summary>
        /// Whether display page index input a select box
        /// </summary>
        public bool ShowPageIndexBox { get; set; }

        /// <summary>
        /// Page index input or select box type
        /// </summary>
        public PageIndexBoxType PageIndexBoxType { get; set; }

        /// <summary>
        /// Page index drop-down box shows the biggest page index number of branches, 
        /// this property only when PageIndexBoxType set to PageIndexBoxType.DropDownList effectively
        /// </summary>
        public int MaximumPageIndexItems { get; set; }

        /// <summary>
        /// Whether display jump button
        /// </summary>
        public bool ShowGoButton { get; set; }

        /// <summary>
        /// Jump button on the text
        /// </summary>
        public string GoButtonText { get; set; }

        /// <summary>
        /// Digital page index format string
        /// </summary>
        public string PageNumberFormatString { get; set; }
        /// <summary>
        /// Current page index format string
        /// </summary>
        public string CurrentPageNumberFormatString { get; set; }

        private string _containerTagName;
        /// <summary>
        /// Paging controls HTML container tag name, the default is div
        /// </summary>
        public string ContainerTagName
        {
            get
            {
                return _containerTagName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new System.ArgumentException("ContainerTagName can't is null or empty string", "ContainerTagName");
                _containerTagName = value;
            }
        }

        /// <summary>
        /// Containing digital page, current page and the top, bottom, before and after page elements of HTML text format string
        /// </summary>
        public string PagerItemWrapperFormatString { get; set; }

        /// <summary>
        /// Containing digital page index page elements of HTML text format string
        /// </summary>
        public string NumericPagerItemWrapperFormatString { get; set; }

        /// <summary>
        /// Containing current page paging elements of HTML text format string
        /// </summary>
        public string CurrentPagerItemWrapperFormatString { get; set; }

        /// <summary>
        /// Tolerance on page, pagedown, homepage and tail first four page elements of HTML text format string
        /// </summary>
        public string NavigationPagerItemWrapperFormatString { get; set; }

        /// <summary>
        /// Include more page paging elements of HTML text format string
        /// </summary>
        public string MorePagerItemWrapperFormatString { get; set; }

        /// <summary>
        /// Inclusive pages index input or select box HTML text format string
        /// </summary>
        public string PageIndexBoxWrapperFormatString { get; set; }

        /// <summary>
        /// Inclusive pages index frame and jump button area HTML text box string
        /// </summary>
        public string GoToPageSectionWrapperFormatString { get; set; }

        /// <summary>
        /// whether or not show first and last numeric page number
        /// </summary>
        public bool AlwaysShowFirstLastPageNumber { get; set; }
        /// <summary>
        /// Display the biggest digital page index button number
        /// </summary>
        public int NumericPagerItemCount { get; set; }
        /// <summary>
        /// Whether display page and on the following page
        /// </summary>
        public bool ShowPrevNext { get; set; }
        /// <summary>
        /// Previous page text
        /// </summary>
        public string PrevPageText { get; set; }
        /// <summary>
        /// The next page text
        /// </summary>
        public string NextPageText { get; set; }
        /// <summary>
        /// Whether display digital page index button and more page button
        /// </summary>
        public bool ShowNumericPagerItems { get; set; }
        /// <summary>
        /// Whether display the first page and on the last page
        /// </summary>
        public bool ShowFirstLast { get; set; }
        /// <summary>
        /// The first page of text
        /// </summary>
        public string FirstPageText { get; set; }
        /// <summary>
        /// The last page text
        /// </summary>
        public string LastPageText { get; set; }
        /// <summary>
        /// Whether display more page button
        /// </summary>
        public bool ShowMorePagerItems { get; set; }
        /// <summary>
        /// More page button text
        /// </summary>
        public string MorePageText { get; set; }
        /// <summary>
        /// Contains paging controls div tags ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Horizontal alignment
        /// </summary>
        public string HorizontalAlign { get; set; }
        /// <summary>
        /// CSS style class
        /// </summary>
        public string CssClass { get; set; }
        /// <summary>
        /// whether or not show disabled navigation buttons
        /// </summary>
        public bool ShowDisabledPagerItems { get; set; }
        /// <summary>
        /// seperating item html
        /// </summary>
        public string SeparatorHtml { get; set; }

        /// <summary>
        /// The maximum limit display pages, the default value is 0, that is, based on the total record number out of the total number of pages
        /// </summary>
        public int MaxPageIndex { get; set; }
        /// <summary>
        /// Whether to use jQuery realize Ajax paging (internal)
        /// </summary>
        internal bool UseJqueryAjax { get; set; }
    }

    public enum PageIndexBoxType
    {
        TextBox,//Text box input
        DropDownList //Drop-down box choose
    }
}