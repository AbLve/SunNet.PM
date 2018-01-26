using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Core.SealModel;
using StructureMap;
using SunNet.PMNew.Entity.SealModel;
using System.Transactions;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Core.SealModel.Interfaces;

namespace SunNet.PMNew.App
{
    public class SealsApplication : BaseApp
    {
        private SealsManager mgr;

        public SealsApplication()
        {
            mgr = new SealsManager(
                                ObjectFactory.GetInstance<ISealRequestsRepository>()
                                , ObjectFactory.GetInstance<ISealsRepository>()
                                , ObjectFactory.GetInstance<ISealFileRepository>()
                                , ObjectFactory.GetInstance<ISealNotesRepository>()
                                , ObjectFactory.GetInstance<ISealUnionRequestsRepository>()
                                , ObjectFactory.GetInstance<IWorkflowHistoryRepository>()
                                )
                                ;
        }

        public List<SealsEntity> GetList()
        {
            return mgr.GetList();
        }

        /// <summary>
        /// 判断 SealName 是否重复，有返回 True ,否则返回 False
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sealName"></param>
        /// <returns></returns>
        public bool CheckSealName(int id, string sealName)
        {
            return mgr.CheckSealName(id, sealName);
        }

        public SealsEntity Get(int entityId)
        {
            return mgr.Get(entityId);
        }

        public bool Update(SealsEntity entity)
        {
            return mgr.Update(entity);
        }

        public int Insert(SealsEntity entity)
        {
            return mgr.Insert(entity);
        }


        /// <summary>
        /// 添加 SealRequest 并且将选中的seal 添加到 SealUnionRequest表中
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int SealRequestsInsert(SealRequestsEntity entity)
        {
            int id = 0;

            try
            {
#if !DEBUG
            using (TransactionScope tran = new TransactionScope())
#endif
                {
                    id = mgr.SealRequestsInsert(entity);
                    if (id > 0)
                    {
                        foreach (SealsEntity sealsEntity in entity.SealList)
                        {
                            int tmpId = mgr.InsertSealUnionRequests(new SealUnionRequestsEntity(sealsEntity, id));
                            if (tmpId < 1)
                                return 0;
                        }
                    }
#if !DEBUG
           tran.Complete();
#endif
                }
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
            }
            return id;
        }

        /// <summary>
        /// 修改 SealRequest 只有在 Open状态时，才可以修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SealRequestsUpdate(SealRequestsEntity entity)
        {
            try
            {
                bool result = true;
#if !DEBUG
            using (TransactionScope tran = new TransactionScope())
#endif
                {
                    List<SealUnionRequestsEntity> list = mgr.GetSealUnionRequestsList(entity.ID);
                    List<SealUnionRequestsEntity> listOld = new List<SealUnionRequestsEntity>();
                    if (mgr.SealRequestsUpdate(entity))
                    {
                        foreach (SealsEntity sealsEntity in entity.SealList)
                        {
                            SealUnionRequestsEntity unionEntity = list.Find(r => r.SealRequestsID == entity.ID && r.SealID == sealsEntity.ID);
                            if (unionEntity == null)
                            {
                                int tmpId = mgr.InsertSealUnionRequests(new SealUnionRequestsEntity(sealsEntity, entity.ID));
                                if (tmpId < 1)
                                {
                                    result = false;
                                }
                            }
                            else
                            {
                                listOld.Add(unionEntity);
                            }
                        }
                    }
                    foreach (SealUnionRequestsEntity sealsEntity in list)
                        if (listOld.Find(r => r.ID == sealsEntity.ID) == null)
                            mgr.DeleteSealUnionRequests(sealsEntity.ID);
#if !DEBUG
           tran.Complete();
#endif
                    return result;
                }
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
                return false;
            }
        }


        /// <summary>
        /// 将 SealUnionRequest 表的 Seal 状态为 sealed ，当所有 Seal 都为 Sealed状态时，将SealRequest 状态改为 sealed 
        /// </summary>
        /// <param name="sealRequestsID"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool SealRequestSealed(int sealRequestsID, int userID)
        {
            bool result = false;
            try
            {
#if !DEBUG
            using (TransactionScope tran = new TransactionScope())
#endif
                {
                    mgr.UpdateSealedDate(sealRequestsID, userID, DateTime.Now);
                    //bool resultBool = mgr.CheckRequestHasPendingRecord(sealRequestsID);  //If this request has pending records in WorkflowHistory
                    result = mgr.UpdateStatus(sealRequestsID, RequestStatus.Processed);
#if !DEBUG
           tran.Complete();
#endif
                    
                }
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
            }
            return result;
        }

