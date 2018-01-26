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
    public partial class EventsRepositorySqlDataProvider : SqlHelper, IEventRepository
    {
        #region EventShares
        #endregion



        #region EventInvit

        public List<EventInviteEntity> GetEventInvites(int eventId)
        {
            List<EventInviteEntity> list = new List<EventInviteEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" SELECT * FROM dbo.EventInvites WHERE EventID={0}", eventId);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    using (IDataReader reader = db.ExecuteReader(dbCommand))
                    {
                        while (reader.Read())
                        {
                            list.Add(new EventInviteEntity(reader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                }
            }
            return list;
        }

        /// <summary>
        /// 删除 UserId= 0 ; 或者 UserId > 0 and optionStatu =3
        /// </summary>
        /// <param name="inviteList"></param>
        /// <returns></returns>
        public bool RemoveInviteUser(List<EventInviteEntity> inviteList)
        {
            if (inviteList.Count == 0) return true;
            string[] userIds = inviteList.Where(r => r.UserID > 0 && r.OptionStatus == 3).Select(r => r.UserID.ToString()).ToArray();
            StringBuilder sb = new StringBuilder();
            if (userIds.Length > 0)
            {
                sb.AppendFormat(" delete from EventInvites where EventID={0} and UserID in ({1}) ;",
                    inviteList[0].EventID, string.Join(",", userIds));
            }
            sb.AppendFormat(" delete from EventInvites where EventID={0}  and UserID = 0",inviteList[0].EventID);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                try
                {
                    db.ExecuteNonQuery(dbCommand);
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", sb.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
            return true;
        }

        public bool AddEventInvites(EventEntity entity, List<EventInviteEntity> inviteList)
        {
            if (inviteList == null || inviteList.Count() == 0) return true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" insert into dbo.EventInvites (CreatedID,EventID,UserID,Status,FromDay,Email,FirstName,LastName)");
            bool isOne = true;
            foreach (EventInviteEntity item in inviteList)
            {
                if (isOne)
                {
                    strSql.AppendFormat(" select {0} , {1} , {2} , 1 , @Date ,'{3}','{4}','{5}'", entity.CreatedBy, entity.ID, item.UserID
                        , item.Email, item.FirstName, item.LastName);
                    isOne = false;
                }
                else
                {
                    strSql.AppendFormat(" Union select {0} , {1} , {2} , 1 , @Date ,'{3}','{4}','{5}'", entity.CreatedBy, entity.ID, item.UserID
                        , item.Email, item.FirstName, item.LastName);
                }
            }
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "date", DbType.DateTime, entity.FromDay.Date);
                    return db.ExecuteNonQuery(dbCommand) > 0;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }
        }

        /// <summary>
        /// 0:失败 ;1:邀请; 2:加入; 3:拒绝; 4:忽略
        /// </summary>
        public int GetStatus(int userId, int eventId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select [Status] from dbo.EventInvites ")
                .AppendFormat(" where UserID={0} and eventId={1} ", userId, eventId);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    IDataReader reader = db.ExecuteReader(dbCommand);
                    if (reader.Read())
                        return (int)reader[0];
                    return 0;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return 0;
                }
            }
        }

        #endregion
    }
}
