using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Framework.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Core.EventsModule
{
    public interface IEventCommentsRepository : IRepository<EventCommentEntity>
    {
    }
}
