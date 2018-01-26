using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Framework.Utils.Helpers
{
    public class SqlHelper
    {
        #region GetDBValue
        public  object GetDBValue(string v, string defaultValue)
        {
            object o = GetDBValue(v);
            if (o == null || o == DBNull.Value)
                return defaultValue;
            return o;
        }
        public  object GetDBValue(string v)
        {
            if (v == null)
                return DBNull.Value;
            return v;
        }
        public  object GetDBValue(Nullable<bool> v, bool defaultValue)
        {
            object o = GetDBValue(v);
            if (o == null || o == DBNull.Value)
                return defaultValue;
            return o;
        }
        public  object GetDBValue(Nullable<bool> v)
        {
            if (v == null)
                return DBNull.Value;
            if (!v.HasValue)
                return DBNull.Value;
            return v.Value;
        }
        public  object GetDBValue(Nullable<DateTime> v, DateTime defaultValue)
        {
            object o = GetDBValue(v);
            if (o == null || o == DBNull.Value)
                return defaultValue;
            return o;
        }
        public  object GetDBValue(Nullable<DateTime> v)
        {
            if (v == null)
                return DBNull.Value;
            if (!v.HasValue)
                return DBNull.Value;
            if (v.Value == DateTime.MinValue)
                return DBNull.Value;
            return v.Value;
        }
        public  object GetDBValue(Nullable<int> v, int defaultValue)
        {
            object o = GetDBValue(v);
            if (o == null || o == DBNull.Value)
                return defaultValue;
            return o;
        }
        public  object GetDBValue(Nullable<int> v)
        {
            if (v == null)
                return DBNull.Value;
            if (!v.HasValue)
                return DBNull.Value;
            return v.Value;
        }
        public  object GetDBValue(Nullable<decimal> v)
        {
            if (v == null)
                return DBNull.Value;
            if (!v.HasValue)
                return DBNull.Value;
            return v.Value;
        }
        #endregion

        public  string Convert2DBArrayString(List<int> ids)
        {
            string s = "";
            foreach(int id in ids)
            {
                s += id.ToString()+",";
            }
            return s;
        }


        public  string FilterSqlString(string sql)
        {
            sql = sql.Replace("[", "[[]");
            sql = sql.Replace("'", "''");
            sql = sql.Replace("%", "[%]");
            sql = sql.Replace("_", "[_]");

            return sql;
        }
    }
}
