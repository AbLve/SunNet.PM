using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.InvoiceModel.Enums
{
    public enum InvoiceStatus
    {
        [Description("Missing Milestone")]
        Missing_Milestone = 1,
        [Description("Missing Invoice")]
        Missing_Invoice = 2,
        [Description("Invoice Created")]
        Invoice_Created = 3,
        [Description("Awaiting Send")]
        Awaiting_Send = 4,
        [Description("Awaiting Payment")]
        Awaiting_Payment = 5,
        [Description("Payment Received")]
        Payment_Received = 6,
        Waive = 7,
        [Description("Payment Confirmed")]
        Payment_Confirmed = 8
    }
}
