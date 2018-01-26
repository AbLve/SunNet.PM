using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.SealModel;
using SunNet.PMNew.Entity.SealModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Impl.SqlDataProvider.Seal
{
    public class SealsRepositorySqlDataProvider : SqlHelper, ISealsRepository
    {
        public int Insert(SealsEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Seals(");
            strSql.Append("SealName,Owner,Approver,Description,Status,CreatedOn)");

            strSql.Append(" values (");
            strSql.Append("@SealName,@Owner,@Approver,@Description,@Status,@CreatedOn)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "SealName", DbType.String, entity.SealName);
                db.AddInParameter(dbCommand, "Owner", DbType.Int32, entity.Owner);
                db.AddInParameter(dbCommand, "Approver", DbType.Int32, entity.Approver);
                db.AddInParameter(dbCommand, "Description", DbType.String, entity.Description);
                db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)entity.Status);
                db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, entity.CreatedOn);

                int result;
                object obj = db.ExecuteScalar(dbCommand);
                if (!int.TryParse(obj.ToString(), out result))
                {
                    return 0;
                }
                return result;
            }
        }


        public bool Update(SealsEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Seals")
            .Append(" set SealName =@SealName ,Owner=@Owner,Approver=@Approver,Description=@Description,Status=@Status");
                strSql.Append(" where id=@id");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ID", DbType.Int32, entity.ID);

                db.AddInParameter(dbCommand, "SealName", DbType.String, entity.SealName);
                db.AddInParameter(dbCommand, "Owner", DbType.Int32, entity.Owner);
                db.AddInParameter(dbCommand, "Approver", DbType.Int32, entity.Approver);
                db.AddInParameter(dbCommand, "Description", DbType.String, entity.Description);
                db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)entity.Status);

                return db.ExecuteNonQuery(dbCommand) > 0;
            }
        }


        public bool Delete(int entityId)
        {
            throw new NotImplementedException();
        }


        public SealsEntity Get(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Seals where id = @ID");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                        return new SealsEntity(dataReader, false);
                }
            }
            return null;
        }
        
        
        public List<SealsEntity> GetList()
        {
            List<SealsEntity> list = new List<SealsEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select s.* ,u.FirstName as OwnerFirstName, u.LastName as OwnerLastName , u2.FirstName as ApproverFirstName ,u2.LastName as ApproverLastName ")
            .Append(" from dbo.Seals s  left join dbo.Users u on u.userid = s.Owner left join dbo.Users u2 on u2.userid = s.Approver ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                        list.Add(new SealsEntity(dataReader, true));
                }
            }
            return list ;
        }

        /// <summary>
        /// 判断 SealName 是否重复，有返回 True ,否则返回 False
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sealName"></param>
        /// <returns></returns>
        public bool CheckSealName(int id, string sealName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(0) from dbo.Seals where id!=@id and SealName=@SealName;");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "SealName", DbType.String, sealName.FilterSqlString());
                db.AddInParameter(dbCommand, "id", DbType.Int32, id);

                int result;
                object obj = db.ExecuteScalar(dbCommand);
                if (!int.TryParse(obj.ToString(), out result))
                {
                    return false ;
                }
                return result > 0;
            }
        }

    }
}
