using Microsoft.Practices.EnterpriseLibrary.Data;
using SunNet.PMNew.Core.ComplaintModel.Interfaces;
using SunNet.PMNew.Entity.ComplaintModel;
using SunNet.PMNew.Framework.Utils.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Impl.SqlDataProvider.Complaint
{
    public class ComplaintHistoryRepositorySqlDataProvider : SqlHelper, IComplaintHistoryRepository
    {
        public int Insert(ComplaintHistoryEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO ComplaintHistory( ");
            strSql.Append("ComplaintID,ModifiedBy,ModifiedOn,Comments,Action) ");
            strSql.Append("VALUES( ");
            strSql.Append("@ComplaintID,@ModifiedByID,@ModifiedOn,@Comments,@Action) ");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ComplaintID", DbType.Int32, entity.ComplaintID);
                    db.AddInParameter(dbCommand, "ModifiedByID", DbType.Int32, entity.ModifiedByID);
                    db.AddInParameter(dbCommand, "ModifiedOn", DbType.DateTime, entity.ModifiedOn);
                    db.AddInParameter(dbCommand, "Comments", DbType.String, entity.Comments);
                    db.AddInParameter(dbCommand, "Action", DbType.String, entity.Action);

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
                        strSql.ToString(),
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                    return 0;
                }
            }
        }

        public bool Update(ComplaintHistoryEntity entity)
        {
            return true;
        }

        public bool Delete(int entityId)
        {
            return true;
        }

        public ComplaintHistoryEntity Get(int entityId)
        {
            return new ComplaintHistoryEntity();
        }

        public List<ComplaintHistoryEntity> GetHistorysByComID(int cid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ch.*, u.FirstName+' '+u.LastName AS ModifiedByName from ComplaintHistory ch, Users u ");
            strSql.Append("WHERE ch.ComplaintID = @ComplaintID ");
            strSql.Append("AND ch.ModifiedBy = u.UserID ");
            strSql.Append("ORDER BY ch.ModifiedOn DESC ");

            List<ComplaintHistoryEntity> list = new List<ComplaintHistoryEntity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ComplaintID", DbType.Int32, cid);
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        while (dataReader.Read())
                        {
                            list.Add(ComplaintHistoryEntity.ReaderBind(dataReader));
                        }
                    }
                }

                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]",
                        strSql.ToString(),
                        base.FormatParameters(dbCommand.Parameters),
                        ex.Message));
                }
            }
            return list;
        }

    }
}
