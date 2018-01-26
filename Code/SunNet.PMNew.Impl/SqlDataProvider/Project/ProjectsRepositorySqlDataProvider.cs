using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Core.ProjectModule;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Entity.ProjectModel.ProjectTicket;
using SunNet.PMNew.Entity.TicketModel;
using System.Linq;

namespace SunNet.PMNew.Impl.SqlDataProvider.Project
{
    /// <summary>
    /// Data access:Projects
    /// </summary>
    public class ProjectsRepositorySqlDataProvider : SqlHelper, IProjectsRepository
    {
        public ProjectsRepositorySqlDataProvider()
        { }
        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(ProjectsEntity model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Projects(");
            strSql.Append("CompanyID,ProjectCode,Title,Description,StartDate,EndDate,Status,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,PMID,Priority")
            .Append(",Billable,TestLinkURL,TestUserName,TestPassword,FreeHour,BugNeedApproved,RequestNeedApproved,IsOverFreeTime,MaintenancePlanOption,TotalHours)");

            strSql.Append(" values (");
            strSql.Append("@CompanyID,@ProjectCode,@Title,@Description,@StartDate,@EndDate,@Status,@CreatedBy,@CreatedOn,@ModifiedBy,@ModifiedOn,@PMID")
            .Append(",@Priority,@Billable,@TestLinkURL,@TestUserName,@TestPassword,@FreeHour,@BugNeedApproved,@RequestNeedApproved,@IsOverFreeTime,@MaintenancePlanOption,@TotalHours)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);declare @ForumShortName varchar(500);set @ForumShortName='Requirement'+convert(varchar(50),@@IDENTITY); exec [" + Config.NearForumsDataBase
                + "].[dbo].[SPForumsInsert] @Title,@ForumShortName,@Title,1,1,NULL,1,0,@@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, model.CompanyID);
                    db.AddInParameter(dbCommand, "ProjectCode", DbType.String, model.ProjectCode);
                    db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                    db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, model.StartDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, model.EndDate);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, model.Status);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, model.ModifiedBy);
                    db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, model.ModifiedOn);
                    db.AddInParameter(dbCommand, "PMID", DbType.Int32, model.PMID);
                    db.AddInParameter(dbCommand, "Priority", DbType.String, model.Priority);
                    db.AddInParameter(dbCommand, "Billable", DbType.Boolean, model.Billable);
                    db.AddInParameter(dbCommand, "TestLinkURL", DbType.String, model.TestLinkURL);
                    db.AddInParameter(dbCommand, "TestUserName", DbType.String, model.TestUserName);
                    db.AddInParameter(dbCommand, "TestPassword", DbType.String, model.TestPassword);
                    db.AddInParameter(dbCommand, "FreeHour", DbType.Int32, model.FreeHour);
                    db.AddInParameter(dbCommand, "BugNeedApproved", DbType.Boolean, model.BugNeedApproved);
                    db.AddInParameter(dbCommand, "RequestNeedApproved", DbType.Boolean, model.RequestNeedApproved);
                    db.AddInParameter(dbCommand, "IsOverFreeTime", DbType.Boolean, model.IsOverFreeTime);
                    db.AddInParameter(dbCommand, "MaintenancePlanOption", DbType.String, model.MainPlanOption);
                    db.AddInParameter(dbCommand, "TotalHours", DbType.Decimal, model.TotalHours);
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
        public bool Update(ProjectsEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Projects set ");
            strSql.Append("CompanyID=@CompanyID,");
            strSql.Append("ProjectCode=@ProjectCode,");
            strSql.Append("Title=@Title,");
            strSql.Append("Description=@Description,");
            strSql.Append("StartDate=@StartDate,");
            strSql.Append("EndDate=@EndDate,");
            strSql.Append("Status=@Status,");
            strSql.Append("CreatedBy=@CreatedBy,");
            strSql.Append("CreatedOn=@CreatedOn,");
            strSql.Append("ModifiedBy=@ModifiedBy,");
            strSql.Append("ModifiedOn=@ModifiedOn,");
            strSql.Append("PMID=@PMID,");
            strSql.Append("Priority=@Priority,");
            strSql.Append("Billable=@Billable,");
            strSql.Append("TestLinkURL=@TestLinkURL,");
            strSql.Append("TestUserName=@TestUserName,");
            strSql.Append("TestPassword=@TestPassword,");
            strSql.Append("FreeHour=@FreeHour,");
            strSql.Append("BugNeedApproved=@BugNeedApproved,");
            strSql.Append("RequestNeedApproved=@RequestNeedApproved,");
            strSql.Append("IsOverFreeTime=@IsOverFreeTime,");
            strSql.Append("TotalHours=@TotalHours,");
            strSql.Append("MaintenancePlanOption=@MaintenancePlanOption");
            strSql.Append(" where ProjectID=@ProjectID ;");
            strSql.Append(" update [" + Config.NearForumsDataBase + "].[dbo].[Forums] set [ForumName]=@Title,[ForumDescription]=@Title,");
            strSql.Append("[Active]=(case when @Status=5 then 0 else 1 end) where ProjectID=@ProjectID");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                    db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, model.CompanyID);
                    db.AddInParameter(dbCommand, "ProjectCode", DbType.String, model.ProjectCode);
                    db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                    db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, model.StartDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, model.EndDate);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, model.Status);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, model.ModifiedBy);
                    db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, model.ModifiedOn);
                    db.AddInParameter(dbCommand, "PMID", DbType.Int32, model.PMID);
                    db.AddInParameter(dbCommand, "Priority", DbType.String, model.Priority);
                    db.AddInParameter(dbCommand, "Billable", DbType.Boolean, model.Billable);
                    db.AddInParameter(dbCommand, "TestLinkURL", DbType.String, model.TestLinkURL);
                    db.AddInParameter(dbCommand, "TestUserName", DbType.String, model.TestUserName);
                    db.AddInParameter(dbCommand, "TestPassword", DbType.String, model.TestPassword);
                    db.AddInParameter(dbCommand, "FreeHour", DbType.Int32, model.FreeHour);
                    db.AddInParameter(dbCommand, "BugNeedApproved", DbType.Boolean, model.BugNeedApproved);
                    db.AddInParameter(dbCommand, "RequestNeedApproved", DbType.Boolean, model.RequestNeedApproved);
                    db.AddInParameter(dbCommand, "IsOverFreeTime", DbType.Boolean, model.IsOverFreeTime);
                    db.AddInParameter(dbCommand, "MaintenancePlanOption", DbType.String, model.MaintenancePlanOption);
                    db.AddInParameter(dbCommand, "TotalHours", DbType.String, model.TotalHours);
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
        public bool Delete(int ProjectID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Projects ");
            strSql.Append(" where ProjectID=@ProjectID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, ProjectID);
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
        public ProjectsEntity Get(int ProjectID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *,(SELECT UserName FROM Users WHERE UserID =ModifiedBy) AS UserName  from Projects ");
            strSql.Append(" where ProjectID=@ProjectID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, ProjectID);
                    ProjectsEntity model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = ProjectsEntity.ReaderBind(dataReader);
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

        #region IProjectsRepository Members

        public SearchProjectsResponse SearchProjects(SearchProjectsRequest request)
        {
            int start = request.CurrentPage * request.PageCount + 1 - request.PageCount;
            int end = request.CurrentPage * request.PageCount;

            string strSelCount = " SELECT COUNT(1)  FROM  Projects p";
            string strOrderby = string.Format(" ({0}) {1} ", request.OrderExpression, request.OrderDirection);
            string strSelAttrs = @" SELECT p.*, (SELECT UserName FROM Users WHERE UserID =P.ModifiedBy) AS ModifiedByUserName, 
                                                (SELECT UserName FROM Users WHERE UserID =P.CreatedBy) AS CreatedByUserName,
                                                (SELECT UserName FROM Users WHERE UserID =P.PMID) AS PMUserName,
                                                (SELECT FirstName FROM Users WHERE UserID =P.PMID) AS PMFirstName,
                                                (SELECT LastName FROM Users WHERE UserID =P.PMID) AS PMLastName,
                                                (select  CompanyName from   Companys where  ComID=p.CompanyID ) as CompanyName  
                                    FROM   Projects p  ";
            string strSelAttrsOrderBy = string.Format(@" Order BY {0}  ", strOrderby);
            string strSelPageModel = string.Format(@"SELECT * FROM(
                                                SELECT ROW_NUMBER() OVER(
                                                Order BY {0}) as  INDEX_ID,p.* , (SELECT UserName FROM Users WHERE UserID =P.ModifiedBy) AS ModifiedByUserName, 
                                                                                    (SELECT UserName FROM Users WHERE UserID =P.CreatedBy) AS CreatedByUserName,
                                                                                    (SELECT UserName FROM Users WHERE UserID =P.PMID) AS PMUserName ,
                                                                                    (SELECT FirstName FROM Users WHERE UserID =P.PMID) AS PMFirstName,
                                                                                    (SELECT LastName FROM Users WHERE UserID =P.PMID) AS PMLastName,
                                                                                    (select  CompanyName from   Companys where  ComID=p.CompanyID ) as CompanyName  
                                                FROM   Projects p   ", strOrderby);
            string strWherePageModel = @") NEW_TB  WHERE INDEX_ID BETWEEN @Strat AND  @End;";
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" WHERE 1=1 ");
            switch (request.SearchType)
            {
                case SearchProjectsType.All:
                    break;
                case SearchProjectsType.AllExceptAssigned:
                    strWhere.Append(" AND  p.[ProjectID] not in (Select ProjectID from ProjectUsers pu where pu.UserID=@UserID)");
                    strWhere.Append(" AND (p.Title LIKE @Keywords OR p.ProjectCode LIKE @Keywords )");
                    break;
                case SearchProjectsType.List:
                    strWhere.Append(" AND (p.Title LIKE @Keywords OR p.ProjectCode LIKE @Keywords )");
                    if (request.CompanyID != 0)
                    {
                        strWhere.Append(" AND CompanyID=@CompanyID");
                    }
                    break;
                case SearchProjectsType.Company:
                    if (request.CompanyID != 0)
                        strWhere.Append(" AND  p.[CompanyID] = @CompanyID");
                    strWhere.Append(" AND (p.Title LIKE @Keywords OR p.ProjectCode LIKE @Keywords )");
                    break;
                case SearchProjectsType.CompanyExceptAssigned:
                    if (request.CompanyID != 0)
                        strWhere.Append(" AND  p.[CompanyID] = @CompanyID");
                    strWhere.Append(" AND (p.Title LIKE @Keywords OR p.ProjectCode LIKE @Keywords )");
                    strWhere.Append(" AND  p.[ProjectID] not in (Select ProjectID from ProjectUsers pu where pu.UserID=@UserID)");
                    break;

                case SearchProjectsType.SingleInstance:
                    strWhere.Append(" AND  p.[ProjectID] = @ProjectID");
                    break;
                case SearchProjectsType.ListByUserID:
                    strWhere.Append(" AND  p.[ProjectID] in (Select ProjectID from ProjectUsers pu where pu.UserID=@UserID)");
                    break;
                case SearchProjectsType.Ticket:
                    strWhere.Append(" AND  p.[ProjectID] IN (SELECT   [ProjectID] FROM [Tickets] WHERE [TicketID]=@TicketID)");
                    break;
                default: break;
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
            List<ProjectDetailDTO> list;
            SearchProjectsResponse response = new SearchProjectsResponse();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, request.UserID);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, request.TicketID);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, request.ProjectID);
                    db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, request.CompanyID);
                    db.AddInParameter(dbCommand, "Keywords", DbType.String, string.Format("%{0}%", request.Keywords.FilterSqlString()));
                    db.AddInParameter(dbCommand, "Strat", DbType.Int32, start);
                    db.AddInParameter(dbCommand, "End", DbType.Int32, end);

                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        list = new List<ProjectDetailDTO>();
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
                            list.Add(ProjectDetailDTO.ReaderBind(dataReader));
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

        #region get active project by user id

        #endregion

        public bool CheckExistsTitle(string title, int exceptThis)
        {
            return base.ExistsRecords("Projects", "Title", title, "ProjectID", exceptThis.ToString());
        }

        public bool CheckExistsCode(string code, int exceptThis)
        {
            return base.ExistsRecords("Projects", "ProjectCode", code, "ProjectID", exceptThis.ToString());
        }

        public List<UsersEntity> GetPojectClientUsers(int projectId, int companyId)
        {
            List<UsersEntity> list = new List<UsersEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("	select u.UserID,u.FirstName,u.LastName,u.UserName,u.Phone from projectusers pu ")
                .Append(" 	inner join users u on u.userid = pu.userid ")
                .AppendFormat(" where isclient = 1 and 	 pu.projectid = {0} and u.companyid={1} and u.Status ='ACTIVE' and u.RoleID={2}", projectId, companyId, (int)RolesEnum.CLIENT);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                        list.Add(new UsersEntity()
                        {
                            UserID = (int)dataReader["UserID"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            UserName = (string)dataReader["UserName"],
                            Phone = (string)dataReader["Phone"]
                        });
                }
            }
            return list;
        }

        public List<UsersEntity> GetPojectPmUsers(int projectId, int companyId)
        {
            List<UsersEntity> list = new List<UsersEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("	select u.UserID,u.FirstName,u.LastName,u.UserName,u.Phone from projectusers pu ")
                .Append(" 	inner join users u on u.userid = pu.userid ")
                .AppendFormat(" where isclient = 0  and 	 pu.projectid = {0} and u.companyid={1} and u.Status ='ACTIVE' and u.RoleID={2}", projectId, companyId, (int)RolesEnum.PM);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                        list.Add(new UsersEntity()
                        {
                            UserID = (int)dataReader["UserID"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            UserName = (string)dataReader["UserName"],
                            Phone = (string)dataReader["Phone"]
                        });
                }
            }
            return list;
        }

        public List<int> GetProjectIdByClientID(int userId)
        {
            List<int> list = new List<int>();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select ProjectID from dbo.ProjectUsers where userID={0} ", userId);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        list.Add((int)dataReader["ProjectID"]);
                    }
                }
            }
            return list;
        }

        public float GetProjectTimeSheetTime(int projectId)
        {
            object obj = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select sum(Hours) from TimeSheets where ProjectID={0} ", projectId);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {

                obj = db.ExecuteScalar(dbCommand);
            }
            if (obj != null && obj != DBNull.Value)
            {
                return float.Parse(obj.ToString());
            }
            else
            {
                return 0;
            }

        }

        public bool updateRemainHoursSendEmailStatus(bool hasSend, int projectId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Projects set ");
            strSql.Append(" hasSendRemainHourEmail=@hasSendRemainHourEmail ");
            strSql.Append(" where ProjectID=@ProjectID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "hasSendRemainHourEmail", DbType.Boolean, hasSend);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, projectId);

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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString()
                        , base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        /// <summary>
        /// 查询Project ongoing tickets
        /// </summary>
        /// <param name="internalProject">是否查询sunnet内部项目</param>
        /// <returns></returns>
        public List<ProjectTicketModel> GetProjectTicketList(bool internalProject, int userId)
        {
            List<ProjectStatus> projectStatus = new List<ProjectStatus>();
            projectStatus.Add(ProjectStatus.Open);
            projectStatus.Add(ProjectStatus.Scheduled);
            projectStatus.Add(ProjectStatus.InProcess);
            projectStatus.Add(ProjectStatus.Other);
            List<TicketsState> ticketStatus = TicketsStateHelper.SunnetUSAllowShowStatus;
            ticketStatus.Remove(TicketsState.Cancelled);
            ticketStatus.Remove(TicketsState.Completed);
            ticketStatus.Remove(TicketsState.Internal_Cancel);
            StringBuilder strProject = new StringBuilder();
            strProject.Append("SELECT  P.ProjectID,P.ProjectCode,P.Title AS ProjectName, P.Status AS ProjectStatus, P.PMID, C.ComID, C.CompanyName ");

            StringBuilder strProjectWhere = new StringBuilder();
            strProjectWhere.Append("FROM dbo.Projects AS P ");
            strProjectWhere.Append("LEFT JOIN dbo.Companys AS C ON C.ComID = P.CompanyID ");
            if (internalProject)
            {
                strProjectWhere.Append("WHERE C.CompanyName = 'Sunnet' ");
            }
            else
            {
                strProjectWhere.Append("WHERE C.CompanyName != 'Sunnet' ");
            }
            strProjectWhere.AppendFormat("AND P.Status IN ({0}) ", string.Join(",", projectStatus.Select(x => (int)x).ToList()));
            strProjectWhere.Append("AND P.ProjectID IN (SELECT DISTINCT PU.ProjectID FROM dbo.ProjectUsers AS PU WHERE PU.UserID = @UserID ) ");
            strProject.Append(strProjectWhere.ToString());
            strProject.Append("ORDER BY P.Title ");

            StringBuilder strTicket = new StringBuilder();
            strTicket.Append("SELECT T.TicketID , T.ProjectID , T.Status AS TicketStatus, T.Title, T.ResponsibleUser AS ResponsibleUserID, T.Priority ");
            strTicket.Append("FROM dbo.Tickets AS T ");
            strTicket.AppendFormat("WHERE T.Status IN ({0}) ", string.Join(",", ticketStatus.Select(x => (int)x).ToList()));
            strTicket.Append("AND T.ProjectID IN (SELECT P.ProjectID ");
            strTicket.Append(strProjectWhere.ToString());
            strTicket.Append(") ");
            strTicket.Append("ORDER BY T.ModifiedOn DESC;");

            string strSql = strTicket.ToString() + strProject.ToString();
            Database db = DatabaseFactory.CreateDatabase();
            List<ProjectTicketModel> modelList = new List<ProjectTicketModel>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
                    using (IDataReader dr = db.ExecuteReader(dbCommand))
                    {
                        List<TicketStatusModel> ticketList = new List<TicketStatusModel>();
                        while (dr.Read())
                        {
                            TicketStatusModel ticket = new TicketStatusModel
                            {
                                TicketID = int.Parse(dr["TicketID"].ToString()),
                                ProjectID = int.Parse(dr["ProjectID"].ToString()),
                                TicketStatus = (TicketsState)dr["TicketStatus"],
                                Title = dr["Title"].ToString(),
                                ResponsibleUserID = int.Parse(dr["ResponsibleUserID"].ToString()),
                                Priority = (PriorityState)dr["Priority"]
                            };
                            ticketList.Add(ticket);
                        }
                        bool isNext = dr.NextResult();
                        while (dr.Read() && isNext)
                        {
                            ProjectTicketModel project = new ProjectTicketModel
                            {
                                ProjectID = int.Parse(dr["ProjectID"].ToString()),
                                ProjectCode = dr["ProjectCode"].ToString(),
                                ProjectName = dr["ProjectName"].ToString(),
                                ProjectStatus = (ProjectStatus)dr["ProjectStatus"],
                                PMID = int.Parse(dr["PMID"].ToString()),
                                CompanyID = int.Parse(dr["ComID"].ToString()),
                                CompanyName = dr["CompanyName"].ToString()
                            };
                            project.Tickets = ticketList.Where(t => t.ProjectID == project.ProjectID).Take(10).ToList();
                            project.OngoingTickets = ticketList.Count(t => t.ProjectID == project.ProjectID);
                            modelList.Add(project);
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
            }
            return modelList;
        }
        #endregion
    }
}

