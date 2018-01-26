using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Impl.SqlDataProvider.Ticket
{
    public class TicketsRelationRepositorySqlDataProvider : SqlHelper, ITicketsRelationRespository
    {
        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(TicketsRelationEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TicketsRelation(");
            strSql.Append("TID,RTID,CreatedOn,CreatedBy)");

            strSql.Append(" values (");
            strSql.Append("@TID,@RTID,@CreatedOn,@CreatedBy)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TID", DbType.Int32, model.TID);
                db.AddInParameter(dbCommand, "RTID", DbType.String, model.RTID);
                db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
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
        public bool Update(TicketsRelationEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TicketsRelation set ");
            strSql.Append("TID=@TID,");
            strSql.Append("RTID=@RTID,");
            strSql.Append("CreatedOn=@CreatedOn,");
            strSql.Append("CreatedBy=@CreatedBy");
            strSql.Append(" where RID=@RID and RID=@RID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "RID", DbType.Int32, model.RID);
                db.AddInParameter(dbCommand, "TID", DbType.Int32, model.TID);
                db.AddInParameter(dbCommand, "RTID", DbType.String, model.RTID);
                db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
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

        public bool Delete(int RID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TicketsRelation ");
            strSql.Append(" where RID=@RID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "RID", DbType.Int32, RID);
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
        public TicketsRelationEntity Get(int TID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select RID,TID,RTID,CreatedOn,CreatedBy from TicketsRelation ");
            strSql.Append(" where TID=@TID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TID", DbType.Int32, TID);
                TicketsRelationEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        model = TicketsRelationEntity.ReaderBind(dataReader);
                    }
                }
                return model;
            }
        }

        #region get all relation, when  getType = true, return link list; else is false ,return id list


        public string GetAllRelationStringById(int tid, bool GetType)
        {

            string stringRid = "";

            List<string> listT = new List<string>();

            StringBuilder strSql = new StringBuilder();

            strSql.Append("select * from TicketsRelation");
            strSql.Append("  where TID = @TID ");


            Database db = DatabaseFactory.CreateDatabase();

            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {

                    db.AddInParameter(dbCommand, "TID", DbType.Int32, tid);

                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {

                            listT.Add(TicketsRelationEntity.ReaderBind(dataReader).RTID.ToString() + ' ');

                            //     listT.Add(string.Format("<a href='#' onclick='ViewTicketModuleDialog({0})'  title='View Related Ticket'>", TicketsRelationEntity.ReaderBind(dataReader).RTID.ToString()) + TicketsRelationEntity.ReaderBind(dataReader).RTID.ToString() + "</a>");

                        }
                    }

                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[Sql:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }

            }
            foreach (string item in listT.Union(GetAllRelationStringByTicketId(tid, GetType)))
            {
                if (item.Length > 0)
                {
                    stringRid += item;
                }

            }
            return stringRid;
        }
        public List<string> GetAllRelationStringByTicketId(int rid, bool GetType)
        {
            List<string> listR = new List<string>();

            StringBuilder strSql = new StringBuilder();

            strSql.Append("select * from TicketsRelation");
            strSql.Append("  where RTID = @RTID");

            Database db = DatabaseFactory.CreateDatabase();

            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {

                    db.AddInParameter(dbCommand, "RTID", DbType.Int32, rid);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            //if (GetType)
                            //{
                            listR.Add(TicketsRelationEntity.ReaderBind(dataReader).TID.ToString() + ' ');
                            //}
                            //else
                            //{
                            //    listR.Add(string.Format("<a href='#' onclick='OpenEditModuleDialog({0})' title='Edit Ticket'>", TicketsRelationEntity.ReaderBind(dataReader).TID.ToString()) + TicketsRelationEntity.ReaderBind(dataReader).TID.ToString() + "</a>");
                            //}
                        }
                    }

                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[Sql:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }

            }
            return listR;
        }

        #endregion
    }
}
