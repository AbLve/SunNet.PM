using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class AddRelationTickets : BaseAscx
    {
        #region declare

        TicketsSearchConditionDTO dto;
        List<TicketsEntity> list = null;
        int page = 1;
        int recordCount;
        TicketsApplication ticketApp = new TicketsApplication();
        TicketsRelationApplication trApp = new TicketsRelationApplication();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            TicketsDataBind();
        }

        public bool CheckSecurity(int uid)
        {
            if ((UserInfo.UserID != uid) &&
                (UserInfo.Role != RolesEnum.PM && UserInfo.Role != RolesEnum.ADMIN &&
                 UserInfo.Role != RolesEnum.QA && UserInfo.Role != RolesEnum.DEV &&
                 UserInfo.Role != RolesEnum.Leader && UserInfo.Role != RolesEnum.Contactor))
            {
                return false;
            }
            return true;
        }
        public void TicketsDataBind()
        {
            string keyWord = this.txtKeyWord.Text.Trim();

            int tid = QS("tid", 0);

            GetProjectIdAndUserIDResponse response = ticketApp.GetProjectIdAndUserID(tid);

            if (!CheckSecurity(response.CreateUserId) || keyWord.Length == 0) return;

            #region set value

            GetTicketsListByConditionRequest request = new GetTicketsListByConditionRequest();
            dto = new TicketsSearchConditionDTO();
            dto.OrderExpression = hidOrderBy.Value;
            dto.OrderDirection = hidOrderDirection.Value;
            dto.KeyWord = keyWord;
            dto.Status = CanShowStatus();
            dto.Project = response.ProjectId.ToString();
            dto.IsInternal = true;//true here, for unlimited
            dto.IsFeedBack = false;
            dto.FeedBackTicketsList = "";
            request.TicketSc = dto;
            #endregion

            string ListNeedRemove = trApp.GetAllRelationStringById(tid, true) + " " + tid;

            list = ticketApp.GetTicketListBySearchCondition(request, out  recordCount, page, anpUsers.PageSize);

            if (null != list && list.Count > 0)
            {
                trNoTickets.Visible = false;
            }

            foreach (string item in ListNeedRemove.Split(' '))
            {
                if (item.Length > 0)
                {
                    int id = Convert.ToInt32(item);
                    list.RemoveAll(x => x.TicketID == id);
                }
            }
            this.rptRelationTicketsList.DataSource = list;
            this.rptRelationTicketsList.DataBind();

            anpUsers.RecordCount = recordCount;
        }
        private string CanShowStatus()
        {
            string list = "";
            int[] array = { (int)TicketsState.Draft, (int)TicketsState.Cancelled, (int)TicketsState.Estimation_Fail };

            foreach (int value in Enum.GetValues(typeof(TicketsState)))
            {
                if (!array.Contains(value))
                {
                    list += value + ",";
                }
            }
            return list.TrimEnd(',');
        }

        protected void anpUsers_PageChanged(object sender, EventArgs e)
        {
            page = anpUsers.CurrentPageIndex;
            TicketsDataBind();
        }
    }
}