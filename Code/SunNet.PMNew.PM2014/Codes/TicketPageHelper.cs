using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.App;
using System.Text;

namespace SunNet.PMNew.PM2014.Codes
{
    public class TicketPageHelper : BasePage
    {
        protected FeedBackMessageHandler fbmHandler;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            fbmHandler = new FeedBackMessageHandler(UserInfo);
        }

        private TicketsApplication ticketsApplication = new TicketsApplication();

        public string FeedBackButtonOrExpanded(object ticketData, string returnUrl)
        {
            var ticket = (TicketsEntity)ticketData;
            int id = ticket.TicketID;
            string feedbackHtml = @"<a  class='collapsed message' href='###' >
                                        <img src='/Images/icons/message.png'  title='New unread feedback'  width='20' height='20' />
                                    </a>";
            string plusButtonHtml = @"<a href='###' ticket={0} class='collapsed'>
                                        <!-- collapsed expanded <img src='/Images/icons/packup.png'  />-->
                                        &nbsp;
                                    </a>";
            string ticketDetailHtml = @"<a href='Detail.aspx?tid={0}&returnurl={1}#lastFeedback' ticket={0} target='_blank'>
                                        <img src='/Images/icons/message.png'  title='Wait feedback'  width='20' height='20' />
                                    </a>";

            //if (ticket.Status == TicketsState.Wait_Client_Feedback && UserInfo.UserType == "CLIENT"
            //    || ticket.Status == TicketsState.Wait_Sunnet_Feedback && UserInfo.UserType == "SUNNET")
            //    return string.Format(ticketDetailHtml, ticket.ID, returnUrl);
            //else 
            if (ticket.ShowNotification)
                return string.Format(ticketDetailHtml, ticket.ID, returnUrl);
            else
                return string.Format(plusButtonHtml, ticket.ID);
        }

        public string GetStatus(object status)
        {
            return fbmHandler.GetSunnetStatusNameByStatus(status, UserInfo.Role);
        }

        public string GetClientStatusNameBySatisfyStatus(int status, int ticketID, bool withFormat = true)
        {
            return fbmHandler.GetClientStatusNameBySatisfyStatus(status, ticketID, withFormat);
        }

