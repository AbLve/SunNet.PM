using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Core.TimeSheetModule;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.TicketModel;


namespace SunNet.PMNew.Impl.SqlDataProvider.TimeSheet
{
    /// <summary>
    /// Data access:TimeSheets
    /// </summary>
    public class TimeSheetsRepositorySqlDataProvider : SqlHelper, ITimeSheetRepository
    {
        public TimeSheetsRepositorySqlDataProvider()
        { }
        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(TimeSheetsEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"if( not exists (select 1 from TimeSheets where TicketID=@TicketID and UserID=@UserID and SheetDate=@SheetDate ) ) 
                            begin ");
            strSql.Append("insert into TimeSheets(");
            strSql.Append("SheetDate,ProjectID,TicketID,UserID,Hours,Percentage,Description,IsSubmitted,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsMeeting,EventID)");

            strSql.Append(" values (");
            strSql.Append("@SheetDate,@ProjectID,@TicketID,@UserID,@Hours,@Percentage,@Description,@IsSubmitted,@CreatedOn,@CreatedBy,@ModifiedOn,@ModifiedBy,@IsMeeting,@EventID);");
            strSql.Append(@"select ISNULL( SCOPE_IDENTITY(),0);
                            end ");
            strSql.Append("else select -1 ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "SheetDate", DbType.DateTime, model.SheetDate);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
                    db.AddInParameter(dbCommand, "Hours", DbType.Decimal, model.Hours);
                    db.AddInParameter(dbCommand, "Percentage", DbType.Decimal, model.Percentage);
                    db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                    db.AddInParameter(dbCommand, "IsSubmitted", DbType.Boolean, model.IsSubmitted);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                    db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, model.ModifiedOn);
                    db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, model.ModifiedBy);
                    db.AddInParameter(dbCommand, "IsMeeting", DbType.Boolean, model.IsMeeting);
                    db.AddInParameter(dbCommand, "EventID", DbType.Int32, model.EventID);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return 0;
                }
            }
        }
        /// <summary>
        /// Update a record
        /// </summary>
        public bool Update(TimeSheetsEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TimeSheets set ");
            strSql.Append("SheetDate=@SheetDate,");
            strSql.Append("ProjectID=@ProjectID,");
            strSql.Append("TicketID=@TicketID,");
            strSql.Append("UserID=@UserID,");
            strSql.Append("Hours=@Hours,");
            strSql.Append("Percentage=@Percentage,");
            strSql.Append("Description=@Description,");
            strSql.Append("IsSubmitted=@IsSubmitted,");
            strSql.Append("CreatedOn=@CreatedOn,");
            strSql.Append("CreatedBy=@CreatedBy,");
            strSql.Append("ModifiedOn=@ModifiedOn,");
            strSql.Append("ModifiedBy=@ModifiedBy,");
            strSql.Append("IsMeeting=@IsMeeting, ");
            strSql.Append("EventID=@EventID");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, model.ID);
                    db.AddInParameter(dbCommand, "SheetDate", DbType.DateTime, model.SheetDate);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
                    db.AddInParameter(dbCommand, "Hours", DbType.Decimal, model.Hours);
                    db.AddInParameter(dbCommand, "Percentage", DbType.Decimal, model.Percentage);
                    db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                    db.AddInParameter(dbCommand, "IsSubmitted", DbType.Boolean, model.IsSubmitted);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                    db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, model.ModifiedOn);
                    db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, model.ModifiedBy);
                    db.AddInParameter(dbCommand, "IsMeeting", DbType.Boolean, model.IsMeeting);
                    db.AddInParameter(dbCommand, "EventID", DbType.Int32, model.EventID);
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

        /// <summary>
        /// Delete a record
        /// </summary>
        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TimeSheets ");
            strSql.Append(" where ID=@ID and IsSubmitted = 0 ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
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

        /// <summary>
        /// Delete a record
        /// </summary>
        public bool DeleteByEventId(int EventID, DateTime Day)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TimeSheets ");
            strSql.Append(" where EventID=@EventID and SheetDate=@SheetDate");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "EventID", DbType.Int32, EventID);
                    db.AddInParameter(dbCommand, "SheetDate", DbType.DateTime, Day);
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

        /// <summary>
        /// Delete a record
        /// </summary>
        public bool DeleteByUserAndDate(int UserID, DateTime Day)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TimeSheets ");
            strSql.Append(" where UserID=@UserID and SheetDate=@SheetDate");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
                    db.AddInParameter(dbCommand, "SheetDate", DbType.DateTime, Day);
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

        /// <summary>
        /// Get an object entity
        /// </summary>
        public TimeSheetsEntity Get(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TimeSheets ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
                    TimeSheetsEntity model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = TimeSheetsEntity.ReaderBind(dataReader);
                        }
                    }
                    return model;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        /// <summary>
        /// Get an object entity
        /// </summary>
        public TimeSheetsEntity GetByEventId(int EventID, DateTime Day)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TimeSheets ");
            strSql.Append(" where EventID=@EventID and SheetDate=@SheetDate");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "EventID", DbType.Int32, EventID);
                    db.AddInParameter(dbCommand, "SheetDate", DbType.DateTime, Day);
                    TimeSheetsEntity model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = TimeSheetsEntity.ReaderBind(dataReader);
                        }
                    }
                    return model;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        public TimeSheetsEntity GetByUserId(int UserID, DateTime Day)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from TimeSheets ");
            strSql.Append(" where UserID=@UserID and SheetDate=@SheetDate");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
                    db.AddInParameter(dbCommand, "SheetDate", DbType.DateTime, Day);
                    TimeSheetsEntity model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = TimeSheetsEntity.ReaderBind(dataReader);
                        }
                    }
                    return model;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        #endregion  Method

        #region ITimeSheetRepository Members

        public decimal GetTimesheetsHoursByWeek(int userID, DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder("");
            strSql.Append("SELECT SUM(Hours) ");
            strSql.Append("FROM TimeSheets ");
            strSql.Append("WHERE  SheetDate BETWEEN @StartDate AND @EndDate ");
            if (userID > 0)
            {
                strSql.Append(" AND UserID=@UserID ");
            }
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, startDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, endDate);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, userID);

                    decimal hours = Convert.ToDecimal(db.ExecuteScalar(dbCommand));
                    return hours;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                          strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return 0;
                }
            }
        }
        public int UpdateTimesheets(DateTime submitDate, int userID, bool submitted)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TimeSheets set ");
            strSql.Append("IsSubmitted=@IsSubmitted");
            strSql.Append(@" where SheetDate between @StartDate and @EndDate  
                                     And UserID=@UserID  
                                     and IsSubmitted <> @IsSubmitted");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    DateTime startDate = new DateTime(submitDate.Year, submitDate.Month, submitDate.Day, 0, 0, 0);
                    DateTime endDate = new DateTime(submitDate.Year, submitDate.Month, submitDate.Day, 23, 59, 59);

                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, userID);
                    db.AddInParameter(dbCommand, "IsSubmitted", DbType.Boolean, submitted);
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, startDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, endDate);
                    int rows = db.ExecuteNonQuery(dbCommand);

                    return rows;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return -1;
                }
            }
        }

        public List<TimeSheetTicket> SearchTimeSheets(int categoryID, DateTime startDate, DateTime endDate, int userID, int projectID, bool addDefaultEmptyModel)
        {
            StringBuilder strSql = new StringBuilder("TimeSheets_Select");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetStoredProcCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryID);
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, startDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, endDate);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, userID);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, projectID);

                    List<TimeSheetTicket> list = new List<TimeSheetTicket>();
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        while (dataReader.Read())
                        {
                            list.Add(TimeSheetTicket.ReaderBind(dataReader, startDate));
                        }
                    if (addDefaultEmptyModel && list.Count == 0)
                    {
                        TimeSheetTicket model = TimeSheetTicket.GetEmptyModel();
                        model.SheetDate = startDate;
                        list.Add(model);
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                          strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        public DataTable GetAllTimeSheetRecord(int userID, DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder("");
            strSql.Append("SELECT  UserID,SheetDate,SUM(Hours) AS [Hours] ");
            strSql.Append("FROM TimeSheets ");
            strSql.Append("WHERE  SheetDate BETWEEN @StartDate AND @EndDate");
            if (userID > 0)
            {
                strSql.Append(" AND UserID=@UserID ");
            }
            strSql.Append(" GROUP BY UserID , SheetDate");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, startDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, endDate);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, userID);

                    DataSet ds = db.ExecuteDataSet(dbCommand);
                    return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                          strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        public DataTable GetUnFinishedTimeSheets(int userID, DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder("");
            strSql.Append("SELECT  UserID,SheetDate,SUM(Hours) AS [Hours] ");
            strSql.Append("FROM TimeSheets ");
            strSql.Append("WHERE  SheetDate BETWEEN @StartDate AND @EndDate  AND IsSubmitted=0 ");
            if (userID > 0)
            {
                strSql.Append(" AND UserID=@UserID ");
            }
            strSql.Append(" GROUP BY UserID , SheetDate");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, startDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, endDate);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, userID);

                    DataSet ds = db.ExecuteDataSet(dbCommand);
                    return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                          strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        public SearchTimeSheetsResponse QueryTimesheets(SearchTimeSheetsRequest request)
        {
            string orderby = string.Empty;
            string keyQuery = "";
            if (request.OrderExpression.ToLower() == "projecttitle")
            {
                orderby = "p.Title";
            }
            else if (request.OrderExpression.ToLower() == "tickettitle")
            {
                orderby = "t.Title";
            }
            else if (request.OrderExpression.ToLower() == "firstname")
                orderby = "u.FirstName";
            else
            {
                if (request.OrderExpression != "ticketcode, sheetdate")
                    orderby = "ts." + request.OrderExpression;
                else
                    orderby = request.OrderExpression;
            }

            StringBuilder strDataSource = new StringBuilder();
            strDataSource.Append(@"SELECT ts.ID as TimeSheetID,p.CompanyID ,t.Accounting, ts.SheetDate, ts.ProjectID, ts.TicketID,
                            ts.UserID, ts.Hours, ts.Percentage, ts.Description AS WorkDetail, ts.IsSubmitted, 
                            ts.CreatedOn, ts.CreatedBy, ts.ModifiedOn, ts.ModifiedBy, ts.IsMeeting,
                            p.Title AS ProjectTitle,
                            t.Title AS TicketTitle,t.Description as TicketDescription,(t.[TicketCode]+Cast(t.[TicketID] as varchar)) as TicketCode, 
                            r.RoleName as RoleName,
                            u.FirstName ,u.LastName 
                            FROM TimeSheets ts LEFT OUTER JOIN 
                            Projects  p ON ts.ProjectID = p.ProjectID LEFT OUTER JOIN 
                            Tickets  t ON ts.TicketID = t.TicketID LEFT OUTER JOIN 
                            Users u ON ts.UserID = u.UserID LEFT OUTER JOIN 
                            Roles r On u.RoleID = r.RoleID ");
            strDataSource.Append(" WHERE 1=1 ");
            switch (request.SearchTimeSheetsType)
            {
                case SearchType.QueryReport:
                    if (request.UserID > 0)
                        strDataSource.Append(" And ts.UserID=@UserID ");
                    if (request.CompanyID > 0)
                        strDataSource.Append(" And p.CompanyID=@CompanyID ");
                    if (request.Accounting> 0)
                        strDataSource.Append(" And t.Accounting=@Accounting");
                    if (request.ProjectID > 0)
                        strDataSource.Append(" And ts.ProjectID=@ProjectID");
                    if (request.TicketID > 0)
                        strDataSource.Append(" And ts.TicketID=@TicketID");
                    if (request.StartDate != null && request.StartDate > DateTime.MinValue)
                        strDataSource.Append(" And ts.SheetDate >= @StartDate");
                    if (request.EndDate != null && request.EndDate > DateTime.MinValue)
                        strDataSource.Append(" And ts.SheetDate <= @EndDate");
                    if (!string.IsNullOrEmpty(request.Keywords))
                    {
                        //string[] keys = request.Keywords.Split(' ');

                        //foreach (string key in keys)
                        //{
                        //    if (key.Trim() != "")
                        //    {
                        //        keyQuery += ",'"+key.Trim().Replace("'","''")+"'";
                        //    }
                        //}

                        strDataSource.Append(SplitKeywords(request.Keywords, "p.Title", "t.Title", "t.TicketID"));
                        // strDataSource.Append(" And (p.Title like @Keyword or t.Title like @Keyword) ");
                    }
                    //strDataSource.Append(" And (p.Title like @Keyword or t.Title like @Keyword) ");

                    if (request.WID > 0)
                        strDataSource.Append(" And ts.TicketID in (select tid from ProposalTrackerRelation where WID = @WID)");
                    break;
                case SearchType.EmailNotice:
                    strDataSource.Append(" AND u.Office=@Office ");
                    strDataSource.Append(" And ts.SheetDate = @SheetDate ");
                    //save is effictive    
                    //strDataSource.Append(" And ts.IsSubmitted = 1 ");
                    break;
                default: break;
            }
            if (!request.IsPageModel)
                strDataSource.AppendFormat(" Order by {0} {1} ",
                    orderby, request.OrderDirection);

            StringBuilder strSql = new StringBuilder();
            if (request.IsPageModel)
            {
                strSql.Append(" Select count(1) from ");
                strSql.Append("(");
                strSql.Append(strDataSource);
                strSql.Append(") as SearchResult ;");
                strSql.Append(" Select * from (");
                strSql.AppendFormat("  Select ROW_NUMBER() OVER( Order BY {0} {1}) as  INDEX_ID,SearchResult.* From "
                    , request.OrderExpression, request.OrderDirection);
                strSql.Append("(");
                strSql.Append(strDataSource);
                strSql.Append(") as SearchResult  ");
                strSql.Append(") as NEW_TB ");
                strSql.Append(" Where Index_ID between @Start and @End ");
            }
            else
            {
                strSql.Append(strDataSource);
            }

            SearchTimeSheetsResponse response = new SearchTimeSheetsResponse();
            List<TimeSheetTicket> list;
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    int start = request.CurrentPage * request.PageCount + 1 - request.PageCount;
                    int end = request.CurrentPage * request.PageCount;

                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, request.UserID);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, request.ProjectID);
                    db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, request.CompanyID);
                    db.AddInParameter(dbCommand, "Accounting", DbType.Int32, request.Accounting);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, request.TicketID);
                    db.AddInParameter(dbCommand, "WID", DbType.Int32, request.WID);
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, request.StartDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, request.EndDate);
                    db.AddInParameter(dbCommand, "Keyword", DbType.String, string.Format("%{0}%", request.Keywords.FilterSqlString()));
                    db.AddInParameter(dbCommand, "Start", DbType.Int32, start);
                    db.AddInParameter(dbCommand, "End", DbType.Int32, end);

                    db.AddInParameter(dbCommand, "Office", DbType.String, string.IsNullOrEmpty(request.Office) ? "CN" : request.Office.ToUpper());
                    db.AddInParameter(dbCommand, "SheetDate", DbType.DateTime, request.SearchDate.Date);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        list = new List<TimeSheetTicket>();
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
                            list.Add(TimeSheetTicket.ReaderBindForReport(dataReader));
                        }
                        response.TimeSheetsList = list;
                        response.IsError = false;
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                         strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    response.IsError = true;
                    return response;
                }
            }
        }

        public SearchTimeSheetsResponse QueryTimesheetsWithTickets(SearchTimeSheetsRequest request)
        {
            string orderby = string.Empty;
            string keyQuery = "";
            if (request.OrderExpression.ToLower() == "projecttitle")
            {
                orderby = "p.Title";
            }
            else if (request.OrderExpression.ToLower() == "tickettitle")
            {
                orderby = "t.Title";
            }
            else if (request.OrderExpression.ToLower() == "firstname")
                orderby = "u.FirstName";
            else
            {
                if (request.OrderExpression != "ticketcode, sheetdate")
                    orderby = "ts." + request.OrderExpression;
                else
                    orderby = request.OrderExpression;
            }

            //Ticket报表
            StringBuilder strTicketDataSource = new StringBuilder();
            strTicketDataSource.Append(@"select t.ProjectID,p.Title as 'ProjectTitle', t.ModifiedOn, t.TicketID,t.Title,t.Description,t.FinalTime  from Tickets t ");
            strTicketDataSource.Append("left join Projects p ");
            strTicketDataSource.Append("on t.ProjectID = p.ProjectID ");
            strTicketDataSource.Append("where t.IsEstimates=1 and t.Status=19 ");

            //Timesheet报表
            StringBuilder strDataSource = new StringBuilder();
            strDataSource.Append(@"SELECT ts.ID as TimeSheetID, ts.SheetDate, ts.ProjectID, ts.TicketID,
                            ts.UserID, ts.Hours, ts.Percentage, ts.Description AS WorkDetail, ts.IsSubmitted, 
                            ts.CreatedOn, ts.CreatedBy, ts.ModifiedOn, ts.ModifiedBy, ts.IsMeeting,
                            p.Title AS ProjectTitle,
                            t.Title AS TicketTitle,t.Description as TicketDescription,(t.[TicketCode]+Cast(t.[TicketID] as varchar)) as TicketCode, 
                            r.RoleName as RoleName,
                            u.FirstName ,u.LastName,
                            t.Accounting
                            FROM TimeSheets ts LEFT OUTER JOIN 
                            Projects  p ON ts.ProjectID = p.ProjectID LEFT OUTER JOIN 
                            Tickets  t ON ts.TicketID = t.TicketID LEFT OUTER JOIN 
                            Users u ON ts.UserID = u.UserID LEFT OUTER JOIN 
                            Roles r On u.RoleID = r.RoleID ");
            strDataSource.Append(" WHERE 1=1 ");
            switch (request.SearchTimeSheetsType)
            {
                case SearchType.QueryReport:
                    if (request.UserID > 0)
                        strDataSource.Append(" And ts.UserID=@UserID ");
                    if (request.ProjectID > 0)
                    {
                        strDataSource.Append(" And ts.ProjectID=@ProjectID");
                        strTicketDataSource.Append(" And  t.ProjectID=@ProjectID");
                    }
                    if (request.CompanyID > 0)
                    {
                        strDataSource.Append(" And t.CompanyID=@CompanyID");
                    }
                    if (request.Accounting > 0)
                    {
                        strDataSource.Append(" And t.Accounting=@Accounting");
                    }
                    if (request.TicketID > 0)
                    {
                        strDataSource.Append(" And ts.TicketID=@TicketID");
                    }
                    if (request.StartDate != null && request.StartDate > DateTime.MinValue)
                    {
                        strDataSource.Append(" And ts.SheetDate >= @StartDate");
                        strTicketDataSource.Append(" And  convert(char(20),t.ModifiedOn,102) >= @StartDate");
                    }
                    if (request.EndDate != null && request.EndDate > DateTime.MinValue)
                    {
                        strDataSource.Append(" And ts.SheetDate <= @EndDate");
                        strTicketDataSource.Append(" And  convert(char(20),t.ModifiedOn,102) <= @EndDate");
                    }
                    if (!string.IsNullOrEmpty(request.Keywords))
                    {
                        strDataSource.Append(SplitKeywords(request.Keywords, "p.Title", "t.Title", "t.TicketID"));
                    }

                    if (request.WID > 0)
                        strDataSource.Append(" And ts.TicketID in (select tid from ProposalTrackerRelation where WID = @WID)");
                    break;
                case SearchType.EmailNotice:
                    strDataSource.Append(" AND u.Office=@Office ");
                    strDataSource.Append(" And ts.SheetDate = @SheetDate ");
                    break;
                default: break;
            }
            if (!request.IsPageModel)
                strDataSource.AppendFormat(" Order by {0} {1} ",
                    orderby, request.OrderDirection);

            StringBuilder strSql = new StringBuilder();
            if (request.IsPageModel)
            {
                strSql.Append(" Select count(1) from ");
                strSql.Append("(");
                strSql.Append(strDataSource);
                strSql.Append(") as SearchResult ;");
                strSql.Append(" Select * from (");
                strSql.AppendFormat("  Select ROW_NUMBER() OVER( Order BY {0} {1}) as  INDEX_ID,SearchResult.* From "
                    , request.OrderExpression, request.OrderDirection);
                strSql.Append("(");
                strSql.Append(strDataSource);
                strSql.Append(") as SearchResult  ");
                strSql.Append(") as NEW_TB ");
                strSql.Append(" Where Index_ID between @Start and @End ");
            }
            else
            {
                strSql.Append(strDataSource);
            }

            SearchTimeSheetsResponse response = new SearchTimeSheetsResponse();
            List<TimeSheetTicket> list;
            List<TicketsEntity> list_tickets;
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    int start = request.CurrentPage * request.PageCount + 1 - request.PageCount;
                    int end = request.CurrentPage * request.PageCount;

                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, request.UserID);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, request.ProjectID);
                    db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, request.CompanyID);
                    db.AddInParameter(dbCommand, "Accounting", DbType.Int32, request.Accounting);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, request.TicketID);
                    db.AddInParameter(dbCommand, "WID", DbType.Int32, request.WID);
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, request.StartDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, request.EndDate);
                    db.AddInParameter(dbCommand, "Keyword", DbType.String, string.Format("%{0}%", request.Keywords.FilterSqlString()));
                    db.AddInParameter(dbCommand, "Start", DbType.Int32, start);
                    db.AddInParameter(dbCommand, "End", DbType.Int32, end);

                    db.AddInParameter(dbCommand, "Office", DbType.String, string.IsNullOrEmpty(request.Office) ? "CN" : request.Office.ToUpper());
                    db.AddInParameter(dbCommand, "SheetDate", DbType.DateTime, request.SearchDate.Date);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        list = new List<TimeSheetTicket>();
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
                            list.Add(TimeSheetTicket.ReaderBindForReport(dataReader));
                        }
                        response.TimeSheetsList = list;
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                         strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    response.IsError = true;
                }
            }
            //查找Ticket
            using (DbCommand dbCommand = db.GetSqlStringCommand(strTicketDataSource.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, request.ProjectID);
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, request.StartDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, request.EndDate);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        list_tickets = new List<TicketsEntity>();
                        while (dataReader.Read())
                        {
                            list_tickets.Add(TicketsEntity.ReaderBind(dataReader));
                        }
                        response.TicketProjectsList = list_tickets;
                        response.IsError = false;
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                         strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    response.IsError = true;
                }
            }
            return response;
        }



        public DataTable QueryReportTotalHoursByProject(int projectID, int userID, DateTime startDate, DateTime endDate, string orderBy, string orderDirectioin)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT p.ProjectID, Title,Sum(Hours) as [Hours]
                                FROM  [Projects] p left join dbo.TimeSheets ts on p.ProjectID =ts.ProjectID");
            strSql.Append(@"    Where  1=1 ");
            if (projectID > 0)
            {
                strSql.Append(" AND ts.ProjectID=@ProjectID ");
            }
            if (userID > 0)
            {
                strSql.Append(" AND ts.UserID=@UserID ");
            }
            if (startDate != null && startDate > DateTime.MinValue)
            {
                strSql.Append(" AND ts.SheetDate >=@StartDate ");
            }
            if (endDate != null && endDate > DateTime.MinValue)
            {
                strSql.Append(" AND ts.SheetDate <=@EndDate ");
            }
            strSql.Append(@"    Group by p.ProjectID,Title");
            if (orderBy.ToLower() == "All".ToLower())
            {
                strSql.AppendFormat(@"    Order by p.Title {0}", orderDirectioin);
            }
            else
            {
                if (orderBy == "ProjectTitle")
                {
                    orderBy = "p.Title";
                }
                strSql.AppendFormat(@"    Order by {0} {1}", orderBy, orderDirectioin);
            }
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, projectID);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, userID);
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, startDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, endDate);

                    DataTable dt = new DataTable();
                    dt = db.ExecuteDataSet(dbCommand).Tables[0];
                    return dt;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                         strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }

            }
        }

        public DataTable QueryReportDetailsByProject(int projectID, int userID, DateTime startDate, DateTime endDate, string orderBy, string orderDirectioin)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"Select ProjDetail.*,p.Title,u.FirstName,u.LastName from 
                            (
                            SELECT ts.ProjectID,ts.UserID, Sum(Hours) as[Hours]
                            FROM  dbo.TimeSheets ts  ");
            strSql.Append(@"    Where  1=1 ");
            if (projectID > 0)
            {
                strSql.Append(" AND ts.ProjectID=@ProjectID ");
            }
            if (userID > 0)
            {
                strSql.Append(" AND ts.UserID=@UserID ");
            }
            if (startDate != null && startDate > DateTime.MinValue)
            {
                strSql.Append(" AND ts.SheetDate >=@StartDate ");
            }
            if (endDate != null && endDate > DateTime.MinValue)
            {
                strSql.Append(" AND ts.SheetDate <=@EndDate ");
            }
            strSql.Append(@"   Group by  ts.ProjectID ,ts.UserID ");
            strSql.Append(@" ) as ProjDetail  join Users u on ProjDetail.UserID=u.UserID
                                 join Projects p on ProjDetail.ProjectID=p.ProjectID ");
            string orderby = string.Empty;
            if (orderBy.ToLower() == "All".ToLower())
            {
                orderby = " p.Title,FirstName asc,LastName asc ";
            }
            else
            {
                if (orderBy.ToLower() == "ProjectTitle".ToLower())
                {
                    orderBy = "p.Title";
                }
                orderby = string.Format(" {0} {1} ", orderBy, orderDirectioin);
            }
            strSql.AppendFormat(@"    Order by {0}", orderby);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, projectID);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, userID);
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, startDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, endDate);

                    DataTable dt = new DataTable();
                    dt = db.ExecuteDataSet(dbCommand).Tables[0];
                    return dt;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                         strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }

            }
        }

        public DataTable GetSheetDateByProjectUser(int projectID, int userID, DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder("");
            strSql.Append("Select distinct SheetDate ");
            strSql.Append("from dbo.TimeSheets ");
            strSql.Append("WHERE 1=1 ");
            if (userID > 0)
            {
                strSql.Append(" AND UserID=@UserID ");
            }
            if (projectID > 0)
            {
                strSql.Append(" AND ProjectID=@ProjectID ");
            }
            if (startDate != null && startDate > DateTime.MinValue)
            {
                strSql.Append(" AND SheetDate >=@StartDate");
            }
            if (endDate != null && endDate > DateTime.MinValue)
            {
                strSql.Append(" AND SheetDate <=@EndDate");
            }
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, startDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, endDate);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, userID);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, projectID);
                    DataSet ds = db.ExecuteDataSet(dbCommand);
                    return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                          strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        #endregion

        #region 报表功能

        public DataTable ReportConsumingComparison(SearchTimeSheetsRequest request, out int totalRows)
        {
            totalRows = 0;
            string orderby = string.Empty;
            if (request.OrderExpression.ToLower() == "projecttitle")
            {
                orderby = "p.Title";
            }
            else if (request.OrderExpression.ToLower() == "tickettitle")
            {
                orderby = "t.Title";
            }
            else if (request.OrderExpression.ToLower() == "firstname")
                orderby = "u.FirstName";
            else
            {
                if (request.OrderExpression != "ticketcode, sheetdate")
                    orderby = "ts." + request.OrderExpression;
                else
                    orderby = request.OrderExpression;
            }

            StringBuilder strDataSource = new StringBuilder();
            strDataSource.Append(@"SELECT    ts.ProjectID, ts.TicketID,SUM(ts.Hours) AS totalHours,
			                        ISNULL(t.FinalTime,0) as Estimations,
		                          p.Title AS ProjectTitle,  t.Title AS TicketTitle,
		                         (t.[TicketCode]+Cast(t.[TicketID] as varchar)) as TicketCode 
                                    FROM TimeSheets ts   JOIN 
                                                    Projects  p ON ts.ProjectID = p.ProjectID   JOIN 
                                                    Tickets  t ON ts.TicketID = t.TicketID   JOIN 
                                                    Users u ON ts.UserID = u.UserID   JOIN 
                                                    Roles r On u.RoleID = r.RoleID ");
            strDataSource.Append(" WHERE 1=1 ");
            switch (request.SearchTimeSheetsType)
            {
                case SearchType.QueryReport:
                    if (request.UserID > 0)
                        strDataSource.Append(" And ts.UserID=@UserID ");
                    if (request.ProjectID > 0)
                        strDataSource.Append(" And ts.ProjectID=@ProjectID");
                    if (request.TicketID > 0)
                        strDataSource.Append(" And ts.TicketID=@TicketID");
                    if (request.StartDate != null && request.StartDate > DateTime.MinValue)
                        strDataSource.Append(" And ts.SheetDate >= @StartDate");
                    if (request.EndDate != null && request.EndDate > DateTime.MinValue)
                        strDataSource.Append(" And ts.SheetDate <= @EndDate");

                    if (request.Keywords != null && request.Keywords.Trim() != string.Empty)
                    {
                        request.Keywords = request.Keywords.Trim();
                        string[] fields = { "p.Title", "t.Title" };
                        strDataSource.Append(SplitKeywords(request.Keywords, "t.TicketID", fields));
                    }

                    break;
                case SearchType.EmailNotice:
                    strDataSource.Append(" AND u.Office=@Office ");
                    strDataSource.Append(" And ts.SheetDate = @SheetDate ");
                    //save is effictive    
                    //strDataSource.Append(" And ts.IsSubmitted = 1 ");
                    break;
                default: break;
            }
            strDataSource.Append(@"   
			                        GROUP BY   ts.ProjectID, ts.TicketID,t.FinalTime,
		                          p.Title ,  t.Title ,
	                        (t.[TicketCode]+Cast(t.[TicketID] as varchar)) ");

            if (!request.IsPageModel)
                strDataSource.AppendFormat(" Order by {0} {1} ",
                    orderby, request.OrderDirection);

            StringBuilder strSql = new StringBuilder();
            if (request.IsPageModel)
            {
                strSql.Append(" Select count(1) from ");
                strSql.Append("(");
                strSql.Append(strDataSource);
                strSql.Append(") as SearchResult ;");
                strSql.Append(" Select * from (");
                strSql.AppendFormat("  Select ROW_NUMBER() OVER( Order BY {0} {1}) as  INDEX_ID,SearchResult.* From "
                    , request.OrderExpression, request.OrderDirection);
                strSql.Append("(");
                strSql.Append(strDataSource);
                strSql.Append(") as SearchResult  ");
                strSql.Append(") as NEW_TB ");
                strSql.Append(" Where Index_ID between @Start and @End ");
            }
            else
            {
                strSql.Append(strDataSource);
            }
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    int start = request.CurrentPage * request.PageCount + 1 - request.PageCount;
                    int end = request.CurrentPage * request.PageCount;

                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, request.UserID);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, request.ProjectID);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, request.TicketID);
                    db.AddInParameter(dbCommand, "WID", DbType.Int32, request.WID);
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, request.StartDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, request.EndDate);
                    db.AddInParameter(dbCommand, "Keyword", DbType.String, string.Format("%{0}%", request.Keywords.FilterSqlString()));
                    db.AddInParameter(dbCommand, "Start", DbType.Int32, start);
                    db.AddInParameter(dbCommand, "End", DbType.Int32, end);

                    db.AddInParameter(dbCommand, "Office", DbType.String, string.IsNullOrEmpty(request.Office) ? "CN" : request.Office.ToUpper());
                    db.AddInParameter(dbCommand, "SheetDate", DbType.DateTime, request.SearchDate.Date);

                    DataSet ds = db.ExecuteDataSet(dbCommand);

                    int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out totalRows);
                    return ds.Tables[1];

                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                         strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return new DataTable();
                }
            }
        }
        public DataTable ReportComparisonAnalysis(SearchTimeSheetsRequest request, out int totalRows)
        {
            totalRows = 0;
            string orderby = string.Empty;
            if (request.OrderExpression.ToLower() == "projecttitle")
            {
                orderby = "p.Title";
            }
            else if (request.OrderExpression.ToLower() == "tickettitle")
            {
                orderby = "t.Title";
            }
            else if (request.OrderExpression.ToLower() == "firstname")
                orderby = "u.FirstName";
            else
            {
                if (request.OrderExpression != "ticketId")
                    orderby = "t." + request.OrderExpression;
                else
                    orderby = request.OrderExpression;
            }

            StringBuilder strDataSource = new StringBuilder();
            strDataSource.Append(@"
                                    SELECT projectId,projectTitle,Source,Bug,Change,Issue,Request,Risk FROM 
                                        ( SELECT t.projectId,p.Title as ProjectTitle,Source,TicketType,TicketId FROM Tickets t  
						            	 JOIN Projects p ON t.ProjectID = p.ProjectID  
                                        ");
            strDataSource.Append(" WHERE 1=1 ");
            switch (request.SearchTimeSheetsType)
            {
                case SearchType.QueryReport:

                    if (request.ProjectID > 0)
                        strDataSource.Append(" And t.ProjectID=@ProjectID");

                    if (request.StartDate != null && request.StartDate > DateTime.MinValue)
                        strDataSource.Append(" And t.StartDate >= @StartDate");
                    if (request.EndDate != null && request.EndDate > DateTime.MinValue)
                        strDataSource.Append(" And t.StartDate <= @EndDate");
                    if (!string.IsNullOrEmpty(request.Keywords))
                    {
                        request.Keywords = request.Keywords.Trim();
                        string[] fields = { "p.Title", "t.Title" };
                        strDataSource.Append(SplitKeywords(request.Keywords, "t.TicketID", fields));
                    }
                    //     strDataSource.Append(" And (p.Title like @Keyword or t.Title like @Keyword) ");
                    if (request.Source > 0)
                        strDataSource.Append(" And t.Source=@Source ");

                    break;
                default: break;
            }
            strDataSource.Append(@" ) AS Tickets 
                                pivot (  COUNT(TicketId) FOR TicketType IN(Bug,Change,Issue,Request,Risk)) AS ourPivot ");

            if (!request.IsPageModel)
                strDataSource.AppendFormat(" Order by {0} {1} ",
                    orderby, request.OrderDirection);

            StringBuilder strSql = new StringBuilder();
            if (request.IsPageModel)
            {
                strSql.Append(" Select count(1) from ");
                strSql.Append("(");
                strSql.Append(strDataSource);
                strSql.Append(") as SearchResult ;");
                strSql.Append(" Select * from (");
                strSql.AppendFormat("  Select ROW_NUMBER() OVER( Order BY {0} {1}) as  INDEX_ID,SearchResult.* From "
                    , request.OrderExpression, request.OrderDirection);
                strSql.Append("(");
                strSql.Append(strDataSource);
                strSql.Append(") as SearchResult  ");
                strSql.Append(") as NEW_TB ");
                strSql.Append(" Where Index_ID between @Start and @End ");
            }
            else
            {
                strSql.Append(strDataSource);
            }
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    int start = request.CurrentPage * request.PageCount + 1 - request.PageCount;
                    int end = request.CurrentPage * request.PageCount;

                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, request.UserID);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, request.ProjectID);


                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, request.StartDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, request.EndDate);
                    db.AddInParameter(dbCommand, "Keyword", DbType.String, string.Format("%{0}%", request.Keywords.FilterSqlString()));
                    db.AddInParameter(dbCommand, "Start", DbType.Int32, start);
                    db.AddInParameter(dbCommand, "End", DbType.Int32, end);
                    db.AddInParameter(dbCommand, "Source", DbType.Int32, request.Source);


                    DataSet ds = db.ExecuteDataSet(dbCommand);

                    int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out totalRows);
                    return ds.Tables[1];

                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                         strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return new DataTable();
                }
            }
        }

        public List<TimeSheetTicket> ComparisonExport(SearchTimeSheetsRequest request)
        {

            string orderby = string.Empty;
            if (request.OrderExpression.ToLower() == "projecttitle")
            {
                orderby = "p.Title";
            }
            else if (request.OrderExpression.ToLower() == "tickettitle")
            {
                orderby = "t.Title";
            }
            else if (request.OrderExpression.ToLower() == "firstname")
                orderby = "u.FirstName";
            else
            {
                if (request.OrderExpression != "ticketcode, sheetdate")
                    orderby = "ts." + request.OrderExpression;
                else
                    orderby = request.OrderExpression;
            }

            StringBuilder strDataSource = new StringBuilder();
            strDataSource.Append(@" SELECT  ts.ProjectID, ts.TicketID,  p.Title AS ProjectTitle,t.Title AS TicketTitle,t.FinalTime,
                                             t.Description as TicketDescription,ts.UserID,
                                             t.[TicketCode], t.[TicketID],r.RoleName as RoleName,u.FirstName,
                                             u.LastName,SUM(ts.Hours) as Hours
                                       FROM TimeSheets ts LEFT OUTER JOIN Projects  p ON ts.ProjectID = p.ProjectID 
				                       LEFT OUTER JOIN Tickets  t ON ts.TicketID = t.TicketID
				                       LEFT OUTER JOIN Users u ON ts.UserID = u.UserID 
				                       LEFT OUTER JOIN Roles r ON u.RoleID = r.RoleID ");
            strDataSource.Append(" WHERE 1=1 ");
            if (request.UserID > 0)
                strDataSource.Append(" And ts.UserID=@UserID ");
            if (request.ProjectID > 0)
                strDataSource.Append(" And ts.ProjectID=@ProjectID");
            if (request.TicketID > 0)
                strDataSource.Append(" And ts.TicketID=@TicketID");
            if (request.StartDate != null && request.StartDate > DateTime.MinValue)
                strDataSource.Append(" And ts.SheetDate >= @StartDate");
            if (request.EndDate != null && request.EndDate > DateTime.MinValue)
                strDataSource.Append(" And ts.SheetDate <= @EndDate");
            if (request.TicketType != null && request.TicketType.Trim() != string.Empty)
                strDataSource.Append(" And t.TicketType = @TicketType");

            if (request.Keywords != null && request.Keywords.Trim() != string.Empty)
            {
                request.Keywords = request.Keywords.Trim();
                string[] fields = { "p.Title", "t.Title" };
                strDataSource.Append(SplitKeywords(request.Keywords, "t.TicketID", fields));
            }
            strDataSource.Append(@"  GROUP BY ts.ProjectID, ts.TicketID,  p.Title,t.Title,t.FinalTime ,t.Description,t.[TicketCode],t.[TicketID], r.RoleName,u.FirstName ,u.LastName,ts.UserID ");
            strDataSource.AppendFormat(" Order by {0} {1} ", orderby, request.OrderDirection);
            Database db = DatabaseFactory.CreateDatabase();
            List<TimeSheetTicket> list = new List<TimeSheetTicket>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strDataSource.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, request.UserID);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, request.ProjectID);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, request.TicketID);
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, request.StartDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, request.EndDate);
                    db.AddInParameter(dbCommand, "Keyword", DbType.String, string.Format("%{0}%", request.Keywords.FilterSqlString()));
                    db.AddInParameter(dbCommand, "TicketType", DbType.String, request.TicketType);

                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        while (dataReader.Read())
                        {
                            list.Add(TimeSheetTicket.ReaderBindForReport(dataReader, true));
                        }
                    return list;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                         strDataSource.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return list;
                }
            }
        }


        #endregion


        public List<CheckTimesheetEntity> GetTimesheetList(DateTime startDate, DateTime endDate)
        {
            List<CheckTimesheetEntity> list = new List<CheckTimesheetEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetStoredProcCommand("CheckTimesheet"))
            {
                try
                {
                    db.AddInParameter(dbCommand, "BeginDate", DbType.DateTime, startDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, endDate);
                    IDataReader reader = db.ExecuteReader(dbCommand);
                    while (reader.Read())
                    {
                        list.Add(CheckTimesheetEntity.ReaderBind(reader));
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                          "GetTimesheetList", base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }
        public List<TimeSheetTicket> GetTimesheet(int invoiceId)
        {
            List<TimeSheetTicket> list = new List<TimeSheetTicket>();
            StringBuilder strSql = new StringBuilder("");
            strSql.Append(@"SELECT ts.ID as TimeSheetID, ts.SheetDate, ts.ProjectID, ts.TicketID,
                            ts.UserID, ts.Hours, ts.Percentage, ts.Description AS WorkDetail, ts.IsSubmitted, 
                            ts.CreatedOn, ts.CreatedBy, ts.ModifiedOn, ts.ModifiedBy, ts.IsMeeting,
                            p.Title AS ProjectTitle,
                            t.Title AS TicketTitle,t.Description as TicketDescription,(t.[TicketCode]+Cast(t.[TicketID] as varchar)) as TicketCode, 
                            r.RoleName as RoleName,
                            u.FirstName ,u.LastName,
                            t.Accounting
                            FROM TimeSheets ts LEFT OUTER JOIN 
                            Projects  p ON ts.ProjectID = p.ProjectID LEFT OUTER JOIN 
                            Tickets  t ON ts.TicketID = t.TicketID LEFT OUTER JOIN 
                            Users u ON ts.UserID = u.UserID LEFT OUTER JOIN 
                            Roles r On u.RoleID = r.RoleID ");
            strSql.Append("where ts.ID in (select TSId from [TSInvoiceRelation] where [InvoiceId]=@invoiceId)");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "invoiceId", DbType.Int32, invoiceId);
                    IDataReader reader = db.ExecuteReader(dbCommand);
                    while (reader.Read())
                    {
                        list.Add(TimeSheetTicket.ReaderBindForReport(reader));
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                          "GetTimesheet", base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        public List<TimeSheetTicket> GetTimesheetByProposalId(int proposalTrackerId)
        {
            List<TimeSheetTicket> list = new List<TimeSheetTicket>();
            StringBuilder strSql = new StringBuilder("");
            strSql.Append(@"SELECT ts.ID as TimeSheetID, ts.SheetDate, ts.ProjectID, ts.TicketID,
                            ts.UserID, ts.Hours, ts.Percentage, ts.Description AS WorkDetail, ts.IsSubmitted, 
                            ts.CreatedOn, ts.CreatedBy, ts.ModifiedOn, ts.ModifiedBy, ts.IsMeeting,
                            p.Title AS ProjectTitle,
                            t.Title AS TicketTitle,t.Description as TicketDescription,(t.[TicketCode]+Cast(t.[TicketID] as varchar)) as TicketCode, 
                            r.RoleName as RoleName,
                            u.FirstName ,u.LastName,
                            t.Accounting
                            FROM TimeSheets ts LEFT OUTER JOIN 
                            Projects  p ON ts.ProjectID = p.ProjectID LEFT OUTER JOIN 
                            Tickets  t ON ts.TicketID = t.TicketID LEFT OUTER JOIN 
                            Users u ON ts.UserID = u.UserID LEFT OUTER JOIN 
                            Roles r On u.RoleID = r.RoleID ");
            strSql.Append("where ts.TicketID in (select TID from ProposalTrackerRelation where WID=@ProposalTrackerId )");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProposalTrackerId", DbType.Int32, proposalTrackerId);
                    IDataReader reader = db.ExecuteReader(dbCommand);
                    while (reader.Read())
                    {
                        list.Add(TimeSheetTicket.ReaderBindForReport(reader));
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                          "GetTimesheet", base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }
    }
}

