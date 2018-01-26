using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Entity.TicketModel;
namespace SunNet.PMNew.Impl.SqlDataProvider.Ticket
{
    /// <summary>
    /// Data access:TicketHistorys
    /// </summary>
    public class TicketHistorysRepositorySqlDataProvider : SqlHelper, ITicketsHistoryRepository
    {
        public TicketHistorysRepositorySqlDataProvider()
        { }
        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(TicketHistorysEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TicketHistorys(");
            strSql.Append("TicketID,Description,ModifiedOn,ModifiedBy,ResponsibleUserId)");

            strSql.Append(" values (");
            strSql.Append("@TicketID,@Description,@ModifiedOn,@ModifiedBy,@ResponsibleUserId)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, model.ModifiedOn);
                db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, model.ModifiedBy);
                db.AddInParameter(dbCommand, "ResponsibleUserId", DbType.Int32, model.ResponsibleUserId);
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
        public bool Update(TicketHistorysEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TicketHistorys set ");
            strSql.Append("TicketID=@TicketID,");
            strSql.Append("Description=@Description,");
            strSql.Append("ModifiedOn=@ModifiedOn,");
            strSql.Append("ModifiedBy=@ModifiedBy");
            strSql.Append(" where TDHID=@TDHID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TDHID", DbType.Int32, model.TDHID);
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, model.ModifiedOn);
                db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, model.ModifiedBy);
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
        public bool Delete(int TDHID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TicketHistorys ");
            strSql.Append(" where TDHID=@TDHID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TDHID", DbType.Int32, TDHID);
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
        public TicketHistorysEntity Get(int TDHID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TicketHistorys ");
            strSql.Append(" where TDHID=@TDHID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TDHID", DbType.Int32, TDHID);
                TicketHistorysEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        model = TicketHistorysEntity.ReaderBind(dataReader);
                    }
                }
                return model;
            }
        }

        #endregion  Method

        #region ITicketsHistoryRepository Members
        #endregion


        public List<TicketHistorysEntity> GetHistoryListByTicketID(int TicketID)
        {
            List<TicketHistorysEntity> list = new List<TicketHistorysEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TicketHistorys ");
            strSql.Append(" where TicketID = @TicketID order by ModifiedOn desc ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, TicketID);

                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        list.Add(TicketHistorysEntity.ReaderBind(dataReader));
                    }
                }
            }
            return list;
        }

        #region ITicketsHistoryRepository Members



        #endregion
    }
}

