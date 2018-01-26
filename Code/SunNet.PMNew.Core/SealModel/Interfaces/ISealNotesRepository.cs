using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Framework.Core.Repository;

namespace SunNet.PMNew.Core.SealModel
{
    public interface ISealNotesRepository : IRepository<SealNotesEntity> 
    {
        List<SealNotesEntity> GetList(int sealRequestId);
    }
}
