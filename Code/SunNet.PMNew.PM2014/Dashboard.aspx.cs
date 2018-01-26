using System;
using System.Collections.Generic;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;
using System.Linq;
using SunNet.PMNew.PM2014.Event;

namespace SunNet.PMNew.PM2014
{
    public partial class Dashboard : BasePage
    {
        private Dictionary<string, int> _ticketCount;
        TicketsApplication ticketAPP = new TicketsApplication();

        /// <summary>
        /// Ticket数量：key - waitting | cancelled | ongoing | drafted | completed | report.
        /// </summary>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/23 12:09
        public Dictionary<string, int> Tickets
        {
            get
            {
                if (_ticketCount == null)
                    _ticketCount = new Dictionary<string, int>();
                if (_ticketCount.Count == 0)
                {
                    if (UserInfo.Role == RolesEnum.ADMIN || UserInfo.Role == RolesEnum.PM)
                    {
                        _ticketCount.Add("waitting", 0);
                        _ticketCount.Add("ongoing",
                                ticketAPP.GetOngoingTicketsCount(UserInfo.UserID, 0, 0, ClientTicketState.None,
                                    TicketsType.None, string.Empty, true));
                        _ticketCount.Add("drafted",
                            ticketAPP.GetDraftedTicketsCount(UserInfo.UserID, 0, 0, TicketsType.None, string.Empty, true));
                    }
                    else
                    {
                        _ticketCount.Add("waitting", ticketAPP.GetWaitingforResponseCount(UserInfo.UserID, 0, UserInfo.CompanyID, ClientTicketState.None, TicketsType.None, string.Empty));
                        _ticketCount.Add("ongoing",
                              ticketAPP.GetOngoingTicketsCount(UserInfo.UserID, 0, UserInfo.CompanyID, ClientTicketState.None, TicketsType.None, string.Empty, true));
                        _ticketCount.Add("drafted",
                             ticketAPP.GetDraftedTicketsCount(UserInfo.UserID, 0, UserInfo.CompanyID, TicketsType.None, string.Empty, true));
                    }
                }
                return _ticketCount;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<UsersEntity> userList = new App.ProjectApplication().GetProjectUsersByUserId(UserInfo);
                userList = userList.Distinct(new CompareUser()).ToList();
                hiUserIds.Value = string.Join(",", userList.Select(r => r.UserID).ToArray());
            }
        }
    }
}