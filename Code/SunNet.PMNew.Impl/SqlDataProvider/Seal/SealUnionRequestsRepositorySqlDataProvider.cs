using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.SealModel;
using SunNet.PMNew.Entity.SealModel;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace SunNet.PMNew.Impl.SqlDataProvider.Seal
{
    public class SealUnionRequestsRepositorySqlDataProvider : SqlHelper, ISealUnionRequestsRepository
    {
        public int Insert(SealUnionRequestsEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into dbo.SealUnionRequests(");
            strSql.Append("SealRequestsID,SealID,ApprovedBy,ApprovedDate,IsSealed,SealedBy,SealedDate)");

            strSql.Append(" values (");
            strSql.Append("@SealRequestsID,@SealID,@ApprovedBy,@ApprovedDate,@IsSealed,@SealedBy,@SealedDate)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "SealRequestsID", DbType.Int32, entity.SealRequestsID);
                db.AddInParameter(dbCommand, "SealID", DbType.Int32, entity.SealID);

                db.AddInParameter(dbCommand, "ApprovedBy", DbType.Int32, entity.ApprovedBy);
                db.AddInParameter(dbCommand, "ApprovedDate", DbType.DateTime, entity.ApprovedDate);

                db.AddInParameter(dbCommand, "IsSealed", DbType.Boolean, entity.IsSealed);
                db.AddInParameter(dbCommand, "SealedBy", DbType.Int32, entity.SealedBy);
                db.AddInParameter(dbCommand, "SealedDate", DbType.DateTime, entity.SealedDate);

                int result;
                object obj = db.ExecuteScalar(dbCommand);
                if (!int.TryParse(obj.ToString(), out result))
                {
                    return 0;
                }
                return result;
            }
        }

        public bool Update(SealUnionRequestsEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool UpdateApprovedDate(int sealRequestId, int userID, DateTime date)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SealUnionRequests");
            strSql.Append(" set ApprovedDate =@ApprovedDate, ApprovedBy=@ApprovedBy ");
            strSql.Append(" where SealRequestsID= @SealRequestsID ");
            
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "SealRequestsID", DbType.Int32, sealRequestId);
                db.AddInParameter(dbCommand, "ApprovedDate", DbType.DateTime, date);
                db.AddInParameter(dbCommand, "ApprovedBy", DbType.Int32, userID);

                return (db.ExecuteNonQuery(dbCommand) > 0);
            }
        }

        public bool UpdateSealedDate(int sealRequestId, int userID, DateTime date)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dbo.SealUnionRequests");
            strSql.Append(" set SealedDate =@SealedDate ,IsSealed=1, SealedBy=@SealedBy ");
            strSql.Append(" where SealRequestsID= @SealRequestsID ");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "SealRequestsID", DbType.Int32, sealRequestId);
                db.AddInParameter(dbCommand, "SealedDate", DbType.DateTime, date);
                db.AddInParameter(dbCommand, "SealedBy", DbType.Int32, userID);

                return (db.ExecuteNonQuery(dbCommand) > 0);
            }
        }

        public bool Delete(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Delete dbo.SealUnionRequests")
                .Append(" where id = @ID;");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);

                return db.ExecuteNonQuery(dbCommand) > 0;
            }
        }

        public SealUnionRequestsEntity Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<SealUnionRequestsEntity> GetList(int sealRequestsID)
        {
            List<SealUnionRequestsEntity> list = new List<SealUnionRequestsEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from SealUnionRequests where sealRequestsID = @SealRequestsID;");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "SealRequestsID", DbType.Int32, sealRequestsID);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                        list.Add(new SealUnionRequestsEntity(dataReader, true));
                }
            }
            return list;
        }

        public List<SealUnionRequestsEntity> GetApprovedByList(int sealRequestsID)
        {
            List<SealUnionRequestsEntity> list = new List<SealUnionRequestsEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" with  List as(select ApprovedBy from dbo.SealUnionRequests where SealRequestsID=@SealRequestsID  group by ApprovedBy) ")
                .Append(" select u.userid ,u.firstname ,u.lastname ,u.email from list l ")
                .Append(" left join users u on l.ApprovedBy = u.userid ");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "SealRequestsID", DbType.Int32, sealRequestsID);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                        list.Add(new SealUnionRequestsEntity(dataReader));
                }
            }
            return list;
        }

        public List<SealUnionRequestsEntity> GetSealedByList(int sealRequestsID)
        {
            List<SealUnionRequestsEntity> list = new List<SealUnionRequestsEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" with  List as(select SealedBy from dbo.SealUnionRequests where SealRequestsID=@SealRequestsID  group by SealedBy) ")
                .Append(" select u.userid ,u.firstname ,u.lastname ,u.email from list l ")
                .Append(" left join users u on l.SealedBy = u.userid ");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "SealRequestsID", DbType.Int32, sealRequestsID);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                        list.Add(new SealUnionRequestsEntity(dataReader));
                }
            }
            return list;
        }
    }
}
