using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.InvoiceModule.Interface;
using SunNet.PMNew.Entity.InvoiceModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.InvoiceModel.Proposal;
using SunNet.PMNew.Entity.InvoiceModel.Enums;

namespace SunNet.PMNew.Impl.SqlDataProvider.Invoice
{
    public class InvoiceRepositorySqlDataProvider : SqlHelper, IinvoiceRepository
    {
        public InvoiceRepositorySqlDataProvider()
        { }

        public int Insert(InvoiceEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO [Invoices](");
            strSql.Append("[ProposalId],[Milestone],[Approved],[Status],[InvoiceNo],[SendOn],[DueOn],[ReceiveOn],[Notes]," +
                          "[Color],[ColorFor],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[ETADate])");

            strSql.Append(" values (");
            strSql.Append("@ProposalId,@Milestone,@Approved,@Status,@InvoiceNo,@SendOn,@DueOn,@ReceiveOn,@Notes," +
                          "@Color,@ColorFor,@CreatedOn,@CreatedBy,@ModifiedOn,@ModifiedBy,@ETADate)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProposalId", DbType.Int32, entity.ProposalId);
                    db.AddInParameter(dbCommand, "Milestone", DbType.Int32, entity.Milestone);
                    db.AddInParameter(dbCommand, "Approved", DbType.Int32, entity.Approved);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, entity.Status);
                    db.AddInParameter(dbCommand, "InvoiceNo", DbType.String, entity.InvoiceNo);
                    db.AddInParameter(dbCommand, "SendOn", DbType.DateTime, entity.SendOn);
                    db.AddInParameter(dbCommand, "DueOn", DbType.DateTime, entity.DueOn);
                    db.AddInParameter(dbCommand, "ReceiveOn", DbType.DateTime, entity.ReceiveOn);
                    db.AddInParameter(dbCommand, "Notes", DbType.String, entity.Notes);
                    db.AddInParameter(dbCommand, "Color", DbType.String, entity.Color);
                    db.AddInParameter(dbCommand, "ColorFor", DbType.String, entity.ColorFor);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, DateTime.Now);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, entity.CreatedBy);
                    db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, DateTime.Now);
                    db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, entity.ModifiedBy);
                    db.AddInParameter(dbCommand, "ETADate", DbType.DateTime, entity.ETADate);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return 0;
                }
            }
        }

        public bool Update(InvoiceEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  UPDATE [Invoices] SET [ProposalId]=@ProposalId");
            strSql.Append(" ,[Milestone]=@Milestone");
            strSql.Append(" ,[Approved]=@Approved");
            strSql.Append(",[Status]=@Status");
            strSql.Append(" ,[InvoiceNo]=@InvoiceNo");
            strSql.Append(" ,[SendOn]=@SendOn");
            strSql.Append(" ,[DueOn]=@DueOn");
            strSql.Append(",[ReceiveOn]=@ReceiveOn");
            strSql.Append(",[Notes]=@Notes,");
            strSql.Append("Color=@Color,");
            strSql.Append("ColorFor=@ColorFor,");
            strSql.Append("ModifiedOn=@ModifiedOn,");
            strSql.Append("ModifiedBy=@ModifiedBy,");
            strSql.Append("ETADate=@ETADate");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProposalId", DbType.Int32, entity.ProposalId);
                    db.AddInParameter(dbCommand, "Milestone", DbType.Int32, entity.Milestone);
                    db.AddInParameter(dbCommand, "Approved", DbType.Int32, entity.Approved);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, entity.Status);
                    db.AddInParameter(dbCommand, "InvoiceNo", DbType.String, entity.InvoiceNo);
                    db.AddInParameter(dbCommand, "SendOn", DbType.DateTime, entity.SendOn);
                    db.AddInParameter(dbCommand, "DueOn", DbType.DateTime, entity.DueOn);
                    db.AddInParameter(dbCommand, "ReceiveOn", DbType.DateTime, entity.ReceiveOn);
                    db.AddInParameter(dbCommand, "Notes", DbType.String, entity.Notes);
                    db.AddInParameter(dbCommand, "Color", DbType.String, entity.Color);
                    db.AddInParameter(dbCommand, "ColorFor", DbType.String, entity.ColorFor);
                    db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, DateTime.Now);
                    db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, entity.ModifiedBy);
                    db.AddInParameter(dbCommand, "ETADate", DbType.DateTime, entity.ETADate);
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entity.ID);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        public bool Delete(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  DELETE [Invoices] WHERE ID=@ID");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        public InvoiceEntity Get(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * from [Invoices]");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);
                InvoiceEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        if (dataReader.Read())
                        {
                            model = InvoiceEntity.ReaderBind(dataReader);
                        }
                    }
                    catch (Exception ex)
                    {
                        WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                            , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                        return null;
                    }
                }
                return model;
            }
        }

        /// <summary>
        /// add timesheet invoice page use this method
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public ProposalInvoiceModel GetInvoiceModelById(int entityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  Invoices.* ,");
            strSql.Append("ProposalTracker.Title AS ProposalTitle,");
            strSql.Append("ProposalTracker.PONo AS PONo,");
            strSql.Append("Projects.ProjectID AS ProjectId,");
            strSql.Append("Projects.Title AS ProjectTitle,");
            strSql.Append("Companys.ComID AS CompanyId,");
            strSql.Append("Companys.CompanyName AS CompanyName, ");
            strSql.Append("TS.Hours AS [HOURS]");
            strSql.Append("FROM Invoices AS Invoices ");
            strSql.Append("LEFT JOIN dbo.ProposalTracker AS ProposalTracker ON ProposalTracker.ProposalTrackerID = Invoices.ProposalId ");
            strSql.Append("LEFT JOIN dbo.Projects AS Projects ON ProposalTracker.ProjectID = Projects.ProjectID ");
            strSql.Append("LEFT JOIN dbo.Companys AS Companys ON Projects.CompanyID = Companys.ComID ");
            strSql.Append("LEFT JOIN dbo.TSInvoiceRelation AS TR ON Invoices.ID = TR.InvoiceId ");
            strSql.Append("LEFT JOIN dbo.TimeSheets AS TS ON TR.TSId = TS.ID ");
            strSql.Append(" where Invoices.ID=@entityId ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "entityId", DbType.Int32, entityId);
                ProposalInvoiceModel model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        if (dataReader.Read())
                        {
                            model = ProposalInvoiceModel.ReaderBind(dataReader);
                        }
                    }
                    catch (Exception ex)
                    {
                        WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                            , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                        return null;
                    }
                }
                return model;
            }
        }

        /// <summary>
        /// TO DO page use this method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SearchInvoiceResponse SearchProposalInvoice(SearchInvoiceRequest request)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append(@"select P.ProjectId,P.Title as ProjectTitle,PT.Title as ProposalTrackerTitle ,PT.PONo, Invoices.Milestone,Invoices.InvoiceNo,
                            Invoices.Status, PT.ProposalTrackerID,Invoices.ID as invoiceId from ProposalTracker PT
                            LEFT JOIN Projects P on  PT.ProjectID = p.ProjectId
                            LEFT JOIN Invoices ON Invoices.ProposalId = PT.ProposalTrackerID ");

            sqlStr.Append("WHERE 1=1 ");

            sqlStr.Append("AND (");
            sqlStr.Append("P.CompanyID != (select ComID from Companys where CompanyName='Sunnet')");
            sqlStr.Append(")");

            if (request.Keywords != "")
            {
                sqlStr.Append("AND (");
                sqlStr.Append("Invoices.InvoiceNo LIKE @Keywords ");
                sqlStr.Append("OR P.Title LIKE @Keywords ");
                sqlStr.Append("OR PT.Title LIKE @Keywords ");
                sqlStr.Append(") ");
            }
            if (request.ProjectId != 0)
            {
                sqlStr.Append("AND P.ProjectID=@ProjectId ");
            }
            if (request.OrderExpression != "")
            {
                sqlStr.AppendFormat("ORDER BY {0} ", request.OrderExpression);
            }
            else
            {
                sqlStr.Append("ORDER BY P.Title ");
            }

            if (request.OrderDirection != "")
            {
                sqlStr.Append(request.OrderDirection);
            }
            else
            {
                sqlStr.Append("DESC;");
            }
            List<ProposalToDoModel> list;
            SearchInvoiceResponse response = new SearchInvoiceResponse();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sqlStr.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Keywords", DbType.String, string.Format("%{0}%", request.Keywords.FilterSqlString()));
                    db.AddInParameter(dbCommand, "ProjectId", DbType.Int32, request.ProjectId);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        list = new List<ProposalToDoModel>();
                        while (dataReader.Read())
                        {
                            list.Add(ProposalToDoModel.ReaderBind(dataReader));
                        }
                        response.ProposalList = list;
                        response.ResultCount = list.Count;
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                                           sqlStr.ToString(),
                                           base.FormatParameters(dbCommand.Parameters),
                                           ex.Message));
                }
            }
            return response;
        }

        /// <summary>
        /// invoice detail use this method
        /// </summary>
        /// <param name="proposalId"></param>
        /// <returns></returns>
        public List<InvoiceEntity> GetInvoiceByProposalId(int proposalId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ISNULL(i.ID,0) as ID,i.ProposalId as ProposalId, ");
            strSql.Append("i.Milestone as Milestone, ");
            strSql.Append("i.Approved as Approved, ");
            strSql.Append("i.Status as Status, ");
            strSql.Append("i.InvoiceNo as InvoiceNo, ");
            strSql.Append("i.SendOn as SendOn, ");
            strSql.Append("i.DueOn as DueOn, ");
            strSql.Append("i.ReceiveOn as ReceiveOn, ");
            strSql.Append("ISNULL(i.Notes,'') as Notes, ");
            strSql.Append("i.Color,i.ColorFor,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,i.ETADate as ETADate ");
            strSql.Append("from Invoices i ");
            strSql.Append("where ProposalId=@proposalId order by Milestone asc ");

            List<InvoiceEntity> list = new List<InvoiceEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "proposalId", DbType.Int32, proposalId);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    try
                    {
                        while (dataReader.Read())
                        {
                            list.Add(InvoiceEntity.ReaderBind(dataReader));
                        }
                    }
                    catch (Exception ex)
                    {
                        WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                            , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                        return null;
                    }
                }
                return list;
            }
        }

        public SearchInvoiceResponse SearchTimesheetInvoice(SearchInvoiceRequest request)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("SELECT ts.ID,[SheetDate] ");
            sqlStr.Append("	,c.CompanyName,p.Title AS ProjectTitle,ts.[TicketID]");
            sqlStr.Append("	 ,t.Title AS TicketTitle,u.FirstName,[Hours]");
            sqlStr.Append("  FROM [PM].[dbo].[TimeSheets] ts ");
            sqlStr.Append("  LEFT JOIN dbo.Tickets t ON ts.TicketID =t.TicketID ");
            sqlStr.Append("  LEFT JOIN dbo.Users u ON  u.UserID=ts.CreatedBy ");
            sqlStr.Append("  LEFT JOIN dbo.Companys c ON c.ComID=t.CompanyID ");
            sqlStr.Append("  LEFT JOIN dbo.Projects p ON ts.ProjectID=p.ProjectID ");

            sqlStr.Append("WHERE 1=1 and p.ProjectID IN (" + request.ProjectIds + ") " +
                          " and not exists (select * from TSInvoiceRelation where TSId=ts.id)");
            if (request.timeTsheetIDs != "" && request.timeTsheetIDs!=null)
            {
                sqlStr.Append("and ts.ID IN (" + request.timeTsheetIDs + ")");
            }
            if (request.CompanyId > 0)
            {
                sqlStr.Append("and c.ComID = @CompanyID ");
            }
            if (request.OrderExpression != "" && request.OrderExpression != null)
            {
                sqlStr.AppendFormat("ORDER BY {0} ", request.OrderExpression);
            }
            else
            {
                sqlStr.Append("ORDER BY CompanyName ");
            }

            if (request.OrderDirection != "" && request.OrderDirection != null)
            {
                sqlStr.Append(request.OrderDirection);
            }
            else
            {
                sqlStr.Append("DESC;");
            }
            List<TimesheetInvoiceModel> list;
            SearchInvoiceResponse response = new SearchInvoiceResponse();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sqlStr.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "OrderExpression", DbType.String, request.OrderExpression);
                    db.AddInParameter(dbCommand, "OrderDirection", DbType.String, request.OrderDirection);
                    // db.AddInParameter(dbCommand, "ProjectIDs", DbType.String, "("+request.ProjectIds+")");
                    db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, request.CompanyId);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        list = new List<TimesheetInvoiceModel>();
                        while (dataReader.Read())
                        {
                            list.Add(TimesheetInvoiceModel.ReaderBind(dataReader));
                        }
                        response.TimesheetList = list;
                        response.TimesheetCount = list.Count;
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                                           sqlStr.ToString(),
                                           base.FormatParameters(dbCommand.Parameters),
                                           ex.Message));
                }
            }
            return response;
        }

        /// <summary>
        /// All Invoices page,Pass Due page,Awaiting Payment page use this method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SearchInvoiceResponse SearchInvoices(SearchInvoiceRequest request)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append(@"SELECT i.ID,i.ProposalId,i.InvoiceNo,i.Milestone Milestone,c.CompanyName,i.SendOn,i.DueOn,i.ReceiveOn,i.Status,
                            (case when i.ProposalId>0 then (select SUM(Hours) from TimeSheets where ticketid in 
                            (select TID from ProposalTrackerRelation where wid=i.ProposalId)) 
                            else sum(ts.Hours) end) [Hours],
                            i.Notes,pt.Title AS ProposalTitle,pt.PONo,p.Title as ProjectTitle
                            FROM Invoices i 
                            LEFT JOIN dbo.TSInvoiceRelation tsr ON i.ID=tsr.InvoiceId 
                            LEFT JOIN dbo.TimeSheets ts ON tsr.TSId=ts.ID 
                            LEFT JOIN dbo.ProposalTracker pt ON pt.ProposalTrackerID=i.ProposalId 
                            LEFT JOIN dbo.Projects p ON (p.ProjectID=ts.ProjectID or p.ProjectID=pt.ProjectID)
                            LEFT JOIN dbo.Companys c ON p.CompanyID=c.ComID ");
            sqlStr.Append("WHERE 1=1 ");
            switch (request.Searchtype)
            {
                case InvoiceSearchType.ProposalOnly:
                    sqlStr.Append("AND i.ProposalId !=0 ");
                    break;
                case InvoiceSearchType.AwitingPayment:
                    sqlStr.Append("AND i.Status =" + (int)InvoiceStatus.Awaiting_Payment + " ");
                    break;
                case InvoiceSearchType.Payment_Received:
                    sqlStr.Append("AND i.Status =" + (int)InvoiceStatus.Payment_Received + " ");
                    break;
                case InvoiceSearchType.PassDue:
                    sqlStr.Append("AND i.DueOn <  (select getdate()-1 ) AND i.Status <=5 ");
                    break;
                case InvoiceSearchType.All:
                    break;
            }
            if (request.Keywords != "")
            {
                sqlStr.Append("AND (");
                sqlStr.Append("i.InvoiceNo LIKE @Keywords ");
                sqlStr.Append("OR p.Title LIKE @Keywords ");
                //sqlStr.Append("OR ProposalTracker.Title LIKE %@Keywords% ");
                sqlStr.Append(") ");
            }
            if (request.CompanyId != 0)
            {
                sqlStr.Append("AND c.ComID=@CompanyId ");
            }
            if ((int)request.InvoiceStatus != 0)
            {
                sqlStr.Append("AND i.Status=@Status ");
            }
            if (request.ProjectId != 0)
            {
                sqlStr.Append("AND p.ProjectId=@ProjectId ");
            }

            sqlStr.Append(" GROUP BY i.ID,i.ProposalId,i.InvoiceNo,i.SendOn,i.DueOn,i.ReceiveOn,i.Status,c.CompanyName,i.Notes,pt.Title,i.Milestone,pt.PONo,p.Title  ");
            if (request.OrderExpression != "")
            {
                sqlStr.AppendFormat("ORDER BY {0} ", request.OrderExpression);
            }
            else
            {
                sqlStr.Append("ORDER BY p.Title ");
            }

            if (request.OrderDirection != "")
            {
                sqlStr.Append(request.OrderDirection);
            }
            else
            {
                sqlStr.Append("DESC;");
            }
                            
            List<ProposalInvoiceModel> list;
            SearchInvoiceResponse response = new SearchInvoiceResponse();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sqlStr.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Keywords", DbType.String, string.Format("%{0}%", request.Keywords.FilterSqlString()));
                    db.AddInParameter(dbCommand, "ProjectId", DbType.Int32, request.ProjectId);
                    db.AddInParameter(dbCommand, "CompanyId", DbType.Int32, request.CompanyId);
                    db.AddInParameter(dbCommand, "OrderExpression", DbType.String, request.OrderExpression);
                    db.AddInParameter(dbCommand, "OrderDirection", DbType.String, request.OrderDirection);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, request.InvoiceStatus);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        list = new List<ProposalInvoiceModel>();
                        while (dataReader.Read())
                        {
                            list.Add(ProposalInvoiceModel.ReaderBind(dataReader));
                        }
                        response.ResultList = list;
                        response.ResultCount = list.Count;
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                                           sqlStr.ToString(),
                                           base.FormatParameters(dbCommand.Parameters),
                                           ex.Message));
                }
            }
            return response;
        }

        /// <summary>
        /// PO List page use this method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SearchInvoiceResponse SearchPOlist(SearchInvoiceRequest request)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append(@"SELECT C.CompanyName,PT.PONo,PT.Title,PT.ApprovedOn,i.Milestone,i.InvoiceNo,i.Status  
                            FROM dbo.Companys C,dbo.ProposalTracker PT,dbo.Projects P,dbo.Invoices i
                            WHERE C.ComID=P.CompanyID AND P.ProjectID=PT.ProjectID AND pt.ProposalTrackerID=i.ProposalId AND pt.ApprovedOn IS NOT NULL ");
            switch (request.Searchtype)
            {
                case InvoiceSearchType.ProposalOnly:
                    sqlStr.Append("AND i.ProposalId !=0 ");
                    break;
                case InvoiceSearchType.AwitingPayment:
                    sqlStr.Append("AND i.Status =" + (int)InvoiceStatus.Awaiting_Payment + " ");
                    break;
                case InvoiceSearchType.PassDue:
                    sqlStr.Append("AND i.DueOn <  (select getdate() )");
                    break;
                case InvoiceSearchType.All:
                    break;
            }
            if (request.Keywords != "")
            {
                sqlStr.Append("AND (");
                sqlStr.Append(" PT.PONo LIKE @Keywords ");
                sqlStr.Append(") ");
            }
            if (request.CompanyId != 0)
            {
                sqlStr.Append("AND c.ComID=@CompanyId ");
            }
            if ((int)request.InvoiceStatus != 0)
            {
                sqlStr.Append("AND i.Status=@Status ");
            }
            if (!string.IsNullOrEmpty(request.ApproveOn))
            {
                sqlStr.Append(" AND PT.ApprovedOn=@ApprovedOn ");
            }

            if (request.OrderExpression != "")
            {
                sqlStr.AppendFormat("ORDER BY {0} ", request.OrderExpression);
            }
            else
            {
                sqlStr.Append("ORDER BY  PT.ApprovedOn ");
            }

            if (request.OrderDirection != "")
            {
                sqlStr.Append(request.OrderDirection);
            }
            else
            {
                sqlStr.Append("DESC;");
            }

            List<POListModel> list;
            SearchInvoiceResponse response = new SearchInvoiceResponse();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sqlStr.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Keywords", DbType.String, string.Format("%{0}%", request.Keywords.FilterSqlString()));
                    db.AddInParameter(dbCommand, "CompanyId", DbType.Int32, request.CompanyId);
                    db.AddInParameter(dbCommand, "OrderExpression", DbType.String, request.OrderExpression);
                    db.AddInParameter(dbCommand, "OrderDirection", DbType.String, request.OrderDirection);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, request.InvoiceStatus);
                    db.AddInParameter(dbCommand, "ApprovedOn", DbType.String, request.ApproveOn);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        list = new List<POListModel>();
                        while (dataReader.Read())
                        {
                            list.Add(POListModel.ReaderBind(dataReader));
                        }
                        response.POList = list;
                        response.POListCount = list.Count;
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                                           sqlStr.ToString(),
                                           base.FormatParameters(dbCommand.Parameters),
                                           ex.Message));
                }
            }
            return response;
        }

        /// <summary>
        /// PaymentEmail Application pragram use this method
        /// </summary>
        /// <param name="model"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool UpdateForPaymentEmail(InvoiceEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Invoices set ");
            strSql.Append("ProposalId=@ProposalId,");
            strSql.Append("Milestone=@Milestone,");
            strSql.Append("Approved=@Approved,");
            strSql.Append("InvoiceNo=@InvoiceNo,");
            strSql.Append("SendOn=@SendOn,");
            strSql.Append("ReceiveOn=@ReceiveOn,");
            strSql.Append("DueOn=@DueOn,");
            strSql.Append("Color=@Color,");
            strSql.Append("ModifiedOn=@ModifiedOn,");
            strSql.Append("ModifiedBy=@ModifiedBy,");
            strSql.Append("ColorFor=@ColorFor");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ProposalId", DbType.Int32, model.ProposalId);
                db.AddInParameter(dbCommand, "Milestone", DbType.String, model.Milestone);
                db.AddInParameter(dbCommand, "Approved", DbType.String, model.Approved);
                db.AddInParameter(dbCommand, "InvoiceNo", DbType.String, model.InvoiceNo);
                db.AddInParameter(dbCommand, "SendOn", DbType.DateTime, model.SendOn);
                db.AddInParameter(dbCommand, "ReceiveOn", DbType.DateTime, model.ReceiveOn);
                db.AddInParameter(dbCommand, "DueOn", DbType.DateTime, model.DueOn);
                db.AddInParameter(dbCommand, "Color", DbType.String, model.Color);
                db.AddInParameter(dbCommand, "ColorFor", DbType.String, model.ColorFor);
                db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, DateTime.Now);
                db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, model.ModifiedBy);
                db.AddInParameter(dbCommand, "ID", DbType.Int32, model.ID);

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
    }
}
