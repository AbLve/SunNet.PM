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

namespace SunNet.PMNew.PM2014.Ticket
{
    public partial class Draft : TicketPageHelper
    {
        protected override string DefaultOrderBy
        {
            get { return "ModifiedOn"; }
        }
        protected override string DefaultDirection
        {
            get
            {
                return "Desc";
            }
        }
        #region declare

        private TicketsSearchConditionDTO dto;
        private ProjectApplication proApp = new ProjectApplication();
        private TicketsApplication ticketAPP = new TicketsApplication();
        private List<ProjectDetailDTO> listPorject = new List<ProjectDetailDTO>();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                SetSearchControlsStatus();
                TicketsDataBind();
            }
        }

        #region initial data bind

        private void SetSearchControlsStatus()
        {
            TicketSatusBind();
            listPorject = proApp.GetUserProjects(UserInfo);
            listPorject.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ProjectID", this.DefaulAllText, "0",
                QS("project"));
            ddlTicketType.SelectItem(QS("tickettype"));
            txtKeyWord.Text = QS("keyword");

            dto = dto ?? new TicketsSearchConditionDTO();
            dto.OrderExpression = OrderBy;
            dto.OrderDirection = OrderDirection;

            dto.KeyWord = ReturnTicketId(QS("keyword").Trim().NoHTML());

            dto.Status = ((int)ClientTicketState.Draft).ToString();
            dto.TicketType = string.IsNullOrEmpty(QS("type")) ? ddlTicketType.SelectedValue : QS("type");
            dto.Project = string.IsNullOrEmpty(QS("project")) ? ddlProject.SelectedValue : QS("project");
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
            list = ticketAPP.GetDraftedTicketsList(UserInfo.ID, int.Parse(dto.Project), int.Parse(dto.Company),
                                dto.TicketType.ToEnum<TicketsType>(), dto.KeyWord, true,
                                CurrentPageIndex, anpDraft.PageSize, dto.OrderExpression, dto.OrderDirection,
                                out pageCount);
            if (pageCount > 0)
            {
                trNoTickets.Visible = false;
                rptTicketsList.Visible = true;
                rptTicketsList.DataSource = list;
                rptTicketsList.DataBind();

                anpDraft.RecordCount = pageCount;
            }
            else
            {
                trNoTickets.Visible = true;
                rptTicketsList.Visible = false;
            }
        }

        private void TicketSatusBind()
        {
            ddlStatus.Items.Add(new ListItem()
            {
                Text = ClientTicketState.Draft.ToString(),
                Value = ((int)ClientTicketState.Draft).ToString()
            });
            ddlStatus.Enabled = false;
        }

        #endregion

    }
}