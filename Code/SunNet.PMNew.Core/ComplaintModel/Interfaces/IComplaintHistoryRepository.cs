using SunNet.PMNew.Entity.ComplaintModel;
using SunNet.PMNew.Framework.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Core.ComplaintModel.Interfaces
{
    public interface IComplaintHistoryRepository : IRepository<ComplaintHistoryEntity>
    {
        List<ComplaintHistoryEntity> GetHistorysByComID(int cid);
    }
}
