using Microsoft.Practices.EnterpriseLibrary.Data;
using SunNet.PMNew.Core.ComplaintModel.Interfaces;
using SunNet.PMNew.Entity.ComplaintModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Web.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Web.Script.Serialization;


namespace SunNet.PMNew.Impl.SqlDataProvider.Complaint
{
    public class ComplaintRepositorySqlDataProvider : SqlHelper, IComplaintRepository
    {
        public int Insert(ComplaintEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO Complaints(");
            strSql.Append("Type,TargetID,Reason,AdditionalInfo,SystemID,AppSrc,ReporterID,ReporterEmail,CreatedOn,Status) ");
            strSql.Append("VALUES(");
            strSql.Append("@Type,@TargetID,@Reason,@AdditionalInfo,@SystemID,@AppSrc,@ReporterID,@ReporterEmail,@CreatedOn,@Status)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Type", DbType.Int32, entity.Type);
                    db.AddInParameter(dbCommand, "TargetID", DbType.Int32, entity.TargetID);
                    db.AddInParameter(dbCommand, "Reason", DbType.Int32, entity.Reason);
                    db.AddInParameter(dbCommand, "AdditionalInfo", DbType.String, entity.AdditionalInfo);
                    db.AddInParameter(dbCommand, "SystemID", DbType.Int32, entity.SystemID);
                    db.AddInParameter(dbCommand, "AppSrc", DbType.Int32, entity.AppSrc);
                    db.AddInParameter(dbCommand, "ReporterID", DbType.Int32, entity.ReporterID);
                    db.AddInParameter(dbCommand, "ReporterEmail", DbType.String, entity.ReporterEmail);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, entity.CreatedOn);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, entity.Status);

