using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;

using SunNet.PMNew.Core.FileModule;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Impl.SqlDataProvider.File
{
    /// <summary>
    /// Data access:Files
    /// </summary>
    public class FilesRepositorySqlDataProvider : SqlHelper, IFilesRepository
    {
        public FilesRepositorySqlDataProvider()
        { }
        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(FilesEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Files(");
            strSql.Append(@"TicketId,CompanyID,ProjectId,FeedbackId,SourceType,FileTitle,ContentType,FileSize,FilePath,ThumbPath,
                                CreatedOn,CreatedBy,IsDelete,IsPublic,Tags,WorkRequestId,SourceID)");

            strSql.Append(" values (");
            strSql.Append(@"@TicketId,@CompanyID,@ProjectId,@FeedbackId,@SourceType,@FileTitle,@ContentType,@FileSize,@FilePath,@ThumbPath,
                                @CreatedOn,@CreatedBy,@IsDelete,@IsPublic,@Tags,@WorkRequestId,@SourceID)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "TicketId", DbType.Int32, model.TicketId);
                    db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, model.CompanyID);
                    db.AddInParameter(dbCommand, "ProjectId", DbType.Int32, model.ProjectId);
                    db.AddInParameter(dbCommand, "FeedbackId", DbType.Int32, model.FeedbackId);
                    db.AddInParameter(dbCommand, "SourceType", DbType.Int32, model.SourceType);
                    db.AddInParameter(dbCommand, "FileTitle", DbType.String, model.FileTitle);
                    db.AddInParameter(dbCommand, "ContentType", DbType.String, model.ContentType);
                    db.AddInParameter(dbCommand, "FileSize", DbType.Int32, model.FileSize);
                    db.AddInParameter(dbCommand, "FilePath", DbType.String, model.FilePath);
                    db.AddInParameter(dbCommand, "ThumbPath", DbType.String, model.ThumbPath);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                    db.AddInParameter(dbCommand, "IsDelete", DbType.Boolean, model.IsDelete);
                    db.AddInParameter(dbCommand, "IsPublic", DbType.Boolean, model.IsPublic);
                    db.AddInParameter(dbCommand, "Tags", DbType.String, model.Tags);
                    db.AddInParameter(dbCommand, "WorkRequestId", DbType.String, model.ProposalTrackerId);
                    db.AddInParameter(dbCommand, "SourceID", DbType.String, model.SourceID);
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
        public bool Update(FilesEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Files set ");
            strSql.Append("CompanyID=@CompanyID,");
            strSql.Append("TicketId=@TicketId,");
            strSql.Append("ProjectId=@ProjectId,");
            strSql.Append("FeedbackId=@FeedbackId,");
            strSql.Append("SourceType=@SourceType,");
            strSql.Append("FileTitle=@FileTitle,");
            strSql.Append("ContentType=@ContentType,");
            strSql.Append("FileSize=@FileSize,");
            strSql.Append("FilePath=@FilePath,");
            strSql.Append("ThumbPath=@ThumbPath,");
            strSql.Append("CreatedOn=@CreatedOn,");
            strSql.Append("CreatedBy=@CreatedBy,");
            strSql.Append("IsDelete=@IsDelete,");
            strSql.Append("IsPublic=@IsPublic,");
            strSql.Append("Tags=@Tags,");
            strSql.Append("SourceID=@SourceID");
            strSql.Append(" where FileID=@FileID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "FileID", DbType.Int32, model.FileID);
                db.AddInParameter(dbCommand, "CompanyID", DbType.Int32, model.CompanyID);
                db.AddInParameter(dbCommand, "TicketId", DbType.Int32, model.TicketId);
                db.AddInParameter(dbCommand, "ProjectId", DbType.Int32, model.ProjectId);
                db.AddInParameter(dbCommand, "FeedbackId", DbType.Int32, model.FeedbackId);
                db.AddInParameter(dbCommand, "SourceType", DbType.Int32, model.SourceType);
                db.AddInParameter(dbCommand, "FileTitle", DbType.String, model.FileTitle);
                db.AddInParameter(dbCommand, "ContentType", DbType.String, model.ContentType);
                db.AddInParameter(dbCommand, "FileSize", DbType.Int32, model.FileSize);
                db.AddInParameter(dbCommand, "FilePath", DbType.String, model.FilePath);
                db.AddInParameter(dbCommand, "ThumbPath", DbType.String, model.ThumbPath);
                db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, model.CreatedOn);
                db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, model.CreatedBy);
                db.AddInParameter(dbCommand, "IsDelete", DbType.Boolean, model.IsDelete);
                db.AddInParameter(dbCommand, "IsPublic", DbType.Boolean, model.IsPublic);
                db.AddInParameter(dbCommand, "Tags", DbType.String, model.Tags);
                db.AddInParameter(dbCommand, "WorkRequestId", DbType.Int32, model.ProposalTrackerId);
                db.AddInParameter(dbCommand, "SourceID", DbType.Int32, model.SourceID);
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
        public bool Delete(int FileID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Files set ");
            strSql.Append("IsDelete=1");
            strSql.Append(" where FileID=@FileID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "FileID", DbType.Int32, FileID);
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
        public FilesEntity Get(int FileID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Files ");
            strSql.Append(" where IsDelete = 0 and FileID = @FileID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "FileID", DbType.Int32, FileID);
                FilesEntity model = null;
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        model = FilesEntity.ReaderBind(dataReader);
                    }
                }
                return model;
            }
        }

        #endregion  Method

        #region IFilesRepository Members

        public List<FilesEntity> GetFileListBySourceId(int Sid, FileSourceType type)
        {
            List<FilesEntity> list = new List<FilesEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Files ");
            switch (type)
            {
                case FileSourceType.Ticket:
                    strSql.Append(" where TicketId=@Sid ");
                    break;
                case FileSourceType.Project:
                    strSql.Append(" where ProjectId=@Sid ");
                    break;
                case FileSourceType.FeedBack:
                    strSql.Append(" where FeedbackId=@Sid ");
                    break;
                case FileSourceType.WorkRequest:
                    strSql.Append(" where WorkRequestId=@Sid ");
                    break;
                case FileSourceType.KnowledgeShare:
                    strSql.Append(" where SourceID=@Sid ");
                    break;
                default:
                    break;
            }
            strSql.Append(" and SourceType=@SourceType and Isdelete =0");
            strSql.Append(" order by CreatedOn desc");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "Sid", DbType.Int32, Sid);
                db.AddInParameter(dbCommand, "SourceType", DbType.Int32, (int)type);

                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        list.Add(FilesEntity.ReaderBind(dataReader));
                    }
                }
            }
            return list;
        }

        public bool Delete(int Sid, FileSourceType type)
        {
            List<FilesEntity> list = new List<FilesEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Files set ");
            strSql.Append("IsDelete=1");
            switch (type)
            {
                case FileSourceType.Ticket:
                    strSql.Append(" where TicketId=@Sid ");
                    break;
                case FileSourceType.Project:
                    strSql.Append(" where ProjectId=@Sid ");
                    break;
                case FileSourceType.FeedBack:
                    strSql.Append(" where FeedbackId=@Sid ");
                    break;
            }
            strSql.Append(" and SourceType=@SourceType ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "Sid", DbType.Int32, Sid);
                db.AddInParameter(dbCommand, "SourceType", DbType.Int32, (int)type);

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

        #region IFilesRepository Members-jack

        public List<FileDetailDto> GetFileList(SearchFilesRequest request)
        {
            int start = request.CurrentPage * request.PageCount + 1 - request.PageCount;
            int end = request.CurrentPage * request.PageCount;

            string selPageModel = string.Format(@"SELECT * FROM( 
                                    Select  ROW_NUMBER() OVER( ORDER BY {0} {1}) AS INDEX_ID ", request.OrderByExpression, request.OrderByDirection)
              + @",SearchResult.* From ({0}) as SearchResult
                                    ) as New_TB  
                                    Where INDEX_ID between @Start and @End
                                ";


            //            string selPageModel = @"SELECT * FROM( 
            //                                    Select  ROW_NUMBER() OVER( ORDER BY FileTitle ASC) AS INDEX_ID,SearchResult.* From ({0}) as SearchResult
            //
            //
            //                                    ) as New_TB  
            //                                    Where INDEX_ID between @Start and @End
            //                                    ";

            string selCount = @"SELECT Count(1) FROM ({0}) as SearchResult ;";

            StringBuilder strNoPageModal = new StringBuilder();

            //strNoPageModal.Append(@"SELECT f.*,c.CompanyName,p.Title as ProjectTitle,t.Title as TicketTitle ,t.TicketCode + cast(t.TicketID as varchar) as TicketCode,fb.Title as FeedBackTitle
            //                                ,u.FirstName,u.LastName
            //                  FROM [Files] f left join Companys c on c.ComID=f.CompanyID 
            //                                 left join Projects p on f.ProjectId=p.ProjectID 
            //                           left join Tickets t on f.TicketId=t.TicketID 
            //                           left join WorkRequest wr on f.WorkRequestId=wr.WorkRequestId 
            //                           left join FeedBacks fb on f.FeedbackId=fb.FeedBackID  
            //                                 left join Users u on f.CreatedBy=u.UserID");
            strNoPageModal.Append(@"SELECT f.*,c.CompanyName,p.Title as ProjectTitle,t.Title as TicketTitle ,t.TicketCode + cast(t.TicketID as varchar) as TicketCode,fb.Title as FeedBackTitle
                                            ,u.FirstName,u.LastName
                              FROM [Files] f left join Companys c on c.ComID=f.CompanyID 
                                             left join Projects p on f.ProjectId=p.ProjectID 
		                                     left join Tickets t on f.TicketId=t.TicketID 
		                                     left join FeedBacks fb on f.FeedbackId=fb.FeedBackID  
                                             left join Users u on f.CreatedBy=u.UserID");
            strNoPageModal.Append(@" Where 1=1 AND f.Isdelete=0 AND ( f.TicketID=0 or t.Status>0 ) ");

            if (request.IsPublic)
            {
                strNoPageModal.Append(@" And f.IsPublic=@IsPublic ");
            }

            switch (request.SearchType)
            {
                case SearchFileType.Company:
                    {
                        if (request.CompanyID > 0)
                        {
                            strNoPageModal.Append(" And f.CompanyID=@ComapnyID");
                        }
                        else
                        {
                            strNoPageModal.Append(" And f.CompanyID>1 ");
                        }
                        strNoPageModal.AppendFormat(" And f.SourceType={0} ", ((int)FileSourceType.Company).ToString());
                        if (!string.IsNullOrEmpty(request.Keyword) && request.Keyword.Trim() != "")
                            strNoPageModal.Append(" AND f.FileTitle like @Keyword ");
                    }
                    break;
                case SearchFileType.Project:
                    {
                        if (request.CompanyID > 0)
                        {
                            strNoPageModal.Append(" And f.CompanyID=@ComapnyID");
                        }
                        if (request.ProjectID > 0)
                        {
                            strNoPageModal.Append(" and f.ProjectId=@ProjectID ");
                        }
                        strNoPageModal.AppendFormat(" And f.SourceType={0} ", ((int)FileSourceType.Project).ToString());
                        if (!string.IsNullOrEmpty(request.Keyword) && request.Keyword.Trim() != "")
                            strNoPageModal.Append(" AND f.FileTitle like @Keyword ");
                    }
                    break;

                case SearchFileType.WorkRequest:
                    {
                        if (request.CompanyID > 0)
                        {
                            strNoPageModal.Append(" And f.CompanyID=@ComapnyID");
                        }
                        if (request.ProjectID > 0)
                        {
                            strNoPageModal.Append(" and f.ProjectId=@ProjectID ");
                        }
                        strNoPageModal.AppendFormat(" And f.SourceType in ({0},{1}) ", ((int)FileSourceType.WorkRequest).ToString(), ((int)FileSourceType.WorkRequestScope).ToString());
                        //if (request.UserID != 0)
                        //{
                        //    strNoPageModal.AppendFormat(" AND wr.ProjectID IN (select ProjectID from ProjectUsers where UserId={0})", request.UserID);
                        //}
                        if (!string.IsNullOrEmpty(request.Keyword) && request.Keyword.Trim() != "")
                            strNoPageModal.Append(" AND f.Tags like @Keyword ");
                    }
                    break;
                case SearchFileType.TicketAndFeedback:
                    {
                        if (request.CompanyID > 0)
                        {
                            strNoPageModal.Append(" And f.CompanyID=@ComapnyID");
                        }
                        if (request.ProjectID > 0)
                        {
                            strNoPageModal.Append(" and f.ProjectId=@ProjectID ");
                        }
                        strNoPageModal.AppendFormat(" And (f.SourceType={0} or f.SourceType={1}) ", ((int)FileSourceType.Ticket).ToString(), ((int)FileSourceType.FeedBack).ToString());
                        strNoPageModal.AppendFormat(" AND f.ProjectId in (select ProjectId from ProjectUsers h where h.UserID= {0} )", request.UserID);
                        if (request.KeywordType == SearchKeywordType.TicketCode)
                        {
                            if (!string.IsNullOrEmpty(request.Keyword) && request.Keyword.Trim() != "")
                            {
                                int ticketcode = 0;
                                int.TryParse(request.Keyword, out ticketcode);
                                if (ticketcode > 0)
                                    strNoPageModal.AppendFormat(@" AND  t.TicketID = {0} ", ticketcode);
                            }
                        }
                        else if (request.KeywordType == SearchKeywordType.TicketTitle)
                        {
                            if (!string.IsNullOrEmpty(request.Keyword) && request.Keyword.Trim() != "")
                                strNoPageModal.Append(@" AND t.Title like @Keyword  ");
                        }
                        else if (request.KeywordType == SearchKeywordType.FileName)
                        {
                            if (!string.IsNullOrEmpty(request.Keyword) && request.Keyword.Trim() != "")
                                strNoPageModal.Append(" AND f.FileTitle like @Keyword ");
                        }
                        else if (request.KeywordType == SearchKeywordType.All)
                        {
                            if (!string.IsNullOrEmpty(request.Keyword) && request.Keyword.Trim() != "")
                            {
                                strNoPageModal.Append(@" AND ( t.Title like @Keyword or f.FileTitle like @Keyword ");

                                int ticketcode = 0;
                                int.TryParse(request.Keyword, out ticketcode);
                                if (ticketcode > 0)
                                    strNoPageModal.AppendFormat(@" or t.TicketID = {0} ", ticketcode);
                                strNoPageModal.Append(")");
                            }
                        }
                    }
                    break;
                default: break;
            }
            StringBuilder strSql = new StringBuilder();
            if (request.IsPageModel)
            {
                strSql.AppendFormat(selCount, strNoPageModal.ToString());
                strSql.AppendFormat(selPageModel, strNoPageModal.ToString());
            }
            else
            {
                strSql.Append(strNoPageModal);
            }
            Database db = DatabaseFactory.CreateDatabase();

            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Keyword", DbType.String, string.Format("%{0}%", request.Keyword.FilterSqlString()));
                    db.AddInParameter(dbCommand, "ComapnyID", DbType.Int32, request.CompanyID);
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, request.ProjectID);
                    db.AddInParameter(dbCommand, "IsPublic", DbType.Boolean, request.IsPublic);
                    db.AddInParameter(dbCommand, "Start", DbType.Int32, start);
                    db.AddInParameter(dbCommand, "End", DbType.Int32, end);
                    List<FileDetailDto> list = new List<FileDetailDto>();
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (request.IsPageModel)
                        {
                            if (dataReader.Read())
                            {
                                request.RecordCount = dataReader.GetInt32(0);
                            }
                            dataReader.NextResult();
                        }
                        while (dataReader.Read())
                        {
                            list.Add(FileDetailDto.ReaderBind(dataReader));
                        }
                        return list;
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:GetFileList = {0},{1}Messages:\r\n{2}]",
                          strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        #endregion
    }
}

