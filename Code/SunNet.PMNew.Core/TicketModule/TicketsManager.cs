using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Entity.TicketModel.TicketsDTO;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Core.UserModule;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel;
using System.Data;

namespace SunNet.PMNew.Core.TicketModule
{
    public class TicketsManager : BaseMgr
    {
        #region declare

        private ITicketEsDetailRespository esRepository;
        private IEmailSender emailSender;
        private ITicketsRepository ticketRepository;
        private ICache<TicketsManager> cache;
        private ITicketsUserRepository ticketUserRpst;
        private ITicketsOrderRespository ticketOrderRespository;
        private ITicketsRelationRespository trRespository;
        private ITaskRespository taskRepository;
        private ITicketsHistoryRepository HistoryRepository;
        private bool UpdateResult;
        public IGetTicketUser userrespository { get; set; }
        #endregion

        #region Constructor
        public TicketsManager(
                                 IEmailSender emailSender,
                                 ITicketsRepository ticketsRespository,
                                 ICache<TicketsManager> cache,
                                 ITicketsUserRepository tUserRespository,
                                 ITicketsOrderRespository ticketOrderRespository,
                                 ITicketsRelationRespository trRespository,
                                 ITaskRespository taskRespository,
                                 ITicketsHistoryRepository HistoryRepository,
                                 ITicketEsDetailRespository EsRepository
                             )
        {
            this.emailSender = emailSender;
            this.ticketRepository = ticketsRespository;
            this.cache = cache;
            this.ticketUserRpst = tUserRespository;
            this.trRespository = trRespository;
            this.ticketOrderRespository = ticketOrderRespository;
            this.taskRepository = taskRespository;
            this.HistoryRepository = HistoryRepository;
            this.esRepository = EsRepository;
        }
        #endregion

        #region ticket

        public int AddTicket(TicketsEntity te)
        {
            this.ClearBrokenRuleMessages();

            BaseValidator<TicketsEntity> validator = new AddTicketsValidator();

            if (!validator.Validate(te))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return 0;
            }

            int id = ticketRepository.Insert(te);

            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }
            //string errorMessage = "";
            //send email
            // emailSender.SendMail("Hacksl@sunnet.us", "service@sninfotechco.com", "test", "", "", out errorMessage);

            te.ID = id;

            cache[string.Format("TicketDetail::{0}", te.ID)] = te;

