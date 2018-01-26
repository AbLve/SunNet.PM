using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Helpers;
namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class MyCategory : BaseWebsitePage
    {
        CateGoryApplication cgApp;
        TicketsApplication tickApp;
        TicketsRelationApplication trApp;
        protected int CateGoryID
        {
            get
            {
                int category = QS("id", 0);
                return category;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            cgApp = new CateGoryApplication();
            tickApp = new TicketsApplication();
            trApp = new TicketsRelationApplication();
            if (!IsPostBack)
            {
                InitControl();
            }

        }
        protected string GetRelatedTickets(object id, object projectID)
        {
            return trApp.GetAllRelationStringById(int.Parse(id.ToString()), false);
        }

        private void InitControl()
        {
            int category = QS("id", 0);
            if (category == 0)
            {
                ShowArgumentErrorMessageToClient();
                return;
            }
            CateGoryEntity model = cgApp.GetCateGory(category);
            if (model == null || model.IsDelete == true)
            {
                ShowArgumentErrorMessageToClient();
                Response.Redirect("/error.html?sourceurl=" + Request.Url.ToString());
            }
            SearchTicketsRequest request = new SearchTicketsRequest(SearchTicketsType.CateGory,
                                                                    " OrderNum ASC ,Priority DESC,TicketTitle ASC ",
                                                                    true);
            request.SheetDate = request.SheetDate = ObjectFactory.GetInstance<ISystemDateTime>().Now.Date;
            request.CateGoryID = category;
            request.CurrentPage = anpTickets.CurrentPageIndex;

            SearchTicketsResponse response = tickApp.SearchTickets(request);
            rptTickets.DataSource = response.ResultList;
            rptTickets.DataBind(); 
            anpTickets.RecordCount = response.ResultCount;
        }
        protected void anpTickets_PageChanged(object sender, EventArgs e)
        {
            InitControl();
        }
    }
}
