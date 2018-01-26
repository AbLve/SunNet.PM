using SunNet.PMNew.Entity.Common;
using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ComplaintModel
{
    public class ComplaintEntity : BaseEntity
    {
        public static ComplaintEntity ReaderBind(System.Data.IDataReader dataReader)
        {
            ComplaintEntity complaintEntity = new ComplaintEntity();
            object obj;

            obj = dataReader["ComplaintID"];
            if (obj != null && obj != DBNull.Value)
            {
                complaintEntity.ComplaintID = (int)obj;
            }

            obj = dataReader["Type"];
            if (obj != null && obj != DBNull.Value)
            {
                complaintEntity.Type = (int)obj;
            }

            obj = dataReader["TargetID"];
            if (obj != null && obj != DBNull.Value)
            {
                complaintEntity.TargetID = (int)obj;
            }

            obj = dataReader["Reason"];
            if (obj != null && obj != DBNull.Value)
            {
                complaintEntity.Reason = (int)obj;
            }

            complaintEntity.AdditionalInfo = dataReader["AdditionalInfo"].ToString();
            complaintEntity.SystemName = dataReader["SystemName"].ToString();

            obj = dataReader["AppSrc"];
            if (obj != null && obj != DBNull.Value)
            {
                complaintEntity.AppSrc = (int)obj;
            }

            obj = dataReader["Status"];
            if (obj != null && obj != DBNull.Value)
            {
                complaintEntity.Status = (int)obj;
            }

            obj = dataReader["CreatedOn"];
            if (obj != null && obj != DBNull.Value)
            {
                complaintEntity.CreatedOn = (DateTime)obj;
            }

            obj = dataReader["UpdatedOn"];
            if (obj != null && obj != DBNull.Value)
            {
                complaintEntity.UpdatedOn = (DateTime)obj;
            }

            complaintEntity.UpdatedByName = dataReader["UpdatedByName"].ToString();

            return complaintEntity;

        }

        public int ComplaintID { get; set; }
        public int Type { get; set; }
        public int TargetID { get; set; }
        public int Reason { get; set; }
        public string AdditionalInfo { get; set; }
        public int SystemID { get; set; }
        public string SystemName { get; set; }
        public int AppSrc { get; set; }
        public int ReporterID { get; set; }
        public string ReporterEmail { get; set; }
        public int Status { get; set; }
        public string Comments { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedByID { get; set; }
        public string UpdatedByName { get; set; }
        
    }
}


