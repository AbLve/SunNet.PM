using Microsoft.Practices.EnterpriseLibrary.Data;
using SunNet.PMNew.Core.UserModule;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Impl.SqlDataProvider.User
{
    public class HideUsersRepositorySqlDataProvider : SqlHelper, IHideUserRepository
    {
        public HideUsersRepositorySqlDataProvider()
        { }

        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(HideUserEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT  INTO dbo.HideUsers ");
            strSql.Append("( UserID, HideUserIds )");

            strSql.Append("VALUES (@UserID,@HideUserIds); ");
            strSql.Append(";SELECT ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
                    db.AddInParameter(dbCommand, "HideUserIds", DbType.String, model.HideUserIds);
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
                    return 0;
                }
            }
        }
        /// <summary>
        /// Update a record
        /// </summary>
        public bool Update(HideUserEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE dbo.HideUsers SET ");
            strSql.Append("UserID=@UserID, ");
            strSql.Append("HideUserIds=@HideUserIds ");
            strSql.Append("WHERE ID=@ID; ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
                    db.AddInParameter(dbCommand, "HideUserIds", DbType.String, model.HideUserIds);
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, model.ID);
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

        /// <summary>
        /// Delete a record
        /// </summary>
        public bool Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE dbo.HideUsers ");
            strSql.Append("WHERE ID = @ID; ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, id);
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

        /// <summary>
        /// Get an object entity
        /// </summary>
        public HideUserEntity Get(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM dbo.HideUsers AS HU ");
            strSql.Append("WHERE HU.ID = @ID; ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, id);
                    HideUserEntity model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = HideUserEntity.ReaderBind(dataReader);
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

        #endregion  Method
        public HideUserEntity GetHideUserByUserId(int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT TOP 1 * FROM dbo.HideUsers AS HU ");
            strSql.Append("WHERE HU.UserID = @UserID; ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
                    HideUserEntity model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = HideUserEntity.ReaderBind(dataReader);
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

        public int IsExistDataByUserId(int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(HU.ID) FROM dbo.HideUsers AS HU ");
            strSql.Append("WHERE HU.UserID = @UserID; ");
            Database db = DatabaseFactory.CreateDatabase();
            int count = -1;
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
                    object obj = db.ExecuteScalar(dbCommand);
                    count = int.Parse(obj.ToString());
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return -1;
                }
            }
            return count;
        }
    }
}
