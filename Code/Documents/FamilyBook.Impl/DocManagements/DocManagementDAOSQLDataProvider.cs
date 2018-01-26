using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyBook.Core.DocManagementModule;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using FamilyBook.Entity.DocManagements;
using SF.Framework.Log;

#region Version Info
/* ========================================================================
*
* Author: lynnm Date: 2013/12/2 9:51:24
* File Name: DocManagementDAOSQLDataProvider
* Version: 4.0.30319.1008
*
* ========================================================================
*/
#endregion
namespace FamilyBook.Impl.DocManagements
{
    public class DocManagementDAOSQLDataProvider : IDocManagementDAO
    {
        /// <summary>
        /// 添加Doc
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>最新数据ID</returns>
        public int Insert(DocManagementEntity entity)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into DocManagements(");
            sb.Append("ProjectID,");
            sb.Append("CompanyID,");
            sb.Append("UserID,");
            sb.Append("ParentID,");
            sb.Append("Type,");
            sb.Append("FileName,");
            sb.Append("DisplayFileName,");
            sb.Append("FileContentType,");
            sb.Append("FileUrl,");
            sb.Append("UpdatedOn,");
            sb.Append("CreatedOn,");
            sb.Append("Extenstions,");
            sb.Append("FileSize,");
            sb.Append("IsDeleted)");

            sb.Append(" values(");

            sb.Append("@ProjectID,");
            sb.Append("@CompanyID,");
            sb.Append("@UserID,");
            sb.Append("@ParentID,");
            sb.Append("@Type,");
            sb.Append("@FileName,");
            sb.Append("@DisplayFileName,");
            sb.Append("@FileContentType,");
            sb.Append("@FileUrl,");
            sb.Append("@UpdatedOn,");
            sb.Append("@CreatedOn,");
            sb.Append("@Extenstions,");
            sb.Append("@FileSize,");
            sb.Append("@IsDeleted);");

            sb.Append(" SELECT @@IDENTITY as ID;");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, entity.ProjectID);
                    db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, entity.CompanyID);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, entity.UserID);
                    db.AddInParameter(dbCommand, "ParentID", DbType.Int32, entity.ParentID);
                    db.AddInParameter(dbCommand, "Type", DbType.Int16, (int)entity.Type);
                    db.AddInParameter(dbCommand, "FileName", DbType.String, entity.FileName);
                    db.AddInParameter(dbCommand, "DisplayFileName", DbType.String, entity.DisplayFileName);
                    db.AddInParameter(dbCommand, "FileContentType", DbType.String, entity.FileContentType);
                    db.AddInParameter(dbCommand, "FileUrl", DbType.String, entity.FileUrl);
                    db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, entity.UpdatedOn);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, entity.CreatedOn);
                    db.AddInParameter(dbCommand, "Extenstions", DbType.String, entity.Extenstions);
                    db.AddInParameter(dbCommand, "FileSize", DbType.Int32, entity.FileSize);
                    db.AddInParameter(dbCommand, "IsDeleted", DbType.Boolean, entity.IsDeleted);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        int id = 0;
                        if (dataReader.Read())
                            id = Convert.ToInt32(dataReader["ID"]);
                        return id;
                    }
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(sb.ToString(), dbCommand.Parameters, ex);
                    return 0;
                }
            }
        }

        public bool Update(DocManagementEntity entity)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("update DocManagements set ");
            sb.Append("ProjectID=@ProjectID,");
            sb.Append("UserID=@UserID,");
            sb.Append("ParentID=@ParentID,");
            sb.Append("Type=@Type,");
            sb.Append("FileName=@FileName,");
            sb.Append("DisplayFileName=@DisplayFileName,");
            sb.Append("FileContentType=@FileContentType,");
            sb.Append("UpdatedOn=@UpdatedOn,");
            sb.Append("CreatedOn=@CreatedOn,");
            sb.Append("Extenstions=@Extenstions,");
            sb.Append("FileSize=@FileSize,");
            sb.Append("IsDeleted=@IsDeleted");
            sb.Append(" where ID=@ID");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, entity.ProjectID);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, entity.UserID);
                    db.AddInParameter(dbCommand, "ParentID", DbType.Int32, entity.ParentID);
                    db.AddInParameter(dbCommand, "Type", DbType.Int16, (int)entity.Type);
                    db.AddInParameter(dbCommand, "FileName", DbType.String, entity.FileName);
                    db.AddInParameter(dbCommand, "DisplayFileName", DbType.String, entity.DisplayFileName);
                    db.AddInParameter(dbCommand, "FileContentType", DbType.String, entity.FileContentType);
                    db.AddInParameter(dbCommand, "FileUrl", DbType.String, entity.FileUrl);
                    db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, entity.UpdatedOn);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, entity.CreatedOn);
                    db.AddInParameter(dbCommand, "Extenstions", DbType.String, entity.Extenstions);
                    db.AddInParameter(dbCommand, "FileSize", DbType.Int32, entity.FileSize);
                    db.AddInParameter(dbCommand, "IsDeleted", DbType.Boolean, entity.IsDeleted);
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entity.ID);
                    return db.ExecuteNonQuery(dbCommand) > 0;
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(sb.ToString(), dbCommand.Parameters, ex);
                    return false;
                }
            }
        }

        public bool Delete(int entityId)
        {
            string sql = "update DocManagements set Isdeleted=1 where ID=@ID";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);
                    db.ExecuteNonQuery(dbCommand);
                    return true;
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(sql.ToString(), dbCommand.Parameters, ex);
                    return false;
                }
            }
        }

        public bool Delete(int projectId, int id)
        {
            string sql = "update DocManagements set Isdeleted=1 where (ID=@ID or ParentID=@ID) and ProjectID=@ProjectID";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, id);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, projectId);
                    db.ExecuteNonQuery(dbCommand);
                    return true;
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(sql.ToString(), dbCommand.Parameters, ex);
                    return false;
                }
            }
        }

        public DocManagementEntity Get(int entityId)
        {
            DocManagementEntity entity = new DocManagementEntity();
            string sql = "select * from DocManagements where ID=@ID and Isdeleted=0";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            entity = new DocManagementEntity(dataReader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(sql.ToString(), dbCommand.Parameters, ex);
                }
            }
            return entity;
        }

        public List<DocManagementEntity> GetList(int parentId)
        {
            List<DocManagementEntity> list = new List<DocManagementEntity>();
            string sql = "select * from DocManagements where ParentID=@ParentID and Isdeleted=0 order by type,DisplayFileName";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ParentID", DbType.Int32, parentId);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            DocManagementEntity entity = new DocManagementEntity(dataReader);
                            list.Add(entity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(sql.ToString(), dbCommand.Parameters, ex);
                }
            }
            return list;
        }


        public List<DocManagementEntity> GetList(int parentId, string order = "", string sort = "")
        {
            List<DocManagementEntity> list = new List<DocManagementEntity>();
            string sql = @"select *,p.Title as ProjectName,Users.FirstName, Users.LastName  from DocManagements 
                            left join [dbo].[Projects] as p  on DocManagements.ProjectID = p.ProjectID
                            left join Users on DocManagements.UserID=Users.UserID
                            where DocManagements.ProjectID=@ParentID and Isdeleted=0 order by ";
            if (order != "" && sort != "")
            {
                sql += order + " " + sort;
            }
            else
            {
                sql += " type,DisplayFileName";
            }
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ParentID", DbType.Int32, parentId);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            DocManagementEntity entity = new DocManagementEntity(dataReader);
                            list.Add(entity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(sql.ToString(), dbCommand.Parameters, ex);
                }
            }
            return list;
        }

        public List<DocManagementEntity> GetList(int projectId, int parentId, string order = "", string sort = "")
        {
            List<DocManagementEntity> list = new List<DocManagementEntity>();
            string sql = @"select *,p.Title as ProjectName,Users.FirstName, Users.LastName  from DocManagements 
                            left join [dbo].[Projects] as p  on DocManagements.ProjectID = p.ProjectID
                             left join Users on DocManagements.UserID=Users.UserID
                            where DocManagements.ProjectID=@ProjectID and ParentID=@ParentID and Isdeleted=0 order by ";
            if (order != "" && sort != "")
            {
                sql += order + " " + sort;
            }
            else
            {
                sql += " type,DisplayFileName";
            }
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, projectId);
                    db.AddInParameter(dbCommand, "ParentID", DbType.Int32, parentId);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            DocManagementEntity entity = new DocManagementEntity(dataReader);
                            list.Add(entity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(sql.ToString(), dbCommand.Parameters, ex);
                }
            }
            return list;
        }

        public List<DocManagementEntity> GetList(string projectId, int parentid, int userId, string order = "", string sort = "")
        {
            List<DocManagementEntity> list = new List<DocManagementEntity>();
            string sql = @"select *,p.Title as ProjectName,Users.FirstName, Users.LastName  from DocManagements 
                            left join [dbo].[Projects] as p  on DocManagements.ProjectID = p.ProjectID
                            left join Users on DocManagements.UserID=Users.UserID
                            where (DocManagements.ProjectID in(" + (string.IsNullOrEmpty(projectId.Trim()) ? "-1" : projectId) + @") 
                            or DocManagements.UserID=@UserID) and ParentID=@ParentID and Isdeleted=0 order by ";
            if (order != "" && sort != "")
            {
                sql += order + " " + sort;
            }
            else
            {
                sql += " p.Title,type,DisplayFileName";
            }
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ParentID", DbType.Int32, parentid);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            DocManagementEntity entity = new DocManagementEntity(dataReader);
                            list.Add(entity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(sql.ToString(), dbCommand.Parameters, ex);
                }
            }
            return list;
        }

        public List<DocManagementEntity> GetList(string filename)
        {
            List<DocManagementEntity> list = new List<DocManagementEntity>();
            string sql = "select *,Users.FirstName , Users.LastName  from DocManagements left join Users on DocManagements.UserID=Users.UserID where Isdeleted=0 ";
            if (!string.IsNullOrEmpty(filename))
                sql += " and DisplayFileName like '%'+@DisplayFileName+'%' ";
            sql += " order by type,DisplayFileName";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
            {
                try
                {
                    if (!string.IsNullOrEmpty(filename))
                        db.AddInParameter(dbCommand, "DisplayFileName", DbType.String, filename);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            DocManagementEntity entity = new DocManagementEntity(dataReader);
                            list.Add(entity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(sql.ToString(), dbCommand.Parameters, ex);
                }
            }
            return list;
        }

        public List<DocManagementEntity> GetList(string projectId, int userId, string filename)
        {
            List<DocManagementEntity> list = new List<DocManagementEntity>();
            string sql = @"select *,p.Title as ProjectName,Users.FirstName, Users.LastName  from DocManagements 
                                left join Users on DocManagements.UserID=Users.UserID 
                                left join [dbo].[Projects] as p on DocManagements.ProjectID = p.ProjectID
                                where Isdeleted=0 and (DocManagements.projectId in (" + projectId + ") or (DocManagements.projectId=0 and DocManagements.UserID=@UserID))";
            if (!string.IsNullOrEmpty(filename))
                sql += " and DisplayFileName like '%'+@DisplayFileName+'%' ";
            sql += " order by type,DisplayFileName";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
                    if (!string.IsNullOrEmpty(filename))
                        db.AddInParameter(dbCommand, "DisplayFileName", DbType.String, filename);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            DocManagementEntity entity = new DocManagementEntity(dataReader);
                            list.Add(entity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(sql.ToString(), dbCommand.Parameters, ex);
                }
            }
            return list;
        }

        public List<DocManagementEntity> GetList(int projectId, string filename)
        {
            List<DocManagementEntity> list = new List<DocManagementEntity>();
            string sql = @"select *,Users.FirstName, Users.LastName from DocManagements 
                                left join Users on DocManagements.UserID=Users.UserID 
                                where ProjectID=@ProjectID and Isdeleted=0 ";
            if (!string.IsNullOrEmpty(filename))
                sql += " and DisplayFileName like '%'+@DisplayFileName+'%' ";
            sql += " order by type,DisplayFileName";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, projectId);
                    if (!string.IsNullOrEmpty(filename))
                        db.AddInParameter(dbCommand, "DisplayFileName", DbType.String, filename);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            DocManagementEntity entity = new DocManagementEntity(dataReader);
                            list.Add(entity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(sql.ToString(), dbCommand.Parameters, ex);
                }
            }
            return list;
        }

        public List<DocManagementEntity> GetListByParentID(int userId, int parentId)
        {
            List<DocManagementEntity> list = new List<DocManagementEntity>();
            string sql = "select * from DocManagements where IsDeleted=0 and UserID=@UserID and parentId=@ParentID and ProjectID=0 order by type,DisplayFileName";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
            {
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
                db.AddInParameter(dbCommand, "ParentID", DbType.Int32, parentId);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        list.Add(new DocManagementEntity(dataReader));
                    }
                }
            }
            return list;
        }

        public List<DocManagementEntity> GetListByParentID(int userId, int parentId, int projectId, string filename)
        {
            List<DocManagementEntity> list = new List<DocManagementEntity>();
            string sql = "select * from DocManagements where IsDeleted=0 and UserID=@UserID and parentId=@ParentID and ProjectID = @ProjectID and DisplayFileName=@DisplayFileName order by type,DisplayFileName";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
            {
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
                db.AddInParameter(dbCommand, "ParentID", DbType.Int32, parentId);
                db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, projectId);
                db.AddInParameter(dbCommand, "DisplayFileName", DbType.String, filename);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        list.Add(new DocManagementEntity(dataReader));
                    }
                }
            }
            return list;
        }

        public List<DocManagementEntity> GetListByUserId(int userId)
        {
            List<DocManagementEntity> list = new List<DocManagementEntity>();
            string sql = "select * from DocManagements where IsDeleted=0  and UserID=@UserID and ProjectID=0 order by type,DisplayFileName";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
            {
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        list.Add(new DocManagementEntity(dataReader));
                    }
                }
            }
            return list;
        }
    }
}
