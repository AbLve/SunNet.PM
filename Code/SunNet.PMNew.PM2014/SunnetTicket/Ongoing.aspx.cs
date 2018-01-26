using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.SunnetTicket
{
    public partial class Ongoing : TicketPageHelper
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

        TicketsSearchConditionDTO dto;
        ProjectApplication proApp = new ProjectApplication();
        TicketsApplication ticketAPP = new TicketsApplication();


        #region initial data bind

        private void FillSearchDto()
        {
            proApp.GetUserProjects(UserInfo).BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ProjectID", this.DefaulAllText, "0",
                    QS("project"));
            GetTicketSatus().BindDropdown<ListItem>(ddlStatus, "Text", "Value", DefaulAllText, "-1", QS("status"));
            txtKeyWord.Text = QS("keyword");
            ddlTicketType.SelectItem(QS("tickettype"));

            dto = dto ?? new TicketsSearchConditionDTO();
            dto.OrderExpression = OrderBy;
            dto.OrderDirection = OrderDirection;

            dto.KeyWord = ReturnTicketId(QS("keyword").Trim().NoHTML());
            dto.Status = ddlStatus.SelectedValue;
            dto.TicketType = ddlTicketType.SelectedValue;
            dto.Project = ddlProject.SelectedValue;
            dto.IsInternal = false;
            if (UserInfo.Role == RolesEnum.ADMIN || UserInfo.Role == RolesEnum.PM)
            {
                dto.Company = "0";
            }
            else
            {
                dto.Company = UserInfo.CompanyID.ToString();
                dto.Client = UserInfo.UserID.ToString();
            }
        }

        private void TicketsDataBind()
        {
            List<TicketsEntity> list = null;

            GetTicketsListByConditionRequest request = new GetTicketsListByConditionRequest();


            int pageCount = 0;
            list = ticketAPP.GetOngoingTicketsList(UserInfo.ID, int.Parse(dto.Project), int.Parse(dto.Company),
                               dto.Status.ToEnum<ClientTicketState>(), dto.TicketType.ToEnum<TicketsType>(), dto.KeyWord, false,
                               CurrentPageIndex, anpOngoing.PageSize, dto.OrderExpression, dto.OrderDirection, out pageCount);
            if (pageCount > 0)
            {

                this.trNoTickets.Visible = false;
                this.rptTicketsList.Visible = true;
                rptTicketsList.DataSource = list;
                this.rptTicketsList.DataBind();
                anpOngoing.RecordCount = pageCount;
            }
            else
            {
                this.trNoTickets.Visible = true;
                this.rptTicketsList.Visible = false;
            }
        }

        private List<ListItem> GetTicketSatus()
        {
            var list = new List<ListItem>();
            foreach (int value in Enum.GetValues(typeof(ClientTicketState)))
            {
                if (value != (int)ClientTicketState.None)
                {
                    if (!ClientDisAllowShowStatus().Contains(value.ToString().ToEnum<ClientTicketState>()))
                    {
                        list.Add(new ListItem(value.ToString().ToEnum<ClientTicketState>().ToText(), value.ToString()));
                    }
                }
            }
            return list;
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ticketList = ticketAPP.GetListByUserId(UserInfo.UserID);
                FillSearchDto();
                TicketsDataBind();
            }
        }

        List<TicketUsersEntity> ticketList;

        protected string GetAction(int ID, TicketsState ticketsState, int confirmEstmateUserId)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<a href=\"Detail.aspx?tid={0}&returnurl=" + ReturnUrl + "\" target='_blank' ticketId='{0}'><img src=\"/Images/icons/view.png\" title=\"View\" id='imageOpen{0}'></a>"
                , ID);
            if (ticketsState == TicketsState.Ready_For_Review)
            {
                TicketUsersEntity ticketUsersEntity = ticketAPP.GetTicketUser(ID, UserInfo.UserID);
                if (ticketUsersEntity != null && ticketUsersEntity.Type == TicketUsersType.Create)
                {
                    sb.Append("&nbsp;<a href=\"Approve.aspx?tid=" + ID.ToString()
                        +"&returnurl=" + Server.UrlEncode(Request.RawUrl) 
                        + "\" data-target=\"#modalsmall\" data-toggle=\"modal\"><img src=\"/Images/icons/approve.png\" title=\"Approve\"></a>");
                    sb.Append("&nbsp;<a href=\"Deny.aspx?tid=" + ID.ToString()
                        + "&returnurl=" + Server.UrlEncode(Request.RawUrl)
                        + "\" data-target=\"#modalsmall\" data-toggle=\"modal\"><img src=\"/Images/icons/deny.png\" title=\"Deny\"></a>");
                }
            }

            if (ticketsState == TicketsState.Waiting_Confirm && confirmEstmateUserId == UserInfo.UserID)
                sb.AppendFormat("&nbsp;<a href='ConfirmEstimates.aspx?tid={0}' data-toggle='modal' data-target='#modalsmall' title='Waiting Confirm'>", ID)
                    .Append("<img src='/Images/icons/pmreview.png' alt='Waiting Confirm'></a>");

            if (ticketList != null && ticketList.Find(r => r.TicketID == ID) != null)
            {
                sb.AppendFormat("&nbsp;<a href='###' id='{0}' onclick='cancelImg({0})'>", ID)
                .Append("<img alt='Cancel' title='Cancel' src='/images/icons/cancel_new.png' /></a>");
            }
            return sb.ToString();
        }
    }
}