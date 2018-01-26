using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TicketModel.TicketsDTO;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.OA.ProposalTracker
{
    public partial class AddRelatedticket : BasePage
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
            get { return "DESC"; }
        }

        #region declare

        List<TicketsEntity> list = null;
        int recordCount;
        TicketsApplication ticketApp = new TicketsApplication();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Pop)this.Master).Width = 780;
            if (UserInfo.Role != RolesEnum.ADMIN && UserInfo.Role != RolesEnum.PM && UserInfo.Role != RolesEnum.Sales)
            {
                ShowFailMessageToClient("unauthorized access.");
                return;
            }
            txtKeyword.Text = QS("keyword");
            TicketsDataBind();
        }


        public void TicketsDataBind()
        {
            string keyWord = txtKeyword.Text.Trim();

            int proposaltrackerId = QS("ID", 0);
            ProposalTrackerEntity entity = new App.ProposalTrackerApplication().Get(proposaltrackerId);
            if (entity == null)
            {
                ShowFailMessageToClient("unauthorized access.");
                return;
            }

            var condition = new SearchTicketCondition();
            if (!string.IsNullOrEmpty(keyWord))
                condition.Keyword = keyWord;
            condition.Statuses.AddRange(TicketsStateHelper.NoneFailStates);
            condition.ProjectId = entity.ProjectID;
            condition.OrderBy = OrderBy;
            condition.OrderDirection = OrderDirection;
            condition.PageCount = anpWaitting.PageSize;
            //condition.CurrentPage = anpWaitting.CurrentPageIndex;
            condition.CurrentPage = CurrentPageIndex;

            condition.UserId = UserInfo.ID;
            
            int wid = QS("ID", 0);

            list = ticketApp.SearchTicketsNotInTid(condition);
            

            if (null != list && list.Count > 0)
            {
                trNoTickets.Visible = false;
            }
            rptTickets.DataSource = list;
            rptTickets.DataBind();

            anpWaitting.RecordCount = condition.TotalRecords;
        }

    }
}