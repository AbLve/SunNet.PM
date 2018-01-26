using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.UserModule
{
    public interface IModulesRepository : IRepository<ModulesEntity>
    {
        List<ModulesEntity> GetAllModules(int parentID, int page, int pageCount);
        int GetAllModulesCount(int parentID);
        List<ModulesEntity> GetModulesList(int roleID);
        bool RemoveAll(int roleID);
    }
}
