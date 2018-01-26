
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace EventSendAlertEmail
{
    public class EventEntity
    {
        public EventEntity()
        {
        }

        public EventEntity(IDataReader reader)
        {
            ID = (int) reader["ID"];
            Icon = (int) reader["Icon"];
            Name = (string) reader["Name"];
            Details = (string) reader["Details"];
            Where = (string) reader["Where"];
            AllDay = (bool) reader["AllDay"];
            FromDay = (DateTime) reader["FromDay"];
            FromTime = (string) reader["FromTime"];
            FromTimeType = (int) reader["FromTimeType"];
            ToDay = (DateTime) reader["ToDay"];
            ToTime = (string) reader["ToTime"];
            ToTimeType = (int) reader["ToTimeType"];
            HasInvite = (bool) reader["HasInvite"];
            hasAlert = (bool) reader["HasAlert"];
            GroupID = (string) reader["GroupID"];
            CreatedBy = (int) reader["CreatedBy"];
            CreatedOn = (DateTime) reader["CreatedOn"];
            UpdatedOn = (DateTime) reader["UpdatedOn"];
            CreateUserFristName = (string) reader["CreateUserFirstName"];
            CreateUserLastName = (string) reader["CreateUserLastName"];
            CreateEmailAccount = (string) reader["CreateUserEmail"];
        }

        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// 图标ID
        /// </summary>
        public int Icon
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Details
        {
            get;
            set;
        }

        public string Where
        {
            get;
            set;
        }

        public bool AllDay
        {
            get;
            set;
        }

        public DateTime FromDay
        {
            get;
            set;
        }

        /// <summary>
        /// AllDay 时为0 ；
        /// </summary>
        public string FromTime
        {
            get;
            set;
        }

        /// <summary>
        /// AllDay 时为0; AM=1 or PM=2
        /// </summary>
        public int FromTimeType
        {
            get;
            set;
        }

        public DateTime ToDay
        {
            get;
            set;
        }

        /// <summary>
        /// AllDay 时为0 ； 
        /// </summary>
        public string ToTime
        {
            get;
            set;
        }

        /// <summary>
        /// AllDay 时为0; AM=1 or PM=2
        /// </summary>
        public int ToTimeType
        {
            get;
            set;
        }

        /// <summary>
        /// 有邀请人
        /// </summary>
        public bool HasInvite
        {
            get;
            set;
        }

        /// <summary>
        /// Privacy 对应的值，以逗号分开
        /// </summary>
        public string GroupID
        {
            get;
            set;
        }

        /// <summary>
        /// 聚集索引
        /// </summary>
        public int CreatedBy
        {
            get;
            set;
        }

        public string CreateUserFristName
        {
            get;
            set;
        }

        public string CreateUserLastName
        {
            get;
            set;
        }

        public string CreateEmailAccount
        {
            get;
            set;
        }

        public DateTime CreatedOn
        {
            get;
            set;
        }

        public DateTime UpdatedOn
        {
            get;
            set;
        }

        private bool hasAlert = false;
        public bool HasAlert
        {
            get
            {
                return hasAlert;
            }
            set
            {
                hasAlert = value;
            }
        }

        /// <summary>
        ///  获取需要alert的Events。没有数据时list.count==0
        /// </summary>
        public List<EventEntity> GetEventsNeedAlert()
        {
            List<EventEntity> list = new List<EventEntity>();
            string strSql = "GetNeedAlertEvents";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PM2012"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand(strSql, conn))
            {
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        list.Add(new EventEntity(reader));

                }
                catch (Exception ex)
                {
                    LogProvider.WriteLog(ex.ToString());
                }
            }
            return list;
        }

        public bool Update(EventEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dbo.Events ");
            strSql.Append("SET Icon=@Icon, Name=@Name,Details=@Details,[Where]=@Where,AllDay=@AllDay,FromDay=@FromDay,FromTime=@FromTime")
            .Append(",FromTimeType=@FromTimeType,ToDay=@ToDay,ToTime=@ToTime,ToTimeType=@ToTimeType,UpdatedOn=@UpdatedOn ")
            .Append(" ,GroupID=@GroupID ,HasInvite=@HasInvite,HasAlert=@HasAlert")
            .Append(" where ID=@ID");

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PM2012"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand(strSql.ToString(), conn))
            {
                try
                {
                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "Icon",
                        DbType = DbType.Int32,
                        Value = entity.Icon
                    });
                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "Name",
                        DbType = DbType.String,
                        Value = entity.Name
                    });
                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "Details",
                        DbType = DbType.String,
                        Value = entity.Details
                    });
                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "Where",
                        DbType = DbType.String,
                        Value = entity.Where
                    });
                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "AllDay",
                        DbType = DbType.Boolean,
                        Value = entity.AllDay
                    });
                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "FromDay",
                        DbType = DbType.DateTime,
                        Value = entity.FromDay
                    });
                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "FromTime",
                        DbType = DbType.String,
                        Value = entity.FromTime

                    });
                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "FromTimeType",
                        DbType = DbType.Int32,
                        Value = entity.FromTimeType

                    });

                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "ToDay",
                        DbType = DbType.DateTime,
                        Value = entity.ToDay.Date

                    });

                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "ToTime",
                        DbType = DbType.String,
                        Value = entity.ToTime

                    });

                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "ToTimeType",
                        DbType = DbType.Int32,
                        Value = entity.ToTimeType

                    });
                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "GroupID",
                        DbType = DbType.String,
                        Value = entity.GroupID == null ? "" : entity.GroupID
                    });

                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "UpdatedOn",
                        DbType = DbType.DateTime,
                        Value = entity.UpdatedOn

                    });
                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "HasInvite",
                        DbType = DbType.Boolean,
                        Value = entity.HasInvite

                    });
                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "HasAlert",
                        DbType = DbType.Boolean,
                        Value = entity.HasAlert

                    });
                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "ID",
                        DbType = DbType.Int32,
                        Value = entity.ID

                    });

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    LogProvider.WriteLog(ex.ToString());
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
