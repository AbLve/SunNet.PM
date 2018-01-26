using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyBook.Entity
{
    /// <summary>
    /// MaintenancePlanOption
    /// </summary>
    public enum UserMaintenancePlanOption
    {
        /// <summary>
        /// Has no maintenance plan,not a really value,choose a option from NEEDAPPROVAL | DONTNEEDAPPROVAL | ALLOWME
        /// </summary>
        NO = 3,
        /// <summary>
        /// Has a maintenance plan
        /// </summary>
        HAS = 2,
        /// <summary>
        /// Does not have a maintenance plan ,Needs a quote approval
        /// </summary>
        NEEDAPPROVAL = 11,
        /// <summary>
        /// Does not have a maintenance plan ,Does not need a quote approval
        /// </summary>
        DONTNEEDAPPROVAL = 12,
        /// <summary>
        /// Does not have a maintenance plan ,Allow me to choose per submission
        /// </summary>
        ALLOWME = 13,
        /// <summary>
        /// For Sunneters
        /// </summary>
        NONE = 0
    }
}