using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.TicketModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Core.TicketModule;

namespace SunNet.PMNew.Impl.SqlDataProvider.Ticket
{
    public class TicketEsDetailRepositorySqlDataProvider : SqlHelper, ITicketEsDetailRespository
    {
        public TicketEsTime Get(int EsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EsID,TicketID,Week,QaAdjust,DevAdjust,GrapTime,DocTime,TrainingTime,TotalTimes,EsByUserId,CreatedTime,Remark,IsPM from EsDetail ");
            strSql.Append(" where EsID=@EsID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "EsID", DbType.Int32, EsID);
                    TicketEsTime model = null;
                    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                    {
                        if (dataReader.Read())
                        {
                            model = TicketEsTime.ReaderBind(dataReader);
                        }
                    }
                    return model;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return null;
                }
            }
        }

        public bool Update(TicketEsTime model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update EsDetail set ");
            strSql.Append("TicketID=@TicketID,");
            strSql.Append("Week=@Week,");
            strSql.Append("QaAdjust=@QaAdjust,");
            strSql.Append("DevAdjust=@DevAdjust,");
            strSql.Append("GrapTime=@GrapTime,");
            strSql.Append("DocTime=@DocTime,");
            strSql.Append("TrainingTime=@TrainingTime,");
            strSql.Append("TotalTimes=@TotalTimes,");
            strSql.Append("EsByUserId=@EsByUserId,");
            strSql.Append("CreatedTime=@CreatedTime,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("IsPM=@IsPM");
            strSql.Append(" where EsID=@EsID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "EsID", DbType.Int32, model.EsID);
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                    db.AddInParameter(dbCommand, "Week", DbType.String, model.Week);
                    db.AddInParameter(dbCommand, "QaAdjust", DbType.Decimal, model.QaAdjust);
                    db.AddInParameter(dbCommand, "DevAdjust", DbType.Decimal, model.DevAdjust);
                    db.AddInParameter(dbCommand, "GrapTime", DbType.Decimal, model.GrapTime);
                    db.AddInParameter(dbCommand, "DocTime", DbType.Decimal, model.DocTime);
                    db.AddInParameter(dbCommand, "TrainingTime", DbType.Decimal, model.TrainingTime);
                    db.AddInParameter(dbCommand, "TotalTimes", DbType.Decimal, model.TotalTimes);
                    db.AddInParameter(dbCommand, "EsByUserId", DbType.Int32, model.EsByUserId);
                    db.AddInParameter(dbCommand, "CreatedTime", DbType.DateTime, model.CreatedTime);
                    db.AddInParameter(dbCommand, "Remark", DbType.AnsiString, model.Remark);
                    db.AddInParameter(dbCommand, "IsPM", DbType.Boolean, model.IsPM);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }

            }
        }

        public int Insert(TicketEsTime model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into EsDetail(");
            strSql.Append("TicketID,Week,QaAdjust,DevAdjust,GrapTime,DocTime,TrainingTime,TotalTimes,EsByUserId,CreatedTime,Remark,IsPM)");

            strSql.Append(" values (");
            strSql.Append("@TicketID,@Week,@QaAdjust,@DevAdjust,@GrapTime,@DocTime,@TrainingTime,@TotalTimes,@EsByUserId,@CreatedTime,@Remark,@IsPM)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();

            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "TicketID", DbType.Int32, model.TicketID);
                    db.AddInParameter(dbCommand, "Week", DbType.String, model.Week);
                    db.AddInParameter(dbCommand, "QaAdjust", DbType.Decimal, model.QaAdjust);
                    db.AddInParameter(dbCommand, "DevAdjust", DbType.Decimal, model.DevAdjust);
                    db.AddInParameter(dbCommand, "GrapTime", DbType.Decimal, model.GrapTime);
                    db.AddInParameter(dbCommand, "DocTime", DbType.Decimal, model.DocTime);
                    db.AddInParameter(dbCommand, "TrainingTime", DbType.Decimal, model.TrainingTime);
                    db.AddInParameter(dbCommand, "TotalTimes", DbType.Decimal, model.TotalTimes);
                    db.AddInParameter(dbCommand, "EsByUserId", DbType.Int32, model.EsByUserId);
                    db.AddInParameter(dbCommand, "CreatedTime", DbType.DateTime, model.CreatedTime);
                    db.AddInParameter(dbCommand, "Remark", DbType.AnsiString, model.Remark);
                    db.AddInParameter(dbCommand, "IsPM", DbType.Boolean, model.IsPM);
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

        public bool Delete(int EsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from EsDetail ");
            strSql.Append(" where EsID=@EsID ");
            Database db = DatabaseFactory.CreateDatabase();

            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "EsID", DbType.Int32, EsID);
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
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }
            }

        }

    }
}
