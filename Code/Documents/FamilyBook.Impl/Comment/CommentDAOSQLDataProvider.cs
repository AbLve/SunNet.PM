using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Text;

using FamilyBook.Core.CommentModule;
using FamilyBook.Entity;
using FamilyBook.Entity.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SF.Framework.Log;

namespace FamilyBook.Impl.Comment
{
    public class CommentDAOSQLDataProvider : ICommentDAO
    {

        public int Insert(CommentEntity entity)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" insert into Comment(Name,Content,ReplyID,Office,UserType,UserID,DocumentID) ");
            sb.Append(" values(@Name,@Content,@ReplyID,@Office,@UserType,@UserID,@DocumentID); ");
            sb.Append(" select isnull(@@identity,0); ");
            Database db = DatabaseFactory.CreateDatabase();

            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "@Name", DbType.String, entity.Name);
                    db.AddInParameter(dbCommand, "@Content", DbType.String, entity.Content);
                    db.AddInParameter(dbCommand, "@ReplyID", DbType.Int32, entity.ReplyID);
                    db.AddInParameter(dbCommand, "@Office", DbType.String, entity.Office);
                    db.AddInParameter(dbCommand, "@UserType", DbType.String, entity.UserType);
                    db.AddInParameter(dbCommand, "@UserID", DbType.String, entity.UserID);
                    db.AddInParameter(dbCommand, "@DocumentID", DbType.String, entity.DocumentID);
                    return Convert.ToInt32(db.ExecuteScalar(dbCommand));
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(sb.ToString(), dbCommand.Parameters, ex);
                    return 0;
                }
            }
        }

        public bool Update(CommentEntity entity)
        {
            return true;
        }

        /// <summary>
        /// 首先检查id是否是评论还是回复
        /// 若是评论，先删除与之有关的回复
        /// 若是回复，直接删除即可
        /// </summary>
        /// <param name="id">字段ID</param>
        /// <returns>是否删除成功</returns>
        public bool Delete(int id)
        {
            List<CommentEntity> commentList = GetListByDynamic("replyID=" + id);
            StringBuilder sb = new StringBuilder();
            if (commentList != null && commentList.Count > 0)
            {
                sb.Append(" delete comment where id in( ");
                for (int i = 0; i < commentList.Count; i++)
                {
                    sb.AppendFormat("{0},", commentList[i].ID);
                    if (i == commentList.Count - 1)
                        sb.Append(id);
                }
                sb.Append(")");
            }
            else
                sb.AppendFormat(" delete comment where id = {0}", id);
            Database db = DatabaseFactory.CreateDatabase();
            int num = db.ExecuteNonQuery(CommandType.Text, sb.ToString());
            return num > 0;
        }

        public CommentEntity Get(int id)
        {
            return new CommentEntity();
        }

        public List<CommentEntity> GetListByDynamic(string strSelect)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from Comment ");
            if (strSelect.Length > 0)
                strSql.Append(" where " + strSelect);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    List<CommentEntity> entityList = new List<CommentEntity>();
                    IDataReader reader = db.ExecuteReader(dbCommand);
                    while (reader.Read())
                    {
                        entityList.Add(new CommentEntity(reader));
                    }
                    reader.Close();
                    return entityList;
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(strSql.ToString(), dbCommand.Parameters, ex);
                    return null;
                }
            }
        }

        public List<CommentEntity> GetList(Func<CommentEntity, bool> f)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from Comment ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    List<CommentEntity> allList = new List<CommentEntity>();
                    IDataReader reader = db.ExecuteReader(dbCommand);
                    while (reader.Read())
                    {
                        allList.Add(new CommentEntity(reader));
                    }
                    reader.Close();

                    List<CommentEntity> commentList = allList.Where(c => c.ReplyID == 0).Where(f).ToList();
                    foreach (CommentEntity item in commentList)
                    {
                        item.ReplyList = allList.Where(o => o.ReplyID == item.ID).ToList();
                    }

                    return commentList;
                }
                catch (Exception ex)
                {
                    new log4netProvider().LogSQL(strSql.ToString(), dbCommand.Parameters, ex);
                    return null;
                }
            }
        }

    }
}
