using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Core.UserModule;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Entity.TicketModel.UserTicket;
using System.Linq;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel.UserModel;

namespace SunNet.PMNew.Impl.SqlDataProvider.User
{
    /// <summary>
    /// Data access:UsersRepository
    /// </summary>
    public class UsersRepositoryRepositorySqlDataProvider : SqlHelper, IUsersRepository
    {
        public UsersRepositoryRepositorySqlDataProvider()
        { }
        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(UsersEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Users(");
            strSql.Append("CompanyID,RoleID,FirstName,LastName,UserName,Email,PassWord,Title,Phone,EmergencyContactFirstName,EmergencyContactLastName")
            .Append(",EmergencyContactPhone,EmergencyContactEmail,MaintenancePlanOption")
            .Append(",CreatedOn,AccountStatus,ForgotPassword,IsDelete,Status,UserType,Skype,Office,PTOHoursOfYear)");

            strSql.Append(" values (");
            strSql.Append("@CompanyID,@RoleID,@FirstName,@LastName,@UserName,@Email,@PassWord,@Title,@Phone,@EmergencyContactFirstName,@EmergencyContactLastName")
            .Append(",@EmergencyContactPhone,@EmergencyContactEmail,@MaintenancePlanOption")
            .Append(",@CreatedOn,@AccountStatus,@ForgotPassword,@IsDelete,@Status,@UserType,@Skype,@Office,@PTOHoursOfYear)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, model.CompanyID);
                    db.AddInParameter(dbCommand, "RoleID", DbType.Int32, model.RoleID);
                    db.AddInParameter(dbCommand, "FirstName", DbType.String, model.FirstName);
                    db.AddInParameter(dbCommand, "LastName", DbType.String, model.LastName);
                    db.AddInParameter(dbCommand, "UserName", DbType.String, model.UserName);
                    db.AddInParameter(dbCommand, "Email", DbType.String, model.Email);
                    db.AddInParameter(dbCommand, "PassWord", DbType.String, model.PassWord);
                    db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                    db.AddInParameter(dbCommand, "Phone", DbType.String, model.Phone);
                    db.AddInParameter(dbCommand, "EmergencyContactFirstName", DbType.String, model.EmergencyContactFirstName);
                    db.AddInParameter(dbCommand, "EmergencyContactLastName", DbType.String, model.EmergencyContactLastName);
                    db.AddInParameter(dbCommand, "EmergencyContactPhone", DbType.String, model.EmergencyContactPhone);
                    db.AddInParameter(dbCommand, "EmergencyContactEmail", DbType.String, model.EmergencyContactEmail);
                    db.AddInParameter(dbCommand, "MaintenancePlanOption", DbType.String, model.MaintenancePlanOption);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "AccountStatus", DbType.Int32, model.AccountStatus);
                    db.AddInParameter(dbCommand, "ForgotPassword", DbType.Int32, model.ForgotPassword);
                    db.AddInParameter(dbCommand, "IsDelete", DbType.Boolean, model.IsDelete);
                    db.AddInParameter(dbCommand, "Status", DbType.String, model.Status);
                    db.AddInParameter(dbCommand, "UserType", DbType.String, model.UserType);
                    db.AddInParameter(dbCommand, "Skype", DbType.String, model.Skype);
                    db.AddInParameter(dbCommand, "Office", DbType.String, model.Office);
                    db.AddInParameter(dbCommand, "PTOHoursOfYear", DbType.Double, model.PTOHoursOfYear);

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
        public bool Update(UsersEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Users set ");
            strSql.Append("CompanyID=@CompanyID,");
            strSql.Append("RoleID=@RoleID,");
            strSql.Append("FirstName=@FirstName,");
            strSql.Append("LastName=@LastName,");
            strSql.Append("UserName=@UserName,");
            strSql.Append("Email=@Email,");
            strSql.Append("PassWord=@PassWord,");
            strSql.Append("Title=@Title,");
            strSql.Append("Phone=@Phone,");
            strSql.Append("EmergencyContactFirstName=@EmergencyContactFirstName,");
            strSql.Append("EmergencyContactLastName=@EmergencyContactLastName,");
            strSql.Append("EmergencyContactPhone=@EmergencyContactPhone,");
            strSql.Append("EmergencyContactEmail=@EmergencyContactEmail,");
            strSql.Append("MaintenancePlanOption=@MaintenancePlanOption,");
            strSql.Append("CreatedOn=@CreatedOn,");
            strSql.Append("AccountStatus=@AccountStatus,");
            strSql.Append("ForgotPassword=@ForgotPassword,");
            strSql.Append("IsDelete=@IsDelete,");
            strSql.Append("Status=@Status,");
            strSql.Append("UserType=@UserType,");
            strSql.Append("Skype=@Skype,");
            strSql.Append("Office=@Office,");
            strSql.Append("PTOHoursOfYear=@PTOHoursOfYear,");
            strSql.Append("[IsNotice]=@IsNotice");
            strSql.Append(" where UserID=@UserID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
                    db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, model.CompanyID);
                    db.AddInParameter(dbCommand, "RoleID", DbType.Int32, model.RoleID);
                    db.AddInParameter(dbCommand, "FirstName", DbType.String, model.FirstName);
                    db.AddInParameter(dbCommand, "LastName", DbType.String, model.LastName);
                    db.AddInParameter(dbCommand, "UserName", DbType.String, model.UserName);
                    db.AddInParameter(dbCommand, "Email", DbType.String, model.Email);
                    db.AddInParameter(dbCommand, "PassWord", DbType.String, model.PassWord);
                    db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                    db.AddInParameter(dbCommand, "Phone", DbType.String, model.Phone);
                    db.AddInParameter(dbCommand, "EmergencyContactFirstName", DbType.String, model.EmergencyContactFirstName);
                    db.AddInParameter(dbCommand, "EmergencyContactLastName", DbType.String, model.EmergencyContactLastName);
                    db.AddInParameter(dbCommand, "EmergencyContactPhone", DbType.String, model.EmergencyContactPhone);
                    db.AddInParameter(dbCommand, "EmergencyContactEmail", DbType.String, model.EmergencyContactEmail);
                    db.AddInParameter(dbCommand, "MaintenancePlanOption", DbType.String, model.MaintenancePlanOption);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "AccountStatus", DbType.Int32, model.AccountStatus);
                    db.AddInParameter(dbCommand, "ForgotPassword", DbType.Int32, model.ForgotPassword);
                    db.AddInParameter(dbCommand, "IsDelete", DbType.Boolean, model.IsDelete);
                    db.AddInParameter(dbCommand, "Status", DbType.String, model.Status);
                    db.AddInParameter(dbCommand, "UserType", DbType.String, model.UserType);
                    db.AddInParameter(dbCommand, "Skype", DbType.String, model.Skype);
                    db.AddInParameter(dbCommand, "Office", DbType.String, model.Office);
                    db.AddInParameter(dbCommand, "PTOHoursOfYear", DbType.Double, model.PTOHoursOfYear);
                    db.AddInParameter(dbCommand, "IsNotice", DbType.Boolean, model.IsNotice);
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
        public bool Delete(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Users set ");
            strSql.Append("IsDelete=@IsDelete");
            strSql.Append(" where UserID=@UserID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "IsDelete", DbType.Boolean, true);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
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
        public UsersEntity Get(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select u.*,c.CompanyName as CompanyName  from Users u left join Companys c on u.CompanyID=c.ComID ");
            strSql.Append(" where UserID=@UserID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
                    UsersEntity model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = UsersEntity.ReaderBind(dataReader);
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

        #region IUsersRepository Members

        public UsersEntity GetUserByUserName(string username)
        {
            string strSql = @"SELECT  u.*,c.CompanyName
                              FROM [Users] u left join Companys c on u.CompanyID=c.ComID  
                              WHERE UserName =@UserName AND [IsDelete]=0";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "UserName", DbType.String, username.FilterSqlString());
                UsersEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        if (dataReader.Read())
                        {
                            model = UsersEntity.ReaderBind(dataReader);
                        }
                    }
                    catch (Exception ex)
                    {
                        WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                            strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                        return null;
                    }
                }
                return model;
            }
        }

        public SearchUserResponse SearchUsers(SearchUsersRequest request)
        {
            int start = request.CurrentPage * request.PageCount + 1 - request.PageCount;
            int end = request.CurrentPage * request.PageCount;

            string strSelCount = " SELECT  COUNT(1)  FROM [Users] u ";
            if (request.OrderExpression.ToLower().Equals("status"))
                request.OrderExpression = " u.Status ";
            string strOrderby = string.Format(" {0} {1} ", request.OrderExpression, request.OrderDirection);
            string strSelAttrs = " SELECT u.*,c.CompanyName  FROM  [Users]  u left join Companys c on u.CompanyID=c.ComID  ";
            string strSelAttrsOrderBy = string.Format(@" Order BY {0}  ", strOrderby);
            string strSelPageModel = string.Format(@"SELECT * FROM(
                                                SELECT ROW_NUMBER() OVER(
                                                Order BY {0}) as  INDEX_ID,u.*,c.CompanyName  FROM [Users]  u left join Companys c on u.CompanyID=c.ComID  ", strOrderby);
            string strWherePageModel = @") NEW_TB  WHERE INDEX_ID BETWEEN @Strat AND  @End;";
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" WHERE IsDelete=0 ");
            switch (request.SearchType)
            {
                case SearchUsersType.All:
                    if (request.IsSunnet)
                    {
                        strWhere.Append(" AND [UserType] = 'SUNNET'");
                    }
                    if (request.IsClient)
                    {
                        strWhere.Append(" AND [UserType] = 'CLIENT'");
                    }
                    //if (!string.IsNullOrEmpty(request.Status) && request.Status.ToUpper() != "ALL")
                    //{
                    //    strWhere.Append(" AND u.Status=@Status ");
                    //}
                    strWhere.Append(" AND u.Status='ACTIVE'");
                    break;
                case SearchUsersType.List:
                    strWhere.Append(" AND ( UserName like @Keywords OR FirstName  like @Keywords  OR LastName  like @Keywords )");
                    if (request.CompanyID != 0)
                    {
                        strWhere.AppendFormat(" AND (CompanyID={0})", request.CompanyID);
                    }
                    if (!string.IsNullOrEmpty(request.Status) && request.Status.ToUpper() != "All".ToUpper())
                    {
                        strWhere.Append(" AND u.Status=@Status ");
                    }
                    break;
                case SearchUsersType.Company:
                    strWhere.Append(" AND u.Status='ACTIVE'");
                    strWhere.Append(" AND u.CompanyID =@CompanyID");
                    break;
                case SearchUsersType.CompanyByProject:
                    strWhere.Append(" AND u.Status='ACTIVE'");
                    strWhere.Append(" AND u.CompanyID IN (SELECT CompanyID FROM Projects  WHERE ProjectID = @ProjectID )");
                    break;
                case SearchUsersType.Project:
                    strWhere.Append(" AND u.Status='ACTIVE'");
                    strWhere.Append(" AND u.UserID IN (SELECT UserID FROM ProjectUsers WHERE ProjectID = @ProjectID)");
                    break;
                case SearchUsersType.Role:
                    strWhere.Append(" AND u.RoleID =@RoleID ");
                    strWhere.Append(" AND u.Status='ACTIVE'");
                    break;
                case SearchUsersType.Ticket:
                    strWhere.Append(" AND u.UserID in ( SELECT  [UserID] FROM  [TicketUsers] WHERE [TicketID]=@TicketID)");
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

            SearchUserResponse response = new SearchUserResponse();
            List<UsersEntity> list;
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, request.UserID);
                    db.AddInParameter(dbCommand, "Keywords", DbType.String, string.Format("%{0}%", request.Keywords.FilterSqlString()));
                    db.AddInParameter(dbCommand, "Status", DbType.String, request.Status);
                    db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, request.CompanyID);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, request.ProjectID);
                    db.AddInParameter(dbCommand, "RoleID", DbType.Int32, (int)request.Role);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, request.TicketID);
                    db.AddInParameter(dbCommand, "Strat", DbType.Int32, start);
                    db.AddInParameter(dbCommand, "End", DbType.Int32, end);

                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        list = new List<UsersEntity>();
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
                            list.Add(UsersEntity.ReaderBind(dataReader));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roles">查询的用户角色</param>
        /// <param name="userIds">要排除的用户Id</param>
        /// <returns></returns>
        public List<UserTicketModel> SearchUserWithRole(List<RolesEnum> roles, string hideUserIds)
        {
            string strUserSql = "";
            StringBuilder strUserSelect = new StringBuilder();
            strUserSelect.Append("SELECT ");
            strUserSelect.Append("U.UserID, ");
            strUserSelect.Append("U.FirstName, ");
            strUserSelect.Append("U.LastName, ");
            strUserSelect.Append("R.RoleID, ");
            strUserSelect.Append("R.RoleName ");
            strUserSelect.Append("FROM dbo.Users AS U ");
            strUserSelect.Append("LEFT JOIN dbo.Roles AS R ON U.RoleID = R.RoleID ");

            StringBuilder strWhere = new StringBuilder();
            strWhere.Append("WHERE ");
            strWhere.Append("U.Status='ACTIVE' ");
            if (roles.Count > 0)
            {
                strWhere.AppendFormat("AND U.RoleID IN({0}) ", string.Join(",", roles.Select(x => (int)x).ToList()));
            }
            if (hideUserIds.Length > 0)
            {
                strWhere.Append("AND U.UserID NOT IN( " + hideUserIds + ") ");
            }
            StringBuilder strUserOrder = new StringBuilder();
            strUserOrder.Append("ORDER BY U.RoleID ,U.FirstName; ");
            strUserSql = strUserSelect.ToString() + strWhere.ToString() + strUserOrder.ToString();

            DateTime now = DateTime.Now;
            DateTime lastMonthStart = now.AddDays(1 - now.Day).AddMonths(-1).Date;
            DateTime currentMonthStart = now.AddDays(1 - now.Day).Date;

            StringBuilder strTicketCountSql = new StringBuilder();
            strTicketCountSql.Append("SELECT TH.ModifiedBy,COUNT(DISTINCT TH.TicketID) AS TicketCount ");
            strTicketCountSql.Append("FROM dbo.TicketHistorys AS TH ");
            strTicketCountSql.Append("WHERE TH.ModifiedBy IN (");
            strTicketCountSql.Append("SELECT  U.UserID FROM dbo.Users AS U ");
            strTicketCountSql.Append(strWhere.ToString());
            strTicketCountSql.Append(") ");
            strTicketCountSql.Append("{0} ");
            strTicketCountSql.Append("GROUP BY TH.ModifiedBy;");

            StringBuilder strPreviousSql = new StringBuilder();
            StringBuilder strPreviousWhereSql = new StringBuilder();
            strPreviousWhereSql.Append("AND TH.ModifiedOn >= '" + lastMonthStart + "' ");
            strPreviousWhereSql.Append("AND TH.ModifiedOn < '" + currentMonthStart + "' ");
            strPreviousSql.AppendFormat(strTicketCountSql.ToString(), strPreviousWhereSql.ToString());

            StringBuilder strCurrentSql = new StringBuilder();
            StringBuilder strCurrentWhereSql = new StringBuilder();
            strCurrentWhereSql.Append("AND TH.ModifiedOn >= '" + currentMonthStart + "' ");
            strCurrentSql.AppendFormat(strTicketCountSql.ToString(), strCurrentWhereSql.ToString());

            StringBuilder strTicketSql = new StringBuilder();
            strTicketSql.Append("SELECT T.ResponsibleUser,T.TicketID,T.Title,T.ProjectID,P.Title AS ProjectName,T.Priority ");
            strTicketSql.Append("FROM dbo.Tickets AS T LEFT JOIN dbo.Projects AS P ON T.ProjectID=P.ProjectID ");
            strTicketSql.Append("WHERE T.ResponsibleUser IN ( ");
            strTicketSql.Append("SELECT  U.UserID FROM dbo.Users AS U ");
            strTicketSql.Append(strWhere.ToString());
            strTicketSql.Append(") ");
            strTicketSql.Append("AND T.ProjectID IN (SELECT ProjectID FROM ProjectUsers PU WHERE  PU.UserID IN (");
            strTicketSql.Append("SELECT  U.UserID FROM dbo.Users AS U ");
            strTicketSql.Append(strWhere.ToString());
            strTicketSql.Append(") ");
            strTicketSql.Append(") ");
            List<TicketsState> status = new List<TicketsState>();
            status.AddRange(TicketsStateHelper.SunnetSHAllowShowStatus);
            string strStatus = string.Join(",", status.Select(x => (int)x).ToList());
            strTicketSql.Append("AND T.Status IN (1, 4, 9, 3, 5, 7, 10, 13, 17, 6, 12, 15, 11, 14, 8, 18, 16, 20, 31, 32) ");
            strTicketSql.Append("ORDER BY T.ModifiedOn DESC;");

            string strSql = strTicketSql.ToString() + strPreviousSql.ToString() + strCurrentSql.ToString() + strUserSql;
            Database db = DatabaseFactory.CreateDatabase();
            List<UserTicketModel> userTickets = new List<UserTicketModel>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    using (IDataReader sdr = db.ExecuteReader(dbCommand))
                    {
                        List<TicketListModel> tickets = new List<TicketListModel>();
                        while (sdr.Read())
                        {
                            TicketListModel ticket = new TicketListModel
                            {
                                ResponsibleUserID = int.Parse(sdr["ResponsibleUser"].ToString()),
                                TicketID = int.Parse(sdr["TicketID"].ToString()),
                                Title = sdr["Title"].ToString(),
                                ProjectID = int.Parse(sdr["ProjectID"].ToString()),
                                ProjectName = sdr["ProjectName"].ToString(),
                                Priority = (PriorityState)sdr["Priority"]
                            };
                            tickets.Add(ticket);
                        }

                        bool isNext = sdr.NextResult();
                        List<TicketHistoryModel> previousList = new List<TicketHistoryModel>();
                        while (sdr.Read() && isNext)
                        {
                            TicketHistoryModel model = new TicketHistoryModel
                            {
                                UserID = int.Parse(sdr[0].ToString()),
                                TicketCount = int.Parse(sdr[1].ToString())
                            };
                            previousList.Add(model);
                        }

                        isNext = sdr.NextResult();
                        List<TicketHistoryModel> currentList = new List<TicketHistoryModel>();
                        while (sdr.Read() && isNext)
                        {
                            TicketHistoryModel model = new TicketHistoryModel
                            {
                                UserID = int.Parse(sdr[0].ToString()),
                                TicketCount = int.Parse(sdr[1].ToString())
                            };
                            currentList.Add(model);
                        }

                        isNext = sdr.NextResult();
                        while (sdr.Read() && isNext)
                        {
                            UserTicketModel model = new UserTicketModel
                            {
                                UserID = int.Parse(sdr[0].ToString()),
                                FirstName = sdr[1].ToString(),
                                LastName = sdr[2].ToString(),
                                RoleID = int.Parse(sdr[3].ToString()),
                                RoleName = sdr[4].ToString()
                            };
                            model.Tickets = tickets.Where(t => t.ResponsibleUserID == model.UserID).Take(10).ToList();
                            model.TicketCount = tickets.Count(t => t.ResponsibleUserID == model.UserID);
                            if (previousList.Count(p => p.UserID == model.UserID) > 0)
                            {
                                model.Previous = previousList.First(p => p.UserID == model.UserID).TicketCount;
                            }
                            else
                            {
                                model.Previous = 0;
                            }
                            if (currentList.Count(p => p.UserID == model.UserID) > 0)
                            {
                                model.Current = currentList.First(p => p.UserID == model.UserID).TicketCount;
                            }
                            else
                            {
                                model.Current = 0;
                            }
                            userTickets.Add(model);
                        }
                    }
                    return userTickets;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql,
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                }
            }
            return userTickets;
        }

