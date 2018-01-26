using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Text;

namespace SunNet.PMNew.PM2014.Ticket
{
    public partial class Waitting : TicketPageHelper
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

            var pageCount = 0;
            list = ticketAPP.GetWaitingforResponseList(UserInfo.ID, int.Parse(dto.Project), int.Parse(dto.Company),
                    dto.Status.ToEnum<ClientTicketState>(), dto.TicketType.ToEnum<TicketsType>(), dto.KeyWord,
                    CurrentPageIndex, anpWaitting.PageSize, dto.OrderExpression, dto.OrderDirection, out pageCount);
            if (pageCount > 0)
            {

                this.trNoTickets.Visible = false;
                this.rptTicketsList.Visible = true;
                rptTicketsList.DataSource = list;
                this.rptTicketsList.DataBind();
                anpWaitting.RecordCount = pageCount;
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
                    if (UnderWaitProcessStatus().Contains(value))
                    {
                        list.Add(new ListItem(value.ToString().ToEnum<ClientTicketState>().ToText(), value.ToString()));
                    }
                }
            }
            return list;
        }

        protected string GetAction(int ID, TicketsState ticketsState, int confirmEstmateUserId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .AppendFormat
                ("<a href=\"Detail.aspx?tid={0}&returnurl=" + ReturnUrl + "\" target='_blank'  ticketId='{0}'><img src=\"/Images/icons/view.png\" title=\"View\" id='imageOpen{0}'></a>"
                , ID);

            if (ticketsState == TicketsState.Ready_For_Review)
            {
                if (ticketAPP.IsTicketUser(ID, UserInfo.UserID))
                {
                    stringBuilder.Append("&nbsp;<a href=\"Approve.aspx?tid=" + ID.ToString()
                        + "&returnurl=" + Server.UrlEncode(Request.RawUrl)
                        + "\" data-target=\"#modalsmall\" data-toggle=\"modal\"><img src=\"/Images/icons/approve.png\" title=\"Approve\"></a>");
                    stringBuilder.Append("&nbsp;<a href=\"Deny.aspx?tid=" + ID.ToString()
                        + "&returnurl=" + Server.UrlEncode(Request.RawUrl)
                        + "\" data-target=\"#modalsmall\" data-toggle=\"modal\"><img src=\"/Images/icons/deny.png\" title=\"Deny\"></a>");
                }
            }
            else
            {
                if (ticketsState == TicketsState.Waiting_Confirm && confirmEstmateUserId == UserInfo.UserID)
                    stringBuilder.AppendFormat("&nbsp;<a href='ConfirmEstimates.aspx?tid={0}' data-toggle='modal' data-target='#modalsmall' title='Waiting Confirm'>", ID)
                        .Append("<img src='/Images/icons/pmreview.png' alt='Waiting Confirm'></a>");
            }

            return stringBuilder.ToString();
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillSearchDto();
                TicketsDataBind();
            }
        }
    }
}