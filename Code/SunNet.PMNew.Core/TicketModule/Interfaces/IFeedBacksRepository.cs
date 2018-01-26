using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.Core.TicketModule
{
    public interface IFeedBacksRepository : IRepository<FeedBacksEntity>
    {
        List<FeedBacksEntity> GetFeedBackListByTicketId(int tid, bool IsSunnet, bool isSunneter
            , bool isPM, bool isSupervisor);

        List<int> UpdatePMFeedbackStatusToReplied(int ticketID);
        List<int> UpdateClientFeedbackStatusToReplied(int ticketID);

        bool ReplyFeedback(int ticketId, bool replyPM, bool replyClient);

        int CountWaiting(int ticketId);
    }
}
