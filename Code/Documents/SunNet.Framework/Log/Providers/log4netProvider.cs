using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Web;
using System.Data.Common;
namespace SF.Framework.Log
{
    public class log4netProvider : ILog
    {
        #region ILog Members

        public LogConfig Config
        {
            get;
            set;
        }

        public void Log(Exception ex)
        {
            Log("", ex);
        }

        public void Log(string message)
        {
            log4net.ILog log = LogManager.GetLogger("info.Logging");
            log.Debug(message);
        }

        public void Log(string message, Exception exception)
        {
            log4net.ILog log = LogManager.GetLogger("info.Logging");
            log.Debug(message, exception);
        }
        #endregion

        #region  //Extend method
        public void Log(Exception[] exceptions)
        {
            for (int i = 0; i < exceptions.Length; i++)
            {
                Log(exceptions[i]);
            }
        }

        public void LogSQL(string procedureNameOrSQL, DbParameterCollection cmdParameters, Exception exception)
        {
            string message = string.Format("[SQLText:{0},{1}]", procedureNameOrSQL, FormatCMDParameters(cmdParameters));
            Log(message, exception);
        }

        public void LogSQL(string procedureNameOrSQL, Exception exception)
        {
            string message = string.Format("[SQLText:{0}]", procedureNameOrSQL);
            Log(message, exception);
        }
        #endregion

        private string FormatCMDParameters(DbParameterCollection dbParameterCollection)
        {
            StringBuilder stringBuilder = new StringBuilder("\r\nParameters-------------------------\r\n");
            foreach (DbParameter dbParameter in dbParameterCollection)
            {
                stringBuilder.AppendFormat("ParameterName:{0},Value:{1},Type:{2};\r\n", dbParameter.ParameterName
                    , (null == dbParameter.Value)? "Null" : dbParameter.Value.ToString(), dbParameter.Direction.ToString());
            }
            stringBuilder.Append("----------------------------------\r\n");
            return stringBuilder.ToString();
        }
    }
}