            return id;
        }

        public bool UpdateTicket(TicketsEntity te, bool isUpdateStatus)
        {
            this.ClearBrokenRuleMessages();

            BaseValidator<TicketsEntity> validator = new UpdateTicketValidator();

            if (null == te) return false;

            if (!validator.Validate(te))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return false;
            }
            UpdateResult = ticketRepository.Update(te, isUpdateStatus);

            if (!UpdateResult)
            {
                this.AddBrokenRuleMessage();
                return false;
            }

            cache[string.Format("TicketDetail::{0}", te.ID)] = te;

            return UpdateResult;
        }

        public bool UpdateConfirmEstmateUserId(int ticketId, int userId)
        {
            return ticketRepository.UpdateConfirmEstmateUserId(ticketId, userId);
        }

        public List<TicketsEntity> GetTicketsByIds(List<int> ticketIds)
        {
            return ticketRepository.GetTicketsByIds(ticketIds);
        }

        #endregion

        #region Ticket user

        /// <summary>
        /// 更新特定用户对特定Ticketd的工作状态
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="user">用户ID，如果要对该Ticket的所有关联用户生效，请传入0.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/27 20:37
        public bool UpdateWorkingOnStatus(int ticket, int user, TicketUserStatus status)
        {
            this.ClearBrokenRuleMessages();
            if (!ticketUserRpst.UpdateWorkingOnStatus(ticket, user, status))
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新指定的Ticket,显示/清除气泡通知给指定类型用户
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="showNtfctn">显示或清除通知.</param>
        /// <param name="users">The users.</param>
        /// <param name="types">指定类型的用户将显示气泡通知.</param>
        /// <returns></returns>
        public bool UpdateNotification(int ticketId, bool showNtfctn, List<int> users, List<TicketUsersType> types)
        {
            this.ClearBrokenRuleMessages();
            if (!ticketUserRpst.UpdateNotification(ticketId, showNtfctn, users, types))
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 产生气泡，通知其它用户
        /// </summary>
        public bool CreateNotification(int ticketId, int userId, bool notTificationClient = true)
        {
            this.ClearBrokenRuleMessages();
            if (!ticketUserRpst.CreateNotification(ticketId, userId, notTificationClient))
            {
                this.AddBrokenRuleMessage();
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
        /// <returns></returns>
        public bool UpdateTicketStatus(int ticketId, UserTicketStatus status, List<int> users, List<TicketUsersType> types)
        {
            this.ClearBrokenRuleMessages();
            if (!ticketUserRpst.UpdateTicketStatus(ticketId, status, users, types))
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            return true;
        }

        public TicketUsersEntity GetTicketUser(int ticket, int user)
        {
            this.ClearBrokenRuleMessages();
            var model = ticketUserRpst.Get(ticket, user);
            return model;
        }

        public List<TicketsEntity> GetTicketsByWorkingStatus(int userid, TicketUserStatus status)
        {
            return ticketRepository.GetTicketsByWorkingStatus(userid, status);
        }

        public List<TicketsEntity> GetTicketsByCreateId(int createId)
        {
            return ticketRepository.GetTicketsByCreateId(createId);
        }

        public void UpdateTicketUserType(int userID, TicketUsersType type, int ticketID)
        {
            ticketUserRpst.UpdateTicketUserType(userID, type, ticketID);
        }


        public List<TicketUsersEntity> GetTicketUser(int ticketID, TicketUsersType type)
        {
            return ticketUserRpst.GetTicketUser(ticketID, type);
        }

        public void UpdateCreateUser(int newClientID, int ticketID)
        {
            ticketUserRpst.UpdateCreateUser(newClientID, ticketID);
        }

        /// <summary>
        /// Counts the specified condition
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public int CountTicketUserRecords(int ticketId, Expression<Func<TicketUsersEntity, bool>> condition)
        {
            var lists = ticketUserRpst.GetListUsersByTicketId(ticketId);
            return lists.Count(condition.Compile());
        }

        #endregion

        #region Ticket Orders

        public bool RemoveAllTicketsOrderByProject(int projectID)
        {
            this.ClearBrokenRuleMessages();
            bool result = ticketOrderRespository.RemoveAllTicketsOrderByProject(projectID);
            if (!result)
            {
                this.AddBrokenRuleMessage();
            }
            return result;
        }

        public int InsertTicketsOrder(TicketsOrderEntity entity)
        {
            this.ClearBrokenRuleMessages();
            int id = ticketOrderRespository.Insert(entity);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
            }
            return id;
        }

        public SearchTicketsResponse SearchTickets(SearchTicketsRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchTicketsResponse response = ticketRepository.SearchTickets(request);
            if (response.IsError)
            {
                this.AddBrokenRuleMessage();
            }
            return response;
        }

        #endregion

        public bool UpdateTicketStar(int ticketID, int star)
        {
            this.ClearBrokenRuleMessages();
            int result = ticketRepository.Update(ticketID, star);
            if (result <= 0)
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            else
            {
                return true;
            }
        }


        #region  es time base method

        public bool UpdateEs(decimal time, int tid, bool IsFinal)
        {
            if (tid <= 0) return false;
            return ticketRepository.UpdateEs(time, tid, IsFinal);
        }

        public int AddEsTime(TicketEsTime es)
        {
            this.ClearBrokenRuleMessages();

            BaseValidator<TicketEsTime> validator = new AddTicketEsValidator();

            if (!validator.Validate(es))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return 0;
            }

            int id = esRepository.Insert(es);

            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }

            return id;
        }

        public bool DeleteTicketEs(int ticketID, bool isDeleteInital, bool isDeleteFinal)
        {
            return ticketRepository.DeleteTicketEs(ticketID, isDeleteInital, isDeleteFinal);
        }

        #endregion


        public TicketsEntity GetTicketWithProjectTitle(int ticketID)
        {
            return ticketRepository.GetTicketWithProjectTitle(ticketID);
        }

        public List<TicketsEntity> SearchTickets(SearchTicketCondition condition)
        {
            this.ClearBrokenRuleMessages();
            var response = ticketRepository.SearchTickets(condition);
            return response;
        }

        public List<TicketsEntity> SearchTicketsNotInTid(SearchTicketCondition condition)
        {
            this.ClearBrokenRuleMessages();
            var response = ticketRepository.SearchTicketsNotInTid(condition);
            return response;
        }
        #region 报表
        public DataTable SearchReortTickets(SearchTicketsRequest request, out int totalRows)
        {
            return ticketRepository.SearchReortTickets(request, out totalRows);
        }
        public DataTable ReortTicketRating(SearchTicketsRequest request, out int totalRows)
        {
            return ticketRepository.ReortTicketRating(request, out totalRows);
        }

        #endregion

        /// <summary>
        /// 获得指定TicketStatus的所有用户名字.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="users">The users.</param>
        /// <returns></returns>
        public List<String> GetUsersWithStatus(int ticketId, UserTicketStatus status, List<TicketUsersType> types)
        {
            return ticketUserRpst.GetUsersWithStatus(ticketId, status, types);
        }
    }
}
