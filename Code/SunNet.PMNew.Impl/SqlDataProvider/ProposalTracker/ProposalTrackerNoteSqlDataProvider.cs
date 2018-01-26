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
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Impl.SqlDataProvider.ProposalTracker
{
    public class ProposalTrackerNoteSqlDataProvider : SqlHelper, IProposalTrackerNoteRepository
    {

        public int Insert(ProposalTrackerNoteEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ProposalTrackerNote(");
            strSql.Append("ProposalTrackerID,Title,Description,ModifyOn,ModifyBy)");

            strSql.Append(" values (");
            strSql.Append("@ProposalTrackerID,@Title,@Description,@ModifyOn,@ModifyBy)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ProposalTrackerID", DbType.Int32, model.ProposalTrackerID);
                db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                db.AddInParameter(dbCommand, "ModifyOn", DbType.DateTime, DateTime.Now);
                db.AddInParameter(dbCommand, "ModifyBy", DbType.Int32, model.ModifyBy);
                int result;
                object obj = db.ExecuteScalar(dbCommand);
                if (!int.TryParse(obj.ToString(), out result))
                {
                    return 0;
                }
                return result;
            }
        }


        public bool Update(ProposalTrackerNoteEntity model)
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
                //db.AddInParameter(dbCommand, "ID", DbType.Int32, model.ID);
                //db.AddInParameter(dbCommand, "WID", DbType.Int32, model.WID);
                //db.AddInParameter(dbCommand, "TID", DbType.String, model.TID);
                //db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                //db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
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


        public ProposalTrackerNoteEntity Get(int WID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,WID,TID,CreatedOn,CreatedBy from ProposalTrackerRelation ");
            strSql.Append(" where WID=@WID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "WID", DbType.Int32, WID);
                ProposalTrackerNoteEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        model = ProposalTrackerNoteEntity.ReaderBind(dataReader);
                    }
                }
                return model;
            }
        }

        public List<ProposalTrackerNoteEntity> GetProposalTrackerNotes(int proposalTrackerId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT w.*,u.UserName,u.LastName,u.FirstName from ProposalTrackerNote w left join Users u
                            on w.ModifyBy= u.UserID where ProposalTrackerID=@ProposalTrackerID");
            strSql.Append(" order by ModifyOn desc");
            List<ProposalTrackerNoteEntity> list = new List<ProposalTrackerNoteEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProposalTrackerID", DbType.Int32, proposalTrackerId);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        while (dataReader.Read())
                        {
                            list.Add(ProposalTrackerNoteEntity.ReaderBind(dataReader));
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
