using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.UserModel.UserModel;

namespace SunNet.PMNew.Core.TicketModule
{
    public interface ITicketsUserRepository : IRepository<TicketUsersEntity>
    {
        List<TicketUsersEntity> GetListUsersByTicketId(int tid);
        List<SelectUserModel> GetSelectUsersByTicketId(int tid);

        List<TicketUsersEntity> GetListByUserId(int userId);

        List<TicketDistinctUsersResponse> GetListDistinctUsersByTicketId(int ticketId);

        List<int> GetTicketUserId(int ticketId, List<int> userIds);

        List<TicketUsersEntity> GetTicketUserList(int ticketId);

        void UpdateTicketPM(int OldPMId, int NewPmId, int ProjectID);

        bool IsTicketUser(int tid, int uid, List<TicketUsersType> types);

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="user">用户ID，如果要对该Ticket的所有关联用户生效，请传入0.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/28 01:40
        bool UpdateWorkingOnStatus(int ticket, int user, TicketUserStatus status);

        TicketUsersEntity Get(int ticket, int user);

        void UpdateTicketUserType(int userID, TicketUsersType type, int ticketID);

        List<TicketUsersEntity> GetTicketUser(int ticketID, TicketUsersType type);

        void UpdateCreateUser(int newClientID, int ticketID);

        bool Delete(int ticketId, List<int> users, List<TicketUsersType> types);

        /// <summary>
        /// 更新指定的Ticket,显示/清除气泡通知给指定类型用户
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="showNtfctn">显示或清除通知.</param>
        /// <param name="users">The users.</param>
        /// <param name="types">指定类型的用户将显示气泡通知.</param>
        /// <returns></returns>
        bool UpdateNotification(int ticketId, bool showNtfctn, List<int> users, List<TicketUsersType> types);

        /// <summary>
        /// 产生气泡，通知其它用户
        /// </summary>
        bool CreateNotification(int ticketId, int userId, bool notTificationClient=true);

        /// <summary>
        /// 更新指定的Ticket,覆盖指定范围用户的Ticket状态(自动设置显示气泡通知).
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="users">The users.</param>
        /// <returns></returns>
        bool UpdateTicketStatus(int ticketId, UserTicketStatus status, List<int> users, List<TicketUsersType> types);

        bool UpdateTicketStatus(int ticketId, UserTicketStatus status);

        /// <summary>
        /// 回复Feedback,清除自己的状态提示,并检查制定类型范围内还有多少人没有处理
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>返回是否所有人都已经回复</returns>
        bool TryClearWaiting(int ticketId, int userId, List<TicketUsersType> types);

        bool ClearWaitingByType(int ticketId, List<TicketUsersType> types);


        List<String> GetUsersWithStatus(int ticketId, UserTicketStatus status, List<TicketUsersType> types);
    }
}
