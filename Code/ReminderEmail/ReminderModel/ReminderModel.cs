using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.ProjectModule;
using SunNet.PMNew.Entity.UserModel;

namespace ReminderEmail.ReminderModel
{
    public class ReminderModel
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public int TicketId { get; set; }

        public string TicketTitle { get; set; }

        public DateTime? ModifiedOn { get; set; }

    }
}
