using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Framework.Core.Repository;

namespace SunNet.PMNew.Core.SealModel
{
    public interface ISealUnionRequestsRepository : IRepository<SealUnionRequestsEntity>
    {
        List<SealUnionRequestsEntity> GetList(int sealRequestsID);
        bool UpdateApprovedDate(int sealRequestId, int userID, DateTime date);
        bool UpdateSealedDate(int sealRequestId, int userID, DateTime date);
        List<SealUnionRequestsEntity> GetSealedByList(int sealRequestsID);
        List<SealUnionRequestsEntity> GetApprovedByList(int sealRequestsID);
    }
}
