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
    public class SealNotesRepositorySqlDataProvider : SqlHelper, ISealNotesRepository
    {

        public List<SealNotesEntity> GetList(int sealRequestId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select n.* ,u.FirstName ,u.LastName from SealNotes n ")
                .Append(" left join users u on n.userid = u.userid ")
                .Append(" where SealRequestsID = @SealRequestsID;");

            List<SealNotesEntity> list = new List<SealNotesEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                db.AddInParameter(dbCommand, "SealRequestsID", DbType.Int32, sealRequestId);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                        list.Add(new SealNotesEntity(dataReader, true));
                }
            }
            return list;
        }

        public int Insert(SealNotesEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into dbo.SealNotes(");
            strSql.Append("Title,SealRequestsID,Description,UserID,CreateOn)");

            strSql.Append(" values (");
            strSql.Append("@Title,@SealRequestsID,@Description,@UserID,@CreateOn)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "Title", DbType.String, entity.Title);
                db.AddInParameter(dbCommand, "Description", DbType.String, entity.Description);
                db.AddInParameter(dbCommand, "SealRequestsID", DbType.Int32, entity.SealRequestsID);
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, entity.UserID);
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

        public bool Update(SealNotesEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public SealNotesEntity Get(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
