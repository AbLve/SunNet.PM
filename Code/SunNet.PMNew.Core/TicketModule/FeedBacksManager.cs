using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core.Validator;
using StructureMap;

namespace SunNet.PMNew.Core.TicketModule
{
    public class FeedBacksManager : BaseMgr
    {
        private IEmailSender emailSender;
        private ICache<FeedBacksManager> cache;
        private IFeedBacksRepository fbRepository;

        #region Constructor

        public FeedBacksManager(
            IEmailSender emailSender,
            IFeedBacksRepository fbRepository,
            ICache<FeedBacksManager> cache
            )
        {
            this.emailSender = emailSender;
            this.fbRepository = fbRepository;
            this.cache = cache;
        }

        #endregion

        public List<int> UpdatePMFeedbackStatusToReplied(int ticketID)
        {
            return fbRepository.UpdatePMFeedbackStatusToReplied(ticketID);
        }

        public List<int> UpdateClientFeedbackStatusToReplied(int ticketID)
        {
            return fbRepository.UpdateClientFeedbackStatusToReplied(ticketID);
        }

        public bool ReplyFeedback(int ticketId, bool replyPM, bool replyClient)
        {
            return fbRepository.ReplyFeedback(ticketId, replyPM, replyClient);
        }

        public int CountWaiting(int ticketId)
        {
            return fbRepository.CountWaiting(ticketId);
        }
    }
}
