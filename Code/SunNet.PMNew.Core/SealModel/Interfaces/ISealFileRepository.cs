using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Framework.Core.Repository;

namespace SunNet.PMNew.Core.SealModel
{
    public interface ISealFileRepository : IRepository<SealFileEntity> 
    {
        bool Delete(int entityId, int userId);

        List<SealFileEntity> GetList(int sealRequestId, int wfhisID);
    }
}
