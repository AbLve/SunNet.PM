using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Core.TicketModule;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core.Validator;


namespace SunNet.PMNew.App
{
    public class FeedBackApplication : BaseApp
    {
        private FeedBacksManager mgr;
        private IFeedBacksRepository repository;

        public FeedBackApplication()
        {
            mgr = new FeedBacksManager(
                                    ObjectFactory.GetInstance<IEmailSender>(),
                                    ObjectFactory.GetInstance<IFeedBacksRepository>(),
                                    ObjectFactory.GetInstance<ICache<FeedBacksManager>>());
            repository = ObjectFactory.GetInstance<IFeedBacksRepository>();

        }

        public int AddFeedBacks(FeedBacksEntity entity)
        {
            this.ClearBrokenRuleMessages();

            BaseValidator<FeedBacksEntity> validator = new AddFeedBacksValidator();


            if (!validator.Validate(entity))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return 0;
            }
            int result = repository.Insert(entity);
            if (result <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }

            return result;
        }

        public FeedBacksEntity GetFeedBacksEntity(int id)
        {
            this.ClearBrokenRuleMessages();

            if (id <= 0) return null;

            FeedBacksEntity entity = repository.Get(id);

            if (entity == null)
            {
                this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
                return null;
            }

            return entity;
        }

        public List<FeedBacksEntity> GetFeedBackListByTicketId(int tid, bool IsSunnet, bool isSunneter, bool isPM, bool isSupervisor)
        {
            this.ClearBrokenRuleMessages();

            if (tid <= 0) return null;

            List<FeedBacksEntity> list = repository.GetFeedBackListByTicketId(tid, IsSunnet, isSunneter, isPM, isSupervisor);
            if (null == list || list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public bool DeleteFeedback(int feedbackID)
        {
            bool result = false;
            this.ClearBrokenRuleMessages();
            if (feedbackID <= 0)
            {
                return false;
            }
            else
            {
                result = repository.Delete(feedbackID);
                if (!result)
                {
                    this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
                }
                return result;
            }
        }

        /// <summary>
        /// 设置所有需要回复的Feedback为已回复.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="replyPM"></param>
        /// <param name="replyClient"></param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ReplyFeedback(int ticketId, bool replyPM, bool replyClient)
        {
            return mgr.ReplyFeedback(ticketId, replyPM, replyClient);
        }

        /// <summary>
        /// 查看Ticket有几条等待回复的消息
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public int CountWaiting(int ticketId)
        {
            return mgr.CountWaiting(ticketId);
        }

    }
}
