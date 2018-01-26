using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		4/21 9:53:14
 * Description:		Please input class summary
 * Version History:	Created,4/21 9:53:14
 * 
 * 
 **************************************************************************/
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.PM2014.Codes
{
    public enum FeedBackType
    {
        /// <summary>
        /// 没有feedback
        /// </summary>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/25 11:33
        None = 0,
        /// <summary>
        /// 需要回复
        /// </summary>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/25 11:33
        NeedReply = 1,
        /// <summary>
        /// 普通通知，仅需要查看
        /// </summary>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/25 11:34
        Normal = 2
    }

    public class FeedBackMessageHandler
    {
        private UsersEntity _user;

        public FeedBackMessageHandler(UsersEntity user)
        {
            _user = user;
        }

        public string GetSunnetStatusNameByStatus(object status, RolesEnum userRole)
        {
            var ticketStatus = (TicketsState)status;
            if (userRole == RolesEnum.PM || userRole == RolesEnum.ADMIN || userRole == RolesEnum.Sales)
            {
                if (ticketStatus == TicketsState.Wait_Client_Feedback ||
                    ticketStatus == TicketsState.Wait_PM_Feedback ||
                    ticketStatus == TicketsState.Wait_Sunnet_Feedback)
                {
                    return "<span style='color:red;'>" + ticketStatus.ToText() + "</span>";
                }
                else
                {
                    return ticketStatus.ToText();
                }
            }
            else
            {
                return ticketStatus.ToText();
            }
        }

        /// <summary>
        /// Gets the client status name by satisfy status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="TicketID">The ticket identifier.</param>
        /// <param name="withFormat">是否带格式，页面显示需要带格式，导出报表不需要.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  6/4 20:27
        public string GetClientStatusNameBySatisfyStatus(int status, int TicketID, bool withFormat = true)
        {
            try
            {
                TicketsState state = (TicketsState)status;
                string format = withFormat ? "<span style='color:red;'>{0}</span>" : "{0}";
                if (state == TicketsState.Wait_Sunnet_Feedback
                    || state == TicketsState.Wait_Client_Feedback
                    || state == TicketsState.Ready_For_Review
                    )
                {
                    return string.Format(format, state.ToText());
                }
                if (TicketsStateHelper.UnderDevelopingStatus.Contains(state))
                {
                    return "In Progress";
                }
                else if (TicketsStateHelper.UnderEstimationStatus.Contains(state))
                {
                    return "Estimating";
                }
                return status.ToString().ToEnum<TicketsState>().ToText();
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
                return TicketsStateHelper.ClientAllowShowStatus;
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
                    list.AddRange(TicketsStateHelper.UnderEstimationStatus);
                    break;
                case ClientTicketState.Denied:
                    list.Add(TicketsState.Denied);
                    break;
                case ClientTicketState.In_Progress:
                    list.AddRange(TicketsStateHelper.UnderDevelopingStatus);
                    break;
                case ClientTicketState.Waiting_Client_Feedback:
                    list.Add(TicketsState.Wait_Client_Feedback);
                    break;
                case ClientTicketState.Waiting_Sunnet_Feedback:
                    list.Add(TicketsState.Wait_Sunnet_Feedback);
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
    }
}