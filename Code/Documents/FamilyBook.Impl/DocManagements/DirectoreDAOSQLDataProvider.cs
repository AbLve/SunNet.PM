using FamilyBook.Core.DocManagementModule;
using FamilyBook.Entity.DocManagements;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace FamilyBook.Impl.DocManagements
{
    public class DirectoreDAOSQLDataProvider:IDirectoryDAO
    {
        #region IDirectoryRepository Members
        public List<DirectoryEntity> GetAllDirectories()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select d.*,'Directory' as Type,u.FirstName,u.LastName,u.UserName,ID as ObjectID ");
            strSql.Append(" FROM Directories d  left join Users u on d.CreatedBy=u.UserID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        List<DirectoryEntity> list = new List<DirectoryEntity>();
                        while (dataReader.Read())
                        {
                            list.Add(DirectoryEntity.ReaderBind(dataReader));
                        }
                        return list;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        #endregion

        #region IRepository<DirectoryEntity> Members

        public int Insert(DirectoryEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("IF not Exists(select 1 from Directories where ID=@ID)  begin ");
            strSql.Append("insert into Directories(");
            strSql.Append("Title,Description,Logo,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,ParentID)");
            strSql.Append(" values (");
            strSql.Append("@Title,@Description,@Logo,@CreatedOn,@CreatedBy,@ModifiedOn,@ModifiedBy,@ParentID)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0); end ");
            strSql.Append(" else begin ");
            strSql.Append(" update Directories set ");
            strSql.Append("Title=@Title,");
            strSql.Append("Description=@Description,");
            strSql.Append("Logo=@Logo,");
            strSql.Append("CreatedOn=@CreatedOn,");
            strSql.Append("CreatedBy=@CreatedBy,");
            strSql.Append("ModifiedOn=@ModifiedOn,");
            strSql.Append("ModifiedBy=@ModifiedBy,");
            strSql.Append("ParentID=@ParentID");
            strSql.Append(" where ID=@ID; ");
            strSql.Append(" select @ID; ");
            strSql.Append(" end ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, model.ID);
                    db.AddInParameter(dbCommand, "Title", DbType.AnsiString, model.Title);
                    db.AddInParameter(dbCommand, "Description", DbType.AnsiString, model.Description);
                    db.AddInParameter(dbCommand, "Logo", DbType.AnsiString, model.Logo);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                    db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, model.ModifiedOn);
                    db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, model.ModifiedBy);
                    db.AddInParameter(dbCommand, "ParentID", DbType.Int32, model.ParentID);
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
                    return 0;
                }
            }
        }

        public bool Update(DirectoryEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int entityId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"if exists (select 1 from [DirectoryObjects] where DirectoryID=@ID ) 
                            begin
                            select 0
                            end
                            else
                            begin ");
            strSql.Append(" delete from Directories ");
            strSql.Append(" where ID=@ID;select 1;");
            strSql.Append(" end ");
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);
                    int result;
                    object obj = db.ExecuteScalar(dbCommand);
                    if (!int.TryParse(obj.ToString(), out result))
                    {
                        return false;
                    }
                    else if (result == 1)
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

        public DirectoryEntity Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int ChangeParent(string directories, int parentid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("IF not Exists(select 1 from Directories where ID=@ID)  begin ");
            strSql.Append(" select -1; end ");
            strSql.Append(" else begin ");
            strSql.Append("update Directories set ");
            strSql.Append("ParentID=@ID");
            strSql.AppendFormat(" where ID in ({0});select 0; ", directories);
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
