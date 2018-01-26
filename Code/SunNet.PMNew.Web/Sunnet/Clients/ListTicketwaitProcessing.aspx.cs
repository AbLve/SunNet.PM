﻿using SunNet.PMNew.Framework.Utils.Helpers;
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
using SunNet.PMNew.Entity.ProjectModel;
using System.Text;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class ListTicketwaitProcessing : BaseWebsitePage
    {
        #region declare

        UserApplication userApp = new UserApplication();
        TicketsRelationApplication trApp = new TicketsRelationApplication();
        string pid = "";
        string allowStatus = "";
        int page = 1;
        TicketsSearchConditionDTO dto;
        List<ProjectDetailDTO> listPorject = new List<ProjectDetailDTO>();
        ProjectApplication proApp = new ProjectApplication();
        TicketsApplication ticketAPP = new TicketsApplication();

        #endregion


        #region FeedbackMessage
        protected FeedBackMessageHandler fbmHandler;
        protected string FeedBackMessage(object ticketId)
        {
            return fbmHandler.FeedBackMessage(ticketId, UserInfo.Role);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            fbmHandler = new FeedBackMessageHandler(UserInfo);
            allowStatus = ReturnSatisfyConditionStatus(ClientAllowShowWaitProcessStatus());
            listPorject = proApp.GetUserProjects(UserInfo);
            if (!IsPostBack)
            {
                TicketSatusBind();
                ProjectTitleBind();
                int projectID = QS("pid", 0);
                if (projectID != 0)
                {
                    ddlProject.SelectedValue = projectID.ToString();
                    TicketsDataBind(projectID.ToString());
                }
                else
                {
                    TicketsDataBind(string.Empty);
                }
            }
        }

        #region initial data bind

        private void TicketsDataBind(string projectID)
        {
            List<TicketsEntity> list = null;

            GetTicketsListByConditionRequest request = new GetTicketsListByConditionRequest();

            string keyWord = this.txtKeyWord.Text.Trim().NoHTML();

            #region set search condition value

            dto = new TicketsSearchConditionDTO();

            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                dto.Company = UserInfo.CompanyID.ToString();
                dto.Client = UserInfo.UserID.ToString();
            }
            else if (UserInfo.Role == RolesEnum.ADMIN ||
                     UserInfo.Role == RolesEnum.PM)
            {
                dto.Company = "0";
            }

            if (!string.IsNullOrEmpty(projectID))
            {
                pid = projectID;
            }
            else
            {
                if (this.ddlProject.SelectedIndex <= 0)
                {
                    pid = "0";
                }
                else
                {
                    pid = this.ddlProject.SelectedValue;
                }
            }

            dto.Project = pid;
            dto.OrderExpression = hidOrderBy.Value;
            dto.OrderDirection = hidOrderDirection.Value;
            dto.KeyWord = ReturnTicketId(keyWord);
            dto.Client = UserInfo.UserID.ToString();

            ClientTicketState state = ClientTicketState.None;
            if (ddlStatus.SelectedIndex > 0)
            {
                state = (ClientTicketState)(int.Parse(ddlStatus.SelectedValue));
            }

            TicketsType ticketType = TicketsType.None;

            ticketType = (TicketsType)int.Parse(ddlTicketType.SelectedValue);
            #endregion

            int pageCount = ticketAPP.GetWaitingforResponseCount(UserInfo.ID, int.Parse(dto.Project), int.Parse(dto.Company), state, ticketType, dto.KeyWord);

            if (pageCount > 0)
            {
                list = ticketAPP.GetWaitingforResponseList(UserInfo.ID, int.Parse(dto.Project), int.Parse(dto.Company), state, ticketType
                    , dto.KeyWord, anpUsers.CurrentPageIndex, anpUsers.PageSize, dto.OrderExpression, dto.OrderDirection);

                this.trNoTickets.Visible = false;
            }
            else
            {
                this.trNoTickets.Visible = true;
            }

            rptTicketsList.DataSource = list;
            this.rptTicketsList.DataBind();

            anpUsers.RecordCount = pageCount;
        }

        private void TicketSatusBind()
        {
            var dictionary = new Dictionary<int, string>();

            foreach (int value in Enum.GetValues(typeof(ClientTicketState)))
            {
                if (UnderWaitProcessStatus().Contains(value))
                {
                    dictionary.Add(value, Enum.GetName(typeof(ClientTicketState), value).Replace('_', ' '));
                }
            }
            ddlStatus.DataSource = dictionary;
            ddlStatus.DataTextField = "Value";
            ddlStatus.DataValueField = "Key";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("Please select...", ""));
        }

        private void ProjectTitleBind()
        {
            this.ddlProject.DataTextField = "Title";
            this.ddlProject.DataValueField = "ProjectID";

            this.ddlProject.DataSource = listPorject;
            this.ddlProject.DataBind();
            if (listPorject.Count != 1)
                this.ddlProject.Items.Insert(0, new ListItem("Please select...", ""));
        }

        #endregion

        #region commen show

        public string GetClientStatusNameBySatisfyStatus(int status, int TicketID)
        {
            return fbmHandler.GetClientStatusNameBySatisfyStatus(status, TicketID);
        }

        protected string GetUserNameByCreateID(string cid)
        {
            string userName = this.GetClientUserName(userApp.GetUser(Convert.ToInt32(cid)));
            return userName.Length == 0 ? "" : userName;
        }

        public string ShowRelatedByTid(string tid)
        {
            return trApp.GetAllRelationStringById(Convert.ToInt32(tid), false).TrimEnd(',');
        }
        public string GetOwnProjectID()
        {
            string pidList = "";

            foreach (ProjectsEntity item in listPorject)
            {
                pidList += item.ProjectID + ",";
            }

            return pidList.TrimEnd(',');
        }

        private string ReturnSatisfyConditionStatus(int[] array)
        {
            string tempStatus = "";
            foreach (int item in array)
            {
                tempStatus += item + ",";
            }
            return tempStatus.TrimEnd(',');
        }

        private string GetStatusBySelValue(int ddlClientStatu)
        {
            if (ddlClientStatu == (int)ClientTicketState.Waiting_Feedback)
            {
                dto.FeedBackTicketsList = fbmHandler.FeedBackRequiredTicketIDs;
            }
            List<TicketsState> list = fbmHandler.GetSearchTicketStatuses(ddlClientStatu);
            StringBuilder statuses = new StringBuilder();
            foreach (TicketsState tstatus in list)
            {
                statuses.Append((int)tstatus);
                statuses.Append(",");
            }
            return statuses.ToString().TrimEnd(",".ToCharArray());
        }
        #endregion

        protected void SearchImgBtn_Click(object sender, ImageClickEventArgs e)
        {
            TicketsDataBind(string.Empty);
        }

        protected void anpUsers_PageChanged(object sender, EventArgs e)
        {
            page = anpUsers.CurrentPageIndex;
            TicketsDataBind(string.Empty);
        }
    }
}