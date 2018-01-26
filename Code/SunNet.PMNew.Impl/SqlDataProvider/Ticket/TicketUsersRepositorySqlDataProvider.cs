using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Entity.UserModel.UserModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Impl.SqlDataProvider.Ticket
{
    /// <summary>
    /// Data access:TicketUsers
    /// </summary>
    public class TicketUsersRepositorySqlDataProvider : SqlHelper, ITicketsUserRepository
    {
        public TicketUsersRepositorySqlDataProvider()
        { }
        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(TicketUsersEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TicketUsers(");
            strSql.Append("TicketID,UserID,Type,[Status])");

            strSql.Append(" values (");
            strSql.Append("@TicketID,@UserID,@Type,@Status)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
                db.AddInParameter(dbCommand, "Type", DbType.Int32, (int)model.Type);
                db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)model.WorkingOnStatus);
                int result;
                object obj = db.ExecuteScalar(dbCommand);
                if (!int.TryParse(obj.ToString(), out result))
                {
                    return 0;
                }
                return result;
            }
        }


        /// <summary>
        /// Update a record
        /// </summary>
        public bool Update(TicketUsersEntity model)
        {
            string strSql = @"update TicketUsers set  
                            TicketID=@TicketID, 
                            UserID=@UserID,
                            [Type]=@Type,
                            [Status]=@Status
                            where TUID=@TUID ";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                db.AddInParameter(dbCommand, "TUID", DbType.Int32, model.TUID);
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
                db.AddInParameter(dbCommand, "Type", DbType.Int32, (int)model.Type);
                db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)model.WorkingOnStatus);
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
        }

        public List<TicketUsersEntity> GetTicketUserList(int ticketId)
        {
            List<TicketUsersEntity> list = new List<TicketUsersEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" select * from ticketUsers where ticketid={0};", ticketId);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            list.Add(TicketUsersEntity.ReaderBind(dataReader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                          strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                }
                return list;
            }
        }

        public List<int> GetTicketUserId(int ticketId, List<int> userIds)
        {
            List<int> noneDeletedUserIds = new List<int>();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" select userID from ticketUsers where ticketid={0};", ticketId)
                .Append("delete from TicketUsers ")
                .AppendFormat(" where ticketid={0} ", ticketId)
                .AppendFormat(" and userid not in({0}) ", string.Join(",", userIds))
                .Append(" and Type<>5 ;");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            noneDeletedUserIds.Add(int.Parse(dataReader[0].ToString()));
                        }
                    }
                    return noneDeletedUserIds;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                          strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return new List<int>();
                }
            }
        }

        public bool Delete(int TUID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TicketUsers ");
            strSql.Append(" where TUID=@TUID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TUID", DbType.Int32, TUID);
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
        }

        /// <summary>
        /// Get an object entity
        /// </summary>
        public TicketUsersEntity Get(int TUID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TicketUsers ");
            strSql.Append(" where TUID=@TUID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TUID", DbType.Int32, TUID);
                TicketUsersEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        model = TicketUsersEntity.ReaderBind(dataReader);
                    }
                }
                return model;
            }
        }


        /// <summary>
        /// get user list by ticket id
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public List<TicketUsersEntity> GetListUsersByTicketId(int tid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TicketUsers ");
            strSql.Append(" where TicketID=@TicketID ");
            Database db = DatabaseFactory.CreateDatabase();
            List<TicketUsersEntity> list = new List<TicketUsersEntity>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, tid);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        list.Add(TicketUsersEntity.ReaderBind(dataReader));
                    }
                }
            }
            return list;
        }
        public List<SelectUserModel> GetSelectUsersByTicketId(int tid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT TU.UserID , U.RoleID, U.FirstName, U.LastName ");
            strSql.Append("FROM dbo.TicketUsers AS TU LEFT JOIN dbo.Users AS U ON TU.UserID = U.UserID ");
            strSql.Append("WHERE   TU.TicketID = @TicketID ");
            Database db = DatabaseFactory.CreateDatabase();
            List<SelectUserModel> list = new List<SelectUserModel>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, tid);
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        SelectUserModel model = new SelectUserModel
                        {
                            UserID = int.Parse(dr["UserID"].ToString()),
                            Role = (RolesEnum)dr["RoleID"],
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString()
                        };
                        list.Add(model);
                    }
                }
            }
            return list;
        }

        public List<TicketUsersEntity> GetListByUserId(int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TicketUsers ");
            strSql.Append(" where UserID=@UserID ");
            Database db = DatabaseFactory.CreateDatabase();
            List<TicketUsersEntity> list = new List<TicketUsersEntity>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        list.Add(TicketUsersEntity.ReaderBind(dataReader));
                    }
                }
            }
            return list;
        }


        public List<TicketDistinctUsersResponse> GetListDistinctUsersByTicketId(int ticketId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct ticketId,UserId from TicketUsers ");
            strSql.Append(" where TicketID=@TicketID ");
            Database db = DatabaseFactory.CreateDatabase();
            List<TicketDistinctUsersResponse> list = new List<TicketDistinctUsersResponse>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketId);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        list.Add(TicketDistinctUsersResponse.ReaderBind(dataReader));
                    }
                }
            }
            return list;
        }

        public void UpdateTicketPM(int OldPMId, int NewPmId, int ProjectID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" insert TicketUsers  select tu1.ticketid , {0} , 1 ,1,0,0 from TicketUsers tu1", NewPmId)
                .AppendFormat(" left join TicketUsers tu2 on tu2.TicketID = tu1.TicketID and tu2.UserId = {0} ", NewPmId)
                .AppendFormat(" where tu1.TicketID in (select TicketID from dbo.Tickets where ProjectID = {0}) ", ProjectID)
                .AppendFormat(" and tu1.UserId={0} and tu2.TicketID is null ", OldPMId);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    db.ExecuteNonQuery(dbCommand);
                }
            }
        }

        public bool IsTicketUser(int tid, int uid, List<TicketUsersType> types)
        {
            if (types == null) types = new List<TicketUsersType>();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) from TicketUsers ");
            strSql.AppendFormat(" where TicketID=@TicketID  and UserID=@UserId and Type in ({0})",
                string.Join(",", types.Select(x => (int)x)));
            Database db = DatabaseFactory.CreateDatabase();
            List<TicketUsersEntity> list = new List<TicketUsersEntity>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, tid);
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, uid);
                return (int)db.ExecuteScalar(dbCommand) > 0 ? true : false;
            }
        }

        public List<TicketUsersEntity> GetTicketUser(int ticketID, TicketUsersType type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TicketUsers ");
            strSql.AppendFormat(" where TicketID=@TicketID and [type] = {0} ", (int)type);
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                var list = new List<TicketUsersEntity>();
                while (dataReader.Read())
                {
                    list.Add(TicketUsersEntity.ReaderBind(dataReader));
                }
                return list;
            }
            return null;
        }

        public void UpdateCreateUser(int newClientID, int ticketID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dbo.TicketUsers set userid = @NewClientId ")
                .Append(" where Type=5 and TicketID=@ticketID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "NewClientId", DbType.Int32, newClientID);
                db.AddInParameter(dbCommand, "ticketID", DbType.Int32, ticketID);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    db.ExecuteNonQuery(dbCommand);
                }
            }
        }

        public void UpdateTicketUserType(int userID, TicketUsersType type, int ticketID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dbo.TicketUsers set type=@type ")
                .Append(" where User=@User and TicketID =@ticketID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "userID", DbType.Int32, userID);
                db.AddInParameter(dbCommand, "type", DbType.Int32, (int)type);
                db.AddInParameter(dbCommand, "ticketID", DbType.Int32, (int)ticketID);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    db.ExecuteNonQuery(dbCommand);
                }
            }
        }

        public bool UpdateWorkingOnStatus(int ticket, int user, Entity.TicketModel.Enums.TicketUserStatus status)
        {
            string strSql = "update TicketUsers set [Status] = @Status where TicketID = @TicketID";
            if (user > 0)
                strSql += " AND UserID = @UserID";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)status);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticket);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, user);
                    int rows = db.ExecuteNonQuery(dbCommand);
                    return rows >= 0;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql, base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        public TicketUsersEntity Get(int ticket, int user)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from TicketUsers where TicketID = @TicketID AND UserID = @UserID Order by TUID desc");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, user);
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticket);
                TicketUsersEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        if (dataReader.Read())
                            model = TicketUsersEntity.ReaderBind(dataReader);
                        return model;
                    }
                    catch (Exception ex)
                    {
                        WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                            strSql, base.FormatParameters(dbCommand.Parameters), ex.Message));
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// 产生气泡，通知其它用户
        /// </summary>
        public bool CreateNotification(int ticketId, int userId, bool notTificationClient = true)
        {
            string strSql = string.Format("update TicketUsers set [ShowNotification] = 1 where TicketID ={0}  and UserId != {1} ", ticketId, userId);
            if (notTificationClient)
                strSql += string.Format(" and type != {0} ", TicketUsersType.Client);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    int rows = db.ExecuteNonQuery(dbCommand);
                    return rows >= 0;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql, base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        public bool UpdateNotification(int ticketId, bool showNtfctn, List<int> users, List<TicketUsersType> types)
        {
            string strSql = "update TicketUsers set [ShowNotification] = @ShowNotification where TicketID = @TicketID ";
            if (showNtfctn)
                strSql += " AND UserID <> @UserID ";
            strSql += " AND ( ";
            if (users != null)
                strSql += "UserId in (" + string.Join(",", users) + ") ";
            else
                strSql += " 1 = 0 ";

            if (types != null)
            {
                var typesId = types.Select(x => (int)x);
                strSql += " OR Type in (" + string.Join(",", typesId) + ")";
            }
            else
                strSql += " OR 1 = 0";
            strSql += ")";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ShowNotification", DbType.Boolean, showNtfctn);
                    db.AddInParameter(dbCommand, "UserId", DbType.Int32, IdentityContext.UserID);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketId);
                    int rows = db.ExecuteNonQuery(dbCommand);
                    return rows >= 0;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql, base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }
        public bool UpdateTicketStatus(int ticketId, UserTicketStatus status, List<int> users, List<TicketUsersType> types)
        {
            string where = " TicketID = @TicketID AND (";
            if (users != null)
                where += " UserId in (" + string.Join(",", users) + ") ";
            else
                where += " 1 = 0 ";
            if (types != null)
            {
                var typesId = types.Select(x => (int)x);
                where += " OR Type in (" + string.Join(",", typesId) + ")";
            }
            else
                where += " OR 1 = 0";
            where += ")";
            var strSql1 = "update TicketUsers set [ShowNotification] = 1 where " + where + " AND UserID <> @UserID ;";
            strSql1 += "update TicketUsers set [TicketStatus] = @Status where " + where;
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql1))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserId", DbType.Int32, IdentityContext.UserID);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketId);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)status);
                    int rows = db.ExecuteNonQuery(dbCommand);
                    return rows >= 0;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql1, base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        public bool UpdateTicketStatus(int ticketId, UserTicketStatus status)
        {
            string where = " TicketID = @TicketID ";

            //var strSql1 = "update TicketUsers set [ShowNotification] = 1 where " + where + " AND UserID <> @UserID ;";//去除更新气泡的操作，更改状态不进行更改气泡
            var strSql1 = "";
            strSql1 += "update TicketUsers set [TicketStatus] = @Status where " + where;
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql1))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserId", DbType.Int32, IdentityContext.UserID);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketId);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)status);
                    int rows = db.ExecuteNonQuery(dbCommand);
                    return rows >= 0;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql1, base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        public int GetCount(string where)
        {
            var strSql = "select count(1) from TicketUsers where " + where;
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                TicketUsersEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        return dataReader.GetInt32(0);
                    }
                    catch (Exception ex)
                    {
                        WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                            strSql, base.FormatParameters(dbCommand.Parameters), ex.Message));
                        return 0;
                    }
                }
            }
        }


        public bool Delete(int ticketId, List<int> users, List<TicketUsersType> types)
        {
            string where = " TicketID = @TicketID AND (";
            if (users != null)
                where += " UserId in (" + string.Join(",", users) + ") ";
            else
                where += " 1 = 0 ";
            if (types != null)
            {
                var typesId = types.Select(x => (int)x);
                where += " OR Type in (" + string.Join(",", typesId) + ")";
            }
            else
                where += " OR 1 = 0";
            where += ")";
            var strSql1 = string.Format("delete TicketUsers where " + where + " AND [Type] Not IN ({0},{1}) ;",
                (int)TicketUsersType.Create,
                (int)TicketUsersType.PM);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql1))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserId", DbType.Int32, IdentityContext.UserID);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketId);
                    int rows = db.ExecuteNonQuery(dbCommand);
                    return rows >= 0;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql1, base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }


        public bool TryClearWaiting(int ticketId, int userId, List<TicketUsersType> types)
        {
            var strSql1 = @"update TicketUsers set [TicketStatus] = @Status where TicketID = @TicketID AND UserId = @UserId;
                            SELECT COUNT(1) FROM TicketUsers WHERE  TicketID = @TicketID AND TicketStatus <> @Status
                            ";
            if (types != null)
            {
                var typesId = types.Select(x => (int)x);
                strSql1 += " AND Type in (" + string.Join(",", typesId) + ")";
            }
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql1))
            {
                try
                {
                    db.AddInParameter(dbCommand, "UserId", DbType.Int32, userId);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketId);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)UserTicketStatus.Normal);
                    using (var dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            var count = dataReader.GetInt32(0);
                            return count == 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql1, base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
            return false;
        }

        public bool ClearWaitingByType(int ticketId, List<TicketUsersType> types)
        {
            var strSql1 = @"update TicketUsers set [TicketStatus] = @Status where TicketID = @TicketID ";
            if (types != null)
            {
                var typesId = types.Select(x => (int)x);
                strSql1 += " AND Type in (" + string.Join(",", typesId) + ")";
            }
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql1))
            {
                try
                {
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketId);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)UserTicketStatus.Normal);
                    using (var dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            var count = dataReader.GetInt32(0);
                            return count == 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql1, base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
            return false;
        }

        public List<String> GetUsersWithStatus(int ticketId, UserTicketStatus status, List<TicketUsersType> types)
        {
            List<String> list = new List<String>();
            var strSql = @"Select u.firstname+' '+u.lastname as UserName from ticketusers tu, users u 
                           Where tu.TicketID = @TicketID AND tu.userid=u.userid AND tu.TicketStatus = @Status";

            if (types != null)
            {
                var typesId = types.Select(x => (int)x);
                strSql += " AND Type in (" + string.Join(",", typesId) + ")";
            }
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketId);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)status);
                    using (var dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            list.Add(dataReader["UserName"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql, base.FormatParameters(dbCommand.Parameters), ex.Message));
                }
            }
            return list;
        }
    }
}


