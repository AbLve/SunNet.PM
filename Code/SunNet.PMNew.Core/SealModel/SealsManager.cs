using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Core.SealModel.Interfaces;

namespace SunNet.PMNew.Core.SealModel
{
    public class SealsManager : BaseMgr
    {
        private ISealRequestsRepository requestsRepository;
        private ISealsRepository sealsRepository;
        private ISealFileRepository fileRepository;
        private ISealNotesRepository notesRepository;
        private ISealUnionRequestsRepository unionRepository;
        private IWorkflowHistoryRepository wfhisRepository;

        #region Constructor

         public SealsManager(
                                  ISealRequestsRepository requestsRepository,
             ISealsRepository sealsRepository, ISealFileRepository fileRepository,
             ISealNotesRepository notesRepository,ISealUnionRequestsRepository unionRepository,
             IWorkflowHistoryRepository wfhisRepository
                                 )
        {
            this.requestsRepository = requestsRepository;
            this.sealsRepository = sealsRepository;
            this.fileRepository = fileRepository;
            this.notesRepository = notesRepository;
            this.unionRepository = unionRepository;
            this.wfhisRepository = wfhisRepository;
        }

        #endregion


         public List<SealsEntity> GetList()
         {
             return sealsRepository.GetList();
         }

        /// <summary>
        /// 判断 SealName 是否重复，有返回 True ,否则返回 False
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sealName"></param>
        /// <returns></returns>
         public bool CheckSealName(int id, string sealName)
         {
             return sealsRepository.CheckSealName(id, sealName);
         }

         public SealsEntity Get(int entityId)
         {
             return sealsRepository.Get(entityId);
         }

         public bool Update(SealsEntity entity)
         {
             return sealsRepository.Update(entity);
         }

         public int Insert(SealsEntity entity)
         {
             return sealsRepository.Insert(entity);
         }

         public int SealRequestsInsert(SealRequestsEntity entity)
         {
             return requestsRepository.Insert(entity);
         }

         public bool SealRequestsUpdate(SealRequestsEntity entity)
         {
             return requestsRepository.Update(entity);
         }

         public SealRequestsEntity GetSealRequests(int entityId)
         {
             return requestsRepository.Get(entityId);
         }

         public void UpdateSealedStatus(int id)
         {
             requestsRepository.UpdateSealedStatus(id);
         }

         public  bool UpdateStatus(int id, RequestStatus status)
         {
             return requestsRepository.UpateStatus(id, status);
         }

         public List<SealRequestsEntity> GetSealRequestsList(int userId, string keyword, int type, List<RequestStatus> status, int sealId, DateTime start, DateTime end
             , string sort, string orderby, int pageNo, int pageSize, out int recordCount)
         {
             return requestsRepository.GetList(userId, keyword, type, status, sealId, start, end, sort, orderby, pageNo, pageSize, out recordCount);
         }

         public List<SealRequestsEntity> GetSealRequestsWaitingList(int userId, string keyword, int type, List<RequestStatus> status, int sealId, DateTime start, DateTime end
              , string sort, string orderby, int pageNo, int pageSize, out int recordCount)
         {
             return requestsRepository.GetWaitingList(userId, keyword, type, status, sealId, start, end, sort, orderby, pageNo, pageSize, out recordCount);
         }

        public int GetSealRequestsWaitingCount(int userId, List<RequestStatus> status)
        {
            return requestsRepository.GetWaitingCount(userId, status);
        }

        public int SealFilesInsert(SealFileEntity entity)
         {
             return  fileRepository.Insert(entity);
         }

         public bool SealFilesDelete(int entityId, int userId)
         {
             return fileRepository.Delete(entityId, userId);
         }

         public List<SealFileEntity> GetSealFilesList(int sealRequestId, int wfhisID)
         {
             return fileRepository.GetList(sealRequestId, wfhisID);
         }

         public SealFileEntity GetSealFiles(int id)
         {
             return fileRepository.Get(id);
         }

         public List<SealNotesEntity> GetSealNotesList(int sealRequestId)
         {
             return notesRepository.GetList(sealRequestId);
         }

         public int InsertSealNotes(SealNotesEntity entity)
         {
             return notesRepository.Insert(entity);
         }

         public int InsertSealUnionRequests(SealUnionRequestsEntity entity)
         {
             return unionRepository.Insert(entity);
         }

         public bool DeleteSealUnionRequests(int ID)
         {
             return unionRepository.Delete(ID);
         }

         public List<SealUnionRequestsEntity> GetSealUnionRequestsList(int sealRequestsID)
         {
             return unionRepository.GetList(sealRequestsID);
         }

         public bool UpdateApprovedDate(int sealRequestId, int userID, DateTime date)
         {
             return unionRepository.UpdateApprovedDate(sealRequestId, userID, date);
         }

         public bool UpdateSealedDate(int sealRequestId, int userID, DateTime date)
         {
             return unionRepository.UpdateSealedDate(sealRequestId, userID, date);
         }

         public List<SealUnionRequestsEntity> GetSealedByList(int sealRequestsID)
         {
             return unionRepository.GetSealedByList(sealRequestsID);
         }

         public List<SealUnionRequestsEntity> GetApprovedByList(int sealRequestsID)
         {
             return unionRepository.GetApprovedByList(sealRequestsID);
         }

         /// <summary>
         /// 获取与 SealRequest 相关相关的所有用户ID
         /// </summary>
         public List<int> GetUsersId(int id)
         {
             return requestsRepository.GetUsersId(id);
         }



        // Work Flow History
         public int WorkflowHistoryInsert(WorkflowHistoryEntity entity)
         {
             return wfhisRepository.Insert(entity);
         }

         public int WorkflowHistoryInsertFirst(WorkflowHistoryEntity entity)
         {
             return wfhisRepository.InsertFirst(entity);
         }

         public int WorkflowHistoryUpdate(WorkflowHistoryEntity entity)
         {
             return wfhisRepository.UpdateReturnID(entity);
         }

         public WorkflowHistoryEntity GetWorkflowHistory(int entityId)
         {
             return wfhisRepository.Get(entityId);
         }

         public List<WorkflowHistoryEntity> GetWorkflowHistoryList(int workflowRequestID)
         {
             return wfhisRepository.GetList(workflowRequestID);
         }

         public bool CheckUserHasRecords(int userID, int requestID, params int[] actions)
         {
             return wfhisRepository.CheckUserHasRecords(userID, requestID, actions);
         }

         public bool CheckRequestHasPendingRecord(int requestID)
         {
             return wfhisRepository.CheckRequestHasPendingRecord(requestID);
         }

         public bool UpdateInactiveToPending(int sealRequestID)
         {
             return wfhisRepository.UpdateInactiveToPending(sealRequestID);
         }

         public bool DeleteWhereActions(int requestID, params int[] actions)
         {
             return wfhisRepository.DeleteWhereActions(requestID, actions);
         }
    }
}
