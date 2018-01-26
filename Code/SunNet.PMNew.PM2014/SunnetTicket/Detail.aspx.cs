using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Extensions;

namespace SunNet.PMNew.PM2014.SunnetTicket
{
    public partial class Detail : BasePage
    {
        TicketsApplication ticketApp = new TicketsApplication();
        private UserApplication userApp = new UserApplication();
        public bool isShowRating = false;
        List<ClientProgressState> displayStates = new List<ClientProgressState>()
        { ClientProgressState.Draft, ClientProgressState.Submit, ClientProgressState.PM_Review,
            ClientProgressState.Developing, ClientProgressState.Testing, ClientProgressState.Client_Confirm,
            ClientProgressState.Completed };
        public bool IsCompleted { get; set; }
        public int TicketID { get; set; }
        public TicketsEntity ticketEntity;

        public string ReviewUrl = "";
        public string ReviewName = "";

        public bool isReadyForReview = false;
        public string UrlApprove = "";
        public string UrlDeny = "";
        public string ConfirmEstmateUser = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Ticket)this.Master).CurrentModule = Codes.SelectedSunnetMenu.Ticket;
            int tid = QS("tid", 0);

            if (tid <= 0)
            {
                this.ShowArgumentErrorMessageToClient();
                return;
            }
            else
            {
                TicketID = tid;
                ticketEntity = ticketApp.GetTicketWithProjectTitle(tid);
                IsCompleted = ticketEntity.Status == TicketsState.Completed;
                HideControlByStatus(ticketEntity);//hidden control by ticket status
                if (ticketEntity != null)
                {
                    progress.orderedDisplayStates = displayStates;
                    progress.CurrentState = ticketApp.ConvertTicketStateToClientProgressState(ticketEntity.RealStatus);
                    this.feedbacks.IsSunnet = true;
                    this.feedbacks.TicketsEntityInfo = ticketEntity;
                    //ticketBasicInfo.TicketsEntity = ticketEntity;
                    //hdStar.Value = ticketEntity.Star.ToString();
                    changeHistory.TicketID = TicketID;
                    GetButtonsHTML();

                    TicketUsersEntity ticketUser = ticketApp.GetTicketUser(ticketEntity.TicketID, UserInfo.ID);
                    if (ticketUser != null)
                        ltlStatus.Text = ticketUser.WorkingOnStatus.ToText();
                    //Ticket 3023 #4. 隐藏按纽 （[WorkingOn][Completed] [Canceled][None]）
                    //if (ticketUser == null || UserInfo.Role == RolesEnum.CLIENT)
                    phlWorkingOn.Visible = false;
                    ticketEntity.CreatedUserEntity = userApp.GetUser(ticketEntity.CreatedBy);

                    this.fileUpload.TicketID = ticketEntity.TicketID;
                    if (ticketEntity.IsEstimates && ticketEntity.Status > TicketsState.Waiting_Confirm && ticketEntity.ConfirmEstmateUserId > 0)
                        ConfirmEstmateUser = userApp.GetUser(ticketEntity.ConfirmEstmateUserId).FirstAndLastName;
                }
                else
                {
                    this.ShowArgumentErrorMessageToClient();
                    return;
                }
            }

        }

        private void HideControlByStatus(TicketsEntity entity)
        {
            if (entity.Status == TicketsState.Completed)
            {
                isShowRating = true;
            }
        }

        protected string GetPriority(int value)
        {
            switch (value)
            {
                case 1:
                    return "Low";
                case 2:
                    return "Medium";
                case 3:
                    return "High";
                case 4:
                    return "Emergency";
                default: return string.Empty;
            }
        }

        protected string GetButtonsHTML()
        {
            StringBuilder resultButtonHTMLBuilder = new StringBuilder();
            TicketsState currentTicketState = ticketEntity.Status;

            if (ticketApp.IsTicketUser(ticketEntity.ID, UserInfo.UserID))
            {
                switch (UserInfo.Role)
                {
                    case RolesEnum.PM:
                        {

                            ReviewUrl = "PMReview.aspx?tid=" + ticketEntity.ID.ToString() + "&pid=" + ticketEntity.ProjectID.ToString();
                            ReviewName = "PM Review";
                            break;
                        }
                    case RolesEnum.CLIENT:
                        {
                            TicketUsersEntity ticketUsersEntity = ticketApp.GetTicketUser(ticketEntity.TicketID, UserInfo.UserID);

                            if (ticketApp.ClientDealState.Find(r => r == ticketEntity.Status) != 0 &&
                                ticketUsersEntity != null && ticketUsersEntity.Type == TicketUsersType.Create)
                            {
                                SetReadyforReview();
                            }
                            break;
                        }

                    case RolesEnum.QA:
                        {
                            if (ticketApp.QaDealState.Find(r => r == ticketEntity.Status) != 0)
                            {
                                if (ticketEntity.Status == TicketsState.Waiting_For_Estimation)
                                {

                                    ReviewUrl = "QAReview.aspx?tid=" + ticketEntity.ID.ToString();
                                    ReviewName = "QA Review";
                                }
                                else
                                {
                                    if (ticketEntity.Status == TicketsState.Ready_For_Review)
                                    {
                                        SetReadyforReview();
                                    }
                                    else
                                    {
                                        ReviewUrl = "QAReview.aspx?tid=" + ticketEntity.ID.ToString();

                                        ReviewName = "QA Review";
                                    }

                                }
                            }
                            break;
                        }

                    case RolesEnum.Leader:
                    case RolesEnum.DEV:
                        {
                            if (ticketApp.DevDealState.Find(r => r == ticketEntity.Status) != 0)
                            {
                                if (ticketEntity.Status == TicketsState.Waiting_For_Estimation)
                                {
                                    if (ticketEntity.EsUserID == UserInfo.UserID)
                                    {
                                        ReviewUrl = "DevReview.aspx?tid=" + ticketEntity.ID.ToString();

                                        ReviewName = "DEV Review";
                                    }
                                }
                                else
                                {
                                    if (UserInfo.Role == RolesEnum.Leader)
                                    {
                                        if (ticketEntity.Status == TicketsState.Ready_For_Review)
                                        {
                                            SetReadyforReview();
                                        }
                                        else
                                        {
                                            ReviewUrl = "DevReview.aspx?tid=" + ticketEntity.ID.ToString();

                                            ReviewName = "DEV Review";
                                        }
                                    }
                                    else
                                    {

                                        ReviewUrl = "DevReview.aspx?tid=" + ticketEntity.ID.ToString();

                                        ReviewName = "DEV Review";
                                    }
                                }
                            }
                            break;
                        }

                    case RolesEnum.Sales:
                        {
                            if (ticketApp.SalerDealState.Find(r => r == ticketEntity.Status) != 0)
                            {

                                if (ticketEntity.Status != TicketsState.Ready_For_Review)
                                {
                                    ReviewUrl = "SalerReview.aspx?tid=" + ticketEntity.ID.ToString();
                                    ReviewName = "Saler Review";
                                }
                                else
                                {
                                    SetReadyforReview();
                                }
                            }
                            break;
                        }
                }
            }
            return resultButtonHTMLBuilder.ToString();
        }

        private void SetReadyforReview()
        {
            if (ticketApp.IsTicketUser(ticketEntity.TicketID, UserInfo.UserID, TicketUsersType.Create))
            {
                isReadyForReview = true;
                UrlApprove = "Approve.aspx?tid=" + ticketEntity.ID + "&returnurl=" + Server.UrlEncode("/SunnetTicket/WaitingResponse.aspx");
                UrlDeny = "Deny.aspx?tid=" + ticketEntity.ID + "&returnurl=" + Server.UrlEncode("/SunnetTicket/WaitingResponse.aspx");
            }
        }

        protected string GetDisplayStatus()
        {
            if (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.Sales || UserInfo.Role == RolesEnum.CLIENT)
            {
                return UserInfo.Role == RolesEnum.CLIENT
                    ? ticketApp.ConvertTicketStateToClientTicketState(ticketEntity.Status).ToText()
                    : ticketEntity.Status.ToText();
            }
            else
            {
                return ticketEntity.Status.ToText();
            }
        }

    }
}