using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Core.ProjectModule;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.ProjectModel;
using System.Data.SqlClient;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Impl.SqlDataProvider.Ticket
{
    /// <summary>
    /// Data access:Tickets
    /// </summary>
    public class TicketsRepositorySqlDataProvider : SqlHelper, ITicketsRepository
    {
        public TicketsRepositorySqlDataProvider()
        { }

        #region  Method

        #region base method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(TicketsEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Tickets(");
            strSql.Append("CompanyID,ProjectID,Title,TicketCode,TicketType,Description,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,PublishDate")
            .Append(",ClientPublished,StartDate,DeliveryDate,ContinueDate,URL,Priority,Status,ConvertDelete,IsInternal,CreateType,SourceTicketID,IsEstimates")
            .Append(",InitialTime,FinalTime,EsUserID,Source,additionalstate,ProprosalName,WorkPlanName,WorkScope,Invoice,Accounting,IsRead,ResponsibleUser)");

            strSql.Append(" values (");
            strSql.Append("@CompanyID,@ProjectID,@Title,@TicketCode,@TicketType,@Description,@CreatedOn,@CreatedBy,@ModifiedOn,@ModifiedBy,@PublishDate")
            .Append(",@ClientPublished,@StartDate,@DeliveryDate,@ContinueDate,@URL,@Priority,@Status,@ConvertDelete,@IsInternal,@CreateType,@SourceTicketID")
            .Append(",@IsEstimates,@InitialTime,@FinalTime,@EsUserID,@Source,@additionalstate,@ProprosalName,@WorkPlanName,@WorkScope,@Invoice,@Accounting,@IsRead,@ResponsibleUser)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, model.CompanyID);
                db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                db.AddInParameter(dbCommand, "TicketCode", DbType.String, model.TicketCode);
                db.AddInParameter(dbCommand, "TicketType", DbType.String, model.TicketType);
                db.AddInParameter(dbCommand, "Description", DbType.String, model.FullDescription);
                db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, model.ModifiedOn);
                db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, model.ModifiedBy);
                db.AddInParameter(dbCommand, "PublishDate", DbType.DateTime, model.PublishDate);
                db.AddInParameter(dbCommand, "ClientPublished", DbType.Boolean, model.ClientPublished);
                db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, model.StartDate);
                db.AddInParameter(dbCommand, "DeliveryDate", DbType.DateTime, model.DeliveryDate);
                db.AddInParameter(dbCommand, "ContinueDate", DbType.Int32, model.ContinueDate);
                db.AddInParameter(dbCommand, "URL", DbType.String, model.URL);
                db.AddInParameter(dbCommand, "Priority", DbType.Int32, model.Priority);
                db.AddInParameter(dbCommand, "Status", DbType.Int32, model.Status);
                db.AddInParameter(dbCommand, "ConvertDelete", DbType.Int32, model.ConvertDelete);
                db.AddInParameter(dbCommand, "IsInternal", DbType.Boolean, model.IsInternal);
                db.AddInParameter(dbCommand, "CreateType", DbType.Int32, model.CreateType);
                db.AddInParameter(dbCommand, "SourceTicketID", DbType.Int32, model.SourceTicketID);
                db.AddInParameter(dbCommand, "IsEstimates", DbType.Boolean, model.IsEstimates);
                db.AddInParameter(dbCommand, "InitialTime", DbType.Decimal, model.InitialTime);
                db.AddInParameter(dbCommand, "FinalTime", DbType.Decimal, model.FinalTime);
                db.AddInParameter(dbCommand, "EsUserID", DbType.Int32, model.EsUserID);
                db.AddInParameter(dbCommand, "Source", DbType.Int32, (int)model.Source);
                db.AddInParameter(dbCommand, "additionalstate", DbType.Int32, AdditionalStates.Normal);
                db.AddInParameter(dbCommand, "ProprosalName", DbType.String, model.ProprosalName);
                db.AddInParameter(dbCommand, "WorkPlanName", DbType.String, model.WorkPlanName);
                db.AddInParameter(dbCommand, "WorkScope", DbType.String, model.WorkScope);
                db.AddInParameter(dbCommand, "Invoice", DbType.String, model.Invoice);
                db.AddInParameter(dbCommand, "Accounting", DbType.String, (int)model.Accounting);
                db.AddInParameter(dbCommand, "IsRead", DbType.Int32, TicketIsRead.Unread);
                db.AddInParameter(dbCommand, "ResponsibleUser", DbType.Int32,model.ResponsibleUser);
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
        public bool Update(TicketsEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tickets set ");
            strSql.Append("CompanyID=@CompanyID,");
            strSql.Append("ProjectID=@ProjectID,");
            strSql.Append("Title=@Title,");
            strSql.Append("TicketType=@TicketType,");
            strSql.Append("Description=@Description,");
            strSql.Append("ModifiedOn=@ModifiedOn,");
            strSql.Append("ModifiedBy=@ModifiedBy,");
            strSql.Append("PublishDate=@PublishDate,");
            strSql.Append("ClientPublished=@ClientPublished,");
            strSql.Append("StartDate=@StartDate,");
            strSql.Append("DeliveryDate=@DeliveryDate,");
            strSql.Append("ContinueDate=@ContinueDate,");
            strSql.Append("URL=@URL,");
            strSql.Append("Priority=@Priority,");
            strSql.Append("Status=@Status,");
            strSql.Append("ConvertDelete=@ConvertDelete,");
            strSql.Append("IsInternal=@IsInternal,");
            strSql.Append("CreateType=@CreateType,");
            strSql.Append("SourceTicketID=@SourceTicketID,");
            strSql.Append("IsEstimates=@IsEstimates,");
            strSql.Append("InitialTime=@InitialTime,");
            strSql.Append("FinalTime=@FinalTime,");
            strSql.Append("EsUserID=@EsUserID ,");
            strSql.Append("Star=@Star,");
            strSql.Append("AdditionalState=@AdditionalState,");
            strSql.Append("ResponsibleUser=@ResponsibleUser,");
            strSql.Append("ProprosalName=@ProprosalName ,");
            strSql.Append("WorkPlanName=@WorkPlanName,");
            strSql.Append("WorkScope=@WorkScope,");
            strSql.Append("Invoice=@Invoice");
            strSql.Append("IsRead=@IsRead");
            strSql.Append(" where TicketID=@TicketID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, model.CompanyID);
                db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                db.AddInParameter(dbCommand, "TicketType", DbType.String, model.TicketType);
                db.AddInParameter(dbCommand, "Description", DbType.String, model.FullDescription);
                db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, model.ModifiedOn);
                db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, model.ModifiedBy);
                db.AddInParameter(dbCommand, "PublishDate", DbType.DateTime, model.PublishDate);
                db.AddInParameter(dbCommand, "ClientPublished", DbType.Boolean, model.ClientPublished);
                db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, model.StartDate);
                db.AddInParameter(dbCommand, "DeliveryDate", DbType.DateTime, model.DeliveryDate);
                db.AddInParameter(dbCommand, "ContinueDate", DbType.Int32, model.ContinueDate);
                db.AddInParameter(dbCommand, "URL", DbType.String, model.URL);
                db.AddInParameter(dbCommand, "Priority", DbType.Int32, (int)model.Priority);
                db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)model.Status);
                db.AddInParameter(dbCommand, "ConvertDelete", DbType.Int32, (int)model.ConvertDelete);
                db.AddInParameter(dbCommand, "IsInternal", DbType.Boolean, model.IsInternal);
                db.AddInParameter(dbCommand, "CreateType", DbType.Int32, model.CreateType);
                db.AddInParameter(dbCommand, "SourceTicketID", DbType.Int32, model.SourceTicketID);
                db.AddInParameter(dbCommand, "IsEstimates", DbType.Boolean, model.IsEstimates);
                db.AddInParameter(dbCommand, "InitialTime", DbType.Decimal, model.InitialTime);
                db.AddInParameter(dbCommand, "FinalTime", DbType.Decimal, model.FinalTime);
                db.AddInParameter(dbCommand, "EsUserID", DbType.Int32, model.EsUserID);
                db.AddInParameter(dbCommand, "Star", DbType.Int32, model.Star);
                db.AddInParameter(dbCommand, "AdditionalState", DbType.Int32, -1);
                db.AddInParameter(dbCommand, "ResponsibleUser", DbType.Int32, model.ResponsibleUser);

                db.AddInParameter(dbCommand, "ProprosalName", DbType.String, model.ProprosalName);
                db.AddInParameter(dbCommand, "WorkPlanName", DbType.String, model.WorkPlanName);
                db.AddInParameter(dbCommand, "WorkScope", DbType.String, model.WorkScope);
                db.AddInParameter(dbCommand, "Invoice", DbType.String, model.Invoice);
                db.AddInParameter(dbCommand, "IsRead", DbType.Int32, TicketIsRead.Unread);

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

        public bool Update(TicketsEntity model, bool isUpdateStatus)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tickets set ");
            strSql.Append("CompanyID=@CompanyID,");
            strSql.Append("ProjectID=@ProjectID,");
            strSql.Append("Title=@Title,");
            strSql.Append("TicketType=@TicketType,");
            strSql.Append("Description=@Description,");
            strSql.Append("ModifiedOn=@ModifiedOn,");
            strSql.Append("ModifiedBy=@ModifiedBy,");
            strSql.Append("PublishDate=@PublishDate,");
            strSql.Append("ClientPublished=@ClientPublished,");
            strSql.Append("StartDate=@StartDate,");
            strSql.Append("DeliveryDate=@DeliveryDate,");
            strSql.Append("ContinueDate=@ContinueDate,");
            strSql.Append("URL=@URL,");
            strSql.Append("Priority=@Priority,");
            if (isUpdateStatus)
                strSql.Append("Status=@Status,");
            strSql.Append("ConvertDelete=@ConvertDelete,");
            strSql.Append("IsInternal=@IsInternal,");
            strSql.Append("CreateType=@CreateType,");
            strSql.Append("SourceTicketID=@SourceTicketID,");
            strSql.Append("IsEstimates=@IsEstimates,");
            strSql.Append("InitialTime=@InitialTime,");
            strSql.Append("FinalTime=@FinalTime,");
            strSql.Append("EsUserID=@EsUserID ,");
            strSql.Append("Star=@Star,");
            strSql.Append("ConfirmEstmateUserId=@ConfirmEstmateUserId,");
            strSql.Append("AdditionalState=@AdditionalState,");
            strSql.Append("ResponsibleUser=@ResponsibleUser,");
            strSql.Append("ProprosalName=@ProprosalName ,");
            strSql.Append("WorkPlanName=@WorkPlanName,");
            strSql.Append("WorkScope=@WorkScope,");
            strSql.Append("Invoice=@Invoice,");
            strSql.Append("Accounting=@Accounting,");
            strSql.Append("IsRead=@IsRead ");
            strSql.Append(" where TicketID=@TicketID ");
            
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, model.CompanyID);
                db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                db.AddInParameter(dbCommand, "TicketType", DbType.String, model.TicketType);
                db.AddInParameter(dbCommand, "Description", DbType.String, model.FullDescription);
                db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, model.ModifiedOn);
                db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, model.ModifiedBy);
                db.AddInParameter(dbCommand, "PublishDate", DbType.DateTime, model.PublishDate);
                db.AddInParameter(dbCommand, "ClientPublished", DbType.Boolean, model.ClientPublished);
                db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, model.StartDate);
                db.AddInParameter(dbCommand, "DeliveryDate", DbType.DateTime, model.DeliveryDate);
                db.AddInParameter(dbCommand, "ContinueDate", DbType.Int32, model.ContinueDate);
                db.AddInParameter(dbCommand, "URL", DbType.String, model.URL);
                db.AddInParameter(dbCommand, "Priority", DbType.Int32, (int)model.Priority);
                db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)model.Status);
                db.AddInParameter(dbCommand, "ConvertDelete", DbType.Int32, (int)model.ConvertDelete);
                db.AddInParameter(dbCommand, "IsInternal", DbType.Boolean, model.IsInternal);
                db.AddInParameter(dbCommand, "CreateType", DbType.Int32, model.CreateType);
                db.AddInParameter(dbCommand, "SourceTicketID", DbType.Int32, model.SourceTicketID);
                db.AddInParameter(dbCommand, "IsEstimates", DbType.Boolean, model.IsEstimates);
                db.AddInParameter(dbCommand, "InitialTime", DbType.Decimal, model.InitialTime);
                db.AddInParameter(dbCommand, "FinalTime", DbType.Decimal, model.FinalTime);
                db.AddInParameter(dbCommand, "EsUserID", DbType.Int32, model.EsUserID);
                db.AddInParameter(dbCommand, "Star", DbType.Int32, model.Star);
                db.AddInParameter(dbCommand, "ConfirmEstmateUserId", DbType.Int32, model.ConfirmEstmateUserId);
                db.AddInParameter(dbCommand, "AdditionalState", DbType.Int32, -1);
                db.AddInParameter(dbCommand, "ResponsibleUser", DbType.Int32, model.ResponsibleUser);

                db.AddInParameter(dbCommand, "ProprosalName", DbType.String, model.ProprosalName);
                db.AddInParameter(dbCommand, "WorkPlanName", DbType.String, model.WorkPlanName);
                db.AddInParameter(dbCommand, "WorkScope", DbType.String, model.WorkScope);
                db.AddInParameter(dbCommand, "Invoice", DbType.String, model.Invoice);
                db.AddInParameter(dbCommand, "Accounting", DbType.Int32, model.Accounting);
                db.AddInParameter(dbCommand, "IsRead", DbType.Int32, TicketIsRead.Unread);

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
        public bool UpdateEs(decimal time, int tid, bool IsFinal)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tickets set ");
            if (IsFinal)
            {
                strSql.Append("FinalTime=@FinalTime");
            }
            else
            {
                strSql.Append("InitialTime=@InitialTime");
            }
            strSql.Append(" where TicketID=@TicketID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, tid);
                db.AddInParameter(dbCommand, "InitialTime", DbType.Decimal, time);
                db.AddInParameter(dbCommand, "FinalTime", DbType.Decimal, time);
                UpdateIsRead(tid, TicketIsRead.Unread);
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

        public bool UpdateStatus(int ticketId, TicketsState state)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tickets set Status=@Status");
            strSql.Append(" where TicketID=@TicketID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketId);
                db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)state);
                UpdateIsRead(ticketId, TicketIsRead.Unread);
                return db.ExecuteNonQuery(dbCommand) > 0;
            }
        }

        public bool UpdateIsRead(int ticketId, TicketIsRead isRead)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tickets set IsRead=@IsRead");
            strSql.Append(" where TicketID=@TicketID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketId);
                db.AddInParameter(dbCommand, "IsRead", DbType.Int32, isRead);
                return db.ExecuteNonQuery(dbCommand) > 0;
            }
        }

        public bool DeleteTicketEs(int ticketID, bool isDeleteInital, bool isDeleteFinal)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from EsDetail ");
            strSql.Append(" where TicketID=@TicketID ");

            if (!isDeleteFinal)
            {
                if (isDeleteInital)
                {
                    strSql.Append(" and  IsPM=0");
                }
            }
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketID);
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
        /// Delete a record
        /// </summary>
        public bool Delete(int TicketID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tickets ");
            strSql.Append(" where TicketID=@TicketID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, TicketID);
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

        #endregion

        #region get dto

        public int GetCompanyIdByTicketId(int tid)
        {
            int companyId = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CompanyID from Tickets ");
            strSql.Append(" where TicketID = @TicketID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, tid);

                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        companyId = int.Parse(dataReader["CompanyID"].ToString());
                    }
                }
                return companyId;
            }
        }

        /// <summary>
        /// Get an object entity
        /// </summary>
        public TicketsEntity Get(int TicketID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT T.*,P.ProjectCode, ProjectTitle = P.Title FROM DBO.GetTicketsView(@UserId) T Left join Projects P on T.ProjectID = P.ProjectID");
            strSql.Append(" where TicketID=@TicketID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, IdentityContext.UserID);
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, TicketID);
                TicketsEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        model = TicketsEntity.ReaderBind(dataReader, true);
                    }
                }
                return model;
            }
        }

        public TicketsEntity GetTicketWithProjectTitle(int ticketID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select t.*,p.Title as ProjectTitle,p.ProjectCode from (SELECT * FROM DBO.GetTicketsView(@UserId)) t inner join projects p on t.projectid=p.projectid");
            strSql.Append(" where TicketID=@TicketID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketID);
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, IdentityContext.UserID);
                TicketsEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        model = TicketsEntity.ReaderBind(dataReader, true);
                    }
                }
                return model;
            }
        }

        /// <summary>
        /// get an dto ProjectIdAndUserID entity
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public GetProjectIdAndUserIDResponse GetProjectIdAndUserID(int ticketId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select projectId,CreatedBy,CompanyID from Tickets ");
            strSql.Append(" where TicketID=@TicketID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketId);
                GetProjectIdAndUserIDResponse model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        model = GetProjectIdAndUserIDResponse.ReaderBind(dataReader);
                    }
                }
                return model;
            }
        }

        #endregion

        #endregion  Method

        #region ITicketsRepository Members

        private string GetStatesArray(List<TicketsState> list)
        {
            StringBuilder status = new StringBuilder();
            foreach (TicketsState state in list)
            {
                status.Append((int)state);
                status.Append(",");
            }
            return status.ToString().TrimEnd(",".ToCharArray());
        }

        public SearchTicketsResponse SearchTickets(SearchTicketsRequest request)
        {
            int start = request.CurrentPage * request.PageCount + 1 - request.PageCount;
            int end = request.CurrentPage * request.PageCount;

            string strSelCount = " SELECT COUNT(1)  FROM DBO.GetTicketsView(@UserId) t ";
            string strOrderby = request.OrderBy.Replace("ProjectTitle", "p.Title")
                                                .Replace("CreatedOn", "t.CreatedOn")
                                                .Replace("ModifiedOn", "t.ModifiedOn")
                                                .Replace("CreatedBy", "u.FirstName")
                                                .Replace("Status", "t.Status")
                                                .Replace("Priority", "t.Priority")
                                                .Replace("TicketTitle", "t.Title")
                                                .Replace("TicketID", "t.TicketID")
                                                .Replace("ShowNotification", "t.ShowNotification");

            string strSelAttrs = @" SELECT t.*,tOrder.OrderNum,u.FirstName,u.LastName,
                                            p.Title AS ProjectTitle ,
                                            (dbo.GetPercentage(t.TicketID,@SheetDate))  AS Percentage 
                                            FROM  DBO.GetTicketsView(@LoginUserId)  t 
                                            left join TicketsOrder tOrder on t.TicketID=tOrder.TicketID 
                                            left join Users u on t.CreatedBy=u.UserID  
                                            left join Projects p on t.ProjectID =p.ProjectID
                                                ";
            string strSelAttrsOrderBy = string.Format(@" Order BY {0}  ", strOrderby);
            string strSelPageModel = string.Format(@"SELECT * FROM( 
                                                SELECT ROW_NUMBER() OVER( ORDER BY  {0}) AS INDEX_ID,
                                                  t.*,tOrder.OrderNum ,u.FirstName,u.LastName,
                                                    p.Title AS ProjectTitle , 
                                                    (dbo.GetPercentage(t.TicketID,@SheetDate))  AS Percentage
                                                    FROM  DBO.GetTicketsView(@LoginUserId)  t 
                                                    left join TicketsOrder tOrder on t.TicketID=tOrder.TicketID 
                                                    left join Users u on t.CreatedBy=u.UserID 
                                                    left join Projects p on t.ProjectID =p.ProjectID
                                                ", strOrderby);
            string strWherePageModel = @") NEW_TB  WHERE INDEX_ID BETWEEN @Strat AND  @End;";

            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" WHERE 1=1 ");
            switch (request.SearchType)
            {
                case SearchTicketsType.All:
                    break;
                case SearchTicketsType.TicketsListByPID:
                    strWhere.Append("  AND t.ProjectID=@ProjectID ");
                    break;
                case SearchTicketsType.Priority:
                    strWhere.Append(" AND t.ProjectID=@ProjectID ");
                    strWhere.Append(" AND t.IsInternal = 0 ");

                    List<TicketsState> listNoPriority = new List<TicketsState>();
                    listNoPriority.Add(TicketsState.Draft);
                    listNoPriority.Add(TicketsState.Cancelled);
                    listNoPriority.Add(TicketsState.Denied);
                    listNoPriority.Add(TicketsState.Completed);
                    strWhere.AppendFormat("  AND t.[Status] NOT IN({0}) ", GetStatesArray(listNoPriority));
                    break;
                case SearchTicketsType.TicketsForTimesheets:
                    strWhere.Append(" AND t.ProjectID=@ProjectID ");
                    strWhere.Append(@" AND ( exists(select 0 from [TicketUsers] tu where tu.[UserID]=@UserID and tu.[TicketID] = t.TicketID)
                                             or exists (select 0 from [Projects] proj where proj.[ProjectID]=t.[ProjectID] and proj.PMID = @UserID)
                                            ) 
                                                  
                                        ");
                    if (request.Status != null && request.Status.Count > 0)
                    {
                        List<TicketsState> listDateStatus = new List<TicketsState>();
                        listDateStatus.Add(TicketsState.Cancelled);
                        listDateStatus.Add(TicketsState.Denied);
                        listDateStatus.Add(TicketsState.Completed);

                        strWhere.AppendFormat(@"  AND( 
                                                        t.[Status] IN({0})
                                                        OR ( t.[Status] in({1}) AND t.ModifiedOn >= '{2}' ) 
                                                    )",
                                                  GetStatesArray(request.Status),
                                                  GetStatesArray(listDateStatus),
                                                  request.SheetDate.AddDays(-7).ToString("yyyy-MM-dd"));
                    }
                    break;
                case SearchTicketsType.CateGory:
                    strWhere.Append(@" AND EXISTS (
                        SELECT 'X' FROM CateGoryTicket CGT WHERE t.TicketID = CGT.TicketID AND CGT.CategoryID = @CategoryID)");
                    break;
                case SearchTicketsType.KnowledgeShare:
                    strWhere.Append(@" AND EXISTS (
                        SELECT 1 FROM CateGoryTicket CGT WHERE t.TicketID = CGT.TicketID AND CGT.CategoryID = @CategoryID) ");
                    if (request.ProjectID > 0)
                        strWhere.Append(" AND t.ProjectID = @ProjectID ");
                    if (request.Keyword != null && request.Keyword.Trim() != string.Empty)
                    {
                        int tmpTicketId = 0;
                        int.TryParse(request.Keyword, out tmpTicketId);
                        strWhere.AppendFormat(" AND (t.Title like '%{0}%' or t.TicketID = '{1}') "
                                , request.Keyword, tmpTicketId);
                    }
                    break;
                case SearchTicketsType.TicketsForReport:
                    strWhere.Append(" AND t.IsInternal = 0 ");
                    if (request.CompanyID > 0)
                    {
                        strWhere.Append(" AND t.projectid in(Select proj.ProjectID from Projects proj ")
                            .Append(" inner join dbo.ProjectUsers pu on proj.ProjectID = pu.ProjectID ")
                            .Append(" where proj.CompanyID=@CompanyID AND pu.UserID = @UserID ) AND t.CompanyID=@CompanyID");
                    }
                    if (request.Status != null && request.Status.Count > 0)
                    {
                        strWhere.AppendFormat(" AND t.Status in ({0})", GetStatesArray(request.Status));
                    }
                    if (request.SearchTicketID)
                    {
                        strWhere.AppendFormat(" AND t.TicketID in ({0})", request.TicketIDS);
                    }
                    if (request.ProjectID > 0)
                    {
                        strWhere.Append(" AND t.ProjectID=@ProjectID");
                    }
                    if (!request.TicketType.ToString().ToUpper().Equals("ALL") && request.TicketType != "-1")
                    {
                        strWhere.AppendFormat(" AND t.TicketType = '{0}' ", request.TicketType);
                    }

                    if (request.Keyword != null && request.Keyword.Trim() != string.Empty)
                    {
                        request.Keyword = request.Keyword.Trim();
                        int tmpTicketId = 0;
                        if (request.Keyword.StartsWith("b") || request.Keyword.StartsWith("B") || request.Keyword.StartsWith("R") || request.Keyword.StartsWith("r"))
                        {
                            int.TryParse(request.Keyword.Substring(1), out tmpTicketId);
                        }
                        else
                            int.TryParse(request.Keyword, out tmpTicketId);

                        strWhere.AppendFormat(" and (t.Title like '%{0}%' or t.TicketID = '{1}') "
                            , request.Keyword, tmpTicketId);
                    }

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
            SearchTicketsResponse response = new SearchTicketsResponse();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, request.ProjectID);
                    db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, request.CateGoryID);
                    db.AddInParameter(dbCommand, "SheetDate", DbType.DateTime, request.SheetDate);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, request.UserID);
                    db.AddInParameter(dbCommand, "LoginUserID", DbType.Int32, IdentityContext.UserID);
                    db.AddInParameter(dbCommand, "Keyword", DbType.String, request.Keyword.FilterSqlString());
                    db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, request.CompanyID);
                    db.AddInParameter(dbCommand, "Strat", DbType.Int32, start);
                    db.AddInParameter(dbCommand, "End", DbType.Int32, end);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        List<ExpandTicketsEntity> list = new List<ExpandTicketsEntity>();
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
                            list.Add(ExpandTicketsEntity.ExReaderBind(dataReader));
                        }
                        response.ResultList = list;
                        response.IsError = false;
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    response.IsError = true;
                }
            }
            return response;
        }

        public int Update(int ticketID, int star)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tickets set ");
            strSql.Append("Star=@Star");
            strSql.Append(" where TicketID=@TicketID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, ticketID);
                    db.AddInParameter(dbCommand, "Star", DbType.Int32, star);
                    int rows = db.ExecuteNonQuery(dbCommand);
                    UpdateIsRead(ticketID, TicketIsRead.Unread);
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

        #endregion

        #region ITicketsRepository Members

        #endregion

        #region  报表
        public DataTable SearchReortTickets(SearchTicketsRequest request, out int totalRows)
        {
            int start = request.CurrentPage * request.PageCount + 1 - request.PageCount;
            int end = request.CurrentPage * request.PageCount;
            totalRows = 0;
            string strSelCount = string.Empty;
            if (request.UserID > 0)
            {
                strSelCount = @" SELECT COUNT(1) FROM ( SELECT dbo.F_IsSearchedUserInReport(t.[TicketID],66) as isSearchedUser FROM  [Tickets] t  left join TicketsOrder tOrder on t.TicketID=tOrder.TicketID";
            }
            else
            {
                strSelCount = @" SELECT COUNT(1)  FROM  [Tickets] t  left join TicketsOrder tOrder on t.TicketID=tOrder.TicketID";
            }
            strSelCount += @" left join Users u on t.CreatedBy=u.UserID
			left join Projects p on t.ProjectID =p.ProjectID";
            string strOrderby = request.OrderBy.Replace("ProjectTitle", "p.Title")
                                                .Replace("CreatedOn", "t.CreatedOn")
                                                .Replace("ModifiedOn", "t.ModifiedOn")
                                                .Replace("CreatedBy", "u.FirstName")
                                                .Replace("Status", "t.Status")
                                                .Replace("Priority", "t.Priority")
                                                .Replace("TicketTitle", "t.Title")
                                                .Replace("TicketID", "t.TicketID");

            string strSelPageModel = "SELECT * FROM( ";
            if (request.UserID > 0)
            {
                strSelPageModel += string.Format(" SELECT ROW_NUMBER() OVER( ORDER BY  {0}) AS INDEX_ID ,* FROM ( ", strOrderby.Replace("t.", "").Replace("u.", "").Replace("p.", ""));
                strSelPageModel += @" SELECT p.Title AS ProjectTitle";
            }
            else
            {
                strSelPageModel += string.Format(@" SELECT ROW_NUMBER() OVER( ORDER BY  {0}) AS INDEX_ID ,p.Title AS ProjectTitle", strOrderby);
            }

            strSelPageModel += @",(t.[TicketCode]+Cast(t.[TicketID] as varchar)) as TicketCode
			                                        ,t.[Title]
			                                        ,ISNULL(t.[Priority],0) AS [Priority]
                                                    ,ISNULL(t.Description,'') AS [Description]
			                                        ,t.[TicketType] 
			                                        ,ISNULL(t.[Source],0) AS [Source]
			                                        ,dbo.GetUserNamesByTicket(t.[TicketID],'DEV')+dbo.GetUserNamesByTicket(t.[TicketID],'Leader') as DEV
			                                        ,dbo.GetUserNamesByTicket(t.[TicketID],'QA') as QA
                                                    ,ISNULL(t.[TicketID],0) as [TicketID]
			                                        ,t.[ProjectID]
                                                    ,t.[StartDate]";

            if (request.UserID > 0)
            {
                strSelPageModel += string.Format(",dbo.F_IsSearchedUserInReport(t.[TicketID],{0}) as isSearchedUser ", request.UserID);
            }
            strSelPageModel += @"FROM [Tickets] t left join TicketsOrder tOrder on t.TicketID=tOrder.TicketID 
                                                                  left join Users u on t.CreatedBy=u.UserID 
                                                                  left join Projects p on t.ProjectID =p.ProjectID
                                                ";
            string strWherePageModel = @") NEW_TB  WHERE INDEX_ID BETWEEN @Start AND  @End;";
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" WHERE 1=1 ");
            if (request.Status != null && request.Status.Count > 0)
            {
                strWhere.AppendFormat(" AND t.Status in ({0})", GetStatesArray(request.Status));
            }
            if (request.Source > 0)
            {
                strWhere.AppendFormat(" AND t.Source ={0} ", request.Source);
            }
            if (request.SearchTicketID && request.TicketIDS != "0")
            {
                strWhere.AppendFormat(" AND t.TicketID = {0}", request.TicketIDS);
            }
            //            if (request.UserID != 0)
            //            {
            //                strWhere.AppendFormat(@"  AND  EXISTS (SELECT TicketID FROM TicketUsers TS WHERE 1=1
            //						            AND TS.TicketID =t.TicketID 
            //						            AND TS.UserID = {0})", request.UserID);
            //            }
            if (request.ProjectID > 0)
            {
                strWhere.Append(" AND t.ProjectID=@ProjectID");
            }
            if (request.Star > -1)
            {
                strWhere.Append(" AND t.Star=@Star");//Star
            }
            if (!request.TicketType.ToString().ToUpper().Equals("ALL") && request.TicketType != "-1")
            {
                strWhere.AppendFormat(" AND t.TicketType = '{0}' ", request.TicketType);
            }
            if (request.Keyword != null && request.Keyword.Trim() != string.Empty)
            {
                request.Keyword = request.Keyword.Trim();
                string[] fields = { "p.Title", "t.Title" };
                strWhere.Append(SplitKeywords(request.Keyword, "t.TicketID", fields));
            }
            strWhere.AppendFormat(" AND t.StartDate >= '{0}' ", request.StartDate);
            strWhere.AppendFormat(" AND t.StartDate <= '{0}' ", request.EndDate);
            StringBuilder strSql = new StringBuilder();
            strSql.Append(strSelCount);
            strSql.Append(strWhere);
            if (request.UserID > 0)
            {
                strSql.Append(" ) T WHERE isSearchedUser>0");
            }
            strSql.Append(";");
            strSql.Append(strSelPageModel);
            strSql.Append(strWhere);
            if (request.UserID > 0)
            {
                strSql.Append(") T WHERE isSearchedUser>0 ");
            }
            strSql.Append(strWherePageModel);
            SearchTicketsResponse response = new SearchTicketsResponse();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, request.ProjectID);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, request.UserID);
                    db.AddInParameter(dbCommand, "Keyword", DbType.String, request.Keyword.FilterSqlString());
                    db.AddInParameter(dbCommand, "Start", DbType.Int32, start);
                    db.AddInParameter(dbCommand, "End", DbType.Int32, end);
                    db.AddInParameter(dbCommand, "Star", DbType.Int32, request.Star);
                    DataSet ds = db.ExecuteDataSet(dbCommand);

                    int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out totalRows);
                    return ds.Tables[1];
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    response.IsError = true;
                }
            }
            return new DataTable();
        }

        public DataTable ReortTicketRating(SearchTicketsRequest request, out int totalRows)
        {
            totalRows = 0;
            string strOrderby = request.OrderBy;
            StringBuilder strDataSource = new StringBuilder();
            strDataSource.Append(@"
                                  Select  projectId,projectTitle,TicketType,[0],[1],[2],[3],[4],[5]
                                   FROM (
	                                   SELECT t.projectId,p.Title as ProjectTitle,TicketType,Star,TicketId FROM Tickets t  
		                                     JOIN Projects p ON t.ProjectID = p.ProjectID    
                                        ");
            strDataSource.Append(" WHERE 1=1 ");

            if (request.ProjectID > 0)
                strDataSource.Append(" And t.ProjectID=@ProjectID");
            if (request.SearchTicketID && request.TicketIDS != "0")
            {
                strDataSource.AppendFormat(" AND t.TicketID = {0}", request.TicketIDS);
            }
            if (request.StartDate != null && request.StartDate > DateTime.MinValue)
                strDataSource.Append(" And t.StartDate >= @StartDate");
            if (request.EndDate != null && request.EndDate > DateTime.MinValue)
                strDataSource.Append(" And t.StartDate <= @EndDate");

            if (request.Keyword != null && request.Keyword.Trim() != string.Empty)
            {
                request.Keyword = request.Keyword.Trim();
                string[] fields = { "p.Title", "t.Title" };
                strDataSource.Append(SplitKeywords(request.Keyword, "t.TicketID", fields));
            }

            //if (!string.IsNullOrEmpty(request.Keyword))
            //    strDataSource.Append(" And (p.Title like @Keyword or t.Title like @Keyword) ");


            if (!request.TicketType.ToString().ToUpper().Equals("ALL") && request.TicketType != "-1")
            {
                strDataSource.AppendFormat(" AND t.TicketType = '{0}' ", request.TicketType);
            }


            strDataSource.Append(@"	) AS Tickets  pivot (  COUNT(TicketId) FOR Star IN([0],[1],[2],[3],[4],[5]) ) AS ourPivot");

            if (!request.IsPageModel)
                strDataSource.AppendFormat(" Order by {0} {1} ",
                    strOrderby, "ASC");

            StringBuilder strSql = new StringBuilder();
            if (request.IsPageModel)
            {
                strSql.Append(" Select count(1) from ");
                strSql.Append("(");
                strSql.Append(strDataSource);
                strSql.Append(") as SearchResult ;");
                strSql.Append(" Select * from (");
                strSql.AppendFormat("  Select ROW_NUMBER() OVER( Order BY {0} {1}) as  INDEX_ID,SearchResult.* From "
                    , strOrderby, "ASC");
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
                    db.AddInParameter(dbCommand, "Keyword", DbType.String, string.Format("%{0}%", request.Keyword.FilterSqlString()));
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
        #endregion

        public List<TicketsEntity> GetTicketsByWorkingStatus(int userid, Entity.TicketModel.Enums.TicketUserStatus status)
        {
            string strSql = @"Select P.[ProjectCode],[ProjectTitle]= P.[Title], T.* , F.*
                                FROM  (SELECT * FROM DBO.GetTicketsView(@UserID)) T LEFT JOIN [Projects] P ON T.[ProjectID] = P.[ProjectID]
                                left join dbo.Files F on F.TicketId = T.TicketId
                                WHERE EXISTS(SELECT 1 FROM [TicketUsers] TU WHERE TU.[TicketID] = T.[TicketID] AND TU.Status = @Status AND TU.UserID = @UserID)
                                ORDER BY ModifiedOn DESC";

            List<TicketsEntity> list = new List<TicketsEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "Status", DbType.Int32, (int)status);
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, userid);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        list.Add(TicketsEntity.ReaderBind(dataReader, true));
                    }
                }
                return list;
            }
        }

        public List<TicketsEntity> GetTicketsByCreateId(int createid)
        {
            string strSql = @"Select P.[ProjectCode],[ProjectTitle]= P.[Title],P.[ProjectID], T.* 
                                FROM [Tickets] T LEFT JOIN [Projects] P ON T.[ProjectID] = P.[ProjectID] where T.[CreatedBy]=@CreatedBy
                                ORDER BY T.[CreatedOn] DESC";

            List<TicketsEntity> list = new List<TicketsEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, createid);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        list.Add(TicketsEntity.ReaderBind(dataReader, true));
                    }
                }
                return list;
            }
        }

        public List<TicketsEntity> SearchTickets(Entity.TicketModel.TicketsDTO.SearchTicketCondition condition)
        {
            var baseTicketsView = " DBO.GetTicketsView(@UserId) ";
            var orderby = string.Empty;
            switch (condition.OrderBy)
            {
                case "ProjectTitle":
                    orderby = string.Format("P.Title {0}", condition.OrderDirection);
                    break;
                case "CreatedBy":
                    orderby = string.Format("T.FirstName {0}", condition.OrderDirection);
                    break;
                case "TicketTitle":
                    orderby = string.Format("T.Title {0}", condition.OrderDirection);
                    break;
                case "CreatedOn":
                case "ModifiedOn":
                case "Status":
                case "Priority":
                case "Title":
                case "TicketID":
                case "TicketType":
                case "CreatedByFirstName":
                    orderby = string.Format("T.{0} {1}", condition.OrderBy, condition.OrderDirection);
                    break;
                case "ShowNotification":
                    orderby = string.Format("T.ShowNotification {0} , T.ModifiedOn desc", condition.OrderDirection);
                    break;
                case "ResponsibleUserName":
                    orderby = string.Format("(Ru.FirstName+' '+Ru.LastName) {0}", condition.OrderDirection);
                    break;
            }

            var leftJoin = @" left join TicketsOrder tOrder on T.TicketID = tOrder.TicketID  
                                                    left join Users U on T.CreatedBy = U.UserID 
                                                    left join Projects P on T.ProjectID = P.ProjectID 
                                                    left join dbo.Users Ru ON t.ResponsibleUser=Ru.UserID";
            var where = " 1 = 1 ";
            if (condition.SearchMyTicket)
            {
                where += " AND T.CreatedBy = @UserId ";
            }
            if (condition.SearchIsInternal)
            {
                where += string.Format(" AND T.IsInternal = {0} ", condition.IsInternal ? "1" : "0");
            }
            if (condition.SearchKeyword)
            {
                int tmpTicketId = 0;
                int.TryParse(condition.Keyword, out tmpTicketId);
                where += string.Format(" AND (t.Title like '%{0}%' or t.TicketID = '{1}') "
                        , condition.Keyword.FilterSqlString(), tmpTicketId);
            }
            if (condition.SearchPriority)
            {
                where += string.Format(" AND T.Priority = {0}", (int)condition.Priority);
            }

            if (condition.SearchCompany)
            {
                if (condition.CompanyId!=0)
                {
                    where += string.Format(" AND P.CompanyID = {0}", condition.CompanyId);
                }
            }

            where += " AND T.ProjectID IN (SELECT ProjectID FROM ProjectUsers PU WHERE PU.UserID = @UserId ) ";
            if (condition.SearchProject)
            {
                where += string.Format(" AND T.ProjectID = {0}", condition.ProjectId);
            }

            if (condition.SearchStatus)
            {
                where += string.Format(" AND T.Status = {0}", (int)condition.Status);
            }
            if (condition.SearchStatusRange)
            {
                if (condition.Statuses.Any())
                {
                    where += string.Format(" AND T.Status IN ({0})",
                    string.Join(",", condition.Statuses.Select(x => (int)x).ToList()));
                }
            }
            if (condition.SearchMultiStatus)
            {
                if (condition.MultiStaus.Any())
                {
                    where += string.Format(" AND T.Status IN ({0})",
                    string.Join(",", condition.MultiStaus.Select(x => x).ToList()));
                }
            }

            if (condition.SearchType)
            {
                where += string.Format(" AND T.TicketType = '{0}'", condition.Type);
            }

            if (condition.SearchCurrentUser)
            {
                where += string.Format(" AND (T.TicketID IN (select TicketID from dbo.TicketUsers TU where TU.UserID = {0} ) OR T.ConfirmEstmateUserId={0} )",
                        condition.UserId);
            }
            if (condition.SearchAssignUser)
            {
                where += string.Format(" AND (T.TicketID IN (select TicketID from dbo.TicketUsers TU where TU.UserID = {0} ) OR T.ConfirmEstmateUserId={0} ) ",
                    condition.AssignUserId);
            }
            if (condition.SearchResponsibleUser)
            {
                where += $" AND T.ResponsibleUser = '{condition.ResponsibleUserId}' ";
            }
            if (condition.SearchCreate)
            {
                where += string.Format(" AND (T.CreatedByFirstName like '%{0}%' OR T.CreatedByLastName like '%{0}%') ", condition.CreateUser.Trim().FilterSqlString());
            }
            if (condition.SearchCreatedStart && condition.CreateStartTime != null)
            {
                where += string.Format(" AND (T.CreatedOn >= '{0}') ", condition.CreateStartTime.Value.ToString("yyyy-MM-dd 00:00:00").FilterSqlString());
            }
            if (condition.SearchCreatedEnd && condition.CreateEndTime != null)
            {
                where += string.Format(" AND (T.CreatedOn <= '{0}') ", condition.CreateEndTime.Value.ToString("yyyy-MM-dd 23:59:59").FilterSqlString());
            }

            var orderView = string.Format("SELECT ROW_NUMBER() OVER( ORDER BY  {0}) AS INDEX_ID,T.*, ProjectTitle = P.Title , P.ProjectCode,(Ru.FirstName+' '+Ru.LastName) AS ResponsibleUserName FROM {1} T {2} WHERE {3} ",
                orderby, baseTicketsView, leftJoin, where);
            var dataView = string.Format("SELECT * FROM ({0}) OrderedView WHERE INDEX_ID BETWEEN @Start AND  @End;", orderView);
            var countView = string.Format("Select count (1) from {0} T {1} WHERE {2}", baseTicketsView, leftJoin, where);
            var strFinal = (condition.OnlyCount ? "" : dataView) + countView;
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strFinal))
            {
                try
                {
                    int start = condition.CurrentPage * condition.PageCount + 1 - condition.PageCount;
                    int end = condition.CurrentPage * condition.PageCount;
                    db.AddInParameter(dbCommand, "UserId", DbType.Int32, condition.UserId);
                    db.AddInParameter(dbCommand, "Start", DbType.Int32, start);
                    db.AddInParameter(dbCommand, "End", DbType.Int32, end);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        List<TicketsEntity> list = new List<TicketsEntity>();
                        if (!condition.OnlyCount)
                        {
                            while (dataReader.Read())
                            {
                                list.Add(TicketsEntity.ReaderBind(dataReader, true));
                            }
                            dataReader.NextResult();
                        }
                        if (dataReader.Read())
                        {
                            condition.TotalRecords = dataReader.GetInt32(0);
                        }
                        return list;
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0}\r\n{1}Messages:\r\n{2}]"
                        , strFinal, base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        public bool UpdateConfirmEstmateUserId(int ticketId, int userId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = string.Format("update Tickets set ConfirmEstmateUserId={0} where TicketID={1}"
                , userId, ticketId);
            using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
            {
                UpdateIsRead(ticketId, TicketIsRead.Unread);
                return db.ExecuteNonQuery(dbCommand) > 0;
            }
        }

        public List<TicketsEntity> GetTicketsByIds(List<int> ticketIds)
        {
            if (ticketIds.Count > 0)
            {
                string strSql = "Select * From Tickets Where TicketID in ( ";
                foreach (int item in ticketIds)
                {
                    strSql += item + ",";
                }
                strSql = strSql.TrimEnd(',') + " )";
                List<TicketsEntity> list = new List<TicketsEntity>();
                Database db = DatabaseFactory.CreateDatabase();
                using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            list.Add(TicketsEntity.ReaderBind(dataReader, true));
                        }
                    }
                    return list;
                }
            }
            else
            {
                return new List<TicketsEntity>();
            }
        }









        public List<TicketsEntity> SearchTicketsNotInTid(Entity.TicketModel.TicketsDTO.SearchTicketCondition condition)
        {
            var baseTicketsView = " DBO.GetTicketsView(@UserId) ";
            var orderby = string.Empty;
            switch (condition.OrderBy)
            {
                case "ProjectTitle":
                    orderby = string.Format("P.Title {0}", condition.OrderDirection);
                    break;
                case "CreatedBy":
                    orderby = string.Format("T.FirstName {0}", condition.OrderDirection);
                    break;
                case "TicketTitle":
                    orderby = string.Format("T.Title {0}", condition.OrderDirection);
                    break;
                case "CreatedOn":
                case "ModifiedOn":
                case "Status":
                case "Priority":
                case "Title":
                case "TicketID":
                case "TicketType":
                case "CreatedByFirstName":
                    orderby = string.Format("T.{0} {1}", condition.OrderBy, condition.OrderDirection);
                    break;
                case "ShowNotification":
                    orderby = string.Format("T.ShowNotification {0} , T.ModifiedOn desc", condition.OrderDirection);
                    break;
            }

            var leftJoin = @" left join TicketsOrder tOrder on T.TicketID = tOrder.TicketID  
                                                    left join Users U on T.CreatedBy = U.UserID 
                                                    left join Projects P on T.ProjectID = P.ProjectID ";
            var where = " 1 = 1 ";
            if (condition.SearchIsInternal)
            {
                where += string.Format(" AND T.IsInternal = {0} ", condition.IsInternal ? "1" : "0");
            }
            if (condition.SearchKeyword)
            {
                int tmpTicketId = 0;
                int.TryParse(condition.Keyword, out tmpTicketId);
                where += string.Format(" AND (t.Title like '%{0}%' or t.TicketID = '{1}') "
                        , condition.Keyword.FilterSqlString(), tmpTicketId);
            }
            if (condition.SearchPriority)
            {
                where += string.Format(" AND T.Priority = {0}", (int)condition.Priority);
            }

            if (condition.SearchCompany)
            {
                where += string.Format(" AND P.CompanyID = {0}", condition.CompanyId);
            }

            where += " AND T.ProjectID  IN (SELECT ProjectID FROM ProjectUsers PU WHERE PU.UserID = @UserId ) ";
            if (condition.SearchProject)
            {
                where += string.Format(" AND T.ProjectID = {0}", condition.ProjectId);
            }

            if (condition.SearchStatus)
            {
                where += string.Format(" AND T.Status = {0}", (int)condition.Status);
            }
            if (condition.SearchStatusRange)
            {
                where += string.Format(" AND T.Status IN ({0})",
                    string.Join(",", condition.Statuses.Select(x => (int)x).ToList()));
            }

            if (condition.SearchType)
            {
                where += string.Format(" AND T.TicketType = '{0}'", condition.Type);
            }

            if (condition.SearchCurrentUser)
            {
                where += string.Format(" AND (T.TicketID IN (select TicketID from dbo.TicketUsers TU where TU.UserID = {0} ) OR T.ConfirmEstmateUserId={0} )",
                        condition.UserId);
            }
            if (condition.SearchAssignUser)
            {
                where += string.Format(" AND (T.TicketID IN (select TicketID from dbo.TicketUsers TU where TU.UserID = {0} ) OR T.ConfirmEstmateUserId={0} ) ",
                    condition.AssignUserId);
            }
            if (condition.SearchCreate)
            {
                where += string.Format(" AND (T.CreatedByFirstName like '%{0}%' OR T.CreatedByLastName like '%{0}%') ", condition.CreateUser.Trim().FilterSqlString());
            }
            if (condition.SearchCreatedStart && condition.CreateStartTime != null)
            {
                where += string.Format(" AND (T.CreatedOn >= '{0}') ", condition.CreateStartTime.Value.ToString("yyyy-MM-dd 00:00:00").FilterSqlString());
            }
            if (condition.SearchCreatedEnd && condition.CreateEndTime != null)
            {
                where += string.Format(" AND (T.CreatedOn <= '{0}') ", condition.CreateEndTime.Value.ToString("yyyy-MM-dd 23:59:59").FilterSqlString());
            }

            where += " AND T.TicketID NOT IN (select TID from dbo.ProposalTrackerRelation) ";

            var orderView = string.Format("SELECT ROW_NUMBER() OVER( ORDER BY  {0}) AS INDEX_ID,T.*, ProjectTitle = P.Title , P.ProjectCode FROM {1} T {2} WHERE {3} ",
                orderby, baseTicketsView, leftJoin, where);
            var dataView = string.Format("SELECT * FROM ({0}) OrderedView WHERE INDEX_ID BETWEEN @Start AND  @End;", orderView);
            var countView = string.Format("Select count (1) from {0} T {1} WHERE {2}", baseTicketsView, leftJoin, where);
            var strFinal = (condition.OnlyCount ? "" : dataView) + countView;
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strFinal))
            {
                try
                {
                    int start = condition.CurrentPage * condition.PageCount + 1 - condition.PageCount;
                    int end = condition.CurrentPage * condition.PageCount;
                    db.AddInParameter(dbCommand, "UserId", DbType.Int32, condition.UserId);
                    db.AddInParameter(dbCommand, "Start", DbType.Int32, start);
                    db.AddInParameter(dbCommand, "End", DbType.Int32, end);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        List<TicketsEntity> list = new List<TicketsEntity>();
                        if (!condition.OnlyCount)
                        {
                            while (dataReader.Read())
                            {
                                list.Add(TicketsEntity.ReaderBind(dataReader, true));
                            }
                            dataReader.NextResult();
                        }
                        if (dataReader.Read())
                        {
                            condition.TotalRecords = dataReader.GetInt32(0);
                        }
                        return list;
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0}\r\n{1}Messages:\r\n{2}]"
                        , strFinal, base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }
    }
}

