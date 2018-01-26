using Microsoft.Practices.EnterpriseLibrary.Data;
using SunNet.PMNew.Core.ComplaintModel.Interfaces;
using SunNet.PMNew.Entity.ComplaintModel;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Framework.Utils.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Impl.SqlDataProvider.Complaint
{
    public class SystemRepositorySqlDataProvider : SqlHelper, ISystemRepository
    {
        public SystemRepositorySqlDataProvider() {}

        public int Insert(SystemEntity entity)
        {
            return 0;
        }

        public bool Update(SystemEntity entity)
        {
            return true;
        }

        public bool Delete(int entityId)
        {
            return true;
        }

        public SystemEntity Get(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT s.* from Systems s ");
            strSql.Append("WHERE s.SystemID=@SystemID");

            SystemEntity systemEntity = null;
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "SystemID", DbType.Int32, entityId);

                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            systemEntity = SystemEntity.ReaderBind(dataReader); 
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
            return systemEntity;

        }

        public SystemEntity GetBySysName(string sysName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT s.* from Systems s ");
            strSql.Append("WHERE s.SystemName=@SystemName");

            SystemEntity systemEntity = null;
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "SystemName", DbType.String, sysName);

                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            systemEntity = SystemEntity.ReaderBind(dataReader);
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
            return systemEntity;

        }


        public SystemEntity SearchSystem(int entityID)
        {
            return new SystemEntity();
        }

        public List<SystemEntity> GetAllSystems()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * from Systems");

            List<SystemEntity> list = new List<SystemEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            list.Add(SystemEntity.ReaderBind(dataReader));
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

    }
}
