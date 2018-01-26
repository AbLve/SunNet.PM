using FamilyBook.Core.DocManagementModule;
using FamilyBook.Entity.DocManagements;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SF.Framework.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace FamilyBook.Impl.DocManagements
{
    public class DirectoryObjectDAOSQLDataProvider : IDirectoryObjectDAO
    {
        #region IDirectoryObjectRepository Members

        public List<DirectoryEntity> GetObjects(int parentID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select do.ID,do.Logo,do.[Type],do.ObjectID,t.Title,t.[Description],do.CreatedOn,do.CreatedBy,do.ModifiedOn,do.ModifiedBy,
                            u.FirstName,u.LastName,u.UserName,@ParentID as ParentID 
                            from DirectoryObjects do left join Tickets t on do.ObjectID=t.TicketID left join Users u on do.CreatedBy=u.UserID
                            where DirectoryID=@ParentID and do.Type='Ticket'
                            union all
                            select do.ID,do.Logo,do.[Type],do.ObjectID,f.FileTitle+f.ContentType as Title,f.ThumbPath as [Description],do.CreatedOn,do.CreatedBy,do.ModifiedOn,do.ModifiedBy,
                            u.FirstName,u.LastName,u.UserName,@ParentID as ParentID 
                            from DirectoryObjects do left join  dbo.Files f on do.ObjectID=f.FileID left join Users u on do.CreatedBy=u.UserID
                            where DirectoryID=@ParentID and do.Type='File'");
            StringBuilder strWhere = new StringBuilder();
            List<DirectoryEntity> list = new List<DirectoryEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ParentID", DbType.Int32, parentID);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        while (dataReader.Read())
                        {
                            list.Add(DirectoryEntity.ReaderBind(dataReader));
                        }
                    return list;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

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
                    return -2;
                }
            }
        }

        public bool Update(DirectoryObjectsEntity entity)
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
                    return null;
                }
            }
        }

        public int ChangeParent(string objects, int parentid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("IF not Exists(select 1 from Directories where ID=@ID)  begin ");
            strSql.Append(" select -1; end ");
            strSql.Append(" else begin ");
            strSql.Append("update DirectoryObjects set ");
            strSql.Append("DirectoryID=@ID");
            strSql.AppendFormat(" where ID in ({0});select 0; ", objects);
            strSql.Append(" end ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, parentid);
                    int result;
                    object obj = db.ExecuteScalar(dbCommand);
                    if (!int.TryParse(obj.ToString(), out result))
                    {
                        return -2;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }
               
        #endregion

    }
}
