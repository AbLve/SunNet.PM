using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SunNet.PMNew.Entity.SealModel.Enum;

namespace SunNet.PMNew.Entity.SealModel
{
    public class WorkflowHistoryEntity
    {
        //public SealUnionRequestsEntity(IDataReader dataReader, bool isList)
        //{
        //    ID = (int)dataReader["ID"];
        //    SealRequestsID = (int)dataReader["SealRequestsID"];
        //    SealID = (int)dataReader["SealID"];
        //    ApprovedBy = (int)dataReader["ApprovedBy"];
        //    ApprovedDate = (DateTime)dataReader["ApprovedDate"];
        //    SealedBy = (int)dataReader["SealedBy"];
        //    SealedDate = (DateTime)dataReader["SealedDate"];
        //    IsSealed = (bool)dataReader["IsSealed"];
        //}

        ///// <summary>
        ///// 扩展实体
        ///// </summary>
        ///// <param name="dataReader"></param>
        //public SealUnionRequestsEntity(IDataReader dataReader)
        //{
        //    UserID = (int)dataReader["UserID"];
        //    Email = (string)dataReader["Email"];
        //    FirstName = (string)dataReader["FirstName"];
        //    LastName = (string)dataReader["LastName"];
        //}


        public static WorkflowHistoryEntity ReaderBind(IDataReader dataReader)
        {
            WorkflowHistoryEntity wfhisEntity = new WorkflowHistoryEntity();
            object obj;

            //obj = dataReader["ProcessedBy"];
            //if (obj != null && obj != DBNull.Value)
            //{
            //    wfhisEntity.ProcessedBy = (int)obj;
            //}

            obj = dataReader["ID"];
            if (obj != null && obj != DBNull.Value)
            {
                wfhisEntity.ID = (int)obj;
            }

            wfhisEntity.ProcessedByName = dataReader["ProcessedByName"].ToString();

            obj = dataReader["ProcessedTime"];
            if (obj != null && obj != DBNull.Value)
            {
                wfhisEntity.ProcessedTime = (DateTime)obj;
            }

            obj = dataReader["Action"];
            if (obj != null && obj != DBNull.Value)
            {
                wfhisEntity.Action = (WorkflowAction)obj;
            }

            wfhisEntity.Comment = dataReader["Comment"].ToString();

            return wfhisEntity;
        }

        public int ID { get; set; }

        public int WorkflowRequestID { get; set; }

        public DateTime CreatedTime { get; set; }

        public int ProcessedBy { get; set; }

        public string ProcessedByName { get; set; }

        public DateTime ProcessedTime { get; set; }

        public WorkflowAction Action { get; set; }

        public string Comment { get; set; }

        public List<SealFileEntity> lstFiles { get; set; }


    }
}
