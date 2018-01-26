using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.SchedulesModel;
using SunNet.PMNew.Core.SchedulesModule.Interfaces;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Impl.SqlDataProvider.Schedules
{
    public class SchedulesRepositorySqlDataProvide : SqlHelper, ISchedulesRepository
    {
        public int Insert(SchedulesEntity model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into Schedules(Title,StartTime,EndTime,Description,CreateBy,CreateOn,")
            .Append("UpdateBy,UpdateOn,MeetingStatus,MeetingID,PlanDate,UserID)")
                .Append(" values(@Title,@StartTime,@EndTime,@Description,@CreateBy,@CreateOn,")
            .Append("@UpdateBy,@UpdateOn,@MeetingStatus,@MeetingID,@PlanDate,@UserID)")
            .Append(";select ISNULL( SCOPE_IDENTITY(),0);");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                db.AddInParameter(dbCommand, "StartTime", DbType.String, model.StartTime);
                db.AddInParameter(dbCommand, "EndTime", DbType.String, model.EndTime);
                db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                db.AddInParameter(dbCommand, "CreateBy", DbType.Int32, model.CreateBy);
                db.AddInParameter(dbCommand, "CreateOn", DbType.DateTime, model.CreateOn);

                db.AddInParameter(dbCommand, "UpdateBy", DbType.Int32, model.UpdateBy);
                db.AddInParameter(dbCommand, "UpdateOn", DbType.DateTime, model.UpdateOn);
                db.AddInParameter(dbCommand, "MeetingStatus", DbType.Int32, model.MeetingStatus);
                db.AddInParameter(dbCommand, "MeetingID", DbType.String, model.MeetingID);
                db.AddInParameter(dbCommand, "PlanDate", DbType.DateTime, model.PlanDate);
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);

                int result;
                object obj = db.ExecuteScalar(dbCommand);
                if (!int.TryParse(obj.ToString(), out result))
                {
                    return 0;
                }
                return result;
            }
        }

        public bool Update(SchedulesEntity model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Schedules set ")
            .Append(" Title=@Title,StartTime=@StartTime,EndTime=@EndTime,Description=@Description")
            .Append(",UpdateBy=@UpdateBy,UpdateOn=@UpdateOn")
            .Append(" where ID=@ID");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                db.AddInParameter(dbCommand, "StartTime", DbType.String, model.StartTime);
                db.AddInParameter(dbCommand, "EndTime", DbType.String, model.EndTime);
                db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);

                db.AddInParameter(dbCommand, "UpdateBy", DbType.Int32, model.UpdateBy);
                db.AddInParameter(dbCommand, "UpdateOn", DbType.DateTime, model.UpdateOn);

                db.AddInParameter(dbCommand, "ID", DbType.Int32, model.ID);

                return db.ExecuteNonQuery(dbCommand) > 0;
            }
        }

        public bool UpdateMeeting(SchedulesEntity entity)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Schedules set ")
            .Append(" Title=@Title,StartTime=@StartTime,EndTime=@EndTime,Description=@Description")
            .Append(",UpdateBy=@UpdateBy,UpdateOn=@UpdateOn")
            .Append(" where MeetingID=@MeetingID and MeetingStatus!=3");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Title", DbType.String, entity.Title);
                    db.AddInParameter(dbCommand, "StartTime", DbType.String, entity.StartTime);
                    db.AddInParameter(dbCommand, "EndTime", DbType.String, entity.EndTime);
                    db.AddInParameter(dbCommand, "Description", DbType.String, entity.Description);

                    db.AddInParameter(dbCommand, "UpdateBy", DbType.Int32, entity.UpdateBy);
                    db.AddInParameter(dbCommand, "UpdateOn", DbType.DateTime, entity.UpdateOn);

                    db.AddInParameter(dbCommand, "MeetingID", DbType.String, entity.MeetingID);

                    db.ExecuteNonQuery(dbCommand);
                    return true;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:UpdateMeeting ; {0},{1}Messages:\r\n{2}]",
                        sb.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        public bool Delete(int entityId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM Schedules where ID=@ID");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);
                return db.ExecuteNonQuery(dbCommand) > 0;
            }
        }

        public bool DeleteMeetingSchedule(string meetingId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM Schedules where MeetingID=@MeetingID");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                db.AddInParameter(dbCommand, "MeetingID", DbType.String, meetingId);
                return db.ExecuteNonQuery(dbCommand) > 0;
            }
        }

        public SchedulesEntity Get(int entityId)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from dbo.Schedules where id={0} ; ", entityId);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            return new SchedulesEntity(dataReader);
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:Get ; {0},{1}Messages:\r\n{2}]",
                        sb.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }


        public List<SchedulesEntity> GetSchedules(DateTime date, int userId)
        {
            List<SchedulesEntity> list = new List<SchedulesEntity>();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from dbo.Schedules where userId={0} and PlanDate = '{1}' ", userId, date.Date);
            sb.AppendFormat(" and MeetingStatus!={0}", 3);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            list.Add(new SchedulesEntity(dataReader));
                        }
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:GetSchedules ; {0},{1}Messages:\r\n{2}]",
                        sb.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        public List<SchedulesEntity> GetSchedules(DateTime startDate, DateTime endDate, int userId)
        {
            List<SchedulesEntity> list = new List<SchedulesEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("select *, convert(datetime,CONVERT(VARCHAR(10),plandate,120)+' '+startTime) as orderkey")
             .Append(" from dbo.Schedules")
            .AppendFormat(" where userId={0} ", userId)
            .AppendFormat(" and PlanDate >= '{0}'", startDate)
            .AppendFormat(" and PlanDate <'{0}'", endDate)
            .AppendFormat(" and MeetingStatus!={0} ", 3)
            .AppendFormat(" order by orderkey {0}", "asc");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            list.Add(new SchedulesEntity(dataReader));
                        }
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:GetSchedules ; {0},{1}Messages:\r\n{2}]",
                        sb.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        /// <summary>
        /// MeetingStatus = 0:normal ; 1: waits for the acknowledgment  ; 2: acknowledgment ;3:cancel
        /// </summary>
        public List<UsersEntity> GetMeetingUsers(string meetingID)
        {
            List<UsersEntity> list = new List<UsersEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("select u.UserID,u.UserName,u.Office,u.FirstName,u.LastName from [Schedules] s ")
                .Append(" inner join Users u on u.userid = s.UserID ")
                .Append(" where MeetingID = @MeetingID and [MeetingStatus]!=3");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                db.AddInParameter(dbCommand, "MeetingID", DbType.String, meetingID);
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            list.Add(new UsersEntity()
                            {
                                UserID = (int)dataReader["UserID"],
                                FirstName = dataReader["FirstName"].ToString(),
                                LastName = dataReader["LastName"].ToString(),
                                UserName = (string)dataReader["UserName"],
                                Office = (string)dataReader["Office"]
                            });
                        }
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:GetMeetingUsers ; {0},{1}Messages:\r\n{2}]",
                        sb.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        /// <summary>
        ///  取消参会资格
        /// </summary>
        public bool VoteMeeting(string meetingId, string[] userIds, DateTime dateTime)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" update [Schedules]  set  [UpdateOn] = @UpdateOn ,[MeetingStatus]=3 ")
                .AppendFormat(" where meetingId = @meetingId and userId in ({0})", string.Join(",", userIds));


            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand cmd = db.GetSqlStringCommand(sb.ToString()))
            {
                try
                {
                    db.AddInParameter(cmd, "UpdateOn", DbType.DateTime, dateTime);
                    db.AddInParameter(cmd, "meetingId", DbType.String, meetingId);
                    db.ExecuteNonQuery(cmd);
                    return true;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:VoteMeeting ; {0},{1}Messages:\r\n{2}]",
                        sb.ToString(), base.FormatParameters(cmd.Parameters), ex.Message));
                    return false;
                }
            }
        }


        /// <summary>
        /// 不同意会议
        /// </summary>
        public bool VoteMeeting(string meetingId, DateTime updateDate, int updateUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" update [Schedules]  set  [UpdateOn] = @UpdateOn ,[MeetingStatus]=3 ,UpdateBy={0}", updateUser)
                .Append(" where meetingId = @meetingId  ");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand cmd = db.GetSqlStringCommand(sb.ToString()))
            {
                try
                {
                    db.AddInParameter(cmd, "UpdateOn", DbType.DateTime, updateDate);
                    db.AddInParameter(cmd, "meetingId", DbType.String, meetingId);
                    db.ExecuteNonQuery(cmd);
                    return true;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:VoteMeeting ; {0},{1}Messages:\r\n{2}]",
                        sb.ToString(), base.FormatParameters(cmd.Parameters), ex.Message));
                    return false;
                }
            }
        }

        /// <summary>
        /// 同意会议
        /// </summary>
        public bool AgreeMeeting(string meetingId, DateTime updateDate, int updateUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" update [Schedules]  set  [UpdateOn] = @UpdateOn ,[MeetingStatus]=2 ,UpdateBy={0}", updateUser)
                .Append(" where meetingId = @meetingId ");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand cmd = db.GetSqlStringCommand(sb.ToString()))
            {
                try
                {
                    db.AddInParameter(cmd, "UpdateOn", DbType.DateTime, updateDate);
                    db.AddInParameter(cmd, "meetingId", DbType.String, meetingId);
                    db.ExecuteNonQuery(cmd);
                    return true;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:AgreeMeeting ; {0},{1}Messages:\r\n{2}]",
                        sb.ToString(), base.FormatParameters(cmd.Parameters), ex.Message));
                    return false;
                }
            }
        }
    }
}
