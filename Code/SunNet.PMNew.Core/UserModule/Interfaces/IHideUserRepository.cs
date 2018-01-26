
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.UserModule
{
    public interface IHideUserRepository : IRepository<HideUserEntity>
    {
        HideUserEntity GetHideUserByUserId(int userId);
        int IsExistDataByUserId(int userId);
    }
}
