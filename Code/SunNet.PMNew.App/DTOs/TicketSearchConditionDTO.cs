using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.App.DTOs
{
    class TicketSearchConditionDTO
    {
        public string KeyWord { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public string Project { get; set; }

        public string AssignedUser { get; set; }

        public string Company { get; set; }

        public string Client { get; set; }

        public string ClientPriority { get; set; }

        public bool PriorityView { get; set; }

        public TicketSearchConditionDTO(string KeyWord, string Status, string Type,
                                        string Project, string AssignedUser,
                                        string Company, string Client,
                                        string ClientPriority, bool PriorityView
                                        )
        {
            this.KeyWord = KeyWord;
            this.Status = Status;
            this.Type = Type;
            this.Project = Project;
            this.AssignedUser = AssignedUser;
            this.Company = Company;
            this.Client = Client;
            this.ClientPriority = ClientPriority;
            this.PriorityView = PriorityView;
        }

    }
}
