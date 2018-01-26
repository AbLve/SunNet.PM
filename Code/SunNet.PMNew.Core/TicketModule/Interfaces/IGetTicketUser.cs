using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.UserModule;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.TicketModule
{
    public interface IGetTicketUser
    {
        /// <summary>
        /// get entity By uid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        UsersEntity GetUserInfo(int uid);
    }
}
