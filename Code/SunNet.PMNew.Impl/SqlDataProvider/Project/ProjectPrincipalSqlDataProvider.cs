using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Core.ProjectModule;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Core.ProjectModule.Interfaces;

namespace SunNet.PMNew.Impl.SqlDataProvider.Project
{
    public class ProjectPrincipalSqlDataProvider : SqlHelper, IProjectPrincipalRepository
    {
        public ProjectPrincipalSqlDataProvider() { }

        public int Insert(ProjectPrincipalEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ProjectPrincipal(");
            strSql.Append("ProjectID,Module,PM,DEV,QA)");
            strSql.Append(" values (");
            strSql.Append("@ProjectID,@Module,@PM,@DEV,@QA)");
            strSql.Append(";select ISNULL( SCOPE_IDENTITY(),0);");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, model.ProjectID);
                    db.AddInParameter(dbCommand, "Module", DbType.String, model.Module);
                    db.AddInParameter(dbCommand, "PM", DbType.String, model.PM);
                    db.AddInParameter(dbCommand, "DEV", DbType.String, model.DEV);
                    db.AddInParameter(dbCommand, "QA", DbType.String, model.QA);
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

        public bool Update(ProjectPrincipalEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ProjectPrincipal ");
            strSql.Append(" set Module=@Module ,PM=@PM,DEV=@DEV,QA=@QA ");
            strSql.AppendFormat(" where id = {0}", model.ID);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "Module", DbType.String, model.Module);
                    db.AddInParameter(dbCommand, "PM", DbType.String, model.PM);
                    db.AddInParameter(dbCommand, "DEV", DbType.String, model.DEV);
                    db.AddInParameter(dbCommand, "QA", DbType.String, model.QA);

                    return db.ExecuteNonQuery(dbCommand) > 0;
                }
                catch (Exception ex)
                {
                    WebLogAgent.Write(string.Format("[SQLText:{0},{1}Messages:\r\n{2}]", strSql.ToString(), base.FormatParameters(dbCommand.Parameters), ex.Message));
                    return false;
                }

            }
        }


        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProjectPrincipal ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString()))
            {
                try
                {
                    db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
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
        public ProjectPrincipalEntity Get(int ID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from ProjectPrincipal ")
                .AppendFormat(" where ID={0} " ,ID);

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                        return ProjectPrincipalEntity.ReaderBind(dataReader);
                    else
                        return null;
                }
            } 
        }


        public List<ProjectPrincipalEntity> GetProjectPrincipal(int projectId)
        {
            List<ProjectPrincipalEntity> list = new List<ProjectPrincipalEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("select id,projectid,module,pm,dev,qa from ProjectPrincipal ")
                .AppendFormat(" where ProjectID=@ProjectID ");

            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString()))
            {
                db.AddInParameter(dbCommand, "ProjectID", DbType.Int32, projectId);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                        list.Add(ProjectPrincipalEntity.ReaderBind(dataReader));
                }
            }
            return list;
        }
    }
}
