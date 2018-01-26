using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Core.UserModule.Enum
{
    public static class RolesEnum
    {
        public enum Roles
        {
            /// <summary>
            /// Administrator
            /// </summary>
            ADMIN = 1,
            /// <summary>
            /// Project Manager
            /// </summary>
            PM = 2,
            /// <summary>
            /// Developer
            /// </summary>
            DEV = 3,
            /// <summary>
            /// QA
            /// </summary>
            QA = 4,
            /// <summary>
            /// Clients
            /// </summary>
            CLIENT = 5
        }
    }
}