                    int result;
                    object obj = db.ExecuteScalar(dbCommand);
                    if (!int.TryParse(obj.ToString (), out result))
                    {
                        return 0;
                    }
                    return result;
                }

                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(),
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                    return 0;
                }
            }
        }

        public bool Update(ComplaintEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Complaints SET ");
            strSql.Append("Status=@Status, ");
            strSql.Append("Comments=@Comments ");
            strSql.Append("WHERE ComplaintID=@ComplaintID");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, entity.Status);
                    db.AddInParameter(dbCommand, "Comments", DbType.String, entity.Comments);
                    db.AddInParameter(dbCommand, "ComplaintID", DbType.Int32, entity.ComplaintID);

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

        public bool Delete(int entityId)
        {
            return true;
        }

        public ComplaintEntity Get(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT c.*, s.SystemName, c.Comments AS UpdatedByName from Complaints c, Systems s ");
            strSql.Append("WHERE c.ComplaintID=@ComplaintID AND c.SystemID=s.SystemID ");

            ComplaintEntity comEntity = null;
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ComplaintID", DbType.Int32, entityId);

                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            comEntity = ComplaintEntity.ReaderBind(dataReader);
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
            return comEntity;
        }

        public string GetComItem(string connStr, string spName, string type, int id)
        {
            Regex regex = new Regex("(?<=database=).*(?=;uid)");
            string dbName = regex.Match(connStr).Value;
            
            //Add a connectionString if not existed
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConnectionStringsSection csSection = config.ConnectionStrings;

            if (csSection.ConnectionStrings[dbName] == null)
            {
                ConnectionStringSettings connection = new ConnectionStringSettings(dbName, connStr, "System.Data.SqlClient");
                csSection.ConnectionStrings.Add(connection);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }

            ComplaintItem com = new ComplaintItem();
            Database db = new SqlDatabase(connStr);
            using (DbCommand dbCommand = db.GetStoredProcCommand(spName))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Action", DbType.String, "View");
                    db.AddInParameter(dbCommand, "Type", DbType.String, type);
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, id);
                    IDataReader reader = db.ExecuteReader(dbCommand);
                    if (reader.Read())
                    {
                        switch(type) {
                            case "Photo":
                            case "Video":
                                com.Path = (string)reader["Path"];
                                break;
                            case "User":
                                com.UserName = (string)reader["UserName"];
                                com.UserEmail = (string)reader["UserEmail"];
                                break;
                            case "Group":
                                com.UserName = (string)reader["UserName"];
                                com.GroupName = (string)reader["GroupName"];
                                break;
                            case "Post":
                                com.UserName = (string)reader["UserName"];
                                com.Message = (string)reader["Message"];
                                break;
                        }
                    }

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    return jss.Serialize(com);
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                          "GetTimesheetList", base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return "";
                }
            }
        }

        //Return true if deletion succeeded
        public bool DeleteComItem(string connStr, string spName, string type, int id)
        {
            Regex r = new Regex("(?<=database=).*(?=;uid)");
            string dbName = r.Match(connStr).Value;

            //Add a connectionString if not existed
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConnectionStringsSection csSection = config.ConnectionStrings;

            if (csSection.ConnectionStrings[dbName] == null)
            {
                ConnectionStringSettings connection = new ConnectionStringSettings(dbName, connStr, "System.Data.SqlClient");
                csSection.ConnectionStrings.Add(connection);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }

            Database db = new SqlDatabase(connStr);
            using (DbCommand dbCommand = db.GetStoredProcCommand(spName))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Action", DbType.String, "Delete");
                    db.AddInParameter(dbCommand, "Type", DbType.String, type);
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, id);
                    db.ExecuteReader(dbCommand);
                    return true;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                          "GetTimesheetList", base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }


        public List<ComplaintEntity> SearchComplaints(ComplaintSearchEntity request, out int recordCount)
        {
            recordCount = 0;

            int startID = request.CurrentPage * request.PageCount + 1 - request.PageCount;
            int endID = request.CurrentPage * request.PageCount;

            StringBuilder strSql = new StringBuilder();
            StringBuilder strSqlBody = new StringBuilder();
            StringBuilder strSelAll = new StringBuilder();
            StringBuilder strSelCnt = new StringBuilder();
            StringBuilder strOrderBy = new StringBuilder();
            StringBuilder strFitPage = new StringBuilder();


            strSelCnt.Append("SELECT Count(*) FROM ");
            strSelAll.Append(String.Format("SELECT * from(SELECT *, Row_Number() Over(Order By {0} {1}) RowNum FROM ", request.OrderExpression, request.OrderDirection));
            

            strSqlBody.Append("(SELECT c.*, s.SystemName, '' AS UpdatedByName ");
            strSqlBody.Append("FROM Complaints c, Systems s ");
            strSqlBody.Append("WHERE c.SystemID=s.SystemID AND c.UpdatedByID is NULL ");
            strSqlBody.Append("UNION ");
            strSqlBody.Append("SELECT c.*, s.SystemName, u.FirstName+' '+u.LastName AS UpdatedByName ");
            strSqlBody.Append("FROM Complaints c, Systems s, Users u ");
            strSqlBody.Append("WHERE c.SystemID=s.SystemID AND c.UpdatedByID=u.UserID) combine ");
            strSqlBody.Append("WHERE combine.ComplaintID>0 ");

            strFitPage.Append(") bigCombine where RowNum BETWEEN @StartID AND @EndID ");

            if (request.Type >= 0) strSqlBody.Append("AND combine.Type=@Type ");
            if (request.Reason >= 0) strSqlBody.Append("AND combine.Reason=@Reason ");
            if (request.SystemID >= 0) strSqlBody.Append("AND combine.SystemID=@SystemID ");
            if (request.AppSrc >= 0) strSqlBody.Append("AND combine.AppSrc=@AppSrc ");
            if (request.Status >= 0) strSqlBody.Append("AND combine.Status=@Status ");

            if (request.Keyword.Length > 0) strSqlBody.Append("AND (combine.AdditionalInfo like @Keywords OR combine.SystemName like @Keywords) ");
            if (request.UpdatedByName.Length > 0) strSqlBody.Append("AND combine.UpdatedByName like @UpdatedByName ");

            strSql.Append(strSelCnt);
            strSql.Append(strSqlBody);
            strSql.Append(";");
            strSql.Append(strSelAll);
            strSql.Append(strSqlBody);
            strSql.Append(strFitPage);

            // Execute
            List<ComplaintEntity> list = new List<ComplaintEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Type", DbType.Int32, request.Type);
                    db.AddInParameter(dbCommand, "Reason", DbType.Int32, request.Reason);
                    db.AddInParameter(dbCommand, "SystemID", DbType.Int32, request.SystemID);
                    db.AddInParameter(dbCommand, "AppSrc", DbType.Int32, request.AppSrc);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, request.Status);
                    db.AddInParameter(dbCommand, "Keywords", DbType.String, request.Keyword);
                    db.AddInParameter(dbCommand, "UpdatedByName", DbType.String, request.UpdatedByName);
                    db.AddInParameter(dbCommand, "StartID", DbType.Int32, startID);
                    db.AddInParameter(dbCommand, "EndID", DbType.Int32, endID);

                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            recordCount = dataReader.GetInt32(0);
                            dataReader.NextResult();
                        }

                        while (dataReader.Read())
                        {
                            list.Add(ComplaintEntity.ReaderBind(dataReader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSqlBody.ToString(),
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                }
                return list;
            }
        }
    }
}
