using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.InvoiceModel.Enums;
using System.Web.UI.HtmlControls;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Entity.TicketModel;
using System.Data;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.PM2014.Invoice
{
    public partial class AllInvoices : BasePage
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
            
              
            if (!IsPostBack)
            {
                ddlCompanyBind();
                FillInvoiceSearch();
                RepDataBind();
                //string method = Request.Form["method"];
                //if (method == "TimeSheetId")
                //{
                //    downloads(int.Parse(Request.Form["timeSheetID"]));
                //}
            }
        }



        public void FillInvoiceSearch()
        {
            txtKeyword.Text = QS("keyword");
            ddlCompany.SelectedValue = QS("Company");
            ddlStatus.SelectedValue = QS("Status");
        }
        public void RepDataBind()
        {
            SearchInvoiceRequest request = new SearchInvoiceRequest();
            request.OrderExpression = OrderBy;
            request.OrderDirection = OrderDirection;
            request.Keywords = txtKeyword.Text;
            request.Searchtype = InvoiceSearchType.All;
            request.CompanyId = ddlCompany.SelectedValue == "" ? 0 : int.Parse(ddlCompany.SelectedValue);
            request.InvoiceStatus = ddlStatus.SelectedValue == "" ? 0 : (InvoiceStatus)Enum.Parse(typeof(InvoiceStatus), ddlStatus.SelectedValue.Trim());
            SearchInvoiceResponse response = iapp.SearchInvoices(request);
            iModle = response.ResultList;
            rptTicketsList.DataSource = response.ResultList;
            rptTicketsList.DataBind();
            
            if (response.ResultCount == 0)
            {
                trNoTickets.Visible = true;
            }
            else
            {
                trNoTickets.Visible = false;
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

        protected void rptTicketsList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                 ProposalInvoiceModel record = (ProposalInvoiceModel)e.Item.DataItem;
                HtmlAnchor  lnkEdit = e.Item.FindControl("lnkEdit") as HtmlAnchor;
                if (lnkEdit != null)
                {
                    if (record.ProposalId == 0)
                    {
                        lnkEdit.HRef = "TM/New.aspx?id=" + record.ID;
                        lnkEdit.Visible = true;
                    }
                    else {
                        lnkEdit.HRef = "Proposal/DetailInvoice.aspx?proposalId=" + record.ProposalId + "&returnUrl=/Invoice/AllInvoices.aspx";
                        lnkEdit.Attributes.Remove("data-target");
                        lnkEdit.Visible = true;
                    }
                    if (record.Status.ToString() == "Waive")
                    {
                        lnkEdit.Visible = false;
                    }
                }
        

            }
        }

        protected void download_Click1(object sender, EventArgs e)
        {
            List<TimeSheetTicket> list =new List<TimeSheetTicket>();
            int invoiceID = int.Parse(hidtsID.Value);
            InvoiceEntity invoice = iapp.GetInvoice(invoiceID);
            if (invoice.ProposalId > 0)
            {
                list = tsApp.GetTimesheetByProposalId(invoice.ProposalId);
            }
            else
            {
                list = tsApp.GetTimesheet(invoiceID);
            }
            ExcelReport report = new ExcelReport();
            report.ExportInvoice(list);
        } 
    }
}