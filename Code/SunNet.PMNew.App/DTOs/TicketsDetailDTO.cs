using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.App.DTOs
{
    public class TicketsDetailDTO
    {
        #region Ticket Attribute

        public int ID { get; set; }
        public int TicketID { get; set; }
        public int CompanyID { get; set; }
        public int ProjectID { get; set; }
        public string Title { get; set; }
        public string TicketType { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreateUserName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime PublishDate { get; set; }
        public bool ClientPublished { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int ContinueDate { get; set; }
        public string URL { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public int ConvertDelete { get; set; }
        public bool IsInternal { get; set; }
        public int CreateType { get; set; }
        public int SourceTicketID { get; set; }
        public bool IsEstimates { get; set; }
        public int DevTsHours { get; set; }
        public int QaTsHours { get; set; }
        public int Hours { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }

        #endregion
    }
}
