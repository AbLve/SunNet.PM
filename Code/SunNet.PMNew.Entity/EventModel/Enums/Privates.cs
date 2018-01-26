using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.EventModel
{
    public enum Privates
    {
        /// <summary>
        /// 只限自己 ,在页面中以 O 表示
        /// </summary>
        OnlyMe = 1,

        /// <summary>
        /// 表示所有好友，在页面中以 P 表示
        /// </summary>
        Public = 2,

        /// <summary>
        /// 组ID
        /// </summary>
        Roles = 3,

        /// <summary>
        /// 特邀 以 0 表示
        /// </summary>
        Invites = 4
    }
}
