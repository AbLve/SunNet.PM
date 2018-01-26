using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.TicketModel.UserTicket;

namespace SunNet.PMNew.Entity.ProjectModel.ProjectTicket
{
    public class ProjectTicketModel
    {
        public int ProjectID { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public int PMID { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int OngoingTickets { get; set; }
        public List<TicketStatusModel> Tickets { get; set; }
    }
}
