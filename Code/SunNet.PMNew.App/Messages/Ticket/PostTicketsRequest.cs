using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Core.TicketModule.Enums;

namespace SunNet.PMNew.App.Messages.Ticket
{
    class PostTicketsRequest
    {
        #region attribute
        public int PostByUserID { get; set; }
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int ProjectID { get; set; }
        public string Title { get; set; }
        public string TicketType { get; set; }
        public string Description { get; set; }
        public string CreateUserName { get; set; }
        public bool ClientPublished { get; set; }
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
        public DateTime PublishDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime Hours { get; set; }
        #endregion

        public TicketsEntity ToBusinessEntity(ISystemDateTime timeProvider)
        {
            TicketsEntity entity = TicketsFactory.Create(this.PostByUserID, timeProvider);

            entity.Title = string.Empty;
            entity.URL = string.Empty;
            entity.CompanyID = 0;
            entity.ContinueDate = 0;
            entity.ConvertDelete = (int)CovertDeleteState.Normal;
            entity.CreateUserName = string.Empty;
            entity.DeliveryDate = timeProvider.Now;
            entity.Description = string.Empty;
            entity.DevTsHours = 0;
            entity.Hours = timeProvider.Now.Hour;
            entity.ID = 0;
            entity.IsEstimates = false;
            entity.IsInternal = false;
            entity.Priority = (int)PriorityState.Normal;
            entity.ProjectID = 0;
            entity.PublishDate = timeProvider.Now;
            entity.QaTsHours = 0;
            entity.SourceTicketID = 0;
            entity.StartDate = timeProvider.Now;
            entity.Status = 0;
            entity.TicketID = 0;
            entity.TicketType = Enum.GetName(typeof(TicketsType), TicketsType.Bug);

            entity.CreatedBy = PostByUserID;
            entity.ModifiedBy = PostByUserID;
            entity.CreatedOn = timeProvider.Now;
            entity.ModifiedOn = timeProvider.Now;
            return entity;
        }

    }
}
