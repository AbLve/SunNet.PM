using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.UserModel;
using System.Text.RegularExpressions;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;


namespace SunNet.PMNew.PM2014.Ticket.Sunnet
{
    public partial class TicketSelect : TicketPageHelper
    {
        protected override string DefaultOrderBy
        {
            get
            {
                return "TicketID";
            }
        }

        protected override string DefaultDirection
        {
            get
            {
                return "Desc";
            }
        }

        #region declare

        string pid = "";
        int page = 1;
        int recordCount;
        TicketsApplication ticketAPP = new TicketsApplication();
        ProjectApplication proApp = new ProjectApplication();
        UserApplication userApp = new UserApplication();

        List<ProjectDetailDTO> listPorject = new List<ProjectDetailDTO>();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            listPorject = proApp.GetUserProjects(UserInfo);
            ((Pop)(this.Master)).Width = 780;
            if (!IsPostBack)
            {
                pid = QS("pid");
                FillSearchDto();
                // Buring 2013_10_14 contactordev select
              
                TicketsDataBind();

                if (!string.IsNullOrEmpty(pid))
                {
                    if (!CheckSecurity(Convert.ToInt32(pid)))
                    {
                        Response.Redirect("~/SunnetTicket/dashboard.aspx");
                        return;
                    }
                }
            }
        }

        #region initial data bind

        private List<ListItem> GetTicketStatus()
        {
            var statuses = TicketsStateHelper.SunnetSHAllowShowStatus;
            if (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.Sales)
            {
                statuses = TicketsStateHelper.SunnetUSAllowShowStatus;
            }
            return statuses.Select(x => new ListItem() { Text = x.ToText(), Value = ((int)x).ToString() }).ToList();
        }

        private void InitProjectTitleBind(string selected)
        {
            this.ddlProject.DataSource = listPorject;
            this.ddlProject.DataBind((ProjectDetailDTO project, string status) => project.Status.ToString() == status);
            this.ddlProject.SelectItem(selected);
        }

        private void FillSearchDto()
        {
            txtKeyWord.Text = QS("keyword");
            GetTicketStatus().BindDropdown<ListItem>(ddlStatus, "Text", "Value", DefaulAllText, "-1", QS("status"));

            ddlTicketType.SelectItem(QS("tickettype"));
            
            InitProjectTitleBind(QS("project"));
        }

        private void TicketsDataBind()
        {
            var project = 0;
            int.TryParse(ddlProject.SelectedValue, out project);
            var statusValue = -1;
            var status = TicketsState.Draft;
            if (int.TryParse(ddlStatus.SelectedValue, out statusValue) && statusValue >= 0)
                status = (TicketsState)statusValue;

            var typeValue = -1;
            var type = TicketsType.None;
            if (int.TryParse(ddlTicketType.SelectedValue, out typeValue) && typeValue >= 0)
                type = (TicketsType)typeValue;

            var priorith = 0;
           
            var assignuser = 0;


            List<TicketsEntity> list = ticketAPP.SearchTicketsForAllTickets(UserInfo, txtKeyWord.Text, project, status, QS("create"), type, priorith, assignuser,
                 CurrentPageIndex, anpOngoing.PageSize, OrderBy, OrderDirection, out recordCount);

            if (null == list || list.Count <= 0)
            {
                this.trNoTickets.Visible = true;
            }
            else
            {
                this.trNoTickets.Visible = false;
            }
            this.rptTicketsList.DataSource = list;
            this.rptTicketsList.DataBind();
            anpOngoing.RecordCount = recordCount;
        }

        private bool CheckSecurity(int pid)
        {
            List<ProjectDetailDTO> list = new List<ProjectDetailDTO>();
            list = proApp.GetUserProjects(UserInfo);
            list = list.FindAll(x => x.ProjectID == pid);
            return null != list && list.Count > 0;
        }

       

        

         
        #endregion

        #region common show

        protected string GetAllowPid()
        {
            string pidList = "";

            foreach (ProjectsEntity item in listPorject)
            {
                pidList += item.ProjectID + ",";
            }

            return pidList.TrimEnd(',');
        }

        #endregion

    }
}