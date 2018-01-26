using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Entity.TicketModel.TicketsDTO;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.TicketModel;
using System.Data;

namespace SunNet.PMNew.Core.TicketModule
{
    public interface ITicketsRepository : IRepository<TicketsEntity>
    {
        int GetCompanyIdByTicketId(int tid);

        GetProjectIdAndUserIDResponse GetProjectIdAndUserID(int ticketId);

        // jack
        SearchTicketsResponse SearchTickets(SearchTicketsRequest request);
        List<TicketsEntity> SearchTickets(SearchTicketCondition condition);
        List<TicketsEntity> SearchTicketsNotInTid(SearchTicketCondition condition);

        //es time
        bool UpdateEs(decimal time, int tid, bool IsFinal);

        bool DeleteTicketEs(int ticketID, bool isDeleteInital, bool isDeleteFinal);
        /// <summary>
        /// Set Ticket's star(-1=Tickets not existed,0=fail,others=success)
        /// </summary>
        /// <param name="ticketID"></param>
        /// <param name="star"></param>
        /// <returns></returns>
        int Update(int ticketID, int star);

        bool UpdateStatus(int ticketId, TicketsState state);

        bool UpdateIsRead(int ticketId, TicketIsRead isRead);

        TicketsEntity GetTicketWithProjectTitle(int ticketID);

        #region 报表
        DataTable SearchReortTickets(SearchTicketsRequest request, out int totalRows);
        DataTable ReortTicketRating(SearchTicketsRequest request, out int totalRows);

        #endregion

        bool Update(TicketsEntity ticket, bool isUpdateStatus);

        List<TicketsEntity> GetTicketsByWorkingStatus(int userid, TicketUserStatus status);
        List<TicketsEntity> GetTicketsByCreateId(int createid);

        bool UpdateConfirmEstmateUserId(int ticketId, int userId);

        List<TicketsEntity> GetTicketsByIds(List<int> ticketIds);
    }
}
