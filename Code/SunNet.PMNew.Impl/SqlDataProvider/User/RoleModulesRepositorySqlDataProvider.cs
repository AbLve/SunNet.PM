using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Core.UserModule;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Providers;
namespace SunNet.PMNew.Impl.SqlDataProvider.User
{
    /// <summary>
    /// Data access:RoleModulesRepository
    /// </summary>
    public class RoleModulesRepositorySqlDataProvider : SqlHelper,IRoleModulesRepository
    {
        public RoleModulesRepositorySqlDataProvider()
        { }
        #region  Method
        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(RoleModulesEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into RoleModules(");
            strSql.Append("RoleID,ModuleID)");

            strSql.Append(" values (");
            strSql.Append("@RoleID,@ModuleID)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "RoleID", DbType.Int32, model.RoleID);
                    db.AddInParameter(dbCommand, "ModuleID", DbType.Int32, model.ModuleID);
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
        public bool Update(RoleModulesEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update RoleModules set ");
            strSql.Append("RoleID=@RoleID,");
            strSql.Append("ModuleID=@ModuleID");
            strSql.Append(" where RMID=@RMID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "RMID", DbType.Int32, model.RMID);
                    db.AddInParameter(dbCommand, "RoleID", DbType.Int32, model.RoleID);
                    db.AddInParameter(dbCommand, "ModuleID", DbType.Int32, model.ModuleID);
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
        public bool Delete(int RMID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from RoleModules ");
            strSql.Append(" where RMID=@RMID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "RMID", DbType.Int32, RMID);
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
        public RoleModulesEntity Get(int RMID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select RMID,RoleID,ModuleID from RoleModules ");
            strSql.Append(" where RMID=@RMID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "RMID", DbType.Int32, RMID);
                    RoleModulesEntity model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = RoleModulesEntity.ReaderBind(dataReader);
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
    }
}

