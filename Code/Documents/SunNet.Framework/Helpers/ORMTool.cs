using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;

namespace SF.Framework.Helpers
{
    public class ORMTool
    {
        private static PropertyInfo FindProperty(object o, string toFindName)
        {
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                if (p.Name != null)
                {
                    if (p.Name.Trim().ToLower() == toFindName.Trim().ToLower())
                        return p;
                }
            }
            return null;
        }
        public static void FillEntity(object targetObj, IDataReader reader)
        {
            string name = string.Empty;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                name = reader.GetName(i);
                PropertyInfo propertyInfo = FindProperty(targetObj, name);
                if (propertyInfo != null)
                {
                    if (reader.GetValue(i) != DBNull.Value && propertyInfo.CanWrite)
                    {
                        if (propertyInfo.PropertyType.Equals(typeof(Boolean)))
                        {
                            propertyInfo.SetValue(targetObj, Convert.ToBoolean(reader.GetValue(i)), null);
                        }
                        else if (propertyInfo.PropertyType.Equals(typeof(System.Single)))
                        {
                            propertyInfo.SetValue(targetObj, Convert.ToSingle(reader.GetValue(i)), null);
                        }
                        else if (propertyInfo.Name.IndexOf("GL_") >= 0)
                        {
                            //XmlDocument collection=LanguageCollection.ConvertFromXMLString((string)reader.GetValue(i));
                            //propertyInfo.SetValue(targetObj, collection, null);
                        }
                        else if (propertyInfo.PropertyType.Equals(typeof(Nullable<bool>)))
                        {
                            Nullable<bool> tb = null;
                            if (reader.GetValue(i) != DBNull.Value)
                                tb = Convert.ToBoolean(reader.GetValue(i));
                            propertyInfo.SetValue(targetObj, tb, null);
                        }
                        else if (propertyInfo.PropertyType.Equals(typeof(Nullable<DateTime>)))
                        {
                            Nullable<DateTime> td = null;
                            if (reader.GetValue(i) != DBNull.Value)
                                td = Convert.ToDateTime(reader.GetValue(i));
                            propertyInfo.SetValue(targetObj, td, null);
                        }
                        else if (propertyInfo.PropertyType.Equals(typeof(Nullable<int>)))
                        {
                            Nullable<int> td = null;
                            if (reader.GetValue(i) != DBNull.Value)
                                td = Convert.ToInt32(reader.GetValue(i));
                            propertyInfo.SetValue(targetObj, td, null);
                        }
                        else if (propertyInfo.PropertyType.Equals(typeof(Nullable<decimal>)))
                        {
                            Nullable<decimal> td = null;
                            if (reader.GetValue(i) != DBNull.Value)
                                td = Convert.ToDecimal(reader.GetValue(i));
                            propertyInfo.SetValue(targetObj, td, null);
                        }
                        else if (propertyInfo.PropertyType.Equals(typeof(long)))
                        {
                            long td = 0;
                            if (reader.GetValue(i) != DBNull.Value)
                                td = Convert.ToInt64(reader.GetValue(i));
                            propertyInfo.SetValue(targetObj, td, null);
                        }
                        else
                        {
                            propertyInfo.SetValue(targetObj, reader.GetValue(i), null);
                        }
                    }
                }
            }
        }
    }
}
