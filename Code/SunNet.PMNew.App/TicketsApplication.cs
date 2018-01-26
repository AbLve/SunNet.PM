using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using StructureMap;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Entity.TicketModel.TicketsDTO;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Entity.UserModel.UserModel;

namespace SunNet.PMNew.App
{
    public class TicketsApplication : BaseApp
    {
        private readonly ITicketsHistoryRepository historyRepository;
        private readonly TicketsManager mgr;
        private FeedBacksManager fdbckMgr;
        private readonly ITicketsRepository repository;
        private readonly ITicketsUserRepository ticketUserRpst;
        private readonly ITaskRespository taskRepository;
        private readonly ITicketsRelationRespository trRepository;
        private IEmailSender EmailSender;
        private bool UpdateResult;
        private ITicketEsDetailRespository esRespository;
        private int result;

        public TicketsApplication()
        {
            mgr = new TicketsManager(
                ObjectFactory.GetInstance<IEmailSender>(),
                ObjectFactory.GetInstance<ITicketsRepository>(),
                ObjectFactory.GetInstance<ICache<TicketsManager>>(),
                ObjectFactory.GetInstance<ITicketsUserRepository>(),
                ObjectFactory.GetInstance<ITicketsOrderRespository>(),
                ObjectFactory.GetInstance<ITicketsRelationRespository>(),
                ObjectFactory.GetInstance<ITaskRespository>(),
                ObjectFactory.GetInstance<ITicketsHistoryRepository>(),
                ObjectFactory.GetInstance<ITicketEsDetailRespository>());
            esRespository = ObjectFactory.GetInstance<ITicketEsDetailRespository>();
            EmailSender = ObjectFactory.GetInstance<IEmailSender>();
            repository = ObjectFactory.GetInstance<ITicketsRepository>();
            trRepository = ObjectFactory.GetInstance<ITicketsRelationRespository>();
            ticketUserRpst = ObjectFactory.GetInstance<ITicketsUserRepository>();
            taskRepository = ObjectFactory.GetInstance<ITaskRespository>();
            historyRepository = ObjectFactory.GetInstance<ITicketsHistoryRepository>();

            fdbckMgr = new FeedBacksManager(
                                    ObjectFactory.GetInstance<IEmailSender>(),
                                    ObjectFactory.GetInstance<IFeedBacksRepository>(),
                                    ObjectFactory.GetInstance<ICache<FeedBacksManager>>());
            mgr.userrespository = ObjectFactory.GetInstance<IGetTicketUser>();
        }

        #region base method ex: add/edit/update/delete

        public List<TicketsEntity> GetTicketsByCreateId(int createId)
        {
            ClearBrokenRuleMessages();

            if (createId <= 0) return null;

            List<TicketsEntity> ticketInfo = mgr.GetTicketsByCreateId(createId);

            if (ticketInfo == null)
            {
                AddBrokenRuleMessages(mgr.BrokenRuleMessages);
                return null;
            }

            return ticketInfo;
        }

        public TicketsEntity GetTickets(int tid)
        {
            ClearBrokenRuleMessages();

            if (tid <= 0) return null;

            TicketsEntity ticketInfo = repository.Get(tid);

            if (ticketInfo == null)
            {
                AddBrokenRuleMessages(mgr.BrokenRuleMessages);
                return null;
            }

            return ticketInfo;
        }

        public TicketsEntity GetTicketWithProjectTitle(int ticketID)
        {
            ClearBrokenRuleMessages();
            if (ticketID <= 0)
            {
                return null;
            }
            TicketsEntity ticketInfo = mgr.GetTicketWithProjectTitle(ticketID);

            if (ticketInfo == null)
            {
                AddBrokenRuleMessages(mgr.BrokenRuleMessages);
                return null;
            }

            return ticketInfo;
        }

        public int GetCompanyIdByTicketId(int tid)
        {
            if (tid <= 0) return 0;
            return repository.GetCompanyIdByTicketId(tid);
        }

        public int AddTickets(TicketsEntity entity)
        {
            ClearBrokenRuleMessages();
            int ticketID = mgr.AddTicket(entity);
            AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return ticketID;
        }

