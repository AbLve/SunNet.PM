using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SunNet.PMNew.Core.ShareModule.Interfaces;
/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		5/25 22:59:10
 * Description:		Please input class summary
 * Version History:	Created,5/25 22:59:10
 * 
 * 
 **************************************************************************/
using SunNet.PMNew.Entity.ShareModel;
using SunNet.PMNew.Entity.ShareModel.DTO;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;


namespace SunNet.PMNew.Impl.SqlDataProvider.Share
{
    public class ShareRepositorySqlDataProvider : SqlHelper, IShareRepository
    {
        public List<ShareTypeEntity> GetShareTypes()
        {
            string strSql = @"SELECT * FROM [ShareType]";
            var list = new List<ShareTypeEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        while (dataReader.Read())
                        {
                            list.Add(ShareTypeEntity.ReaderBind(dataReader));
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

        public SearchShareResponse GetShares(SearchShareRequest request)
        {
            string strWhere = "";
            if (request.CreatedBy > 0)
                strWhere += " AND CreatedBy = @CreatedBy ";
            if (request.Type > 0)
                strWhere += " AND Type = @Type ";
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                int tmpTicketId;
                int.TryParse(request.Keyword, out tmpTicketId);
                strWhere += string.Format(" AND( Note like '%{0}%' OR TicketID ={1})", request.Keyword.FilterSqlString(), tmpTicketId);
            }
            if (request.StartDate > MinDate)
                strWhere += " And CreatedOn >= @StartDate ";
            else
                request.StartDate = MinDate;
            if (request.EndDate > MinDate)
                strWhere += " And CreatedOn <= @EndDate ";
            else request.EndDate = MinDate;

            string strSql = string.Format(@"SELECT BASICFILTER.*, 
                                TypeID = ST.ID, TypeTitle = ST.[Title], TypeCreatedBy = ST.[CreatedBy],
                                TypeCreatedOn = ST.[CreatedOn],TypeType = ST.[Type] FROM (
                                SELECT * ,Files = 
                                    (select CAST(FileID AS NVARCHAR)+ '_' + FileTitle + [ContentType]+'|' from [Files] 
                                        WHERE [SourceType] = 7 AND [SourceID] = S.ID AND IsDelete = 0 FOR XML PATH('') )
                                ,Row_Number() OVER(ORDER BY {0} {1}) AS INDEX_ FROM [Share] S 
                                Where [IsDeleted] = 0 {2}
                                ) AS BASICFILTER LEFT JOIN [ShareType] ST ON BASICFILTER.[Type] = ST.ID 
                                ", request.OrderExpression, request.OrderDirection, strWhere);
            if (request.IsPageModel)
            {
                strSql += string.Format("WHERE BASICFILTER.INDEX_ BETWEEN {0} AND {1} ;",
                                        (request.CurrentPage - 1) * request.PageCount + 1,
                                        request.CurrentPage * request.PageCount)
                                        ;
                strSql += string.Format(@"SELECT Count(1) as [Count] FROM [Share] 
                                            Where [IsDeleted] = 0 {0}", strWhere);
            }

            var response = new SearchShareResponse();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, request.CreatedBy);
                    db.AddInParameter(dbCommand, "Type", DbType.Int32, request.Type);
                    db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, request.StartDate);
                    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, request.EndDate);

                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            response.Dataset.Add(ShareEntity.ReaderBind(dataReader));
                        }
                        if (dataReader.NextResult() && dataReader.Read())
                            response.Count = dataReader.GetInt32(0);
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2},\r\nSourceCode:{3}]"
                        , strSql, base.FormatParameters(dbCommand.Parameters), ex.Message, ex.StackTrace));
                    return null;
                }
            }
            return response;
        }

        public int Insert(ShareEntity entity)
        {
            string insertSql = @"  INSERT INTO [Share]([Title],[Note],[CreatedBy],[CreatedOn],[Type],[IsDeleted],TicketID,ModifiedOn)
                                        VALUES(@Title, @Note, @CreatedBy, @CreatedOn, @Type, @IsDeleted,@TicketID,@ModifiedOn);
                                        SELECT ISNULL( SCOPE_IDENTITY(),0);";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(insertSql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Title", DbType.String, entity.Title);
                    db.AddInParameter(dbCommand, "Note", DbType.String, entity.Note);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, entity.CreatedBy);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, entity.CreatedOn);
                    db.AddInParameter(dbCommand, "Type", DbType.Int32, entity.Type);
                    db.AddInParameter(dbCommand, "IsDeleted", DbType.Boolean, false);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, entity.TicketID);
                    db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, entity.ModifiedOn);
                    int result;
                    object obj = db.ExecuteScalar(dbCommand);
                    if (!int.TryParse(obj.ToString(), out result))
                        return 0;
                    return result;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , insertSql, base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return 0;
                }
            }
        }

        public int InsertShareType(ShareTypeEntity entity)
        {
            string insertSql = @"   INSERT INTO [ShareType]([Title],[CreatedBy],[CreatedOn],[Type])
                                    VALUES(@Title, @CreatedBy, @CreatedOn, @Type);
                                    SELECT ISNULL( SCOPE_IDENTITY(),0);";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(insertSql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Title", DbType.String, entity.Title);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, entity.CreatedBy);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, entity.CreatedOn);
                    db.AddInParameter(dbCommand, "Type", DbType.Int32, entity.Type);
                    int result;
                    object obj = db.ExecuteScalar(dbCommand);
                    if (!int.TryParse(obj.ToString(), out result))
                        return 0;
                    return result;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , insertSql, base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return 0;
                }
            }
        }

        public bool Update(ShareEntity entity)
        {
            string insertSql = @"UPDATE [dbo].[Share]
                                   SET [Title]     = @Title
                                      ,[Note]      = @Note
                                      ,[CreatedBy] = @CreatedBy
                                      ,[CreatedOn] = @CreatedOn
                                      ,[Type]      = @Type
                                      ,TicketID    = @TicketID
                                      ,ModifiedOn  = @ModifiedOn
                                  WHERE ID = @ID";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(insertSql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Title", DbType.String, entity.Title);
                    db.AddInParameter(dbCommand, "Note", DbType.String, entity.Note);
                    db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, entity.CreatedBy);
                    db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, entity.CreatedOn);
                    db.AddInParameter(dbCommand, "Type", DbType.Int32, entity.Type);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, entity.TicketID);
                    db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, DateTime.Now);
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entity.ID);
                    var count = db.ExecuteNonQuery(dbCommand);
                    return count == 1;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , insertSql, base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        public bool Delete(int entityId)
        {
            var deleteSql = @"UPDATE [Share] SET [IsDeleted] = 1 WHERE ID = @ID";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(deleteSql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);
                    var count = db.ExecuteNonQuery(dbCommand);
                    return count == 1;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , deleteSql, base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
            return false;
        }

        public ShareEntity Get(int entityId)
        {
            string strSql = @"SELECT S.*, TypeID = ST.ID, TypeTitle = ST.[Title], TypeCreatedBy = ST.[CreatedBy],
                                TypeCreatedOn = ST.[CreatedOn],TypeType = ST.[Type] ,Files = 
                                    (select CAST(FileID AS NVARCHAR)+ '_' + FileTitle + [ContentType]+'|' from [Files] 
                                    WHERE [SourceType] = 7 AND [SourceID] = S.ID AND IsDelete = 0 FOR XML PATH('') ) 
                                FROM [Share] S  LEFT JOIN [ShareType] ST ON S.[Type] = ST.ID 
                                Where S.[IsDeleted] = 0 AND S.ID = @ID";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, entityId);

                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                        if (dataReader.Read())
                        {
                            return ShareEntity.ReaderBind(dataReader);
                        }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]"
                        , strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
            return null;
        }

    }
}
