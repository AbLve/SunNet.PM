using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.UserModel;
using System.Text.RegularExpressions;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;


namespace SunNet.PMNew.PM2014.Ticket.Sunnet
{
    public partial class Index : TicketPageHelper
    {
        protected override string DefaultOrderBy
        {
            get
            {
                return "ShowNotification";
            }
        }

        protected override string DefaultDirection
        {
            get
            {
                return "Desc";
            }
        }

        #region declare

        string pid = "";
        int page = 1;
        int recordCount;
        TicketsApplication ticketAPP = new TicketsApplication();
        ProjectApplication proApp = new ProjectApplication();
        UserApplication userApp = new UserApplication();

        List<ProjectDetailDTO> listPorject = new List<ProjectDetailDTO>();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            listPorject = proApp.GetUserProjects(UserInfo);
            if (!IsPostBack)
            {
                pid = QS("pid");
                FillSearchDto();
                // Buring 2013_10_14 contactordev select
                //if (this.UserInfo.Role == RolesEnum.Contactor)
                //{
                //    ddlAssignUser.SelectedValue = this.UserInfo.UserID.ToString();
                //    ddlAssignUser.Enabled = false;
                //}
                TicketsDataBind();

                if (!string.IsNullOrEmpty(pid))
                {
                    if (!CheckSecurity(Convert.ToInt32(pid)))
                    {
                        Response.Redirect("~/SunnetTicket/dashboard.aspx");
                        return;
                    }
                }
            }
        }



        #region initial data bind

        private List<ListItem> GetTicketStatus()
        {
            List<TicketsState> ticketStatus = (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.Sales) ? TicketsStateHelper.SunnetUSAllowShowStatus : TicketsStateHelper.SunnetSHAllowShowStatus;
            List<ListItem> status = ticketStatus.Select(x => new ListItem() { Text = x.ToText(), Value = ((int)x).ToString() }).ToList();
            return status ?? new List<ListItem>();
        }

        private string GetSelectedTicketStatus()
        {
            List<ListItem> status = this.GetTicketStatus();
            var cache = new CookieHelper().Get($"TicketStatus_{UserInfo.Role}_{DateTime.Now.ToString("yyyyMMdd")}");
            if (!string.IsNullOrEmpty(cache) && !cache.Equals("[]"))
            {
                var cacheStatus = new JSONHelper().GetEntity<List<string>>(cache);
                status = status.FindAll(it => cacheStatus.Contains(it.Value));
            }
            string statusJson = string.Join(",", status.Select(it => it.Value).ToList());
            return statusJson;
        }

        private void InitProjectTitleBind(string selected)
        {
            this.ddlProject.DataSource = listPorject;
            this.ddlProject.DataBind((ProjectDetailDTO project, string status) => project.Status.ToString() == status);
            this.ddlProject.SelectItem(selected);
        }

        private void FillSearchDto()
        {
            txtKeyWord.Text = QS("keyword");
            txtCreated.Text = QS("create").Trim();

            //selectValue.Value = QS("status");
            selectValue.Value = GetSelectedTicketStatus();
            GetTicketStatus().BindDropdown(ddlStatus, "Text", "Value");

            ddlTicketType.SelectItem(QS("tickettype"));
            InitInternalUserBind();
            ddlAssignUser.SelectItem(QS("ticketAssignedUser"));
            InitClientPriorityBind();
            ddlPriority.SelectItem(QS("ticketPriority"));

            InitProjectTitleBind(QS("project"));
        }

        private void TicketsDataBind()
        {
            var project = 0;
            int.TryParse(ddlProject.SelectedValue, out project);
            var statusStr = QS("status");
            if (statusStr == "null")
            {
                this.trNoTickets.Visible = true;
                this.rptTicketsList.DataSource = new List<TicketsEntity>();
                this.rptTicketsList.DataBind();
                anpOngoing.RecordCount = 0;
                return;
            }
            var typeValue = -1;
            var type = TicketsType.None;
            if (int.TryParse(ddlTicketType.SelectedValue, out typeValue) && typeValue >= 0)
                type = (TicketsType)typeValue;

            var priorith = 0;
            int.TryParse(ddlPriority.SelectedValue, out priorith);
            var assignuser = 0;
            int.TryParse(ddlAssignUser.SelectedValue, out assignuser);

            List<TicketsEntity> list = ticketAPP.SearchTicketsForAllTickets(UserInfo, txtKeyWord.Text, project, statusStr, QS("create"), type, priorith, assignuser,
                 CurrentPageIndex, anpOngoing.PageSize, OrderBy, OrderDirection, false, out recordCount);

            if (null == list || list.Count <= 0)
            {
                this.trNoTickets.Visible = true;
            }
            else
            {
                this.trNoTickets.Visible = false;
            }
            this.rptTicketsList.DataSource = list;
            this.rptTicketsList.DataBind();
            anpOngoing.RecordCount = recordCount;
        }

        private bool CheckSecurity(int pid)
        {
            List<ProjectDetailDTO> list = new List<ProjectDetailDTO>();
            list = proApp.GetUserProjects(UserInfo);
            list = list.FindAll(x => x.ProjectID == pid);
            return null != list && list.Count > 0;
        }

        private string ValidateDDLIsFirstIndexReturnEmpty(DropDownList ddl, bool IsValue)
        {
            if (IsValue)
            {
                return ddl.SelectedIndex <= 0 ? "" : ddl.SelectedValue;
            }
            return ddl.SelectedIndex <= 0 ? "" : ddl.SelectedItem.Text;
        }

        private void InitClientPriorityBind()
        {
            var dictionary = new Dictionary<int, string>();
            foreach (int value in Enum.GetValues(typeof(PriorityState)))
            {
                dictionary.Add(value, Enum.GetName(typeof(PriorityState), value));
            }

            ddlPriority.DataSource = dictionary;

            ddlPriority.DataTextField = "Value";
            ddlPriority.DataValueField = "Key";
            ddlPriority.DataBind();
            ddlPriority.Items.Insert(0, new ListItem("ALL", ""));
        }

        private void InitInternalUserBind()
        {
            SearchUsersRequest request = new SearchUsersRequest(
                SearchUsersType.All, false, " FirstName ", " ASC ");
            request.IsSunnet = true;

            SearchUserResponse response = userApp.SearchUsers(request);

            ddlAssignUser.DataSource = response.ResultList.FindAll(x => x.Role != RolesEnum.CLIENT);
            ddlAssignUser.DataBind(delegate (UsersEntity user, string status)
            {
                return user.Status == status;
            });
        }

        #endregion

        #region common show

        protected string GetAllowPid()
        {
            string pidList = "";

            foreach (ProjectsEntity item in listPorject)
            {
                pidList += item.ProjectID + ",";
            }

            return pidList.TrimEnd(',');
        }

        #endregion


        protected string TicketCreateUser(string firstName, string lastName, bool isEstimates)
        {
            if (isEstimates)
            {
                if (UserInfo.Office == "US" && (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.Sales || UserInfo.Role == RolesEnum.ADMIN))
                    return string.Format("{0} {1}", firstName, lastName);
                else
                    return "Client";
            }
            return string.Format("{0} {1}", firstName, lastName);
        }
    }
}