        public bool UpdateTickets(TicketsEntity entity, bool isUpdateStatus = true, int pmId = 0)
        {
            ClearBrokenRuleMessages();
            TicketsEntity originalTicketsEntity = GetTickets(entity.TicketID);
            var stringBuilder = new StringBuilder();

            if (isUpdateStatus && originalTicketsEntity.Status != entity.Status)
            {
                switch (entity.Status)
                {
                    case TicketsState.Cancelled:
                    case TicketsState.Completed:
                    case TicketsState.Internal_Cancel:
                    case TicketsState.Submitted:
                        entity.ResponsibleUser = 0;
                        break;
                    case TicketsState.Waiting_For_Estimation:
                        entity.ResponsibleUser = entity.EsUserID;
                        break;
                    case TicketsState.PM_Verify_Estimation:
                        entity.ResponsibleUser = pmId;
                        break;
                    case TicketsState.Waiting_Confirm:
                        entity.ResponsibleUser = entity.ConfirmEstmateUserId;
                        break;
                    case TicketsState.Denied:
                    case TicketsState.Estimation_Approved:
                        entity.ResponsibleUser = pmId;
                        break;
                    case TicketsState.Not_Approved:
                        entity.ResponsibleUser = pmId;
                        break;
                }

                stringBuilder.AppendFormat("Change status to {0}.", entity.Status.ToText());
            }
            if (originalTicketsEntity.ResponsibleUser != entity.ResponsibleUser)
            {
                string fromUser = "System";
                if (originalTicketsEntity.ResponsibleUser > 0)
                {
                    UsersEntity originalUserEntity = new UserApplication().GetUser(originalTicketsEntity.ResponsibleUser);
                    if (originalUserEntity != null)
                    {
                        fromUser = originalUserEntity.FirstName + " " + originalUserEntity.LastName;
                    }
                }
                string toUser = "System";
                if (entity.ResponsibleUser > 0)
                {
                    UsersEntity nowUserEntity = new UserApplication().GetUser(entity.ResponsibleUser);
                    if (nowUserEntity != null)
                    {
                        toUser = nowUserEntity.FirstName + " " + nowUserEntity.LastName;
                    }
                }
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" And ");
                }
                stringBuilder.AppendFormat("Change response user from {0} to {1}.", fromUser, toUser);
            }
            if (stringBuilder.Length > 0)
            {
                AddTicketStatusChangeHistory(entity.TicketID, entity.ModifiedBy, stringBuilder.ToString(), entity.ResponsibleUser);
            }
            UpdateResult = mgr.UpdateTicket(entity, isUpdateStatus);
            AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return UpdateResult;
        }

        public bool UpdateTicket(TicketsEntity entity)
        {
            ClearBrokenRuleMessages();
            TicketsEntity originalTicketsEntity = GetTickets(entity.TicketID);
            originalTicketsEntity.ProjectID = entity.ProjectID;
            originalTicketsEntity.CompanyID = entity.CompanyID;
            originalTicketsEntity.Description = entity.Description;
            originalTicketsEntity.ResponsibleUser = entity.ResponsibleUser;

            var stringBuilder = new StringBuilder();

            UpdateResult = mgr.UpdateTicket(entity, false);
            AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return UpdateResult;
        }

        public bool UpdateConfirmEstmateUserId(int ticketId, int userId)
        {
            return mgr.UpdateConfirmEstmateUserId(ticketId, userId);
        }

        public bool RemoveTicket(int tid)
        {
            ClearBrokenRuleMessages();

            if (tid <= 0) return false;

            bool result = repository.Delete(tid);

            if (result == false)
            {
                AddBrokenRuleMessage();
            }

            return result;
        }

        public bool UpdateStatus(int ticketId, TicketsState state)
        {
            return repository.UpdateStatus(ticketId, state);
        }

        public bool UpdateIsRead(int ticketId, TicketIsRead isRead)
        {
            return repository.UpdateIsRead(ticketId, isRead);
        }

        public List<TicketsEntity> GetTicketsByIds(List<int> ticketIds)
        {
            return repository.GetTicketsByIds(ticketIds);
        }

        #endregion


        #region get ticket list
        /// <summary>
        ///     Sunnet 用户登录时，获取他负责的ticket
        /// </summary>
        /// <param name="userId">当前登陆用户</param>
        /// <param name="projectId">全部project时，填0</param>
        /// <param name="status">指定的状态</param>
        /// <param name="type">不指定具体类型时，填 None</param>
        /// <returns></returns>
        public List<TicketsEntity> GetMyTicketsList(UsersEntity user, int projectId, int priority, string create,
            TicketsType type, string key, int pageNo, int pageSize
            , string sortBy, string direction, out int recordCount)
        {
            var condition = GetMyTickets(user, projectId, type, key, priority, create, pageNo, pageSize, sortBy, direction);
            var list = mgr.SearchTickets(condition);
            recordCount = condition.TotalRecords;
            return list;
        }

        /// <summary>
        /// 重载查询方法
        /// </summary>
        /// <param name="user"></param>
        /// <param name="projectId"></param>
        /// <param name="priority"></param>
        /// <param name="status"></param>
        /// <param name="create"></param>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<TicketsEntity> GetMyTicketsList(UsersEntity user, int projectId, int priority, string status, string create,
            TicketsType type, string key, int pageNo, int pageSize
            , string sortBy, string direction, out int recordCount)
        {
            var condition = GetMyTickets(user, projectId, status, type, key, priority, create, pageNo, pageSize, sortBy, direction);
            var list = mgr.SearchTickets(condition);
            recordCount = condition.TotalRecords;
            return list;
        }



        public IEnumerable<TicketsState> GetAllowStatusOfMyTicket(UsersEntity user)
        {
            List<TicketsState> ticketsStates = new List<TicketsState>();
            switch (user.Role)
            {
                case RolesEnum.PM:
                    {
                        ticketsStates = TicketsStateHelper.SunnetUSAllowShowStatus;
                        break;
                    }
                case RolesEnum.Leader:
                case RolesEnum.DEV:
                    {
                        ticketsStates = this.DevDealState;
                        break;
                    }
                case RolesEnum.QA:
                    {
                        ticketsStates = this.QaDealState;
                        break;
                    }
                case RolesEnum.Sales:
                    {
                        ticketsStates = this.SalerDealState;
                        break;
                    }

            }
            return ticketsStates;
        }

        /// <summary>
        /// 重载 构造条件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="projectId"></param>
        /// <param name="status"></param>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="priority"></param>
        /// <param name="create"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private SearchTicketCondition GetMyTickets(UsersEntity user, int projectId, string status,
           TicketsType type, string key, int priority, string create,
            int pageNo, int pageSize, string sortBy, string direction)
        {
            var condition = new SearchTicketCondition
            {
                SearchCurrentUser = true,
                UserId = user.ID
            };
            if (projectId > 0)
                condition.ProjectId = projectId;
            if (priority > 0)
                condition.Priority = (PriorityState)priority;
            if (type != TicketsType.None)
                condition.Type = type;
            if (!string.IsNullOrEmpty(key))
                condition.Keyword = key;
            if (string.IsNullOrEmpty(status))
            {
                condition.Statuses.AddRange(this.GetAllowStatusOfMyTicket(user));
            }
            else
            {
                var stutuArray = status.Split(',');
                var liststatus = stutuArray.Select(int.Parse).ToList();
                condition.MultiStaus = liststatus;
            }
            if (create.Trim() != string.Empty)
                condition.CreateUser = create;

            condition.OrderBy = sortBy;
            condition.OrderDirection = direction;
            condition.CurrentPage = pageNo;
            condition.PageCount = pageSize;
            return condition;
        }

        private SearchTicketCondition GetMyTickets(UsersEntity user, int projectId,
           TicketsType type, string key, int priority, string create,
            int pageNo, int pageSize, string sortBy, string direction)
        {
            var condition = new SearchTicketCondition
            {
                SearchCurrentUser = true,
                UserId = user.ID
            };
            if (projectId > 0)
                condition.ProjectId = projectId;
            if (priority > 0)
                condition.Priority = (PriorityState)priority;
            if (type != TicketsType.None)
                condition.Type = type;
            if (!string.IsNullOrEmpty(key))
                condition.Keyword = key;

            condition.Statuses.AddRange(TicketsStateHelper.Map(ClientTicketState.None));
            //condition.Statuses.AddRange(TicketsStateHelper.SunnetUSAllowShowStatus);
            //condition.Statuses.Remove(TicketsState.Cancelled);
            //condition.Statuses.Remove(TicketsState.Completed);

            if (create.Trim() != string.Empty)
                condition.CreateUser = create;

            condition.OrderBy = sortBy;
            condition.OrderDirection = direction;
            condition.CurrentPage = pageNo;
            condition.PageCount = pageSize;
            condition.SearchMyTicket = true;
            return condition;
        }

        public List<TicketsEntity> SearchTicketsForAllTickets(UsersEntity user, string keyword, int project, TicketsState status, string create,
            TicketsType type, int priority, int assignUser, int pageNo, int pageSize, string sortBy, string direction, out int recordCount)
        {
            var condition = GetAllTickets(user, project, status, type, keyword, priority, assignUser, create, pageNo, pageSize, sortBy, direction);
            var list = mgr.SearchTickets(condition);
            recordCount = condition.TotalRecords;
            return list;
        }
        /// <summary>
        /// 重载函数，在ALL Tickets 中使用
        /// </summary>
        /// <param name="user"></param>
        /// <param name="keyword"></param>
        /// <param name="project"></param>
        /// <param name="status"></param>
        /// <param name="create"></param>
        /// <param name="type"></param>
        /// <param name="priority"></param>
        /// <param name="assignUser"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<TicketsEntity> SearchTicketsForAllTickets(UsersEntity user, string keyword, int project, string status, string create,
            TicketsType type, int priority, int assignUser, int pageNo, int pageSize, string sortBy, string direction, bool waitingforResponse, out int recordCount)
        {
            var condition = GetAllTickets(user, project, status, type, keyword, priority, assignUser, create, pageNo, pageSize, sortBy, direction, waitingforResponse);
            var list = mgr.SearchTickets(condition);
            recordCount = condition.TotalRecords;
            return list;
        }

        /// <summary>
        /// 重载构造条件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="projectId"></param>
        /// <param name="status"></param>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="priority"></param>
        /// <param name="assignUser"></param>
        /// <param name="create"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private SearchTicketCondition GetAllTickets(UsersEntity user, int projectId, string status,
           TicketsType type, string key, int priority, int assignUser, string create,
            int pageNo, int pageSize, string sortBy, string direction, bool waitingForResponse)
        {
            var condition = new SearchTicketCondition
            {
                SearchCurrentUser = false,
                UserId = user.ID
            };
            if (projectId > 0)
                condition.ProjectId = projectId;
            if (priority > 0)
                condition.Priority = (PriorityState)priority;
            if (type != TicketsType.None)
                condition.Type = type;
            if (!string.IsNullOrEmpty(key))
                condition.Keyword = key;

            if (assignUser > 0)
                condition.ResponsibleUserId = assignUser;

            if (string.IsNullOrEmpty(status))
            {
                if (user.Role == RolesEnum.PM || user.Role == RolesEnum.Sales)
                    condition.Statuses.AddRange(TicketsStateHelper.SunnetUSAllowShowStatus);
                else
                    condition.Statuses.AddRange(TicketsStateHelper.SunnetSHAllowShowStatus);
            }
            else
            {
                var stutuArray = status.Split(',');
                var liststatus = stutuArray.Select(int.Parse).ToList();
                condition.MultiStaus = liststatus;
            }
            if (waitingForResponse)
            {
                condition.Statuses.Remove(TicketsState.Completed);
                condition.Statuses.Remove(TicketsState.Cancelled);
            }
            if (create.Trim() != string.Empty)
                condition.CreateUser = create.Trim();

            condition.OrderBy = sortBy;
            condition.OrderDirection = direction;
            condition.CurrentPage = pageNo;
            condition.PageCount = pageSize;
            return condition;
        }

        private SearchTicketCondition GetAllTickets(UsersEntity user, int projectId, TicketsState status,
           TicketsType type, string key, int priority, int assignUser, string create,
            int pageNo, int pageSize, string sortBy, string direction)
        {
            var condition = new SearchTicketCondition
            {
                SearchCurrentUser = false,
                UserId = user.ID
            };
            if (projectId > 0)
                condition.ProjectId = projectId;
            if (priority > 0)
                condition.Priority = (PriorityState)priority;
            if (type != TicketsType.None)
                condition.Type = type;
            if (!string.IsNullOrEmpty(key))
                condition.Keyword = key;

            if (assignUser > 0)
                condition.AssignUserId = assignUser;

            if (status == TicketsState.Draft)
            {
                if (user.Role == RolesEnum.PM || user.Role == RolesEnum.Sales)
                    condition.Statuses.AddRange(TicketsStateHelper.SunnetUSAllowShowStatus);
                else
                    condition.Statuses.AddRange(TicketsStateHelper.SunnetSHAllowShowStatus);
            }
            else
            {
                condition.Status = status;
            }
            if (create.Trim() != string.Empty)
                condition.CreateUser = create.Trim();

            condition.OrderBy = sortBy;
            condition.OrderDirection = direction;
            condition.CurrentPage = pageNo;
            condition.PageCount = pageSize;
            return condition;
        }

        public List<TicketsEntity> SearchTickets(SearchTicketCondition condition)
        {
            var list = mgr.SearchTickets(condition);
            return list;
        }
        public List<TicketsEntity> SearchTicketsNotInTid(SearchTicketCondition condition)
        {
            var list = mgr.SearchTicketsNotInTid(condition);
            return list;
        }
        #endregion

        #region get dto ticket

        public GetProjectIdAndUserIDResponse GetProjectIdAndUserID(int tid)
        {
            if (tid <= 0) return null;

            GetProjectIdAndUserIDResponse response = repository.GetProjectIdAndUserID(tid);

            return response;
        }

        #endregion

        #region  base task

        #endregion

        #region history

        public List<TicketHistorysEntity> GetHistoryListByTicketID(int TicketID)
        {
            ClearBrokenRuleMessages();

            if (TicketID <= 0) return null;

            List<TicketHistorysEntity> listHistory = historyRepository.GetHistoryListByTicketID(TicketID);

            if (null == listHistory || listHistory.Count <= 0) return null;

            return listHistory;
        }

        public int AddTicketHistory(TicketHistorysEntity historyEntity)
        {
            ClearBrokenRuleMessages();

            if (historyEntity == null) return 0;

            int result = historyRepository.Insert(historyEntity);

            return result > 0 ? result : 0;
        }

        #endregion

        #region Ticket Orders

        public bool RemoveAllTicketsOrderByProject(int projectID)
        {
            ClearBrokenRuleMessages();
            bool result = mgr.RemoveAllTicketsOrderByProject(projectID);
            AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }

        public int InsertTicketsOrder(TicketsOrderEntity entity)
        {
            ClearBrokenRuleMessages();
            int id = mgr.InsertTicketsOrder(entity);
            AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }

        public SearchTicketsResponse SearchTickets(SearchTicketsRequest request)
        {
            ClearBrokenRuleMessages();
            SearchTicketsResponse response = mgr.SearchTickets(request);
            AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return response;
        }

        public bool UpdateTicketStar(int ticketID, int star)
        {
            ClearBrokenRuleMessages();
            bool result = mgr.UpdateTicketStar(ticketID, star);
            AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }

        #endregion

        #region relation

        #endregion

        #region ticket User

        public List<TicketUsersEntity> GetListUsersByTicketId(int tid)
        {
            return ticketUserRpst.GetListUsersByTicketId(tid);
        }

        public List<SelectUserModel> GetSelectUsersByTicketId(int tid)
        {
            return ticketUserRpst.GetSelectUsersByTicketId(tid);
        }

        public List<TicketUsersEntity> GetListByUserId(int userId)
        {
            return ticketUserRpst.GetListByUserId(userId);
        }

        /// <summary>
        ///     查询特定用户WorkingOn的Ticket.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/27 22:18
        public List<TicketsEntity> GetWorkingOnTickets(int userid)
        {
            return mgr.GetTicketsByWorkingStatus(userid, TicketUserStatus.WorkingOn);
        }

        public int AddTicketUser(TicketUsersEntity Entity)
        {
            ClearBrokenRuleMessages();

            BaseValidator<TicketUsersEntity> validator = new AddTicketUserValidator();

            if (!validator.Validate(Entity))
            {
                AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return 0;
            }
            int result = ticketUserRpst.Insert(Entity);
            if (result <= 0)
            {
                AddBrokenRuleMessage();
            }
            return result;
        }

        public List<TicketUsersEntity> GetTicketUserList(int ticketId)
        {
            return ticketUserRpst.GetTicketUserList(ticketId);
        }

        public List<int> GetTicketUserId(int ticketId, List<int> userIds)
        {
            if (ticketId <= 0) return new List<int>();
            return ticketUserRpst.GetTicketUserId(ticketId, userIds);
        }

        public bool IsTicketUser(int tid, int uid)
        {
            return IsTicketUser(tid, uid,
                TicketUsersType.Client, TicketUsersType.Create, TicketUsersType.Dev,
                TicketUsersType.Other, TicketUsersType.PM, TicketUsersType.QA);
        }

        public bool IsTicketUser(int tid, int uid, params TicketUsersType[] types)
        {
            return ticketUserRpst.IsTicketUser(tid, uid, types.ToList());
        }

        /// <summary>
        ///     更新特定用户对特定Ticketd的工作状态
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="user">用户ID.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/27 21:23
        public bool UpdateWorkingOnStatus(int ticket, int user, TicketUserStatus status)
        {
            ClearBrokenRuleMessages();
            if (!mgr.UpdateWorkingOnStatus(ticket, user, status))
            {
                AddBrokenRuleMessages(mgr.BrokenRuleMessages);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 产生气泡，通知其它用户
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CreateNotification(int ticketId, int userId, bool notTificationClient = false)
        {
            ClearBrokenRuleMessages();
            if (!mgr.CreateNotification(ticketId, userId, notTificationClient))
            {
                AddBrokenRuleMessages(mgr.BrokenRuleMessages);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     更新指定的Ticket,显示气泡通知给指定用户
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="types">指定类型的用户将显示气泡通知.</param>
        /// <returns></returns>
        public bool UpdateNotification(int ticketId, params int[] users)
        {
            ClearBrokenRuleMessages();
            if (!mgr.UpdateNotification(ticketId, true, users.ToList(), null))
            {
                AddBrokenRuleMessages(mgr.BrokenRuleMessages);
                return false;
            }
            return true;
        }



        public bool ClearNotification(int ticketId, int userId)
        {
            ClearBrokenRuleMessages();
            if (!mgr.UpdateNotification(ticketId, false, new List<int>() { userId }, null))
            {
                AddBrokenRuleMessages(mgr.BrokenRuleMessages);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     更新指定的Ticket,覆盖指定范围用户的Ticket状态(自动设置显示气泡通知).
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="users">The users.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool UpdateTicketStatus(int ticketId, UserTicketStatus status, params int[] users)
        {
            ClearBrokenRuleMessages();
            if (!mgr.UpdateTicketStatus(ticketId, status, users.ToList(), null))
            {
                AddBrokenRuleMessages(mgr.BrokenRuleMessages);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新指定的Ticket,覆盖指定范围用户的Ticket状态(自动设置显示气泡通知).
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="users">The users.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool UpdateTicketStatus(int ticketId, UserTicketStatus status, params TicketUsersType[] types)
        {
            this.ClearBrokenRuleMessages();
            if (!mgr.UpdateTicketStatus(ticketId, status, null, types.ToList()))
            {
                this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     根据TicketState更新特定Ticket所有相关人员的Working Status.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/28 01:44
        public bool UpdateWorkingOnStatus(int ticket, TicketsState status)
        {
            ClearBrokenRuleMessages();
            var target = TicketUserStatus.None;
            if (status == TicketsState.Completed)
                target = TicketUserStatus.Completed;
            if (status == TicketsState.Cancelled
                || status == TicketsState.Denied
                || status == TicketsState.Internal_Cancel
                )
                target = TicketUserStatus.Canceled;
            if (target == TicketUserStatus.None)
                return true;
            if (!mgr.UpdateWorkingOnStatus(ticket, 0, target))
            {
                AddBrokenRuleMessages(mgr.BrokenRuleMessages);
                return false;
            }
            return true;
        }

        public TicketUsersEntity GetTicketUser(int ticket, int user)
        {
            ClearBrokenRuleMessages();
            TicketUsersEntity model = mgr.GetTicketUser(ticket, user);
            return model;
        }
        public IEnumerable<TicketUsersEntity> GetTicketUser(int ticket, TicketUsersType type)
        {
            ClearBrokenRuleMessages();
            var list = mgr.GetTicketUser(ticket, type);
            return list;
        }
        public void UpdateTicketUserType(int userID, TicketUsersType type, int ticketID)
        {
            mgr.UpdateTicketUserType(userID, type, ticketID);
        }

        public TicketUsersEntity GetTicketCreateUser(int ticketID)
        {
            var list = mgr.GetTicketUser(ticketID, TicketUsersType.Create);
            return list != null && list.Count > 0 ? list[0] : null;
        }
        public TicketUsersEntity GetTicketPM(int ticketID)
        {
            var list = mgr.GetTicketUser(ticketID, TicketUsersType.PM);
            return list != null && list.Count > 0 ? list[0] : null;
        }
        public void UpdateCreateUser(int newClientID, int ticketID)
        {
            mgr.UpdateCreateUser(newClientID, ticketID);
        }

        public bool CanFeedbackWaiting(int ticketId)
        {
            this.ClearBrokenRuleMessages();
            var count =
                mgr.CountTicketUserRecords(ticketId,
                    x =>
                        x.TicketID == ticketId &&
                        (x.TicketStatus == UserTicketStatus.WaitClientFeedback ||
                         x.TicketStatus == UserTicketStatus.WaitSunnetFeedback));
            var count2 = fdbckMgr.CountWaiting(ticketId);
            return count == 0 && count2 == 0;
        }

        public bool DeleteUserFromTicket(int ticketId, params int[] users)
        {
            return ticketUserRpst.Delete(ticketId, users.ToList(), null);
        }
        public bool DeleteUserFromTicket(int ticketId, params TicketUsersType[] types)
        {
            return ticketUserRpst.Delete(ticketId, null, types.ToList());
        }
        /// <summary>
        /// 回复Feedback,清除自己的状态提示
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>返回是否所有人都已经回复</returns>
        public bool TryClearWaiting(int ticketId, int userId, params TicketUsersType[] types)
        {
            return ticketUserRpst.TryClearWaiting(ticketId, userId, types.ToList());
        }

        public bool ClearWaitingByType(int ticketId, params TicketUsersType[] types)
        {
            return ticketUserRpst.ClearWaitingByType(ticketId, types.ToList());
        }

        #endregion

        #region task

        #endregion

        #region Update TicketUser PM

        public void UpdateTicketPM(int OldPMId, int NewPmId, int ProjectID)
        {
            ticketUserRpst.UpdateTicketPM(OldPMId, NewPmId, ProjectID);
        }

        #endregion

        #region 估时

        public int AddEsTime(TicketEsTime es)
        {
            ClearBrokenRuleMessages();
            return mgr.AddEsTime(es);
        }


        //public bool UpdateEs(decimal time, int tid, int userId, bool IsFinal)
        //{
        //    if (tid <= 0) return false;
        //    bool result = false;

        //    var item = new TicketEsTime();
        //    item.TotalTimes = time;
        //    item.CreatedTime = DateTime.Now.Date;
        //    item.EsByUserId = userId;
        //    item.IsPM = IsFinal;
        //    item.Week = "Week1";
        //    item.TicketID = tid;
        //    result = AddEsTime(item) > 0;
        //    if (!result) return false;
        //    return mgr.UpdateEs(time, tid, IsFinal);
        //}


        //public bool DeleteTicketEs(int ticketID, bool isDeleteInitial, bool isDeleteFinal)
        //{
        //    return mgr.DeleteTicketEs(ticketID, isDeleteInitial, isDeleteFinal);
        //}

        #endregion

        public int GetCancelCount(int userId, int projectId, int companyId, ClientTicketState status, TicketsType type, string key)
        {
            var condition = GetCanceled(userId, projectId, companyId, status, type, key, 1, 2, "TicketID", "Asc");
            condition.OnlyCount = true;
            var list = mgr.SearchTickets(condition);
            var recordCount = condition.TotalRecords;
            return recordCount;
        }

        public List<TicketsEntity> GetCancelList(int userId, int projectId, int companyId, ClientTicketState status, TicketsType type,
            string key, int pageNo, int pageSize, string sortBy, string direction, out int recordCount)
        {
            var condition = GetCanceled(userId, projectId, companyId, status, type, key, pageNo, pageSize, sortBy, direction);
            var list = mgr.SearchTickets(condition);
            recordCount = condition.TotalRecords;
            return list;
        }

        private SearchTicketCondition GetCanceled(int userId, int projectId, int companyId, ClientTicketState status,
           TicketsType type, string key, int pageNo, int pageSize, string sortBy, string direction)
        {
            var condition = new SearchTicketCondition
            {
                SearchCurrentUser = false,
                UserId = userId,
                IsInternal = false
            };
            if (projectId > 0)
                condition.ProjectId = projectId;
            if (companyId > 0)
                condition.CompanyId = companyId;
            if (status == ClientTicketState.None)
            {
                condition.Statuses.Add(TicketsState.Cancelled);
                condition.Statuses.Add(TicketsState.Internal_Cancel);
                condition.Statuses.Add(TicketsState.Denied);
            }
            else
            {
                condition.Statuses.AddRange(TicketsStateHelper.Map(status));
            }

            if (type != TicketsType.None)
                condition.Type = type;
            if (!string.IsNullOrEmpty(key))
                condition.Keyword = key;

            condition.OrderBy = sortBy;
            condition.OrderDirection = direction;
            condition.CurrentPage = pageNo;
            condition.PageCount = pageSize;
            return condition;
        }

        public int GetWaitingforResponseCount(int userId, int projectId, int companyId, ClientTicketState status,
            TicketsType type, string key)
        {
            var condition = GetWaiting(userId, projectId, companyId, status, type, key, 1, 2, "TicketID", "Asc");
            condition.OnlyCount = true;
            var list = mgr.SearchTickets(condition);
            var recordCount = condition.TotalRecords;
            return recordCount;
        }

        public List<TicketsEntity> GetWaitingforResponseList(int userId, int projectId, int companyId,
            ClientTicketState status, TicketsType type, string key, int pageNo, int pageSize
            , string sortBy, string direction, out int recordCount)
        {
            var condition = GetWaiting(userId, projectId, companyId, status, type, key, pageNo, pageSize, sortBy, direction);

            var list = mgr.SearchTickets(condition);
            recordCount = condition.TotalRecords;
            return list;
        }

        private SearchTicketCondition GetWaiting(int userId, int projectId, int companyId, ClientTicketState status,
            TicketsType type, string key, int pageNo, int pageSize, string sortBy, string direction)
        {
            var condition = new SearchTicketCondition
            {
                SearchCurrentUser = true,
                UserId = userId,
                IsInternal = false
            };
            if (projectId > 0)
                condition.ProjectId = projectId;
            if (companyId > 0)
                condition.CompanyId = companyId;
            // 客户Waiting for response : Ready_For_Review | Wait_Client_Feedback
            if (status == ClientTicketState.None)
            {
                condition.Statuses.Add(TicketsState.Ready_For_Review);
                condition.Statuses.Add(TicketsState.Wait_Client_Feedback);
                condition.Statuses.Add(TicketsState.Waiting_Confirm);
            }
            else
            {
                condition.Status = TicketsStateHelper.Map(status)[0];
            }

            if (type != TicketsType.None)
                condition.Type = type;
            if (!string.IsNullOrEmpty(key))
                condition.Keyword = key;

            condition.OrderBy = sortBy;
            condition.OrderDirection = direction;
            condition.CurrentPage = pageNo;
            condition.PageCount = pageSize;
            return condition;
        }

        public int GetOngoingTicketsCount(int userId, int projectId, int companyId, ClientTicketState status,
            TicketsType type, string key, bool searchIsInternal)
        {
            var condition = GetOngoing(userId, projectId, companyId, status, type, key, searchIsInternal, 1, 2, "TicketID", "Asc");
            condition.OnlyCount = true;
            var list = mgr.SearchTickets(condition);
            var recordCount = condition.TotalRecords;
            return recordCount;
        }

        public int GetMyOngoingTicketsCount(UsersEntity user, int projectId, int companyId, ClientTicketState status,
            TicketsType type, string key)
        {
            var condition = GetMyTickets(user, projectId, type, "", 0, "", 1, 2, "TicketID", "Asc");
            condition.OnlyCount = true;
            var list = mgr.SearchTickets(condition);
            var recordCount = condition.TotalRecords;
            return recordCount;
        }


        public List<TicketsEntity> GetOngoingTicketsList(int userId, int projectId, int companyId,
            ClientTicketState status, TicketsType type, string key, bool searchIsInternal, int pageNo, int pageSize
            , string sortBy, string direction, out int recordCount)
        {
            var condition = GetOngoing(userId, projectId, companyId, status, type, key, searchIsInternal, pageNo, pageSize, sortBy, direction);
            var list = mgr.SearchTickets(condition);
            recordCount = condition.TotalRecords;
            return list;
        }

        private SearchTicketCondition GetOngoing(int userId, int projectId, int companyId, ClientTicketState status,
           TicketsType type, string key, bool searchIsInternal, int pageNo, int pageSize, string sortBy, string direction)
        {
            var condition = new SearchTicketCondition
            {
                SearchCurrentUser = false,
                UserId = userId,
                IsInternal = false
            };
            condition.SearchIsInternal = searchIsInternal;
            if (projectId > 0)
                condition.ProjectId = projectId;
            if (companyId > 0)
                condition.CompanyId = companyId;
            // 客户On going
            condition.Statuses.AddRange(TicketsStateHelper.Map(status));

            if (type != TicketsType.None)
                condition.Type = type;
            if (!string.IsNullOrEmpty(key))
                condition.Keyword = key;

            condition.OrderBy = sortBy;
            condition.OrderDirection = direction;
            condition.CurrentPage = pageNo;
            condition.PageCount = pageSize;
            return condition;
        }


        public int GetDraftedTicketsCount(int userId, int projectId, int companyId, TicketsType type, string key, bool searchIsInternal)
        {
            var condition = GetOngoing(userId, projectId, companyId, ClientTicketState.Draft, type, key, searchIsInternal, 1, 2, "TicketID", "Asc");
            condition.OnlyCount = true;
            var list = mgr.SearchTickets(condition);
            var recordCount = condition.TotalRecords;
            return recordCount;
        }

        public List<TicketsEntity> GetDraftedTicketsList(int userId, int projectId, int companyId, TicketsType type,
            string key, bool searchIsInternal, int pageNo, int pageSize, string sortBy, string direction, out int recordCount)
        {
            var condition = GetOngoing(userId, projectId, companyId, ClientTicketState.Draft, type, key, searchIsInternal, pageNo, pageSize, sortBy, direction);
            var list = mgr.SearchTickets(condition);
            recordCount = condition.TotalRecords;
            return list;
        }

        public List<TicketsEntity> GetCompletedTicketsList(int userId, int projectId, int companyId, TicketsType type,
            string key, bool searchIsInternal, int pageNo, int pageSize, string sortBy, string direction, out int recordCount)
        {
            var condition = GetOngoing(userId, projectId, companyId, ClientTicketState.Completed, type, key, searchIsInternal, pageNo, pageSize, sortBy, direction);
            var list = mgr.SearchTickets(condition);
            recordCount = condition.TotalRecords;
            return list;
        }



        public string ConvertTicketTypeToTicketCode(TicketsType ticketsType)
        {
            switch (ticketsType)
            {
                case TicketsType.Bug:
                    return "B";
                case TicketsType.Request:
                    return "R";
                case TicketsType.Risk:
                    return "R";
                case TicketsType.Issue:
                    return "I";
                case TicketsType.Change:
                    return "C";
                default:
                    return "";
            }
        }

        public ClientProgressState ConvertTicketStateToClientProgressState(TicketsState state)
        {
            switch (state)
            {
                case TicketsState.Draft:
                    return ClientProgressState.Draft;
                case TicketsState.Submitted:
                case TicketsState.Not_Approved:
                    return ClientProgressState.Submit;
                case TicketsState.PM_Reviewed:
                case TicketsState.Estimation_Approved:
                case TicketsState.PM_Verify_Estimation:
                case TicketsState.Waiting_For_Estimation:
                case TicketsState.Waiting_Confirm:
                case TicketsState.PM_Deny:
                    return ClientProgressState.PM_Review;
                case TicketsState.Developing:
                    return ClientProgressState.Developing;
                case TicketsState.Testing_On_Local:
                case TicketsState.Tested_Fail_On_Local:
                case TicketsState.Tested_Success_On_Local:
                case TicketsState.Testing_On_Client:
                case TicketsState.Tested_Fail_On_Client:
                case TicketsState.Tested_Success_On_Client:
                    return ClientProgressState.Testing;
                case TicketsState.Ready_For_Review:
                    return ClientProgressState.Client_Confirm;
                case TicketsState.Completed:
                    return ClientProgressState.Completed;

                default:
                    return ClientProgressState.None;
            }
        }

        public ClientTicketState ConvertTicketStateToClientTicketState(TicketsState ticketState)
        {
            switch (ticketState)
            {
                case TicketsState.Waiting_For_Estimation:
                case TicketsState.PM_Verify_Estimation:
                case TicketsState.Waiting_Confirm:
                    return ClientTicketState.Estimating;
                case TicketsState.Denied:
                    return ClientTicketState.Denied;
                case TicketsState.Developing:
                case TicketsState.Testing_On_Local:
                case TicketsState.Tested_Fail_On_Local:
                case TicketsState.Tested_Success_On_Local:
                case TicketsState.Testing_On_Client:
                case TicketsState.Tested_Fail_On_Client:
                case TicketsState.Tested_Success_On_Client:
                case TicketsState.PM_Deny:
                case TicketsState.Estimation_Approved:
                    return ClientTicketState.In_Progress;
                case TicketsState.Ready_For_Review:
                    return ClientTicketState.Ready_For_Review;
                case TicketsState.Not_Approved:
                    return ClientTicketState.Not_Approved;
                case TicketsState.Completed:
                    return ClientTicketState.Completed;
                case TicketsState.Wait_Client_Feedback:
                    return ClientTicketState.Waiting_Client_Feedback;
                case TicketsState.Wait_Sunnet_Feedback:
                    return ClientTicketState.Waiting_Sunnet_Feedback;
                default:
                    return (ClientTicketState)ticketState;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticketsEntity">ticket 是修改前的数据</param>
        /// <param name="userInfo"></param>
        /// <param name="isRConverttoRequest">Bug 转为 Request</param>
        /// <param name="selectedStatus">选择的状态</param>
        /// <param name="isEstimation">需要估时</param>
        /// <param name="esUserID"></param>
        /// <param name="initialTime"></param>
        /// <param name="initialTime">初步估时</param>
        /// <param name="finalTime">最终估时</param>
        /// <param name="convertToRequestReason"></param>
        /// <param name="denyReason"></param>
        /// <returns></returns>
        public bool PMReview(TicketsEntity ticketsEntity, UsersEntity userInfo, bool isRConverttoRequest,
            string selectedStatus, bool isEstimation, int esUserID, int confirmEstmateUserId, decimal initialTime, decimal finalTime,
            Dictionary<string, string> selectedUsers, string convertToRequestReason, string denyReason, int pmId, int newClientId)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    AssignUser(ticketsEntity, selectedUsers, new List<int> { pmId }, userInfo);

                    if (isRConverttoRequest)
                    {
                        ConverttoRequest(ticketsEntity, userInfo, convertToRequestReason);
                    }

                    //是否更改了ticket状态
                    bool isUpdataeStatus = false;
                    if (selectedStatus != "-1")
                    {
                        var selectTicketState = (TicketsState)(int.Parse(selectedStatus));
                        if (selectTicketState != ticketsEntity.Status)
                        {
                            isUpdataeStatus = true;
                        }
                    }
                    if (isUpdataeStatus)
                    {
                        var changedTicketState = (TicketsState)(int.Parse(selectedStatus));

                        if (changedTicketState == TicketsState.PM_Reviewed)
                        {
                            if (isEstimation)
                            {
                                //估时的ticket ，走完了估时流程，最终结果不满足用户要求，重新打回，PM Reviewed 后，将状态，处理为估时通过
                                if (ticketsEntity.Status > TicketsState.Estimation_Approved)
                                {
                                    ticketsEntity.Status = TicketsState.Estimation_Approved;
                                }
                                else //需要估时的ticket ，PM Reviewed 后，将状态处理为等待估时
                                {
                                    ticketsEntity.Status = TicketsState.Waiting_For_Estimation;
                                    ticketsEntity.EsUserID = esUserID;
                                }
                                ticketsEntity.IsEstimates = true;
                            }
                            else
                            {
                                ///已确认过估时的，不能取消估时
                                if (!(ticketsEntity.Status >= TicketsState.Estimation_Approved && ticketsEntity.IsEstimates))
                                {
                                    ticketsEntity.IsEstimates = false;
                                    ticketsEntity.Status = TicketsState.PM_Reviewed;
                                }
                            }
                        }
                        else
                            ticketsEntity.Status = changedTicketState;


                        if (changedTicketState == TicketsState.Waiting_Confirm)
                        {
                            ticketsEntity.ConfirmEstmateUserId = confirmEstmateUserId;
                            ticketsEntity.InitialTime = initialTime;
                            ticketsEntity.FinalTime = finalTime;
                        }
                    }

                    ticketsEntity.ModifiedBy = userInfo.UserID;
                    ticketsEntity.ModifiedOn = DateTime.Now;

                    UpdateTickets(ticketsEntity, selectedStatus != "-1");

                    //根据TicketID，更新TicketUser中状态
                    ticketUserRpst.UpdateTicketStatus(ticketsEntity.TicketID, UserTicketStatus.Normal);

                    scope.Complete();

                    if (isUpdataeStatus)
                    {
                        switch (ticketsEntity.Status)
                        {
                            case TicketsState.Ready_For_Review:
                                if (newClientId != -1 && newClientId != ticketsEntity.CreatedBy)
                                {
                                    ticketsEntity.CreatedBy = newClientId;
                                }
                                new TicketStatusManagerApplication().SendEmailtoCreate(ticketsEntity, userInfo);
                                break;
                            case TicketsState.Cancelled:
                                {
                                    if (ticketsEntity.CreatedBy != userInfo.UserID)
                                    {
                                        new TicketStatusManagerApplication().SendEmailByCancel(ticketsEntity, userInfo);
                                    }
                                    break;
                                }
                            case TicketsState.Internal_Cancel:
                                {
                                    if (ticketsEntity.CreatedBy != userInfo.UserID)
                                    {
                                        new TicketStatusManagerApplication().SendEmailByInternalCancel(ticketsEntity, userInfo);
                                    }
                                    break;
                                }
                            case TicketsState.Waiting_Confirm:
                                new TicketStatusManagerApplication().SendEmailWaitConfirm(ticketsEntity);
                                break;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
                return false;
            }
        }

        public bool AssignUser(TicketsEntity ticketsEntity, Dictionary<string, string> selectedUsers, List<int> usUserIds, UsersEntity userEntity)
        {
            var ticketStatusMgr = new TicketStatusManagerApplication();
            List<int> userIds = new List<int>();
            foreach (var dictionaryItem in selectedUsers)
            {
                userIds.Add(int.Parse(dictionaryItem.Key));
            }
            userIds.AddRange(usUserIds);

            List<int> reservedUserIDs = GetTicketUserId(ticketsEntity.TicketID, userIds);


            bool enableEmail = !Config.IsTest;
            var entity = new TicketUsersEntity();
            entity.TicketID = ticketsEntity.TicketID;
            int result = 0;
            bool hasError = false;
            foreach (var dictionaryItem in selectedUsers)
            {
                entity.UserID = Convert.ToInt32(dictionaryItem.Key);
                entity.Type = (TicketUsersType)int.Parse(dictionaryItem.Value);
                entity.TicketStatus = UserTicketStatus.Normal;

                if (reservedUserIDs.Contains(entity.UserID) == false)
                {
                    result = AddTicketUser(entity);
                }
                else
                {
                    continue;
                }
                if (result > 0)
                {
                }
                else
                {
                    hasError = true;
                }
            }
            return !hasError;
        }

        public void SendEmailToResponsibile(TicketsEntity ticketsEntity, UsersEntity userEntity)
        {
            var ticketStatusMgr = new TicketStatusManagerApplication();

            var entity = new TicketUsersEntity();
            entity.TicketID = ticketsEntity.TicketID;
            bool hasError = false;
            entity.UserID = ticketsEntity.ResponsibleUser;
            ticketStatusMgr.SendEmailToResponsibile(entity, ticketsEntity, userEntity);
        }

        public bool AssignUsers(int ticketID, TicketUsersType type, params int[] users)
        {
            var entity = new TicketUsersEntity();
            entity.TicketID = ticketID;
            entity.Type = type;
            entity.TicketStatus = UserTicketStatus.Normal;
            entity.WorkingOnStatus = TicketUserStatus.WorkingOn;
            var existsUsers = ticketUserRpst.GetTicketUser(ticketID, type);
            int result = 0;
            bool hasError = false;
            foreach (var user in users)
            {
                if (existsUsers.Any(x => x.UserID == user))
                    continue;
                entity.UserID = user;
                result = AddTicketUser(entity);
                if (result < 1)
                    hasError = true;
            }
            return !hasError;
        }

        private bool ConverttoRequest(TicketsEntity ticketsEntity, UsersEntity userInfo, string convertToRequestReason)
        {
            ticketsEntity.TicketType = TicketsType.Request;
            ticketsEntity.Status = TicketsState.Submitted;
            ticketsEntity.TicketCode = "R";
            ticketsEntity.IsInternal = false; //?为什么这里是 False ???
            ticketsEntity.Source = userInfo.Role;
            ticketsEntity.ModifiedOn = DateTime.Now;
            ticketsEntity.ModifiedBy = userInfo.UserID;
            ticketsEntity.PublishDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
            ticketsEntity.ConvertDelete = CovertDeleteState.Normal;
            ticketsEntity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
            ticketsEntity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
            ticketsEntity.FullDescription = "Original Description:\r\n**********\r\n" + ticketsEntity.FullDescription
                                            + "\r\n-----------------\r\n" + "Bug Convert Request Reason:" +
                                            "\r\n**********\r\n" + convertToRequestReason;
            UpdateTickets(ticketsEntity, true, userInfo.ID);
            var historEntity = new TicketHistorysEntity();
            historEntity.ModifiedBy = userInfo.UserID;
            historEntity.ModifiedOn = DateTime.Now;
            historEntity.TicketID = ticketsEntity.TicketID;
            historEntity.Description = "Change ticket type to request.";
            AddTicketHistory(historEntity);
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public bool AddTicketStatusChangeHistory(int ticketID, int modifyUserID, string description, int responsibleUserId)
        {
            var historEntity = new TicketHistorysEntity();
            historEntity.ModifiedBy = modifyUserID;
            historEntity.ModifiedOn = DateTime.Now;
            historEntity.TicketID = ticketID;
            historEntity.Description = description;
            historEntity.ResponsibleUserId = responsibleUserId;
            return AddTicketHistory(historEntity) > 0;
        }

        #region Common

        /// <summary>
        ///     DEV需要处理的状态.
        /// </summary>
        /// <value>
        ///     The state of the dev deal.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/5 16:41
        public List<TicketsState> DevDealState
        {
            get
            {
                return new List<TicketsState>
                {
                    TicketsState.PM_Deny,
                    TicketsState.PM_Reviewed,
                    TicketsState.Tested_Fail_On_Client,
                    TicketsState.Tested_Fail_On_Local,
                    TicketsState.Tested_Success_On_Local,
                    TicketsState.Waiting_For_Estimation,
                    TicketsState.Estimation_Approved,
                    TicketsState.Developing,
                    TicketsState.Ready_For_Review
                };
            }
        }

        /// <summary>
        ///     需要DEV估时，下一步的状态.
        /// </summary>
        /// <value>
        ///     The estimation next.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/5 16:41
        public List<TicketsState> DevEstimationNext
        {
            get
            {
                return new List<TicketsState>
                {
                    TicketsState.PM_Verify_Estimation
                };
            }
        }

        /// <summary>
        ///     Dev处理结果.
        /// </summary>
        /// <value>
        ///     The dev next states.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/5 16:42
        public List<TicketsState> DevNextStates
        {
            get
            {
                return new List<TicketsState>
                {
                    TicketsState.Developing,
                    TicketsState.Testing_On_Local,
                    TicketsState.Testing_On_Client
                };
            }
        }

        /// <summary>
        ///     Qa需要处理的状态.
        /// </summary>
        /// <value>
        ///     The state of the qa deal.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/5 16:42
        public List<TicketsState> QaDealState
        {
            get
            {
                return new List<TicketsState>
                {
                    TicketsState.Waiting_For_Estimation,
                    TicketsState.Testing_On_Local,
                    TicketsState.Testing_On_Client,
                    TicketsState.Ready_For_Review
                };
            }
        }

        /// <summary>
        ///     Qa处理结果.
        /// </summary>
        /// <value>
        ///     The qa next states.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/5 16:42
        public List<TicketsState> QaNextStates
        {
            get
            {
                return new List<TicketsState>
                {
                    TicketsState.Tested_Success_On_Local,
                    TicketsState.Tested_Fail_On_Local,
                    TicketsState.Tested_Success_On_Client,
                    TicketsState.Tested_Fail_On_Client
                };
            }
        }

        /// <summary>
        ///     PM需要处理的状态.
        /// </summary>
        /// <value>
        ///     The state of the pm deal.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/5 16:42
        public List<TicketsState> PmDealState
        {
            get
            {
                return new List<TicketsState>
                {
                    TicketsState.Submitted,
                    TicketsState.Not_Approved,
                    TicketsState.PM_Verify_Estimation,
                    TicketsState.Tested_Success_On_Client,
                    TicketsState.Wait_Client_Feedback,
                    TicketsState.Wait_Sunnet_Feedback
                };
            }
        }

        public List<TicketsState> ClientDealState
        {
            get
            {
                return new List<TicketsState>
                {
                    TicketsState.Ready_For_Review
                };
            }
        }

        public List<TicketsState> SalerDealState
        {
            get
            {
                return new List<TicketsState>
                {
                    TicketsState.Waiting_Confirm,
                    TicketsState.Wait_Sunnet_Feedback,
                    TicketsState.Ready_For_Review
                };
            }
        }

        public List<TicketsState> SalerNextState
        {
            get { return new List<TicketsState> { TicketsState.Estimation_Approved, TicketsState.Denied }; }
        }

        #endregion

        #region 报表

        public DataTable SearchReortTickets(SearchTicketsRequest request, out int rowCount)
        {
            return mgr.SearchReortTickets(request, out rowCount);
        }

        public DataTable ReortTicketRating(SearchTicketsRequest request, out int rowCount)
        {
            return mgr.ReortTicketRating(request, out rowCount);
        }


        public List<TicketsEntity> TickectsExport(UsersEntity user, string keyword, int project,
         TicketsType type, DateTime? startTime, DateTime? endTime, int pageNo, int pageSize, string sortBy, string direction, out int recordCount)
        {
            var condition = TickectsExportCondition(user, project, type, keyword, startTime, endTime, pageNo, pageSize, sortBy, direction);
            var list = mgr.SearchTickets(condition);
            recordCount = condition.TotalRecords;
            return list;
        }
        private SearchTicketCondition TickectsExportCondition(UsersEntity user, int projectId,
           TicketsType type, string key, DateTime? startDateTime, DateTime? endDateTime,
            int pageNo, int pageSize, string sortBy, string direction)
        {
            var condition = new SearchTicketCondition
            {
                SearchCurrentUser = false,
                UserId = user.ID
            };
            if (projectId > 0)
                condition.ProjectId = projectId;

            if (type != TicketsType.None)
                condition.Type = type;
            if (!string.IsNullOrEmpty(key))
                condition.Keyword = key;
            if (startDateTime != null)
                condition.CreateStartTime = startDateTime;
            if (endDateTime != null)
                condition.CreateEndTime = endDateTime;

            condition.OrderBy = sortBy;
            condition.OrderDirection = direction;
            condition.CurrentPage = pageNo;
            condition.PageCount = pageSize;
            condition.IsInternal = false;
            return condition;
        }

        #endregion

        /// <summary>
        ///     获得指定TicketStatus的所有用户名字.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="users">The users.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public List<String> GetUsersWithStatus(int ticketId, UserTicketStatus status, params TicketUsersType[] users)
        {
            return mgr.GetUsersWithStatus(ticketId, status, users.ToList());
        }

        #region OA
        public int GetOfficeAutomationWaitingforResponseCount(int userId, int projectId, int companyId, ClientTicketState status,
            TicketsType type, string key)
        {
            var condition = GetWaiting(userId, projectId, companyId, status, type, key, 1, 2, "TicketID", "Asc");
            condition.OnlyCount = true;
            var list = mgr.SearchTickets(condition);
            var recordCount = condition.TotalRecords;
            return recordCount;
        }


        #endregion
    }
}