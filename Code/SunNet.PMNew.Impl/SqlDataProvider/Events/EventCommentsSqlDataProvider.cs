using Microsoft.Practices.EnterpriseLibrary.Data;
using SunNet.PMNew.Core.EventsModule;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Framework.Utils.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Impl.SqlDataProvider.Events
{
    public partial class EventCommentsSqlDataProvider :SqlHelper, IEventCommentsRepository
    {
        public int Insert(EventCommentEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into dbo.EventComments(");
            strSql.Append("EventID,UserID,Context,CreateOn,ParentID)");
            strSql.Append(" values (");
            strSql.Append("@EventID,@UserID,@Context,@CreateOn,@ParentID)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "EventID", DbType.Int32, entity.EventID);
                    db.AddInParameter(dbCommand, "UserID", DbType.Int32, entity.UserID);
                    db.AddInParameter(dbCommand, "Context", DbType.String, entity.Context);
                    db.AddInParameter(dbCommand, "CreateOn", DbType.DateTime, entity.CreateOn);
                    db.AddInParameter(dbCommand, "ParentID", DbType.Int32, entity.ParentID);

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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return 0;
                }
            }
        }

        public bool Update(EventCommentEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public EventCommentEntity Get(int entityId)
        {
            throw new NotImplementedException();
        }

    }
}
