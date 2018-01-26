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
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Helpers;


namespace SunNet.PMNew.Web.UserControls
{
    public partial class AddWorkRequestRelation : BaseAscx
    {

       
        UserApplication userApp = new UserApplication();
        #region declare

        TicketsSearchConditionDTO dto;
        List<TicketsEntity> list = null;
        int page = 1;
        int recordCount;
        TicketsApplication ticketApp = new TicketsApplication();
        WorkRequestApplication wrApp = new WorkRequestApplication();

        #endregion

        public UsersEntity UserInfo
        {
            get
            {
                IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                string userID = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("LoginUserID")));
                if (string.IsNullOrEmpty(userID))
                {
                    return null;
                }
                UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("LoginUserID"), 30);

                int id = int.Parse(userID);
                UsersEntity model = userApp.GetUser(id);
                return model;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                TicketsDataBind();
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
        private void TicketsDataBind()
        {
            string keyWord = this.txtKeyWord.Text.Trim();

            int wid = QS("wid", 0);

            //GetProjectIdAndUserIDResponse response = ticketApp.GetProjectIdAndUserID(wid);

            //if (!CheckSecurity(response.CreateUserId) || keyWord.Length == 0) return;

            #region set value


            GetTicketsListByConditionRequest request = new GetTicketsListByConditionRequest();
            dto = new TicketsSearchConditionDTO();
            dto.OrderExpression = "CreatedOn";
            dto.OrderDirection = "asc";
            dto.KeyWord = keyWord;
            dto.Status = CanShowStatus();
            dto.IsInternal = true;//true here, for unlimited
            dto.IsFeedBack = false;
            dto.FeedBackTicketsList = "";
            int pid = QS("pid", 0);
            if (pid > 0)
                dto.Project = pid.ToString();
            request.TicketSc = dto;
            #endregion

            List<string> ListNeedRemove = wrApp.GetAllRelationStringByWorkRequest(wid);

            list = ticketApp.GetTicketListBySearchCondition(request, out  recordCount, page, anpUsers.PageSize);

            if (null != list && list.Count > 0)
            {
                trNoTickets.Visible = false;
            }

            foreach (string item in ListNeedRemove)
            {
                if (item.Length > 0)
                {
                    int id = Convert.ToInt32(item);
                    list.RemoveAll(x => x.TicketID == id);
                }
            }
            //int pid = QS("pid", 0);
            //if (pid > 0)
            //    list = list.FindAll(x => x.ProjectID == pid);
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