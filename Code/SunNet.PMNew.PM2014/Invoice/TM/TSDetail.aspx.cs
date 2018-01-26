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

namespace SunNet.PMNew.PM2014.Invoice.TM
{
    public partial class TSDetail : BasePage
    {
        InvoicesApplication iapp = new InvoicesApplication();
        CompanyApplication companyApp = new CompanyApplication();

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
            RepDataBind();
        }

        public void RepDataBind()
        {
            
            hidProject.Value = QS("projectIds");
            SearchInvoiceRequest request = new SearchInvoiceRequest();
            request.OrderExpression = OrderBy;
            request.OrderDirection = OrderDirection;
            request.ProjectIds = QS("projectIds");
            request.CompanyId = QS("companyId", 0);
            SearchInvoiceResponse response = iapp.SearchTimesheetInvoice(request);
            rptTicketsList.DataSource = response.TimesheetList;
            rptTicketsList.DataBind();
            decimal numHours=0;
            foreach (var item in response.TimesheetList)
            {
                numHours+= item.Hours;
            }
            lblTotalHours.Text = numHours.ToString();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Redirect("/Invoice/ToDoList.aspx");
        }
       
    }
}