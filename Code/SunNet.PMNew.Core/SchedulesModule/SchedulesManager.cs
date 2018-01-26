using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Core.SchedulesModule.Interfaces;
using SunNet.PMNew.Entity.SchedulesModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.UserModel;
using System.Transactions;
using SunNet.PMNew.Framework;
using System.Linq.Expressions;


namespace SunNet.PMNew.Core.SchedulesModule
{
    public class SchedulesManager : BaseMgr
    {
        private ISchedulesRepository schedulesRepository;
        private IEmailSender emailSender;

        public SchedulesManager(IEmailSender emailSender
                            , ISchedulesRepository schedulesRepository)
        {
            this.emailSender = emailSender;
            this.schedulesRepository = schedulesRepository;
        }

        public SchedulesEntity GetInfo(int id)
        {
            return schedulesRepository.Get(id);
        }

        public int Add(SchedulesEntity entity)
        {
            return schedulesRepository.Insert(entity);
        }

        public int Add(SchedulesEntity entity, List<UsersEntity> userList, UsersEntity currentUser)
        {
            using (TransactionScope tr = new TransactionScope())
            {
                foreach (UsersEntity tmpUser in userList)
                {
                    entity.UserID = tmpUser.UserID;
                    if (schedulesRepository.Insert(entity) < 1)//添加失败
                    {
                        return 0;
                    }
                }
                tr.Complete();
            }

            string body = UtilFactory.Helpers.FileHelper.GetTemplateFileContent("MeetingInvitations.txt");
            body = body.Replace("[Title]", entity.Title).Replace("[DataTime]", entity.PlanDate.ToString("MM/dd/yyyy"));
            userList.Remove(currentUser);
            foreach (UsersEntity tmpUser in userList)
            {
                string content = body.Replace("{FristName}", tmpUser.FirstName);
                emailSender.SendMail(tmpUser.UserName, Config.DefaultSendEmail, string.Format("[Meeting]{0}", entity.Title), content);
            }
            return 1;
        }

        public bool Update(SchedulesEntity entity)
        {
            return schedulesRepository.Update(entity);
        }

        public bool Update(SchedulesEntity entity, List<UsersEntity> moveUsers, List<UsersEntity> newUsers)
        {
            using (TransactionScope tr = new TransactionScope())
            {
                if (moveUsers.Count > 0)
                {
                    var userIds = moveUsers.Select(r => r.UserID.ToString()).ToArray();
                    if (schedulesRepository.VoteMeeting(entity.MeetingID, userIds, DateTime.Now.Date) == false)
                        return false;
                }
                if (schedulesRepository.UpdateMeeting(entity) == false)
                    return false;

                foreach (UsersEntity tmp in newUsers)
                {
                    entity.UserID = tmp.UserID;
                    if (schedulesRepository.Insert(entity) < 1)
                        return false;
                }
                tr.Complete();
            }


            string body = UtilFactory.Helpers.FileHelper.GetTemplateFileContent("MeetingInvitations.txt");
            body = body.Replace("[Title]", entity.Title).Replace("[DataTime]", entity.PlanDate.ToString("MM/dd/yyyy"));
            foreach (UsersEntity tmpUser in newUsers) //新加的参会人员发邮件
            {
                emailSender.SendMail(tmpUser.UserName, Config.DefaultSendEmail, string.Format("[Meeting]{0}", entity.Title), body);
            }
            body = UtilFactory.Helpers.FileHelper.GetTemplateFileContent("MeetingCancel.txt");
            body = body.Replace("[Title]", entity.Title);
            foreach (UsersEntity tmpUser in moveUsers)//取消参会资格的人员发送取消邮件
            {
                emailSender.SendMail(tmpUser.UserName, Config.DefaultSendEmail, string.Format("[Meeting]{0}", entity.Title), body);
            }

            return true;
        }

        /// <summary>
        /// 获取用户指定日期的Schdules
        /// </summary>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SchedulesEntity> GetSchedules(DateTime date, int userId)
        {
            return schedulesRepository.GetSchedules(date, userId);
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
            return schedulesRepository.GetSchedules(startDate, endDate, userId);
        }

        /// <summary>
        /// 根据会议ID ，获取计划参会人员
        /// </summary>
        /// <param name="meetingID"></param>
        /// <returns></returns>
        public List<UsersEntity> GetMeetingUsers(string meetingID)
        {
            return schedulesRepository.GetMeetingUsers(meetingID);
        }

        /// <summary>
        /// 不同意会议
        /// </summary>
        public bool VoteMeeting(string meetingId, DateTime updateDate, int updateUser)
        {
            return schedulesRepository.VoteMeeting(meetingId, updateDate, updateUser);
        }

        /// <summary>
        /// 同意会议
        /// </summary>
        public bool AgreeMeeting(string meetingId, DateTime updateDate
            , int updateUser)
        {
            return schedulesRepository.AgreeMeeting(meetingId, updateDate, updateUser);
        }

        public List<UsersEntity> GetMeetingUsersExceptCurrent(string meetingId, int currentUserId)
        {
            return schedulesRepository.GetMeetingUsers(meetingId)
                   .FindAll(r => r.UserID != currentUserId);
        }

        public void SendMeetingEmailToUsersExceptCurrent(SchedulesEntity schedulesEntity, List<UsersEntity> meetingUsers
            , string contentAppendix)
        {
            string body = UtilFactory.Helpers.FileHelper.GetTemplateFileContent("MeetingInvitations.txt");
            body = body.Replace("[Title]", schedulesEntity.Title).Replace("[DataTime]"
                , DateTime.Now.ToString("MM/dd/yyyy") + " " + contentAppendix);
            foreach (UsersEntity meetingUser in meetingUsers)
            {
                emailSender.SendMail(meetingUser.UserName, Config.DefaultSendEmail
                    , string.Format("[Meeting]{0}", schedulesEntity.Title + " " + contentAppendix), body);
            }
        }

        public bool Delete(int entityId)
        {
            return schedulesRepository.Delete(entityId);
        }

        public bool DeleteMeetingSchedule(string meetingId)
        {
            return schedulesRepository.DeleteMeetingSchedule(meetingId);
        }
    }
}
