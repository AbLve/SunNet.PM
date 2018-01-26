using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.TicketModel
{
    public enum ClientTicketState
    {
        [Description("All")]
        None = -1,
        Draft = 0,
        Submitted = 1,
        Cancelled = 2,
        Estimating = 3,

        /// <summary>
        /// Estimation Fail 改变为 Denied
        /// </summary>
        [Description("Denied")]
        Denied = 4,
        [Description("In Progress")]
        In_Progress = 5,
        [Description("Waiting for Client's feedback")]
        Waiting_Client_Feedback = 6,
        [Description("Ready For Review")]
        Ready_For_Review = 7,
        [Description("Not Approved")]
        Not_Approved = 8, //ClientDeny
        Completed = 9, // ClientApp
        [Description("Waiting for SunNet's feedback")]
        Waiting_Sunnet_Feedback = 10
    }
}
