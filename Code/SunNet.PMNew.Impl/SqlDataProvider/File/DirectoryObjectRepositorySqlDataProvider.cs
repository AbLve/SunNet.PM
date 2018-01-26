using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.FileModule;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Impl.SqlDataProvider.File
{
    public class DirectoryObjectRepositorySqlDataProvider : SqlHelper, IDirectoryObjectRepository
    {
        #region IDirectoryObjectRepository Members

        #endregion

        #region IRepository<DirectoryObjectsEntity> Members

        public int Insert(DirectoryObjectsEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            // first
            strSql.Append(@"if exists (select 1 from [DirectoryObjects] where DirectoryID=@DirectoryID and ObjectID=@ObjectID and [Type]=@Type)  
                            begin 
                            select 0 
                            end 
                            else 
                            begin ");
            strSql.Append(@"if not exists (select 1 from Directories where ID=@DirectoryID) 
                            begin
                            select -1
                            end
                            else
                            begin ");
            strSql.Append(" insert into DirectoryObjects(");
            strSql.Append("DirectoryID,Type,Logo,ObjectID,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy)");
            strSql.Append(" values (");
            strSql.Append("@DirectoryID,@Type,@Logo,@ObjectID,@CreatedOn,@CreatedBy,@ModifiedOn,@ModifiedBy)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0); ");
            strSql.Append(" end ");
            //first end
            strSql.Append(" end");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "DirectoryID", DbType.Int32, model.DirectoryID);
                    db.AddInParameter(dbCommand, "Type", DbType.String, model.ObjectType.ToString());
                    db.AddInParameter(dbCommand, "Logo", DbType.AnsiString, model.Logo);
                    db.AddInParameter(dbCommand, "ObjectID", DbType.Int32, model.ObjectID);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                    db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, model.ModifiedOn);
                    db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, model.ModifiedBy);
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
                    return -2;
                }
            }
        }

        public bool Update(SunNet.PMNew.Entity.FileModel.DirectoryObjectsEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from DirectoryObjects ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);
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

        public DirectoryObjectsEntity Get(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from DirectoryObjects ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);
                    DirectoryObjectsEntity model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = DirectoryObjectsEntity.ReaderBind(dataReader);
                        }
                    }
                    return model;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        #endregion
    }
}
