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
    /// Data access:CateGoryTicket
    /// </summary>
    public class CateGoryTicketsRepositorySqlDataProvider : SqlHelper, ICateGoryTicketRepository
    {
        public CateGoryTicketsRepositorySqlDataProvider()
        { }
        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(CateGoryTicketEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("IF not exists (SELECT 1 FROM dbo.CateGoryTicket WHERE CategoryID=@CategoryID AND TicketID=@TicketID)  BEGIN ");
            strSql.Append("insert into CateGoryTicket(");
            strSql.Append("CategoryID,TicketID,CreatedOn)");

            strSql.Append(" values (");
            strSql.Append("@CategoryID,@TicketID,@CreatedOn)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);END");
            strSql.Append(" else begin select 0 end;");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, model.CategoryID);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
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
                        strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return 0;
                }
            }
        }
        /// <summary>
        /// Update a record
        /// </summary>
        public bool Update(CateGoryTicketEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CateGoryTicket set ");
            strSql.Append("CategoryID=@CategoryID,");
            strSql.Append("TicketID=@TicketID,");
            strSql.Append("CreatedOn=@CreatedOn");
            strSql.Append(" where CGTID=@CGTID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "CGTID", DbType.Int32, model.CGTID);
                    db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, model.CategoryID);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
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
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        /// <summary>
        /// Delete a record
        /// </summary>
        public bool Delete(int CGTID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from CateGoryTicket ");
            strSql.Append(" where CGTID=@CGTID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "CGTID", DbType.Int32, CGTID);
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
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        /// <summary>
        /// Get an object entity
        /// </summary>
        public CateGoryTicketEntity Get(int CGTID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from CateGoryTicket ");
            strSql.Append(" where CGTID=@CGTID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "CGTID", DbType.Int32, CGTID);
                CateGoryTicketEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        if (dataReader.Read())
                        {
                            model = CateGoryTicketEntity.ReaderBind(dataReader);
                        } return model;
                    }
                    catch (Exception ex)
                    {
                        WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                            strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                        return null;
                    }
                }

            }
        }



        #endregion  Method

        #region ICateGoryTicketRepository Members

        public bool Delete(int ticketID, int cateGoryID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from CateGoryTicket ");
            strSql.Append(" where CategoryID=@CategoryID");
            if (ticketID > 0)
            {
                strSql.Append(" AND TicketID=@TicketID ");
            }
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, cateGoryID);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketID);
                    int rows = db.ExecuteNonQuery(dbCommand);

                    return rows >= 0;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        #endregion
    }
}

