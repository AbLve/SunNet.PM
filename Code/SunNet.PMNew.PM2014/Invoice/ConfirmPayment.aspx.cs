using SunNet.PMNew.App;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Impl.SqlDataProvider.Company;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.Invoice
{
    public partial class ConfirmPayment : BasePage
    {
        InvoicesApplication iapp = new InvoicesApplication();
        TimeSheetApplication tsApp = new TimeSheetApplication();
        ProjectApplication proApp = new ProjectApplication();

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
                BindProject();
                FillInvoiceSearch(); 
                RepDataBind();
            }
        }

        public void FillInvoiceSearch()
        {
            txtKeyword.Text = QS("keyword");
            ddlProject.SelectedValue = QS("project");
        }
        public void RepDataBind()
        {
            ProposalInvoiceModel model = new ProposalInvoiceModel();
            SearchInvoiceRequest request = new SearchInvoiceRequest();
            request.OrderExpression = OrderBy;
            request.OrderDirection = OrderDirection;
            request.Keywords = txtKeyword.Text;
            request.Searchtype = InvoiceSearchType.Payment_Received;
            request.ProjectId = ddlProject.SelectedValue == "" ? 0 : int.Parse(ddlProject.SelectedValue);
            SearchInvoiceResponse response = iapp.SearchInvoices(request);
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

        private void BindProject()
        {
            CompanysRepositorySqlDataProvider crsp = new CompanysRepositorySqlDataProvider();
            int cid = crsp.GetCompanyId("Sunnet");
            IList<ProjectDetailDTO> listProject = proApp.GetAllProjects();
            listProject = listProject.Where(c => c.CompanyID != cid).ToList();

            this.ddlProject.DataSource = listProject;
            this.ddlProject.DataBind((ProjectDetailDTO project, string status) => project.Status.ToString() == status);
            this.ddlProject.SelectItem("0");
        }

        protected void rptTicketsList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    ProposalInvoiceModel record = (ProposalInvoiceModel)e.Item.DataItem;
            //    HtmlAnchor lnkEdit = e.Item.FindControl("lnkEdit") as HtmlAnchor;
            //    if (lnkEdit != null)
            //    {
            //        if (record.ProposalTitle == "")
            //        {
            //            lnkEdit.HRef = "TM/New.aspx?id=" + record.ID;
            //            lnkEdit.Visible = true;
            //        }
            //        else
            //        {
            //            lnkEdit.HRef = "Proposal/DetailInvoice.aspx?proposalId=" + record.ProposalId + "&returnUrl=/Invoice/AwaingPayment.aspx";
            //            lnkEdit.Attributes.Remove("data-target");
            //            lnkEdit.Visible = true;
            //        }
            //        if (record.Status.ToString() == "Waive")
            //        {
            //            lnkEdit.Visible = false;
            //        }
            //    }


            //}
        }
        protected void download_Click1(object sender, EventArgs e)
        {
            List<TimeSheetTicket> list = new List<TimeSheetTicket>();
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