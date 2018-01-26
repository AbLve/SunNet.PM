using SunNet.PMNew.App;
using SunNet.PMNew.Entity.InvoiceModel.Enums;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Entity.CompanyModel;
using System.Data;
using SunNet.PMNew.PM2014.Event;
using SunNet.PMNew.Impl.SqlDataProvider.Company;

namespace SunNet.PMNew.PM2014.Invoice.ToDo
{
    public partial class ToDoList : BasePage
    {
        ProjectApplication proApp = new ProjectApplication();
        InvoicesApplication invoiceApp = new InvoicesApplication();
        CompanyApplication companyApp = new CompanyApplication();

        protected override string DefaultOrderBy
        {
            get
            {
                return "ProjectTitle";
            }
        }

        protected override string DefaultDirection
        {
            get { return "DESC"; }
        }

        public int tabIndex
        {
            get
            {
                return QS("tmtab", 0);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                InitDdlBind();
                FillInvoiceSearch();
                FillProposalInvoice();

                // BindCompanyList();
            }
        }

        private void InitDdlBind()
        {
            CompanysRepositorySqlDataProvider crsp = new CompanysRepositorySqlDataProvider();
            int cid = crsp.GetCompanyId("Sunnet");
            IList<ProjectDetailDTO> listProject = proApp.GetAllProjects();
            listProject = listProject.Where(c => c.CompanyID != cid).ToList();

            this.ddlProject.DataSource = listProject;
            this.ddlProject.DataBind((ProjectDetailDTO project, string status) => project.Status.ToString() == status);
            this.ddlProject.SelectItem("0");

            this.tm_ddlProject.DataSource = listProject;
            this.tm_ddlProject.DataBind((ProjectDetailDTO project, string status) => project.Status.ToString() == status);
            this.tm_ddlProject.SelectItem("0");

            List<CompanysEntity> list = companyApp.GetCompaniesHasProject();
            list.BindDropdown(tm_ddlCompany, "CompanyName", "ComID", "All", "0", QS("company"));
        }

        private void FillInvoiceSearch()
        {
            txtKeyword.Text = QS("keyword");
            ddlProject.SelectedValue = QS("project");

            tm_ddlCompany.SelectedValue = QS("tm_company");
            tm_ddlProject.SelectedValue = QS("tm_project");
        }

        private void FillProposalInvoice()
        {
            ProposalInvoiceModel model = new ProposalInvoiceModel();
            SearchInvoiceRequest request = new SearchInvoiceRequest();
            request.OrderExpression = OrderBy;
            request.OrderDirection = OrderDirection;
            request.Keywords = txtKeyword.Text;
            request.ProjectId = ddlProject.SelectedValue == "" ? 0 : int.Parse(ddlProject.SelectedValue);
            request.Searchtype = InvoiceSearchType.ProposalOnly;
            SearchInvoiceResponse response = invoiceApp.SearchProposalInvoice(request);
            //这儿状态过滤 去掉了
            //rptInvoiceList.DataSource = response.ProposalList.Where(e => e.Status == InvoiceStatus.Missing_Milestone
            //                                                             || e.Status == InvoiceStatus.Missing_Invoice ||
            //                                                             e.Status == InvoiceStatus.Awaiting_Send).ToList();
            rptInvoiceList.DataSource = response.ProposalList;
            rptInvoiceList.DataBind();
            //proposalInvoiceCount.Text = response.ProposalList.Count(e => e.Status == InvoiceStatus.Missing_Milestone
            //                                                             || e.Status == InvoiceStatus.Missing_Invoice ||
            //                                                             e.Status == InvoiceStatus.Awaiting_Send).ToString();
            proposalInvoiceCount.Text = response.ProposalList.Count.ToString();
            if (response.ResultCount == 0)
            {
                trNoProposalInvoice.Visible = true;
            }
            else
            {
                trNoProposalInvoice.Visible = false;
            }
        }

        protected void tm_ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchProjectsRequest request = new SearchProjectsRequest(SearchProjectsType.Company, false, " Title ", " ASC ");
            request.CompanyID = int.Parse(this.tm_ddlCompany.SelectedValue);
            SearchProjectsResponse response = proApp.SearchProjects(request);
            if (response.ResultList != null || response.ResultList.Count > 0)
            {
                this.tm_ddlProject.DataSource = response.ResultList;
                this.tm_ddlProject.DataBind();
                //this.tm_ddlProject.SelectItem("0");
            }


        }
    }
}