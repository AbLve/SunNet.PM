using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.TicketModule;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Impl.SqlDataProvider.Ticket
{
    public class WeekPlanRepositorySqlDataProvider : SqlHelper, IWeekPlanRepository
    {
        public int Insert(WeekPlanEntity weekPlanEntity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeekPlan set IsDeleted=1 where  UserID=@UserID and WeekDay=@WeekDay ;");
            strSql.Append("insert into WeekPlan(");
            strSql.Append("UserID,[WeekDay],CreateDate,UpdateDate,UpdateUserID,Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,IsDeleted");
            strSql.Append(",MondayEstimate,TuesdayEstimate,WednesdayEstimate,ThursdayEstimate,FridayEstimate,SaturdayEstimate,SundayEstimate " +
                           ",[SundayTickets],[MondayTickets],[TuesdayTickets],[WednesdayTickets],[ThursdayTickets],[FridayTickets],[SaturdayTickets])");
            strSql.Append(" values (");
            strSql.Append("@UserID,@WeekDay,@CreateDate,@UpdateDate,@UpdateUserID,@Sunday,@Monday,@Tuesday,@Wednesday,@Thursday,@Friday,@Saturday,@IsDeleted");
            strSql.Append(",@MondayEstimate,@TuesdayEstimate,@WednesdayEstimate,@ThursdayEstimate,@FridayEstimate,@SaturdayEstimate,@SundayEstimate" +
                          ",@SundayTickets ,@MondayTickets ,@TuesdayTickets ,@WednesdayTickets ,@ThursdayTickets ,@FridayTickets ,@SaturdayTickets)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, weekPlanEntity.UserID);
                db.AddInParameter(dbCommand, "WeekDay", DbType.DateTime, weekPlanEntity.WeekDay.Date);
                db.AddInParameter(dbCommand, "CreateDate", DbType.DateTime, weekPlanEntity.CreateDate);
                db.AddInParameter(dbCommand, "UpdateDate", DbType.DateTime, weekPlanEntity.UpdateDate);
                db.AddInParameter(dbCommand, "UpdateUserID", DbType.Int32, weekPlanEntity.UpdateUserID);

                db.AddInParameter(dbCommand, "Sunday", DbType.String, weekPlanEntity.Sunday);
                db.AddInParameter(dbCommand, "Monday", DbType.String, weekPlanEntity.Monday);
                db.AddInParameter(dbCommand, "Tuesday", DbType.String, weekPlanEntity.Tuesday);
                db.AddInParameter(dbCommand, "Wednesday", DbType.String, weekPlanEntity.Wednesday);
                db.AddInParameter(dbCommand, "Thursday", DbType.String, weekPlanEntity.Thursday);
                db.AddInParameter(dbCommand, "Friday", DbType.String, weekPlanEntity.Friday);
                db.AddInParameter(dbCommand, "Saturday", DbType.String, weekPlanEntity.Saturday);

                db.AddInParameter(dbCommand, "MondayEstimate", DbType.String, weekPlanEntity.MondayEstimate);
                db.AddInParameter(dbCommand, "TuesdayEstimate", DbType.String, weekPlanEntity.TuesdayEstimate);
                db.AddInParameter(dbCommand, "WednesdayEstimate", DbType.String, weekPlanEntity.WednesdayEstimate);
                db.AddInParameter(dbCommand, "ThursdayEstimate", DbType.String, weekPlanEntity.ThursdayEstimate);
                db.AddInParameter(dbCommand, "FridayEstimate", DbType.String, weekPlanEntity.FridayEstimate);
                db.AddInParameter(dbCommand, "SaturdayEstimate", DbType.String, weekPlanEntity.SaturdayEstimate);
                db.AddInParameter(dbCommand, "SundayEstimate", DbType.String, weekPlanEntity.SundayEstimate);

                db.AddInParameter(dbCommand, "SundayTickets", DbType.String, weekPlanEntity.SundayTickets);
                db.AddInParameter(dbCommand, "MondayTickets", DbType.String, weekPlanEntity.MondayTickets);
                db.AddInParameter(dbCommand, "TuesdayTickets", DbType.String, weekPlanEntity.TuesdayTickets);
                db.AddInParameter(dbCommand, "WednesdayTickets", DbType.String, weekPlanEntity.WednesdayTickets);
                db.AddInParameter(dbCommand, "ThursdayTickets", DbType.String, weekPlanEntity.ThursdayTickets);
                db.AddInParameter(dbCommand, "FridayTickets", DbType.String, weekPlanEntity.FridayTickets);
                db.AddInParameter(dbCommand, "SaturdayTickets", DbType.String, weekPlanEntity.SaturdayTickets);
              

                db.AddInParameter(dbCommand, "IsDeleted", DbType.String, weekPlanEntity.IsDeleted);
                int result;
                object obj = db.ExecuteScalar(dbCommand);
                if (!int.TryParse(obj.ToString(), out result))
                {
                    return 0;
                }
                return result;
            }
        }

        public bool Update(WeekPlanEntity entity, bool isVerify)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeekPlan")
            .Append(" set UpdateDate =@UpdateDate ,UpdateUserID=@UpdateUserID,Sunday=@Sunday,Monday=@Monday,Tuesday=@Tuesday")
            .Append(",Wednesday=@Wednesday,Thursday=@Thursday,Friday=@Friday,Saturday=@Saturday,MondayEstimate=@MondayEstimate")
            .Append(",TuesdayEstimate=@TuesdayEstimate,WednesdayEstimate=@WednesdayEstimate,ThursdayEstimate=@ThursdayEstimate")
            .Append(",FridayEstimate=@FridayEstimate,SaturdayEstimate=@SaturdayEstimate,SundayEstimate=@SundayEstimate")

            .Append(",SundayTickets=@SundayTickets,MondayTickets=@MondayTickets,TuesdayTickets=@TuesdayTickets,WednesdayTickets=@WednesdayTickets")
             .Append(",ThursdayTickets=@ThursdayTickets,FridayTickets=@FridayTickets,SaturdayTickets=@SaturdayTickets")
            ;
            if (isVerify)
                strSql.Append(" where id=@id and UserId = @UpdateUserID");
            else
                strSql.Append(" where id=@id");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "id", DbType.Int32, entity.ID);

                db.AddInParameter(dbCommand, "UpdateDate", DbType.DateTime, entity.UpdateDate);
                db.AddInParameter(dbCommand, "UpdateUserID", DbType.Int32, entity.UpdateUserID);
                db.AddInParameter(dbCommand, "Sunday", DbType.String, entity.Sunday);
                db.AddInParameter(dbCommand, "Monday", DbType.String, entity.Monday);
                db.AddInParameter(dbCommand, "Tuesday", DbType.String, entity.Tuesday);
                db.AddInParameter(dbCommand, "Wednesday", DbType.String, entity.Wednesday);
                db.AddInParameter(dbCommand, "Thursday", DbType.String, entity.Thursday);
                db.AddInParameter(dbCommand, "Friday", DbType.String, entity.Friday);
                db.AddInParameter(dbCommand, "Saturday", DbType.String, entity.Saturday);
                db.AddInParameter(dbCommand, "MondayEstimate", DbType.String, entity.MondayEstimate);
                db.AddInParameter(dbCommand, "TuesdayEstimate", DbType.String, entity.TuesdayEstimate);
                db.AddInParameter(dbCommand, "WednesdayEstimate", DbType.String, entity.WednesdayEstimate);
                db.AddInParameter(dbCommand, "ThursdayEstimate", DbType.String, entity.ThursdayEstimate);
                db.AddInParameter(dbCommand, "FridayEstimate", DbType.String, entity.FridayEstimate);
                db.AddInParameter(dbCommand, "SaturdayEstimate", DbType.String, entity.SaturdayEstimate);
                db.AddInParameter(dbCommand, "SundayEstimate", DbType.String, entity.SundayEstimate);

                db.AddInParameter(dbCommand, "SundayTickets", DbType.String, entity.SundayTickets);
                db.AddInParameter(dbCommand, "MondayTickets", DbType.String, entity.MondayTickets);
                db.AddInParameter(dbCommand, "TuesdayTickets", DbType.String, entity.TuesdayTickets);
                db.AddInParameter(dbCommand, "WednesdayTickets", DbType.String, entity.WednesdayTickets);
                db.AddInParameter(dbCommand, "ThursdayTickets", DbType.String, entity.ThursdayTickets);
                db.AddInParameter(dbCommand, "FridayTickets", DbType.String, entity.FridayTickets);
                db.AddInParameter(dbCommand, "SaturdayTickets", DbType.String, entity.SaturdayTickets);
              

                return db.ExecuteNonQuery(dbCommand) > 0;
            }
        }

        public bool Update(WeekPlanEntity entity)
        {
            throw new NotImplementedException();
        }


        public bool Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public WeekPlanEntity Get(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WeekPlan where id = @ID and IsDeleted=0;");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                        return new WeekPlanEntity(dataReader, false);
                }
            }
            return null;
        }


        public WeekPlanEntity Get(int userid, DateTime day)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WeekPlan where userid = @userid and [WeekDay] =@WeekDay and IsDeleted=0;");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "userid", DbType.Int32, userid);
                db.AddInParameter(dbCommand, "WeekDay", DbType.DateTime, day.Date);

                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                        return new WeekPlanEntity(dataReader, false);
                }
            }
            return null;
        }

        public WeekPlanEntity Get(int id, int userId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WeekPlan where id = @ID and userid=@userId and IsDeleted=0;");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ID", DbType.Int32, id);
                db.AddInParameter(dbCommand, "userId", DbType.Int32, userId);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                        return new WeekPlanEntity(dataReader, false);
                }
            }
            return null;
        }


        public List<WeekPlanEntity> GetList(int userId, DateTime startDate, DateTime endDate, RolesEnum role, int pageNo, int pageSize, out int recordCount)
        {
            recordCount = 0;
            List<WeekPlanEntity> list = new List<WeekPlanEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetStoredProcCommand("WeekPlan_GetList"))
            {
                db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);
                db.AddInParameter(dbCommand, "@StartDate", DbType.DateTime, startDate);
                db.AddInParameter(dbCommand, "@EndDate", DbType.DateTime, endDate);
                db.AddInParameter(dbCommand, "@PageNo", DbType.Int32, pageNo);
                db.AddInParameter(dbCommand, "@PageSize", DbType.Int32, pageSize);
                db.AddInParameter(dbCommand, "@Role", DbType.Int32, (int)role);
                db.AddOutParameter(dbCommand, "@RecordCount", DbType.Int32, 4);

                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {

                    while (dataReader.Read())
                    {
                        list.Add(new WeekPlanEntity(dataReader, true));
                    }
                }
                recordCount = (int)db.GetParameterValue(dbCommand, "RecordCount");
            }
            return list;
        }

        /// <summary>
        /// 返回的对象只有ID　与WeekDay
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<WeekPlanEntity> GetWeekDay(int userId)
        {
            List<WeekPlanEntity> list = new List<WeekPlanEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand("select ID, WeekDay from WeekPlan where userid=@userId and  IsDeleted=0;"))
            {
                db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);

                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        list.Add(new WeekPlanEntity() { ID = (int)dataReader["ID"], WeekDay = (DateTime)dataReader["WeekDay"] });
                    }
                }
            }
            return list;
        }
    }
}
