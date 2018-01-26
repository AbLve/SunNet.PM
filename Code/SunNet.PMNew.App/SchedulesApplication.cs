using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Core.SchedulesModule;
using StructureMap;
using SunNet.PMNew.Core.SchedulesModule.Interfaces;
using SunNet.PMNew.Entity.SchedulesModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.App
{
    public class SchedulesApplication : BaseApp
    {
        private SchedulesManager mgr;
        public SchedulesApplication()
        {
            mgr = new SchedulesManager(
                 ObjectFactory.GetInstance<IEmailSender>(),
                                    ObjectFactory.GetInstance<ISchedulesRepository>()
                                    );
        }

        public SchedulesEntity GetInfo(int id)
        {
            return mgr.GetInfo(id);
        }

        public int Add(SchedulesEntity entity)
        {
            return mgr.Add(entity);
        }

        public int Add(SchedulesEntity entity, List<UsersEntity> userList, UsersEntity currentUser)
        {
            this.ClearBrokenRuleMessages();
            int id = mgr.Add(entity, userList, currentUser);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }

        public bool Update(SchedulesEntity entity)
        {
            return mgr.Update(entity);
        }

        /// <summary>
        /// 修改会议
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="moveUsers">取消参加会议资格的人员</param>
        /// <param name="newUsers">新增加的会议人员</param>
        /// <returns></returns>
        public bool Update(SchedulesEntity entity, List<UsersEntity> moveUsers, List<UsersEntity> newUsers)
        {
            return mgr.Update(entity, moveUsers, newUsers);
        }


        /// <summary>
        /// 获取用户指定日期的Schdules
        /// </summary>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SchedulesEntity> GetSchedules(DateTime date, int userId)
        {
            return mgr.GetSchedules(date, userId);
        }

        /// <summary>
        /// 获取用户指定日期范围内的Schduels
        /// </summary>
        /// <param name="startDate"> >= startDate</param>
        /// <param name="endDate"> < endDate </param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SchedulesEntity> GetSchedules(DateTime startDate, DateTime endDate, int userId)
        {
            return mgr.GetSchedules(startDate, endDate, userId);
        }

        /// <summary>
        /// 根据会议ID ，获取计划参会人员
        /// </summary>
        /// <param name="meetingID"></param>
        /// <returns></returns>
        public List<UsersEntity> GetMeetingUsers(string meetingID)
        {
            return mgr.GetMeetingUsers(meetingID);
        }

        /// <summary>
        /// 不同意会议
        /// </summary>
        public bool VoteMeeting(SchedulesEntity schedulesEntity, DateTime updateDate, UsersEntity updateUser)
        {
            //Save the meeting users before delete the users.
            List<UsersEntity> meetingMailUsers = mgr.GetMeetingUsersExceptCurrent(schedulesEntity.MeetingID, updateUser.UserID);
            if (mgr.VoteMeeting(schedulesEntity.MeetingID, updateDate, updateUser.ID))
            {
                mgr.SendMeetingEmailToUsersExceptCurrent(schedulesEntity, meetingMailUsers, "was cancelld");
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 同意会议
        /// </summary>
        public bool AgreeMeeting(SchedulesEntity schedulesEntity, DateTime updateDate, UsersEntity updateUser)
        {
            if (mgr.AgreeMeeting(schedulesEntity.MeetingID, updateDate, updateUser.ID))
            {
                List<UsersEntity> meetingMailUsers = mgr.GetMeetingUsersExceptCurrent(schedulesEntity.MeetingID, updateUser.UserID);
                mgr.SendMeetingEmailToUsersExceptCurrent(schedulesEntity, meetingMailUsers, "was agreed");
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(int entityId)
        {
            return mgr.Delete(entityId);
        }

        public bool DeleteMeetingSchedule(string meetingId)
        {
            return mgr.DeleteMeetingSchedule(meetingId);
        }
    }

}
