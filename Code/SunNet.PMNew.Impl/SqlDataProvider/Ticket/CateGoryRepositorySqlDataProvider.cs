using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Utils.Providers;
namespace SunNet.PMNew.Impl.SqlDataProvider.Ticket
{
    /// <summary>
    /// Data access:CateGory
    /// </summary>
    public class CateGoryRepositorySqlDataProvider : SqlHelper, ICateGoryRepository
    {
        public CateGoryRepositorySqlDataProvider()
        { }
        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(CateGoryEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into CateGory(");
            strSql.Append("Title,CreatedOn,CreatedBy,IsOnlyShowTody,IsDelete)");

            strSql.Append(" values (");
            strSql.Append("@Title,@CreatedOn,@CreatedBy,@IsOnlyShowTody,@IsDelete)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                    db.AddInParameter(dbCommand, "IsOnlyShowTody", DbType.Boolean, model.IsProtected);
                    db.AddInParameter(dbCommand, "IsDelete", DbType.Boolean, model.IsDelete);
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
        public bool Update(CateGoryEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CateGory set ");
            strSql.Append("Title=@Title,");
            strSql.Append("CreatedOn=@CreatedOn,");
            strSql.Append("CreatedBy=@CreatedBy,");
            strSql.Append("IsOnlyShowTody=@IsOnlyShowTody,");
            strSql.Append("IsDelete=@IsDelete");
            strSql.Append(" where GID=@GID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "GID", DbType.Int32, model.GID);
                    db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                    db.AddInParameter(dbCommand, "IsOnlyShowTody", DbType.Boolean, model.IsProtected);
                    db.AddInParameter(dbCommand, "IsDelete", DbType.Boolean, model.IsDelete);
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
        public bool Delete(int GID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CateGory set ");
            strSql.Append("IsDelete=@IsDelete ");
            strSql.Append(" where GID=@GID and IsOnlyShowTody = 0");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "IsDelete", DbType.Int32, 1);
                    db.AddInParameter(dbCommand, "GID", DbType.Int32, GID);
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
        public CateGoryEntity Get(int GID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from CateGory ");
            strSql.Append(" where GID=@GID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "GID", DbType.Int32, GID);
                CateGoryEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        if (dataReader.Read())
                        {
                            model = CateGoryEntity.ReaderBind(dataReader);
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
        }

        #endregion  Method

        #region ICateGoryRepository Members

        public List<CateGoryEntity> GetCateGoryListByUserID(int UserID)
        {
            string strSql = @"SELECT  [GID]
                                  ,[Title]
                                  ,[CreatedOn]
                                  ,[CreatedBy]
                                  ,[IsOnlyShowTody]
                                  ,[IsDelete]
                              FROM  [CateGory]
                              WHERE [CreatedBy]=@UserID AND IsDelete = 0
                              Order by GID asc ";
            List<CateGoryEntity> list = new List<CateGoryEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        while (dataReader.Read())
                        {
                            list.Add(CateGoryEntity.ReaderBind(dataReader));
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

        public int CountCateGory(string title, int userID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select Count(1) from CateGory ");
            strSql.Append(" where Title=@Title AND CreatedBy=@CreatedBy AND IsDelete=0");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Title", DbType.String, title);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, userID);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        int count = 0;
                        if (dataReader.Read())
                        {
                            count = dataReader.GetInt32(0);
                        }
                        return count;
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return 0;
                }
            }
        }

        #endregion
    }
}

