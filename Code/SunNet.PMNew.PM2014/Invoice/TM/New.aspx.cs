using System.Net;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Entity.InvoiceModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Invoice.TM
{
    public partial class New : BasePage
    {
        InvoicesApplication iapp = new InvoicesApplication();
        public string timeTsheetIDs = "";
        TimeSheetApplication tsApp = new TimeSheetApplication();
        protected void Page_Load(object sender, EventArgs e)
        {
            ((Pop)(this.Master)).Width = 480;
            if (!IsPostBack)
            {
                StatusBind();
                if (QS("id", 0) != 0)
                {
                    int invoiceID = QS("id", 0);
                    ViewBind(invoiceID);
                }

                if (HttpContext.Current.Request.Cookies["timeTsheetIDs"] != null)
                {
                    timeTsheetIDs = HttpContext.Current.Request.Cookies["timeTsheetIDs"].Value;
                    timeTsheetIDs = System.Web.HttpUtility.UrlDecode(timeTsheetIDs);
                }
                if (timeTsheetIDs != "" && QS("id", 0) == 0)
                {
                    TimeTsheetBind(timeTsheetIDs);
                    lblTimeTsheetIDs.Text = timeTsheetIDs;
                }
            }
        }
        public void TimeTsheetBind(string timeTsheetIDs)
        {
            string projects = QS("projectid");
            SearchInvoiceRequest request = new SearchInvoiceRequest();
            request.ProjectIds = projects;
            request.timeTsheetIDs = timeTsheetIDs;
            SearchInvoiceResponse response = iapp.SearchTimesheetInvoice(request);
            lblCompany.Text = response.TimesheetList[0].CompanyName;
            decimal numHours = 0;
            foreach (var item in response.TimesheetList)
            {
                numHours += item.Hours;
            }
            lblHours.Text = numHours.ToString();
        }
        public void ViewBind(int id)
        {
            this.lbltitle.Text = "Edit";
            this.btnsave.Text = "Submit";
            ProposalInvoiceModel model = iapp.GetInvoiceModelById(id);
            this.lblCompany.Text = model.CompanyName;
            this.lblHours.Text = model.HOURS.ToString();
            this.lblStatus.Text = model.Status.ToText();
            this.txtInvoice.Text = model.InvoiceNo;
            this.txtSendDate.Text = model.SendOn.Value.ToString("MM/dd/yyyy");
            this.txtDueDate.Text = model.DueOn.Value.ToString("MM/dd/yyyy");
            if (model.ReceiveOn != null)
                this.txtReceiveDate.Text = ((DateTime)model.ReceiveOn).ToString("MM/dd/yyyy");
            ddlStatus.SelectedValue = ((int)model.Status).ToString();
            this.txtNote.Text = model.Notes;
        }

        public void StatusBind()
        {
            var invoiceAllStatus = InvoiceStatus.Awaiting_Payment.ToSelectList().ToList();
            var awaitingPayment = InvoiceStatus.Awaiting_Payment.ToText();
            var paymentReceived = InvoiceStatus.Payment_Received.ToText();
            var waive = InvoiceStatus.Waive.ToText();
            var tmInvoice =
                invoiceAllStatus.Where(e => e.Text == awaitingPayment || e.Text == paymentReceived || e.Text == waive)
                    .ToList();
            ddlStatus.DataSource = tmInvoice;
            ddlStatus.DataValueField = "Value";
            ddlStatus.DataTextField = "Text";
            ddlStatus.DataBind();
        }

        protected void AddInvoice(object sender, EventArgs e)
        {
            InvoiceEntity model = new InvoiceEntity();
            TSInvoiceRelationEntity tsmodel = new TSInvoiceRelationEntity();
            model.InvoiceNo = this.txtInvoice.Text;
            model.SendOn = DateTime.Parse(this.txtSendDate.Text);
            model.DueOn = DateTime.Parse(this.txtDueDate.Text);
            if (txtReceiveDate.Text != "")
                model.ReceiveOn = DateTime.Parse(this.txtReceiveDate.Text);
            model.Status = (InvoiceStatus)Enum.Parse(typeof(InvoiceStatus), ddlStatus.SelectedValue);
            model.Notes = txtNote.Text;
            Random rd = new Random();
            if (QS("id", 0) == 0)
            {
                string projects = QS("projectid");
                timeTsheetIDs = lblTimeTsheetIDs.Text;
                SearchInvoiceRequest request = new SearchInvoiceRequest();
                request.ProjectIds = projects;
                request.timeTsheetIDs = timeTsheetIDs;
                //SearchInvoiceResponse response = iapp.SearchTimesheetInvoice(request);
                char[] separator = { ',' };
                int result = 0;
                result = iapp.AddInvoice(model);
                string[] tsids = timeTsheetIDs.Split(separator);
                if (result > 0)
                {
                    for (int i = 0; i < tsids.Count(); i++)
                    {
                        tsmodel.TSId = int.Parse(tsids[i]);
                        tsmodel.InvoiceId = result;
                        iapp.AddTSInvoiceRelation(tsmodel);
                    }
                }
                else
                {
                    ShowFailMessageToClient();
                }
                //SearchTimeSheetsResponse response = new SearchTimeSheetsResponse();
                //List<TimeSheetTicket> list = tsApp.GetTimesheet(result);
                //ExcelReport report = new ExcelReport();
                //report.ExportInvoice(list);
            }
            else
            {
                model.ID = QS("id", 0);
                InvoiceEntity imodel = iapp.GetInvoice(model.ID);
                model.ProposalId = imodel.ProposalId;
                model.Milestone = imodel.Milestone;
                if (iapp.UpdateInvoice(model))
                {
                }
                else
                {
                    ShowFailMessageToClient();
                }
            }
            Response.Write("<script>window.top.location.href = '/Invoice/ToDoList.aspx';</script>");
        }
    }
}