using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Entity.InvoiceModel.Enums;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Helpers;
using System.Web.UI.HtmlControls;

namespace SunNet.PMNew.PM2014.Invoice
{
    public partial class POList : BasePage
    {
        TimeSheetApplication tsApp = new TimeSheetApplication();
        InvoicesApplication iapp = new InvoicesApplication();
        CompanyApplication companyApp = new CompanyApplication();
        public List<ProposalInvoiceModel> iModle = new List<ProposalInvoiceModel>();

        protected override string DefaultOrderBy
        {
            get
            {
                return "CompanyName";
            }
        }

        protected override string DefaultDirection
        {
            get { return "DESC"; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlCompanyBind();
            FillInvoiceSearch();
            RepDataBind();
        }
        public void FillInvoiceSearch()
        {
            txtKeyword.Text = QS("keyword");
            ddlCompany.SelectedValue = QS("Company");
            ddlStatus.SelectedValue = QS("Status");
            txtApproveOn.Text = QS("approveOn");
        }
        public void RepDataBind()
        {

            SearchInvoiceRequest request = new SearchInvoiceRequest();
            request.OrderExpression = OrderBy;
            request.OrderDirection = OrderDirection;
            request.Keywords = txtKeyword.Text;
            request.Searchtype = InvoiceSearchType.All;
            request.CompanyId = ddlCompany.SelectedValue == "" ? 0 : int.Parse(ddlCompany.SelectedValue);
            request.ApproveOn = txtApproveOn.Text;
            request.InvoiceStatus = ddlStatus.SelectedValue == "" ? 0 : (InvoiceStatus)Enum.Parse(typeof(InvoiceStatus), ddlStatus.SelectedValue.Trim());
            SearchInvoiceResponse response = iapp.SearchPOlist(request);
            iModle = response.ResultList;
            rptTicketsList.DataSource = response.POList;
            rptTicketsList.DataBind();
            if (response.POListCount == 0)
            {
                trNoTickets.Visible = true;
            }
            else
            {
                trNoTickets.Visible = false;
            }
            MergeCell("CompanyNames", "PONos");
        }

        private void MergeCell(string CompanyNames, string PONos)
        {
            for (int i = rptTicketsList.Items.Count - 1; i > 0; i--)
            {
                MergeCellSet(CompanyNames, PONos, i);
            }
        }
        private void MergeCellSet(string CompanyNames, string PONos, int i)
        {
            HtmlTableCell cellPrev = rptTicketsList.Items[i - 1].FindControl(CompanyNames) as HtmlTableCell;
            HtmlTableCell cell = rptTicketsList.Items[i].FindControl(CompanyNames) as HtmlTableCell;
            cell.RowSpan = (cell.RowSpan == -1) ? 1 : cell.RowSpan;
            cellPrev.RowSpan = (cellPrev.RowSpan == -1) ? 1 : cellPrev.RowSpan;
            if (cell.InnerText == cellPrev.InnerText)
            {
                cell.Visible = false;
                cellPrev.RowSpan += cell.RowSpan;

                //关键代码，再判断执行第2列的合并单元格方法
                if (PONos != "") MergeCellSet(PONos, "", i);
            }
        }         
        public void ddlCompanyBind()
        {
            List<CompanysEntity> list = companyApp.GetAllCompanies();
            list.BindDropdown<CompanysEntity>(ddlCompany, "CompanyName", "ComID", "All", "", QS("Company"));
            ddlStatus.DataSource = InvoiceStatus.Awaiting_Payment.ToSelectList().ToList();
            ddlStatus.DataValueField = "Value";
            ddlStatus.DataTextField = "Text";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("All", ""));
        }
    }
}