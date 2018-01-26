using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class AssignUserTs : BaseWebsitePage
    {
        #region declare

        UserApplication userApp = new UserApplication();
        TicketsApplication ticketApp = new TicketsApplication();
        SearchUserResponse response = new SearchUserResponse();
        TicketStatusManagerApplication ticketStatusMgr = new TicketStatusManagerApplication();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitEsUserBind();
            }
        }

        private void InitEsUserBind()
        {
            SearchUsersRequest request = new SearchUsersRequest(
                SearchUsersType.Project, false, " FirstName ", " ASC ");

            int tid = QS("tid", 0);

            GetProjectIdAndUserIDResponse responseProject = ticketApp.GetProjectIdAndUserID(tid);//get pid
            request.ProjectID = responseProject.ProjectId;
            response = userApp.SearchUsers(request);
            ddlEs.DataTextField = "FirstName";
            ddlEs.DataValueField = "UserID";
            SetDefaultValueForDropdownList<UsersEntity>(ddlEs,
                response.ResultList.FindAll(x => (x.Role != RolesEnum.CLIENT && x.Role != RolesEnum.Sales && x.Role != RolesEnum.Supervisor)));
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            ddlEs.SelectedIndex = 0;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            int tid = QS("tid", 0);

            if (!BaseValidate(tid)) return;

            TicketsEntity dtoEntity = GetModelByTid(tid);
            dtoEntity.EsUserID = int.Parse(this.ddlEs.SelectedValue);
            dtoEntity.ModifiedBy = UserInfo.UserID;
            dtoEntity.ModifiedOn = DateTime.Now;
            bool Update = ticketApp.UpdateTickets(dtoEntity);

            #region update method

            if (Update)
            {
                ticketStatusMgr.SendEmailToAssignedUserTs(dtoEntity);
            }

            #endregion

            if (Update)
            {
                this.ShowSuccessMessageToClient();
            }
            else
            {
                this.ShowFailMessageToClient();
            }
        }

        private TicketsEntity GetModelByTid(int tid)
        {
            if (tid <= 0) return null;

            TicketsEntity entity = ticketApp.GetTickets(tid);

            return entity != null ? entity : null;
        }

        private bool BaseValidate(int tid)
        {
            bool IsPass = true;

            if (this.ddlEs.SelectedIndex <= 0)
            {
                IsPass = false;
                ShowMessageToClient("Please Select At Least One Es User!", 0, false, false);
            }
            else if (tid <= 0)
            {
                IsPass = false;
                ShowMessageToClient("Please Select At Least One Ticket!", 0, false, false);
            }
            return IsPass;
        }

        private string ValidateDDLIsFirstIndexReturnEmpty(DropDownList ddl)
        {
            return ddl.SelectedIndex <= 0 ? "0" : ddl.SelectedValue;
        }
    }
}
