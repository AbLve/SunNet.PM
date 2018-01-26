using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Framework.Core.Repository;
using System.Data;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.EventsModule
{
    public interface IEventRepository : IRepository<EventEntity>
    {
        EventEntity GetEventByCreateId(int creatId);
        List<EventEntity> GetEvents(int currentUserId, DateTime startDate, DateTime endDate, int userId, string allUser, int projectID);
        List<EventEntity> GetEvents(DateTime startDate, int userId, int projectID, int top);
        List<EventEntity> GetEvents(int currentUserId, DateTime startDate, int userId, string allUser, int projectID, int pageSize, int pageNo, out int recordCount);


        #region EventInvites
        List<EventInviteEntity> GetEventInvites(int eventId);
        bool AddEventInvites(EventEntity entity, List<EventInviteEntity> inviteList);
        int GetStatus(int userId, int eventId);
        /// <summary>
        /// 删除 UserId= 0 ; 或者 UserId > 0 and optionStatu =1
        /// </summary>
        /// <param name="inviteList"></param>
        /// <returns></returns>
        bool RemoveInviteUser(List<EventInviteEntity> inviteList);
        #endregion

        #region
        int AddWorkTime(WorkTimeEntity workTimeEntitie);
        List<WorkTimeEntity> GetWorkTime(int userId);
        bool DeleteWorkTimeByUserId(int userId);
        #endregion

        bool DeleteAll(int createdBy, DateTime createdOn, DateTime fromDate);

        DataSet GetUpdateAndDeleteEvents(int createdBy, DateTime createdOn, DateTime fromDate);

        DataTable QueryReportTotalHoursByProject(int projectID, int userID, DateTime startDate,
            DateTime endDate, string orderBy, string orderDirectioin);
        DataTable QueryReportDetailsByProject(int projectID, int userID, DateTime startDate,
            DateTime endDate, string orderBy, string orderDirectioin);

        DataTable GetPtoByProjectUser(int projectID, int userID, DateTime startDate,
            DateTime endDate);

    }
}
