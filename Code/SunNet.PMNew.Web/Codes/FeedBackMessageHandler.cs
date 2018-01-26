using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using System.Text;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Web.Codes
{
    public class FeedBackMessageHandler
    {

        private UsersEntity _user;
        private FeedBackMessagesApplication _fbmApp;
        private List<FeedBackMessagesEntity> _listFbms;

        public List<FeedBackMessagesEntity> FeedBackMessages
        {
            get
            {
                return _listFbms;
            }
        }
        public string FeedBackRequiredTicketIDs
        {
            get
            {
                StringBuilder tickets = new StringBuilder();
                tickets.Append("0,");
                foreach (FeedBackMessagesEntity fbm in _listFbms)
                {
                    if (fbm.WaitClientFeedback > 0)
                    {
                        tickets.Append(fbm.TicketID);
                        tickets.Append(",");
                    }
                }
                return tickets.ToString().TrimEnd(",".ToCharArray());
            }
        }

        //show to client status
        private int[] ClientDisAllowShowStatus()
        {
            int[] DisAllowShowToClientStatus = { 
                                                   (int)ClientTicketState.Draft, 
                                                   (int)ClientTicketState.Cancelled,
                                                   (int)ClientTicketState.Completed};
            return DisAllowShowToClientStatus;
        }
        private int[] ClientAllowShowStatus()
        {
            //allow status
            int[] ClientAllowShowStatus = {
                                              (int)TicketsState.Submitted,      
                                              (int)TicketsState.PM_Reviewed,
                                              (int)TicketsState.Estimation_Fail, 
                                              (int)TicketsState.Estimation_Approved,
                                              (int)TicketsState.Developing,     
                                              (int)TicketsState.Waiting_For_Estimation,
                                              (int)TicketsState.Testing_On_Client,    
                                              (int)TicketsState.Testing_On_Local ,
                                              (int)TicketsState.Tested_Fail_On_Client,   
                                              (int)TicketsState.Tested_Fail_On_Local,
                                              (int)TicketsState.Tested_Success_On_Client,    
                                              (int)TicketsState.Tested_Success_On_Local, 
                                              (int)TicketsState.PM_Deny,         
                                              (int)TicketsState.PM_Verify_Estimation ,
                                              (int)TicketsState.Not_Approved,    
                                              (int)TicketsState.Ready_For_Review,
                                              (int)TicketsState.Waiting_Sales_Confirm,
                                              (int)TicketsState.Completed
                                };
            return ClientAllowShowStatus;
        }
        private int[] UnderDevelopingStatus()
        {
            //developing UnderDevelopingStatus
            int[] UnderDevelopingStatus = {
                                              (int)TicketsState.Testing_On_Client,   
                                              (int)TicketsState.Tested_Success_On_Client,
                                              (int)TicketsState.Tested_Fail_On_Client,  
                                              (int)TicketsState.Tested_Fail_On_Local,
                                              (int)TicketsState.Testing_On_Local,
                                              (int)TicketsState.Tested_Success_On_Local, 
                                              (int)TicketsState.Developing,    
                                              (int)TicketsState.PM_Deny,
                                              (int)TicketsState.Estimation_Approved
                                      };
            return UnderDevelopingStatus;
        }
        private int[] UnderEstimationStatus()
        {
            int[] UnderEstimationStatus = { 
                                              (int)TicketsState.PM_Verify_Estimation, 
                                              (int)TicketsState.Waiting_Sales_Confirm,
                                              (int)TicketsState.Waiting_For_Estimation};
            return UnderEstimationStatus;
        }

        public FeedBackMessageHandler(UsersEntity user)
        {
            _user = user;
            _fbmApp = new FeedBackMessagesApplication();
            _listFbms = _fbmApp.GetList(_user.ID);
        }

        public string FeedBackMessage(object ticketID)
        {
            FeedBackMessagesEntity tmpEntity = _listFbms.Find(r => r.TicketID == (int)ticketID);
            if (tmpEntity != null)
            {
                return string.Format("<a href=\"#\" onclick=\"OpenTicketDetail({0},'f')\" title=\"FeedBack\"><img src=\"/icons/ticket_status.gif\" alt=\"FeedBack\" /></a>"
                    , tmpEntity.TicketID);
            }
            return "";
        }

        public string FeedBackMessage(object ticketID, RolesEnum role)
        {
            FeedBackMessagesEntity tmpEntity = _listFbms.Find(r => r.TicketID == (int)ticketID);
            if (tmpEntity != null)
            {
                if ((role == RolesEnum.PM && tmpEntity.WaitPMFeedback > 0) ||
                    (role == RolesEnum.CLIENT && tmpEntity.WaitClientFeedback > 0))
                {
                    int fid = role == RolesEnum.PM ? tmpEntity.WaitPMFeedback : tmpEntity.WaitClientFeedback;
                    return string.Format("<a href=\"#\" onclick=\"OpenReplyFeedBackDialog({0},{1})\" id=\"feedback{1}\" title=\"Reply FeedBack\"><img src=\"/icons/25.gif\" alt=\"Reply Feedback\" /></a>"
                   , fid, tmpEntity.TicketID);
                }
                else
                {
                    return string.Format("<a href=\"#\" onclick=\"OpenTicketDetail({0},'f')\" title=\"Feedback\"><img src=\"/icons/ticket_status.gif\" alt=\"Feedback\" /></a>"
                   , tmpEntity.TicketID);
                }
            }
            return "";
        }

        public string ShowAction(object ticketID)
        {
            FeedBackMessagesEntity tmpEntity = _listFbms.Find(r => r.TicketID == (int)ticketID);
            if (tmpEntity == null)
            {
                return "";
            }
            return "class ='action'";
        }

        public string GetSunnetStatusNameByStatus(object status, int ticketID)
        {
            FeedBackMessagesEntity tmpEntity = _listFbms.Find(r => r.TicketID == ticketID && r.WaitPMFeedback > 0);
            if (tmpEntity != null)
                return tmpEntity.WaitPMFeedback > 0 ? "<span style='color:red;'>Wait PM Feedback</span>" : "<span style='color:red;'>Wait Client Feedback</span>";
            return status.ToString().Replace('_', ' ');
        }
        public string GetDashboardStatus(object status, int ticketID)
        {
            FeedBackMessagesEntity tmpEntity = _listFbms.Find(r => r.TicketID == ticketID && r.WaitPMFeedback > 0);
            if (tmpEntity != null)
                return tmpEntity.WaitPMFeedback > 0 ? "<span style='color:red;'>Wait PM Feedback</span>" : "<span style='color:red;'>Wait Client Feedback</span>";
            return status.ToString();
        }
        public string GetClientStatusNameBySatisfyStatus(int status, int TicketID)
        {
            try
            {
                FeedBackMessagesEntity tmpEntity = _listFbms.Find(r => r.TicketID == TicketID && r.WaitClientFeedback > 0);
                if (tmpEntity != null)
                    return "<span style='color:red;'>Wait Feedback</span>";
                if (status == (int)TicketsState.Ready_For_Review)
                {
                    return "<span style='color:red;'>Ready For Review</span>";
                }
                if (UnderDevelopingStatus().Contains(status))
                {
                    return "In Progress";
                }
                else if (UnderEstimationStatus().Contains(status))
                {
                    return "Estimating";
                }
                return Enum.GetName(typeof(TicketsState), status).Replace('_', ' ');
            }
            catch (Exception ex)
            {
                WebLogAgent.Write("Excepted Exception in FeedBackMessageHandler.GetClientStatusNameBySatisfyStatus(" + status.ToString() + "," + TicketID.ToString() + ")\n\r" + ex.Message);
                return "Expected.";
            }
        }
        public List<TicketsState> GetSearchTicketStatuses(int clientStatusValue)
        {
            if (clientStatusValue < 0)
            {
                return GetAllSearchticketStatuses();
            }
            ClientTicketState clientStatus = (ClientTicketState)clientStatusValue;
            List<TicketsState> list = new List<TicketsState>();
            switch (clientStatus)
            {
                case ClientTicketState.Draft:
                    list.Add(TicketsState.Draft);
                    break;
                case ClientTicketState.Cancelled:
                    list.Add(TicketsState.Cancelled);
                    break;
                case ClientTicketState.Submitted:
                    list.Add(TicketsState.Submitted);
                    break;
                case ClientTicketState.Estimating:
                    foreach (int s in UnderEstimationStatus())
                    {
                        list.Add((TicketsState)s);
                    }
                    break;
                case ClientTicketState.Estimation_Fail:
                    list.Add(TicketsState.Estimation_Fail);
                    break;
                case ClientTicketState.In_Progress:
                    foreach (int s in UnderDevelopingStatus())
                    {
                        list.Add((TicketsState)s);
                    }
                    break;
                case ClientTicketState.Waiting_Feedback:
                    break;
                case ClientTicketState.Ready_For_Review:
                    list.Add(TicketsState.Ready_For_Review);
                    break;
                case ClientTicketState.Not_Approved:
                    list.Add(TicketsState.Not_Approved);
                    break;
                case ClientTicketState.Completed:
                    list.Add(TicketsState.Completed);
                    break;
                default: break;
            }
            return list;
        }
        public List<TicketsState> GetAllSearchticketStatuses()
        {
            List<TicketsState> list = new List<TicketsState>();
            foreach (int s in ClientAllowShowStatus())
            {
                list.Add((TicketsState)s);
            }
            return list;
        }
    }
}
