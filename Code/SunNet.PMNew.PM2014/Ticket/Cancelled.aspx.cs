using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.PM2014.Ticket
{
    public partial class Cancelled : BasePage
    {

        protected override string DefaultOrderBy
        {
            get
            {
                return "ModifiedOn";
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
            list = ticketAPP.GetCancelList(UserInfo.ID, int.Parse(dto.Project), int.Parse(dto.Company),
                                dto.Status.ToEnum<ClientTicketState>(), dto.TicketType.ToEnum<TicketsType>(), dto.KeyWord,
                                CurrentPageIndex, anpCancel.PageSize, dto.OrderExpression, dto.OrderDirection, out pageCount);
            if (pageCount > 0)
            {

                this.trNoTickets.Visible = false;
                this.rptTicketsList.Visible = true;
                rptTicketsList.DataSource = list;
                this.rptTicketsList.DataBind();
                anpCancel.RecordCount = pageCount;
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
            var array = new ClientTicketState[] { ClientTicketState.Cancelled, ClientTicketState.Denied };
            foreach (ClientTicketState value in array)
            {
                list.Add(new ListItem(value.ToText(), ((int)value).ToString()));
            }
            return list;
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            _fbmHandler = new FeedBackMessageHandler(UserInfo);
            if (!IsPostBack)
            {

                FillSearchDto();
                TicketsDataBind();
            }
        }

        #region common method

        private FeedBackMessageHandler _fbmHandler;

        #endregion
    }
}