        public List<DashboardUserModel> GetUserByRoles(List<RolesEnum> roles)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT U.UserID , U.FirstName, U.LastName, U.RoleID, R.RoleName ");
            strSql.Append("FROM dbo.Users AS U ");
            strSql.Append("LEFT JOIN dbo.Roles AS R ON U.RoleID = R.RoleID ");
            strSql.Append("WHERE ");
            strSql.Append("U.Status='ACTIVE' ");
            if (roles.Count > 0)
            {
                string strRoles = "";
                foreach (RolesEnum role in roles)
                {
                    strRoles += (int)role + ",";
                }
                strRoles = strRoles.Remove(strRoles.Length - 1);
                strSql.Append("AND U.RoleID IN( " + strRoles + ") ");
            }
            Database db = DatabaseFactory.CreateDatabase();
            List<DashboardUserModel> userModels = new List<DashboardUserModel>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    using (IDataReader sdr = db.ExecuteReader(dbCommand))
                    {
                        while (sdr.Read())
                        {
                            DashboardUserModel model = new DashboardUserModel
                            {
                                RoleID = int.Parse(sdr["RoleID"].ToString()),
                                RoleName = sdr["RoleName"].ToString(),
                                UserID = int.Parse(sdr["UserID"].ToString()),
                                FirstName = sdr["FirstName"].ToString(),
                                LastName = sdr["LastName"].ToString(),
                                IsHide = false
                            };
                            userModels.Add(model);
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
            return userModels;
        }

        public bool ExistsUserName(string username, int exceptThis)
        {
            return base.ExistsRecords("Users", "UserName", username, "UserID", exceptThis.ToString());
        }

        public bool IsLoginSuccess(string uname, string upwd)
        {
            bool isLoginSuccess = false;
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbComm = db.GetSqlStringCommand(@"SELECT COUNT(1) FROM Users WHERE IsDelete = 0 AND Status = 'ACTIVE' AND UserName = @UName"))
            {
                db.AddInParameter(dbComm, "UName", DbType.String, uname.FilterSqlString());
                //db.AddInParameter(dbComm, "UPwd", DbType.String, upwd.FilterSqlString());
                isLoginSuccess = Convert.ToInt32(db.ExecuteScalar(dbComm)) > 0 ? true : false;
            }
            return isLoginSuccess;
        }

        #endregion
    }
}

