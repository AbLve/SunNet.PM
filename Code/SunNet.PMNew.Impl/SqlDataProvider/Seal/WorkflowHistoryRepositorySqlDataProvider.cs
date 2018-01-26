using Microsoft.Practices.EnterpriseLibrary.Data;
using SunNet.PMNew.Core.SealModel.Interfaces;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Framework.Utils.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Impl.SqlDataProvider.Seal
{
    public class WorkflowHistoryRepositorySqlDataProvider : SqlHelper, IWorkflowHistoryRepository
    {
        public int Insert(WorkflowHistoryEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO WorkflowHistory");
            strSql.Append("(WorkflowRequestID,CreatedTime,ProcessedBy,Action) ");
            strSql.Append("VALUES(");
            strSql.Append("@WorkflowRequestID,@CreatedTime,@ProcessedBy,@Action)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "WorkflowRequestID", DbType.Int32, entity.WorkflowRequestID);
                    db.AddInParameter(dbCommand, "CreatedTime", DbType.DateTime, entity.CreatedTime);
                    db.AddInParameter(dbCommand, "ProcessedBy", DbType.Int32, entity.ProcessedBy);
                    db.AddInParameter(dbCommand, "Action", DbType.Int32, (int)entity.Action);

                    int result;
                    object obj = db.ExecuteScalar(dbCommand);
                    if (!int.TryParse(obj.ToString(), out result))
                    {
                        return 0;
                    }
                    return result;
                }
                catch(Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(),
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                    return 0;
                }
            }

        }

        public int InsertFirst(WorkflowHistoryEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO WorkflowHistory");
            strSql.Append("(WorkflowRequestID,CreatedTime,ProcessedBy,ProcessedTime,Action) ");
            strSql.Append("VALUES(");
            strSql.Append("@WorkflowRequestID,@CreatedTime,@ProcessedBy,@ProcessedTime,@Action)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "WorkflowRequestID", DbType.Int32, entity.WorkflowRequestID);
                    db.AddInParameter(dbCommand, "CreatedTime", DbType.DateTime, entity.CreatedTime);
                    db.AddInParameter(dbCommand, "ProcessedBy", DbType.Int32, entity.ProcessedBy);
                    db.AddInParameter(dbCommand, "ProcessedTime", DbType.DateTime, entity.ProcessedTime);
                    db.AddInParameter(dbCommand, "Action", DbType.Int32, (int)entity.Action);

                    int result;
                    object obj = db.ExecuteScalar(dbCommand);
                    if (!int.TryParse(obj.ToString(), out result))
                    {
                        return 0;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(),
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                    return 0;
                }
            }
        }

        public bool Update(WorkflowHistoryEntity entity)
        {
            return true;
        }

        public int UpdateReturnID(WorkflowHistoryEntity entity)
        {
            int resultID = -1;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ID from WorkflowHistory ");
            strSql.Append("WHERE WorkflowRequestID=@WorkflowRequestID AND ProcessedBy=@ProcessedBy AND Action=-1; ");
            strSql.Append("Update WorkflowHistory Set ");
            strSql.Append("ProcessedTime=@ProcessedTime,Action=@Action,Comment=@Comment ");
            strSql.Append("WHERE WorkflowRequestID=@WorkflowRequestID AND ProcessedBy=@ProcessedBy AND Action=-1");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "WorkflowRequestID", DbType.Int32, entity.WorkflowRequestID);
                    db.AddInParameter(dbCommand, "ProcessedBy", DbType.Int32, entity.ProcessedBy); 
                    db.AddInParameter(dbCommand, "ProcessedTime", DbType.DateTime, entity.ProcessedTime);
                    db.AddInParameter(dbCommand, "Action", DbType.Int32, (int)entity.Action);
                    db.AddInParameter(dbCommand, "Comment", DbType.String, entity.Comment);

                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            resultID = (int)dataReader["ID"];
                        }
                    }

                    db.ExecuteScalar(dbCommand);
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(),
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                }
            }
            return resultID;
        }

        public bool Delete(int entityId)
        {
            return true;
        }

        public bool DeleteWhereActions(int requestID, params int[] actions)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Delete from WorkflowHistory ")
                .Append("where WorkflowRequestID = @WorkflowRequestID and (1=0 ");
            foreach (int action in actions)
            {
                strSql.AppendFormat("Or Action={0} ", action);
            }
            strSql.Append(")");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "WorkflowRequestID", DbType.Int32, requestID);

                return db.ExecuteNonQuery(dbCommand) > 0;
            }
        }

        public WorkflowHistoryEntity Get(int entityId)
        {
            return null;
        }

        public List<WorkflowHistoryEntity> GetList(int workflowRequestID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select wfhis.*, u.FirstName+' '+u.LastName As ProcessedByName from WorkflowHistory wfhis, Users u ");
            strSql.Append(" where WorkflowRequestID=@WorkflowRequestID and wfhis.ProcessedBy=u.UserID; ");
            Database db = DatabaseFactory.CreateDatabase();
            List<WorkflowHistoryEntity> list = new List<WorkflowHistoryEntity>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "WorkflowRequestID", DbType.Int32, workflowRequestID);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            list.Add(WorkflowHistoryEntity.ReaderBind(dataReader));
                        }
                    }

                    foreach (WorkflowHistoryEntity ent in list)
                    {
                        SealFilesRepositorySqlDataProvider sealFilesRep = new SealFilesRepositorySqlDataProvider();
                        ent.lstFiles = sealFilesRep.GetListByHistoryID(ent.ID);
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                }
                return list;
            }
        }

        public bool CheckUserHasRecords(int userID, int requestID, params int[] actions)
        {
            int count;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) as Count from WorkflowHistory ");
            strSql.Append(" where ProcessedBy=@ProcessedBy and WorkflowRequestID=@WorkflowRequestID and (0=1 ") ;
            foreach (int action in actions)
            {
                strSql.AppendFormat("Or Action={0} ", action);
            }
            strSql.Append(")");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProcessedBy", DbType.Int32, userID);
                    db.AddInParameter(dbCommand, "WorkflowRequestID", DbType.Int32, requestID);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            count = (int)dataReader["Count"];
                            if (count == 0)
                                return false;
                            else if (count == 1)
                                return true;
                            else
                                throw new Exception();
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                }
                return true;
            }
        }

        public bool CheckRequestHasPendingRecord(int requestID)
        {
            int count;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) as Count from WorkflowHistory ");
            strSql.Append(" where WorkflowRequestID=@WorkflowRequestID and Action=-1");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "WorkflowRequestID", DbType.Int32, requestID);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            count = (int)dataReader["Count"];
                            if (count == 0)
                                return false;
                            else if (count == 1)
                                return true;
                            else
                                throw new Exception();
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                }
                return true;
            }
        }

        public bool UpdateInactiveToPending(int sealRequestID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Update WorkflowHistory Set ");
            strSql.Append("Action=-1 ");
            strSql.Append("WHERE WorkflowRequestID=@WorkflowRequestID AND Action=-2");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "WorkflowRequestID", DbType.Int32, sealRequestID);
                    db.ExecuteScalar(dbCommand);
                    return true;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(),
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                    return false;
                }
            }
        }
    }
}
