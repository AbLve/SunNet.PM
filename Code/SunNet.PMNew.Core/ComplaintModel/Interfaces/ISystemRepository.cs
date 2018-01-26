using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.ComplaintModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Core.ComplaintModel.Interfaces
{
    public interface ISystemRepository : IRepository<SystemEntity>
    {
        SystemEntity SearchSystem(int entityID);
        List<SystemEntity> GetAllSystems();

        SystemEntity GetBySysName(string sysName);
    }
}
