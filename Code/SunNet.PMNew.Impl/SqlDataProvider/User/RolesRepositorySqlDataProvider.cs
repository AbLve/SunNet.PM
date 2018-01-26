using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Core.UserModule;
using SunNet.PMNew.Entity.UserModel;

using SunNet.PMNew.Impl.SqlDataProvider;
using SunNet.PMNew.Framework.Utils.Providers;
namespace SunNet.PMNew.Impl.SqlDataProvider.User
{
    /// <summary>
    /// Data access:RolesRepository
    /// </summary>
    public  class RolesRepositorySqlDataProvider :SqlHelper, IRolesRepository
    {
        public RolesRepositorySqlDataProvider()
        { }
        #region  Method
        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(RolesEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Roles(");
            strSql.Append("RoleName,Description,Status,CreatedOn,CreatedBy)");

            strSql.Append(" values (");
            strSql.Append("@RoleName,@Description,@Status,@CreatedOn,@CreatedBy)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "RoleName", DbType.String, model.RoleName);
                    db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, model.Status);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return 0;
                }
            }
        }
        /// <summary>
        /// Update a record
        /// </summary>
        public bool Update(RolesEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Roles set ");
            strSql.Append("RoleName=@RoleName,");
            strSql.Append("Description=@Description,");
            strSql.Append("Status=@Status,");
            strSql.Append("CreatedOn=@CreatedOn,");
            strSql.Append("CreatedBy=@CreatedBy");
            strSql.Append(" where RoleID=@RoleID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "RoleID", DbType.Int32, model.RoleID);
                    db.AddInParameter(dbCommand, "RoleName", DbType.String, model.RoleName);
                    db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, model.Status);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        /// <summary>
        /// Delete a record
        /// </summary>
        public bool Delete(int RoleID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Roles ");
            strSql.Append(" where RoleID=@RoleID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "RoleID", DbType.Int32, RoleID);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        /// <summary>
        /// Get an object entity
        /// </summary>
        public RolesEntity Get(int RoleID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select RoleID,RoleName,Description,Status,CreatedOn,CreatedBy from Roles ");
            strSql.Append(" where RoleID=@RoleID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "RoleID", DbType.Int32, RoleID);
                    RolesEntity model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = RolesEntity.ReaderBind(dataReader);
                        }
                    }
                    return model;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }


        #endregion  Method

        #region IRolesRepository Members

        public List<RolesEntity> GetAllRoles()
        {
            string strSql = @"SELECT *
                              FROM [Roles]
                              WHERE [Status]=0 OR [Status]=1
                              ORDER BY  [RoleID] DESC";
            List<RolesEntity> list = new List<RolesEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    while (dataReader.Read())
                    {
                        list.Add(RolesEntity.ReaderBind(dataReader));
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

        #endregion
    }
}

