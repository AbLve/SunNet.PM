using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Core.UI;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.EventModel;

namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class Dashboard : BaseWebsitePage
    {
        List<ProjectDetailDTO> listPorject = new List<ProjectDetailDTO>();
        ProjectApplication proApp = new ProjectApplication();
        TicketsApplication ticketAPP = new TicketsApplication();
        protected List<UpcomingEvent> upcomingEvents = new List<UpcomingEvent>();
        protected FeedBackMessageHandler fbmHandler;
        string allowStatus = string.Empty;
        int recordCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            allowStatus = ReturnSatisfyConditionStatus(ClientAllowShowStatus());
            fbmHandler = new FeedBackMessageHandler(UserInfo);
            listPorject = proApp.GetUserProjects(UserInfo);
            if (!IsPostBack)
            {
                upcomingEvents = new EventsApplication().GetEventList(DateTime.Now, UserInfo.UserID, 0, 10);
            }
            InitialControl();
        }

        public void InitialControl()
        {
            //ltrlWaitforYou.Text = GetWaitingforYouCount(ReturnSatisfyConditionStatus(ClientAllowShowWaitProcessStatus())).ToString();

            if (UserInfo.Role == RolesEnum.ADMIN || UserInfo.Role == RolesEnum.PM)
            {
                ltrlCancelledTickets.Text = ticketAPP.GetCancelCount(UserInfo.UserID, 0, 0, 0, TicketsType.None, string.Empty).ToString();
                ltrlViewTicketsProgress.Text = ticketAPP.GetOngoingTicketsCount(UserInfo.UserID, 0, 0, ClientTicketState.None, TicketsType.None, string.Empty).ToString(); //Ongoing Tickets
                ltrlDraftedTickets.Text = ticketAPP.GetDraftedTicketsCount(UserInfo.UserID, 0, 0, TicketsType.None, string.Empty).ToString();
                ltrlCompletedTickets.Text = ticketAPP.GetCompletedTicketsCount(UserInfo.UserID, 0, 0, TicketsType.None, string.Empty).ToString();
            }
            else
            {
                ltrlWaitforYou.Text = ticketAPP.GetWaitingforResponseCount(UserInfo.UserID, 0, UserInfo.CompanyID, ClientTicketState.None, TicketsType.None, string.Empty).ToString();
                ltrlCancelledTickets.Text = ticketAPP.GetCancelCount(UserInfo.UserID, 0, UserInfo.CompanyID, 0, TicketsType.None, string.Empty).ToString();
                ltrlViewTicketsProgress.Text = ticketAPP.GetOngoingTicketsCount(UserInfo.UserID, 0, UserInfo.CompanyID, ClientTicketState.None, TicketsType.None, string.Empty).ToString(); //Ongoing Tickets
                ltrlDraftedTickets.Text = ticketAPP.GetDraftedTicketsCount(UserInfo.UserID, 0, UserInfo.CompanyID, TicketsType.None, string.Empty).ToString();
                ltrlCompletedTickets.Text = ticketAPP.GetCompletedTicketsCount(UserInfo.UserID, 0, UserInfo.CompanyID, TicketsType.None, string.Empty).ToString();
            }

            ltrlTicketReport.Text = GetTicketReportCount().ToString();
        }

        public int GetWaitingforYouCount(string status)
        {
            List<TicketsEntity> list = null;
            GetTicketsListByConditionRequest request = new GetTicketsListByConditionRequest();
            string keyWord = string.Empty;
            TicketsSearchConditionDTO dto = new TicketsSearchConditionDTO();
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                dto.Company = UserInfo.CompanyID.ToString();
                dto.Client = UserInfo.UserID.ToString();
            }
            else if (UserInfo.Role == RolesEnum.ADMIN ||
                     UserInfo.Role == RolesEnum.PM)
            {
                dto.Company = "";
            }
            dto.Project = GetOwnProjectID();
            dto.OrderExpression = "ModifiedOn";
            dto.OrderDirection = "desc";
            dto.Client = UserInfo.UserID.ToString();
            dto.IsInternal = false;
            dto.Status = status;
            dto.TicketType = string.Empty;
            request.TicketSc = dto;
            list = ticketAPP.GetTicketListBySearchConditionWithStatusWaitingProcess(request, out  recordCount, 1, 0);
            return recordCount;
        }

        private string GetOwnProjectID()
        {
            listPorject = proApp.GetUserProjects(UserInfo);
            string pidList = "";
            foreach (ProjectsEntity item in listPorject)
            {
                pidList += item.ProjectID + ",";
            }
            return pidList.TrimEnd(',');
        }

        private string ReturnSatisfyConditionStatus(int[] array)
        {
            string tempStatus = "";
            foreach (int item in array)
            {
                tempStatus += item + ",";
            }
            return tempStatus.TrimEnd(',');
        }

        private int GetTicketReportCount()
        {
            SearchTicketsRequest request = new SearchTicketsRequest(
                   SearchTicketsType.TicketsForReport,
                   string.Format(" {0} {1} ", "ModifiedOn", "desc"),
                   true);
            request.CurrentPage = 1;
            request.PageCount = 0;
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                request.CompanyID = UserInfo.CompanyID;
                request.UserID = UserInfo.UserID;
            }
            else
            {
                request.CompanyID = 0;
            }
            request.TicketType = "ALL";
            request.Status = fbmHandler.GetSearchTicketStatuses(-1);
            request.Keyword = string.Empty;
            SearchTicketsResponse response = ticketAPP.SearchTickets(request);
            return response.ResultCount;
        }

        public int GetCount(string status)
        {
            TicketsSearchCondition ticketsSearchCondition = new TicketsSearchCondition();
            ticketsSearchCondition.IsInternal = false;
            ticketsSearchCondition.KeyWord = "";

            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                ticketsSearchCondition.Client = UserInfo.UserID.ToString();
                ticketsSearchCondition.Company = UserInfo.CompanyID.ToString();
            }
            ticketsSearchCondition.Project = GetOwnProjectID();
            ticketsSearchCondition.IsFeedBack = false;
            ticketsSearchCondition.Status = status;
            return ticketAPP.GetTicketListBySearchConditionCount(ticketsSearchCondition);
        }

        public string GetAllowPid(List<ProjectDetailDTO> listPorject)
        {
            string pidList = "";

            foreach (ProjectsEntity item in listPorject)
            {
                pidList += item.ProjectID + ",";
            }

            return pidList.TrimEnd(',');
        }
    }
}
