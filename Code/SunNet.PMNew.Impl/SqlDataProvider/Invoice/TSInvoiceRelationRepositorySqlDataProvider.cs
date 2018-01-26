using Microsoft.Practices.EnterpriseLibrary.Data;
using SunNet.PMNew.Core.InvoiceModule.Interface;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Framework.Utils.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Impl.SqlDataProvider.Invoice
{
    public class TSInvoiceRelationRepositorySqlDataProvider : SqlHelper, ITSInvoiceRelationRpository
    {
        public TSInvoiceRelationRepositorySqlDataProvider()
        { }
        #region 
        public int Insert(TSInvoiceRelationEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO [TSInvoiceRelation](");
            strSql.Append("[InvoiceId],[TSId])");

            strSql.Append(" values (");
            strSql.Append("@InvoiceId,@TSId)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "InvoiceId", DbType.Int32, entity.InvoiceId);
                    db.AddInParameter(dbCommand, "TSId", DbType.Int32, entity.TSId);
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

        public bool Update(TSInvoiceRelationEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  UPDATE [Invoices] SET [InvoiceId]=@InvoiceId");
            strSql.Append(" ,[TSId]=@TSId");
            strSql.Append(" WHERE ID=@ID");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "InvoiceId", DbType.DateTime, entity.InvoiceId);
                    db.AddInParameter(dbCommand, "TSId", DbType.String, entity.TSId);
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entity.ID);
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

        public bool Delete(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  DELETE [TSInvoiceRelation] WHERE ID=@entityId");
             Database db = DatabaseFactory.CreateDatabase();
             using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
             {
                 try
                 {
                     db.AddInParameter(dbCommand, "entityId", DbType.Int32, entityId);
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

        public TSInvoiceRelationEntity Get(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * from [TSInvoiceRelation]");
            strSql.Append(" where ID=@entityId ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "entityId", DbType.Int32, entityId);
                TSInvoiceRelationEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        if (dataReader.Read())
                        {
                            model = TSInvoiceRelationEntity.ReaderBind(dataReader);
                        }
                    }
                    catch (Exception ex)
                    {
                        WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                            , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                        return null;
                    }
                }
                return model;
            }
        }

        public List<TSInvoiceRelationEntity> GetAllTSInvoiceRelation()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * from [TSInvoiceRelation]");
            Database db = DatabaseFactory.CreateDatabase();
            List<TSInvoiceRelationEntity> list = new List<TSInvoiceRelationEntity>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                TSInvoiceRelationEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        if (dataReader.Read())
                        {
                            model = TSInvoiceRelationEntity.ReaderBind(dataReader);
                            list.Add(model);
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
        }
        #endregion
    }
}
