using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Framework.Core.Repository;

namespace SunNet.PMNew.Core.SealModel
{
    public interface ISealRequestsRepository : IRepository<SealRequestsEntity> 
    {
        List<SealRequestsEntity> GetList(int userId, string keyword, int type, List<RequestStatus> status, int sealId, DateTime start, DateTime end, string sort, string orderby, int pageNo, int pageSize, out int recordCount);
        List<SealRequestsEntity> GetWaitingList(int userId, string keyword, int type, List<RequestStatus> status, int sealId, DateTime start, DateTime end, string sort, string orderby, int pageNo, int pageSize, out int recordCount);
        int GetWaitingCount(int userId, List<RequestStatus> status);
        void UpdateSealedStatus(int id);
        bool UpateStatus(int id, RequestStatus status);
        /// <summary>
        /// 获取与 SealRequest 相关相关的所有用户ID
        /// </summary>
        List<int> GetUsersId(int id);
    }
}
