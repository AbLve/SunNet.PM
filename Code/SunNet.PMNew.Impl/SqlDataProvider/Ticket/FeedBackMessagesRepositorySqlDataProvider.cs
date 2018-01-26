using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Impl.SqlDataProvider.Ticket
{
    public class FeedBackMessagesRepositorySqlDataProvider : SqlHelper, IFeedBackMessagesRepository
    {

        bool IFeedBackMessagesRepository.Delete(int TicketID, int UserID)
        {
            throw new NotImplementedException();
        }

        List<FeedBackMessagesEntity> IFeedBackMessagesRepository.GetList(int userID)
        {
            throw new NotImplementedException();
        }

        bool IFeedBackMessagesRepository.DeleteInternalMessage(int TicketID)
        {
            throw new NotImplementedException();
        }

        bool IFeedBackMessagesRepository.DeleteClientFeedbackMessages(int ticketID, int userID, List<int> feedbackEntityIDs)
        {
            throw new NotImplementedException();
        }

        bool IFeedBackMessagesRepository.DeletePMFeedbackMessages(int ticketID, int userID, List<int> feedbackEntityIDs)
        {
            throw new NotImplementedException();
        }

        bool IFeedBackMessagesRepository.DeleteFeedbackMessage(int ticketID, int userID)
        {
            throw new NotImplementedException();
        }

        int Framework.Core.Repository.IRepository<FeedBackMessagesEntity>.Insert(FeedBackMessagesEntity entity)
        {
            throw new NotImplementedException();
        }

        bool Framework.Core.Repository.IRepository<FeedBackMessagesEntity>.Update(FeedBackMessagesEntity entity)
        {
            throw new NotImplementedException();
        }

        bool Framework.Core.Repository.IRepository<FeedBackMessagesEntity>.Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        FeedBackMessagesEntity Framework.Core.Repository.IRepository<FeedBackMessagesEntity>.Get(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
