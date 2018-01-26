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
    /// Data access:ModulesRepository
    /// </summary>
    public class ModulesRepositorySqlDataProvider : SqlHelper, IModulesRepository
    {
        public ModulesRepositorySqlDataProvider()
        { }
        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(ModulesEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Modules(");
            strSql.Append("ModuleTitle,ModulePath,DefaultPage,Status,Orders,ShowInMenu,PageOrModule,ClickFunctioin,ParentID)");

            strSql.Append(" values (");
            strSql.Append("@ModuleTitle,@ModulePath,@DefaultPage,@Status,@Orders,@ShowInMenu,@PageOrModule,@ClickFunctioin,@ParentID)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ModuleTitle", DbType.String, model.ModuleTitle);
                    db.AddInParameter(dbCommand, "ModulePath", DbType.String, model.ModulePath);
                    db.AddInParameter(dbCommand, "DefaultPage", DbType.String, model.DefaultPage);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, model.Status);
                    db.AddInParameter(dbCommand, "Orders", DbType.Int32, model.Orders);
                    db.AddInParameter(dbCommand, "ShowInMenu", DbType.Boolean, model.ShowInMenu);
                    db.AddInParameter(dbCommand, "PageOrModule", DbType.Int32, model.PageOrModule);
                    db.AddInParameter(dbCommand, "ClickFunctioin", DbType.String, model.ClickFunctioin);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return 0;
                }
            }
        }
        /// <summary>
        /// Update a record
        /// </summary>
        public bool Update(ModulesEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Modules set ");
            strSql.Append("ModuleTitle=@ModuleTitle,");
            strSql.Append("ModulePath=@ModulePath,");
            strSql.Append("DefaultPage=@DefaultPage,");
            strSql.Append("Status=@Status,");
            strSql.Append("Orders=@Orders,");
            strSql.Append("ShowInMenu=@ShowInMenu,");
            strSql.Append("PageOrModule=@PageOrModule,");
            strSql.Append("ClickFunctioin=@ClickFunctioin,");
            strSql.Append("ParentID=@ParentID");
            strSql.Append(" where ModuleID=@ModuleID and ModuleID=@ModuleID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ModuleID", DbType.Int32, model.ModuleID);
                    db.AddInParameter(dbCommand, "ModuleTitle", DbType.String, model.ModuleTitle);
                    db.AddInParameter(dbCommand, "ModulePath", DbType.String, model.ModulePath);
                    db.AddInParameter(dbCommand, "DefaultPage", DbType.String, model.DefaultPage);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, model.Status);
                    db.AddInParameter(dbCommand, "Orders", DbType.Int32, model.Orders);
                    db.AddInParameter(dbCommand, "ShowInMenu", DbType.Boolean, model.ShowInMenu);
                    db.AddInParameter(dbCommand, "PageOrModule", DbType.Int32, model.PageOrModule);
                    db.AddInParameter(dbCommand, "ClickFunctioin", DbType.String, model.ClickFunctioin);
                    db.AddInParameter(dbCommand, "ParentID", DbType.Int32, model.ParentID);
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
        public bool Delete(int ModuleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Modules ");
            strSql.Append(" where ModuleID=@ModuleID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ModuleID", DbType.Int32, ModuleID);
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
        public ModulesEntity Get(int ModuleID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Modules ");
            strSql.Append(" where ModuleID=@ModuleID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ModuleID", DbType.Int32, ModuleID);
                    ModulesEntity model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = ModulesEntity.ReaderBind(dataReader);
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

        #region IModulesRepository Members

        public List<ModulesEntity> GetModulesList(int roleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("IF(@RoleID=0)");
            strSql.Append("     BEGIN");
            strSql.Append("		SELECT m.*");
            strSql.Append("		  FROM [Modules] m ");
            strSql.Append("		  WHERE [Status] =0 OR [Status] =1");
            strSql.Append("         ORDER BY Orders DESC ,ModuleID DESC");
            strSql.Append("	    END    ");
            strSql.Append("ELSE");
            strSql.Append("     BEGIN");
            strSql.Append("		SELECT m.*");
            strSql.Append("		  FROM [Modules] m,RoleModules rm");
            strSql.Append("		  WHERE rm.RoleID=@RoleID AND m.ModuleID=rm.ModuleID");
            strSql.Append("         ORDER BY Orders DESC ,ModuleTitle ASC");
            strSql.Append("	    END");
            List<ModulesEntity> list = new List<ModulesEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "RoleID", DbType.Int32, roleID);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            list.Add(ModulesEntity.ReaderBind(dataReader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(),
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                }
            }
            return list;
        }

        public List<ModulesEntity> GetAllModules(int parentID, int page, int pageCount)
        {
            int start = page * pageCount + 1 - pageCount;
            int end = page * pageCount;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM (");
            strSql.Append("SELECT ROW_NUMBER() OVER(Order BY ParentID ASC, Orders DESC,ModuleTitle ASC) AS INDEX_ID,* ");
            strSql.Append("FROM [Modules] ");
            strSql.Append(@"WHERE [ParentID]=@ParentID AND ([Status] =0 OR [Status] =1 )
                                ) NEW_TB WHERE INDEX_ID BETWEEN @Strat AND @End;   
                                ");
            List<ModulesEntity> list = new List<ModulesEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ParentID", DbType.Int32, parentID);
                    db.AddInParameter(dbCommand, "Strat", DbType.Int32, start);
                    db.AddInParameter(dbCommand, "End", DbType.Int32, end);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            list.Add(ModulesEntity.ReaderBind(dataReader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(),
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                }
            }
            return list;
        }

        public int GetAllModulesCount(int parentID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT COUNT(1) FROM [Modules] ");
            strSql.Append(@"WHERE [ParentID]=@ParentID AND ([Status] =0 OR [Status] =1)");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ParentID", DbType.Int32, parentID);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        int result;
                        object obj = db.ExecuteScalar(dbCommand);
                        if (!int.TryParse(obj.ToString(), out result))
                        {
                            return 0;
                        }
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(),
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                }
            }
            return 0;
        }

        public bool RemoveAll(int roleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [RoleModules] ");
            strSql.Append(" where [RoleID]=@RoleID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "RoleID", DbType.Int32, roleID);
                    int rows = db.ExecuteNonQuery(dbCommand);
                    return true;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        #endregion
    }
}

