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
    /// Data access:TicketsOrder
    /// </summary>
    public class TicketsOrderRepositorySqlDataProvider : SqlHelper, ITicketsOrderRespository
    {
        public TicketsOrderRepositorySqlDataProvider()
        { }
        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(TicketsOrderEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TicketsOrder(");
            strSql.Append("ProjectID,TicketID,OrderNum)");

            strSql.Append(" values (");
            strSql.Append("@ProjectID,@TicketID,@OrderNum)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                    db.AddInParameter(dbCommand, "OrderNum", DbType.Int32, model.OrderNum);
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
        /// <summary>
        /// Update a record
        /// </summary>
        public bool Update(TicketsOrderEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TicketsOrder set ");
            strSql.Append("ProjectID=@ProjectID,");
            strSql.Append("TicketID=@TicketID,");
            strSql.Append("OrderNum=@OrderNum");
            strSql.Append(" where TicketOrderID=@TicketOrderID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketOrderID", DbType.Int32, model.TicketOrderID);
                db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                db.AddInParameter(dbCommand, "OrderNum", DbType.Int32, model.OrderNum);
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
        /// Delete a record
        /// </summary>
        public bool Delete(int TicketOrderID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TicketsOrder ");
            strSql.Append(" where TicketOrderID=@TicketOrderID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketOrderID", DbType.Int32, TicketOrderID);
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
        public TicketsOrderEntity Get(int TicketID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TicketsOrder ");
            strSql.Append(" where TicketID=@TicketID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, TicketID);
                TicketsOrderEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        model = TicketsOrderEntity.ReaderBind(dataReader);
                    }
                }
                return model;
            }
        }

        #endregion  Method

        #region ITicketsOrderRespository Members

        public bool RemoveAllTicketsOrderByProject(int projectID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TicketsOrder ");
            strSql.Append(" where ProjectID=@ProjectID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, projectID);
                    int rows = db.ExecuteNonQuery(dbCommand);
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

        #endregion
    }
}

