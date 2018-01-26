using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Core.ProjectModule;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Framework.Utils.Providers;
namespace SunNet.PMNew.Impl.SqlDataProvider.Project
{
    /// <summary>
    /// Data access:ProjectUsers
    /// </summary>
    public class ProjectUsersRepositorySqlDataProvider : SqlHelper, IProjectUsersRepository
    {
        public ProjectUsersRepositorySqlDataProvider()
        { }
        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(ProjectUsersEntity model)
        {
            StringBuilder strSql = new StringBuilder("IF NOT EXISTS(SELECT 1 FROM [ProjectUsers] WHERE [ProjectID] = @ProjectID AND [UserID] = @UserID ) BEGIN ");
            strSql.Append("insert into ProjectUsers(");
            strSql.Append("ProjectID,UserID,ISClient)");

            strSql.Append(" values (");
            strSql.Append("@ProjectID,@UserID,@ISClient)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            strSql.Append("END ELSE BEGIN ");
            strSql.Append(@"Set @PUID = (SELECT TOP 1 [PUID] FROM [ProjectUsers] 
                            WHERE [ProjectID] = @ProjectID AND [UserID] = @UserID ) ");
            strSql.Append("update ProjectUsers set ");
            strSql.Append("ProjectID=@ProjectID,");
            strSql.Append("UserID=@UserID,");
            strSql.Append("ISClient=@ISClient");
            strSql.Append(" where PUID=@PUID ; SELECT @PUID ;");
            strSql.Append("END ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "PUID", DbType.Int32, 0);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
                    db.AddInParameter(dbCommand, "ISClient", DbType.Boolean, model.ISClient);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return 0;
                }
            }
        }
        /// <summary>
        /// Update a record
        /// </summary>
        public bool Update(ProjectUsersEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ProjectUsers set ");
            strSql.Append("ProjectID=@ProjectID,");
            strSql.Append("UserID=@UserID,");
            strSql.Append("ISClient=@ISClient");
            strSql.Append(" where PUID=@PUID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "PUID", DbType.Int32, model.PUID);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
                    db.AddInParameter(dbCommand, "ISClient", DbType.Boolean, model.ISClient);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        /// <summary>
        /// Delete a record
        /// </summary>
        public bool Delete(int PUID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProjectUsers ");
            strSql.Append(" where PUID=@PUID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "PUID", DbType.Int32, PUID);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        public bool Delete(int projectId, int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProjectUsers ");
            strSql.AppendFormat(" where ProjectID={0} and UserID={1}", projectId, userId);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        /// <summary>
        /// Get an object entity
        /// </summary>
        public ProjectUsersEntity Get(int PUID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PUID,ProjectID,UserID,ISClient from ProjectUsers ");
            strSql.Append(" where PUID=@PUID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "PUID", DbType.Int32, PUID);
                ProjectUsersEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        if (dataReader.Read())
                        {
                            model = ProjectUsersEntity.ReaderBind(dataReader);
                        }
                    }
                    catch (Exception ex)
                    {
                        WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters)
                            , ex.Message));
                        return null;
                    }
                }
                return model;
            }
        }

        #endregion  Method

        #region IProjectUsersRepository Members

        public List<ProjectUsersEntity> GetProjectSunnetUserList(int projectID)
        {
            List<ProjectUsersEntity> list = new List<ProjectUsersEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select A.*,B.RoleID from ProjectUsers A inner join [Users] B on A.UserID=B.UserID ");
            strSql.Append(" where ProjectID=@ProjectID and IsClient=0 ");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, projectID);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        while (dataReader.Read())
                        {
                            list.Add(ProjectUsersEntity.ReaderBind(dataReader));
                        }
                    }
                    catch (Exception ex)
                    {
                        WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString()
                            , base.FormatParameters(dbCommand.Parameters), ex.Message));
                        return null;
                    }
                }
                return list;
            }
        }
        #endregion


        public List<int> GetUserIdByProjectId(int projectId)
        {
            List<int> list = new List<int>();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select userid from ProjectUsers where ProjectID={0}", projectId);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        while (dataReader.Read())
                        {
                            list.Add((int)dataReader["userid"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString()
                            , base.FormatParameters(dbCommand.Parameters), ex.Message));
                        return null;
                    }
                }
                return list;
            }
        }

        public List<int> GetActiveUserIdByProjectId(int projectId)
        {
            List<int> list = new List<int>();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select p.userid from projectusers p inner join users u on p.userid = u.userid and p.projectid = {0} and u.status='ACTIVE'", projectId);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        while (dataReader.Read())
                        {
                            list.Add((int)dataReader["userid"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString()
                            , base.FormatParameters(dbCommand.Parameters), ex.Message));
                        return null;
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// 获取与User同一个项目下的用户ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<int> GetProjectUserIds(int userId)
        {
            List<int> list = new List<int>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select userid from ProjectUsers ")
                .AppendFormat("  where ProjectID in (select ProjectID from ProjectUsers where UserID={0})", userId);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        while (dataReader.Read())
                        {
                            list.Add((int)dataReader["userid"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString()
                            , base.FormatParameters(dbCommand.Parameters), ex.Message));
                        return null;
                    }
                }
                return list;
            }
        }
    }
}


