using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.UserControls.Ticket
{
    public partial class TicketBasicInfo : BaseAscx
    {
        public TicketsEntity TicketsEntity { get; set; }
        private TicketsApplication ticketsApplication = new TicketsApplication();
        private UserApplication userApp = new UserApplication();
        public bool FromSunnet { get; set; }

        public string ReviewUrl = "";
        public string ReviewName = "";

        public bool isReadyForReview = false;
        public string UrlApprove = "";
        public string UrlDeny = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (TicketsEntity != null)
                {
                    GetButtonsHTML();

                    this.fileUpload.TicketID = TicketsEntity.TicketID;
                    TicketUsersEntity ticketUser = ticketsApplication.GetTicketUser(TicketsEntity.TicketID, UserInfo.ID);
                    if (ticketUser != null)
                        ltlStatus.Text = ticketUser.WorkingOnStatus.ToText();
                    if (ticketUser == null || UserInfo.Role == RolesEnum.CLIENT)
                        phlWorkingOn.Visible = false;
                }
            }
            TicketsEntity.CreatedUserEntity = userApp.GetUser(TicketsEntity.CreatedBy);
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
            TicketsState currentTicketState = TicketsEntity.Status;

            if (ticketsApplication.IsTicketUser(TicketsEntity.ID, UserInfo.UserID))
            {
                switch (UserInfo.Role)
                {
                    case RolesEnum.PM:
                        {

                            ReviewUrl = "PMReview.aspx?tid=" + TicketsEntity.ID.ToString() + "&pid=" + TicketsEntity.ProjectID.ToString();
                            ReviewName = "PM Review";
                            break;
                        }
                    case RolesEnum.CLIENT:
                        {
                            TicketUsersEntity ticketUsersEntity = ticketsApplication.GetTicketUser(TicketsEntity.TicketID, UserInfo.UserID);

                            if (ticketsApplication.ClientDealState.Find(r => r == TicketsEntity.Status) != 0 &&
                                ticketUsersEntity != null && ticketUsersEntity.Type == TicketUsersType.Create)
                            {
                                SetReadyforReview();
                            }
                            break;
                        }

                    case RolesEnum.QA:
                        {
                            if (ticketsApplication.QaDealState.Find(r => r == TicketsEntity.Status) != 0)
                            {
                                if (TicketsEntity.Status == TicketsState.Waiting_For_Estimation)
                                {

                                    ReviewUrl = "QAReview.aspx?tid=" + TicketsEntity.ID.ToString();
                                    ReviewName = "QA Review";
                                }
                                else
                                {
                                    if (TicketsEntity.Status == TicketsState.Ready_For_Review)
                                    {
                                        SetReadyforReview();
                                    }
                                    else
                                    {
                                        ReviewUrl = "QAReview.aspx?tid=" + TicketsEntity.ID.ToString();

                                        ReviewName = "QA Review";
                                    }

                                }
                            }
                            break;
                        }

                    case RolesEnum.Leader:
                    case RolesEnum.DEV:
                        {
                            if (ticketsApplication.DevDealState.Find(r => r == TicketsEntity.Status) != 0)
                            {
                                if (TicketsEntity.Status == TicketsState.Waiting_For_Estimation)
                                {
                                    if (TicketsEntity.EsUserID == UserInfo.UserID)
                                    {
                                        ReviewUrl = "DevReview.aspx?tid=" + TicketsEntity.ID.ToString();

                                        ReviewName = "DEV Review";
                                    }
                                }
                                else
                                {
                                    if (UserInfo.Role == RolesEnum.Leader)
                                    {
                                        if (TicketsEntity.Status == TicketsState.Ready_For_Review)
                                        {
                                            SetReadyforReview();
                                        }
                                        else
                                        {
                                            ReviewUrl = "DevReview.aspx?tid=" + TicketsEntity.ID.ToString();

                                            ReviewName = "DEV Review";
                                        }
                                    }
                                    else
                                    {

                                        ReviewUrl = "DevReview.aspx?tid=" + TicketsEntity.ID.ToString();

                                        ReviewName = "DEV Review";
                                    }
                                }
                            }
                            break;
                        }

                    case RolesEnum.Sales:
                        {
                            if (ticketsApplication.SalerDealState.Find(r => r == TicketsEntity.Status) != 0)
                            {

                                if (TicketsEntity.Status != TicketsState.Ready_For_Review)
                                {
                                    ReviewUrl = "SalerReview.aspx?tid=" + TicketsEntity.ID.ToString();
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
            if (ticketsApplication.IsTicketUser(TicketsEntity.TicketID, UserInfo.UserID, TicketUsersType.Create))
            {
                isReadyForReview = true;
                UrlApprove = "Approve.aspx?tid=" + TicketsEntity.ID.ToString();
                UrlDeny = "Deny.aspx?tid=" + TicketsEntity.ID.ToString();
            }
        }

        protected string GetDisplayStatus()
        {
            if (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.Sales || UserInfo.Role == RolesEnum.CLIENT)
            {
                return UserInfo.Role == RolesEnum.CLIENT
                    ? ticketsApplication.ConvertTicketStateToClientTicketState(TicketsEntity.Status).ToText()
                    : TicketsEntity.Status.ToText();
            }
            else
            {
                return TicketsEntity.Status.ToText();
            }
        }
    }
}