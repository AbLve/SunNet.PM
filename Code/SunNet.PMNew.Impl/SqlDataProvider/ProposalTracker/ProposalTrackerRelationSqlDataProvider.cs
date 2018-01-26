using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.ProposalTrackerModule;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Impl.SqlDataProvider.ProposalTracker
{
    public class ProposalTrackerRelationSqlDataProvider : SqlHelper, IProposalTrackerRelationRepository
    {

        public int Insert(ProposalTrackerRelationEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ProposalTrackerRelation(");
            strSql.Append("WID,TID,CreatedOn,CreatedBy)");

            strSql.Append(" values (");
            strSql.Append("@WID,@TID,@CreatedOn,@CreatedBy)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "WID", DbType.Int32, model.WID);
                db.AddInParameter(dbCommand, "TID", DbType.String, model.TID);
                db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, DateTime.Now);
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


        public bool Update(ProposalTrackerRelationEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ProposalTrackerRelation set ");
            strSql.Append("WID=@WID,");
            strSql.Append("TID=@TID,");
            strSql.Append("CreatedOn=@CreatedOn,");
            strSql.Append("CreatedBy=@CreatedBy");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ID", DbType.Int32, model.ID);
                db.AddInParameter(dbCommand, "WID", DbType.Int32, model.WID);
                db.AddInParameter(dbCommand, "TID", DbType.String, model.TID);
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


        public bool Delete(int WID, int TID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProposalTrackerRelation ");
            strSql.Append(" where WID=@WID and TID=@TID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "WID", DbType.Int32, WID);
                db.AddInParameter(dbCommand, "TID", DbType.Int32, TID);
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

        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProposalTrackerRelation ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
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


        public ProposalTrackerRelationEntity Get(int WID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,WID,TID,CreatedOn,CreatedBy from ProposalTrackerRelation ");
            strSql.Append(" where WID=@WID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "WID", DbType.Int32, WID);
                ProposalTrackerRelationEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        model = ProposalTrackerRelationEntity.ReaderBind(dataReader);
                    }
                }
                return model;
            }
        }


        public List<TicketsEntity> GetAllRelation(int wid)
        {
            List<string> listR = new List<string>();

            StringBuilder strSql = new StringBuilder();

            strSql.Append(@"WITH Tickets AS (SELECT * FROM DBO.GetTicketsView(@UserId))
                            select * from Tickets where ticketid in
                            (select tid from ProposalTrackerRelation where wid =@wid)");
            List<TicketsEntity> list = new List<TicketsEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "wid", DbType.Int32, wid);
                    db.AddInParameter(dbCommand, "UserId", DbType.Int32, IdentityContext.UserID);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        while (dataReader.Read())
                        {
                            list.Add(TicketsEntity.ReaderBind(dataReader, false));
                        }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
            return list;
        }

    }
}
