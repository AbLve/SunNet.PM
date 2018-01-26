using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.PM2014.SunnetTicket
{
    public partial class MyCategory : TicketPageHelper
    {
        protected override string DefaultOrderBy
        {
            get
            {
                return "ModifiedOn";
            }
        }
        protected override string DefaultDirection
        {
            get { return "Desc"; }
        }
        private CateGoryApplication cgApp = new CateGoryApplication();
        private TicketsApplication tickApp = new TicketsApplication();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cgApp.GetCateGroyListByUserID(UserInfo.ID)
                    .BindDropdown<CateGoryEntity>(ddlCategories, "Title", "GID", DefaulSelectText, "", QS("category"));
                if (ddlCategories.Items.Count > 1 && string.IsNullOrEmpty(ddlCategories.SelectedValue))
                    ddlCategories.SelectedIndex = 1;
                CurrentModel = GetEntity();
                if (CurrentModel != null)
                {
                    InitControl();
                }
                else
                {
                    phlDelete.Visible = false;
                    phlEmpty.Visible = false;

                    CurrentModel = new CateGoryEntity();
                }
            }
            else
            {
                CurrentModel = GetEntity();
            }
        }

        private void SetControlsStatus(CateGoryEntity category)
        {
            if (category == null || category.IsProtected)
            {
                phlDelete.Visible = false;
            }
        }

        protected CateGoryEntity CurrentModel { get; set; }

        private CateGoryEntity GetEntity()
        {
            int category = 0;
            int.TryParse(ddlCategories.SelectedValue, out category);
            if (category == 0)
                return null;
            return cgApp.GetCateGory(category);
        }

        private void InitControl()
        {
            CateGoryEntity model = CurrentModel;
            if (model == null || model.IsDelete == true)
                Response.Redirect(ErrorPageUrl);
            SetControlsStatus(model);

            SearchTicketsRequest request = new SearchTicketsRequest(SearchTicketsType.CateGory,
                                                                    " OrderNum ASC ,Priority DESC,TicketTitle ASC ", false);
            request.OrderBy = string.Format("{0} {1}", OrderBy, OrderDirection);
            request.SheetDate = request.SheetDate = ObjectFactory.GetInstance<ISystemDateTime>().Now.Date;
            request.CateGoryID = model.GID;
            request.CurrentPage = CurrentPageIndex;

            SearchTicketsResponse response = tickApp.SearchTickets(request);
            if (response.ResultList != null && response.ResultList.Count > 0)
            {
                rptTicketsList.DataSource = response.ResultList;
                rptTicketsList.DataBind();
                trNoTickets.Visible = false;
            }
            else
            {
                trNoTickets.Visible = true;
                rptTicketsList.Visible = false;
            }
        }

        protected void btnEmpty_Click(object sender, EventArgs e)
        {
            CateGoryEntity model = GetEntity();
            if (model == null || model.IsDelete == true)
                Response.Redirect(ErrorPageUrl);
            if (cgApp.RemoveTicketFromCateGory(0, model.GID))
            {
                Redirect(Request.RawUrl, true);
            }
            else
            {
                ShowFailMessageToClient(cgApp.BrokenRuleMessages);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            CateGoryEntity model = GetEntity();
            if (model == null || model.IsDelete == true)
                Response.Redirect(ErrorPageUrl);
            if (model.IsProtected || cgApp.DeleteCateGroy(model.GID))
            {
                Redirect(Request.RawUrl, true);
            }
            else
            {
                ShowFailMessageToClient(cgApp.BrokenRuleMessages);
            }
        }
    }
}