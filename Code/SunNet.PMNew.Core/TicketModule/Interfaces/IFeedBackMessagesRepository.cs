using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.Core.TicketModule
{
    public interface IFeedBackMessagesRepository : IRepository<FeedBackMessagesEntity>
    {
        bool Delete(int TicketID, int UserID);
        List<FeedBackMessagesEntity> GetList(int userID);
        bool DeleteInternalMessage(int TicketID);
        bool DeleteClientFeedbackMessages(int ticketID, int userID, List<int> feedbackEntityIDs);
        bool DeletePMFeedbackMessages(int ticketID, int userID, List<int> feedbackEntityIDs);
        bool DeleteFeedbackMessage(int ticketID, int userID);
    }
}
