using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Impl.SqlDataProvider.Ticket
{
    /// <summary>
    /// Data access:FeedBacks
    /// </summary>
    public class FeedBacksRepositorySqlDataProvider : SqlHelper, IFeedBacksRepository
    {
        public FeedBacksRepositorySqlDataProvider()
        { }
        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(FeedBacksEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("set @Order = [dbo].[GetFeedbackIndexByTicket] (@TicketID);insert into FeedBacks(");
            strSql.Append("TicketID,Title,Description,CreatedBy,CreatedOn,IsDelete,IsPublic,WaitClientFeedback,WaitPMFeedback,[Order])");

            strSql.Append(" values (");
            strSql.Append(@"@TicketID,@Title,@Description,@CreatedBy,@CreatedOn,@IsDelete,@IsPublic,@WaitClientFeedback,@WaitPMFeedback,@Order)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                db.AddInParameter(dbCommand, "IsDelete", DbType.Boolean, model.IsDelete);
                db.AddInParameter(dbCommand, "IsPublic", DbType.Boolean, model.IsPublic);
                db.AddInParameter(dbCommand, "WaitClientFeedback", DbType.Int32, (int)model.WaitClientFeedback);
                db.AddInParameter(dbCommand, "WaitPMFeedback", DbType.Int32, (int)model.WaitPMFeedback);
                db.AddOutParameter(dbCommand, "Order", DbType.Int32, 8);
                int result;
                object obj = db.ExecuteScalar(dbCommand);
                if (!int.TryParse(obj.ToString(), out result))
                    return 0;
                model.Order = int.Parse("0" + db.GetParameterValue(dbCommand, "Order").ToString());
                return result;
            }
        }

        /// <summary>
        /// Delete a record
        /// </summary>
        public bool Delete(int FeedBackID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update FeedBacks set ");
            strSql.Append("IsDelete=@IsDelete, ");
            strSql.Append("WaitClientFeedback=@WaitClientFeedback, ");
            strSql.Append("WaitPMFeedback=@WaitPMFeedback ");
            strSql.Append(" where FeedBackID=@FeedBackID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "IsDelete", DbType.Boolean, true);
                db.AddInParameter(dbCommand, "WaitClientFeedback", DbType.Int32, FeedbackReplyStatus.Normal);
                db.AddInParameter(dbCommand, "WaitPMFeedback", DbType.Int32, FeedbackReplyStatus.Normal);
                db.AddInParameter(dbCommand, "FeedBackID", DbType.Int32, FeedBackID);
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
        public FeedBacksEntity Get(int FeedBackID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from FeedBacks ");
            strSql.Append(" where FeedBackID=@FeedBackID");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "FeedBackID", DbType.Int32, FeedBackID);
                FeedBacksEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        model = FeedBacksEntity.ReaderBind(dataReader);
                    }
                }
                return model;
            }
        }



        #endregion  Method

        #region IFeedBacksRepository Members

        public List<FeedBacksEntity> GetFeedBackListByTicketId(int tid, bool isSunnet, bool isSunneter
            , bool isPM, bool isSupervisor)
        {
            List<FeedBacksEntity> list = new List<FeedBacksEntity>();

            StringBuilder strSql = new StringBuilder();

            strSql.Append("select * from feedbacks A inner join users B ")
                .Append(" on A.CreatedBy=B.userID")
                .Append(" where TicketID = @TicketID ");

            if (isSunnet && !isSunneter) //Client 只能看pm和client的feedback
            {
                strSql.Clear();
                strSql.Append("select * from feedbacks A inner join users B ")
                    .Append("  inner join Roles C on B.RoleID=C.RoleID ")
                    .Append(" on A.CreatedBy=B.userID")
                    .Append(" where TicketID = @TicketID ")
                    .Append("  and (C.RoleName='PM' or C.RoleName='Client' or C.RoleName='Sales')")
                    .Append(" and IsPublic = 1");
            }

            if (isSupervisor) //Supervisor 只能看pm和Supervisor的feedback
            {
                strSql.Clear();
                strSql.Append("select * from feedbacks A inner join users B ")
                    .Append("  inner join Roles C on B.RoleID=C.RoleID ")
                    .Append(" on A.CreatedBy=B.userID")
                    .Append(" where TicketID = @TicketID ")
                    .Append("  and (C.RoleName='PM' or C.RoleName='Supervisor')");
            }

            strSql.Append(" order by FeedBackID asc ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, tid);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            list.Add(FeedBacksEntity.ReaderBind(dataReader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString()
                        , base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
            return list;
        }

        #endregion

        public bool Update(FeedBacksEntity entity)
        {
            throw new NotImplementedException();
        }

        public List<int> UpdateClientFeedbackStatusToReplied(int ticketID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FeedBackID from FeedBacks where ticketid=@TicketID and WaitClientFeedback=1;")
            .Append(" update FeedBacks set ");
            strSql.Append(" WaitClientFeedback=2");
            strSql.Append(" where ticketid=@TicketID and WaitClientFeedback=1 ");
            Database db = DatabaseFactory.CreateDatabase();
            List<int> updatedIDs = new List<int>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketID);
                IDataReader datareader = db.ExecuteReader(dbCommand);
                while (datareader.Read())
                {
                    updatedIDs.Add((int)datareader[0]);
                }

            }
            return updatedIDs;
        }


        public List<int> UpdatePMFeedbackStatusToReplied(int ticketID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FeedBackID from FeedBacks where ticketid=@TicketID and WaitPMFeedback=1;")
            .Append("update FeedBacks set ");
            strSql.Append(" WaitPMFeedback=2");
            strSql.Append(" where ticketid=@TicketID and WaitPMFeedback=1 ");
            Database db = DatabaseFactory.CreateDatabase();
            List<int> updatedIDs = new List<int>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketID);
                IDataReader datareader = db.ExecuteReader(dbCommand);
                while (datareader.Read())
                {
                    updatedIDs.Add((int)datareader[0]);
                }
            }

            return updatedIDs;
        }


        public bool ReplyFeedback(int ticketId, bool replyPM, bool replyClient)
        {
            if (!replyPM && !replyClient)
                return true;
            var strSql = "";
            if (replyPM)
                strSql += string.Format("update FeedBacks set WaitPMFeedback = {0}  where TicketID = @TicketID and WaitPMFeedback= {1} ;", (int)FeedbackReplyStatus.Replied, (int)FeedbackReplyStatus.Requested);
            if (replyClient)
                strSql += string.Format("update FeedBacks set WaitClientFeedback = {0}  where TicketID = @TicketID and WaitClientFeedback= {1} ;", (int)FeedbackReplyStatus.Replied, (int)FeedbackReplyStatus.Requested);
            Database db = DatabaseFactory.CreateDatabase();
            List<int> updatedIDs = new List<int>();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketId);
                return db.ExecuteNonQuery(dbCommand) >= 0;
            }
            return false;
        }


        public int CountWaiting(int ticketId)
        {
            var strSql = "select Count([FeedBackID]) from [FeedBacks] Where [TicketID] = @TicketID AND ( [WaitClientFeedback] = @Waiting OR [WaitPMFeedback] = @Waiting )";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketId);
                    db.AddInParameter(dbCommand, "Waiting", DbType.Int32, (int)FeedbackReplyStatus.Requested);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            return dataReader.GetInt32(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString()
                        , base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return -1;
                }
            }
            return -1;
        }
    }
}

