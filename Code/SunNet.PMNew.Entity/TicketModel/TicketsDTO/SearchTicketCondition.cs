using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**************************************************************************
 * Developer: 		JackZhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		2014/7/24 4:17:34
 * Description:		Please input class summary
 * Version History:	Created,2014/7/24 4:17:34
 * 
 * 
 **************************************************************************/
namespace SunNet.PMNew.Entity.TicketModel.TicketsDTO
{
    public class SearchTicketCondition
    {
        public bool SearchKeyword { get; private set; }
        private string _keyword;
        public string Keyword
        {
            get { return _keyword; }
            set
            {
                SearchKeyword = true;
                _keyword = value;
            }
        }

        public bool SearchCompany { get; private set; }
        private int _companyId;

        public int CompanyId
        {
            get
            {
                SearchCompany = true;
                return _companyId;
            }
            set { _companyId = value; }
        }


        public bool SearchProject { get; private set; }
        private int _project;
        public int ProjectId
        {
            get { return _project; }
            set
            {
                SearchProject = true;
                _project = value;
            }
        }

        public bool SearchStatus { get; private set; }
        private TicketsState _status;
        public TicketsState Status
        {
            get { return _status; }
            set
            {
                SearchStatus = true;
                _status = value;
            }
        }

        public bool SearchStatusRange { get; private set; }
        private List<TicketsState> _statusList;
        public List<TicketsState> Statuses
        {
            get
            {
                SearchStatusRange = true;
                return _statusList ?? (_statusList = new List<TicketsState>());
            }
        }

        public bool SearchType { get; private set; }
        private TicketsType _type;
        public TicketsType Type
        {
            get { return _type; }
            set
            {
                SearchType = true;
                _type = value;
            }
        }

        public bool SearchPriority { get; private set; }
        private PriorityState _priority;
        public PriorityState Priority
        {
            get { return _priority; }
            set
            {
                SearchPriority = true;
                _priority = value;
            }
        }

        public bool SearchAssignUser { get; private set; }
        private int _user;
        public int AssignUserId
        {
            get { return _user; }
            set
            {
                SearchAssignUser = true;
                _user = value;
            }
        }

        public bool SearchResponsibleUser { get; private set; }
        public int RespUser;
        public int ResponsibleUserId
        {
            get { return RespUser; }
            set
            {
                SearchResponsibleUser = true;
                RespUser = value;
            }
        }

        public bool SearchMyTicket { get; set; }

        /// <summary>
        /// 是否 搜索 ticket 创建者
        /// </summary>
        public bool SearchCreate { get; private set; }
        private string _create;
        /// <summary>
        /// 搜索 ticket 创建者(模糊)
        /// </summary>
        public string CreateUser
        {
            get { return _create; }
            set
            {
                SearchCreate = true;
                _create = value;
            }
        }

        public bool SearchCurrentUser { get; set; }

        /// <summary>
        /// 是否只查询数量,不返回具体数据
        /// </summary>
        /// <value><c>true</c> if [only count]; otherwise, <c>false</c>.</value>
        public bool OnlyCount { get; set; }

        public bool SearchIsInternal { get; set; }
        private bool _isInternal;

        public bool IsInternal
        {
            get
            {
                return _isInternal;
            }
            set
            {
                _isInternal = value;
            }
        }


        /// <summary>
        /// Current Login userId
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int TotalRecords { get; set; }


        public bool SearchCreatedStart { get; private set; }
        private DateTime? _createStart;
        public DateTime? CreateStartTime
        {
            get
            {
                return _createStart;
            }
            set
            {
                SearchCreatedStart = true;
                _createStart = value;
            }
        }

        public bool SearchCreatedEnd { get; private set; }
        private DateTime? _createEnd;
        public DateTime? CreateEndTime
        {
            get
            {
                return _createEnd;
            }
            set
            {
                SearchCreatedEnd = true;
                _createEnd = value;
            }
        }


        public bool SearchMultiStatus { get; private set; }
        private List<int> _multiStaus;

        public List<int> MultiStaus
        {
            get { return _multiStaus; }
            set
            {
                SearchMultiStatus = true;
                _multiStaus = value;
            }
        }
    }
}
