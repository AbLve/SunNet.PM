using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SF.Framework;
using SF.Framework.Helpers;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core;
using ReminderEmail.ReminderModel;

namespace ReminderEmail.ReminderDal
{
    public class ReminderDetailDal
    {
        public List<ReminderModel.ReminderModel> GetReminderTickets(UsersEntity user, DateTime startTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT p.ProjectID AS 'ProjectID', p.Title AS 'ProjectName', T.TicketID AS 'TicketId', T.Title AS 'TicketTitle', T.ModifiedOn FROM dbo.Tickets AS T ");
            strSql.Append("LEFT JOIN dbo.Projects AS P ");
            strSql.Append("ON T.ProjectID = P.ProjectID ");
            strSql.Append("WHERE T.Status NOT IN(2, 19, 20) AND T.ResponsibleUser = @ResponsibleUser AND T.ModifiedOn < @QueryDate ");
            strSql.Append("ORDER BY T.ModifiedOn ASC ");

            Database db = DatabaseFactory.CreateDatabase("PM");
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                db.AddInParameter(dbCommand, "ResponsibleUser", DbType.Int32, user.UserID);
                db.AddInParameter(dbCommand, "QueryDate", DbType.Date, startTime);

                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    List<ReminderModel.ReminderModel> list = new List<ReminderModel.ReminderModel>();
                    while (dataReader.Read())
                    {
                        ReminderModel.ReminderModel ticket = new ReminderModel.ReminderModel();
                        ticket.ProjectId = dataReader["ProjectId"].ToInt32();
                        ticket.ProjectName= dataReader["ProjectName"].ToString();
                        ticket.TicketId = dataReader["TicketId"].ToInt32();
                        ticket.TicketTitle = dataReader["TicketTitle"].ToString();
                        ticket.ModifiedOn = dataReader["ModifiedOn"].ToDateTime();

                        list.Add(ticket);
                    }
                    return list;
                }
            }
        }
    }
}
