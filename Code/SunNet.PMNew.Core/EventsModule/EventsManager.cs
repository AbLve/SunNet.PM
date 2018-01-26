using SunNet.PMNew.Core.EventsModule;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Entity.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.Core.EventsModule
{
    public class EventsManager : BaseMgr
    {
        private IEventRepository eventRepository;
        private IEventCommentsRepository eventCommentsRepository;

        public EventsManager(IEventRepository eventRepository, IEventCommentsRepository eventCommentsRepository)
        {
            this.eventRepository = eventRepository;
            this.eventCommentsRepository = eventCommentsRepository;
        }

        /// <summary>
        /// 删除event 并且将权限与邀请都删除
        /// </summary>
        public bool Delete(int entityId)
        {
            return eventRepository.Delete(entityId);
        }

        public EventEntity Get(int id)
        {
            return eventRepository.Get(id);
        }

        public EventEntity GetEventByCreateId(int creatId)
        {
            return eventRepository.GetEventByCreateId(creatId);
        }

        /// <summary>
        /// 大于等于 startDate 小于 endDate
        /// 没有数据时，list.count == 0 
        /// </summary>
        public List<EventEntity> GetEvents(int currentUserId, DateTime startDate, DateTime endDate, int userId, string allUser, int projectID)
        {
            return eventRepository.GetEvents(currentUserId, startDate, endDate, userId, allUser, projectID);
        }

        public List<EventEntity> GetEvents(DateTime startDate, int userId, int projectID, int top)
        {
            return eventRepository.GetEvents(startDate, userId, projectID, top);
        }

        /// UserId 为 零时，表示所有,使用 allUser 参数
        public List<EventEntity> GetEvents(int currentUserId, DateTime startDate, int userId, string allUser, int projectID, int pageSize, int pageNo, out int recordCount)
        {
            return eventRepository.GetEvents(currentUserId, startDate, userId, allUser, projectID, pageSize, pageNo, out recordCount);
        }

        ///// <summary>
        ///// 0:失败 ;1:邀请; 2:加入; 3:拒绝; 4:忽略
        ///// </summary>
        //public int GetStatus(int userId, int eventId)
        //{
        //    return eventRepository.GetStatus(userId, eventId);
        //}

        public int AddEvents(EventEntity entity, int Times, bool isOff)
        {
            entity.Times = Times;
            entity.IsOff = isOff;
            return eventRepository.Insert(entity);
        }

        public bool EditEvents(EventEntity entity)
        {
            return eventRepository.Update(entity);
        }

        #region EventShares
        #endregion EventShares

        /// <summary>
        /// 添加Event的邀请者
        /// </summary>
        /// <param name="createdId">Event创建者</param>
        /// <param name="invites">被邀请者列表</param>
        /// <param name="date">Event 的活动时间</param>
        /// <returns></returns>
        public bool AddEventInvites(EventEntity entity, List<EventInviteEntity> inviteList)
        {
            return eventRepository.AddEventInvites(entity, inviteList);
        }

        public List<EventInviteEntity> GetEventInvites(int eventId)
        {
            return eventRepository.GetEventInvites(eventId);
        }

        /// <summary>
        /// 删除 UserId= 0 ; 或者 UserId > 0 and optionStatu =1
        /// </summary>
        /// <param name="inviteList"></param>
        /// <returns></returns>
        public bool RemoveInviteUser(List<EventInviteEntity> inviteList)
        {
            return eventRepository.RemoveInviteUser(inviteList);
        }

        #region IEventCommentsDAO

        #endregion IEventCommentsDAO


        public bool DeleteAll(int createdBy, DateTime createdOn, DateTime fromDate)
        {
            return eventRepository.DeleteAll(createdBy, createdOn, fromDate);
        }

        /// <summary>
        /// 获取可更新和删除的Event的id集合
        /// </summary>
        /// <param name="createdBy"></param>
        /// <param name="createdOn"></param>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public DataSet GetUpdateAndDeleteEvents(int createdBy, DateTime createdOn, DateTime fromDate)
        {
            return eventRepository.GetUpdateAndDeleteEvents(createdBy, createdOn, fromDate);
        }

        public DataTable QueryReportDetailsByProject(int projectID, int userID, DateTime startDate
            , DateTime endDate, string orderBy, string orderDirectioin)
        {
            this.ClearBrokenRuleMessages();
            DataTable dt = eventRepository.QueryReportDetailsByProject(projectID, userID, startDate, endDate, orderBy, orderDirectioin);
            if (dt == null)
                this.AddBrokenRuleMessage();
            return dt;
        }

        public DataTable QueryReportTotalHoursByProject(int projectID, int userID, DateTime startDate
            , DateTime endDate, string orderBy, string orderDirectioin)
        {
            this.ClearBrokenRuleMessages();
            DataTable dt = eventRepository.QueryReportTotalHoursByProject(projectID, userID, startDate, endDate, orderBy, orderDirectioin);
            if (dt == null)
                this.AddBrokenRuleMessage();
            return dt;
        }

        public DataTable GetPtoByProjectUser(int projectID, int userID, DateTime startDate, DateTime endDate)
        {
            this.ClearBrokenRuleMessages();
            if (projectID <= 0 || userID <= 0)
            {
                return new DataTable();
            }
            DataTable dt = eventRepository.GetPtoByProjectUser(projectID, userID, startDate, endDate);
            if (dt == null)
            {
                this.AddBrokenRuleMessage();
            }
            return dt;
        }
        #region
        /// <summary>
        /// insert     
        /// </summary>
        /// <param name="workTimeEntitie"></param>
        /// <returns></returns>
        public int AddWorkTime(WorkTimeEntity workTimeEntitie)
        {
            return eventRepository.AddWorkTime(workTimeEntitie);
        }


        public List<WorkTimeEntity> GetWorkTime(int userId)
        {
            return eventRepository.GetWorkTime(userId);
        }

        public bool DeleteWorkTimeByUserId(int userId)
        {
            return eventRepository.DeleteWorkTimeByUserId(userId);
        }
        #endregion


    }
}
