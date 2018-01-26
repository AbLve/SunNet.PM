using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Core.KPIModule;
using SunNet.PMNew.Entity.KPIModel;
using SunNet.PMNew.Impl.SqlDataProvider;
using SunNet.PMNew.Framework.Utils.Providers;
using System.Data;



namespace SunNet.PMNew.Impl.SqlDataProvider.KPI
{
    public class KPICaterogryRepositorySqlDataProvider : SqlHelper, IKPICategoryRepository
    {

        public KPICaterogryRepositorySqlDataProvider()
        { }

        #region  Method
        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(KPICategoriesEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO [KPICategory](");
            strSql.Append("[Name],[Status],[CreatedOn],[CreatedBy])");

            strSql.Append(" values (");
            strSql.Append("@Name,@Status,@CreatedOn,@CreatedBy)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Name", DbType.String, model.CategoryName);
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
        public bool Update(KPICategoriesEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [KPICategory] set ");
            strSql.Append("Name=@Name,");
            strSql.Append("Status=@Status");
            //strSql.Append("CreatedOn=@CreatedOn,");
           // strSql.Append("CreatedBy=@CreatedBy");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, model.ID);
                    db.AddInParameter(dbCommand, "Name", DbType.String, model.CategoryName);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, model.Status);
                    //db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                   // db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
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
        public bool Delete(int CategoryID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [KPICategory] ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, CategoryID);
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
        public KPICategoriesEntity Get(int CategoryID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [KPICategory] ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, CategoryID);
                    KPICategoriesEntity model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = KPICategoriesEntity.ReaderBind(dataReader);
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

        public List<KPICategoriesEntity> GetAllKPICategories()
        {
            string strSql = @"SELECT *
                              FROM [KPICategory]
                              ORDER BY ID DESC";
            List<KPICategoriesEntity> list = new List<KPICategoriesEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        while (dataReader.Read())
                        {
                            list.Add(KPICategoriesEntity.ReaderBind(dataReader));
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
