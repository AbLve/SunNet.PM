using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Core.CompanyModule;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Impl.SqlDataProvider.Company
{
    /// <summary>
    /// Data access:Companys
    /// </summary>
    public class CompanysRepositorySqlDataProvider : SqlHelper, ICompanyRepository
    {
        public CompanysRepositorySqlDataProvider()
        { }
        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(CompanysEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Companys(");
            strSql.Append("CompanyName,Phone,Fax,Website,AssignedSystemUrl,Address1,Address2,City,State,Logo,Status,CreatedOn,CreatedBy,CreateUserName,ModifiedOn,ModifiedBy)");

            strSql.Append(" values (");
            strSql.Append("@CompanyName,@Phone,@Fax,@Website,@AssignedSystemUrl,@Address1,@Address2,@City,@State,@Logo,@Status,@CreatedOn,@CreatedBy,@CreateUserName,@ModifiedOn,@ModifiedBy)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "CompanyName", DbType.String, model.CompanyName);
                    db.AddInParameter(dbCommand, "Phone", DbType.String, model.Phone);
                    db.AddInParameter(dbCommand, "Fax", DbType.String, model.Fax);
                    db.AddInParameter(dbCommand, "Website", DbType.String, model.Website);
                    db.AddInParameter(dbCommand, "AssignedSystemUrl", DbType.String, model.AssignedSystemUrl);
                    db.AddInParameter(dbCommand, "Address1", DbType.String, model.Address1);
                    db.AddInParameter(dbCommand, "Address2", DbType.String, model.Address2);
                    db.AddInParameter(dbCommand, "City", DbType.String, model.City);
                    db.AddInParameter(dbCommand, "State", DbType.String, model.State);
                    db.AddInParameter(dbCommand, "Logo", DbType.String, model.Logo);
                    db.AddInParameter(dbCommand, "Status", DbType.String, model.Status);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                    db.AddInParameter(dbCommand, "CreateUserName", DbType.String, model.CreateUserName);
                    db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, model.ModifiedOn);
                    db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, model.ModifiedBy);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return 0;
                }
            }
        }
        /// <summary>
        /// Update a record
        /// </summary>
        public bool Update(CompanysEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Companys set ");
            strSql.Append("CompanyName=@CompanyName,");
            strSql.Append("Phone=@Phone,");
            strSql.Append("Fax=@Fax,");
            strSql.Append("Website=@Website,");
            strSql.Append("AssignedSystemUrl=@AssignedSystemUrl,");
            strSql.Append("Address1=@Address1,");
            strSql.Append("Address2=@Address2,");
            strSql.Append("City=@City,");
            strSql.Append("State=@State,");
            strSql.Append("Logo=@Logo,");
            strSql.Append("Status=@Status,");
            strSql.Append("CreatedOn=@CreatedOn,");
            strSql.Append("CreatedBy=@CreatedBy,");
            strSql.Append("CreateUserName=@CreateUserName,");
            strSql.Append("ModifiedOn=@ModifiedOn,");
            strSql.Append("ModifiedBy=@ModifiedBy");
            strSql.Append(" where ComID=@ComID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ComID", DbType.Int32, model.ComID);
                    db.AddInParameter(dbCommand, "CompanyName", DbType.String, model.CompanyName);
                    db.AddInParameter(dbCommand, "Phone", DbType.String, model.Phone);
                    db.AddInParameter(dbCommand, "Fax", DbType.String, model.Fax);
                    db.AddInParameter(dbCommand, "Website", DbType.String, model.Website);
                    db.AddInParameter(dbCommand, "AssignedSystemUrl", DbType.String, model.AssignedSystemUrl);
                    db.AddInParameter(dbCommand, "Address1", DbType.String, model.Address1);
                    db.AddInParameter(dbCommand, "Address2", DbType.String, model.Address2);
                    db.AddInParameter(dbCommand, "City", DbType.String, model.City);
                    db.AddInParameter(dbCommand, "State", DbType.String, model.State);
                    db.AddInParameter(dbCommand, "Logo", DbType.String, model.Logo);
                    db.AddInParameter(dbCommand, "Status", DbType.String, model.Status);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                    db.AddInParameter(dbCommand, "CreateUserName", DbType.String, model.CreateUserName);
                    db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, model.ModifiedOn);
                    db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, model.ModifiedBy);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        /// <summary>
        /// Delete a record
        /// </summary>
        public bool Delete(int ComID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Companys ");
            strSql.Append(" where ComID=@ComID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ComID", DbType.Int32, ComID);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        /// <summary>
        /// Get an object entity
        /// </summary>
        public CompanysEntity Get(int ComID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ComID,CompanyName,Phone,Fax,Website,AssignedSystemUrl,Address1,Address2,City,State,Logo,Status,CreatedOn,CreatedBy,CreateUserName,ModifiedOn,ModifiedBy from Companys ");
            strSql.Append(" where ComID=@ComID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ComID", DbType.Int32, ComID);
                    CompanysEntity model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = CompanysEntity.ReaderBind(dataReader, false);
                        }
                    }
                    return model;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        #endregion  Method

        #region ICompanyRepository Members

        public SearchCompaniesResponse SearchCompanies(SearchCompaniesRequest request)
        {
            int start = request.CurrentPage * request.PageCount + 1 - request.PageCount;
            int end = request.CurrentPage * request.PageCount;

            string strSelCount = " SELECT  COUNT(1)  FROM [Companys] ";
            string strOrderby = string.Format(" {0} {1} ", request.OrderExpression, request.OrderDirection);
            string strSelAttrs = @" SELECT *
                                    ,(SELECT COUNT(1) FROM Projects WHERE CompanyID = COM.ComID) AS ProjectsCount 
                                    ,(SELECT COUNT(1) FROM Users WHERE CompanyID = COM.ComID) AS ClientsCount
                                    FROM [Companys]  COM ";
            string strSelAttrsOrderBy = string.Format(@" Order BY {0}  ", strOrderby);
            string strSelPageModel = string.Format(@"SELECT * FROM(
                                                SELECT ROW_NUMBER() OVER(
                                                Order BY {0}) as  INDEX_ID,*
                                                ,(SELECT COUNT(1) FROM Projects WHERE CompanyID = COM.ComID) AS ProjectsCount  
                                                ,(SELECT COUNT(1) FROM Users WHERE CompanyID = COM.ComID) AS ClientsCount 
                                                FROM [Companys] COM ", strOrderby);
            string strWherePageModel = @") NEW_TB  WHERE INDEX_ID BETWEEN @Strat AND  @End;";
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" WHERE 1=1 ");
            switch (request.SearchType)
            {
                case SearchCompanyType.SingleCompany:
                    strWhere.Append(" AND [ComID]= @ComID ");
                    break;
                case SearchCompanyType.Project:
                    strWhere.Append(" AND [ComID] IN (SELECT CompanyID FROM Projects WHERE ProjectID =@ProjectID) ");
                    break;
                case SearchCompanyType.User:
                    strWhere.Append(" AND [ComID] IN (SELECT CompanyID FROM Users WHERE UserID =@UserID) ");
                    break;
                case SearchCompanyType.List:
                    strWhere.Append(" AND (CompanyName like @CompanyName or State  like @CompanyName or City  like @CompanyName )  ");
                    break;
                case SearchCompanyType.All:
                    break;
                default:
                    break;
            }

            StringBuilder strSql = new StringBuilder();
            if (request.IsPageModel)
            {
                strSql.Append(strSelCount);
                strSql.Append(strWhere);
                strSql.Append(";");
                strSql.Append(strSelPageModel);
                strSql.Append(strWhere);
                strSql.Append(strWherePageModel);
            }
            else
            {
                strSql.Append(strSelAttrs);
                strSql.Append(strWhere);
                strSql.Append(strSelAttrsOrderBy);
                strSql.Append(";");
            }
            SearchCompaniesResponse response = new SearchCompaniesResponse();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, request.UserID);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, request.ProjectID);
                    db.AddInParameter(dbCommand, "ComID", DbType.Int32, request.ComID);
                    db.AddInParameter(dbCommand, "CompanyName", DbType.String, string.Format("%{0}%", request.CompanyName.FilterSqlString()));

                    db.AddInParameter(dbCommand, "Strat", DbType.Int32, start);
                    db.AddInParameter(dbCommand, "End", DbType.Int32, end);

                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        List<CompanysEntity> list = new List<CompanysEntity>();

                        if (request.IsPageModel)
                        {
                            if (dataReader.Read())
                            {
                                response.ResultCount = dataReader.GetInt32(0);
                                dataReader.NextResult();
                            }
                        }
                        while (dataReader.Read())
                        {
                            list.Add(CompanysEntity.ReaderBind(dataReader, true));
                        }
                        response.ResultList = list;
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
            return response;
        }

        public bool ExistsCompanyName(string name, int exceptThis)
        {
            return base.ExistsRecords("Companys", "CompanyName", name.FilterSqlString(), "ComID", exceptThis.ToString());
        }

        public List<CompanysEntity> GetCompaniesHasUser()
        {
            string strSql = "Select distinct C.* from users U inner join  Companys C on U.CompanyID=C.ComID order by CompanyName asc";
            List<CompanysEntity> list = new List<CompanysEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            list.Add(CompanysEntity.ReaderBind(dataReader, false));
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
                return list;
            }

        }
        /// <summary>
        /// 获取公司的ID 
        /// </summary>
        /// <param name="CompanyName">公司名称</param>
        /// <returns></returns>
        public int GetCompanyId(string companyName)
        {
            string strSql = string.Format("select ComID from dbo.Companys where CompanyName='{0}'", companyName);
            int companyId = 0;
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    companyId = (int)db.ExecuteScalar(CommandType.Text, strSql);
                }
                catch (Exception ex)
                {
                    return 0;
                }
                return companyId;
            }
        }
        public List<CompanysEntity> GetCompaniesHasProject()
        {
            int comId = GetCompanyId("Sunnet");
            string strSql = string.Format(" Select distinct C.* from Projects U inner join Companys C on U.CompanyID=C.ComID where C.ComID != '{0}' order by CompanyName",comId);
            List<CompanysEntity> list = new List<CompanysEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            list.Add(CompanysEntity.ReaderBind(dataReader, false));
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql,
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                }
                return list;
            }
        }

        public Dictionary<int, CompanyProjectModel> GetCompanyProjectModels(int companyId, int projectId)
        {
            int comId = GetCompanyId("Sunnet");
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("SELECT DISTINCT ");
            sqlStr.Append("Companys.ComID, ");
            sqlStr.Append("Companys.CompanyName, ");
            sqlStr.Append("Projects.ProjectID, ");
            sqlStr.Append("Projects.Title ");
            sqlStr.Append("FROM  dbo.Companys AS Companys ");
            sqlStr.Append("INNER JOIN dbo.Projects AS Projects ON Projects.CompanyID = Companys.ComID ");
            sqlStr.Append("INNER JOIN dbo.TimeSheets AS TS ON TS.ProjectID = Projects.ProjectID ");
            sqlStr.Append("WHERE ");
            sqlStr.Append("NOT EXISTS (SELECT * FROM dbo.TSInvoiceRelation AS TIR WHERE TIR.TSId = TS.ID) ");
            sqlStr.Append("AND Companys.ComID != "+comId+"");
            if (companyId > 0 && projectId == 0)
            {
                sqlStr.AppendFormat("AND Companys.ComID={0} ", companyId);
            }
            else if (projectId > 0 && companyId == 0)
            {
                sqlStr.AppendFormat("AND Projects.ProjectID={0} ", projectId);
            }
            else if (companyId > 0 && projectId > 0)
            {
                sqlStr.AppendFormat("AND Projects.ProjectID={0} AND Companys.ComID={1} ", projectId, companyId);
            }

            Database db = DatabaseFactory.CreateDatabase();
            Dictionary<int, CompanyProjectModel> dicModels = new Dictionary<int, CompanyProjectModel>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sqlStr.ToString()))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            int comid = 0;
                            object obj = new object();
                            obj = dataReader["ComID"];
                            if (obj != null && obj != DBNull.Value)
                            {
                                comid = (int)obj;
                            }

                            if (comid > 0 && !dicModels.ContainsKey(comid))
                            {
                                CompanyProjectModel model = new CompanyProjectModel();
                                model.CompanyId = comid;
                                model.CompanyName = dataReader["CompanyName"].ToString();
                                model.Projects = new List<ProjectSelectModel>();
                                dicModels.Add(comid, model);
                            }
                            if (comid > 0)
                            {
                                ProjectSelectModel projectModel = new ProjectSelectModel();
                                obj = dataReader["ProjectID"];
                                if (obj != null && obj != DBNull.Value)
                                {
                                    projectModel.ProjectId = (int)obj;
                                }
                                projectModel.ProjectTitle = dataReader["Title"].ToString();
                                dicModels[comid].Projects.Add(projectModel);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        sqlStr,
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                }
            }
            return dicModels;
        }
        #endregion
    }
}

