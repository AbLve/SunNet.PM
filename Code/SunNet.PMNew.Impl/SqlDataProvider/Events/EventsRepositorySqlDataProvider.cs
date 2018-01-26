using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.EventsModule;
using SunNet.PMNew.Entity.EventModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;

namespace SunNet.PMNew.Impl.SqlDataProvider.Events
{
    public partial class EventsRepositorySqlDataProvider : SqlHelper, IEventRepository
    {

        public int Insert(EventEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into dbo.Events(");
            strSql.Append("Icon,Name,Details,[Where],AllDay,FromDay,FromTime,FromTimeType,ToDay,ToTime,ToTimeType,Privacy,GroupID")
            .Append(",CreatedBy,CreatedOn,Highlight,UpdatedOn,Alert,HasInvite,ProjectID,Times,IsOff)");

            strSql.Append(" values (");
            strSql.Append("@Icon,@Name,@Details,@Where,@AllDay,@FromDay,@FromTime,@FromTimeType,@ToDay,@ToTime,@ToTimeType,@Privacy,@GroupID")
            .Append(",@CreatedBy,@CreatedOn,@Highlight,@UpdatedOn,@Alert,@HasInvite,@ProjectID,@Times,@IsOff)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Icon", DbType.Int32, entity.Icon);
                    db.AddInParameter(dbCommand, "Name", DbType.String, entity.Name);
                    db.AddInParameter(dbCommand, "Details", DbType.String, entity.Details);
                    db.AddInParameter(dbCommand, "Where", DbType.String, entity.Where);
                    db.AddInParameter(dbCommand, "AllDay", DbType.Boolean, entity.AllDay);
                    db.AddInParameter(dbCommand, "FromDay", DbType.DateTime, entity.FromDay);
                    db.AddInParameter(dbCommand, "FromTime", DbType.String, entity.FromTime);
                    db.AddInParameter(dbCommand, "FromTimeType", DbType.Int32, entity.FromTimeType);
                    db.AddInParameter(dbCommand, "ToDay", DbType.DateTime, entity.ToDay.Date);
                    db.AddInParameter(dbCommand, "ToTime", DbType.String, entity.ToTime);
                    db.AddInParameter(dbCommand, "ToTimeType", DbType.Int32, entity.ToTimeType);
                    db.AddInParameter(dbCommand, "Privacy", DbType.Int32, (int)entity.Privacy);
                    db.AddInParameter(dbCommand, "GroupID", DbType.String, entity.GroupID == null ? "" : entity.GroupID);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, entity.CreatedBy);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, entity.CreatedOn);
                    db.AddInParameter(dbCommand, "Highlight", DbType.Boolean, entity.Highlight);
                    db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, entity.UpdatedOn);
                    db.AddInParameter(dbCommand, "Alert", DbType.Int32, (int)entity.Alert);
                    db.AddInParameter(dbCommand, "HasInvite", DbType.Boolean, entity.HasInvite);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, entity.ProjectID);
                    db.AddInParameter(dbCommand, "Times", DbType.Int32, entity.Times);
                    db.AddInParameter(dbCommand, "IsOff", DbType.Boolean, entity.IsOff);
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

        public bool Update(EventEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dbo.Events ");
            strSql.Append("SET Icon=@Icon, Name=@Name,Details=@Details,[Where]=@Where,AllDay=@AllDay,FromDay=@FromDay,FromTime=@FromTime")
            .Append(",FromTimeType=@FromTimeType,ToDay=@ToDay,ToTime=@ToTime,ToTimeType=@ToTimeType,Privacy=@Privacy,UpdatedOn=@UpdatedOn,ProjectID=@ProjectID ")
            .Append(" ,Alert=@Alert ,GroupID=@GroupID ,HasInvite=@HasInvite ,HasAlert=@HasAlert ,IsOff=@IsOff")
            .Append(" where ID=@ID");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Icon", DbType.Int32, entity.Icon);
                    db.AddInParameter(dbCommand, "Name", DbType.String, entity.Name);
                    db.AddInParameter(dbCommand, "Details", DbType.String, entity.Details);
                    db.AddInParameter(dbCommand, "Where", DbType.String, entity.Where);
                    db.AddInParameter(dbCommand, "AllDay", DbType.Boolean, entity.AllDay);
                    db.AddInParameter(dbCommand, "FromDay", DbType.DateTime, entity.FromDay);
                    db.AddInParameter(dbCommand, "FromTime", DbType.String, entity.FromTime);
                    db.AddInParameter(dbCommand, "FromTimeType", DbType.Int32, entity.FromTimeType);
                    db.AddInParameter(dbCommand, "ToDay", DbType.DateTime, entity.ToDay.Date);
                    db.AddInParameter(dbCommand, "ToTime", DbType.String, entity.ToTime);
                    db.AddInParameter(dbCommand, "ToTimeType", DbType.Int32, entity.ToTimeType);
                    db.AddInParameter(dbCommand, "Privacy", DbType.Int32, (int)entity.Privacy);
                    db.AddInParameter(dbCommand, "GroupID", DbType.String, entity.GroupID == null ? "" : entity.GroupID);
                    db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, entity.UpdatedOn);
                    db.AddInParameter(dbCommand, "Alert", DbType.Int32, (int)entity.Alert);
                    db.AddInParameter(dbCommand, "HasInvite", DbType.Boolean, entity.HasInvite);
                    db.AddInParameter(dbCommand, "HasAlert", DbType.Boolean, entity.HasAlert);
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entity.ID);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, entity.ProjectID);
                    db.AddInParameter(dbCommand, "IsOff", DbType.Boolean, entity.IsOff);
                    return db.ExecuteNonQuery(dbCommand) > 0;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        public bool Delete(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("DELETE FROM dbo.Events  WHERE id = {0};", entityId)
                .AppendFormat("DELETE FROM dbo.EventShares WHERE EventID={0};", entityId)
                .AppendFormat("DELETE FROM dbo.EventInvites WHERE EventID={0};", entityId);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.ExecuteNonQuery(dbCommand);
                    return true;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }
        #region   工作时间表
        public int AddWorkTime(WorkTimeEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into dbo.WorkTime(");
            strSql.Append("UserID,FromTime,FromTimeType,CreateOn,ToTimeType,ToTime)");

            strSql.Append(" values (");
            strSql.Append("@UserID,@FromTime,@FromTimeType,@CreateOn,@ToTimeType,@ToTime)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, entity.UserID);
                db.AddInParameter(dbCommand, "FromTime", DbType.String, entity.FromTime);

                db.AddInParameter(dbCommand, "FromTimeType", DbType.Int32, entity.FromTimeType);
                db.AddInParameter(dbCommand, "CreateOn", DbType.DateTime, entity.CreateOn);

                db.AddInParameter(dbCommand, "ToTimeType", DbType.Int32, entity.ToTimeType);
                db.AddInParameter(dbCommand, "ToTime", DbType.String, entity.ToTime);

                int result;
                object obj = db.ExecuteScalar(dbCommand);
                if (!int.TryParse(obj.ToString(), out result))
                {
                    return 0;
                }
                return result;
            }
        }

        public List<WorkTimeEntity> GetWorkTime(int userId)
        {
            List<WorkTimeEntity> list = new List<WorkTimeEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" SELECT * FROM dbo.WorkTime WHERE UserID={0}", userId);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    using (IDataReader reader = db.ExecuteReader(dbCommand))
                    {
                        while (reader.Read())
                        {
                            list.Add(new WorkTimeEntity(reader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                }
            }
            return list;
        }

        public bool DeleteWorkTimeByUserId(int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("DELETE FROM dbo.WorkTime  WHERE UserID = {0};", userId);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.ExecuteNonQuery(dbCommand);
                    return true;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }
        #endregion

        public bool DeleteAll(int createdBy, DateTime createdOn, DateTime fromDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("DELETE FROM dbo.Events WHERE CreatedBy = @CreatedBy And CreatedOn = @CreatedOn And FromDay >= @FromDay ;");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, createdBy);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
                    db.AddInParameter(dbCommand, "FromDay", DbType.DateTime, fromDate.Date);
                    db.ExecuteNonQuery(dbCommand);
                    return true;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        public EventEntity Get(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from dbo.Events")
            .AppendFormat(" where ID={0}", entityId);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    IDataReader reader = db.ExecuteReader(dbCommand);
                    if (reader.Read())
                        return new EventEntity(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                }
            }
            return null;
        }

        public EventEntity GetEventByCreateId(int creatId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from dbo.Events")
            .AppendFormat(" where CreatedBy={0} ORDER BY CreatedOn DESC", creatId);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    IDataReader reader = db.ExecuteReader(dbCommand);
                    if (reader.Read())
                        return new EventEntity(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                }
            }
            return null;
        }
        /// <summary>
        /// 大于等于 startDate 小于 endDate
        /// 没有数据时，list.count == 0
        /// 获取指定 UserID 创建的与别人邀请的 ,并且project 是 currentUserId 所有的
        /// UserId 为 零时，表示所有,使用 allUser 参数
        /// </summary>
        public List<EventEntity> GetEvents(int currentUserId, DateTime startDate, DateTime endDate, int userId, string allUser, int projectID)
        {
            List<EventEntity> list = new List<EventEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM dbo.Events where 1=1")
                .AppendFormat(" and FromDay >= '{0}' and FromDay < '{1}' ", startDate, endDate);

            //显示指定 project 的event
            if (projectID > 0)
            {
                strSql.AppendFormat(" and ProjectID ={0} ", projectID);
            }
            else
            {
                strSql.AppendFormat(" and ProjectID in (select ProjectID from ProjectUsers where userid ={0}) ", currentUserId);
            }

            if (userId == 0)
                strSql.AppendFormat(" and (CreatedBy in ({0})  or ID in (select EventID from EventInvites where UserID in ({0})))", allUser);
            else
                strSql.AppendFormat(" and (CreatedBy={0}  or ID in (select EventID from EventInvites where UserID = {0}))", userId);

            strSql.Append(" order by FromDay ,ID");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    IDataReader reader = db.ExecuteReader(dbCommand);
                    while (reader.Read())
                        list.Add(new EventEntity(reader));

                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                }
            }
            return list;
        }

        /// <summary>
        /// 大于等于 startDate < endDate
        /// 没有数据时，list.count == 0
        /// </summary>
        public List<EventEntity> GetEvents(DateTime startDate, int userId, int projectID, int top)
        {
            List<EventEntity> list = new List<EventEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select top {0} * from dbo.Events", top)
            .AppendFormat(" where (CreatedBy={0} or id in (select EventID from dbo.EventInvites where userId={0} and Status in(1,2))) "
            , userId)//自己创建的 ，别人邀请的，已确认了别人邀请的
            .Append(" and FromDay>=@BeginDate ");
            if (projectID != -1)
            {
                strSql.Append(" and ProjectID=@ProjectID ");
            }
            strSql.Append(" order by FromDay ,ID;");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "BeginDate", DbType.DateTime, startDate.Date);
                    if (projectID != -1)
                    {
                        db.AddInParameter(dbCommand, "ProjectID", DbType.Int16, projectID);
                    }
                    IDataReader reader = db.ExecuteReader(dbCommand);
                    while (reader.Read())
                        list.Add(new EventEntity(reader));
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                }
            }
            return list;
        }

        /// <summary>
        /// 获取指定UserID 创建的与别人邀请的 ,并且ticket 的 Project 是 指定 currentUserId可见的 
        /// UserId 为 零时，表示所有,使用 allUser 参数
        /// </summary>
        public List<EventEntity> GetEvents(int currentUserId, DateTime startDate, int userId, string allUser, int projectID, int pageSize, int pageNo, out int recordCount)
        {
            recordCount = 0;
            List<EventEntity> list = new List<EventEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) FROM dbo.Events  where 1=1")
                .AppendFormat(" and FromDay >= '{0}' ", startDate);

            //显示指定 project 的event
            if (projectID > 0)
            {
                strSql.AppendFormat(" and ProjectID ={0} ", projectID);
            }
            else
            {
                strSql.AppendFormat(" and ProjectID in (select ProjectID from ProjectUsers where userid ={0}) ", currentUserId);
            }

            if (userId == 0)
            {
                strSql.AppendFormat(" and  (CreatedBy in ({0})  or  ID in (select EventID from EventInvites where UserID in ({0})))", allUser);
            }
            else
                strSql.AppendFormat(" and  (CreatedBy={0}  or  ID in (select EventID from EventInvites where UserID = {0}))", userId);



            strSql.Append(";SELECT * FROM ( SELECT ROW_NUMBER() OVER (ORDER BY FromDay asc) AS RowNumber, * FROM dbo.Events")
                .AppendFormat(" where FromDay >= '{0}' ", startDate);

            //显示指定 project 的event
            if (projectID > 0)
            {
                strSql.AppendFormat(" and ProjectID ={0} ", projectID);
            }
            else
            {
                strSql.AppendFormat(" and ProjectID in (select ProjectID from ProjectUsers where userid ={0}) ", currentUserId);
            }

            if (userId == 0)
                strSql.AppendFormat(" and  (CreatedBy in ({0})  or  ID in (select EventID from EventInvites where UserID in ({0})))", allUser);
            else
                strSql.AppendFormat(" and  (CreatedBy={0}  or  ID in (select EventID from EventInvites where UserID = {0}))", userId);

            strSql.Append(") AS TB")
                  .AppendFormat(" WHERE RowNumber BETWEEN {0} AND {1} ", (pageNo - 1) * pageSize + 1, pageNo * pageSize);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    IDataReader reader = db.ExecuteReader(dbCommand);
                    if (reader.Read())
                        recordCount = (int)reader[0];
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                            list.Add(new EventEntity(reader));
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                }
            }
            return list;
        }


        public DataSet GetUpdateAndDeleteEvents(int createdBy, DateTime createdOn, DateTime fromDate)
        {
            DataSet ds = new DataSet();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT ID,FromDay FROM dbo.Events WHERE CreatedBy = @CreatedBy And CreatedOn = @CreatedOn And FromDay >= @FromDay ;");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, createdBy);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
                    db.AddInParameter(dbCommand, "FromDay", DbType.DateTime, fromDate.Date);
                    ds = db.ExecuteDataSet(dbCommand);
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                }
                return ds;
            }
        }

        public DataTable QueryReportTotalHoursByProject(int projectID, int userID, DateTime startDate, DateTime endDate, string orderBy,
            string orderDirectioin)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT ts.ProjectID, Title,Sum(Hours) as [Hours]
                                FROM  dbo.Events ts");
            strSql.Append(@"    Where  1=1 ");
            if (projectID > 0)
            {
                strSql.Append(" AND ts.ProjectID=@ProjectID ");
            }
            if (userID > 0)
            {
                strSql.Append(" AND ts.CreatedBy=@UserID ");
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

        public DataTable QueryReportDetailsByProject(int projectID, int userID, DateTime startDate, DateTime endDate, string orderBy,
            string orderDirectioin)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"Select p.Title,tk.ProjectID,tk.CreatedBy,tk.Name,tk.AllDay,u.UserID,u.FirstName,u.LastName,u.Office,u.PTOHoursOfYear,tk.FromDay,tk.FromTime,tk.FromTimeType,tk.ToDay,tk.ToTime,tk.ToTimeType from 
                            dbo.Events tk join Users u on tk.CreatedBy=u.UserID join Projects p on tk.ProjectID=p.ProjectID");
            strSql.Append(@"    Where  1=1 ");
            if (projectID > 0)
            {
                strSql.Append(" AND tk.ProjectID=@ProjectID ");
            }
            if (userID > 0)
            {
                strSql.Append(" AND tk.CreatedBy=@UserID ");
            }
            if (startDate != null && startDate > DateTime.MinValue)
            {
                strSql.Append(" AND tk.FromDay >=@StartDate ");
            }
            if (endDate != null && endDate > DateTime.MinValue)
            {
                strSql.Append(" AND tk.ToDay <=@EndDate ");
            }
            string orderby = string.Empty;
            if (orderBy.ToLower() == "All".ToLower())
            {
                orderby = " p.Title,u.FirstName asc,u.LastName asc ";
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

        public DataTable GetPtoByProjectUser(int projectID, int userID, DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"Select p.Title,u.UserID,u.Office,tk.Details,tk.CreatedOn,tk.AllDay,tk.Name,tk.FromDay,tk.FromTime,tk.FromTimeType,tk.ToDay,tk.ToTime,tk.ToTimeType from 
                            dbo.Events tk join Users u on tk.CreatedBy=u.UserID join Projects p on tk.ProjectID=p.ProjectID");
            strSql.Append(@"    Where  1=1 ");
            if (projectID > 0)
            {
                strSql.Append(" AND tk.ProjectID=@ProjectID ");
            }
            if (userID > 0)
            {
                strSql.Append(" AND tk.CreatedBy=@UserID ");
            }
            if (startDate != null && startDate > DateTime.MinValue)
            {
                strSql.Append(" AND tk.CreatedOn >=@StartDate ");
            }
            if (endDate != null && endDate > DateTime.MinValue)
            {
                strSql.Append(" AND tk.CreatedOn <=@EndDate ");
            }
            strSql.AppendFormat(@"    Order by tk.CreatedOn");
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
    }
}
