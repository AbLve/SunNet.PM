using FamilyBook.Core.DocManagementModule;
using FamilyBook.Entity.DocManagements;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace FamilyBook.Impl.DocManagements
{
    public class FileDAOSQLDataProvider : IFileDAO
    {

        #region  Method

        /// <summary>
        /// Add a record
        /// </summary>
        public int Insert(FilesEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Files(");
            strSql.Append("TicketId,CompanyID,ProjectId,FeedbackId,SourceType,FileTitle,ContentType,FileSize,FilePath,ThumbPath,CreatedOn,CreatedBy,IsDelete,IsPublic,Tags,ProposalTrackerId)");

            strSql.Append(" values (");
            strSql.Append("@TicketId,@CompanyID,@ProjectId,@FeedbackId,@SourceType,@FileTitle,@ContentType,@FileSize,@FilePath,@ThumbPath,@CreatedOn,@CreatedBy,@IsDelete,@IsPublic,@Tags,@ProposalTrackerId)");
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
                    db.AddInParameter(dbCommand, "ProposalTrackerId", DbType.String, model.ProposalTrackerId);
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
            strSql.Append("Tags=@Tags");
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
                db.AddInParameter(dbCommand, "ProposalTrackerId", DbType.Int32, model.ProposalTrackerId);
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
            strSql.Append(" where FileID=@FileID ");
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

        public FilesEntity Get(int fileId, decimal size)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FileID,CompanyID,ProjectId,TicketId,FeedbackId,SourceType,FileTitle,ContentType,FileSize,FilePath,ThumbPath,CreatedOn,CreatedBy,IsDelete,IsPublic from Files ");
            strSql.Append(" where FileID=@FileID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "FileID", DbType.Int32, fileId);
                db.AddInParameter(dbCommand, "FileSize", DbType.Int32, size);
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

        public bool CheckValidityOfFile(FilesEntity fe)
        {
            throw new NotImplementedException();
        }

        public string GetFileStreamReturnThumpFilePath(System.IO.FileStream stream)
        {
            throw new NotImplementedException();
        }

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
                case FileSourceType.ProposalTracker:
                    strSql.Append(" where ProposalTrackerId=@Sid ");
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

    }
}
