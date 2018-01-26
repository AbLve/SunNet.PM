using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SF.Framework;
using SF.Framework.Helpers;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core;
using ReminderEmail.ReminderModel;

namespace ReminderEmail.ReminderDal
{
    public class ReminderHistoryDal
    {
        private Database db = DatabaseFactory.CreateDatabase("PM");

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>是否成功</returns>
        public bool Create(ReminderHistoryModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT ReminderHistory(RunStartTime, RunEndTime, RunDate, DataStartTime, DataEndTime, TotalCount, SuccessCount, FailCount, ErrorCount, State, CreateTime) ");
            strSql.Append("OUTPUT Inserted.Id, Inserted.RunStartTime, Inserted.RunEndTime, Inserted.RunDate, Inserted.DataStartTime, Inserted.DataEndTime, Inserted.TotalCount, Inserted.SuccessCount, Inserted.FailCount, Inserted.ErrorCount, Inserted.State, Inserted.CreateTime ");
            strSql.Append("VALUES(@RunStartTime, @RunEndTime, @RunDate, @DataStartTime, @DataEndTime, @TotalCount, @SuccessCount, @FailCount, @ErrorCount, @State, @CreateTime) ");

            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "RunStartTime", DbType.DateTime, model.RunStartTime);
                db.AddInParameter(dbCommand, "RunEndTime", DbType.DateTime, model.RunEndTime);
                db.AddInParameter(dbCommand, "RunDate", DbType.Date, model.RunDate);
                db.AddInParameter(dbCommand, "DataStartTime", DbType.DateTime, model.DataStartTime);
                db.AddInParameter(dbCommand, "DataEndTime", DbType.DateTime, model.DataEndTime);
                db.AddInParameter(dbCommand, "TotalCount", DbType.Int32, model.TotalCount);
                db.AddInParameter(dbCommand, "SuccessCount", DbType.Int32, model.SuccessCount);
                db.AddInParameter(dbCommand, "FailCount", DbType.Int32, model.FailCount);
                db.AddInParameter(dbCommand, "ErrorCount", DbType.Int32, model.ErrorCount);
                db.AddInParameter(dbCommand, "State", DbType.Int32, model.State);
                db.AddInParameter(dbCommand, "CreateTime", DbType.DateTime, model.CreateTime);


                var effectCount = db.ExecuteNonQuery(dbCommand);
                if (effectCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 插入一条数据，并返回插入的实体
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>插入后的实体</returns>
        public ReminderHistoryModel CreateAndReturn(ReminderHistoryModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT ReminderHistory(RunStartTime, RunEndTime, RunDate, DataStartTime, DataEndTime, TotalCount, SuccessCount, FailCount, ErrorCount, State, CreateTime) ");
            strSql.Append("OUTPUT Inserted.Id, Inserted.RunStartTime, Inserted.RunEndTime, Inserted.RunDate, Inserted.DataStartTime, Inserted.DataEndTime, Inserted.TotalCount, Inserted.SuccessCount, Inserted.FailCount, Inserted.ErrorCount, Inserted.State, Inserted.CreateTime ");
            strSql.Append("VALUES(@RunStartTime, @RunEndTime, @RunDate, @DataStartTime, @DataEndTime, @TotalCount, @SuccessCount, @FailCount, @ErrorCount, @State, @CreateTime) ");

            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "RunStartTime", DbType.DateTime, model.RunStartTime);
                db.AddInParameter(dbCommand, "RunEndTime", DbType.DateTime, model.RunEndTime);
                db.AddInParameter(dbCommand, "RunDate", DbType.Date, model.RunDate);
                db.AddInParameter(dbCommand, "DataStartTime", DbType.DateTime, model.DataStartTime);
                db.AddInParameter(dbCommand, "DataEndTime", DbType.DateTime, model.DataEndTime);
                db.AddInParameter(dbCommand, "TotalCount", DbType.Int32, model.TotalCount);
                db.AddInParameter(dbCommand, "SuccessCount", DbType.Int32, model.SuccessCount);
                db.AddInParameter(dbCommand, "FailCount", DbType.Int32, model.FailCount);
                db.AddInParameter(dbCommand, "ErrorCount", DbType.Int32, model.ErrorCount);
                db.AddInParameter(dbCommand, "State", DbType.Int32, model.State);
                db.AddInParameter(dbCommand, "CreateTime", DbType.DateTime, model.CreateTime);

                DataSet dataSet = db.ExecuteDataSet(dbCommand);

                if (dataSet != null && dataSet.Tables[0].Rows.Count>0)
                {
                    model.Id = dataSet.Tables[0].Rows[0]["Id"].ToInt32();
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public int UpdateModel(ReminderHistoryModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE ReminderHistory SET RunStartTime = @RunStartTime, RunEndTime = @RunEndTime, RunDate = @RunDate, DataStartTime = @DataStartTime, DataEndTime = @DataEndTime, TotalCount = @TotalCount, SuccessCount = @SuccessCount, FailCount = @FailCount, ErrorCount = @ErrorCount, State = @State ");
            strSql.Append("WHERE Id = @Id ");

            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "Id", DbType.Int32, model.Id);
                db.AddInParameter(dbCommand, "RunStartTime", DbType.DateTime, model.RunStartTime);
                db.AddInParameter(dbCommand, "RunEndTime", DbType.DateTime, model.RunEndTime);
                db.AddInParameter(dbCommand, "RunDate", DbType.Date, model.RunDate);
                db.AddInParameter(dbCommand, "DataStartTime", DbType.DateTime, model.DataStartTime);
                db.AddInParameter(dbCommand, "DataEndTime", DbType.DateTime, model.DataEndTime);
                db.AddInParameter(dbCommand, "TotalCount", DbType.Int32, model.TotalCount);
                db.AddInParameter(dbCommand, "SuccessCount", DbType.Int32, model.SuccessCount);
                db.AddInParameter(dbCommand, "FailCount", DbType.Int32, model.FailCount);
                db.AddInParameter(dbCommand, "ErrorCount", DbType.Int32, model.ErrorCount);
                db.AddInParameter(dbCommand, "State", DbType.Int32, model.State);
                db.AddInParameter(dbCommand, "CreateTime", DbType.DateTime, model.CreateTime);


                int effectCount = db.ExecuteNonQuery(dbCommand); ;
                
                return effectCount;
            }
        }


        /// <summary>
        /// 查询指定日期是否执行过
        /// </summary>
        public bool GetLastModelByRunDate(DateTime runDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from ReminderHistory ");
            strSql.Append("WHERE RunDate = @RunDate ");

            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "RunDate", DbType.Date, runDate.Date);

                var resultsOfDataSet = db.ExecuteDataSet(dbCommand);

                if (resultsOfDataSet.Tables[0].Rows.Count>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 是否存在正在执行的数据
        /// </summary>
        public bool ExistsRunning()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from dbo.ReminderHistory where state=1 and datediff(hour, RunStartTime, getdate()) <= 24 ");

            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                var resultsOfDataSet = db.ExecuteDataSet(dbCommand);

                if (resultsOfDataSet.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
