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
using SunNet.PMNew.Framework.Utils.Providers;


namespace SunNet.PMNew.PM2014.SunnetTicket
{
    public partial class MyTicket : TicketPageHelper
    {
        protected override string DefaultOrderBy
        {
            get
            {
                return "ShowNotification";
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
        TicketsApplication ticketAPP = new TicketsApplication();
        ProjectApplication proApp = new ProjectApplication();

        List<ProjectDetailDTO> listPorject = new List<ProjectDetailDTO>();
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            listPorject = proApp.GetUserProjects(UserInfo);
            if (!IsPostBack)
            {
                //InitTicketSatusBind();
                InitProjectTitleBind();
                InitClientPriorityBind();
                //selectValue.Value  = QS("status");
                ddlPriority.SelectedValue = QS("ticketPriority", 0).ToString();
                ddlProject.SelectItem(QS("project"));
                ddlTicketType.SelectedValue = QS("tickettype");
                txtKeyWord.Text = QS("keyword").Trim();
                txtCreated.Text = QS("create").Trim();
                TicketsDataBind();
            }
        }

        #region initial data bind

        private void InitTicketSatusBind()
        {
            //var statuses = ticketAPP.GetAllowStatusOfMyTicket(UserInfo);
            //foreach (var v in statuses)
            //{
            //    ddlStatus.Items.Add(new ListItem() { Value = ((int)v).ToString(), Text = v.ToText() });
            //}
        }

        private void InitProjectTitleBind()
        {
            this.ddlProject.DataSource = listPorject;
            this.ddlProject.DataBind(delegate(ProjectDetailDTO project, string status)
            {
                return project.Status.ToString() == status;
            });
        }

        private void InitClientPriorityBind()
        {
            var dictionary = new Dictionary<int, string>();
            foreach (int value in Enum.GetValues(typeof(PriorityState)))
            {
                dictionary.Add(value, Enum.GetName(typeof(PriorityState), value));
            }

            ddlPriority.DataSource = dictionary;

            ddlPriority.DataTextField = "Value";
            ddlPriority.DataValueField = "Key";
            ddlPriority.DataBind();
            ddlPriority.Items.Insert(0, new ListItem("ALL", "0"));
        }

        private void TicketsDataBind()
        {
            List<TicketsEntity> list = null;

            TicketsType ticketType = TicketsType.None;
            if (ddlTicketType.SelectedValue != "")
                ticketType = (TicketsType)(int.Parse(ddlTicketType.SelectedValue));

            //var statusStr = QS("status");
            //if (statusStr == "null")
            //{
            //    this.trNoTickets.Visible = true;
            //    this.rptTicketsList.DataSource = new List<TicketsEntity>();
            //    this.rptTicketsList.DataBind();
            //    anpOngoing.RecordCount = 0;
            //    return;
            //}
            var recordCount = 0;
            list = ticketAPP.GetMyTicketsList(UserInfo, int.Parse("0" + ddlProject.SelectedValue), int.Parse(ddlPriority.SelectedValue), QS("create"), ticketType, QS("keyword"), CurrentPageIndex, anpOngoing.PageSize
                , OrderBy, OrderDirection, out recordCount);


            if (null == list || list.Count <= 0)
            {
                this.trNoTickets.Visible = true;
            }
            this.rptTicketsList.DataSource = list;
            this.rptTicketsList.DataBind();
            anpOngoing.RecordCount = recordCount;
        }

        #endregion

        protected string TicketCreateUser(string firstName, string lastName,bool isEstimates)
        {
            if (isEstimates)
            {
                if (UserInfo.Office == "US" && (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.Sales || UserInfo.Role == RolesEnum.ADMIN))
                    return string.Format("{0} {1}",firstName,lastName);
                else
                    return "Client";
            }
            return string.Format("{0} {1}", firstName, lastName); 
        }

    }
}