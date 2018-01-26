using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.TicketModel
{
    public class TicketsSearchCondition
    {

        public string KeyWord { get; set; }

        public string Status { get; set; }

        /// <summary>
        /// Type = "Bug" or "Request"
        /// </summary>
        public string Type { get; set; }

        public string Project { get; set; }
        public int ProjectID { get; set; }

        public string AssignedUser { get; set; }

        public string Company { get; set; }

        public string Client { get; set; }

        public string ClientPriority { get; set; }

        public bool IsInternal { get; set; }

        public string OrderExpression { get; set; }

        public string OrderDirection { get; set; }

        public string FeedBackTicketsList { get; set; }

        public bool IsFeedBack { get; set; }
        private DateTime _sheetDate;
        public DateTime SheetDate
        {
            get
            {
                if (_sheetDate == null || _sheetDate == DateTime.MinValue)
                {
                    return new DateTime(1753, 1, 1);
                }
                return _sheetDate;
            }
            set { _sheetDate = value; }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get
            {
                if (_startDate == null || _startDate == DateTime.MinValue)
                {
                    return new DateTime(1753, 1, 1);
                }
                return _startDate;
            }
            set
            {
                _startDate = value;
            }
        }
        private DateTime _endDate;
        public DateTime EndDate
        {
            get
            {
                if (_endDate == null || _endDate == DateTime.MinValue)
                {
                    return new DateTime(1753, 1, 1);
                }
                return _endDate;
            }
            set { _endDate = value; }
        }
        /// <summary>
        /// 是否分页
        /// </summary>
        public bool IsPageModel { get; set; }

        public TicketsSearchCondition()
        {

        }

        public TicketsSearchCondition(string KeyWord, string Status, string Type,
                                      string Project,int ProjectID, string AssignedUser,
                                      string Company, string Client,
                                      string ClientPriority, bool IsInternal,
                                      string OrderExpression, string OrderDirection,
                                      string FeedBackTicketsList, bool IsFeedBack
                                      )
        {
            this.KeyWord = KeyWord;
            this.Status = Status;
            this.Type = Type;
            this.Project = Project;
            this.ProjectID = ProjectID;
            this.AssignedUser = AssignedUser;
            this.Company = Company;
            this.Client = Client;
            this.ClientPriority = ClientPriority;
            this.IsInternal = IsInternal;
            this.OrderExpression = OrderExpression;
            this.OrderDirection = OrderDirection;
            this.FeedBackTicketsList = FeedBackTicketsList;
            this.IsFeedBack = IsFeedBack;
        }

    }
}
