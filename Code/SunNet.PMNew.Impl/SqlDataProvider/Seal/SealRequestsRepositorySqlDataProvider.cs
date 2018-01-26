using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.SealModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Impl.SqlDataProvider.Seal
{
    public class SealRequestsRepositorySqlDataProvider : SqlHelper, ISealRequestsRepository
    {
        public int Insert(SealRequestsEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into dbo.SealRequests(");
            strSql.Append("Title,Description,RequestedBy,RequestedDate,Status,Type)");

            strSql.Append(" values (");
            strSql.Append("@Title,@Description,@RequestedBy,@RequestedDate,@Status,@Type)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "Title", DbType.String, entity.Title);
                db.AddInParameter(dbCommand, "Description", DbType.String, entity.Description);

                db.AddInParameter(dbCommand, "RequestedBy", DbType.Int32, entity.RequestedBy);
                db.AddInParameter(dbCommand, "RequestedDate", DbType.DateTime, entity.RequestedDate);

                db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)entity.Status);
                db.AddInParameter(dbCommand, "Type", DbType.Int32, (int)entity.Type);

                int result;
                object obj = db.ExecuteScalar(dbCommand);
                if (!int.TryParse(obj.ToString(), out result))
                {
                    return 0;
                }
                return result;
            }
        }

        public bool Update(SealRequestsEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SealRequests")
            .Append(" set Title =@Title ,Description=@Description,Status=@Status");
            strSql.Append(" where id=@id");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ID", DbType.Int32, entity.ID);

                db.AddInParameter(dbCommand, "Title", DbType.String, entity.Title);
                db.AddInParameter(dbCommand, "Description", DbType.String, entity.Description);
                db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)entity.Status);

                return db.ExecuteNonQuery(dbCommand) > 0;
            }
        }

        public void UpdateSealedStatus(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" if(  (select count(0) from dbo.SealUnionRequests ")
                .Append(" where SealRequestsID = @SealRequestsID and IsSealed=0) =0 )")
                .Append("update SealRequests")
            .Append(" set Status=@Status")
            .Append(" where id=@SealRequestsID");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "SealRequestsID", DbType.Int32, id);
                db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)RequestStatus.Processed);

                db.ExecuteNonQuery(dbCommand);
            }
        }

        public bool UpateStatus(int id, RequestStatus status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SealRequests")
            .Append(" set Status=@Status")
            .Append(" where id=@SealRequestsID");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "SealRequestsID", DbType.Int32, id);
                db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)status);

                return db.ExecuteNonQuery(dbCommand) > 0;
            }
        }

        public bool Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public SealRequestsEntity Get(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from SealRequests where id = @ID");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                        return new SealRequestsEntity(dataReader, false);
                }
            }
            return null;
        }

        public List<SealRequestsEntity> GetList(int userId, string keyword, int type, List<RequestStatus> status, int sealId, DateTime start, DateTime end, string sort
            , string orderby, int pageNo, int pageSize, out int recordCount)
        {
            StringBuilder sbCondition = new StringBuilder();
            sbCondition.Append(" Where 1=1 ");
            if (userId != Config.WorkflowAdmin)
            {
                sbCondition.AppendFormat(" and ((id in(select SealRequestsID from SealUnionRequests where sealedBy = {0} or ApprovedBy = {0}) ", userId);
                sbCondition.AppendFormat(" or id in(select distinct(WorkflowRequestID) from WorkflowHistory where processedby= {0})) ", userId);
                sbCondition.AppendFormat(" or (r.Status <> 1 and r.RequestedBy={0}) ) ", userId);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sbCondition.AppendFormat(" and r.title like '%{0}%' ", keyword);
            }
            if (type >= 0)
                sbCondition.AppendFormat(" and Type={0}", type);
            if (sealId > 0)
                sbCondition.AppendFormat(" and id in(select SealRequestsID from SealUnionRequests where sealId={0})", sealId);
            if (start > DateTime.Parse("1753-1-1"))
                sbCondition.AppendFormat(" and RequestedDate >= '{0}' ", start.Date);
            if (end >= start && end > DateTime.Parse("1753-1-1"))
                sbCondition.AppendFormat(" and RequestedDate< '{0}' ", end.AddDays(1).Date);
            if (status.Count > 0)
            {
                if (status.Count == 1)
                {
                    if (status[0] != RequestStatus.All)
                        sbCondition.AppendFormat(" and r.Status = {0} ", (int)status[0]);
                }
                else
                {
                    string tmp = string.Empty;
                    foreach (var v in status)
                        tmp += string.Format("{0},", (int)v);
                    if (tmp.EndsWith(","))
                        tmp = tmp.Remove(tmp.Length - 1);
                    sbCondition.AppendFormat(" and r.Status in( {0}) ", tmp);
                }
            }


            StringBuilder sbCount = new StringBuilder();
            sbCount.AppendFormat("select count(0) from SealRequests r {0}", sbCondition);

            StringBuilder sb = new StringBuilder();
            sb.Append(" with list as(select r.* ,u.FirstName as RequestedFirstName ,u.LastName as RequestedLastName  from SealRequests r ")
                .Append(" left join users u on u.userid = r.RequestedBy ")
                .AppendFormat(" {0} )", sbCondition);


            sb.Append(" SELECT * FROM ( ")
                .AppendFormat(" SELECT ROW_NUMBER() OVER( ORDER BY {0} {1}) AS INDEX_ID ,", sort, orderby)
                .Append("  * from list ) AS result")
                .AppendFormat(" WHERE INDEX_ID BETWEEN {0} AND {1} ", (pageNo - 1) * pageSize + 1, pageNo * pageSize);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sbCount.ToString()))
            {
                try
                {
                    int result;
                    object obj = db.ExecuteScalar(dbCommand);
                    if (!int.TryParse(obj.ToString(), out result))
                    {
                        recordCount = 0;
                    }
                    else recordCount = result;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        sb.ToString(),
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                    recordCount = 0;
                }
            }

            List<SealRequestsEntity> list = new List<SealRequestsEntity>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                            list.Add(new SealRequestsEntity(dataReader, true));
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        sb.ToString(),
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                    return new List<SealRequestsEntity>();
                }
            }

            return list;
        }

        public List<SealRequestsEntity> GetWaitingList(int userId, string keyword, int type, List<RequestStatus> status, int sealId, DateTime start, DateTime end, string sort
            , string orderby, int pageNo, int pageSize, out int recordCount)
        {
            StringBuilder sbCondition = new StringBuilder();
            sbCondition.AppendFormat(" Where (r.Status=1 AND RequestedBy = {0}) ", userId);
            sbCondition.AppendFormat(" or id in(select distinct(WorkflowRequestID) from WorkflowHistory where processedby= {0} and action=-1) ", userId);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sbCondition.AppendFormat(" and r.title like '%{0}%' ", keyword);
            }
            if (type >= 0)
                sbCondition.AppendFormat(" and Type={0}", type);
            if (sealId > 0)
                sbCondition.AppendFormat(" and id in(select SealRequestsID from SealUnionRequests where sealId={0})", sealId);
            if (start > DateTime.Parse("1753-1-1"))
                sbCondition.AppendFormat(" and RequestedDate >= '{0}' ", start.Date);
            if (end >= start && end > DateTime.Parse("1753-1-1"))
                sbCondition.AppendFormat(" and RequestedDate< '{0}' ", end.AddDays(1).Date);
            if (status.Count > 0)
            {
                if (status.Count == 1)
                {
                    if (status[0] != RequestStatus.All)
                        sbCondition.AppendFormat(" and r.Status = {0} ", (int)status[0]);
                }
                else
                {
                    string tmp = string.Empty;
                    foreach (var v in status)
                        tmp += string.Format("{0},", (int)v);
                    if (tmp.EndsWith(","))
                        tmp = tmp.Remove(tmp.Length - 1);
                    sbCondition.AppendFormat(" and r.Status in( {0}) ", tmp);
                }
            }


            StringBuilder sbCount = new StringBuilder();
            sbCount.AppendFormat("select count(0) from SealRequests r {0}", sbCondition);

            StringBuilder sb = new StringBuilder();
            sb.Append(" with list as(select r.* ,u.FirstName as RequestedFirstName ,u.LastName as RequestedLastName  from SealRequests r ")
                .Append(" left join users u on u.userid = r.RequestedBy ")
                .AppendFormat(" {0} )", sbCondition);


            sb.Append(" SELECT * FROM ( ")
                .AppendFormat(" SELECT ROW_NUMBER() OVER( ORDER BY {0} {1}) AS INDEX_ID ,", sort, orderby)
                .Append("  * from list ) AS result")
                .AppendFormat(" WHERE INDEX_ID BETWEEN {0} AND {1} ", (pageNo - 1) * pageSize + 1, pageNo * pageSize);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sbCount.ToString()))
            {
                int result;
                object obj = db.ExecuteScalar(dbCommand);
                if (!int.TryParse(obj.ToString(), out result))
                {
                    recordCount = 0;
                }
                else recordCount = result;
            }
            List<SealRequestsEntity> list = new List<SealRequestsEntity>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                        list.Add(new SealRequestsEntity(dataReader, true));
                }
            }
            return list;
        }

        public int GetWaitingCount(int userId, List<RequestStatus> status)
        {
            StringBuilder sbCondition = new StringBuilder();
            sbCondition.AppendFormat(" Where (r.Status=1 AND RequestedBy = {0}) ", userId);
            sbCondition.AppendFormat(" or id in(select distinct(WorkflowRequestID) from WorkflowHistory where processedby= {0} and action=-1) ", userId);
            if (status.Count > 0)
            {
                if (status.Count == 1)
                {
                    if (status[0] != RequestStatus.All)
                        sbCondition.AppendFormat(" and r.Status = {0} ", (int)status[0]);
                }
                else
                {
                    string tmp = string.Empty;
                    foreach (var v in status)
                    {
                        tmp += string.Format("{0},", (int)v);
                    }

                    if (tmp.EndsWith(","))
                    {
                        tmp = tmp.Remove(tmp.Length - 1);
                    }
                    
                    sbCondition.AppendFormat(" and r.Status in( {0}) ", tmp);
                }
            }


            StringBuilder sbCount = new StringBuilder();
            sbCount.AppendFormat("select count(0) from SealRequests r {0}", sbCondition);

           

            Database db = DatabaseFactory.CreateDatabase();

            int recordCount = 0;
            using (DbCommand dbCommand = db.GetSqlStringCommand(sbCount.ToString()))
            {
                int result;
                object obj = db.ExecuteScalar(dbCommand);
                if (!int.TryParse(obj.ToString(), out result))
                {
                    recordCount = 0;
                }
                else recordCount = result;
            }

            return recordCount;
        }

        public List<int> GetUsersId(int id)
        {
            List<int> list = new List<int>();
            StringBuilder sb = new StringBuilder();
            sb.Append("select RequestedBy as userid from dbo.SealRequests where id=@SealRequestsID ")
                .Append(" union select ProcessedBy as userid from dbo.WorkflowHistory where WorkflowRequestID=@SealRequestsID ");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                db.AddInParameter(dbCommand, "SealRequestsID", DbType.Int32, id);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                        list.Add((int)dataReader["userid"]);
                }
            }
            return list;
        }
    }
}
