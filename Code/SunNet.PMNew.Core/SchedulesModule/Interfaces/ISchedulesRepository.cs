using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.SchedulesModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.SchedulesModule.Interfaces
{
    public interface ISchedulesRepository : IRepository<SchedulesEntity>
    {
        /// <summary>
        /// 获取用户指定日期的Schdules
        /// </summary>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<SchedulesEntity> GetSchedules(DateTime date, int userId);

        /// <summary>
        /// 获取用户指定日期范围内的Schduels
        /// </summary>
        /// <param name="startDate"> >= startDate</param>
        /// <param name="endDate"> < endDate </param>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<SchedulesEntity> GetSchedules(DateTime startDate, DateTime endDate, int userId);

        /// <summary>
        /// 根据会议ID ，获取计划参会人员
        /// </summary>
        /// <param name="meetingID"></param>
        /// <returns></returns>
        List<UsersEntity> GetMeetingUsers(string meetingID);

        /// <summary>
        ///  取消参会资格
        /// </summary>
        bool VoteMeeting(string meetingId, string[] userIds, DateTime dateTime);

        /// <summary>
        /// 不同意会议
        /// </summary>
        bool VoteMeeting(string meetingId, DateTime updateDate, int updateUser);

        /// <summary>
        /// 同意会议
        /// </summary>
        bool AgreeMeeting(string meetingId, DateTime updateDate, int updateUser);

        /// <summary>
        /// 修改会议内容
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdateMeeting(SchedulesEntity entity);

        bool DeleteMeetingSchedule(string meetingId);
    }
}
