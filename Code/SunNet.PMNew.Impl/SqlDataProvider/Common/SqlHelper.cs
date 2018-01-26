using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Framework.Utils.Providers;
using System.Data;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Impl.SqlDataProvider
{
    public class SqlHelper
    {
        /// <summary>
        /// 数据库支持的最小时间
        /// </summary>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/26 03:19
        protected static DateTime MinDate { get { return new DateTime(1753, 1, 1); } }
        public string FormatParameters(DbParameterCollection parameters)
        {
            StringBuilder sbparas = new StringBuilder("\r\nParameters-------------------------\r\n");
            foreach (DbParameter para in parameters)
            {
                sbparas.AppendFormat("ParameterName:{0},Value:{1},Type:{2};\r\n", para.ParameterName, (null == para.Value)
                    ? "Null" : para.Value.ToString(), para.Direction.ToString());
            }
            sbparas.Append("----------------------------------\r\n");
            return sbparas.ToString();
        }
        protected bool ExistsRecords(string tableName, string attrName, string attrValue, string primaryKey, string exceptValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("Select Count(1) from {0} ", tableName);
            strSql.AppendFormat(" where {0}<>@ExceptValue ", primaryKey);
            strSql.AppendFormat(" AND {0} = @CheckValue", attrName);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ExceptValue", DbType.String, exceptValue.FilterSqlString());
                    db.AddInParameter(dbCommand, "CheckValue", DbType.String, attrValue.FilterSqlString());
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        if (dataReader.Read())
                        {
                            int rows = dataReader.GetInt32(0);
                            if (rows > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    return false;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        /// <summary>
        /// 传入 一些 keywords 以空格区分，得出一个 like字符串， 
        /// 例如 "key1 key2 key3" 传出: field1 like %key1% or field2 like %key2%   
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public string SplitKeywords(string keywordStr,params string[] fields)
        {
            string returnStr = " AND ( 1= 0 ";
            if (string.IsNullOrEmpty(keywordStr) || fields==null || fields.Length == 0)
            {
                return "";
            }
            else
            {
                string[] keywords = keywordStr.Trim().Split(',');
                foreach (string key in keywords)
                {
                    if (key.Trim() != "")
                    {
                        foreach (string field in fields)
                        {
                            string queryStr = "";
                            queryStr = " or " + field + " like '%" + key.Replace(" ","%").FilterSqlString().Replace("'", "''") + "%'";
                            returnStr += queryStr;
                        }
                    }
                }
            }
            return returnStr + ")";
        }

        /// <summary>
        /// 
        /// 传入 一些 keywords 以空格区分，得出一个 like字符串，支持全匹配查询。 
        /// 其中 单词间如果是 空格连接，则是 与查询，如果是逗号隔开，则是 或查询。
        /// </summary>
        /// <param name="keyword">关键字字符串</param>
        /// <param name="equalField"> 全查询字段</param>
        /// <param name="likeFields">模糊搜索字段</param>
        /// <returns>组合查询字符串</returns>
        public string SplitKeywords(string keywordStr, string equalField, string[] likeFields)
        {
            string returnStr = " AND ( 1= 0 ";
            if (string.IsNullOrEmpty(keywordStr) || likeFields == null || likeFields.Length == 0)
            {
                return "";
            }
            else
            {
                string[] keywords = keywordStr.Trim().Split(',');
                foreach (string key in keywords)
                {
                    if (key.Trim() != "")
                    {
                        // 如果关键字是 ID 则进行转换

                        if (!string.IsNullOrEmpty(equalField))
                        {
                            returnStr += "  or  CONVERT(NVARCHAR(500)," + equalField.Trim() + ") = '" + key.Trim() + "'";
                        } 
                        foreach (string field in likeFields)
                        {
                            string queryStr = "";
                            queryStr = " or " + field.Trim() + " like '%" + key.Trim().Replace(" ", "%").FilterSqlString().Replace("'", "''") + "%'";
                            returnStr += queryStr;
                        }
                    }
                }
            }
            return returnStr + ")";
        }
    }
}
