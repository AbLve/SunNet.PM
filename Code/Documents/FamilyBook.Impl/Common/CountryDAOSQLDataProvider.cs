using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyBook.Core.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using FamilyBook.Entity.Common;
using SF.Framework.Log;

namespace FamilyBook.Impl.Common
{
    public class CountryDAOSQLDataProvider : ICountryDAO
    {
        public int Insert(CountryEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Countries(");
            strSql.Append(" Country, Language )");
            strSql.Append(" values (");
            strSql.Append("@Country, @Language)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Country", DbType.String, entity.Country);
                    db.AddInParameter(dbCommand, "Language", DbType.Int32, entity.Language);
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
                    new log4netProvider().LogSQL(strSql.ToString(), dbCommand.Parameters, ex);
                    return 0;
                }
            }
        }

        public bool Update(CountryEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Countries ");
            strSql.Append(" set Country=@Country, Language=@Language ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Country", DbType.String, entity.Country);
                    db.AddInParameter(dbCommand, "Language", DbType.Int32, entity.Language);
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entity.ID);
                    return db.ExecuteNonQuery(dbCommand) > 0;
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(strSql.ToString(), dbCommand.Parameters, ex);
                    return false;
                }
            }
        }

        public bool Delete(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete Countries where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);
                    return db.ExecuteNonQuery(dbCommand) > 0;
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(strSql.ToString(), dbCommand.Parameters, ex);
                    return false;
                }
            }
        }

        public CountryEntity Get(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select Country, Language ");
            strSql.Append(" from Countries ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    CountryEntity entity = null;
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);

                    IDataReader reader = db.ExecuteReader(dbCommand);
                    if (reader.Read())
                    {
                        entity = new CountryEntity(reader);
                        reader.Close();
                        return entity;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(strSql.ToString(), dbCommand.Parameters, ex);
                    return null;
                }
            }
        }

        public CountryEntity GetCountryByCountry(string country)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select ID, Country, Language ");
            strSql.Append(" from Countries ");
            strSql.Append(" where Country=@Country ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    CountryEntity entity = null;
                    db.AddInParameter(dbCommand, "Country", DbType.String, country);

                    IDataReader reader = db.ExecuteReader(dbCommand);
                    if (reader.Read())
                    {
                        entity = new CountryEntity(reader);
                        reader.Close();
                        return entity;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(strSql.ToString(), dbCommand.Parameters, ex);
                    return null;
                }
            }
        }

        public List<CountryEntity> GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select ID, Country, Language ");
            strSql.Append(" from Countries order by Country asc ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    List<CountryEntity> entityList = new List<CountryEntity>(); 
                    IDataReader reader = db.ExecuteReader(dbCommand);
                    while (reader.Read())
                    {
                        entityList.Add(new CountryEntity(reader));

                    }
                    reader.Close();
                    return entityList;
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(strSql.ToString(), dbCommand.Parameters, ex);
                    return null;
                }
            }
        } 
    }
}
