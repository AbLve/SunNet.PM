using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.SealModel;
using SunNet.PMNew.Entity.SealModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace SunNet.PMNew.Impl.SqlDataProvider.Seal
{
    public class SealFilesRepositorySqlDataProvider : SqlHelper, ISealFileRepository
    {
      
        public int Insert(SealFileEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into dbo.SealFiles(");
            strSql.Append("Path,Name,Title,SealRequestsID,UserID,IsDeleted,WorkflowHistoryID,CreateOn)");

            strSql.Append(" values (");
            strSql.Append("@Path,@Name,@Title,@SealRequestsID,@UserID,@IsDeleted,@WorkflowHistoryID,@CreateOn)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "Path", DbType.String, entity.Path);
                db.AddInParameter(dbCommand, "Title", DbType.String, entity.Title);
                db.AddInParameter(dbCommand, "Name", DbType.String, entity.Name);
                db.AddInParameter(dbCommand, "SealRequestsID", DbType.Int32, entity.SealRequestsID);
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, entity.UserID);
                db.AddInParameter(dbCommand, "IsDeleted", DbType.Boolean, entity.IsDeleted);
                db.AddInParameter(dbCommand, "WorkflowHistoryID", DbType.Int32, entity.WorkflowHistoryID);
                db.AddInParameter(dbCommand, "CreateOn", DbType.DateTime, entity.CreateOn);

                int result;
                object obj = db.ExecuteScalar(dbCommand);
                if (!int.TryParse(obj.ToString(), out result))
                {
                    return 0;
                }
                return result;
            }
        }

        public bool Update(SealFileEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int entityId,int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete dbo.SealFiles")
                .Append(" where id = @id and UserID= @UserID;");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "id", DbType.Int32, entityId);
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
                return db.ExecuteNonQuery(dbCommand) > 0;
            }
        }


        public SealFileEntity Get(int entityId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("  select * from SealFiles  ")
                .Append(" where ID = @ID and IsDeleted=0");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                        return new SealFileEntity(dataReader, false);
                }
            }
            return null;
        }


        public List<SealFileEntity> GetList(int sealRequestId, int wfhisID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select f.* ,u.FirstName ,u.LastName from SealFiles f ")
                .Append(" left join users u on f.userid = u.userid ")
                .Append(" where SealRequestsID = @SealRequestsID and IsDeleted=0 and WorkflowHistoryID = @WorkflowHistoryID ");

            List<SealFileEntity> list = new List<SealFileEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                db.AddInParameter(dbCommand, "SealRequestsID", DbType.Int32, sealRequestId);
                db.AddInParameter(dbCommand, "WorkflowHistoryID", DbType.Int32, wfhisID);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                        list.Add(new SealFileEntity(dataReader, true));
                }
            }
            return list;
        }

        public List<SealFileEntity> GetListByHistoryID(int wfhisID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select * from SealFiles f ")
                .Append(" where WorkflowHistoryID = @WorkflowHistoryID and IsDeleted=0");

            List<SealFileEntity> list = new List<SealFileEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                db.AddInParameter(dbCommand, "WorkflowHistoryID", DbType.Int32, wfhisID);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                        list.Add(new SealFileEntity(dataReader, false));
                }
            }
            return list;
        }
    }
}