        /// <summary>
        /// 将sealRequest状态改为 Approved ，只有Approver全部Approve才可以
        /// </summary>
        public bool SealRequestApproved(int sealRequestsID, int userID)
        {
            bool result = false;
            try
            {
#if !DEBUG
           using (TransactionScope tran = new TransactionScope())
#endif
                {
                    mgr.UpdateApprovedDate(sealRequestsID, userID, DateTime.Now);  
                    //bool resultBool = mgr.CheckRequestHasPendingRecord(sealRequestsID);  //If this request has pending records in WorkflowHistory
                    result = mgr.UpdateStatus(sealRequestsID, RequestStatus.Approved);

#if !DEBUG
           tran.Complete();
#endif
                }
                
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
            }
            return result;
        }





        public bool SealRequestDenied(int sealRequestsID, int approvedBy)
        {
            bool result = false;
            try
            {
                mgr.UpdateApprovedDate(sealRequestsID, approvedBy, DateTime.Now);
                result = mgr.UpdateStatus(sealRequestsID, RequestStatus.Denied);
                
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
            }
            return result;
        }


        public SealRequestsEntity GetSealRequests(int entityId)
        {
            return mgr.GetSealRequests(entityId);
        }

        public List<SealRequestsEntity> GetSealRequestsList(int userId, string keyword, int type, List<RequestStatus> status, int sealId, DateTime start, DateTime end
            , string sort, string orderby, int pageNo, int pageSize, out int recordCount)
        {
            return mgr.GetSealRequestsList(userId, keyword, type, status, sealId, start, end, sort, orderby, pageNo, pageSize, out recordCount);
        }

        public List<SealRequestsEntity> GetSealRequestsWaitingList(int userId, string keyword, int type, List<RequestStatus> status, int sealId, DateTime start, DateTime end
     , string sort, string orderby, int pageNo, int pageSize, out int recordCount)
        {
            return mgr.GetSealRequestsWaitingList(userId, keyword, type, status, sealId, start, end, sort, orderby, pageNo, pageSize, out recordCount);
        }

        public int GetSealRequestsWaitingCount(int userId, List<RequestStatus> status)
        {
            return mgr.GetSealRequestsWaitingCount(userId, status);
        }

        public int SealFilesInsert(SealFileEntity entity)
        {
            return mgr.SealFilesInsert(entity);
        }

        public bool SealFilesDelete(int entityId, int userId)
        {
            return mgr.SealFilesDelete(entityId, userId);
        }

        public List<SealFileEntity> GetSealFilesList(int sealRequestId, int wfhisID)
        {
            return mgr.GetSealFilesList(sealRequestId, wfhisID);
        }

        public SealFileEntity GetSealFiles(int id)
        {
            return mgr.GetSealFiles(id);
        }

        public List<SealNotesEntity> GetSealNotesList(int sealRequestId)
        {
            return mgr.GetSealNotesList(sealRequestId);
        }

        public int InsertSealNotes(SealNotesEntity entity)
        {
            return mgr.InsertSealNotes(entity);
        }

        public List<SealUnionRequestsEntity> GetSealUnionRequestsList(int sealRequestsId)
        {
            return mgr.GetSealUnionRequestsList(sealRequestsId);
        }

        public List<SealUnionRequestsEntity> GetSealedByList(int sealRequestsID)
        {
            return mgr.GetSealedByList(sealRequestsID);
        }

        public List<SealUnionRequestsEntity> GetApprovedByList(int sealRequestsID)
        {
            return mgr.GetApprovedByList(sealRequestsID);
        }

        /// <summary>
        /// 获取与 SealRequest 相关相关的所有用户ID
        /// </summary>
        public List<int> GetUsersId(int id)
        {
            return mgr.GetUsersId(id);
        }

        public bool UpdateStatus(int id, RequestStatus status)
        {
            return mgr.UpdateStatus(id, status);
        }


        // Work flow History
        public int WorkflowHistoryInsert(WorkflowHistoryEntity entity)
        {
            return mgr.WorkflowHistoryInsert(entity);
        }

        public int WorkflowHistoryInsertFirst(WorkflowHistoryEntity entity)
        {
            return mgr.WorkflowHistoryInsertFirst(entity);
        }

        public int WorkflowHistoryUpdate(WorkflowHistoryEntity entity)
        {
            return mgr.WorkflowHistoryUpdate(entity);
        }

        public WorkflowHistoryEntity GetWorkflowHistory(int entityId)
        {
            return mgr.GetWorkflowHistory(entityId);
        }

        public List<WorkflowHistoryEntity> GetWorkflowHistoryList(int workflowRequestID)
        {
            return mgr.GetWorkflowHistoryList(workflowRequestID);
        }

        public bool CheckUserHasRecords(int userID, int requestID, params int[] actions)
        {
            return mgr.CheckUserHasRecords(userID, requestID, actions);
        }

        public bool UpdateInactiveToPending(int sealRequestID)
        {
            return mgr.UpdateInactiveToPending(sealRequestID);
        }

        public bool DeleteWhereActions(int requestID, params int[] actions)
        {
            return mgr.DeleteWhereActions(requestID, actions);
        }
    }
}