        /// <summary>
        /// Gets the action HTML.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="ticketID">The ticket identifier.</param>
        /// <param name="isEstimate">The is estimate.</param>
        /// <param name="esUerID">The es uer identifier.</param>
        /// <param name="buttonType">onlyIcon,iconText.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  6/19 04:39
        public string GetActionHTML( object projectID,object status, object ticketID, object isEstimate, object esUerID, object createdBy, int confirmEstmateUserId, string buttonType = "onlyIcon", string className = "")
        {
            string PMReview = "<a href=\"PMReview.aspx?tid={0}&pid={1}\" data-toggle='modal' data-target='#modalsmall' title='Review'><img src='/Images/icons/pmreview.png' alt='View' /></a>";
            string DevReview = "<a href=\"DevReview.aspx?tid={0}\" data-toggle='modal' data-target='#modalsmall' title='Review'><img src='/Images/icons/pmreview.png' alt='View' /></a>";
            string QAReview = "<a href=\"QAReview.aspx?tid={0}\" data-toggle='modal' data-target='#modalsmall' title='Review'><img src='/Images/icons/pmreview.png' alt='View' /></a>";
            string SalterReview = "<a href=\"SalerReview.aspx?tid={0}\" data-toggle='modal' data-target='#modalsmall' title=\"Saler Review\"><img src='/Images/icons/pmreview.png' alt='View' /></a>";
            if (buttonType == "iconText")
            {
                PMReview = "<a href=\"PMReview.aspx?tid={0}&pid={1}\" data-toggle='modal' data-target='#modalsmall' class='" + className + "' title='Review'>Review</a>";
                DevReview = "<a href=\"DevReview.aspx?tid={0}\" data-toggle='modal' data-target='#modalsmall' class='" + className + "'  title='Review'>Review</a>";
                QAReview = "<a href=\"QAReview.aspx?tid={0}\" data-toggle='modal' data-target='#modalsmall' class='" + className + "'  title='Review'>Review</a>";
                SalterReview = "<a href=\"SalerReview.aspx?tid={0}\" data-toggle='modal' data-target='#modalsmall' class='" + className + "'  title=\"Saler Review\">Review</a>";
            }
            var ticketState = (TicketsState)status;

            string result = string.Empty;
            if (ticketState != TicketsState.Ready_For_Review)
            {
                if (UserInfo.Role == RolesEnum.PM)
                {
                    result = string.Format(PMReview, ticketID.ToString(), projectID.ToString());
                }
                else if (UserInfo.Role == RolesEnum.DEV || UserInfo.Role == RolesEnum.Leader)
                {
                    if (ticketsApplication.DevDealState.Find(r => r == ticketState) != 0)
                    {
                        if (ticketState == TicketsState.Waiting_For_Estimation)
                        {
                            result = (bool)isEstimate && (int)esUerID == UserInfo.UserID
                                ? string.Format(DevReview, ticketID.ToString())
                                : "";
                        }
                        else
                            result = string.Format(DevReview, ticketID.ToString());
                    }
                    else
                        result = "";
                }
                else if (UserInfo.Role == RolesEnum.QA)
                {
                    if (ticketsApplication.QaDealState.Find(r => r == ticketState) != 0)
                    {
                        if (ticketState == TicketsState.Waiting_For_Estimation)
                        {
                            if ((bool)isEstimate && (int)esUerID == UserInfo.UserID)
                            {
                                result = string.Format(QAReview, ticketID.ToString());
                            }
                            else
                            {
                                result = "";
                            }
                        }
                        else
                        {
                            result = string.Format(QAReview, ticketID.ToString());
                        }
                    }
                    else
                    {
                        result = "";
                    }
                }
                else if (UserInfo.Role == RolesEnum.Sales)
                {
                    if (ticketsApplication.SalerDealState.Find(r => r == ticketState) != 0 && confirmEstmateUserId == UserInfo.UserID)
                    {
                        result = string.Format(SalterReview, ticketID.ToString());
                    }
                    else
                    {
                        result = "";
                    }
                }
                else
                {
                    result = "";
                }
            }
            return result + GetAction((int)projectID,(int)ticketID, ticketState, (int)createdBy, PMReview);
        }


        protected string GetAction(int projectID,int ticketID, TicketsState ticketsState, int createdBy, string pMReview)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (ticketsState == TicketsState.Ready_For_Review)
            {

                if (ticketsApplication.IsTicketUser(ticketID, UserInfo.UserID, TicketUsersType.Create)
                    || createdBy == UserInfo.UserID)
                {

                    if (UserInfo.Role != RolesEnum.ADMIN && UserInfo.Role != RolesEnum.PM)
                    {
                        stringBuilder.Append("&nbsp;<a href=\"Approve.aspx?tid=" + ticketID.ToString()
                            + "&returnurl="+Server.UrlEncode(Request.RawUrl)
                            + "\" data-target=\"#modalsmall\" data-toggle=\"modal\"><img src=\"/Images/icons/approve.png\" title=\"Approve\"></a>");
                        stringBuilder.Append("&nbsp;<a href=\"Deny.aspx?tid=" + ticketID.ToString()
                            + "&returnurl=" + Server.UrlEncode(Request.RawUrl)
                            + "\" data-target=\"#modalsmall\" data-toggle=\"modal\"><img src=\"/Images/icons/deny.png\" title=\"Deny\"></a>");
                    }
                    else
                    {
                        stringBuilder.AppendFormat(pMReview, ticketID.ToString(), projectID.ToString());
                    }
                }
                else
                {
                    if (UserInfo.Role == RolesEnum.ADMIN || UserInfo.Role == RolesEnum.PM)
                    {
                        stringBuilder.AppendFormat(pMReview, ticketID.ToString(),projectID.ToString());
                    }
                }

            }
            return stringBuilder.ToString();
        }
    }
}