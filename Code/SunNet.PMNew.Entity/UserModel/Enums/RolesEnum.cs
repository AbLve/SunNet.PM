using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.UserModel
{
    public enum RolesEnum : int
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
        CLIENT = 5,
        /// <summary>
        /// Sales
        /// </summary>
        Sales = 6,
        /// <summary>
        /// Leader 
        /// </summary>
        Leader = 7,

        Contactor = 10,

        Supervisor = 16
    }
}