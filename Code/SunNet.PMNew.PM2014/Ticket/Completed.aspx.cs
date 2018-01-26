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
    public partial class Completed : TicketPageHelper
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
            list = ticketAPP.GetCompletedTicketsList(UserInfo.ID, int.Parse(dto.Project), int.Parse(dto.Company),
                               dto.TicketType.ToEnum<TicketsType>(), dto.KeyWord, true,
                               CurrentPageIndex, anpCompleted.PageSize, dto.OrderExpression, dto.OrderDirection, out pageCount);
            if (pageCount > 0)
            {
                this.trNoTickets.Visible = false;
                this.rptTicketsList.Visible = true;
                rptTicketsList.DataSource = list;
                this.rptTicketsList.DataBind();
                anpCompleted.RecordCount = pageCount;
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
            list.Add(new ListItem(TicketsState.Completed.ToString(), ((int)TicketsState.Completed).ToString()));
            return list;
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

        protected int GetCreateUser(object ticketID)
        {
            return ticketAPP.GetTicketCreateUser((int)ticketID).UserID;
        }
    }
}