using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.ProposalTrackerModule;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using SunNet.PMNew.Framework.Utils.Providers;
using System.Data;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Impl.SqlDataProvider.ProposalTracker
{
    public class ProposalTrackerSqlDataProvider : SqlHelper, IProposalTrackerRepository
    {
        public ProposalTrackerSqlDataProvider()
        { }

        public int Insert(ProposalTrackerEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ProposalTracker(");
            strSql.Append("RequestNo,ProjectID,Title,Description,Status,InvoiceNo,Payment,DueDate,CreatedOn,CreatedBy,WorkScope,WorkScopeDisplayName");
            strSql.Append(",ProposalSentTo,ProposalSentOn,PONo,ApprovedBy,ApprovedOn,InvoiceSentOn,Reminded,RemindTime, PoTotalLessThenProposalTotal)");

            strSql.Append(" values (");
            strSql.Append("@RequestNo,@ProjectID,@Title,@Description,@Status,@InvoiceNo,@Payment,@DueDate,@CreatedOn,@CreatedBy,@WorkScope,@WorkScopeDisplayName");
            strSql.Append(",@ProposalSentTo,@ProposalSentOn,@PONo,@ApprovedBy,@ApprovedOn,@InvoiceSentOn,@Reminded,@RemindTime, @PoTotalLessThenProposalTotal)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "RequestNo", DbType.String, model.RequestNo);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                    db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                    db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, model.Status);
                    db.AddInParameter(dbCommand, "InvoiceNo", DbType.String, model.InvoiceNo);
                    db.AddInParameter(dbCommand, "Payment", DbType.String, model.Payment);
                    db.AddInParameter(dbCommand, "DueDate", DbType.DateTime, model.DueDate);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.String, model.CreatedOn);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                    db.AddInParameter(dbCommand, "WorkScope", DbType.String, model.WorkScope);
                    db.AddInParameter(dbCommand, "WorkScopeDisplayName", DbType.String, model.WorkScopeDisplayName);

                    db.AddInParameter(dbCommand, "ProposalSentTo", DbType.String, model.ProposalSentTo);
                    db.AddInParameter(dbCommand, "ProposalSentOn", DbType.DateTime, model.ProposalSentOn);
                    db.AddInParameter(dbCommand, "PONo", DbType.String, model.PONo);
                    db.AddInParameter(dbCommand, "ApprovedBy", DbType.String, model.ApprovedBy);
                    db.AddInParameter(dbCommand, "ApprovedOn", DbType.DateTime, model.ApprovedOn);
                    db.AddInParameter(dbCommand, "InvoiceSentOn", DbType.DateTime, model.InvoiceSentOn);
                    db.AddInParameter(dbCommand, "Reminded", DbType.Int32, model.Reminded);
                    db.AddInParameter(dbCommand, "RemindTime", DbType.DateTime, model.RemindTime);
                    db.AddInParameter(dbCommand, "PoTotalLessThenProposalTotal", DbType.Boolean, model.PoTotalLessThenProposalTotal);
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

        public bool Update(ProposalTrackerEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ProposalTracker set ");
            strSql.Append("ProjectID=@ProjectID,");
            strSql.Append("InvoiceNo=@InvoiceNo,");
            strSql.Append("Payment=@Payment,");
            strSql.Append("Status=@Status,");
            strSql.Append("DueDate=@DueDate,");
            strSql.Append("Title=@Title,");
            strSql.Append("Description=@Description,");
            strSql.Append("WorkScope=@WorkScope,");
            strSql.Append("WorkScopeDisplayName=@WorkScopeDisplayName,");
            strSql.Append("ModifyOn=@ModifyOn,");
            strSql.Append("ModifyBy=@ModifyBy,");

            strSql.Append("ProposalSentTo=@ProposalSentTo,");
            strSql.Append("ProposalSentOn=@ProposalSentOn,");
            strSql.Append("PONo=@PONo,");
            strSql.Append("ApprovedBy=@ApprovedBy,");
            strSql.Append("ApprovedOn=@ApprovedOn,");
            strSql.Append("InvoiceSentOn=@InvoiceSentOn,");
            strSql.Append("Reminded=@Reminded,");
            strSql.Append("PoTotalLessThenProposalTotal=@PoTotalLessThenProposalTotal,");
            strSql.Append("RemindTime=@RemindTime ");

            strSql.Append(" where ProposalTrackerID=@ProposalTrackerID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProposalTrackerID", DbType.Int32, model.ProposalTrackerID);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                    db.AddInParameter(dbCommand, "InvoiceNo", DbType.String, model.InvoiceNo);
                    db.AddInParameter(dbCommand, "Payment", DbType.Int32, model.Payment);
                    db.AddInParameter(dbCommand, "Status", DbType.Int32, model.Status);
                    db.AddInParameter(dbCommand, "DueDate", DbType.DateTime, model.DueDate);
                    db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                    db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                    db.AddInParameter(dbCommand, "WorkScope", DbType.String, model.WorkScope);
                    db.AddInParameter(dbCommand, "WorkScopeDisplayName", DbType.String, model.WorkScopeDisplayName);
                    db.AddInParameter(dbCommand, "ModifyOn", DbType.DateTime, model.ModifyOn);
                    db.AddInParameter(dbCommand, "ModifyBy", DbType.Int32, model.ModifyBy);

                    db.AddInParameter(dbCommand, "ProposalSentTo", DbType.String, model.ProposalSentTo);
                    db.AddInParameter(dbCommand, "ProposalSentOn", DbType.DateTime, model.ProposalSentOn);
                    db.AddInParameter(dbCommand, "PONo", DbType.String, model.PONo);
                    db.AddInParameter(dbCommand, "ApprovedBy", DbType.String, model.ApprovedBy);
                    db.AddInParameter(dbCommand, "ApprovedOn", DbType.DateTime, model.ApprovedOn);
                    db.AddInParameter(dbCommand, "InvoiceSentOn", DbType.DateTime, model.InvoiceSentOn);
                    db.AddInParameter(dbCommand, "Reminded", DbType.Int32, model.Reminded);
                    db.AddInParameter(dbCommand, "RemindTime", DbType.DateTime, model.RemindTime);
                    db.AddInParameter(dbCommand, "PoTotalLessThenProposalTotal", DbType.Boolean, model.PoTotalLessThenProposalTotal);

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

        public bool Delete(int ID)
        {
            return true;
        }

        public ProposalTrackerEntity Get(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *,'' as ProjectName, '' as CompanyName, 1 as CompanyID from ProposalTracker ");
            strSql.Append(" where ProposalTrackerID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
                    ProposalTrackerEntity model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = ProposalTrackerEntity.ReaderBind(dataReader);
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


        public List<ProposalTrackerEntity> GetProposalTrackers(string keyword, int projectId, int status,
            int payment, int userId, string order, string dir)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT w.*,p.Title as ProjectName,ROW_NUMBER() OVER (order by workrequestid desc) as rowno");

            strSql.Append(@" FROM ProposalTracker w ");
            strSql.Append(@"left join Projects p on w.ProjectID= p.ProjectID");
            strSql.Append(@" where 1=1 ");
            if (projectId > 0)
                strSql.Append(@" AND w.projectId=@projectId ");
            if (status > 0)
                strSql.Append(@" AND w.status=@status ");
            if (payment > 0)
                strSql.Append(@" AND w.payment=@payment ");

            if (!string.IsNullOrEmpty(keyword))
                strSql.Append(@" AND (w.requestNo LIKE @keyword 
                                OR w.Title LIKE @keyword OR InvoiceNo LIKE @keyword )");

            if (userId > 0)
                strSql.Append(@" AND w.projectid in (select projectid from projectusers where userid = @userId) ");

            strSql.Append(@" order by @order+ ' '+ @dir ");
            List<ProposalTrackerEntity> list = new List<ProposalTrackerEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "keyword", DbType.String, string.Format("%{0}%", keyword.FilterSqlString()));
                    db.AddInParameter(dbCommand, "Status", DbType.String, status);
                    db.AddInParameter(dbCommand, "projectId", DbType.Int32, projectId);
                    db.AddInParameter(dbCommand, "payment", DbType.Int32, payment);
                    db.AddInParameter(dbCommand, "userId", DbType.Int32, userId);
                    db.AddInParameter(dbCommand, "order", DbType.String, order);
                    db.AddInParameter(dbCommand, "dir", DbType.String, dir);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        while (dataReader.Read())
                        {
                            list.Add(ProposalTrackerEntity.ReaderBind(dataReader));
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

        public ProposalTrackerRelationEntity GetProposalTrackerByTid(int Tid)
        {
            ProposalTrackerRelationEntity model = new ProposalTrackerRelationEntity();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from ProposalTrackerRelation where TID=@Tid");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Tid", DbType.Int32, Tid);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        while (dataReader.Read())
                        {
                            model = (ProposalTrackerRelationEntity.ReaderBind(dataReader));
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

        public bool DelProposalTrackerRelationByID(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE ProposalTrackerRelation ");
            strSql.Append(" where ID=@id ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "id", DbType.Int32, ID);
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

        public bool UpdateProposalByProposal(ProposalTrackerRelationEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ProposalTrackerRelation set ");
            strSql.Append("WID=@wid,");
            strSql.Append("TID=@tid");

            strSql.Append(" where ID=@id ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {

                    db.AddInParameter(dbCommand, "wid", DbType.Int32, model.WID);
                    db.AddInParameter(dbCommand, "tid", DbType.Int32, model.TID);
                    db.AddInParameter(dbCommand, "id", DbType.Int32, model.ID);
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

        public List<ProposalTrackerEntity> GetProposalTrackerByPid(int projectId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT pt.*,p.Title as ProjectName,p.CompanyID");
            strSql.Append(@" FROM ProposalTracker pt,Projects p");
            strSql.Append(@" where 1=1 and pt.ProjectID=p.ProjectID");
            strSql.Append(@" AND pt.ProjectID=@projectId ");
            List<ProposalTrackerEntity> list = new List<ProposalTrackerEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "projectId", DbType.Int32, projectId);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        while (dataReader.Read())
                        {
                            list.Add(ProposalTrackerEntity.ReaderBind(dataReader));
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


        public SearchProposalTrackerRequest GetProposalTrackers(string keyword, int projectId, int status, int companyId,
            int payment, int userId,DateTime? beginTime,DateTime? endTime , string order, string dir, int pageCount, int pageIndex)
        {
            string strOrderby = string.Format(" {0} {1} ", order, dir);
            int start = pageIndex * pageCount + 1 - pageCount;
            int end = pageIndex * pageCount;

            StringBuilder strSql = new StringBuilder();

            strSql.Append(@"SELECT  count(1)");
            strSql.Append(@" from (SELECT w.RequestNo,w.Title,w.ProjectID,w.status,w.payment,p.CompanyID,");
            strSql.Append(@"(SELECT STUFF(( SELECT  ',' + PP.InvoiceNo FROM Invoices AS PP  
WHERE PP.Proposalid = w.ProposalTrackerID AND PP.InvoiceNo != '' FOR XML PATH('') ), 1, 1, '')) AS InvoiceNo");

            strSql.Append(@" FROM (ProposalTracker w ");
            strSql.Append(@"left join Projects p on w.ProjectID= p.ProjectID LEFT JOIN dbo.Companys C ON C.ComID = p.CompanyID))NT");
            strSql.Append(@" where 1=1 ");
            if (projectId > 0)
                strSql.Append(@" AND NT.projectId=@projectId ");
            if (status > 0)
                strSql.Append(@" AND NT.status=@status ");
            if (companyId > 0)
                strSql.Append(@" AND NT.CompanyID=@companyId ");
            if (payment > 0)
                strSql.Append(@" AND NT.payment=@payment ");

            if (!string.IsNullOrEmpty(keyword))
                strSql.Append(@" AND (NT.requestNo LIKE @keyword 
                                OR NT.Title LIKE @keyword OR NT.InvoiceNo LIKE @keyword)");

            if (userId > 0)
                strSql.Append(@" AND NT.projectid in (select projectid from projectusers where userid = @userId); ");

            strSql.Append(@"select * from ");
            strSql.Append(@"(select *,");
            strSql.Append(string.Format(@"ROW_NUMBER() OVER (order by {0} )rownumber ", strOrderby));
            strSql.Append(@"FROM(SELECT "); 
            strSql.Append(@"w.ProposalTrackerID ,");
            strSql.Append(@"w.ProjectID ,");
            strSql.Append(@"p.CompanyID ,");
            strSql.Append(@"c.CompanyName ,");
            strSql.Append(@"w.Status ,");
            strSql.Append(@"w.Title ,");
            strSql.Append(@"w.Description ,");
            strSql.Append(@"w.ProposalSentTo ,");
            strSql.Append(@"w.ProposalSentOn ,");
            strSql.Append(@"w.PONo ,");
            strSql.Append(@"w.ApprovedBy ,");
            strSql.Append(@"w.ApprovedOn ,");
            strSql.Append(@"w.InvoiceSentOn ,");
            strSql.Append(@"w.WorkScope ,");
            strSql.Append(@"w.WorkScopeDisplayName ,");
            strSql.Append(@"w.RequestNo ,");
            strSql.Append(@"w.Payment ,");
            strSql.Append(@"w.DueDate ,");
            strSql.Append(@"w.CreatedOn ,");
            strSql.Append(@"w.CreatedBy ,");
            strSql.Append(@"w.ModifyOn ,");
            strSql.Append(@"w.ModifyBy ,");
            strSql.Append(@"w.Reminded ,");
            strSql.Append(@"w.RemindTime ,");
            strSql.Append(@"w.PoTotalLessThenProposalTotal ,");
            strSql.Append(@"p.Title as ProjectName,");
            strSql.Append(@"(SELECT STUFF(( SELECT  ',' + PP.InvoiceNo FROM Invoices AS PP ");
            strSql.Append(@" WHERE PP.Proposalid = w.ProposalTrackerID AND PP.InvoiceNo != '' FOR XML PATH('') ), 1, 1, '')) AS InvoiceNo ");

            strSql.Append(@" FROM (ProposalTracker w ");
            strSql.Append(@"left join Projects p on w.ProjectID= p.ProjectID");
            strSql.Append(@" LEFT JOIN dbo.Companys C ON C.ComID = p.CompanyID))NK ");
            strSql.Append(@" where 1=1 ");
            if (!string.IsNullOrEmpty(keyword))
                strSql.Append(@" AND (NK.InvoiceNo LIKE @keyword or NK.requestNo LIKE @keyword 
                                OR NK.Title LIKE @keyword )");
            if (projectId > 0)
                strSql.Append(@" AND NK.projectId=@projectId ");
            if (status > 0)
                strSql.Append(@" AND NK.status=@status ");
            if (companyId > 0)
                strSql.Append(@" AND NK.CompanyID=@companyId ");
            if (payment > 0)
                strSql.Append(@" AND NK.payment=@payment ");

            if (userId > 0)
                strSql.Append(@" AND w.projectid in (select projectid from projectusers where userid = @userId) ");

            if(beginTime != null)
                strSql.Append(@" AND w.ApprovedOn>=@approvedOnBegin ");
            if (endTime != null)
                strSql.Append(@" AND w.ApprovedOn<=@approvedOnEnd ");

            strSql.Append(@" )NEWTT where rownumber between @start and  @end ");
            List<ProposalTrackerEntity> list;
            SearchProposalTrackerRequest response = new SearchProposalTrackerRequest();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "keyword", DbType.String, string.Format("%{0}%", keyword.FilterSqlString()));
                    db.AddInParameter(dbCommand, "Status", DbType.String, status);
                    db.AddInParameter(dbCommand, "companyId", DbType.Int32, companyId);
                    db.AddInParameter(dbCommand, "projectId", DbType.Int32, projectId);
                    db.AddInParameter(dbCommand, "payment", DbType.Int32, payment);
                    db.AddInParameter(dbCommand, "userId", DbType.Int32, userId);

                    if (beginTime != null)
                        db.AddInParameter(dbCommand, "approvedOnBegin", DbType.DateTime, beginTime);
                    if (endTime != null)
                        db.AddInParameter(dbCommand, "approvedOnEnd", DbType.DateTime, endTime);


                    db.AddInParameter(dbCommand, "order", DbType.String, order);
                    db.AddInParameter(dbCommand, "dir", DbType.String, dir);
                    db.AddInParameter(dbCommand, "start", DbType.Int32, start);
                    db.AddInParameter(dbCommand, "end", DbType.Int32, end);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        list = new List<ProposalTrackerEntity>();
                        if (dataReader.Read())
                        {
                            response.ResultCount = dataReader.GetInt32(0);
                            dataReader.NextResult();
                        }
                        while (dataReader.Read())
                        {
                            list.Add(ProposalTrackerEntity.ReaderBind(dataReader));
                        }
                        response.ResultList = list;
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
            return response;
        }

        public decimal GetProposalTrackerHours(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  isnull(sum(Hours),0) from dbo.TimeSheets where ticketid in ");
            strSql.Append("(select tid from dbo.ProposalTrackerRelation where wid=@wid)");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "wid", DbType.Int32, ID);
                    object obj = db.ExecuteScalar(dbCommand);
                    if (obj == null)
                        return 0;
                    return (decimal)db.ExecuteScalar(dbCommand);
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(),
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                }
            }
            return 0;
        }

        public bool UpdateProposalTrackerForPayment(ProposalTrackerEntity model, string connStr)
        {
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ProposalTracker set ");
                strSql.Append("ProjectID=@ProjectID,");
                strSql.Append("InvoiceNo=@InvoiceNo,");
                strSql.Append("Payment=@Payment,");
                strSql.Append("Status=@Status,");
                strSql.Append("DueDate=@DueDate,");
                strSql.Append("Title=@Title,");
                strSql.Append("Description=@Description,");
                strSql.Append("WorkScope=@WorkScope,");
                strSql.Append("WorkScopeDisplayName=@WorkScopeDisplayName,");
                strSql.Append("ModifyOn=@ModifyOn,");
                strSql.Append("ModifyBy=@ModifyBy,");

                strSql.Append("ProposalSentTo=@ProposalSentTo,");
                strSql.Append("ProposalSentOn=@ProposalSentOn,");
                strSql.Append("PONo=@PONo,");
                strSql.Append("ApprovedBy=@ApprovedBy,");
                strSql.Append("ApprovedOn=@ApprovedOn,");
                strSql.Append("InvoiceSentOn=@InvoiceSentOn,");
                strSql.Append("Reminded=@Reminded,");
                strSql.Append("RemindTime=@RemindTime ");

                strSql.Append(" where ProposalTrackerID=@ProposalTrackerID ");
                Database db = DatabaseFactory.CreateDatabase(connStr);
                using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
                {
                    try
                    {
                        db.AddInParameter(dbCommand, "ProposalTrackerID", DbType.Int32, model.ProposalTrackerID);
                        db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                        db.AddInParameter(dbCommand, "InvoiceNo", DbType.String, model.InvoiceNo);
                        db.AddInParameter(dbCommand, "Payment", DbType.Int32, model.Payment);
                        db.AddInParameter(dbCommand, "Status", DbType.Int32, model.Status);
                        db.AddInParameter(dbCommand, "DueDate", DbType.DateTime, model.DueDate);
                        db.AddInParameter(dbCommand, "Title", DbType.String, model.Title);
                        db.AddInParameter(dbCommand, "Description", DbType.String, model.Description);
                        db.AddInParameter(dbCommand, "WorkScope", DbType.String, model.WorkScope);
                        db.AddInParameter(dbCommand, "WorkScopeDisplayName", DbType.String, model.WorkScopeDisplayName);
                        db.AddInParameter(dbCommand, "ModifyOn", DbType.DateTime, DateTime.Now);
                        db.AddInParameter(dbCommand, "ModifyBy", DbType.Int32, model.ModifyBy);

                        db.AddInParameter(dbCommand, "ProposalSentTo", DbType.String, model.ProposalSentTo);
                        db.AddInParameter(dbCommand, "ProposalSentOn", DbType.DateTime, model.ProposalSentOn);
                        db.AddInParameter(dbCommand, "PONo", DbType.String, model.PONo);
                        db.AddInParameter(dbCommand, "ApprovedBy", DbType.String, model.ApprovedBy);
                        db.AddInParameter(dbCommand, "ApprovedOn", DbType.DateTime, model.ApprovedOn);
                        db.AddInParameter(dbCommand, "InvoiceSentOn", DbType.DateTime, model.InvoiceSentOn);
                        db.AddInParameter(dbCommand, "Reminded", DbType.Int32, model.Reminded);
                        db.AddInParameter(dbCommand, "RemindTime", DbType.DateTime, model.RemindTime);
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
        }

        public List<ProposalTrackerEntity> GetEntitiesForPaymentEmail(string condition, string connStr)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ProposalTracker.*,Projects.Title AS ProjectName FROM dbo.ProposalTracker LEFT JOIN dbo.Projects ON ProposalTracker.ProjectID=Projects.ProjectID ");
            if (!string.IsNullOrEmpty(condition))
            {
                strSql.Append(" WHERE ");
                strSql.Append(condition);
            }
            List<ProposalTrackerEntity> list = new List<ProposalTrackerEntity>();
            Database db = DatabaseFactory.CreateDatabase(connStr);
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        while (dataReader.Read())
                        {
                            list.Add(ProposalTrackerEntity.ReaderBind(dataReader));
                        }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},Messages:\r\n{1}]"
                        , strSql.ToString(), ex.Message));
                    return null;
                }
            }
            return list;
        }

        public ProposalViewModel GetProposalViewModel(int proposalId)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("SELECT ");
            sqlStr.Append("PT.ProposalTrackerID ProposalId ,");
            sqlStr.Append("PT.Title ProposalTitle ,");
            sqlStr.Append("PT.PONo PONo ,");
            sqlStr.Append("P.ProjectID ProjectId ,");
            sqlStr.Append("P.Title ProjectTitle ,");
            sqlStr.Append("C.ComID CompanyId ,");
            sqlStr.Append("C.CompanyName CompanyName ");
            sqlStr.Append("FROM ");
            sqlStr.Append("dbo.ProposalTracker AS PT ");
            sqlStr.Append("LEFT JOIN dbo.Projects AS P ON PT.ProjectID = P.ProjectID ");
            sqlStr.Append("LEFT JOIN dbo.Companys AS C ON P.CompanyID = C.ComID ");
            sqlStr.Append("WHERE ");
            sqlStr.AppendFormat("PT.ProposalTrackerID = {0} ", proposalId);

            ProposalViewModel model = new ProposalViewModel();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sqlStr.ToString()))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        while (dataReader.Read())
                        {
                            model = ProposalViewModel.ReaderBind(dataReader);
                        }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},Messages:\r\n{1}]"
                        , sqlStr.ToString(), ex.Message));
                    return null;
                }
            }
            return model;
        }

    }
}
