using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.SealModel;

namespace SunNet.PMNew.Core.SealModel.Interfaces
{
    public interface IWorkflowHistoryRepository : IRepository<WorkflowHistoryEntity>
    {
        int UpdateReturnID(WorkflowHistoryEntity entity);
        List<WorkflowHistoryEntity> GetList(int workflowRequestID);
        bool CheckUserHasRecords(int userID, int requestID, params int[] actions);
        bool CheckRequestHasPendingRecord(int requestID);
        int InsertFirst(WorkflowHistoryEntity entity);
        bool UpdateInactiveToPending(int sealRequestID);
        bool DeleteWhereActions(int requestID, params int[] actions);
    }
}
