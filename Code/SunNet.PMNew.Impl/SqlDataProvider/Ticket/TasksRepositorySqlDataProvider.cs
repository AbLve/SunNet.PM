using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Impl.SqlDataProvider.Ticket
{
    /// <summary>
    /// Data access:Tasks
    /// </summary>
    public class TasksRepositorySqlDataProvide : SqlHelper, ITaskRespository
    {
        public TasksRepositorySqlDataProvide()
        { }
        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(TasksEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Tasks(");
            strSql.Append("ProjectID,TicketID,Title,Description,IsCompleted,CompletedDate)");

            strSql.Append(" values (");
            strSql.Append("@ProjectID,@TicketID,@Title,@Description,@IsCompleted,@CompletedDate)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                db.AddInParameter(dbCommand, "IsCompleted", DbType.Boolean, model.IsCompleted);
                db.AddInParameter(dbCommand, "CompletedDate", DbType.Date, model.CompletedDate);
                int result;
                object obj = db.ExecuteScalar(dbCommand);
                if (!int.TryParse(obj.ToString(), out result))
                {
                    return 0;
                }
                return result;
            }
        }
        /// <summary>
        /// Update a record
        /// </summary>
        public bool Update(TasksEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tasks set ");
            strSql.Append("ProjectID=@ProjectID,");
            strSql.Append("TicketID=@TicketID,");
            strSql.Append("Title=@Title,");
            strSql.Append("Description=@Description,");
            strSql.Append("IsCompleted=@IsCompleted");
            strSql.Append(" where TaskID=@TaskID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TaskID", DbType.Int32, model.TaskID);
                db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                db.AddInParameter(dbCommand, "IsCompleted", DbType.Boolean, model.IsCompleted);
                int rows = db.ExecuteNonQuery(dbCommand);

                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }  /// <summary>

        /// <summary>
        /// Delete a record
        /// </summary>
        public bool Delete(int TaskID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tasks ");
            strSql.Append(" where TaskID=@TaskID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TaskID", DbType.Int32, TaskID);
                int rows = db.ExecuteNonQuery(dbCommand);

                if (rows > 0)
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
        /// Get an object entity
        /// </summary>
        public TasksEntity Get(int TaskID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Tasks ");
            strSql.Append(" where TaskID=@TaskID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TaskID", DbType.Int32, TaskID);
                TasksEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        model = TasksEntity.ReaderBind(dataReader);
                    }
                }
                return model;
            }
        }

        #endregion  Method
    }
}

