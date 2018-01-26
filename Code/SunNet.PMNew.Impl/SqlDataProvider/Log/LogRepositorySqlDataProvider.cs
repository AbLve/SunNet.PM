using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.Log;
using SunNet.PMNew.Entity.LogModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using SunNet.PMNew.Framework.Utils.Providers;
using System.Data;

namespace SunNet.PMNew.Impl.SqlDataProvider.Log
{
    public class LogRepositorySqlDataProvider : SqlHelper, ILogRepository
    {

        #region IRepository<LogEntity> Members

        public int Insert(LogEntity logEntity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO [Logs]");
            strSql.Append("([CurrentUserId],[LogType],[OperatingTime],[IPAddress],[Referrer],[Description],[IsSuccess])");
            strSql.Append(" VALUES ");
            strSql.Append("(@CurrentUserId,@LogType,@OperatingTime,@IPAddress,@Referrer,@Description,@IsSuccess)");
            strSql.Append(";SELECT ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "CurrentUserId", DbType.Int64, logEntity.currentUserId);
                    db.AddInParameter(dbCommand, "LogType", DbType.Int32, (int)logEntity.logType);
                    db.AddInParameter(dbCommand, "OperatingTime", DbType.DateTime, logEntity.operatingTime);
                    db.AddInParameter(dbCommand, "IPAddress", DbType.String, logEntity.iPAddress);
                    db.AddInParameter(dbCommand, "Referrer", DbType.String, logEntity.referrer);
                    db.AddInParameter(dbCommand, "Description", DbType.String, logEntity.Description);
                    db.AddInParameter(dbCommand, "IsSuccess", DbType.Boolean, logEntity.IsSuccess);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString()
                        , base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return 0;
                }
            }
        }

        public bool Update(SunNet.PMNew.Entity.LogModel.LogEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public SunNet.PMNew.Entity.LogModel.LogEntity Get(int entityId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ILogRepository Members

        #endregion
    }
}
