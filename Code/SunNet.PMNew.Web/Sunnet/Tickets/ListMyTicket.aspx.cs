using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class ListMyTicket : BaseWebsitePage
    {
        #region declare

        string pid = "";
        int page = 1;
        int recordCount;
        TicketsRelationApplication trApp = new TicketsRelationApplication();
        TicketsApplication ticketAPP = new TicketsApplication();
        ProjectApplication proApp = new ProjectApplication();
        UserApplication userApp = new UserApplication();
        FeedBackMessagesApplication fbmApp = new FeedBackMessagesApplication();
        List<ProjectDetailDTO> listPorject = new List<ProjectDetailDTO>();
        #endregion

        #region FeedbackMessage
        FeedBackMessageHandler fbmHandler;
        protected string FeedBackMessage(object ticketId)
        {
            return fbmHandler.FeedBackMessage(ticketId, UserInfo.Role);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            fbmHandler = new FeedBackMessageHandler(UserInfo);
            listPorject = proApp.GetUserProjects(UserInfo);
            if (!IsPostBack)
            {
                pid = QS("pid");
                InitTicketSatusBind();
                InitProjectTitleBind();
                InitCompanyBind();
                InitClientPriorityBind();

                // Buring 2013_10_14 contactordev select
                if (this.UserInfo.Role == RolesEnum.Contactor)
                {
                    dvCompany.Visible = false;
                }
                TicketsDataBind();

                if (!string.IsNullOrEmpty(pid))
                {
                    if (!CheckSecurity(Convert.ToInt32(pid)))
                    {
                        Response.Redirect("~/Sunnet/Tickets/dashboard.aspx");
                        return;
                    }
                }
                ShowAddTicketBuutton();
            }
        }

        #region show add ticket button

        private void ShowAddTicketBuutton()
        {
            if (UserInfo.Role == RolesEnum.PM ||
                UserInfo.Role == RolesEnum.ADMIN ||
                UserInfo.Role == RolesEnum.Leader ||
                UserInfo.Role == RolesEnum.QA ||
                UserInfo.Role == RolesEnum.Sales ||
                UserInfo.Role == RolesEnum.Supervisor)
            {
                this.AddNewTicket.Visible = true;
            }
        }

        #endregion

        #region initial data bind

        #region  all show status
        int[] allowShowStatus = { (int)TicketsState.Submitted, (int)TicketsState.Waiting_For_Estimation, (int)TicketsState.Developing, 
                                  (int)TicketsState.PM_Reviewed, (int)TicketsState.PM_Verify_Estimation, (int)TicketsState.Estimation_Fail, 
                                  (int)TicketsState.Testing_On_Local, (int)TicketsState.Testing_On_Client, (int)TicketsState.Ready_For_Review, 
                                  (int)TicketsState.Completed, (int)TicketsState.Waiting_Sales_Confirm ,
                                  (int)TicketsState.Tested_Success_On_Local, (int)TicketsState.Tested_Success_On_Client, 
                                  (int)TicketsState.Tested_Fail_On_Local, (int)TicketsState.Tested_Fail_On_Client, (int)TicketsState.Estimation_Approved, 
                                  (int)TicketsState.Not_Approved, (int)TicketsState.PM_Deny,(int)TicketsState.Cancelled,(int)TicketsState.Internal_Cancel};

        string allowStatus = "";

        private string AllowStatus()
        {
            foreach (int item in allowShowStatus)
            {
                allowStatus += item + ",";
            }
            return allowStatus.TrimEnd(',');
        }

        #endregion

        private void InitTicketSatusBind()
        {
            var dictionary = new Dictionary<int, string>();

            foreach (int value in Enum.GetValues(typeof(TicketsState)))
            {
                if (allowShowStatus.Contains(value))
                {
                    dictionary.Add(value, Enum.GetName(typeof(TicketsState), value).Replace('_', ' '));
                }
            }
            if (UserInfo.Role == RolesEnum.PM)
                dictionary.Add((int)TicketsState.Wait_PM_Feedback, TicketsState.Wait_PM_Feedback.ToString().Replace("_", " "));

            ddlStatus.DataSource = dictionary;
            ddlStatus.DataTextField = "Value";
            ddlStatus.DataValueField = "Key";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("Please select...", ""));
        }

        private void InitProjectTitleBind()
        {
            this.ddlProject.DataTextField = "Title";
            this.ddlProject.DataValueField = "ProjectID";

            this.ddlProject.DataSource = listPorject;
            this.ddlProject.DataBind();
            this.ddlProject.Items.Insert(0, new ListItem("Please select...", ""));
        }

        private void TicketsDataBind()
        {
            List<TicketsEntity> list = null;

            GetTicketsListByConditionRequest request = new GetTicketsListByConditionRequest();

            #region set search condition value

            TicketsSearchConditionDTO DTO = new TicketsSearchConditionDTO();
            string keyWord = this.txtKeyWord.Text.Trim();
            string state = string.IsNullOrEmpty(ValidateDDLIsFirstIndexReturnEmpty(ddlStatus, true))
                            ? AllowStatus() : ValidateDDLIsFirstIndexReturnEmpty(ddlStatus, true);
            DTO.KeyWord = ReturnTicketId(keyWord).Trim().NoHTML();
            DTO.Status = state;
            DTO.IsFeedBack = false;
            DTO.FeedBackTicketsList = "";
            DTO.TicketType = ddlTicketType.SelectedValue;
            DTO.AssignedUser = UserInfo.ID.ToString();
            //ValidateDDLIsFirstIndexReturnEmpty(ddlAssUser, true);
            DTO.Client = "";
            DTO.ClientPriority = ValidateDDLIsFirstIndexReturnEmpty(ddlClientPriority, true);
            DTO.Company = ValidateDDLIsFirstIndexReturnEmpty(ddlCompany, true);
            DTO.OrderExpression = hidOrderBy.Value;
            DTO.OrderDirection = hidOrderDirection.Value;
            DTO.IsInternal = true;//true here, for unlimited
            if (pid.Length > 0)
            {
                DTO.Project = pid;
                ListItem li = ddlProject.Items.FindByValue(pid);
                if (li != null)
                    li.Selected = true;
            }
            else if (this.ddlProject.SelectedIndex <= 0)
            {
                if (GetAllowPid().Length > 0)
                {
                    DTO.Project = GetAllowPid();
                }
                else
                {
                    return;
                }
            }
            else
            {
                DTO.Project = ValidateDDLIsFirstIndexReturnEmpty(ddlProject, true);
            }

            request.TicketSc = DTO;

            #endregion

            int pm = 0;
            if (ddlStatus.SelectedValue != "")
                if (int.Parse(ddlStatus.SelectedValue) == (int)TicketsState.Wait_PM_Feedback)
                    pm = UserInfo.UserID;

            list = ticketAPP.GetTicketListBySearchCondition(request, pm, out  recordCount, page, anpUsers.PageSize);

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
            anpUsers.RecordCount = recordCount;
        }

        private bool CheckSecurity(int pid)
        {
            List<ProjectDetailDTO> list = new List<ProjectDetailDTO>();
            list = proApp.GetUserProjects(UserInfo);
            list = list.FindAll(x => x.ProjectID == pid);
            if (null == list || list.Count <= 0)
            {
                return false;
            }
            return true;
        }

        private string ValidateDDLIsFirstIndexReturnEmpty(DropDownList ddl, bool IsValue)
        {
            if (IsValue)
            {
                return ddl.SelectedIndex <= 0 ? "" : ddl.SelectedValue;
            }
            return ddl.SelectedIndex <= 0 ? "" : ddl.SelectedItem.Text;
        }

        private void InitCompanyBind()
        {
            CompanyApplication comApp = new CompanyApplication();
            List<CompanysEntity> list = comApp.GetAllCompanies();

            this.ddlCompany.DataTextField = "CompanyName";
            this.ddlCompany.DataValueField = "ComID";

            this.ddlCompany.DataSource = list;
            this.ddlCompany.DataBind();

            this.ddlCompany.Items.Insert(0, new ListItem("Please select...", ""));
        }

        private void InitClientPriorityBind()
        {
            var dictionary = new Dictionary<int, string>();
            foreach (int value in Enum.GetValues(typeof(PriorityState)))
            {
                dictionary.Add(value, Enum.GetName(typeof(PriorityState), value));
            }

            ddlClientPriority.DataSource = dictionary;

            ddlClientPriority.DataTextField = "Value";
            ddlClientPriority.DataValueField = "Key";
            ddlClientPriority.DataBind();
            ddlClientPriority.Items.Insert(0, new ListItem("Please select...", ""));
        }

        #endregion

        protected void SearchImgBtn_Click(object sender, ImageClickEventArgs e)
        {
            TicketsDataBind();
        }

        protected void anpUsers_PageChanged(object sender, EventArgs e)
        {
            page = anpUsers.CurrentPageIndex;
            TicketsDataBind();
        }

        #region common show

        protected string ShowRelatedByTid(string tid)
        {
            return trApp.GetAllRelationStringById(Convert.ToInt32(tid), false).TrimEnd(',');
        }
        protected string GetAllowPid()
        {
            string pidList = "";

            foreach (ProjectsEntity item in listPorject)
            {
                pidList += item.ProjectID + ",";
            }

            return pidList.TrimEnd(',');
        }
        protected string ChangeStatus(object status, int ticketId)
        {
            return fbmHandler.GetSunnetStatusNameByStatus(status, ticketId);
        }
        protected string GetUserNameByCreateID(string cid)
        {
            string userName = userApp.GetLastNameFirstName(Convert.ToInt32(cid));
            return userName.Length == 0 ? "" : userName;
        }

        #endregion

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            pid = "";
        }
    }
}
